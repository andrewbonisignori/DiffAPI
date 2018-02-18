namespace Diff.Repository
{
    /// <summary>
    /// Repository that manages diff bkocks.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// When overrided in a derived class, persists the data.
        /// </summary>
        /// <param name="data">Diff data to be persisted.</param>
        InMemoryDiffData GetById(int id);

        /// <summary>
        /// When overrided in a derived class, retrieves the data related to provided <paramref name="id"/> or
        /// <c>null</c> in case of the data was not found.
        /// </summary>
        /// <param name="id">Id that identifies the data to be retrieved.</param>
        /// <returns>Data found or <c>null</c>.</returns>
        void Save(InMemoryDiffData data);

        /// <summary>
        /// When overrided in a derived class, erase all data in memory.
        /// </summary>
        void ClearData();

        /// <summary>
        /// When overrided in a derived class, returns the amount of data stored right now.
        /// </summary>
        /// <returns></returns>
        int DiffDataCount();
    }
}