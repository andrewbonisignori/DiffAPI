using AutoMapper;
using Diff.Repository;
using System;
using System.Threading.Tasks;

namespace Diff.Domain.Repository
{
    /// <summary>
    /// Persists the data to be diff-ed.
    /// </summary>
    /// <remarks>
    /// This repository have access to final repository layer and could
    /// contais additional and more complex logic than logic exposed by
    /// repository layer.
    /// </remarks>
    public sealed class DiffRepositoryManager : IDiffRepositoryManager
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DiffRepositoryManager(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Save the data to storage, adding the data if not present
        /// or updating the same if it already exists.
        /// </summary>
        /// <param name="id">Identifier of left/right blocks to be persisted</param>
        /// <param name="data">Data to be saved.</param>
        /// <param name="itemType">Define if the data being saved is related to left or right.</param>
        public Task Save(int id, string data, DiffItemType itemType)
        {
            byte[] binaryData = Convert.FromBase64String(data);

            // There isn't any invalid possible id, so any received id not present in
            // database will generate a new entry.
            InMemoryDiffData diffData = _repository.GetById(id);
            if (diffData == null)
            {
                diffData = new InMemoryDiffData { Id = id };
            }

            switch (itemType)
            {
                case DiffItemType.Left:
                    diffData.Left = binaryData;
                    break;
                case DiffItemType.Right:
                    diffData.Right = binaryData;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemType));
            }
                        
            _repository.Save(diffData);

            // Here a Task is being returned just in order to alow
            // other methods to simulate a async call to some IO resource.
            return Task.CompletedTask;
        }

        /// <summary>
        /// Retrieve the left and right blocks associated with the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Identifier of left/right blocks.</param>
        /// <returns>
        /// <see cref="DiffData"/> releated to provided <paramref name="id"/>, otherwise <c>null</c>.
        /// </returns>
        public Task<DiffData> GetById(int id)
        {
            // Here there is only the mapping between domain object
            // to final repository layer, but we could add specific
            // domain logic if needed.
            var diffData = _repository.GetById(id);

            // Here a Task is being returned just in order to alow
            // other methods to simulate a async call to some IO resource.
            return Task.FromResult(_mapper.Map<DiffData>(diffData));
        }
    }
}