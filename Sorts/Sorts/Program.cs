using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Sorts
{
    class Program
    {
        static void Main(string[] args)
        {
            var unSortedList = new List<int>{ 100,10,200,1,54,45,5,66,99 };
            var rand = new Random().Next(1000000);
            var rand1 = new Random();

            for (var i = rand; i > 1; i--)            
                unSortedList.Add(rand1.Next(1000000));
            
            Console.WriteLine($"No Of Items {unSortedList.Count}");
            var inArray1 = unSortedList.ToArray();
            Console.WriteLine($"Time Start Quick Sort {DateTime.Now.ToString("HH:mm:ss")}");           
            Sort.QuickSort(inArray1);
            Console.WriteLine($"Time Completed Quick Sort {DateTime.Now.ToString("HH:mm:ss")}");
            var inArray2 = unSortedList.ToArray();
            Console.WriteLine($"Time Start Min Heap Construction & Sort {DateTime.Now.ToString("HH:mm:ss")}");
            Sort.MinHeapSort(inArray2);
            Console.WriteLine($"Time Completed Min Heap  Construction & Sort {DateTime.Now.ToString("HH:mm:ss")}");              
            Console.ReadLine();
        }
    }


    public class Sort
    {
        public static void Swap(int[] inArray,int pos1,int pos2)
        {
            var temp = inArray[pos1];
            inArray[pos1] = inArray[pos2];
            inArray[pos2] = temp;          
        }
        private static void RecursiveQuickSort(int[] inArray, int startInd, int endInd)
        {
            if (startInd >= endInd)
                return;
            var pivotInd = (startInd + endInd) / 2;
            var pivotEl = inArray[pivotInd];

            var left = startInd;
            var right = endInd;

            while (left <= right)
            {
                while (inArray[left] < pivotEl)
                    left++;

                while (inArray[right] > pivotEl)
                    right--;

                if (left <= right)
                {
                    Swap(inArray, left, right);
                    left++;
                    right--;
                }
            }

            RecursiveQuickSort(inArray, startInd, left-1);
            RecursiveQuickSort(inArray, left , endInd);
        }
        public static void QuickSort(int[] inArray)
        {            
            if(inArray==null || inArray.Length == 0)//validation
                return;            
            
            RecursiveQuickSort(inArray,0,inArray.Length-1);                
        }

        public static void MinHeapSort(int[] inArray)
        {
            MinHeap heap = new MinHeap(inArray.Length);

            foreach (var el in inArray) heap.Add(el);
                      
            int index = 0;
            foreach(var el in heap.Poll())
            {
                inArray[index] = el;
                index++;
            }
        }      

    }

    public class MinHeap
    {
        private int _size = 0;
        private int _capacity = 10;
        private int[] _items;

        public MinHeap(int capacity)
        {
            _items = new int[capacity];
            _capacity = capacity;         
        }
        private static int getParentIndex(int index) => (index - 1) / 2; //RG or index-1>>1 for bit manipulation

        private static int getLeftChildIndex(int index) => (2 * index) + 1;

        private static int getRightChildIndex(int index) => (2 * index) + 2;

        private void increaseCapacity()
        {
            _capacity = _capacity * 2;
            var temp = new int[_capacity];
            Array.Copy(_items, temp, _items.Length);
            _items = temp;
        }

        public void Add(int value)
        {                                 
            _items[_size] = value;            
            heapifyUp(_size);
            _size++;
        }
        public IEnumerable<int> Poll()
        {
            var size = _size;
            for (var index = 0; index < size; index++)
            {
                var retDt = _items[0];
                _size--;
                if (_size > 0)
                {
                    _items[0] = _items[_size - 1];
                    heapifyDown(0);
                }
                yield return retDt;
            }
        }

        private void heapifyDown(int parentIndex)
        {           
            var leftChildIndex = getLeftChildIndex(parentIndex);
            var rightChildIndex = getRightChildIndex(parentIndex);

            if (_size < leftChildIndex + 1)
                return;

            var leftChild = _items[leftChildIndex];
            if (_size >= rightChildIndex + 1)
            {
                var rightChild = _items[rightChildIndex];
                if (rightChild < _items[parentIndex] && rightChild < leftChild)
                {
                    Sort.Swap(_items, parentIndex, rightChildIndex);
                    heapifyDown(rightChildIndex);
                    return;
                }
            }
            if (leftChild < _items[parentIndex])
            {
                Sort.Swap(_items, parentIndex, leftChildIndex);
                heapifyDown(leftChildIndex);
            }
        }
        private void heapifyUp(int childIndex)
        {
            if (childIndex == 0)
                return;
            var parentIndex = getParentIndex(childIndex);
            if (_items[parentIndex] > _items[childIndex])
            {
                Sort.Swap(_items, parentIndex, childIndex);
                heapifyUp(parentIndex);
            }
        }

    }
}


