using Diff.Controllers;
using Diff.Domain;
using Diff.Ioc;
using Diff.Repository;
using Diff.Repository.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.Results;

namespace Diff.Tests.Integration.Controllers
{
    [TestClass]
    public class DiffControllerTest
    {
        // Default objects shared among tests when no more specific setup is required.
        private static readonly byte[] _leftValidData = new byte[] { 1, 2, 3, 4, 5 };
        private static readonly byte[] _rightValidData = new byte[] { 6, 7, 8, 9, 10 };
        private static readonly string _leftBase64StringValidData = Convert.ToBase64String(_leftValidData);
        private static readonly string _rightBase64StringValidData = Convert.ToBase64String(_rightValidData);

        private DiffController _diffController;
        private IRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            IDependencyResolver dependencyResolver = DependencyRegistrations.GetDependencyResolver();
            // While creating the controller for tests we could use a different repository speficic
            // for tests, but here the ideia is use the same for the exercise purposes.
            _diffController = (DiffController)dependencyResolver.GetService(typeof(DiffController));
            _repository = (IRepository)dependencyResolver.GetService(typeof(IRepository));
            _repository.ClearData();
        }

        [TestMethod]
        public async Task PostDiffDataShouldInsertOnlyLeftDataWhenCallToLeftDataIsValid()
        {
            // Arrange
            var someValidId = int.MaxValue;
            var validData = new byte[] { 1, 2, 3, 4, 5 };
            var diffAnaliser = new DiffAnalyser();

            // Act
            IHttpActionResult response = await _diffController.PostLeftDiffData(someValidId, _leftBase64StringValidData);

            // Assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
            // Only the newly created record should be present.
            Assert.AreEqual(1, _repository.DiffDataCount());

            InMemoryDiffData savedData = _repository.GetById(someValidId);
            // The record in db should be retrieved with the provided id via API call.
            Assert.IsNotNull(savedData);

            // The left data stored should be the same as provided via API call.
            Assert.IsTrue(validData.SequenceEqual(savedData.Left));

            // Right should not contains any data, since the record was created
            // and only left data was provided.
            Assert.AreEqual(null, savedData.Right);
        }

        [TestMethod]
        public async Task PostDiffDataShouldUpdateLeftDataWhenLeftDataAlreadyExists()
        {
            // Arrange
            var someValidId = int.MaxValue;
            var dataToBeUpdated = new byte[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            var base64StringToBeUpdated = Convert.ToBase64String(dataToBeUpdated);

            // Act
            IHttpActionResult insertResponse = await _diffController.PostLeftDiffData(someValidId, _leftBase64StringValidData);
            IHttpActionResult updatedResponse = await _diffController.PostLeftDiffData(someValidId, base64StringToBeUpdated);

            // Assert
            Assert.IsInstanceOfType(insertResponse, typeof(OkResult));
            Assert.IsInstanceOfType(updatedResponse, typeof(OkResult));

            // Only the updated record should be present.
            Assert.AreEqual(1, _repository.DiffDataCount());

            InMemoryDiffData savedData = _repository.GetById(someValidId);
            // The record in db should be retrieved with the provided id via API call.
            Assert.IsNotNull(savedData);

            // The Left data stored should be the same as provided in the second API call.
            Assert.IsTrue(dataToBeUpdated.SequenceEqual(savedData.Left));

            // Right should not contains any data, since the record was updated with Left data only.
            Assert.AreEqual(null, savedData.Right);
        }

        [TestMethod]
        public async Task PostDiffDataShouldUpdateRightDataWhenLeftDataAlreadyExists()
        {
            // Arrange
            var someValidId = int.MaxValue;

            // Act
            IHttpActionResult insertLeftResponse = await _diffController.PostLeftDiffData(someValidId, _leftBase64StringValidData);
            IHttpActionResult updatedRightResponse = await _diffController.PostRightDiffData(someValidId, _rightBase64StringValidData);

            // Assert
            Assert.IsInstanceOfType(insertLeftResponse, typeof(OkResult));
            Assert.IsInstanceOfType(updatedRightResponse, typeof(OkResult));

            // Only the updated record should be present.
            Assert.AreEqual(1, _repository.DiffDataCount());

            InMemoryDiffData savedData = _repository.GetById(someValidId);
            // The record in db should be retrieved with the provided id via API call.
            Assert.IsNotNull(savedData);

            // The left and the right data should be populated.
            // The first API call will create the record and the second API call will update the same record.
            Assert.IsTrue(_leftValidData.SequenceEqual(savedData.Left));
            Assert.IsTrue(_rightValidData.SequenceEqual(savedData.Right));
        }

        [TestMethod]
        public async Task PostDiffDataShouldInsertTwoRecordsWhenProvidedIdsAreDifferents()
        {
            // Arrange
            var firstRecordId = 1;
            var secondRecordId = 2;

            // Act
            IHttpActionResult firstRecordResponse = await _diffController.PostLeftDiffData(firstRecordId, _leftBase64StringValidData);
            IHttpActionResult secondRecordResponse = await _diffController.PostLeftDiffData(secondRecordId, _rightBase64StringValidData);

            // Assert
            Assert.IsInstanceOfType(firstRecordResponse, typeof(OkResult));
            Assert.IsInstanceOfType(secondRecordResponse, typeof(OkResult));

            // Database need contains the 2 inserted records.
            Assert.AreEqual(2, _repository.DiffDataCount());

            InMemoryDiffData firstSavedData = _repository.GetById(firstRecordId);
            // The record in db should be retrieved with the provided id(firstRecordId) via API call.
            Assert.IsNotNull(firstSavedData);

            InMemoryDiffData secondSavedData = _repository.GetById(secondRecordId);
            // The record in db should be retrieved with the provided id(secondRecordId) via API call.
            Assert.IsNotNull(secondSavedData);

            // Both records should contains left data.
            Assert.IsTrue(_leftValidData.SequenceEqual(firstSavedData.Left));
            Assert.IsTrue(_rightValidData.SequenceEqual(secondSavedData.Left));
        }

        [TestMethod]
        public async Task PostDiffDataShouldInsertOnlyRightDataWhenCallToRightDataIsValid()
        {
            // Arrange
            var someValidId = int.MaxValue;

            // Act
            IHttpActionResult response = await _diffController.PostRightDiffData(someValidId, _rightBase64StringValidData);

            // Assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
            // Only the newly created record should be present.
            Assert.AreEqual(1, _repository.DiffDataCount());

            InMemoryDiffData savedData = _repository.GetById(someValidId);
            // The record in db should be retrieved with the provided id via API call.
            Assert.IsNotNull(savedData);

            // The Right data stored should be the same as provided via API call.
            Assert.IsTrue(_rightValidData.SequenceEqual(savedData.Right));

            // Left should not contains any data, since the record was created
            // and only right data was provided.
            Assert.AreEqual(null, savedData.Left);
        }

        [TestMethod]
        public async Task PostDiffDataShouldUpdateRightDataWhenRightDataAlreadyExists()
        {
            // Arrange
            var someValidId = int.MaxValue;
            var dataToBeUpdated = new byte[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            var base64StringToBeUpdated = Convert.ToBase64String(dataToBeUpdated);

            // Act
            IHttpActionResult insertResponse = await _diffController.PostRightDiffData(someValidId, _rightBase64StringValidData);
            IHttpActionResult updatedResponse = await _diffController.PostRightDiffData(someValidId, base64StringToBeUpdated);

            // Assert
            Assert.IsInstanceOfType(insertResponse, typeof(OkResult));
            Assert.IsInstanceOfType(updatedResponse, typeof(OkResult));

            // Only the updated record should be present.
            Assert.AreEqual(1, _repository.DiffDataCount());

            InMemoryDiffData savedData = _repository.GetById(someValidId);
            // The record in db should be retrieved with the provided id via API call.
            Assert.IsNotNull(savedData);

            // The Right data stored should be the same as provided in the second API call.
            Assert.IsTrue(dataToBeUpdated.SequenceEqual(savedData.Right));

            // Left should not contains any data, since the record was updated with Left data only.
            Assert.AreEqual(null, savedData.Left);
        }

        [TestMethod]
        public async Task GetDiffDataShouldReturnBlocksAreNotOfSameSizeWhenProvidedBlocksAreDifferents()
        {
            // Arrange
            var someValidId = int.MaxValue;
            var leftBlock = new byte[] { 1, 2, 3, 4, 5 };
            var leftBlockBase64String = Convert.ToBase64String(leftBlock);

            var rightBlock = new byte[] { 1, 2, 3, 4 };
            var rightBlockBase64String = Convert.ToBase64String(rightBlock);

            // Act
            IHttpActionResult insertLeftResponse = await _diffController.PostLeftDiffData(someValidId, leftBlockBase64String);
            IHttpActionResult insertRightResponse = await _diffController.PostRightDiffData(someValidId, rightBlockBase64String);
            IHttpActionResult getDataResponse = await _diffController.GetDiffData(someValidId);

            // Assert
            Assert.IsInstanceOfType(insertLeftResponse, typeof(OkResult));
            Assert.IsInstanceOfType(insertRightResponse, typeof(OkResult));
            Assert.IsInstanceOfType(getDataResponse, typeof(OkNegotiatedContentResult<Models.Diff.DiffResult>));

            var diffResult = ((OkNegotiatedContentResult<Models.Diff.DiffResult>)getDataResponse).Content;
            Assert.AreEqual("BlocksAreNotOfSameSize", diffResult.Status);
        }
    }
}
