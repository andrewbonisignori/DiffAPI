using AutoMapper;
using Diff.Domain;
using Diff.Domain.Repository;
using Diff.Filters;
using Microsoft.Web.Http;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Diff.Controllers
{
    /// <summary>
    /// Reponsible for receive the blocks that are going to be diff-ed and responsible for the diff analysis itself.
    /// </summary>
    [ApiVersion("1.0")]
    [RoutePrefix("v{version:apiVersion}/diff")]
    public class DiffController : ApiController
    {
        private readonly IDiffRepositoryManager _repository;
        private readonly IDiffAnalyser _diffAnalyser;
        private readonly IMapper _mapper;

        public DiffController(IDiffRepositoryManager repository, IDiffAnalyser diffAnalyser, IMapper mapper)
        {
            _repository = repository;
            _diffAnalyser = diffAnalyser;
            _mapper = mapper;
        }

        /// <summary>
        /// Provide the left data that is going to be diff-ed.
        /// </summary>
        /// <param name="id">Identify one block that are going to be diff-ed.</param>
        /// <param name="data">Base64 string to be diff-ed.</param>
        /// <returns>HTTP success(200) if the data was saved successfully, otherwise HTTP bad request(400) or internal server error (500).</returns>
        /// <remarks>
        /// Two blocks related to same <paramref name="id"/> are expected in order to a analysis be executed, one to left and another to right side.
        /// </remarks>
        /// <response code="200">Data was updated.</response>
        /// <response code="400">The parameter <paramref name="data"/> cannot be null.</response>
        /// <response code="400">The parameter <paramref name="data"/> is not a valid base 64 string.</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("{id:int}/left")]
        [ValidateBase64Parameter("data")]
        public async Task<IHttpActionResult> PostLeftDiffData(int id, [FromBody]string data)
        {
            return await SavePostDiffData(id, data, DiffItemType.Left);
        }

        /// <summary>
        /// Provide the right data that is going to be diff-ed.
        /// </summary>
        /// <param name="id">Identify one block that are going to be diff-ed.</param>
        /// <param name="data">Base64 string to be diff-ed.</param>
        /// <returns>HTTP success(200) if the data was saved successfully, otherwise HTTP bad request(400) or internal server error (500).</returns>
        /// <remarks>
        /// Two blocks related to same <paramref name="id"/> are expected in order to a analysis be executed, one to left and another to right side.
        /// </remarks>
        /// <response code="200">Data was updated.</response>
        /// <response code="400">The parameter <paramref name="data"/> cannot be null.</response>
        /// <response code="400">The parameter <paramref name="data"/> is not a valid base 64 string.</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPost]
        [Route("{id:int}/right")]
        [ValidateBase64Parameter("data")]
        public async Task<IHttpActionResult> PostRightDiffData(int id, [FromBody]string data)
        {
            return await SavePostDiffData(id, data, DiffItemType.Right);
        }

        /// <summary>
        /// Persists received data accordingly to <paramref name="diffItemType"/>.
        /// </summary>
        /// <param name="id">Data identity.</param>
        /// <param name="data">Base 64 string data to be persisted.</param>
        /// <param name="diffItemType">Define if data belongs to left or to the right.</param>
        /// <returns>The HTTP status of the operation.</returns>
        private async Task<IHttpActionResult> SavePostDiffData(int id, string data, DiffItemType diffItemType)
        {
            try
            {
                await _repository.Save(id, data, diffItemType);
            }
            catch
            {
                // Some additional log routines could be added.
                // No further information is being send to the cliente due to a possible security reasons.
                return InternalServerError();
            }

            return Ok();
        }

        /// <summary>
        /// Execute the diff-ed analysis of the previously provided blocks (left and right).
        /// </summary>
        /// <param name="id">Identify one block that are going to be diff-ed.</param>
        /// <returns>
        /// If success, returns <see cref="Models.Diff.DiffResult"/> with the analisys results.
        /// If the <paramref name="id"/> was not found, returns HTTP not found (404).
        /// In case of an unexpected error, returns a Internal server error (500).
        /// </returns>
        /// <response code="200">Data found.</response>
        /// <response code="400">No data was found for provided <paramref name="id"/>.</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetDiffData(int id)
        {
            DiffData diffData = await _repository.GetById(id);
            if(diffData == null)
            {
                return NotFound();
            }

            try
            {
                Domain.DiffResult result = _diffAnalyser.Analyse(diffData.Left, diffData.Right);
                return Ok(_mapper.Map<Models.Diff.DiffResult>(result));
            }
            catch
            {
                // Some additional log routines could be added.
                // No further information is being send to the cliente due to a possible security reasons.
                return InternalServerError();
            }
        }
    }
}