using System;
using System.Collections.Generic;

using AO3scrape;

namespace AO3scrape
{
	class Program
	{		
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
			const String msg_end = "Press enter to terminate...";
			const String msg_help = "\n---AO3scrape help---\n\n-h, -help\tdisplay this help text\n-work\t\trequests a certain work's stats by its ID\n-min\t\tset minimum word count\n-max\t\tset maximum word count\n-tag\t\tset tag to filter on\n-search\t\tadd your own search parameters\n-simple\t\tonly output result number\n";
			
			if(queryArg("help", args) || queryArg("h", args)) {
				Console.WriteLine(msg_help);
				Console.WriteLine(msg_end);
				return;
			}
			
			if(queryArg("work", args)) {
				String id_str;
				int id;
				queryArg("work", args, out id_str);
				if(String.IsNullOrEmpty(id_str) == false && Int32.TryParse(id_str, out id)) {
					Work result = WorkQuery.beginQuery(id);
					Console.WriteLine("Work: {0} by {1}\nPublished\t{2}\nUpdated\t\t{3}\nWords\t\t{4}\nChapters\t{5}\nKudos\t\t{6}\nComments\t{7}\nBookmarks\t{8}\nHits\t\t{9}", result.title, result.author, result.published.ToLongDateString(), result.updated.ToLongDateString(), result.words, result.chapters, result.kudos, result.comments, result.bookmarks, result.hits);
					
					return;
				}
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
				Console.WriteLine(msg_help);
				Console.WriteLine(msg_end);
				Console.ReadLine();
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
			
			
//			int works = getWorkNumbers(tag, min, max, custom_search);
			
			FandomQuery query = new FandomQuery(tag);
			query.minimum = min;
			query.maximum = max;
			query.custom = custom_search;
			query.beginQuery();
			int works = query.results;
			
			if(min < 0)
				min = 0;
			if(max < 0)
				max = 5000000;
			
			if(queryArg("simple", args) == true) {
				Console.WriteLine("{0}\t\t{1}\t\t{2}", min, max, works);
			} else {
				Console.WriteLine("There are {0} works tagged '{1}' between {2} and {3} words that fit your query.", works, tag, min, max);
				Console.WriteLine(msg_end);
				Console.ReadLine();
			}
		}
	}
}