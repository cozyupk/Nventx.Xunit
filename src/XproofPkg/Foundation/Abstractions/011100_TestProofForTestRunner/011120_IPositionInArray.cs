namespace Xproof.Abstractions.TestProofForTestRunner
{
    /// <summary>
    /// Represents a position in an array, providing the index and total count of elements.
    /// </summary>
    public interface IPositionInArray
    {
        /// <summary>
        /// Gets the index of the current position in the array.
        /// Note that this is a 1-based index, meaning the first element has an index of 1.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Gets the total number of elements in the array.
        /// </summary>
        int Total { get; }

        /// <summary>
        /// Moves to the next position in the array.
        /// </summary>
        void MoveToNext();
    }
}
