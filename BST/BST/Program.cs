using System;
using System.Collections.Generic;
using System.Linq;

namespace BST
{
    class Program
    {
        static void Main(string[] args)
        {
            var unSortedList = new List<int>();
            var rand = new Random().Next(1000000);
            
            var rand1 = new Random();
            for (var i = rand; i > 1; i--)
            {
                unSortedList.Add(rand1.Next(1000000));
            }
            Console.WriteLine($"No Of Items {unSortedList.Count}");
            BST bst = new BST();
            Console.WriteLine($"Time Start BST Construct & In Order {DateTime.Now.ToString("HH:mm:ss")}");
            bst.ConstructTree(unSortedList);
            var ret=bst.GetSortedValues();
            Console.WriteLine($"Time Completed BST Cosntruct & In Order {DateTime.Now.ToString("HH:mm:ss")}");
            Console.WriteLine($"Time Start BST Balance & In Order {DateTime.Now.ToString("HH:mm:ss")}");
            bst.Balance();
            var ret1=bst.GetSortedValues();
            Console.WriteLine($"Time Completed BST Balance & In Order {DateTime.Now.ToString("HH:mm:ss")}");
            Console.ReadLine();
        }
    }

    public class BST
    {
        private Node _root;
       
        public List<int> GetSortedValues()
        {
            List<int> sqValues = new List<int>();
            RecursiveInOrder(_root, sqValues);
            return sqValues;
        }
        public void Balance()
        {
            var sortedValues = GetSortedValues();
            _root = null;
            RecursiveBal(sortedValues);
        }
        private void RecursiveBal( IEnumerable<int> sortedValues)
        {
            if (sortedValues.Count() > 0)
            {
                var pivot = sortedValues.Count() / 2;
                AddNode(sortedValues.ElementAt(pivot));
                RecursiveBal(sortedValues.Take(pivot));
                RecursiveBal(sortedValues.Skip(pivot+1));
            }
        }
        private void RecursiveInOrder(Node curNode, List<int> valuesList)
        {
            if (valuesList == null)
            {
                valuesList = new List<int>();
            }
            if (curNode == null)
            {
                return;
            }
            RecursiveInOrder(curNode.LeftNode, valuesList);
            valuesList.Add(curNode.Value);
            RecursiveInOrder(curNode.RightNode, valuesList);
        }
        public void ConstructTree(IEnumerable<int> inArray)
        {
            foreach(var ar in inArray)
            {
                AddNode(ar);               
            }
        }
        private Node RecursiveFindNode(int val,Node curNode)
        {
            
            if (val > curNode.Value && curNode.RightNode!=null)
            {
                return RecursiveFindNode(val, curNode.RightNode);
            }
            else if (val <curNode.Value && curNode.LeftNode != null)
            {
                return RecursiveFindNode(val, curNode.LeftNode);
            }
            else
            {
                return curNode;
            }
        }
        private void AddNode(int val)
        {
            if (_root == null)
            {
                _root = new Node(val);
            }
            else
            {
                var curNode = RecursiveFindNode(val, _root);
                Node nd = new Node(val);
                 if (val<curNode.Value)
                {
                    curNode.LeftNode = nd;
                }
                else
                {
                    curNode.RightNode = nd;
                }
            }
        }
    }

    public class Node
    {
        public int Value { get; set; }

        public Node LeftNode { get; set; }

        public Node RightNode { get; set; }

        public Node(int val)
        {
            Value = val;
        }
    }
}
