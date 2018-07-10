using System;
using System.Collections.Generic;
using System.Linq;

namespace jrh.Algorithms.MedianMaintenance
{
    class MinHeap<T> where T : IComparable<T>
    {
        // Conceptually a MinHeap is a tree, but we store it as a list as described in lecture
        private List<T> _heap;

        // We'll track the heap's size by +1/-1 as we add/remove items
        public int Size { get; private set; }

        public MinHeap()
        {
            _heap = new List<T>();
            Size = 0;
        }

        // Insert a new item at the end of the tree/list, and bubble it up to make sure
        // keys are less than or equal to their children
        public void Add(T item)
        {
            _heap.Add(item);
            Size++;

            int index = Size - 1;

            while (index > 0)
            {
                var parent = GetParentOf(index);

                if (_heap[parent].CompareTo(_heap[index]) < 0)
                    break;

                Swap(index, parent);

                index = parent;
            }
        }

        // Remove and return the minimum element of the heap, which due to all our work
        // is already at the very top
        public T ExtractMin()
        {
            if (Size == 0)
                throw new InvalidOperationException("Trying to extract from empty heap");

            T minimum = _heap[0];

            // Move the last element to where the first (now extracted) element is
            _heap[0] = _heap[Size - 1];
            _heap.RemoveAt(Size - 1);
            Size--;

            if (Size == 0)
                return minimum;

            // Bubble down the now possibly out-of-order new root node
            int parent = 0;
            while (true)
            {
                int left = parent * 2 + 1;
                int right = parent * 2 + 2;
                int swap = parent;

                T parentNode = _heap[parent];

                if (right < Size)
                {
                    // If we have a right child, we for sure have a left one as well since the heap
                    // is populated as tree leaves left-to-right
                    T rightNode = _heap[right];
                    T leftNode = _heap[left];

                    if (leftNode.CompareTo(rightNode) < 0)
                    {
                        if (parentNode.CompareTo(leftNode) > 0)
                            swap = left;
                    }
                    else
                    {
                        if (parentNode.CompareTo(rightNode) > 0)
                            swap = right;
                    }
                }
                else if (left < Size)
                {
                    T leftNode = _heap[left];

                    if (parentNode.CompareTo(leftNode) > 0)
                        swap = left;
                }

                if (parent == swap)
                    break;

                Swap(parent, swap);

                parent = swap;
            }

            return minimum;
        }

        // Return the minimum element in the heap without removing it
        public T Min()
        {
            if (Size == 0)
                throw new InvalidOperationException("Trying to call Min() on an empty heap");

            return _heap[0];
        }

        private void Swap(int i, int j)
        {
            T temp = _heap[i];
            _heap[i] = _heap[j];
            _heap[j] = temp;
        }

        private int GetParentOf(int index)
        {
            return Convert.ToInt32(Math.Floor((double)(index - 1) / 2));
        }
    }
}