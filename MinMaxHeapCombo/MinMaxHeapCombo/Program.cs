using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MinMaxHeapCombo
{
    /// <summary>
    /// create two heaps
    /// max heap of lesser number 
    /// min heap of bigger number
    /// make sure both heaps are of same size or differ only by 1
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Heap minHeap = new Heap(HeapType.Min);
            Heap maxHeap = new Heap(HeapType.Max);
            Console.WriteLine("Welcome..");
            Console.WriteLine("Enter comma separated numbers and press enter (Type EXIT and  press enter to exit)....");
            Console.WriteLine("Numbers are consolidated from previous entry");
            var line=Console.ReadLine();
            while (line.Trim().ToLower()!="exit") {
                line.Split(',').ToList().ForEach(el =>
                {
                    if (Int32.TryParse(el, out int num))
                    {
                        AddDataToHeap(minHeap, maxHeap, num);
                        BalanceHeap(minHeap, maxHeap);
                    }
                });
                Console.WriteLine($"Median calculated is {GetMedian(minHeap,maxHeap)}");
                Console.WriteLine("Enter comma separated numbers and press enter (Type EXIT and  press enter to exit)....");
                Console.WriteLine("Numbers are consolidated from previous entry");
                line = Console.ReadLine();
            }                       
        }

        static double GetMedian(Heap heap1,Heap heap2)
        {
            if (heap1.GetSize() > heap2.GetSize())
                return heap1.Peak();
            else if (heap2.GetSize() > heap1.GetSize())
                return heap2.Peak();
            else
            {
                return ((double)(heap1.Peak() + heap2.Peak())) / 2;
            }
        }
        static void BalanceHeap(Heap heap1,Heap heap2)
        {
            if (heap1.GetSize() > heap2.GetSize() + 1)
                heap2.Add(heap1.poll());  

            else if (heap2.GetSize() > heap1.GetSize() + 1)
                heap1.Add(heap2.poll());

        }
        static void AddDataToHeap(Heap minHeap, Heap maxHeap,int data)
        {
            if (minHeap == null || maxHeap == null)
                return;

            if (minHeap.GetSize() == 0)
                minHeap.Add(data);
            else
            {
                int peak = minHeap.Peak();
                if (data > peak)
                    minHeap.Add(data);
                else
                    maxHeap.Add(data);
            }
        }
    }
    public enum HeapType { Min,Max}
    public class Heap
    {
        private int[] _data;
        private int _size=0;
        private int _capacity=5;
        private HeapType _type;
        public Heap(HeapType type)
        {
            _type = type;
            _data = new int[_capacity];
        }

        private void ensureCapacity()
        {
            if (_size == _capacity-1)
            {
                _capacity = _capacity * 2;
                var temp = new int[_capacity];
                Array.Copy(_data, temp, _size);
                _data = temp;
            }
        }
        private void swap(int index1, int index2)
        {
            var temp = _data[index1];
            _data[index1] = _data[index2];
            _data[index2] = temp;
        }

        private int getLeftChildIndex(int index) => (index * 2) + 1;

        private int getRightChildIndex(int index) => (index * 2) + 2;

        private int getParentIndex(int index) => (index -1)/2;

        public void Add(int value)
        {
            ensureCapacity();
            _size++;
            _data[_size - 1] = value;
            heapifyUp(_size-1);            
        }

        public int Peak()
        {
            return _data[0];
        }

        public int poll()
        {
            var ret = _data[0];
            _data[0] = _data[_size - 1];
            _data[_size - 1] = -1;
            _size--;
            heapifyDown(0);
            return ret;
        }

        public int GetSize()
        {
            return _size;
        }
        private void heapifyUp(int index)
        {
            if (index == 0)
                return;

            var parentIndex = getParentIndex(index);
            if (_type == HeapType.Min)
            {
                if (_data[index] < _data[parentIndex])
                {
                    swap(index, parentIndex);
                    heapifyUp(parentIndex);
                }
            }
            else
            {
                if (_data[index] > _data[parentIndex])
                {
                    swap(index, parentIndex);
                    heapifyUp(parentIndex);
                }
            }
        }


        private void heapifyDown(int index)
        {
            if (index == _size - 1)
                return;

            var leftChildIndex = getLeftChildIndex(index);
            var rightChildIndex = getRightChildIndex(index);

            var pickedIndex = index;
            if (_type == HeapType.Min)
            {
                if (leftChildIndex < _size && _data[leftChildIndex] < _data[pickedIndex])
                    pickedIndex = leftChildIndex;

                if (rightChildIndex < _size && _data[rightChildIndex] < _data[pickedIndex])
                    pickedIndex = rightChildIndex;
            }
            else
            {
                if (leftChildIndex < _size && _data[leftChildIndex] > _data[pickedIndex])
                    pickedIndex = leftChildIndex;

                if (rightChildIndex < _size && _data[rightChildIndex] > _data[pickedIndex])
                    pickedIndex = rightChildIndex;
            }

            if (pickedIndex != index)
            {
                swap(index, pickedIndex);
                heapifyDown(pickedIndex);
            }

        }

    }
}
