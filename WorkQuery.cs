using System;
using System.Text.RegularExpressions;
using AO3scrape;

namespace AO3scrape
{
	public class WorkQuery
	{
		public WorkQuery()
		{
			
		}
		
		public static Regex regexNumericField(String name) {
			return new Regex("<dd class=\"" + name + "\">(?'" + name + "'\\d*)");
		}
		
		public static String matchProperty(Match match, String field) {
			return match.Groups[field].Success == true ? match.Groups[field].ToString().Trim() : "";
		}
		
		public static Work beginQuery(int id) {
			String url = UrlGenerator.workUrl(id);
			String raw = Scraper.scrape(url);
			
			
			if(String.IsNullOrEmpty(raw) == true) {
				throw new System.ArgumentException("Work could not be found!", id.ToString());
			}
			
			Work result = new Work();
			
			
			Regex title_regex = new Regex("<h2 class=\"[^\"]*title[^\"]*\">(?'title'[^\\<]*)");
			Regex author_regex = new Regex("<a[^>]*rel=\"author\"[^>]*>(?'author'[^<]*)");
			Regex publish_regex = new Regex("<dd class=\"published\">(?'publish'\\d\\d\\d\\d-\\d\\d-\\d\\d)");
			Regex update_regex = new Regex("<dd class=\"status\">(?'update'\\d\\d\\d\\d-\\d\\d-\\d\\d)");
			Regex bookmarks_regex = new Regex("<dd class=\"bookmarks\"><a[^>]*>(?'bookmarks'\\d*)");
			
			String title = "";
			String author = "";
			int chapters = 1;
			int words = 0;
			int comments = 0;
			int kudos = 0;
			int bookmarks = 0;
			int hits = 0;
			
			Match title_match = title_regex.Match(raw);
			Match author_match = author_regex.Match(raw);
			
			if(title_match.Groups["title"].Success == true) {
				title = title_match.Groups["title"].ToString().Trim();
			}
			if(author_match.Groups["author"].Success == true) {
				author = author_match.Groups["author"].ToString().Trim();
			}
			
			Match chapters_match = regexNumericField("chapters").Match(raw);
			Int32.TryParse(matchProperty(chapters_match, "chapters"), out chapters);
			
			Match words_match = regexNumericField("words").Match(raw);
			Int32.TryParse(matchProperty(words_match, "words"), out words);
			
			Match comments_match = regexNumericField("comments").Match(raw);
			Int32.TryParse(matchProperty(comments_match, "comments"), out comments);
			
			Match kudos_match = regexNumericField("kudos").Match(raw);
			Int32.TryParse(matchProperty(kudos_match, "kudos"), out kudos);
			
			Match bookmarks_match = bookmarks_regex.Match(raw);
			Int32.TryParse(matchProperty(bookmarks_match, "bookmarks"), out bookmarks);
			
			Match hits_match = regexNumericField("hits").Match(raw);
			Int32.TryParse(matchProperty(hits_match, "hits"), out hits);
			
			
			result.title = title;
			result.author = author;
			result.chapters = chapters;
			result.words = words;
			result.comments = comments;
			result.kudos = kudos;
			result.bookmarks = bookmarks;
			result.hits = hits;
			
			
			return result;
		}
	}
}
