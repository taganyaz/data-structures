// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Given a reference head of type ListNode that is the head of a singly linked list, reorder the list in-place such that the nodes are reordered to form the following pattern:
1st node -> last node -> 2nd node -> 2nd to last node -> 3rd node ...
Example1:
Input: head = [1,2,3,4]
Output: [1,4,2,3]

Example2:
Input: head = [1,2,3,4,5]
Output: [1,5,2,4,3]

Approach:
- Find midpoint of the list
- Reverse secons half of the list
- Travrese the list from both ends, changing the next pointer to reoder the list

**/
var node1 = ListNodeUtil.CreateFromList([1, 2, 3, 4]);
List<int> output1 = [1, 4, 2, 3];
var result1 = ListNodeUtil.ToList(ListNodeUtil.ReOrder(node1));

var node2 = ListNodeUtil.CreateFromList([1,2,3,4,5]);
List<int> output2 = [1,5,2,4,3];
var result2 = ListNodeUtil.ToList(ListNodeUtil.ReOrder(node2));

Debug.Assert(result1.SequenceEqual(output1), "Test 1: Should return expected output");
Debug.Assert(result2.SequenceEqual(output2), "Test 2: Should return expected output");

Console.WriteLine("All tests passed!");
class ListNodeUtil
{
    public static ListNode ReOrder(ListNode? head)
    {
        if (head == null || head.Next == null)
            return head;

        ListNode? slow = head;
        ListNode? fast = head;

        while (fast.Next != null && fast.Next.Next != null)
        {
            slow = slow.Next;
            fast = fast.Next.Next;
        }

        ListNode? prev = null;
        ListNode? current = slow.Next;
        slow.Next = null;

        while (current != null)
        {
            ListNode? next = current.Next;
            current.Next = prev;
            prev = current;
            current = next;
        }

        ListNode? first = head;
        ListNode? second = prev;

        while (second != null)
        {
            var firstNext = first.Next;
            var secondNext = second.Next;

            first.Next = second;
            second.Next = firstNext;

            first = firstNext;
            second = secondNext;
        }

        return head;

    }

    public static ListNode CreateFromList(List<int> list)
    {
        if (list == null || list.Count == 0)
            return null;

        var node = new ListNode(list[0]);
        for (int i = 1; i < list.Count; i++)
        {
            node.Add(list[i]);
        }
        return node;
    }

    public static List<int> ToList(ListNode? node)
    {
        if (node == null)
            return new List<int>();

        var nodes = new List<int>();
        var current = node;

        while (current != null)
        {
            nodes.Add(current.Value);
            current = current.Next;
        }

        return nodes;
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
