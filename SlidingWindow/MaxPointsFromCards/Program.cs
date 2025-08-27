// See https://aka.ms/new-console-template for more information
/**
Problem:
Given an array of integers representing the value of cards, write a function to calculate the maximum score you can achieve by selecting exactly k cards from either the beginning or the end of the array.
For example, if k = 3, then you have the option to select:
the first three cards,
the last three cards,
the first card and the last two cards
the first two cards and the last card
Example 1: Input:
cards = [2,11,4,5,3,9,2]
k = 3
Output: 17

Approach:
- Use sliding window strategy
- Initialize windowSize =  n - k
- Calculate total cards point
- Calculate total points for window starting from first card
- Compute total points = totalpoints - windowPoints
- Traverse through the cards array, sliding the window ro the right and calculatind the total points for the window, 
    update maxPoints = max(maxPoints, currentWindowPoints)
**/
int[] cards = {2,11,4,5,3,9,2};
int k = 3;

Console.WriteLine($"Max Points: {CardGame.FindMaxScore(cards, k)}");
class CardGame
{
    public static int FindMaxScore(int[] cards, int k)
    {
        if (cards == null || cards.Length < k || k <= 0)
        {
            return 0;
        }

        int n = cards.Length;
        int windowSize = n - k;
        int currentWindowPoints = 0;
        int totalpoints = 0;
        int start = 0;

        for (int i = 0; i < n; i++)
        {
            totalpoints += cards[i];
        }

        if (k == n)
        {
            return totalpoints;
        }
        
        for (int i = 0; i < windowSize; i++)
            {
                currentWindowPoints += cards[i];
            }

        int maxPoints = totalpoints -  currentWindowPoints;

        for (int end = windowSize; end < cards.Length; end++)
        {
            currentWindowPoints = currentWindowPoints  +  cards[end] - cards[start];
            maxPoints = Math.Max(maxPoints, totalpoints - currentWindowPoints);
            start++;
        }

        return maxPoints;
    }


}
