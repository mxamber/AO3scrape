using System;

namespace AO3scrape
{
	public class UrlGenerator
	{
		const String str1 = "https://archiveofourown.org/works?work_search[words_from]=";
		const String str2 = "&work_search[words_to]=";
		const String str3 = "&tag_id=";
		
		String return_url;
		
		public UrlGenerator(int min_words, int max_words, String tag_name)
		{
			this.return_url = str1 + min_words + str2 + max_words + str3 + tag_name;
		}
		
		public static String worksUrl(int min_words, int max_words, String tag_name)
		{
			return(str1 + min_words + str2 + max_words + str3 + tag_name);
		}
	}
}
