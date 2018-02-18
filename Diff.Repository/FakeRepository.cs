using Diff.Repository.Repositories;

namespace Diff.Repository
{
    /// <summary>
    /// Repository that manages diff bkocks.
    /// Concrete implementation to use <see cref="FakeInMemoryRepository"/>.
    /// </summary>
    public sealed class FakeRepository: IRepository
    {
        /// <summary>
        /// Persists the data.
        /// </summary>
        /// <param name="data">Diff data to be persisted.</param>
        public void Save(InMemoryDiffData data)
        {
            FakeInMemoryRepository.AddOrUpdate(data);
        }

        /// <summary>
        /// Retrieves the data related to provided <paramref name="id"/> or
        /// <c>null</c> in case of the data was not found.
        /// </summary>
        /// <param name="id">Id that identifies the data to be retrieved.</param>
        /// <returns>Data found or <c>null</c>.</returns>
        public InMemoryDiffData GetById(int id)
        {
            return FakeInMemoryRepository.Get(id);
        }

        /// <summary>
        /// Erase all data in memory.
        /// </summary>
        public void ClearData()
        {
            FakeInMemoryRepository.ClearData();
        }

        /// <summary>
        /// Returns the amount of data stored right now.
        /// </summary>
        /// <returns></returns>
        public int DiffDataCount()
        {
            return FakeInMemoryRepository.DiffDataCount();
        }
    }
}