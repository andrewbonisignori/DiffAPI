using AutoMapper;
using Diff.Controllers;
using Diff.Domain;
using Diff.Domain.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Diff.Tests.Controllers
{
    [TestClass]
    public class DiffControllerTest
    {
        // Default objects shared among tests when no more specific setup is required.
        private readonly Mock<IDiffAnalyser> _defaultDiffAnalyserMock = new Mock<IDiffAnalyser>();
        private readonly Mock<IMapper> _defaultMapperMock = new Mock<IMapper>();
        private readonly Mock<IDiffRepositoryManager> _defaultRepositoryMock = new Mock<IDiffRepositoryManager>();
        private readonly string _validBase64string = Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5 });

        #region PostDiffData

        [TestMethod]
        public async Task PostLeftDiffDataShouldReturnStatusCodeOkWhenNoErrorsOccurs()
        {
            // Arrange
            var someValidId = int.MaxValue;
            var diffController = new DiffController(_defaultRepositoryMock.Object, _defaultDiffAnalyserMock.Object, _defaultMapperMock.Object);

            // Act
            IHttpActionResult result = await diffController.PostLeftDiffData(someValidId, _validBase64string);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task PostLeftDiffDataShouldReturnInternalServerErrorWhenSomeUnexpectedErrorsOccurs()
        {
            // Arrange
            var someValidId = int.MaxValue;
            var repositoryMock = new Mock<IDiffRepositoryManager>();
            repositoryMock
                .Setup(r => r.Save(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DiffItemType>()))
                .ThrowsAsync(new Exception());

            var diffController = new DiffController(repositoryMock.Object, _defaultDiffAnalyserMock.Object, _defaultMapperMock.Object);

            // Act
            IHttpActionResult result = await diffController.PostLeftDiffData(someValidId, _validBase64string);

            // Assert
            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
        }

        [TestMethod]
        public async Task PostRightDiffDataShouldReturnStatusCodeOkWhenNoErrorsOccurs()
        {
            // Arrange
            var someValidId = int.MaxValue;
            var diffController = new DiffController(_defaultRepositoryMock.Object, _defaultDiffAnalyserMock.Object, _defaultMapperMock.Object);

            // Act
            IHttpActionResult result = await diffController.PostRightDiffData(someValidId, _validBase64string);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task PostRightDiffDataShouldReturnInternalServerErrorWhenSomeUnexpectedErrorsOccurs()
        {
            // Arrange
            var someValidId = int.MaxValue;
            var repositoryMock = new Mock<IDiffRepositoryManager>();
            repositoryMock
                .Setup(r => r.Save(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DiffItemType>()))
                .ThrowsAsync(new Exception());

            var diffController = new DiffController(repositoryMock.Object, _defaultDiffAnalyserMock.Object, _defaultMapperMock.Object);

            // Act
            IHttpActionResult result = await diffController.PostRightDiffData(someValidId, _validBase64string);

            // Assert
            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
        }

        #endregion

        #region GetDiffData

        [TestMethod]
        public async Task GetDiffDataShouldReturnNotFoundWhenIdDoesNotExists()
        {
            // Arrange
            var repositoryMock = new Mock<IDiffRepositoryManager>();
            repositoryMock
                .Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(default(DiffData));

            var someValidId = int.MaxValue;

            var diffController = new DiffController(repositoryMock.Object, _defaultDiffAnalyserMock.Object, _defaultMapperMock.Object);

            // Act
            IHttpActionResult result = await diffController.GetDiffData(someValidId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetDiffDataShouldReturnInternalServerErrorResultWhenExceptionIsThrownInAnalysis()
        {
            // Arrange
            var repositoryMock = new Mock<IDiffRepositoryManager>();
            repositoryMock
                .Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(new DiffData());

            var diffAnalyserMock = new Mock<IDiffAnalyser>();
            diffAnalyserMock
                .Setup(d => d.Analyse(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Throws<Exception>();

            var someValidId = int.MaxValue;

            var diffController = new DiffController(repositoryMock.Object, diffAnalyserMock.Object, _defaultMapperMock.Object);

            // Act
            IHttpActionResult result = await diffController.GetDiffData(someValidId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
        }

        [TestMethod]
        public async Task GetDiffDataShouldReturnDiffResultWhenAnalysisIsSuccessful()
        {
            // Arrange
            var repositoryMock = new Mock<IDiffRepositoryManager>();
            repositoryMock
                .Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(new DiffData());

            var diffAnalyserMock = new Mock<IDiffAnalyser>();
            diffAnalyserMock
                .Setup(d => d.Analyse(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(default(Domain.DiffResult));

            var someValidId = int.MaxValue;

            var diffController = new DiffController(repositoryMock.Object, diffAnalyserMock.Object, _defaultMapperMock.Object);

            // Act
            IHttpActionResult result = await diffController.GetDiffData(someValidId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Models.Diff.DiffResult>));
        }

        #endregion
    }
}