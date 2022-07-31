using System;

using ScrapingFromOurOwn;

namespace AO3scrape
{
	class Program
	{
		const String msg_help = "\n---AO3scrape help---\n\n-h, -help\tdisplay this help text\n-work\t\trequests a certain work's stats by its ID\n-min\t\tset minimum word count\n-max\t\tset maximum word count\n-tag\t\tset tag to filter on\n-search\t\tadd your own search parameters\n-simple\t\tonly output result number\n-ranges\t\tOverrides min and max flags. Specify a range of word counts.\n-export\t\tSpecify a file to hold the output.";
		
		static bool queryArg(String arg, string[] args, out String result) {
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
		
		static bool queryArg(String arg, string[] args) {
			String temp;
			return queryArg(arg, args, out temp);
		}
		
		
		static String getTag(String[] args) {
			String tag = "";
			String a_tag;
			if(queryArg("tag", args, out a_tag) == true && String.IsNullOrEmpty(a_tag) == false) {
				tag = a_tag.Trim();
			}
			
			if(String.IsNullOrEmpty(tag)) {
				Console.WriteLine("ERROR: please provide a tag!");
				Console.WriteLine(msg_help);
				Environment.Exit(0);
			}
			return tag;
		}
		
		
		static void pollRanges(int[] ranges, String[] args, String filename = "") {
			if(ranges.Length < 2) {
				Console.WriteLine("ERROR: Please provide at least 2 values!");
				Console.WriteLine(msg_help);
				Environment.Exit(0);
			}
			
			bool export = false;
			if(String.IsNullOrEmpty(filename) == false) {
				export = true;
			}
			
			String tag = getTag(args);
			
			String custom_search = "";
			queryArg("search", args, out custom_search);
			
			
			String output = "";
			FandomQuery query = new FandomQuery(tag);
			query.custom = custom_search;
			
			
			for(int i = 0; i < (ranges.Length - 1); i++) {
				query.minimum = ranges[i];
				query.maximum = ranges[i+1];
				query.BeginQuery();
				output = Exporter.addColumn(output, query.minimum.ToString(), query.maximum.ToString(), query.results.ToString());
			}
			
			
			if(export == true) {
				Exporter.writeFile(filename, output);
			} else {
				Console.WriteLine(output);
			}
			
			Environment.Exit(0);
		}
		
		static int[] getRanges(String raw) {
			String[] ranges_raw = raw.Split(',');
			int[] ranges = {};
			
			for(int i = 0; i < ranges_raw.Length; i++) {
				int current;
				if(Int32.TryParse(ranges_raw[i], out current)) {
					int[] ranges_temp = new int[ranges.Length + 1];
					ranges.CopyTo(ranges_temp,0);
					ranges_temp[ranges_temp.Length - 1] = current;
					ranges = ranges_temp;
				}
			}
			
			return ranges;
		}
		
		
		static void Main(string[] args)
		{
			
			if(queryArg("help", args) || queryArg("h", args)) {
				Console.WriteLine(msg_help);
				return;
			}
			
			String filename = "";
			queryArg("export", args, out filename);
			
			if(queryArg("work", args)) {
				String id_str;
				int id;
				queryArg("work", args, out id_str);
				if(String.IsNullOrEmpty(id_str) == false && Int32.TryParse(id_str, out id)) {
					try {
						Work result = WorkQuery.BeginQuery(id);
						String output = "Work:\t\t" + result.title + " by " + result.author + "\nPublished\t" + result.published.ToLongDateString() + "\nUpdated\t\t" + result.updated.ToLongDateString() + "\nWords\t\t" + result.words + "\nChapters\t" + result.chapters + "\nKudos\t\t" + result.kudos + "\nComments\t" + result.comments + "\nBookmarks\t" + result.bookmarks + "\nHits\t\t" + result.hits + "\nLanguage:\t" + result.language.ToString().Replace("_", " ");
						if(String.IsNullOrEmpty(filename) == false) {
							Exporter.writeFile(filename, output);
						} else {
							Console.WriteLine(output);
						}
					} catch (System.ArgumentException e) {
						Console.WriteLine("Work could not be found! Please enter a valid work ID.");
						Console.WriteLine("Exception: {0}", e.ToString());
					}
					
					return;
				}
			}
			
			
			String range_str;
			if(queryArg("range", args, out range_str) == true && String.IsNullOrEmpty(range_str) == false) {
				int[] ranges = getRanges(range_str);
				
				pollRanges(ranges, args, filename);
			}
			
			
			
			
			String custom_search = "";
			queryArg("search", args, out custom_search);
			
			
			String tag = getTag(args);
			
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
			
			FandomQuery query = new FandomQuery(tag);
			query.minimum = min;
			query.maximum = max;
			query.custom = custom_search;
			query.BeginQuery();
			int works = query.results;
			
			if(min < 0)
				min = 0;
			if(max < 0)
				max = 5000000;
			
			if(queryArg("simple", args) == true) {
				Console.WriteLine("{0}\t\t{1}\t\t{2}", min, max, works);
			} else if(String.IsNullOrEmpty(filename) == false) {
				Exporter.writeFile(filename, Exporter.addColumn(tag, min.ToString(), max.ToString(), works.ToString()));
			} else {
				Console.WriteLine("There are {0} works tagged '{1}' between {2} and {3} words that fit your query.", works, tag, min, max);
			}
		}
	}
}