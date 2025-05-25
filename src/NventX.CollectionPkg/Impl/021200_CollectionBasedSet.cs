using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NventX.CollectionPkg.Impl
{
    /// <summary>
    /// A set implementation based on an internal collection and set.
    /// Provides thread-safe add, remove, clear, and set operations.
    /// For thread safety, the base collection and internal set must themselves be thread-safe,
    /// and must not be modified externally outside of this set implementation.
    /// </summary>
    /// <remarks>
    /// This implementation assumes exclusive ownership over the provided collection and set.
    /// Any external modifications may lead to data corruption or race conditions.
    /// </remarks>
    public class CollectionBasedSet<TElement>
        : ICollection<TElement>, IReadOnlyCollection<TElement>, ISet<TElement>
    {
        /// <summary>
        /// A lock object for synchronizing collection operations.
        /// </summary>
        private object OperationLock { get; } = new();

        /// <summary>
        /// The base collection used to store set elements.
        /// </summary>
        private ICollection<TElement> BaseCollection { get; }

        /// <summary>
        /// The internal set that synchronizes with the base collection for fast lookups.
        /// </summary>
        private ISet<TElement> InternalSet { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionBasedSet{TElement}"/> class.
        /// </summary>
        /// <param name="funcToCreateExclusiveBaseCollection">A function that creates the base collection instance for storing set elements.</param>
        /// <param name="funcToCreateExclusiveInternalHashSet">A function that creates the internal set for fast lookups, given the base collection. If null, a default HashSet is used.</param>
        /// <exception cref="ArgumentNullException">Thrown if the function or its result is null.</exception>
        public CollectionBasedSet(
            Func<ICollection<TElement>> funcToCreateExclusiveBaseCollection,
            Func<ICollection<TElement>, ISet<TElement>>? funcToCreateExclusiveInternalHashSet)
        {
            lock (OperationLock)
            {
                if (funcToCreateExclusiveBaseCollection == null)
                    throw new ArgumentNullException(nameof(funcToCreateExclusiveBaseCollection));

                BaseCollection = funcToCreateExclusiveBaseCollection()
                                    ?? throw new ArgumentNullException(nameof(funcToCreateExclusiveBaseCollection), $"{nameof(funcToCreateExclusiveBaseCollection)} returned null.");

                funcToCreateExclusiveInternalHashSet ??= baseCollection => new HashSet<TElement>(baseCollection);

                InternalSet = funcToCreateExclusiveInternalHashSet(BaseCollection)
                                ?? throw new ArgumentNullException(nameof(funcToCreateExclusiveInternalHashSet), $"{nameof(funcToCreateExclusiveInternalHashSet)} returned null.");
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        public int Count => BaseCollection.Count;

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public bool IsReadOnly => BaseCollection.IsReadOnly;

        /// <summary>
        /// Adds an item to the collections.
        /// </summary>
        /// <param name="item">The item to add to the set.</param>
        /// <returns>Nothing. If the item already exists, the method returns without adding.</returns>
        public virtual void Add(TElement item)
        {
            lock (OperationLock)
            {
                if (IsReadOnly)
                    throw new NotSupportedException("The collection is read-only.");
                if (InternalSet.Contains(item))
                    return; // No need to add if it already exists
                InternalSet.Add(item);
                BaseCollection.Add(item);
            }
        }

        /// <summary>
        /// Removes the specified item from the collections.
        /// </summary>
        /// <summary>
        /// Removes the specified item from the collections.
        /// </summary>
        /// <param name="item">The item to remove from the set.</param>
        /// <returns>True if the item was successfully removed; otherwise, false.</returns>
        /// <exception cref="NotSupportedException">Thrown if the collection is read-only.</exception>
        public virtual bool Remove(TElement item)
        {
            lock (OperationLock)
            {
                if (IsReadOnly)
                    throw new NotSupportedException("The collection is read-only.");
                if (!InternalSet.Contains(item))
                    return false; // No need to remove if it doesn't exist
                InternalSet.Remove(item);
                return BaseCollection.Remove(item);
            }
        }

        /// <summary>
        /// Removes all items from the collections.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if the collection is read-only.</exception>
        public void Clear()
        {
            lock (OperationLock)
            {
                if (IsReadOnly)
                    throw new NotSupportedException("The collection is read-only.");
                InternalSet.Clear();
                BaseCollection.Clear();
            }
        }

        /// <inheritdoc />
        public bool Contains(TElement item)
        {
            return InternalSet.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(TElement[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex + Count > array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Array index is out of range.");
            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("The destination array has insufficient space.", nameof(array));
            InternalSet.CopyTo(array, arrayIndex);

        }

        /// <summary>
        /// Returns an enumerator that iterates through a snapshot of the collection at the time of the call.
        /// For thread safety, the enumerator is created from a copy of the collection.
        /// </summary>
        /// <returns>An enumerator for the snapshot of the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a thread-safe enumerator for the collection.
        /// Internally creates a snapshot of the current state to avoid enumeration issues due to concurrent modifications.
        /// </summary>
        /// <returns>An enumerator over a snapshot of the elements.</returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            lock (OperationLock)
            {
                return InternalSet.ToArray().AsEnumerable().GetEnumerator();
            }
        }

        /// <inheritdoc/>
        bool ISet<TElement>.Add(TElement item)
        {
            if (IsReadOnly)
                throw new NotSupportedException("The collection is read-only.");

            lock (OperationLock)
            {
                if (Contains(item))
                    return false;
                InternalSet.Add(item);
                BaseCollection.Add(item);
                return true;
            }
        }

        /// <inheritdoc/>
        public void IntersectWith(IEnumerable<TElement> other)
        {
            if (IsReadOnly)
                throw new NotSupportedException("The collection is read-only.");

            if (other == null)
                throw new ArgumentNullException(nameof(other));
            lock (OperationLock)
            {

                var toRemain = new HashSet<TElement>(other);
                var toRemove = new List<TElement>();

                foreach (var item in InternalSet)
                {
                    if (!toRemain.Contains(item))
                    {
                        toRemove.Add(item);
                    }
                }
                foreach (var item in toRemove)
                {
                    InternalSet.Remove(item);
                    BaseCollection.Remove(item);
                }
            }
        }

        /// <inheritdoc/>
        public void ExceptWith(IEnumerable<TElement> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (IsReadOnly)
                throw new NotSupportedException("The collection is read-only.");

            foreach (var item in other)
            {
                Remove(item);
            }
        }

        /// <inheritdoc/>
        public void SymmetricExceptWith(IEnumerable<TElement> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (IsReadOnly)
                throw new NotSupportedException("The collection is read-only.");

            lock (OperationLock)
            {
                var otherSet = new HashSet<TElement>(other);

                // categrize items to add and remove
                var toRemove = new List<TElement>();
                var toAdd = new List<TElement>();

                foreach (var item in otherSet)
                {
                    if (InternalSet.Contains(item))
                        toRemove.Add(item);
                    else
                        toAdd.Add(item);
                }

                foreach (var item in toRemove)
                {
                    InternalSet.Remove(item);
                    BaseCollection.Remove(item);
                }

                foreach (var item in toAdd)
                {
                    InternalSet.Add(item);
                    BaseCollection.Add(item);
                }
            }
        }

        /// <inheritdoc/>
        public void UnionWith(IEnumerable<TElement> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (IsReadOnly)
                throw new NotSupportedException("The collection is read-only.");

            lock (OperationLock)
            {
                foreach (var item in other)
                {
                    if (!InternalSet.Contains(item))
                    {
                        InternalSet.Add(item);
                        BaseCollection.Add(item);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public bool IsSupersetOf(IEnumerable<TElement> other)
        {
            return InternalSet.IsSupersetOf(other);
        }

        /// <inheritdoc/>
        public bool IsProperSupersetOf(IEnumerable<TElement> other)
        {
            return InternalSet.IsProperSupersetOf(other);
        }

        /// <inheritdoc/>
        public bool IsSubsetOf(IEnumerable<TElement> other)
        {
            return InternalSet.IsSubsetOf(other);
        }

        /// <inheritdoc/>
        public bool IsProperSubsetOf(IEnumerable<TElement> other)
        {
            return InternalSet.IsProperSubsetOf(other);
        }

        /// <inheritdoc/>
        public bool Overlaps(IEnumerable<TElement> other)
        {
            return InternalSet.Overlaps(other);
        }

        /// <inheritdoc/>
        public bool SetEquals(IEnumerable<TElement> other)
        {
            return InternalSet.SetEquals(other);
        }
    }

}
