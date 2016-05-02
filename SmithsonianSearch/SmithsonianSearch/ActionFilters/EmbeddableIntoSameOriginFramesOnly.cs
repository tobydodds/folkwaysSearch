using System.Web.Mvc;

namespace SmithsonianSearch.ActionFilters
{
    /// <summary>
    /// Adds X-FRAME-OPTIONS HTTP header with value = SAMEORIGIN.
    /// This restricts result from being embedded into iframes not from the page's domain.
    /// </summary>
    public class EmbeddableIntoSameOriginFramesOnly : ActionFilterAttribute
    {
        private readonly string frameHeader = "X-FRAME-OPTIONS";

        private readonly string frameOption = "SAMEORIGIN";

        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.RequestContext.HttpContext.Response.AddHeader(this.frameHeader, this.frameOption);
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}