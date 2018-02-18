using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Diff.Domain.Tests
{
    [TestClass]
    public class DiffAnalyserTest
    {
        private readonly DiffAnalyser _diffAnalyser = new DiffAnalyser();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AnalyseMethodShouldThrownArgumentNullExceptionWhenLeftIsNull()
        {
            // Arrange
            byte[] leftBlock = null;
            var rightBlock = new byte[0];

            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AnalyseMethodShouldThrownArgumentNullExceptionWhenRightIsNull()
        {
            // Arrange
            var leftBlock = new byte[0]; ;
            byte[] rightBlock = null;

            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);
        }

        [TestMethod]
        public void AnalyseMethodShouldReturnBlocksAreNotOfSameSizeWhenBlocksAreOfDifferentSize()
        {
            // Arrange
            var leftBlock = new byte[1];
            var rightBlock = new byte[2];

            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);

            // Assert
            Assert.AreEqual(DiffStatus.BlocksAreNotOfSameSize, result.Status);
        }

        [TestMethod]
        public void AnalyseMethodShouldReturnDiffBlocksWithNullWhenBlocksAreOfDifferentSize()
        {
            // Arrange
            var leftBlock = new byte[1];
            var rightBlock = new byte[2];

            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);

            // Assert
            Assert.IsNull(result.DiffBlocks);
        }

        [TestMethod]
        public void AnalyseMethodShouldReturnDifferencesNotFoundWhenBlocksAreOfSameSizeAndSameSequence()
        {
            // Arrange
            var leftBlock = new byte[] { 1, 2, 3, 4, 5 };
            var rightBlock = new byte[] { 1, 2, 3, 4, 5 };

            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);

            // Assert
            Assert.AreEqual(DiffStatus.DifferencesNotFound, result.Status);
        }

        [TestMethod]
        public void AnalyseMethodShouldReturnDiffBlocksWithNullWhenBlocksAreOfSameSizeAndSameSequence()
        {
            // Arrange
            var leftBlock = new byte[] { 1, 2, 3, 4, 5 };
            var rightBlock = new byte[] { 1, 2, 3, 4, 5 };

            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);

            // Assert
            Assert.IsNull(result.DiffBlocks);
        }

        [TestMethod]
        public void AnalyseMethodShouldReturnOneDifferenceFoundWhenDifferencesAreInTheBeginningInLeftBlock()
        {
            // Arrange
            var leftBlock = new byte[] { 0, 0, 0, 4, 5 };
            var rightBlock = new byte[] { 1, 2, 3, 4, 5 };

            var expectedDiffBlock = new DiffBlock(0, 3);

            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);

            // Assert
            AssertDifferenceFound(result, expectedDiffBlock);
        }

        [TestMethod]
        public void AnalyseMethodShouldReturnOneDifferenceFoundWhenDifferencesAreInTheBeginningInRightBlock()
        {
            // Arrange
            var leftBlock = new byte[] { 1, 2, 3, 4, 5 };
            var rightBlock = new byte[] { 0, 0, 0, 4, 5 };

            var expectedDiffBlock = new DiffBlock(0, 3);
            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);

            // Assert
            AssertDifferenceFound(result, expectedDiffBlock);
        }

        [TestMethod]
        public void AnalyseMethodShouldReturnOneDifferenceFoundWhenDifferencesAreInTheEndOfLeftBlock()
        {
            // Arrange
            var leftBlock = new byte[] { 1, 2, 0, 0, 0 };
            var rightBlock = new byte[] { 1, 2, 3, 4, 5 };

            var expectedDiffBlock = new DiffBlock(2, 3);

            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);

            // Assert
            AssertDifferenceFound(result, expectedDiffBlock);
        }

        [TestMethod]
        public void AnalyseMethodShouldReturnOneDifferenceFoundWhenDifferencesAreInTheEndOfRightBlock()
        {
            // Arrange
            var leftBlock = new byte[] { 1, 2, 3, 4, 5 };
            var rightBlock = new byte[] { 1, 2, 0, 0, 0 };

            var expectedDiffBlock = new DiffBlock(2, 3);

            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);

            // Assert
            AssertDifferenceFound(result, expectedDiffBlock);
        }

        [TestMethod]
        public void AnalyseMethodShouldReturnOneDifferenceFoundWhenDifferencesAreInTheMiddleOfLeftBlock()
        {
            // Arrange
            var leftBlock = new byte[] { 1, 2, 0, 4, 5 };
            var rightBlock = new byte[] { 1, 2, 3, 4, 5 };

            var expectedDiffBlock = new DiffBlock(2, 1);

            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);

            // Assert
            AssertDifferenceFound(result, expectedDiffBlock);
        }

        [TestMethod]
        public void AnalyseMethodShouldReturnOneDifferenceFoundWhenDifferencesAreInTheMiddleOfRightBlock()
        {
            // Arrange
            var leftBlock = new byte[] { 1, 2, 3, 4, 5 };
            var rightBlock = new byte[] { 1, 2, 0, 4, 5 };

            var expectedDiffBlock = new DiffBlock(2, 1);

            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);

            // Assert
            AssertDifferenceFound(result, expectedDiffBlock);
        }

        [TestMethod]
        public void AnalyseMethodShouldReturnOneDifferenceFoundWhenAllBytesAreDifferentsInTheLeftBlock()
        {
            // Arrange
            var leftBlock = new byte[] { 0, 0, 0, 0, 0 };
            var rightBlock = new byte[] { 1, 2, 3, 4, 5 };

            var expectedDiffBlock = new DiffBlock(0, 5);

            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);

            // Assert
            AssertDifferenceFound(result, expectedDiffBlock);
        }

        [TestMethod]
        public void AnalyseMethodShouldReturnOneDifferenceFoundWhenAllBytesAreDifferentsInTheRightBlock()
        {
            // Arrange
            var leftBlock = new byte[] { 1, 2, 3, 4, 5 };
            var rightBlock = new byte[] { 0, 0, 0, 0, 0 };

            var expectedDiffBlock = new DiffBlock(0, 5);
            
            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);

            // Assert
            AssertDifferenceFound(result, expectedDiffBlock);
        }

        [TestMethod]
        public void AnalyseMethodShouldReturnTwoDifferencesFoundsWhenThereAreTwoBlocksDifferentsInTheLeft()
        {
            // Arrange
            var leftBlock = new byte[] { 0, 0, 3, 0, 0 };
            var rightBlock = new byte[] { 1, 2, 3, 4, 5 };

            DiffBlock[] expectedDiffBlocks =
            {
                new DiffBlock(0, 2),
                new DiffBlock(3, 2)
            };

            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);

            // Assert
            AssertDifferenceFound(result, expectedDiffBlocks);
        }

        [TestMethod]
        public void AnalyseMethodShouldReturnTwoDifferencesFoundsWhenThereAreTwoBlocksDifferentsInTheRight()
        {
            // Arrange
            var leftBlock = new byte[] { 1, 2, 3, 4, 5 };
            var rightBlock = new byte[] { 1, 0, 3, 0, 5 };

            DiffBlock[] expectedDiffBlocks =
            {
                new DiffBlock(1, 1),
                new DiffBlock(3, 1)
            };

            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);

            // Assert
            AssertDifferenceFound(result, expectedDiffBlocks);
        }

        [TestMethod]
        public void AnalyseMethodShouldReturnTwoDifferencesFoundsWhenThereIsOneDifferentBlockInTheLeftAndOneInTheRight()
        {
            // Arrange
            var leftBlock = new byte[] { 0, 0, 3, 4, 5 };
            var rightBlock = new byte[] { 1, 2, 3, 0, 0 };

            DiffBlock[] expectedDiffBlocks =
            {
                new DiffBlock(0, 2),
                new DiffBlock(3, 2)
            };

            // Act
            var result = _diffAnalyser.Analyse(leftBlock, rightBlock);

            // Assert
            AssertDifferenceFound(result, expectedDiffBlocks);
        }

        private static void AssertDifferenceFound(DiffResult result, params DiffBlock[] expectedDiffBlocks)
        {
            Assert.AreEqual(DiffStatus.DifferencesFound, result.Status);
            Assert.AreEqual(expectedDiffBlocks.Length, result.DiffBlocks.Count);

            for (int i = 0; i < expectedDiffBlocks.Length; i++)
            {
                DiffBlock expected = expectedDiffBlocks[i];
                DiffBlock resultDiffBlock = result.DiffBlocks.ElementAt(i);
                Assert.AreEqual(expected.Offset, resultDiffBlock.Offset);
                Assert.AreEqual(expected.Lenght, resultDiffBlock.Lenght);
            }
        }
    }
}