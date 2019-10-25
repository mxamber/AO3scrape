using System;
using System.IO;
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
		
		
		
		public static bool queryArg(String arg, string[] args, out String result) {
			arg = arg.Trim();
			
			for(int i = 0; i < args.Length; i++) {
				args[i] = args[i].TrimStart('-');
				
				if(args[i] == arg) {
					if (i+1 < args.Length && args[i + 1].Trim().StartsWith("-") == false) {
						result = args[i + 1];
					} else {
						result = null;
					}
					return true;
				}
			}
			
			result = null;
			return false;
		}
		
		public static bool queryArg(String arg, string[] args) {
			String temp;
			return queryArg(arg, args, out temp);
		}
		
		
		public static void Main(string[] args)
		{
			String tag;
			String a_tag;
			if(queryArg("tag", args, out a_tag) == true && String.IsNullOrEmpty(a_tag) == false) {
				tag = a_tag.Trim();
			} else {
				Console.Write("Enter tag: ");
				tag = Console.ReadLine();
			}
			
			
			int min = 0;
			String a_min;
			
			if(queryArg("min", args, out a_min) == true && a_min != null) {
				if(Int32.TryParse(a_min, out min) == true) {
					goto nomin;
				}
			}
			
		min:
			// will be skipped via goto if a minimum word count can
			// successfully be parsed from the console arguments
			Console.Write("Enter min words: ");
			String min_t = Console.ReadLine();			// temporary value
			if(Int32.TryParse(min_t, out min) == false) {
				if(min_t.Trim() == "exit") {			// if the NaN value entered is "exit", shut down
					return;
				}
				Console.WriteLine("ERROR: invalid number! Please try again.");
				goto min;
			}
			
		nomin:
			
			
			
			int max = 0;
			String a_max;
			if(queryArg("max", args, out a_max) == true && a_max != null) {
				if(Int32.TryParse(a_max, out max) == true) {
					goto nomax;
				}
			}
		
		max:
			// will be skipped via goto if a maximum word count can
			// successfully be parsed from the console arguments
			Console.Write("Enter max words: ");
			
			String max_t = Console.ReadLine();			// temporary value
			if(Int32.TryParse(max_t, out max) == false) {
				if(max_t.Trim() == "exit") {					// if the NaN value entered is "exit", shut down
					return;
				}
				Console.WriteLine("ERROR: invalid number! Please try again.");
				goto max;
			}
		
		nomax:
			
			if(max > 500000) {
				max = 500000;
			}
			if(max < min) {			// min is actually max
				int temp = max;		// max and temp are both actually min
				max = min;			// max is now truly max
				min = temp;			// min gets new value from temp, the former max
			}
			
			
			
			
			
			
			

			
			int works = getWorkNumbers(tag, min, max);
			Console.WriteLine("There are {0} works tagged '{1}' between {2} and {3} words.", works, tag, min, max);
//			String output = Exporter.addColumn(min.ToString(), works.ToString());
//			Exporter.writeFile(Exporter.exeDirectory() + @"\output.txt", output);
			Console.WriteLine("Press enter to terminate...");
			Console.ReadLine();
		}
	}
}