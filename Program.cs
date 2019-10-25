using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using AO3scrape;

namespace AO3scrape
{
	class Program
	{
		public static int getWorkNumbers(String tag, int min, int max, String custom = "") {
			Regex work_regex = new Regex(@"([\d]+)\sWorks(\sfound|)\sin");
			Regex error_regex = new Regex("div[^\\>]*class=\"[\\w\\s]*errors");
			
			int works = 0;												// number of works: default 0 until a number can be found
			
			String url = "";
			
			if (min <0 && max > -1) {
				url = UrlGenerator.worksUrlMax(max, tag, custom);
			} else if (max < 0 && min > -1) {
				url = UrlGenerator.worksUrlMin(min, tag, custom);
			} else if (max > -1 && min > -1) {
				url = UrlGenerator.worksUrlMinMax(min, max, tag, custom);
			} else {
				url = UrlGenerator.worksUrl(tag, custom);
			}
			
//			url = UrlGenerator.worksUrl(min, max, tag, custom);	// generate search results URL
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
		
		
		public static int[] getRangesNumeric(String range_str) {
			String[] ranges_str = range_str.Split(',');
			List<int> ranges_l = new List<int>();
			
			for(int i = 0; i < ranges_str.Length; i++) {
				int x;
				if(Int32.TryParse(ranges_str[i], out x)) { ranges_l.Add(x); }
			}			
			int[] ranges = ranges_l.ToArray();
			return ranges;
		}
		
		public static void pollRanges(int[] ranges) {
			//			Console.WriteLine("{0} - {1}", 0, ranges[0]);
//			
//			for(int i = 1; i < ranges.Length - 1; i++) {
//				Console.WriteLine("{0} - {1}", ranges[i], ranges[i+1]);
//			}
//			
//			Console.WriteLine("{0} to unlimited", ranges[ranges.Length-1]);
		}
		
		
		public static void Main(string[] args)
		{	
			if(queryArg("help", args) || queryArg("h", args)) {
				Console.WriteLine("\n---AO3scrape help---\n\n-h, -help\tdisplay this help text\n-min\t\tset minimum word count\n-max\t\tset maximum word count\n-tag\t\tset tag to filter on\n-search\t\tadd your own search parameters\n-simple\t\tonly output result number\n");
				Console.WriteLine("Press enter to terminate...");
				Console.ReadLine();
				return;
			}
			
			
			
			String range_str;
			if(queryArg("range", args, out range_str) == true && String.IsNullOrEmpty(range_str) == false) {
				
			}
			
			
			
			
			String custom_search = "";
			queryArg("search", args, out custom_search);
			
			
			String tag = "";
			String a_tag;
			if(queryArg("tag", args, out a_tag) == true && String.IsNullOrEmpty(a_tag) == false) {
				tag = a_tag.Trim();
			} else {
				Console.WriteLine("ERROR: please provide a tag!");
				return;
			}
			
			int min = -1;
			String a_min;
			if(queryArg("min", args, out a_min) == true && a_min != null) {
				Int32.TryParse(a_min, out min);
			}
			
			
			int max = -1;
			String a_max;
			if(queryArg("max", args, out a_max) == true && a_max != null) {
				Int32.TryParse(a_max, out max);
			}
			
			
			
			
			if(max > 5000000) {
				max = 5000000;
			}
			if(max < min) {			// min is actually max
				int temp = max;		// max and temp are both actually min
				max = min;			// max is now truly max
				min = temp;			// min gets new value from temp, the former max
			}
			
			
			int works = getWorkNumbers(tag, min, max, custom_search);
			
			if(min < 0)
				min = 0;
			if(max < 0)
				max = 5000000;
			
			if(queryArg("simple", args) == true) {
				Console.WriteLine("{0}\t\t{1}\t\t{2}", min, max, works);
			} else {
				Console.WriteLine("There are {0} works tagged '{1}' between {2} and {3} words that fit your query.", works, tag, min, max);
				Console.WriteLine("Press enter to terminate...");
				Console.ReadLine();
			}
		}
	}
}