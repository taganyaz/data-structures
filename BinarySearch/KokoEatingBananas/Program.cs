// See https://aka.ms/new-console-template for more information


using System.Diagnostics;

/**
Problem:
Koko loves to eat bananas. There are n piles of bananas, the ith pile has piles[i] bananas. The guards have gone and will come back in h hours.
Koko can decide her bananas-per-hour eating speed of k. Each hour, she chooses some pile of bananas and eats k bananas from that pile. If the pile has less than k bananas, she eats all of them instead and will not eat any more bananas during this hour.
Koko likes to eat slowly but still wants to finish eating all the bananas before the guards return.
Return the minimum integer k such that she can eat all the bananas within h hours.

Example 1:
Input: piles = [3,6,7,11], h = 8
Output: 4

Example 2:
Input: piles = [30,11,23,4,20], h = 5
Output: 30

Example 3:
Input: piles = [30,11,23,4,20], h = 6
Output: 23

Approach:
- Use binary search to find max k such that sum of i + k - 1 / k <= h
- Find the highest value in the pile
- Set left = 1 and right = max pile value
- Find mid = left + (right - left) / 2
- Find sum of time when k = mid
- If total time > h then set left = mid + 1
- Else, track current mid as k, and set right = mid to search for a smaller k
- return k
**/
int[] piles1 = [3, 6, 7, 11];
int h1 = 8;
int output1 = 4;
var result1 = KokoBananaSolver.MinEatingSpeed(piles1, h1);

int[] piles2 = [30, 11, 23, 4, 20];
int h2 = 5;
int output2 = 30;
var result2 = KokoBananaSolver.MinEatingSpeed(piles2, h2);

int[] piles3 = [30,11,23,4,20];
int h3 = 6;
int output3 = 23;
var result3 = KokoBananaSolver.MinEatingSpeed(piles3, h3);

Debug.Assert(result1 == output1, "Test 1: Should return correct result");
Debug.Assert(result2 == output2, "Test 2: Should return correct result");
Debug.Assert(result3 == output3, "Test 3: Should return correct result");

Console.WriteLine("All tests have passed!");

class KokoBananaSolver
{
    public static int MinEatingSpeed(int[] piles, int hours)
    {
        if (piles == null)
            throw new ArgumentNullException(nameof(piles));
        if (piles.Length == 0)
            throw new ArgumentException("Piles array cannot be empty");
        if (hours < piles.Length)
            throw new ArgumentException($"Hours {hours} must be greate or equal to {piles.Length}");

        int left = 1;
        int right = piles.Max();

        while (left < right)
        {
            int speed = left + (right - left) / 2;

            if (CanEatAllBananas(piles, hours, speed))
            {
                right = speed;
            }
            else
            {
                left = speed + 1;
            }
        }

        return left;
    }

    private static bool CanEatAllBananas(int[] piles, int hours, int eatingSpeed)
    {
        var eatingTime = 0;
        foreach (var pile in piles)
        {
            eatingTime += Math.DivRem(pile, eatingSpeed, out int remainder);
            if (remainder > 0)
                eatingTime++;

            if (eatingTime > hours)
                return false;
        }
        return true;
    }
}
