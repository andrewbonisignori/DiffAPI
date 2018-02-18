namespace Diff.Domain
{
    /// <summary>
    /// Status of the result of an analysis executed in two blocks of data.
    /// </summary>
    public enum DiffStatus
    {
        /// <summary>
        /// Blocks of data were analysed and no differences were found.
        /// </summary>
        DifferencesNotFound,
        /// <summary>
        /// Blocks of data were analysed and differences were found.
        /// </summary>
        DifferencesFound,
        /// <summary>
        /// Blocks could not be analysed because aren't of the same size.
        /// </summary>
        BlocksAreNotOfSameSize
    }
}