using System;
using System.Collections.Generic;
using System.Linq;

namespace Sorts
{
    class Program
    {
        static void Main(string[] args)
        {

            Sort st = new Sort();
            var unSortedList = new List<int>();
            var rand = new Random().Next(1000000);
            var rand1 = new Random();
            for (var i = rand; i >1; i--)
            {
                unSortedList.Add(rand1.Next(1000000));
            }

            Console.WriteLine($"No Of Items {unSortedList.Count}");
            Console.WriteLine($"Time Start Quick Sort {DateTime.Now.ToString("HH:mm:ss")}");
            var test = st.QuickSort(unSortedList.ToArray());
            Console.WriteLine($"Time Completed Quick Sort {DateTime.Now.ToString("HH:mm:ss")}");            
            Console.WriteLine($"Time Start Max Heap Sort {DateTime.Now.ToString("HH:mm:ss")}");
            var ret=st.MaxHeapSort(unSortedList.ToArray());
            Console.WriteLine($"Time Completed Max Heap Sort {DateTime.Now.ToString("HH:mm:ss")}");
            Console.WriteLine($"Time Start Min Heap Sort {DateTime.Now.ToString("HH:mm:ss")}");
            var ret1 = st.MinHeapSort(unSortedList.ToArray());
            Console.WriteLine($"Time Completed Min Heap Sort {DateTime.Now.ToString("HH:mm:ss")}");
            Console.ReadLine();
        }
    }

    public class Sort
    {
        
        private List<int> RecursiveQuickSort(List<int> inList)
        {            
            var pivotEl = inList[inList.Count / 2];
            var lessList = new List<int>();
            var highList = new List<int>();
            var equalList = new List<int>();
            inList.ForEach(el =>
            {
                if (el < pivotEl)
                {
                    lessList.Add(el);
                }
                else if (el > pivotEl)
                {
                    highList.Add(el);
                }
                else
                {
                    equalList.Add(el);
                }
            });
            inList.Clear();
            if (lessList.Count > 0)
                inList.AddRange(RecursiveQuickSort(lessList));

            inList.AddRange(equalList);
            if (highList.Count > 0)
                inList.AddRange(RecursiveQuickSort(highList));
            return inList;
        }
        public int[] QuickSort(int[] inArray)
        {
            
            if(inArray==null || inArray.Length == 0)
            {
                //validation
                return null;
            }

            return RecursiveQuickSort(inArray.ToList()).ToArray();                
        }

        public int[] MaxHeapSort(int[] inArray)
        {
            var len = inArray.Length;
            var revSortedList = new List<int>();
            var newArray=BuildMaxHeap(inArray);
            for (var i = 0; i < len; i++)
            {
                int largest;
                newArray = RemoveLargest(newArray, out largest);
                revSortedList.Add(largest);
            }
            revSortedList.Reverse();
            return revSortedList.ToArray();
        }

        public int[] MinHeapSort(int[] inArray)
        {

            var len = inArray.Length;
            var sortedLst = new List<int>();
            var newArray=BuildMinHeap(inArray);
            for (var i = 0; i < len; i++)
            {
                int smallest;
                newArray = RemoveSmallest(newArray, out smallest);
                sortedLst.Add(smallest);
            }

            return sortedLst.ToArray();

        }

       private int[] RemoveLargest(int[] array,out int larg)
        {
            larg = array[0];
            Swap(array, 0, array.Length - 1);
            int[] retArray = new int[array.Length - 1];
            Array.Copy(array, 0, retArray, 0, array.Length - 1);
            MaxBubbleDown(retArray, 0);
            return retArray;
        }

        private int[] RemoveSmallest(int[] array, out int small)
        {
            small = array[0];
            Swap(array, 0, array.Length - 1);
            int[] retArray = new int[array.Length - 1];
            Array.Copy(array, 0, retArray, 0, array.Length - 1);
            MinBubbleDown(retArray, 0);
            return retArray;
        }

        private int[] BuildMaxHeap(int[] inArray)
        {
            int[] newArray = new int[inArray.Length];

            for(var ind = 0; ind < inArray.Length; ind++)
            {
                newArray[ind] = inArray[ind];
                if (ind > 0)
                {
                    MaxBubbleUp(newArray, ind, ind - 1 >> 1);
                }
            }
            return newArray;
        }

        private int[] BuildMinHeap(int[] inArray)
        {
            int[] newArray = new int[inArray.Length];

            for (var ind = 0; ind < inArray.Length; ind++)
            {
                newArray[ind] = inArray[ind];
                if (ind > 0)
                {
                    MinBubbleUp(newArray, ind, ind - 1 >> 1);
                }
            }

            return newArray;
        }



        private void MaxBubbleUp(int[] array,int curInd,int parInd)
        {
            if (array[curInd] > array[parInd])
            {
                Swap(array, curInd, parInd);
                if (parInd > 0)
                {
                    MaxBubbleUp(array, parInd, parInd - 1 >> 1);
                }
            }
        }


        private void MinBubbleUp(int[] array, int curInd, int parInd)
        {
            if (array[curInd] < array[parInd])
            {
                Swap(array, curInd, parInd);
                if (parInd > 0)
                {
                    MinBubbleUp(array, parInd, parInd - 1 >> 1);
                }
            }
        }
        private void MaxBubbleDown(int[] array, int curInd)
        {
            var childInd1 = curInd * 2 + 1;
            var childInd2 = curInd * 2 + 2;
            if(array.Length > childInd1
                 && array.Length > childInd2)
            {
                if(array[childInd1] > array[curInd] && array[childInd1] >= array[childInd2])
                {
                    Swap(array, curInd, childInd1);
                    MaxBubbleDown(array, childInd1);
                }
               else if (array[childInd2] > array[curInd] && array[childInd2] > array[childInd1])
                {
                    Swap(array, curInd, childInd2);
                    MaxBubbleDown(array, childInd2);
                }
            }
            else if(array.Length > childInd1)
            {
                if (array[childInd1] > array[curInd])
                {
                    Swap(array, curInd, childInd1);
                    MaxBubbleDown(array, childInd1);
                }
            }
            else if (array.Length > childInd2)
            {
                if (array[childInd2] > array[curInd])
                {
                    Swap(array, curInd, childInd2);
                    MaxBubbleDown(array, childInd2);
                }
            }
        }


        private void MinBubbleDown(int[] array, int curInd)
        {
            var childInd1 = curInd * 2 + 1;
            var childInd2 = curInd * 2 + 2;
            if (array.Length > childInd1
                 && array.Length > childInd2)
            {
                if (array[childInd1] < array[curInd] && array[childInd1] <= array[childInd2])
                {
                    Swap(array, curInd, childInd1);
                    MinBubbleDown(array, childInd1);
                }
               else  if (array[childInd2] < array[curInd] && array[childInd2] < array[childInd1])
                {
                    Swap(array, curInd, childInd2);
                    MinBubbleDown(array, childInd2);
                }
            }
            else if (array.Length > childInd1)
            {
                if (array[childInd1] < array[curInd])
                {
                    Swap(array, curInd, childInd1);
                    MinBubbleDown(array, childInd1);
                }
            }
            else if (array.Length > childInd2)
            {
                if (array[childInd2] < array[curInd])
                {
                    Swap(array, curInd, childInd2);
                    MinBubbleDown(array, childInd2);
                }
            }
        }
        private void Swap(int[] inArray, int ind1, int ind2)
        {
            var tmp = inArray[ind2];
            inArray[ind2] = inArray[ind1];
            inArray[ind1] = tmp;
        }
       


    }

   
}


