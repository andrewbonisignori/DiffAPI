namespace Diff.Domain
{
    /// <summary>
    /// Represents one difference found while comparing blocks of data.
    /// </summary>
    public sealed class DiffBlock
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DiffBlock"/>.
        /// </summary>
        internal DiffBlock()
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="DiffBlock"/>.
        /// </summary>
        /// <param name="offset">Position of the difference in array. It is a zero based, so the first position is 0.</param>
        /// <param name="lenght">Length of the difference.</param>
        public DiffBlock(int offset, int lenght)
        {
            Offset = offset;
            Lenght = lenght;
        }

        /// <summary>
        /// Position of the difference in array.
        /// It is a zero based, so the first position is 0.
        /// </summary>
        public int Offset { get; internal set; }
        /// <summary>
        /// Length of the difference.
        /// </summary>
        public int Lenght { get; internal set; }
    }
}