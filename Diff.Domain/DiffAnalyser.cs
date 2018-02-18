using System;
using System.Collections.Generic;
using System.Linq;

namespace Diff.Domain
{
    /// <summary>
    /// Analyse two blocks of data in order to provide the differences between them, if exists.
    /// </summary>
    public sealed class DiffAnalyser : IDiffAnalyser
    {
        /// <summary>
        /// Compare two blocks of data in order to check if there are differences between them.
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
        /// Status <see cref="DiffStatus.DifferencesNotFound"/>, when analisys is executed and the blocks has no differences.
        /// Status <see cref="DiffStatus.DifferencesFound"/>, when analysis is executed and differences were found.
        /// </returns>
        public DiffResult Analyse(byte[] left, byte[] right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            if(left.Length != right.Length)
            {
                return new DiffResult() { Status = DiffStatus.BlocksAreNotOfSameSize };
            }

            if(left.SequenceEqual(right))
            {
                return new DiffResult() { Status = DiffStatus.DifferencesNotFound };
            }

            // Since left and right are of the same size and are not equals,
            // compare both arrays to get the differences.
            return new DiffResult()
            {
                Status = DiffStatus.DifferencesFound,
                DiffBlocks = GetDiffBlocks(left, right)
            };
        }

        /// <summary>
        /// Evaluate both blocks to identify the differences between them and returns
        /// the offset and the length of each difference.
        /// </summary>
        /// <param name="left">Left block to be analysed.</param>
        /// <param name="right">Right block to be analysed.</param>
        /// <returns>Returns the offset and the length of each difference.</returns>
        private ICollection<DiffBlock> GetDiffBlocks(byte[] left, byte[] right)
        {
            // When a different byte is found, save it and calculate
            // the lenght of this difference.
            DiffBlock currentDiff = null;

            // Save all diferences found along left and right comparison.
            var result = new List<DiffBlock>();

            for (byte i = 0; i < left.Length; i++)
            {
                // Get the byte for comparison in each array.
                byte leftByte = left[i];
                byte rightByte = right[i];

                if(leftByte != rightByte)
                {
                    // If bytes are differents, create a new
                    // diff block and continue the iteration
                    // to discover the length.
                    if(currentDiff == null)
                    {
                        currentDiff = new DiffBlock
                        {
                            Offset = i + 1
                        };
                    }
                    currentDiff.Lenght++;
                }
                else if(currentDiff != null)
                {
                    // Indicates that current diff block analysis is ended.
                    // Add the diff block to the list os results.
                    result.Add(currentDiff);
                    // Indicates that a new diff block could be started, if needed.
                    currentDiff = null;
                }
            }

            // Acumulate the last diff block in case of
            // it belongs to the end of the arrays.
            if (currentDiff != null)
            {
                result.Add(currentDiff);
            }

            return result.AsReadOnly();
        }
    }
}