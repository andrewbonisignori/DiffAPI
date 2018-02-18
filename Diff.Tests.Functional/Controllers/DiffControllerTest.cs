using Diff.Tests.Functional.Models.Diff;
using Microsoft.Owin.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Diff.Tests.Functional.Controllers
{
    [TestClass]
    public class DiffControllerTest
    {
        #region Functional Tests

        #region Tests for API call v1/diff/{id}/left and v1/diff/{id}/right

        [TestMethod]
        public async Task PostLeftDiffDataShouldReturnBadRequestWhenDataIsNull()
        {
            await PostDiffDataShouldReturnBadRequestWhenDataIsNull(DiffItemTypeApiCall.Left);
        }

        [TestMethod]
        public async Task PostLeftRightDataShouldReturnBadRequestWhenDataIsNull()
        {
            await PostDiffDataShouldReturnBadRequestWhenDataIsNull(DiffItemTypeApiCall.Right);
        }

        private async Task PostDiffDataShouldReturnBadRequestWhenDataIsNull(DiffItemTypeApiCall diffItemType)
        {
            // Arrange
            var someValidId = 1;
            byte[] nullData = null;

            // Act
            HttpResponseMessage response = await CallPostDiffData(someValidId, diffItemType, nullData);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task PostLeftDiffDataShouldReturnBadRequestWhenDataIsNotConvertedFromBase64()
        {
            await PostDiffDataShouldReturnBadRequestWhenDataIsNotConvertedFromBase64(DiffItemTypeApiCall.Left);
        }

        [TestMethod]
        public async Task PostRightDiffDataShouldReturnBadRequestWhenDataIsNotConvertedFromBase64()
        {
            await PostDiffDataShouldReturnBadRequestWhenDataIsNotConvertedFromBase64(DiffItemTypeApiCall.Right);
        }

        private async Task PostDiffDataShouldReturnBadRequestWhenDataIsNotConvertedFromBase64(DiffItemTypeApiCall diffItemType)
        {
            // Arrange
            var someValidId = 2;
            var notBase64String = "notBase64String";

            // Act
            HttpResponseMessage response = await CallPostDiffData(someValidId, diffItemType, notBase64String);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task PostLeftDiffDataShouldReturnIsSuccessStatusCodeWhenIdAndDataAreValid()
        {
            await PostDiffDataShouldReturnIsSuccessStatusCodeWhenIdAndDataAreValid(DiffItemTypeApiCall.Left);
        }

        [TestMethod]
        public async Task PostRightDiffDataShouldReturnIsSuccessStatusCodeWhenIdAndDataAreValid()
        {
            await PostDiffDataShouldReturnIsSuccessStatusCodeWhenIdAndDataAreValid(DiffItemTypeApiCall.Right);
        }

        private async Task PostDiffDataShouldReturnIsSuccessStatusCodeWhenIdAndDataAreValid(DiffItemTypeApiCall diffItemType)
        {
            // Arrange
            var someValidId = 3;
            var validData = new byte[] { 1, 2, 3, 4, 5 };

            // Act
            HttpResponseMessage response = await CallPostDiffData(someValidId, diffItemType, validData);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        #endregion

        #region Tests for API call v1/diff/{id}

        [TestMethod]
        public async Task GetDiffDataShouldReturnNotFoundWhenIdNotExists()
        {
            // Arrange
            var leftBlock = new byte[1];
            var rightBlock = new byte[1];
            int idToAddData = 1;
            int idToRetrieveData = 2;

            // Act
            HttpResponseMessage response = await GetDiffAnalysisResponse(leftBlock, rightBlock, idToAddData, idToRetrieveData);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetDiffDataShouldReturnBlocksAreNotOfSameSizeWhenBlocksAreOfDifferentSize()
        {
            // Arrange
            var leftBlock = new byte[] { 1, 2, 3, 4, 5 };
            var rightBlock = new byte[] { 1, 2, 3, 4 };

            // Act
            ClientSideDiffResult diffResultValue = await GetDiffAnalysisResponseWithSuccess(leftBlock, rightBlock);

            // Assert
            Assert.AreEqual("BlocksAreNotOfSameSize", diffResultValue.Status);
        }

        [TestMethod]
        public async Task GetDiffDataShouldReturnDifferencesNotFoundWhenBlocksAreOfSameSizeAndSameSequence()
        {
            // Arrange
            var leftBlock = new byte[] { 1, 2, 3, 4, 5 };
            var rightBlock = new byte[] { 1, 2, 3, 4, 5 };

            // Act
            ClientSideDiffResult diffResultValue = await GetDiffAnalysisResponseWithSuccess(leftBlock, rightBlock);

            // Assert
            Assert.AreEqual("DifferencesNotFound", diffResultValue.Status);
        }

        [TestMethod]
        public async Task GetDiffDataShouldReturnOneDifferenceFoundWhenDifferencesAreInTheBeginningInLeftBlock()
        {
            // Arrange
            var leftBlock = new byte[] { 0, 0, 3, 4, 5 };
            var rightBlock = new byte[] { 1, 2, 3, 4, 5 };
            var expectedDiffBlocks = new ClientSideDiffBlock { Offset = 0, Lenght = 2 };

            // Act
            ClientSideDiffResult diffResultValue = await GetDiffAnalysisResponseWithSuccess(leftBlock, rightBlock);

            // Assert
            AssertDifferenceFounds(diffResultValue, expectedDiffBlocks);
        }

        [TestMethod]
        public async Task GetDiffDataShouldReturnOneDifferenceFoundWhenDifferencesAreInTheEndOfRightBlock()
        {
            // Arrange
            var leftBlock = new byte[] { 1, 2, 3, 4, 5 };
            var rightBlock = new byte[] { 1, 2, 3, 0, 0 };
            var expectedDiffBlocks = new ClientSideDiffBlock { Offset = 3, Lenght = 2 };

            // Act
            ClientSideDiffResult diffResultValue = await GetDiffAnalysisResponseWithSuccess(leftBlock, rightBlock);

            // Assert
            AssertDifferenceFounds(diffResultValue, expectedDiffBlocks);
        }

        [TestMethod]
        public async Task GetDiffDataShouldReturnOneDifferenceFoundWhenDifferencesAreInTheMiddleOfLeftBlock()
        {
            // Arrange
            var leftBlock = new byte[] { 1, 2, 0, 4, 5 };
            var rightBlock = new byte[] { 1, 2, 3, 4, 5 };
            var expectedDiffBlocks = new ClientSideDiffBlock { Offset = 2, Lenght = 1 };

            // Act
            ClientSideDiffResult diffResultValue = await GetDiffAnalysisResponseWithSuccess(leftBlock, rightBlock);

            // Assert
            AssertDifferenceFounds(diffResultValue, expectedDiffBlocks);
        }

        [TestMethod]
        public async Task GetDiffDataShouldReturnTwoDifferencesFoundsWhenThereIsOneDifferentBlockInTheLeftAndOneInTheRight()
        {
            // Arrange
            var leftBlock = new byte[] { 0, 0, 3, 4, 5 };
            var rightBlock = new byte[] { 1, 2, 3, 0, 0 };

            ClientSideDiffBlock[] expectedDiffBlocks =
            {
                new ClientSideDiffBlock { Offset = 0, Lenght = 2 },
                new ClientSideDiffBlock { Offset = 3, Lenght = 2 }
            };

            // Act
            ClientSideDiffResult diffResultValue = await GetDiffAnalysisResponseWithSuccess(leftBlock, rightBlock);

            // Assert
            AssertDifferenceFounds(diffResultValue, expectedDiffBlocks);
        }

        #endregion

        #region Asssert helper methods

        /// <summary>
        /// When the expected result found differences, execute the analysis of received response.
        /// </summary>
        /// <param name="result">Result from API call to be analysed.</param>
        /// <param name="expectedDiffBlocks">Expected differences.</param>
        private static void AssertDifferenceFounds(ClientSideDiffResult result, params ClientSideDiffBlock[] expectedDiffBlocks)
        {
            Assert.AreEqual("DifferencesFound", result.Status);
            Assert.AreEqual(expectedDiffBlocks.Length, result.DiffBlocks.Count);

            for (int i = 0; i < expectedDiffBlocks.Length; i++)
            {
                ClientSideDiffBlock expected = expectedDiffBlocks[i];
                ClientSideDiffBlock resultDiffBlock = result.DiffBlocks.ElementAt(i);
                Assert.AreEqual(expected.Offset, resultDiffBlock.Offset);
                Assert.AreEqual(expected.Lenght, resultDiffBlock.Lenght);
            }
        }

        #endregion

        #region API call helper methods

        /// <summary>
        /// Execute the API calls needed in order to provide necessary data to be analysed
        /// and to make possible to a result be retrieved.
        /// This method is supposed to be used when a success (HTTP 200) return is expected.
        /// </summary>
        /// <param name="leftBlock">Left block of data to be analysed.</param>
        /// <param name="rightBlock">Right block of data to be analysed.</param>
        /// <returns>Returns the analysis result.</returns>
        private async Task<ClientSideDiffResult> GetDiffAnalysisResponseWithSuccess(byte[] leftBlock, byte[] rightBlock)
        {
            // API call to retrieve the result from analysis of the provided data.
            HttpResponseMessage diffResult = await GetDiffAnalysisResponse(leftBlock, rightBlock);
            Assert.IsTrue(diffResult.IsSuccessStatusCode);

            string message = await diffResult.Content.ReadAsStringAsync();

            // Convert the result for to the expected model object.
            var diffResultValue = JsonConvert.DeserializeObject<ClientSideDiffResult>(message);
            return diffResultValue;
        }

        /// <summary>
        /// Execute the API calls needed in order to provide necessary data to be analysed
        /// and to make possible to a result be retrieved.
        /// </summary>
        /// <param name="leftBlock">Left block of data to be analysed.</param>
        /// <param name="rightBlock">Right block of data to be analysed.</param>
        /// <param name="idToAddData">Identifier used to send block data to server.</param>
        /// <param name="idToRetrieveData">Identity send to retrieve the data from server.</param>
        /// <returns>Returns the analysis result.</returns>
        private async Task<HttpResponseMessage> GetDiffAnalysisResponse(byte[] leftBlock, byte[] rightBlock, int idToAddData = 1, int idToRetrieveData = 1)
        {
            using (var client = GetClient())
            {
                // API call to provide left data.
                HttpResponseMessage addLeftResult = await CallPostDiffData(idToAddData, DiffItemTypeApiCall.Left, leftBlock, client);
                Assert.IsTrue(addLeftResult.IsSuccessStatusCode);

                // API call to provide right data.
                HttpResponseMessage addRightResult = await CallPostDiffData(idToAddData, DiffItemTypeApiCall.Right, rightBlock, client);
                Assert.IsTrue(addRightResult.IsSuccessStatusCode);

                // API call to retrieve the result from analysis of the provided data.
                HttpResponseMessage diffResult = await CallGetToRetrieveDiffResult(idToRetrieveData, client);

                return diffResult;
            }
        }

        /// <summary>
        /// Call API v1/diff/{id}/left or v1/diff/{id}/right depending on provided parameter <paramref name="diffItemType"/>.
        /// </summary>
        /// <param name="id">Identify one left/right blocks that are going to be diff-ed.</param>
        /// <param name="diffItemType">Define if received data is the left or right block.</param>
        /// <param name="data">Data to be converted to base64 string and stored in order to be diff-ed in future.</param>
        /// <param name="client">Configured client to execute the http request. If no one is provided, a new one will be created.</param>
        private static async Task<HttpResponseMessage> CallPostDiffData(int id, DiffItemTypeApiCall diffItemType, byte[] data, HttpClient client = null)
        {
            string base64Data = data == null ? null : Convert.ToBase64String(data);
            return await CallPostDiffData(id, diffItemType, base64Data, client);
        }

        /// <summary>
        /// Call API v1/diff/{id}/left or v1/diff/{id}/right depending on provided parameter <paramref name="diffItemType"/>.
        /// </summary>
        /// <param name="id">Identify one left/right blocks that are going to be diff-ed.</param>
        /// <param name="diffItemType">Define if received data is the left or right block.</param>
        /// <param name="data">Data to be stored in order to be diff-ed in future.</param>
        /// <param name="client">Configured client to execute the http request. If no one is provided, a new one will be created.</param>
        private static async Task<HttpResponseMessage> CallPostDiffData(int id, DiffItemTypeApiCall diffItemType, string data, HttpClient client = null)
        {
            // This method could be used as a part of a sequence of calls to API
            // that need use an existing HttpClient, provided as parameter.
            // If this method is being called outside of the scope of other calls and
            // no HttpClient was provided, we need create it and dispose it.
            bool clientCreated = false;
            if (client == null)
            {
                clientCreated = true;
                client = GetClient();
            }

            try
            {
                string url = $"v1/diff/{id}/{diffItemType}";
                return await client.PostAsJsonAsync<string>(url, data);
            }
            finally
            {
                if (clientCreated)
                {
                    client.Dispose();
                }
            }
        }

        /// <summary>
        /// Call API v1/diff/{id}
        /// </summary>
        /// <param name="id">Identify one left/right blocks that are going to be diff-ed.</param>
        /// <param name="client">Configured client to execute the http request.</param>
        private static async Task<HttpResponseMessage> CallGetToRetrieveDiffResult(int id, HttpClient client)
        {
            string url = $"v1/diff/{id}";
            return await client.GetAsync(url);
        }

        /// <summary>
        /// Configure client to be used along the requests.
        /// </summary>
        private static HttpClient GetClient()
        {
            return new HttpClient { BaseAddress = baseAddress };
        }

        #endregion

        #endregion

        #region Self hosted HTTP Server

        // Self hosted server to make it possible to integration tests be executed
        // without the need of an external web server. It also make possible to run
        // integration tests in build servers in an easier way.
        private static IDisposable webApp;
        private static readonly Uri baseAddress = new Uri("http://localhost:9999/");

        [AssemblyInitialize]
        public static void SetUp(TestContext context)
        {
            webApp = WebApp.Start<Startup>(baseAddress.AbsoluteUri);
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            webApp.Dispose();
        }

        #endregion
    }
}