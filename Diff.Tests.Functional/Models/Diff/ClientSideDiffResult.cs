using System.Collections.Generic;

namespace Diff.Tests.Functional.Models.Diff
{
    /// <summary>
    /// Represents the status that results from the analysis between
    /// two blocks (left and right) and the differences found, if any.
    /// </summary>
    public sealed class ClientSideDiffResult
    {
        /// <summary>
        /// Status that results from the analysis between
        /// two blocks (left and right).
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Differences found in analysis, if any.
        /// </summary>
        public ICollection<ClientSideDiffBlock> DiffBlocks { get; set; }
    }
}