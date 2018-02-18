namespace Diff.Tests.Functional.Models.Diff
{
    /// <summary>
    /// Represents one difference found while comparing blocks of data.
    /// </summary>
    public sealed class ClientSideDiffBlock
    {
        /// <summary>
        /// Position of the difference in array.
        /// It is a zero based, so the first position is 0.
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// Length of the difference.
        /// </summary>
        public int Lenght { get; set; }
    }
}