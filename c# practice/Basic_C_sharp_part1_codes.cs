// C# practice examples: strings, collections, loops, basic methods, and simple counting tasks.
// The calculator at the end reads three stdin lines: first number, operator, and second number.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

	public class Program
	{
		// Utility methods used by the practice examples in Main.
		//reverse a string
		public static string reverseM(string a){
			char[] allChars = a.ToCharArray();
			Array.Reverse(allChars);
			return new string(allChars);
		}
		//Find sum of num from 1 to N
		public static int sumofnum(int n){
			int sum=0;
			for(int i=0;i<=n;i++){
				sum+=i;
			}
			return sum;
		}
		//Largest of 3 numbers
		public static int largestthreenum(int a,int b,int c){
			int largest=a;
			if(a>b && a>c){
				largest=a;
			}else if(b>a && b>c){
				largest=b;
			}else{
				largest=c;
			}
			return largest;
		}
		public static void Main()
		{
			// Demonstrations: splitting and joining text, reversing strings, and using collections.
			// Early Level Practice.
			string sentence = "I am babuva jodd don";
			string[] words = sentence.Split(" ");
			foreach(string w in words){
				Console.Write(w+" ");
			}
			string ns = string.Join(" ",words);
			Console.WriteLine(ns);
			Console.WriteLine("----------------");
			// Reverse a string
			string hi = "Prats";
			string revs = "";
			for(int i=hi.Length-1;i>=0;i--){
				revs+=hi[i];
			}
			Console.WriteLine(revs);

			//2nd way reverse 
			Console.WriteLine(reverseM("Joddd"));

			//--------------------------------------//
			// List //
			List<int> data = new List<int>();
			data.Add(10);
			data.Add(20);
			data.Remove(20);
			foreach(int i in data){
				Console.WriteLine(i+data.Count());
			}
			//Dictionary
			Dictionary<int,string> dict = new Dictionary<int,string>(){
				{24,"Prats"},{25,"Dev"}
			};
			foreach(KeyValuePair<int,string>an in dict){
				Console.WriteLine($"{an.Key},{an.Value}");
			}
			if(dict.ContainsKey(24)){
				Console.WriteLine("7crore");
			}
			//Hashset
			HashSet<int> numbers = new HashSet<int>();
			numbers.Add(10); numbers.Add(20); numbers.Add(30);
			Console.WriteLine(numbers.Count);
			foreach(int num in numbers.ToList()){
				Console.WriteLine(num);

			}

			// Practice questions: even numbers, sums, largest of three, calculator,
			// vowel counting, list frequencies, and duplicate detection.
			//_______________________Pratice Questions______________//
			// Print even numbs bw 1-50
			for(int i=1;i<=50;i++){
				if(i%2==0){
					Console.Write(i+" ");
				}
			}
			Console.WriteLine();
			//SUm of 1 to n
			Console.WriteLine(sumofnum(50));
			//largest of 3 numbs
			Console.WriteLine(largestthreenum(2,4,1));

			//Calculator
			int num1 = int.Parse(Console.ReadLine());
			char opr = char.Parse(Console.ReadLine());
			int num2 = int.Parse(Console.ReadLine());

			switch(opr){
				case '+':
					Console.WriteLine(num1+num2);
					break;
				case '-':
					Console.WriteLine(num1-num2);
					break;
				default:
					Console.WriteLine("Invalid");
					break;
			}

			//Vowels in a string
			string inp = "PratwaJod";
			HashSet<char> vowels = new HashSet<char>{'a','e','i','o','u'};
			int count=0;
			foreach(char c in inp.ToLower()){
				if(vowels.Contains(c)){
					count=count+1;
				}
			}
			Console.WriteLine(count);

			//Freq of a number in the List
			List<int> list  = new List<int>{1,67,1,1,1,3,67};
			Dictionary<int,int> freq = new Dictionary<int,int>();
			foreach(int n in list){
				if(freq.ContainsKey(n)){
					freq[n]++;
				}else{
					freq[n]=1;
				}
			}
			foreach(KeyValuePair<int,int> fre in freq){
				Console.WriteLine($"Num={fre.Key} , frequency= {fre.Value}");
				if(fre.Value==2) Console.WriteLine(fre.Key);
			}
			// Remove Duplicates in a list
			List<int> withDupli = new List<int>{1,1,2,2,67,67,67,3,4,6};
			HashSet<int> unique = new HashSet<int>();
			HashSet<int> Dupli = new HashSet<int>();

			foreach(int i in withDupli){
				if(unique.Contains(i)){
					Dupli.Add(i);
				}else{
					unique.Add(i);
				}
			}
			Console.Write("Unique Elements :");
			foreach(int i in unique.ToList()){
				Console.Write(" "+i+" ");
			}
		}
	}
