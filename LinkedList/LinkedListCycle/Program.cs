// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

/**
Problem:
Write a function that takes in a parameter head of type ListNode that is a reference to the head of a linked list. 
The function should return True if the linked list contains a cycle, and False otherwise, without modifying the linked list in any way.

Approach:
- Use two pointers, slow and fast pointer
- Initialize both pointers to the head, and navigate through the list
- Fast pointer move two steps while slow pointer move 1 step
- If the two pointer overlap, there is a cycle, return true
- Otherwise when the fast pointer reach the end of the linked list, return false

**/
ListNode node1 = new ListNode(5);
node1.Next = new ListNode(4);
node1.Next.Next = new ListNode(3);
node1.Next.Next.Next = new ListNode(2);
node1.Next.Next.Next.Next = new ListNode(0);

ListNode node2 = new ListNode(5);
node1.Next = new ListNode(4);
node1.Next.Next = new ListNode(3);
node1.Next.Next.Next = new ListNode(2);
node1.Next.Next.Next.Next = node2;


ListNode node3 = new ListNode(1);



Debug.Assert(!LinkedListManager.HasCycle(node1), $"Test 1: should not have a cycle. Expected: False. Actual: {LinkedListManager.HasCycle(node1)}");
Debug.Assert(!LinkedListManager.HasCycle(node2), $"Test 2: should have a cycle. Expected: False. Actual: {LinkedListManager.HasCycle(node2)}");
Debug.Assert(!LinkedListManager.HasCycle(node3), $"Test 3: single node should not have a cycle. Expected: False. Actual: {LinkedListManager.HasCycle(node3)}");
Debug.Assert(!LinkedListManager.HasCycle(null), $"Test 4: Empty list should not have a cycle. Expected: False. Actual: {LinkedListManager.HasCycle(null)}");

Console.WriteLine("All tests have passed!");
class LinkedListManager
{
    public static bool HasCycle(ListNode? head)
    {
        if (head == null || head.Next == null)
        {
            return false;
        }

        ListNode? slow = head;
        ListNode? fast = head;

        while (fast != null && fast.Next != null)
        {
            slow = slow?.Next;
            fast = fast.Next.Next;

            if (slow == fast)
            {
                return true;
            }
        }

        return false;
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
}
