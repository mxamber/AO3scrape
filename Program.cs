using System;
using System.Text.RegularExpressions;

using AO3scrape;

namespace AO3scrape
{
	class Program
	{
		public static int getWorkNumbers(String tag, int min, int max) {
			Regex work_regex = new Regex(@"([\d]+)\sWorks\sin");
			Regex error_regex = new Regex("div[^\\>]*class=\"[\\w\\s]*errors");
			
			int works = 0;												// number of works: default 0 until a number can be found
			
			String url = UrlGenerator.worksUrl(min, max, tag);			// generate search results URL
			String raw = Scraper.scrape(url);							// scrape search results page 
			
			if(String.IsNullOrEmpty(raw) != true) {
				Int32.TryParse(work_regex.Match(raw).Groups[1].ToString().Trim(), out works);
				// if the response is not empty, regex for number and attempt to parse as int
				// if successful: parsed number will be stored in var works, replacing default 0
				// if unsuccessful, default 0 will remain				
			}
			
			// either way, return number of works
			// successful scraping will return number
			// unsuccessful scraping will return 0
			return works;
		}
		
		
		public static void Main(string[] args)
		{
			
			
			Console.Write("Enter tag: ");
			String tag = Console.ReadLine();
			
		min:
			Console.Write("Enter min words: ");
			int min = 0;
			String min_t = Console.ReadLine();			// temporary value
			if(Int32.TryParse(min_t, out min) == false) {
				if(min_t.Trim() == "exit") {					// if the NaN value entered is "exit", shut down
					return;
				}
				Console.WriteLine("ERROR: invalid number! Please try again.");
				goto min;
			}
			
		max:
			Console.Write("Enter max words: ");
			int max = 0;
			String max_t = Console.ReadLine();			// temporary value
			if(Int32.TryParse(max_t, out max) == false) {
				if(max_t.Trim() == "exit") {					// if the NaN value entered is "exit", shut down
					return;
				}
				Console.WriteLine("ERROR: invalid number! Please try again.");
				goto max;
			}
			if(max > 500000) {
				max = 500000;
			}
			if(max < min) {			// min is actually max
				int temp = max;		// max and temp are both actually min
				max = min;			// max is now truly max
				min = temp;			// min gets new value from temp, the former max
			}
			
			
			
			
			
			
			

			
			
			Console.WriteLine("There are " + getWorkNumbers(tag, min, max) + " works tagged '" + tag + "' between " + min + " and " + max + " words.");
			Console.WriteLine("Press enter to terminate...");
			Console.ReadLine();
		}
	}
}