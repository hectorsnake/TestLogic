using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestLogic.Models;

namespace TestLogic.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public IActionResult Test1()
		{
			//this method take to much time
			//double result = 0;
			//Debug.WriteLine("Math.Pow(10, 6) {0}", Math.Pow(10, 6));
			//for(double k = 1; k <= Math.Pow(10, 6); k++) {
			//	Debug.WriteLine("{0} : {1}/{2} = {3}", k, Math.Pow(-1, (k + 1)), ((2 * k) - 1),   Math.Pow(-1, (k + 1)) / ((2 * k) - 1));
			//	result += ( Math.Pow(-1, (k+1)) / ((2*k)-1)  );
			//}
			//result = 4  * result;
			//return View(result);

			/*compute PI*/
			int limit = (int)Math.Pow(10, 6);
			double[] array = new double[limit];
			//int[] values = Enumerable.Range(1, limit).ToArray();
			Debug.WriteLine("{0:HH:mm:ss.ffff} array.len", DateTime.Now, array.Length);
			Parallel.For(1, limit, k => {
				array[k] = (Math.Pow(-1, (k + 1)) / ((2 * k) - 1));
			});
			Debug.WriteLine("{0:HH:mm:ss.ffff} before sum", DateTime.Now, array.Length);
			double result = 4 * array.Sum();
			Debug.WriteLine("{0:HH:mm:ss.ffff} after sum", DateTime.Now, array.Length);
			return View(result);
		}

		public IActionResult Test2()
		{
			/*
			 * Write a function that Print numbers from 1 to 200 and every time a number is divisible by 3,
				print the word "fizz" instead of the number; every time the number is divisible by 5 print the
				number "buzz" and every time the number is divisible by both (3 & 5) print the word "fizzbuzz".
			*/
			Dictionary<int,string> values = new Dictionary<int, string>();
			for(int i = 1; i <= 200; i++) {
				string value = i.ToString();
				if ( (i%3) == 0 && (i%5) == 0) {
					value = "fizzbuzz";
				}
				else if ((i % 3) == 0) {
					value = "fizz";
				}
				else if ((i % 5) == 0) {
					value = "buzz";
				}
				values.Add(i, value);
			}
			return View(values);
		}


		#region Test3
		[HttpGet]
		public IActionResult Test3()
		{
			ConverterModel model = new ConverterModel();
			model.BaseId = 2;
			ViewData["Bases"] = new SelectList(BaseType.List(), "Id", "Code");

			return View(model);
		}

		[HttpPost]
		public IActionResult Test3(ConverterModel pModel)
		{
			try {
				/* Write a function that takes a string containing a number in base X along with and integer of base X. 
				   The function must return the integer value of that string/base pair.
					- E.g., int Convert.ToInt32(string number, int base);
					- Do NOT use .NET Framework’s Convert.ToInt32 or similar.
				 */
				ViewData["Bases"] = new SelectList(BaseType.List(), "Id", "Code");

				switch(pModel.BaseId) 
				{
					case 2: 
						ConverterBinary converterBinary = new ConverterBinary();
						ViewBag.Result = converterBinary.ConvertToInt(pModel.Value);
						break;
					case 8:
						ConverterOctal converterOctal = new ConverterOctal();
						ViewBag.Result = converterOctal.ConvertToInt(pModel.Value);
						break;
					case 16:
						ConverterHexadecimal converterHexadecimal = new ConverterHexadecimal();
						ViewBag.Result = converterHexadecimal.ConvertToInt(pModel.Value);
						break;
				}

				return View(pModel);
			}
			catch (Exception pE) {
				return View("_error", pE);
			}
		}
		#endregion

		#region Test4
		[HttpGet]
		public IActionResult Test4()
		{
			/*just show a view with two textbox*/
			return View();
		}

		[HttpPost]
		public IActionResult Test4(TwoStrings pModel)
		{
			/* Write a function that, given two strings, test whether one is an anagram of the other.
			 */
			string string1 = pModel.String1?.Replace(" ", "").ToLower() ?? String.Empty;
			string string2 = pModel.String2?.Replace(" ", "").ToLower() ?? String.Empty;
			if (isAnagram(string1,string2)) 
				ViewBag.Result = String.Format("'{0}' is an anagram of '{1}'", pModel.String1, pModel.String2);
			else if (isAnagram(string2, string1))
				ViewBag.Result = String.Format("'{0}' is an anagram of '{1}'", pModel.String2, pModel.String1);
			else
				ViewBag.Result = "There is no coincidence";
			return View();
		}

		private bool isAnagram(string pString2, string pString1)
		{
			char[] array2 = pString1.ToCharArray();
			foreach (char c in pString1.ToCharArray()) {
				if (!array2.Contains(c))
					return false;
			}
			return true;
		}
		#endregion

		#region Test5
		[HttpGet]
		public IActionResult Test5()
		{
			/*just show a view with two textbox*/
			return View();
		}

		[HttpPost]
		public IActionResult Test5(CompressModel pModel)
		{
			try {
				/* Write a function to perform basic string compression using the counts of repeated characters;
					e.g. "aabcccccaaa" would become "a2b1c5a3". If the compressed string would not become
					smaller than the original string, just print the original string.
				 */
				string compressed = String.Empty;

				char[] chars = pModel.StringValue.ToCharArray();
				char? last = null;
				int count =0;
				
				for (int i = 0; i < chars.Length; i++) {
					if (last == chars[i]) {
						count++;
					}
					else {
						if (last != null) {
							compressed += last + count.ToString();
						}
						last = chars[i];
						count = 1;
					}
				}

				if (last != null && count > 0) 
					compressed += last + count.ToString();

				if (compressed.Length < pModel.StringValue.Length)
					ViewBag.Result = compressed;
				else
					ViewBag.Result = pModel.StringValue;
				return View();
			}
			catch(Exception pE) {
				return View("_error", pE);
			}
		}
		#endregion

		#region Test6
		[HttpGet]
		public IActionResult Test6()
		{
			/*just show a view with two textbox*/
			return View();
		}

		[HttpPost]
		public IActionResult Test6(ReverseModel pModel)
		{
			/* Write a function that reverses a string in a memory-efficient manner and without using the
				Enumerable.Reverse(IEnumerable<TSource>) extension method.
			 */
			string string1 = pModel.StringValue ?? String.Empty;
			
			char[] array1 = string1.ToCharArray();

			//iterate only the first half
			for(int i=0; i < (array1.Length / 2); i++) {
				//use a temp var to switch values in the same array
				char temp = array1[i];
				array1[i] = array1[array1.Length -1 -i];
				array1[array1.Length -1  -i] = temp;
			}
			char[] best = string1.Reverse().ToArray();
			ViewBag.Result = new string(array1);
			return View();
		}
		#endregion

		#region Test7
		[HttpGet]
		public IActionResult Test7()
		{
			/*just show a view with two textbox*/
			return View(new SwapNumbers());
		}

		[HttpPost]
		public IActionResult Test7(SwapNumbers pModel)
		{
			/*Write a function that swaps two integers without using a temporary variable.*/
			//we add #2 into #1
			pModel.Number1 = pModel.Number1 + pModel.Number2;
			//we substract (#1 + #2) - #2 = #1
			pModel.Number2 = pModel.Number1 - pModel.Number2;
			//we substract (#1 + #2) - #1 = #2
			pModel.Number1 = pModel.Number1 - pModel.Number2;

			ViewBag.Result = "we swapped without temporary variable";
			return View(pModel);
		}
		#endregion

		#region Test8
		public IActionResult Test8()
		{
			try {
				int[,] values = new int[4,5]{ { 1, 2, 3, 4, 5}, {5, 0, 7, 8, 9}, { 11, 22, 33, 44, 55}, { 55, 66, 77, 88, 99}};
				ViewBag.Before = getMatriz(values);

				List<int> rows = new List<int>();
				List<int> columns = new List<int>();
				for (int i = 0; i < values.GetLength(0); i++) {
					for (int j = 0; j < values.GetLength(1); j++) {
						if (values[i,j] == 0) {
							if (!rows.Contains(i))
								rows.Add(i);
							if (!columns.Contains(j))
								columns.Add(j);
						}
					}
				}
				for (int i = 0; i < values.GetLength(0); i++) {
					for (int j = 0; j < values.GetLength(1); j++) {
						if (rows.Contains(i) || columns.Contains(j))
							values[i, j] = 0;
					}
				}

				ViewBag.After = getMatriz(values);
				return View();
			}
			catch (Exception pE) {
				return View("_error", pE);
			}
			
		}

		private string getMatriz(int[,] pValues)
		{
			string temp = "[";
			try {
				for(int i=0; i < pValues.GetLength(0); i++) {
					if (i > 0)
						temp += ",\r\n\t[";
					else
						temp += "\r\n\t[";
					for (int j=0; j < pValues.GetLength(1); j++) {
						if (j > 0)
							temp += ",";

						if (pValues[i, j].ToString().Length == 1)
							temp += " ";

						temp += pValues[i, j].ToString();
					}
					temp += "]";
				}
				temp += "\r\n]";
			}
			catch{ }
			return temp;
		}
		#endregion
	}
}
