namespace SmithsonianSearch.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Xml.Serialization;

    using Elmah;

    using SmithsonianSearch.ActionFilters;
    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Helpers;
    using SmithsonianSearch.Models;
    using SmithsonianSearch.Models.Enums;
    using SmithsonianSearch.Models.SearchResultsModels;

    using StackExchange.Profiling;

    /// <summary>
    ///     The search controller.
    /// </summary>
    [EmbeddableIntoSameOriginFramesOnly]
    public class SearchController : Controller
    {
        // GET: /Search/

        //// [OutputCache(Duration = 120, Location = OutputCacheLocation.ServerAndClient)]
        #region Public Methods and Operators

        /// <summary>
        ///     The index.
        /// </summary>
        /// <param name="jsonSearchModel">
        ///     The JSON search model.
        /// </param>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        //[OutputCache(Duration = 120, Location = OutputCacheLocation.Any, VaryByParam = "*")]
        [ValidateInput(false)]
        public async Task<ActionResult> Index(string jsonSearchModel, string query)
        {
            ISearchResultsModel resultsModel = null;
            SearchModel searchModel = null;

            using (MiniProfiler.Current.Step("Deserializing model from JSON"))
            {
                searchModel = !string.IsNullOrEmpty(jsonSearchModel)
                                  ? SearchModel.FromJson(jsonSearchModel, Config.Instance)
                                  : new SearchModel();
            }

            searchModel.Query = !string.IsNullOrEmpty(query) ? query : searchModel.Query;

            using (MiniProfiler.Current.Step("Perform search"))
            {
                bool searchFirstSpellingSuggestion = !searchModel.SpellingSuggestionSearchRestricted
                                                     && Config.Instance.SearchSpellingSuggestion;
                resultsModel = await this.Search(searchModel, searchFirstSpellingSuggestion);
            }

            return this.View("Index", resultsModel ?? new SearchResultsModelMixed());
        }

        /// <summary>
        ///     Get next page for lazy loading
        /// </summary>
        /// <param name="jsonSearchModel">
        ///     JSON-serialized search model
        /// </param>
        /// <returns>
        ///     HTML of the required page of results
        /// </returns>
        [RequireRequestParameters("jsonSearchModel")]
        //[OutputCache(Duration = 120, Location = OutputCacheLocation.Any, VaryByParam = "*")]
        [ValidateInput(false)]
        public async Task<ActionResult> NextPage(string jsonSearchModel)
        {
            ISearchResultsModel resultsModel = null;
            SearchModel searchModel = null;

            using (MiniProfiler.Current.Step("Deserializing model from JSON"))
            {
                searchModel = !string.IsNullOrEmpty(jsonSearchModel)
                                  ? SearchModel.FromJson(jsonSearchModel, Config.Instance)
                                  : new SearchModel();
            }

            using (MiniProfiler.Current.Step("Perform search"))
            {
                resultsModel = await this.Search(searchModel);
            }

            this.ViewData["printHeader"] = false;

            return searchModel.SelectedView == ViewOptionsEnum.Tiles
                       ? this.PartialView(
                           "~/Views/Search/SearchResultsTilesView.cshtml",
                           resultsModel as SearchResultsModelMixed)
                       : this.PartialView("~/Views/Search/SearchResultsTableView.cshtml", resultsModel);
        }

        /// <summary>
        ///     Perform search according to JSON-serialized search model and optional new query
        /// </summary>
        /// <param name="jsonSearchModel">
        ///     The JSON-serialized search model.
        /// </param>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        [RequireRequestParameters("jsonSearchModel")]
        //[OutputCache(Duration = 120, Location = OutputCacheLocation.Any, VaryByParam = "*")]
        [ValidateInput(false)]
        public async Task<ActionResult> Search(string jsonSearchModel, string query)
        {
            ISearchResultsModel resultsModel = null;
            SearchModel searchModel = null;

            using (MiniProfiler.Current.Step("Deserializing model from JSON"))
            {
                searchModel = !string.IsNullOrEmpty(jsonSearchModel)
                                  ? SearchModel.FromJson(jsonSearchModel, Config.Instance)
                                  : new SearchModel();
            }

            if (!string.IsNullOrEmpty(query))
            {
                searchModel.Query = query;

                // Enforce ALL tab when a new query is provided
                searchModel.FiltersModel.ContentType = ContentTypesEnum.Mixed;
            }

            using (MiniProfiler.Current.Step("Perform search"))
            {
                bool searchFirstSpellingSuggestion = !searchModel.SpellingSuggestionSearchRestricted
                                                     && Config.Instance.SearchSpellingSuggestion;
                resultsModel = await this.Search(searchModel, searchFirstSpellingSuggestion);
            }

            return this.PartialView("~/Views/Search/SearchResultsBase.cshtml", resultsModel);
        }

        //[OutputCache(Duration = 120, Location = OutputCacheLocation.Any, VaryByParam = "*")]
        public ActionResult Help()
        {
            return this.View();
        }

        public ActionResult SearchFormIntegrationExample()
        {
            return View("/Views/SearchFormIntegration/SearchFormIntegrationExample.cshtml");
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Parses search with xml serializer.
        /// </summary>
        /// <param name="stream">
        ///     GSA output XML stream.
        /// </param>
        /// <returns>
        ///     Deserialized GSP object <see cref="GSP" />.
        /// </returns>
        private GSP ParseSearchWithXmlSerializer(Stream stream)
        {
            var ser = new XmlSerializer(typeof(GSP));
            object res = ser.Deserialize(stream);

            return res as GSP;
        }

        /// <summary>
        ///     The search.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        private async Task<ISearchResultsModel> Search(SearchModel model, bool searchFirstSpellingSuggestion = false)
        {
            ISearchResultsModel resultsModel = null;
            bool isTilesView = model.SelectedView == ViewOptionsEnum.Tiles;

            if (!string.IsNullOrEmpty(model.Query))
            {
                model.Query = model.Query.RemoveAlertScript().ReplaceTagBrackets().Trim();

                if (model.PaginationModel.ResultsPerPage <= 0)
                {
                    model.PaginationModel.ResultsPerPage = isTilesView
                                                               ? Config.Instance.PageSizeTilesView
                                                               : Config.Instance.PageSizeTableView;
                }

                //Search removed to support dropdown filters
                //string searchPageUrl = this.Url.Action(
                //    "Index",
                //    "Search",
                //    model.GetRouteValueObject(true),
                //    this.Request.Url.Scheme);

                string searchPageUrl = this.Url.Action(
                    "Index",
                    "Search",
                    model.GetRouteValueObject(true),
                    this.Request.Url.Scheme);

                GSP result = await this.SearchGsa(model) ?? new GSP();

                resultsModel = isTilesView
                                   ? new SearchResultsModelMixed(result, searchPageUrl, model, Config.Instance)
                                   : ResultsModelFactory.BuildResultsModel(
                                       (model.FiltersModel.ContentType == ContentTypesEnum.Mixed)
                                           ? ContentTypesEnum.PurchasableMediaContent
                                           : model.FiltersModel.ContentType,
                                       result,
                                       model,
                                       Config.Instance,
                                       searchPageUrl);

                // Search the first spelling suggestion if required
                if (searchFirstSpellingSuggestion && resultsModel.SpellingSuggestions.SpellingSuggestions.Any())
                {
                    resultsModel = await this.SearchFirstSpellingSuggestion(resultsModel);
                }
            }
            else
            {
                if (isTilesView)
                {
                    resultsModel = new SearchResultsModelMixed { SearchModel = model };
                }
                else
                {
                    resultsModel = new SearchResultsModelPurchasableMedia { SearchModel = model };
                }
            }

            return resultsModel;
        }

        /// <summary>
        /// Search the first spelling suggestion
        /// </summary>
        /// <param name="resultsModel">Results that contain spelling suggestion(s)</param>
        /// <returns></returns>
        private async Task<ISearchResultsModel> SearchFirstSpellingSuggestion(ISearchResultsModel resultsModel)
        {
            ISearchResultsModel spellingSuggestionResultsModel = null;

            string firstSuggestion = resultsModel.SpellingSuggestions.SpellingSuggestions.First().RemoveHtmlTags();
            var newSearchModel = new SearchModel(resultsModel.SearchModel, Config.Instance) { Query = firstSuggestion };

            spellingSuggestionResultsModel = await this.Search(newSearchModel);
            spellingSuggestionResultsModel.OriginallySpelledQuery = resultsModel.SearchModel.Query;
            spellingSuggestionResultsModel.IsSpellingSuggestionSearch = true;

            return spellingSuggestionResultsModel;
        }

        /// <summary>
        ///     Queries GSA to search for the query
        /// </summary>
        /// <param name="searchModel">
        ///     The search Model.
        /// </param>
        /// <returns>
        ///     GSP object <see cref="GSP" />.
        /// </returns>
        private async Task<GSP> SearchGsa(SearchModel searchModel)
        {
            string url = searchModel.ComposeGsaSearchUrl(Config.Instance);

            GSP parsedResult = null;
            Stream stream = null;

            using (var client = new HttpClient())
            {
                try
                {
                    stream = await client.GetStreamAsync(url);
                }
                catch (HttpRequestException e)
                {
                    ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Error(e));
                }

                if (stream != null)
                {
                    using (stream)
                    {
                        try
                        {
                            parsedResult = this.ParseSearchWithXmlSerializer(stream);
                        }
                        catch (InvalidOperationException exception)
                        {
                            ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Error(exception));
                        }
                    }
                }
            }

            return parsedResult;
        }

        #endregion
    }
}