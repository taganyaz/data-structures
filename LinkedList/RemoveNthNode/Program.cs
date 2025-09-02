// See https://aka.ms/new-console-template for more information


using System.Diagnostics;
using System.Text;

/**
Problem:
Given the head of a linked list, remove the nth node from the end of the list and return its head.
Example 1:
Input: head = [1,2,3,4,5], n = 2
Output: [1,2,3,5]

Example 2:
Input: head = [1], n = 1
Output: []

Example 3:
Input: head = [1,2], n = 1
Output: [1]

Approach:
- Use two pointers, slow and fast with a dummy node
- Create a dummy node with next pointing to head
- Advance fast  pointer n+1 step
- Then advance slow and fast pointers until fast becomes null
- Set slow.next = slow.next.next to delete target
- return dummy.next

**/
var node1 = CreateLinkendList([1, 2, 3, 4, 5]);
int n1 = 2;
List<int> output1 = [1, 2, 3, 5];
var result1 = ToList(ListNodeUtil.RemoveNthNode(node1, n1));

var node2 = CreateLinkendList([1]);
int n2 = 1;
List<int> output2 = [];
var result2 = ToList(ListNodeUtil.RemoveNthNode(node2, n2));

var node3 = CreateLinkendList([1,2]);
int n3 = 1;
List<int> output3 = [1];
var result3 = ToList(ListNodeUtil.RemoveNthNode(node3, n3));

Debug.Assert(result1.SequenceEqual(output1), "Test1: should return expected result");
Debug.Assert(result2.SequenceEqual(output2), "Test2: should return expected result");
Debug.Assert(result3.SequenceEqual(output3), "Test3: should return expected result");

Console.WriteLine("All tests have passed!");

static ListNode CreateLinkendList(List<int> list)
{
    var node = new ListNode(list[0]);
    for (int i = 1; i < list.Count; i++)
    {
        node.Add(list[i]);
    }
    return node;
}

static List<int> ToList(ListNode? head)
{
    if (head == null)
    {
        return new List<int>();
    }
    var current = head;
    var nodes = new List<int>();

    while (current != null)
    {
        nodes.Add(current.Value);
        current = current.Next;
    }
    return nodes;
}
class ListNodeUtil
{
    public static ListNode? RemoveNthNode(ListNode? head, int n)
    {
        if (head == null)
            return head;

        ListNode dummy = new ListNode(-1);
        dummy.Next = head;

        ListNode slow = dummy;
        ListNode fast = dummy;

        for (int i = 0; i <= n; i++)
        {
            if (fast == null)
                return head;
                
            fast = fast.Next;
        }

        while (fast != null)
        {
            slow = slow.Next;
            fast = fast.Next;
        }

        slow.Next = slow.Next.Next;

        return dummy.Next;
    
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
        ListNode newNode = new ListNode(value);

        ListNode current = this;
        while (current.Next != null)
        {
            current = current.Next;
        }
        current.Next = newNode;
        return this;
    }

    public string Tostring()
    {

        List<int> nodes = ToList();
        return $"[{string.Join(",", nodes)}]";
    }

    public List<int> ToList()
    {
        List<int> nodes = new List<int>();
        ListNode? current = this;
        while (current != null)
        {
            nodes.Add(current.Value);
            current = current.Next;
        }
        return nodes;
    }
}
