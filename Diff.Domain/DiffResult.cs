using System.Collections.Generic;

namespace Diff.Domain
{
    /// <summary>
    /// Represents the status that results from the analysis between
    /// two bloks (left and right) and the differences found, if any.
    /// </summary>
    public sealed class DiffResult
    {
        /// <summary>
        /// Status that results from the analysis between
        /// two bloks (left and right).
        /// </summary>
        public DiffStatus Status { get; set; }
        /// <summary>
        /// Differences found in analysis, if any.
        /// </summary>
        public ICollection<DiffBlock> DiffBlocks { get; set; }
    }
}