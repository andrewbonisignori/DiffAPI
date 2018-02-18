using Diff.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Diff.Tests.Filters
{
    [TestClass]
    public class ValidateBase64ParameterAttributeTest
    {
        [TestMethod]
        public void ValidateBase64ParameterShouldReturnBadRequestWhenReceivedValueIsNull()
        {
            // Arrange
            var parameterName = "base64Parameter";
            var receivedValue = default(string);

            HttpActionContext context = CreateFakeHttpActionContext();
            context.ActionArguments.Add(parameterName, receivedValue);

            var validateBase64ParameterFilter = new ValidateBase64ParameterAttribute(parameterName);

            // Act
            validateBase64ParameterFilter.OnActionExecutingAsync(context, CancellationToken.None);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, context.Response.StatusCode);
        }

        [TestMethod]
        public void ValidateBase64ParameterShouldReturnBadRequestWhenReceivedValueIsInvalidBase64String()
        {
            // Arrange
            var parameterName = "base64Parameter";
            var invalidBase64String = "invalidBase64String";

            HttpActionContext context = CreateFakeHttpActionContext();
            context.ActionArguments.Add(parameterName, invalidBase64String);

            var validateBase64ParameterFilter = new ValidateBase64ParameterAttribute(parameterName);

            // Act
            validateBase64ParameterFilter.OnActionExecutingAsync(context, CancellationToken.None);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, context.Response.StatusCode);
        }

        [TestMethod]
        public void ValidateBase64ParameterShouldNotPopulateContextResponseWhenReceivedValueIsValidBase64String()
        {
            // Arrange
            var parameterName = "base64Parameter";
            var validBase64String = Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5 });

            HttpActionContext context = CreateFakeHttpActionContext();
            context.ActionArguments.Add(parameterName, validBase64String);

            var validateBase64ParameterFilter = new ValidateBase64ParameterAttribute(parameterName);

            // Act
            validateBase64ParameterFilter.OnActionExecutingAsync(context, CancellationToken.None);

            // Assert
            Assert.IsNull(context.Response);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateBase64ParameterShouldThrownArgumentExceptionWhenDataParameterIsNull()
        {
            // Arrange
            var parameterName = default(string);

            // Act
            new ValidateBase64ParameterAttribute(parameterName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateBase64ParameterShouldThrownArgumentExceptionWhenDataParameterIsEmpty()
        {
            // Arrange
            var parameterName = " ";

            // Act
            new ValidateBase64ParameterAttribute(parameterName);
        }

        private static HttpActionContext CreateFakeHttpActionContext()
        {
            var config = new HttpConfiguration();
            var routeData = new HttpRouteData(new HttpRoute());
            var request = new HttpRequestMessage();
            var controllerContext = new HttpControllerContext(config, routeData, request);
            var descriptorMock = new Mock<HttpActionDescriptor>();
            var context = new HttpActionContext(controllerContext, descriptorMock.Object);
            return context;
        }
    }
}