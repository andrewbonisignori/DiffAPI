namespace Diff.Repository
{
    /// <summary>
    /// Represents a diff block to be persisted in memory.
    /// </summary>
    public sealed class InMemoryDiffData
    {
        /// <summary>
        /// Identifies the diff data.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Left part of data to be diff-ed.
        /// </summary>
        public byte[] Left { get; set; }
        /// <summary>
        /// Right part of data to be diff-ed.
        /// </summary>
        public byte[] Right { get; set; }
    }
}