using System;
using System.Text.RegularExpressions;

using AO3scrape;

namespace AO3scrape
{
	class Program
	{
		public static void Main(string[] args)
		{
			
			Regex work_regex = new Regex(@"([\d]+)\sWorks\sin");
			
			Regex error_regex = new Regex("div[^\\>]*class=\"[\\w\\s]*errors");
			
			
			Console.Write("Enter tag: ");
			String tag = Console.ReadLine();
			
		min:
			Console.Write("Enter min words: ");
			int min = 0;
			if(Int32.TryParse(Console.ReadLine(), out min) == false) {
				Console.WriteLine("ERROR: invalid number! Please try again.");
				goto min;
			}
			
		max:
			Console.Write("Enter max words: ");
			int max = 0;
			if(Int32.TryParse(Console.ReadLine(), out max) == false) {
				Console.WriteLine("ERROR: invalid number! Please try again.");
				goto max;
			}
			
			String url = UrlGenerator.worksUrl(min, max, tag);
			
			String raw = Scraper.scrape(url);
			
			if(raw == null) {
				Console.ReadLine();
				return;
			}
			
			int works = 0;
			if(Int32.TryParse(work_regex.Match(raw).Groups[1].ToString().Trim(), out works) == false) {
				Console.WriteLine("ERROR! Could not parse response!");
				Console.ReadLine();
				return;
			}
			
			Console.WriteLine("There are " + works + " works tagged '" + tag + "' between " + min + " and " + max + " words.");
			
			Console.ReadLine();
		}
	}
}