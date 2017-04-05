namespace SmithsonianSearch.Helpers
{
    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Models;
    using SmithsonianSearch.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.Web.Routing;

    /// <summary>
    ///     The search links helper.
    /// </summary>
    public static class SearchLinksHelper
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The get clear filters URL.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetClearFiltersUrl(this SearchModel model, RequestContext requestContext = null)
        {
            var copyModel = new SearchModel(model, Config.Instance);
            copyModel.FiltersModel.AppliedFilters = new Dictionary<string, HashSet<string>>();
            copyModel.PaginationModel.StartItemIndex = 0;

            RouteValueDictionary routeValueDictionary = copyModel.GetRouteValueDictionary();
            //Search removed to support clearing of dropdown filters
            //routeValueDictionary["action"] = "Search";
            routeValueDictionary["action"] = "";
            routeValueDictionary["controller"] = "Search";

            string url = RouteTable.Routes.GetVirtualPath(requestContext, routeValueDictionary).VirtualPath;

            return url;
        }

        /// <summary>
        ///     The get filters URLs.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>
        /// <param name="filterKey">
        ///     The filter key.
        /// </param>
        /// <param name="filterValue">
        ///     The filter value.
        /// </param>
        /// <param name="isFilterApplied">
        ///     True if URL must include the filter option, false - if skip.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetFilterUrl(
            this SearchModel model,
            string filterKey,
            string filterValue,
            bool isFilterApplied = true,
            RequestContext requestContext = null)
        {
            var tempSearchModel = new SearchModel(model, Config.Instance);
            tempSearchModel.PaginationModel.StartItemIndex = 0;

            if (Config.Instance.FilteringTabsStrategy == Config.FilteringTabsStrategyEnum.RevertToAllTab)
            {
                tempSearchModel.FiltersModel.ContentType = ContentTypesEnum.Mixed;
            }

            bool containsKey = tempSearchModel.FiltersModel.AppliedFilters.ContainsKey(filterKey);

            if (!containsKey && isFilterApplied)
            {
                tempSearchModel.FiltersModel.AppliedFilters.Add(filterKey, new HashSet<string> { filterValue });
            }
            else if (containsKey && isFilterApplied)
            {
                tempSearchModel.FiltersModel.AppliedFilters[filterKey].UnionWith(new List<string> { filterValue });
            }
            else if (containsKey)
            {
                // If need to exclude the filter option from URL
                tempSearchModel.FiltersModel.AppliedFilters[filterKey].Remove(filterValue);
            }

            RouteValueDictionary routeValueDictionary = tempSearchModel.GetRouteValueDictionary();
            routeValueDictionary["action"] = "";
            routeValueDictionary["controller"] = "Search";

            string url = RouteTable.Routes.GetVirtualPath(requestContext, routeValueDictionary).VirtualPath;

            return url;
        }

        /// <summary>
        ///     The get next page url.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetNextPageUrl(this SearchModel model, RequestContext requestContext = null)
        {
            var tempSearchModel = new SearchModel(model, Config.Instance);

            int startItemIndex = model.PaginationModel.CurrentPageIndex * model.PaginationModel.ResultsPerPage;
            tempSearchModel.PaginationModel.StartItemIndex = startItemIndex;

            RouteValueDictionary routeValueDictionary = tempSearchModel.GetRouteValueDictionary();
            routeValueDictionary["action"] = "NextPage";
            routeValueDictionary["controller"] = "Search";

            string url = RouteTable.Routes.GetVirtualPath(requestContext, routeValueDictionary).VirtualPath;

            return url;
        }

        /// <summary>
        /// Gets originally spelled query URL after searching the first spelling suggestion.
        /// </summary>
        /// <param name="model">SearchModel</param>
        /// <param name="originallySpelledQuery">Originally spelled query</param>
        /// <param name="requestContext">Optional request context</param>
        /// <returns>URL</returns>
        public static string GetOriginallySpelledQueryUrl(
            this SearchModel model,
            string originallySpelledQuery,
            RequestContext requestContext = null)
        {
            originallySpelledQuery = System.Web.HttpUtility.HtmlDecode(originallySpelledQuery.RemoveHtmlTags());
            
            var tempSearchModel = new SearchModel(model, Config.Instance);
            tempSearchModel.PaginationModel.StartItemIndex = 0;
            tempSearchModel.Query = originallySpelledQuery;
            tempSearchModel.SpellingSuggestionSearchRestricted = true;

            RouteValueDictionary routeValueDictionary = tempSearchModel.GetRouteValueDictionary();
            routeValueDictionary["action"] = "Search";
            routeValueDictionary["controller"] = "Search";

            string url = RouteTable.Routes.GetVirtualPath(requestContext, routeValueDictionary).VirtualPath;

            return url;
        }

        /// <summary>
        ///     The get related query url.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>
        /// <param name="relatedQuery">
        ///     The related query.
        /// </param>
        /// <returns>
        ///     The <see cref="string" /> URL.
        /// </returns>
        public static string GetRelatedQueryUrl(
            this SearchModel model,
            string relatedQuery,
            RequestContext requestContext = null)
        {
            relatedQuery = System.Web.HttpUtility.HtmlDecode(relatedQuery.RemoveHtmlTags());

            var tempSearchModel = new SearchModel(model, Config.Instance);
            tempSearchModel.PaginationModel.StartItemIndex = 0;
            tempSearchModel.Query = relatedQuery;

            RouteValueDictionary routeValueDictionary = tempSearchModel.GetRouteValueDictionary();
            routeValueDictionary["action"] = "Search";
            routeValueDictionary["controller"] = "Search";

            string url = RouteTable.Routes.GetVirtualPath(requestContext, routeValueDictionary).VirtualPath;

            return url;
        }

        /// <summary>
        ///     The get route value dictionary to be used to form URL parameters
        /// </summary>
        /// <param name="model">
        ///     The search model.
        /// </param>
        /// <returns>
        ///     The <see cref="RouteValueDictionary" /> to be used to form URL parameters.
        /// </returns>
        public static RouteValueDictionary GetRouteValueDictionary(this SearchModel model)
        {
            return new RouteValueDictionary(GetRouteValueObject(model));
        }

        /// <summary>
        ///     The get route value dictionary to be used to form URL parameters
        /// </summary>
        /// <param name="model">
        ///     The search model.
        /// </param>
        /// <param name="resetPaging">
        ///     The reset Paging.
        /// </param>
        /// <returns>
        ///     Object to be used to form URL parameters
        /// </returns>
        public static object GetRouteValueObject(this SearchModel model, bool resetPaging = false)
        {
            if (!resetPaging)
            {
                return new { JsonSearchModel = model.ToJson() };
            }

            var tempModel = new SearchModel(model, Config.Instance);
            tempModel.PaginationModel.StartItemIndex = 0;

            return new { JsonSearchModel = tempModel.ToJson() };
        
        
        }

        /// <summary>
        /// Get new instance of SearchModel prepared for search URL building
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static SearchModel GetSearchModelForNewSearch(this SearchModel model)
        {
            var tempModel = new SearchModel(model, Config.Instance);
            tempModel.FiltersModel = new FiltersModel() { ContentType = model.FiltersModel.ContentType };
            tempModel.PaginationModel.StartItemIndex = 0;
            tempModel.SpellingSuggestionSearchRestricted = false;

            return tempModel;
        }

        /// <summary>
        ///     The get sorting option url.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>
        /// <param name="sortingOption">
        ///     The sorting option.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetSortingOptionUrl(
            this SearchModel model,
            SortOptionsEnum sortingOption,
            RequestContext requestContext = null)
        {
            var tempSearchModel = new SearchModel(model, Config.Instance);
            tempSearchModel.PaginationModel.StartItemIndex = 0;
            tempSearchModel.SortingOption = sortingOption;

            RouteValueDictionary routeValueDictionary = tempSearchModel.GetRouteValueDictionary();
            routeValueDictionary["action"] = "Search";
            routeValueDictionary["controller"] = "Search";

            string url = RouteTable.Routes.GetVirtualPath(requestContext, routeValueDictionary).VirtualPath;

            return url;
        }

        /// <summary>
        /// Get spelling suggestion URL
        /// </summary>
        /// <param name="model">Search model</param>
        /// <param name="spellingSuggestion">Spelling suggestion text</param>
        /// <param name="requestContext">Optional request context</param>
        /// <returns>URL</returns>
        public static string GetSpellingSuggestionUrl(
            this SearchModel model,
            string spellingSuggestion,
            RequestContext requestContext = null)
        {
            return GetRelatedQueryUrl(model, spellingSuggestion, requestContext);
        }

        /// <summary>
        ///     The get tab url.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>
        /// <param name="contentType">
        ///     The content type.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetTabUrl(
            this SearchModel model,
            ContentTypesEnum contentType,
            RequestContext requestContext = null)
        {
            var tempSearchModel = new SearchModel(model, Config.Instance);

            tempSearchModel.FiltersModel.ContentType = contentType;
            tempSearchModel.PaginationModel.StartItemIndex = 0;

            RouteValueDictionary routeValueDictionary = tempSearchModel.GetRouteValueDictionary();
            routeValueDictionary["action"] = "Search";
            routeValueDictionary["controller"] = "Search";

            string url = RouteTable.Routes.GetVirtualPath(requestContext, routeValueDictionary).VirtualPath;

            return url;
        }

        /// <summary>
        ///     The get view options URLs.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>
        /// <returns>
        ///     The <see cref="IDictionary" />.
        /// </returns>
        public static IDictionary<ViewOptionsEnum, string> GetViewOptionsUrls(
            this SearchModel model,
            RequestContext requestContext = null)
        {
            var viewOptionsUrls = new Dictionary<ViewOptionsEnum, string>();

            var tempSearchModel = new SearchModel(model, Config.Instance);

            foreach (object view in Enum.GetValues(typeof(ViewOptionsEnum)))
            {
                tempSearchModel.SelectedView = (ViewOptionsEnum)view;
                if (tempSearchModel.SelectedView == ViewOptionsEnum.Table)
                {
                    tempSearchModel.PaginationModel.ResultsPerPage = Config.Instance.PageSizeTableView;
                }
                else if (tempSearchModel.SelectedView == ViewOptionsEnum.Tiles)
                {
                    tempSearchModel.PaginationModel.ResultsPerPage = Config.Instance.PageSizeTilesView;
                }

                RouteValueDictionary routeValueDictionary = tempSearchModel.GetRouteValueDictionary();
                routeValueDictionary["action"] = "Search";
                routeValueDictionary["controller"] = "Search";

                string url = RouteTable.Routes.GetVirtualPath(requestContext, routeValueDictionary).VirtualPath;

                viewOptionsUrls.Add((ViewOptionsEnum)view, url);
            }

            return viewOptionsUrls;
        }

        #endregion
    }
}