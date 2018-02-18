using AutoMapper;
using Diff.Domain.Repository;
using Diff.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diff.Domain.Tests.Repository
{
    [TestClass]
    public class DiffRepositoryManagerTest
    {
        // Default objects shared among tests when no more specific setup is required.
        private readonly Mock<IMapper> _defaultMapperMock = new Mock<IMapper>();
        private readonly Mock<IRepository> _defaultRepositoryMock = new Mock<IRepository>();
        private readonly string _validBase64string = Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5 });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SaveShouldThrownArgumentNullExceptionWhenDataIsNull()
        {
            // Arrange
            var someValidId = 1;
            var data = default(string);
            var someValidItemType = DiffItemType.Left;
            var repositoryManager = new DiffRepositoryManager(_defaultRepositoryMock.Object, _defaultMapperMock.Object);

            // Act
            var result = repositoryManager.Save(someValidId, data, someValidItemType);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void SaveShouldThrownFormatExceptionWhenDataIsNotValidBase64String()
        {
            // Arrange
            var someValidId = 1;
            var invalidBase64string = "invalidBase64string";
            var someValidItemType = DiffItemType.Left;
            var repositoryManager = new DiffRepositoryManager(_defaultRepositoryMock.Object, _defaultMapperMock.Object);

            // Act
            var result = repositoryManager.Save(someValidId, invalidBase64string, someValidItemType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SaveShouldThrownArgumentOutOfRangeExceptionWhenItemTypeIsInvalid()
        {
            // Arrange
            var someValidId = 1;
            var invalidItemType = (DiffItemType)int.MinValue;
            var repositoryManager = new DiffRepositoryManager(_defaultRepositoryMock.Object, _defaultMapperMock.Object);

            // Act
            var result = repositoryManager.Save(someValidId, _validBase64string, invalidItemType);
        }

        [TestMethod]
        public void SaveShouldPersistTheDataInRepositoryWhenAllParametersAreValid()
        {
            // Arrange
            var someValidId = 1;
            var someValidItemType = DiffItemType.Left;
            var repositoryMock = new Mock<IRepository>();
            var repositoryManager = new DiffRepositoryManager(repositoryMock.Object, _defaultMapperMock.Object);

            // Act
            repositoryManager.Save(someValidId, _validBase64string, someValidItemType);

            // Assert
            repositoryMock.Verify(r => r.Save(It.IsAny<InMemoryDiffData>()));
        }
    }
}
