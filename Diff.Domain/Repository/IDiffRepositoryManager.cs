using System.Threading.Tasks;

namespace Diff.Domain.Repository
{
    /// <summary>
    /// Persists the data to be diff-ed.
    /// </summary>
    public interface IDiffRepositoryManager
    {
        /// <summary>
        /// When overrided in a class, retrieve the left and right blocks associated with the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Identifier of left/right blocks.</param>
        /// <returns>
        /// <see cref="DiffData"/> releated to provided <paramref name="id"/>, otherwise <c>null</c>.
        /// </returns>
        Task<DiffData> GetById(int id);
        /// <summary>
        /// When overrided in a class, save the data to storage, adding the data if not present
        /// or updating the same if it already exists.
        /// </summary>
        /// <param name="id">Identifier of left/right blocks to be persisted</param>
        /// <param name="data">Data to be saved.</param>
        /// <param name="itemType">Define if the data being saved is related to left or right.</param>
        Task Save(int id, string data, DiffItemType itemType);
    }
}