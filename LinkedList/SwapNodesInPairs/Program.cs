// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Given a reference head of type ListNode that is the head of a singly linked list, write a function to swap every two adjacent nodes and return its head.
You must solve the problem without modifying the values in the list's nodes (i.e., only nodes themselves may be changed.)
Example 1:
Input: head = [1,2,3,4]
Output: [2,1,4,3]

Example 2:
Input: head = [1,2,3]
Output: [2,1,3]

Approach:
- Use a dummy node to point to the head
- Iterative through the pairs of nodes, swapping them
- return dummy.next

**/
var node1 = ListNodeUtil.CreateFromList([1, 2, 3, 4]);
List<int> output1 = [2, 1, 4, 3];
var result1 = ListNodeUtil.ToList(ListNodeUtil.SwapPairs(node1));

var node2 = ListNodeUtil.CreateFromList([1,2,3]);
List<int> output2 =  [2,1,3];
var result2 = ListNodeUtil.ToList(ListNodeUtil.SwapPairs(node2));

Debug.Assert(result1.SequenceEqual(output1), "Test 1: should return expected output");
Debug.Assert(result2.SequenceEqual(output2), "Test 1: should return expected output");

Console.WriteLine("All tests have passed!");
class ListNodeUtil
{
    public static ListNode SwapPairs(ListNode head)
    {
        if (head == null || head.Next == null)
        {
            return head;
        }

        var dummy = new ListNode(-1);
        dummy.Next = head;

        var first = dummy.Next;
        var prev = dummy;
        //var second = dummy.Next.Next;

        while (first != null && first.Next != null)
        {
            var second = first.Next;
            var nextFirst = second.Next;

            first.Next = second.Next;
            second.Next = first;

            prev.Next = second;

            prev = first;
            first = nextFirst;
        }

        return dummy.Next;
    }

    public static ListNode CreateFromList(List<int> list)
    {
        if (list == null || list.Count == 0)
            return null;

        var head = new ListNode(list[0]);
        for (int i = 1; i < list.Count; i++)
        {
            head.Add(list[i]);
        }
        return head;
    }

    public static List<int> ToList(ListNode? head)
    {
        if (head == null)
            return new List<int>();

        var list = new List<int>();
        var current = head;
        while (current != null)
        {
            list.Add(current.Value);
            current = current.Next;
        }

        return list;
    }
}
class ListNode
{
    public ListNode(int value, ListNode? next = null)
    {
        Value = value;
        Next = next;
    }
    public int Value { get; private set; }
    public ListNode? Next { get; set; }

    public ListNode Add(int value)
    {
        var current = this;
        while (current.Next != null)
        {
            current = current.Next;
        }
        current.Next = new ListNode(value);
        return this;
    }
}
