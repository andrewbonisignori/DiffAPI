namespace Diff.Domain
{
    /// <summary>
    /// Analyse two blocks of data in order to provide the differences between them, if exists.
    /// </summary>
    public interface IDiffAnalyser
    {
        /// <summary>
        /// When overrided in a derived class, compare two blocks of data in order to check if there are differences between them.
        /// If any difference is found, they are returned as a list containing
        /// the offset plus the length of each difference.
        /// If no differences were found, returns only the status.
        /// The <paramref name="left"/> and <paramref name="right"/> need to be of same size,
        /// otherwise the analysis will not be possible.
        /// </summary>
        /// <param name="left">Left block.</param>
        /// <param name="right">Right block.</param>
        /// <returns>
        /// Status <see cref="DiffStatus.BlocksAreNotOfSameSize"/>, when blocks do not have same size and
        /// the analisys could not be executed.
        /// Status <see cref="DiffStatus.DifferencesNotFound"/>, when analisys is executed and the blocks haven't differences.
        /// Status <see cref="DiffStatus.DifferencesFound"/>, when analysis is executed and differences were found.
        /// </returns>
        DiffResult Analyse(byte[] left, byte[] right);
    }
}