using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

namespace NventX.CollectionPkg.Impl
{
    /// <summary>
    /// A collection implementation based on an internal dictionary.
    /// Provides thread-safe add, remove, clear, and enumeration operations.
    /// </summary>
    /// <remarks>
    /// For thread safety, dictionary provided by <paramref name="funcToCreateDictionary"/> must be thread-safe.
    /// No reentrant lock is used, so operations on this collection should not be nested.
    /// This implementation assumes exclusive ownership over the provided collection.
    /// Any external modifications may lead to data corruption or race conditions.
    /// </remarks>
    public class DictionaryBasedCollection<TElement>
        : ICollection<TElement>, IReadOnlyCollection<TElement>
    {
        /// <summary>
        /// The internal dictionary used to store collection elements.
        /// </summary>
        private IDictionary<TElement, byte> InternalDictionary { get; }

        /// <summary>
        /// Lock object for synchronizing collection operations.
        /// </summary>
        private object OperationLock { get; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryBasedCollection{TElement}"/> class.
        /// </summary>
        /// <param name="funcToCreateBaseDictionary">A function that creates the internal dictionary instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if the function or its result is null.</exception>
        public DictionaryBasedCollection(Func<IDictionary<TElement, byte>> funcToCreateBaseDictionary)
        {
            if (funcToCreateBaseDictionary == null)
            {
                throw new ArgumentNullException(nameof(funcToCreateBaseDictionary));
            }
            InternalDictionary = funcToCreateBaseDictionary()
                                   ?? throw new ArgumentNullException(nameof(funcToCreateBaseDictionary), $"{nameof(funcToCreateBaseDictionary)} returned null.");
        }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        public int Count => InternalDictionary.Keys.Count;

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public bool IsReadOnly => InternalDictionary.IsReadOnly;

        /// <summary>
        /// Called when an item is added to the collection.
        /// Can be overridden to perform custom actions.
        /// </summary>
        /// <param name="item">The item that was added.</param>
        public virtual void OnAdded(TElement item)
        {
            // Override to perform actions when an item is added.
        }

        /// <summary>
        /// Called when an item is removed from the collection or the collection is cleared.
        /// Can be overridden to perform custom actions.
        /// </summary>
        /// <param name="item">The item that was removed.</param>
        public virtual void OnRemovedIncludingCleared(TElement item)
        {
            // Override to perform actions when an item is removed or cleared.
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public virtual void Add(TElement item)
        {
            lock (OperationLock)
            {
                InternalDictionary[item] = 0;
                OnAdded(item);
            }
        }

        /// <summary>
        /// Removes the specified item from the collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was removed; otherwise, false.</returns>
        public virtual bool Remove(TElement item)
        {
            lock (OperationLock)
            {
                var isRemoved = InternalDictionary.Remove(item);
                if (isRemoved)
                {
                    OnRemovedIncludingCleared(item);
                }
                return isRemoved;
            }
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            lock (OperationLock)
            {
                // Copy keys to a list to avoid modifying the collection during iteration
                var items = InternalDictionary.Keys.ToList();
                InternalDictionary.Clear();
                foreach (var item in items)
                {
                    OnRemovedIncludingCleared(item);
                }
            }
        }

        /// <summary>
        /// Determines whether the collection contains a specific value.
        /// </summary>
        /// <param name="item">The item to locate.</param>
        /// <returns>True if the item is found; otherwise, false.</returns>
        public bool Contains(TElement item)
        {
            return InternalDictionary.Keys.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the collection to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">Thrown if the array is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the array index is out of range.</exception>
        public void CopyTo(TElement[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex + Count > array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Array index is out of range.");
            int i = 0;
            foreach (var item in InternalDictionary.Keys)
            {
                array[arrayIndex + i] = item;
                ++i;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a snapshot of the collection at the time of the call.
        /// This avoids potential exceptions due to concurrent modifications.
        /// </summary>
        /// <returns>An enumerator for a copy of the collection's elements.</returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            return InternalDictionary.Keys.ToList().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection (non-generic).
        /// Internally calls <see cref="GetEnumerator"/> to get a snapshot of the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
