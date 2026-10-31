using System;
using System.Collections.Generic;

    public class Program
{
    public static string rev(string a)
    {
        char[] allChars = a.ToCharArray();
        Array.Reverse(allChars);
        return new string(allChars);
    }
    //fibonachi
    public static int fibonachi(int n){
        if(n < 2){
            return n;
        }else{
            return fibonachi(n-1)+fibonachi(n-2);
        }
    }
    public static void Main(string[] args)
    {
        //Check if string is palindrome
        string word = "MoM";
        if (word == rev(word))
        {
            System.Console.WriteLine("Palindrome");
        }
        else
        {
            System.Console.WriteLine("Not Palindrome");

        }
        // Count frequency of a character
        Dictionary<char, int> freq = new Dictionary<char, int>();
        foreach (char c in word)
        {
            if (freq.ContainsKey(c))
            {
                freq[c]++;
            }
            else
            {
                freq[c] = 1;
            }
        }
        foreach (KeyValuePair<char, int> m in freq)
        {
            System.Console.WriteLine($"{m.Key} , {m.Value}");
        }

        //Reverse words in a sentence
        string? sentence = "My name is babuva jodd";
        string[] words = sentence.Split(" ");
        List<string> revWords = new List<string>();
        foreach(string w in words){
            revWords.Add(rev(w));
        }
        string newSentence = string.Join(" ",revWords);
        System.Console.WriteLine(newSentence);

        // Second largest number in an array
        int[] arr = { 10, 20, 30, 40, 50 };
        int largest = arr[0];
        int secondLargest = arr[1];
        for(int i=0;i<arr.Length;i++){
            if(arr[i]>largest){
                secondLargest=largest;
                largest=arr[i];
            }
        }
        System.Console.WriteLine(secondLargest);
        System.Console.WriteLine(largest);

        // Frequency of a word in a sentence
        string sentencez = "Hi I am am am a Hi boy Prats Prats";
        string[] wordzu = sentencez.Split(" ");
        Dictionary<string,int> frequencyy = new Dictionary<string, int>();

        foreach(string w in wordzu){
            if(frequencyy.ContainsKey(w)){
                frequencyy[w]++;
            }
            else
            {
                frequencyy[w]=1;
            }
        }
        foreach(KeyValuePair<string,int> asd in frequencyy){
            System.Console.Write($"{asd.Key} - {asd.Value}  ");
        }

       //Move zeroes to end
       //Approach - take pointer J , check in loop what is not equal to 0, if not equal to zero copy the non-zero value to zero
       //and incremenet the pointer only when its non-zero, so each value is copied to zero values, and pointer reaches to the last copied value
       //Now the remaining values after the J pointer are values which are already copies , so we need to just replace them with 0.
        int[] arrayy = {7,0,0,1,2,3};
        int j=0;
        for(int i=0;i<arrayy.Length;i++){
            if(arrayy[i]!=0){
                arrayy[j]=arrayy[i];
                j++;
            }
        }
        for(int i = j; i < arrayy.Length; i++)
        {
            arrayy[i]=0;
        }
        foreach(int i in arrayy){
            System.Console.Write(i+" ");
        }
        System.Console.WriteLine();

        // Return common unique elements from 2 arrays
        int[] arr1 = { 2, 2, 3, 4, 5, 6 };
        int[] arr2 = { 3, 3, 4, 6, 8, 0 };

        HashSet<int> set1 = new HashSet<int>();

        foreach(int ib in arr1)
        {
            set1.Add(ib);
        }
        HashSet<int> uniquee = new HashSet<int>();
        foreach(int i in arr2){
            if(set1.Contains(i)){
                uniquee.Add(i);
            }
        }
        System.Console.Write("Unique Elements - ");
        foreach (int t in uniquee)
        {
            System.Console.Write(t + " ");
        }
        System.Console.WriteLine();

        //Matrix Diagonal Sum
        int[,] matrix =
        {
            {1,2,3},
            {5,2,1},
            {9,8,7}
        };
        int res = 0;
        int n = matrix.GetLength(0);
        for(int i=0;i<n;i++){
            res+=matrix[i,i];
        }
        System.Console.WriteLine(res);

        //Prime number
        int[] nums = {13,17,4,6,9}; 
        foreach(int nu in nums){

            if(nu<2){
                continue;
            }
            bool isPrime=true;
            for(int i=2;i<nu;i++){
                if(nu%i==0){
                    isPrime=false;
                    break;
                }
            }
            if(isPrime){
                System.Console.Write(nu+" ");
            }
        }

        //Factorial code
        int fac = 5;
        int final = 1;
        for(int i=2;i<=fac;i++){
            final*=i;
        }
        System.Console.WriteLine(final);

        //Fibonachi series
        System.Console.WriteLine(fibonachi(7));
    }   
}
