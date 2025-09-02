// See https://aka.ms/new-console-template for more information


using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

/**
Problem:
Given a reference of type ListNode which is the head of a singly linked list, write a function to determine if the linked list is a palindrome.

Approach:
- Use fast and slow pointer to find the midpoint of the list
- Reverse the second half of the list
- Traverse the list from both ends using two pointers, comparing the value, and return false if they differ
- Return true when the two pointer meet
**/

ListNode node1 = new ListNode(5).Add(4).Add(3).Add(4).Add(5);
var result1 = LinkedListUtil.IsPalindrome(node1);

ListNode node2 = new ListNode(5).Add(4).Add(3).Add(4);
var result2 = LinkedListUtil.IsPalindrome(node2);

ListNode node3 = new ListNode(4);
var result3 = LinkedListUtil.IsPalindrome(node3);

var result4 = LinkedListUtil.IsPalindrome(null);

Debug.Assert(result1 == true, "Test 1: should return true");
Debug.Assert(result2 == false, "Test 2: should return false");
Debug.Assert(result3 == true, "Test 3: should return true");
Debug.Assert(result4 == true, "Test 4: should return true");

Console.WriteLine("All tests have passed!");

class LinkedListUtil
{
    public static bool IsPalindrome(ListNode? head)
    {
        if (head == null || head.Next == null)
        {
            return true;
        }

        ListNode? slow = head;
        ListNode? fast = head;

        while (fast != null && fast.Next != null)
        {
            slow = slow!.Next;
            fast = fast.Next.Next;
        }

        // reverse second half

        ListNode? prev = null;
        ListNode? current = slow;

        while (current != null)
        {
            ListNode? next = current.Next;
            current.Next = prev;
            prev = current;
            current = next;
        }

        ListNode tail = prev!;
        current = head;

        while (tail != null)
        {
            if (current.Value != tail.Value)
            {
                return false;
            }
            tail = tail.Next;
            current = current.Next;
        }

        return true;
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
        ListNode node = new ListNode(value);
        Add(node);
        return this;
    }

    public void Add(ListNode node)
    {

        ListNode current = this;
        while (current.Next != null)
        {
            current = current.Next;
        }

        current.Next = node;
    }
}
