using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NventX.FoundationPkg.Abstractions.Traits;

namespace NventX.FoundationPkg.Abstractions.Observing
{
    /// <summary>
    /// Thread-safe set implementation for elements that can remove themselves.
    /// Elements are stored in a concurrent dictionary and are automatically removed if they indicate so.
    /// </summary>
    internal class ConcurrentSelfCleaningSet<TType>
        : IEnumerable<TType>, IReadOnlyCollection<TType>, ICollection<TType>, ISet<TType>
        where TType : notnull
    {
        /// <summary>
        /// Underlying concurrent dictionary to store elements as keys
        /// </summary>
        protected internal ConcurrentDictionary<TType, byte> Dictionary { get; } = new();

        /// <summary>
        /// Lock to ensure atomic snapshot cleanup during iteration
        /// </summary>
        protected internal object SnapShotLock { get; } = new object();

        /// <summary>
        /// Thread-safe snapshot of the current elements.
        /// </summary>
        protected internal List<TType>? CachedSnapShot { get; set; } = null;

        /// <summary>
        /// Returns a thread-safe copy of the current elements, removing any that indicate they should be removed.
        /// </summary>
        /// <remarks>
        /// Order of elements is not guaranteed.
        /// </remarks>
        /// <returns>A copy of the current elements.</returns>
        public IReadOnlyList<TType> Snapshot {
            get {

                // Lock to ensure thread-safe access to the handler list
                lock (SnapShotLock)
                {
                    // Return the cached snapshot if it exists
                    if (CachedSnapShot != null)
                    {
                        return CachedSnapShot;
                    }

                    // Create a new snapshot
                    CachedSnapShot = new();

                    foreach (var element in Dictionary.Keys)
                    {
                        // Remove handlers that indicate they should be removed
                        if (element is ISelfRemovable selfRemovable && selfRemovable.CanRemove())
                        {
                            Dictionary.TryRemove(element, out _);
                            continue;
                        }
                        // Add valid element to the snapshot
                        CachedSnapShot.Add(element);
                    }

                    return CachedSnapShot;
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the current snapshot of the set.
        /// </summary>
        public IEnumerator<TType> GetEnumerator() => Snapshot.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through the current snapshot of the set (non-generic version).
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets the number of elements contained in the current snapshot of the set.
        /// </summary>
        public int Count => Snapshot.Count();

        /// <summary>
        /// Gets a value indicating whether the set is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Adds one or more elements to the set in a thread-safe manner.
        /// Throws an exception if any element is null.
        /// </summary>
        /// <param name="elements">Elements to add.</param>
        public void AddElements(params TType[] elements)
        {
            lock (SnapShotLock)
            {
                if (elements == null)
                    throw new ArgumentNullException(nameof(elements), "elements cannot be null.");

                foreach (var elem in elements)
                {
                    if (elem == null)
                        throw new ArgumentNullException(nameof(elements), "elements contains a null element.");
                    // Add the handler to the dictionary
                    Dictionary.TryAdd(elem, byte.MinValue);
                }

                CachedSnapShot = null; // Invalidate the cached snapshot
            }
        }

        /// <summary>
        /// Removes one or more elements from the set in a thread-safe manner.
        /// Ignores null elements.
        /// </summary>
        /// <param name="elements">Elements to remove.</param>
        public void RemoveElements(params TType[] elements)
        {
            lock (SnapShotLock)
            {
                foreach (var elem in elements)
                {
                    if (elem == null)
                        continue;

                    // Remove the handler from the dictionary
                    Dictionary.TryRemove(elem, out _);
                }

                CachedSnapShot = null; // Invalidate the cached snapshot
            }
        }

        /// <summary>
        /// Adds an element to the set in a thread-safe manner.
        /// Throws an exception if the element is null.
        /// </summary>
        /// <param name="item">Element to add.</param>
        public void Add(TType item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            lock (SnapShotLock)
            {
                Dictionary.TryAdd(item, byte.MinValue);
                CachedSnapShot = null; // Invalidate the cached snapshot
            }
        }

        /// <summary>
        /// Removes all elements from the set in a thread-safe manner.
        /// </summary>
        public void Clear()
        {
            lock (SnapShotLock)
            {
                Dictionary.Clear();
                CachedSnapShot = null; // Invalidate the cached snapshot
            }
        }

        /// <summary>
        /// Determines whether the set contains a specific element.
        /// </summary>
        /// <param name="item">Element to locate.</param>
        /// <returns>True if the element is found; otherwise, false.</returns>
        public bool Contains(TType item)
        {
            return Dictionary.ContainsKey(item);
        }

        /// <summary>
        /// Copies the elements of the set to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">Destination array.</param>
        /// <param name="arrayIndex">Starting index in the destination array.</param>
        public void CopyTo(TType[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            var snapshot = Snapshot;
            if (array.Length - arrayIndex < snapshot.Count())
                throw new ArgumentException("The destination array has insufficient space.");
            int i = 0;
            foreach (var item in snapshot)
            {
                array[arrayIndex + i] = item;
                i++;
            }
        }

        /// <summary>
        /// Removes the specified element from the set in a thread-safe manner.
        /// </summary>
        /// <param name="item">Element to remove.</param>
        /// <returns>True if the element was removed; otherwise, false.</returns>
        public bool Remove(TType item)
        {
            lock (SnapShotLock)
            {
                var removed = Dictionary.TryRemove(item, out _);
                if (removed)
                    CachedSnapShot = null; // Invalidate the cached snapshot
                return removed;
            }
        }

        /// <summary>
        /// Adds an element to the set and returns true if the element was added.
        /// </summary>
        /// <param name="item">Element to add.</param>
        /// <returns>True if the element was added; false if it was already present.</returns>
        bool ISet<TType>.Add(TType item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "item cannot be null.");
            lock (SnapShotLock)
            {
                var added = Dictionary.TryAdd(item, byte.MinValue);
                if (added)
                    CachedSnapShot = null; // Invalidate the cached snapshot
                return added;
            }
        }

        /// <summary>
        /// Removes all elements in the specified collection from the set.
        /// </summary>
        /// <param name="other">Collection of elements to remove.</param>
        public void ExceptWith(IEnumerable<TType> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            lock (SnapShotLock)
            {
                foreach (var item in other)
                {
                    if (item != null)
                        Dictionary.TryRemove(item, out _);
                }
                CachedSnapShot = null; // Invalidate the cached snapshot
            }
        }

        /// <summary>
        /// Modifies the set to contain only elements that are also in the specified collection.
        /// </summary>
        /// <param name="other">Collection to intersect with.</param>
        public void IntersectWith(IEnumerable<TType> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            var otherSet = new HashSet<TType>(other);
            lock (SnapShotLock)
            {
                foreach (var item in Dictionary.Keys)
                {
                    if (!otherSet.Contains(item))
                        Dictionary.TryRemove(item, out _);
                }
                CachedSnapShot = null; // Invalidate the cached snapshot
            }
        }

        /// <summary>
        /// Determines whether the set is a proper subset of the specified collection.
        /// </summary>
        public bool IsProperSubsetOf(IEnumerable<TType> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            var otherSet = new HashSet<TType>(other);
            return Snapshot.Count() < otherSet.Count && Snapshot.All(otherSet.Contains);
        }

        /// <summary>
        /// Determines whether the set is a proper superset of the specified collection.
        /// </summary>
        public bool IsProperSupersetOf(IEnumerable<TType> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            var otherSet = new HashSet<TType>(other);
            return Snapshot.Count() > otherSet.Count && otherSet.All(Snapshot.Contains);
        }

        /// <summary>
        /// Determines whether the set is a subset of the specified collection.
        /// </summary>
        public bool IsSubsetOf(IEnumerable<TType> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            var otherSet = new HashSet<TType>(other);
            return Snapshot.All(otherSet.Contains);
        }

        /// <summary>
        /// Determines whether the set is a superset of the specified collection.
        /// </summary>
        public bool IsSupersetOf(IEnumerable<TType> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            return other.All(item => Snapshot.Contains(item));
        }

        /// <summary>
        /// Determines whether the set overlaps with the specified collection.
        /// </summary>
        public bool Overlaps(IEnumerable<TType> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            return other.Any(item => Snapshot.Contains(item));
        }

        /// <summary>
        /// Determines whether the set and the specified collection contain the same elements.
        /// </summary>
        public bool SetEquals(IEnumerable<TType> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            var snapshot = new HashSet<TType>(Snapshot);
            var otherSet = new HashSet<TType>(other);
            return snapshot.SetEquals(otherSet);
        }

        /// <summary>
        /// Modifies the set to contain only elements present either in the set or in the specified collection, but not both.
        /// </summary>
        public void SymmetricExceptWith(IEnumerable<TType> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            lock (SnapShotLock)
            {
                var otherSet = new HashSet<TType>(other);
                foreach (var item in otherSet)
                {
                    if (!Dictionary.TryRemove(item, out _))
                        Dictionary.TryAdd(item, byte.MinValue);
                }
                CachedSnapShot = null; // Invalidate the cached snapshot
            }
        }

        /// <summary>
        /// Modifies the set to contain all elements that are present in itself or in the specified collection.
        /// </summary>
        public void UnionWith(IEnumerable<TType> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            lock (SnapShotLock)
            {
                foreach (var item in other)
                {
                    if (item != null)
                        Dictionary.TryAdd(item, byte.MinValue);
                }
                CachedSnapShot = null; // Invalidate the cached snapshot
            }
        }

        public void InvalidateSnapshot()
        {
            CachedSnapShot = null;
        }
    }
}
