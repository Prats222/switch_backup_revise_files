using System;
using System.Collections.Generic;

public class Program
{
    //Two sum using Dictionary
    public static int[] TwoSum(int[] nums, int target)
    {
        Dictionary<int,int> map = new Dictionary<int, int>();
        for(int i = 0; i < nums.Length; i++)
        {
            int required = target- nums[i]; // checkks target-num = 9-2 = 7 , stored? notin map so goes to 18 line
            //now check 2nd number 7 , no 9-7=2 , so required = 2, it is in map at 0th index , so now we return . check 16th.
            if (map.ContainsKey(required))
            {
                return new int[]{map[required],i}; // now we return index of required element already stored(2)(i=0), and current element index that is 7, its index is 1.
            }
            map[nums[i]]=i; // stores num 2 in map's index that is 0, num[i]=num[0]=2 ,i=0 , so 2 is stores at 0th index.
        }
        return new int[]{};
    }
    public static void Main(string[] args)
    {
        //Reverse digits of a number
        int number = 12345;
        int reversed = 0;
        while (number > 0)
        {
            int lastDigit = number%10; // 12345%10 = 10 divided 12340 , leaves remainder 5.
            reversed = reversed*10+lastDigit; // 0*10+5 = 5 , further - 5*10+4=54, further- 54*10+3=543
            number=number/10; // 12345/10 = 1234 , 1234/10 = 123.
        }
        System.Console.WriteLine(reversed);

        // Armstrong number
        int[] numbers = { 10, 153, 20, 370, 371, 407, 500 };

        foreach(int num in numbers)
        {
            int temp = num;
            int sum=0;
            int length = num.ToString().Length;

            while (temp > 0)
            {
                int digit = temp%10;
                sum+= (int)Math.Pow(digit,length);
                temp=temp/10;
            }
           
            if (sum == num)
            {
                System.Console.Write(num+" ");
            }
        }
        System.Console.WriteLine();

        //Anagram
        string str1 = "abhc";
        string str2 = "hbca";

        char[] ar1 = str1.ToCharArray();
        char[] ar2 = str2.ToCharArray();
        Array.Sort(ar1);
        Array.Sort(ar2);

        String sorted1 = new string(ar1);
        String sorted2 = new string(ar2);

        if (sorted1 == sorted2)
        {
            System.Console.WriteLine("Anagram");
        }
        else
        {
            System.Console.WriteLine("Not anagram");
        }

        //first non-repeating character
        string s = "swiss";
        Dictionary<char,int> freq = new Dictionary<char, int>();

        foreach(char c in s)
        {
            if (freq.ContainsKey(c))
            {
                freq[c]++;
            }
            else
            {
                freq[c]=1;
            }
        }
        foreach(char c in s)
        {
            if (freq[c] == 1)
            {
                System.Console.WriteLine(c);
                break;
            }
        }

        //15,17,18,22
        //Missing number
        int[] missArr = {1,2,3,5,6};
        int n = 6;
        int expectedSum = n*(n+1)/2;

        int sumz=0;
        foreach(int a in missArr)
        {
            sumz+=a;
        }
        int missingNum = expectedSum-sumz;
        System.Console.WriteLine(missingNum);

        //Two Sum
        int[] array = {2,7,11,13};
        int target = 9;
        int[] result = TwoSum(array, target);
        Console.WriteLine($"[{result[0]}, {result[1]}]");
    }
}