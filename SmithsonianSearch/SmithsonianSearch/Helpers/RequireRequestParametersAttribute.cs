namespace SmithsonianSearch.Helpers
{
    using System.Reflection;
    using System.Web.Mvc;

    /// <summary>
    ///     Attribute that verifies that required parameter is provided with the request.
    /// </summary>
    public class RequireRequestParametersAttribute : ActionMethodSelectorAttribute
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequireRequestParametersAttribute"/> class.
        /// </summary>
        /// <param name="singleParameterName">
        /// The single parameter name.
        /// </param>
        public RequireRequestParametersAttribute(string singleParameterName)
        {
            this.ParameterNames = new[] { singleParameterName };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequireRequestParametersAttribute"/> class.
        /// </summary>
        /// <param name="parameterNames">
        /// The value name.
        /// </param>
        public RequireRequestParametersAttribute(string[] parameterNames)
        {
            this.ParameterNames = parameterNames;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the value name.
        /// </summary>
        public string[] ParameterNames { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The is valid for request.
        /// </summary>
        /// <param name="controllerContext">
        /// The controller context.
        /// </param>
        /// <param name="methodInfo">
        /// The method info.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            bool result = true;

            foreach (string parameterName in this.ParameterNames)
            {
                result &= controllerContext.HttpContext.Request[parameterName] != null;
            }

            return result;
        }

        #endregion
    }
}