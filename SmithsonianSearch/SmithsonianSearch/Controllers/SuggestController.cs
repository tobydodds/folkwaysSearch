namespace SmithsonianSearch.Controllers
{
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.UI;

    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Helpers;
    using SmithsonianSearch.Models;

    /// <summary>
    /// The suggest controller.
    /// </summary>
    public class SuggestController : Controller
    {
        // GET: /Suggest/

        #region Fields

        private string emptySuggestionsJsonResponse = "[ \"\", [] ]";

        #endregion Fields

        #region Public Methods and Operators

        /// <summary>
        /// The index.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        //[OutputCache(Duration = 120, Location = OutputCacheLocation.Any, VaryByParam = "*")]
        [ValidateInput(false)]
        public async Task<ActionResult> Index(SearchModel model)
        {
            var query = model != null && !string.IsNullOrEmpty(model.Query) ? model.Query.RemoveAlertScript().ReplaceTagBrackets().Trim() : null;

            string json = !string.IsNullOrEmpty(query) ? await this.GetSuggestionsFromGsa(query) : this.emptySuggestionsJsonResponse;

            return this.Content(json, "text/json");
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get suggestions from GSA.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<string> GetSuggestionsFromGsa(string query)
        {
            if (query == null)
            {
                throw new System.ArgumentNullException("query");
            }

            string result = null;

            string gsaUrl = Config.Instance.GsaSuggestBaseUri.AbsoluteUri;
            string url = string.Format("{0}&q={1}", gsaUrl, this.Server.UrlEncode(query));
            WebRequest r = WebRequest.Create(url);

            var client = new HttpClient();
            Stream stream = await client.GetStreamAsync(url);

            using (stream)
            {
                using (var sr = new StreamReader(stream))
                {
                    result = sr.ReadToEnd();
                }
            }

            return result;
        }

        #endregion
    }
}