namespace Diff.Domain.Repository
{
    /// <summary>
    /// Represents the blocks of data that could be diff-ed.
    /// </summary>
    public sealed class DiffData
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