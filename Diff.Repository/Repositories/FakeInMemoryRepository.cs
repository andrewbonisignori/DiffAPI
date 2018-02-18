using System;
using System.Collections.Concurrent;

namespace Diff.Repository.Repositories
{
    /// <summary>
    /// In memory repository created in order to store data needed
    /// to accomplish the proposed exercise.
    /// </summary>
    public sealed class FakeInMemoryRepository
    {
        // Keeps the data in a thread safe way.
        private static readonly ConcurrentDictionary<int, InMemoryDiffData> _fakeDb;

        static FakeInMemoryRepository()
        {
            _fakeDb = new ConcurrentDictionary<int, InMemoryDiffData>();
        }

        /// <summary>
        /// Persists the data checking if the provided data already exists in order to insert or update.
        /// </summary>
        /// <param name="data">Diff data to be persisted.</param>
        public static void AddOrUpdate(InMemoryDiffData data)
        {
            if(data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            _fakeDb.AddOrUpdate(data.Id, data, (key, existingData) =>
            {
                existingData.Left = data.Left;
                existingData.Right = data.Right;
                return existingData;
            });
        }

        /// <summary>
        /// Retrieves the data related to provided <paramref name="id"/> or
        /// <c>null</c> in case of the data was not found.
        /// </summary>
        /// <param name="id">Id that identifies the data to be retrieved.</param>
        /// <returns>Data found or <c>null</c>.</returns>
        public static InMemoryDiffData Get(int id)
        {
            _fakeDb.TryGetValue(id, out InMemoryDiffData data);
            return data;
        }

        /// <summary>
        /// Erase all data in memory.
        /// </summary>
        public static void ClearData()
        {
            _fakeDb.Clear();
        }

        /// <summary>
        /// Returns the amount of data stored right now.
        /// </summary>
        /// <returns></returns>
        public static int DiffDataCount()
        {
            return _fakeDb.Count;
        }
    }
}