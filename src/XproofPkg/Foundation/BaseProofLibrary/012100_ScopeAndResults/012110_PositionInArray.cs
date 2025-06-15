using System;
using Xproof.Abstractions.TestProofForTestRunner;

namespace Xproof.BaseProofLibrary.ScopeAndResults
{
    /// <summary>
    /// Represents a position in an array, defined by its index and total length.
    /// </summary>
    public record PositionInArray : IPositionInArray
    {
        /// <summary>
        /// Represents an index within a finite sequence (1‑based) and its total length.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Represents the total number of elements in the sequence.
        /// </summary>
        public int Total { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionInArray"/> record.
        /// </summary>
        public PositionInArray(int index, int total)
        {
            if (index < 1) throw new ArgumentOutOfRangeException(nameof(index), "Index must be 1-based.");
            if (total < 1) throw new ArgumentOutOfRangeException(nameof(total), "Total must be at least 1.");
            Index = index;
            Total = total;
        }

        /// <summary>
        /// Increments the index by one, moving to the next position in the array.
        /// </summary>
        public void MoveToNext()
        {
            if (Index < Total)
            {
                Index++;
            }
            else
            {
                throw new InvalidOperationException("Cannot increment index beyond total.");
            }
        }

        /// <summary>
        /// Returns a string representation of the position in the format "Index/Total".
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Index}/{Total}";
    }
}
