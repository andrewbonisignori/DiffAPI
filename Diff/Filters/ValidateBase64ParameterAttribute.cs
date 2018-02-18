using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Diff.Filters
{
    /// <summary>
    /// Executes the validation needed in order to check if a <c>string</c> is a valid base 64 string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ValidateBase64ParameterAttribute : ActionFilterAttribute
    {
        private readonly string _parameterName;

        /// <summary>
        /// Initialize a new instance of <see cref="ValidateBase64ParameterAttribute"/>.
        /// </summary>
        /// <param name="parameterName">Parameter name to be checked.</param>
        public ValidateBase64ParameterAttribute(string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentException($"{nameof(parameterName)} must be provided for Base64 parameter validation.");

            _parameterName = parameterName;
        }

        /// <summary>
        /// Validate the string before the execution of the action.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionArguments.TryGetValue(_parameterName, out object value))
            {
                if(value == null)
                {
                    actionContext.Response =  actionContext.Request.CreateErrorResponse(
                        HttpStatusCode.BadRequest, $"The parameter {_parameterName} cannot be null.");
                }
                else
                {
                    try
                    {
                        Convert.FromBase64String(value.ToString());
                    }
                    catch (FormatException)
                    {
                        actionContext.Response = actionContext.Request.CreateErrorResponse(
                            HttpStatusCode.BadRequest, $"The parameter {_parameterName} is not a valid base 64 string.");
                    }
                }
            }
        }
    }
}