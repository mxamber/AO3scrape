using System;

namespace AO3scrape
{
	public class UrlGenerator
	{
		const String strbase = "https://archiveofourown.org/works?work_search[sort_column]=revised_at";
		const String str1 = "&work_search[words_from]=";
		const String str2 = "&work_search[words_to]=";
		const String str3 = "&tag_id=";
		
		public UrlGenerator() {
			// empty			
		}
		
		public static String worksUrl(String tag_name, String custom = "") {
			return strbase + str3 + tag_name + custom;
		}
		
		public static String worksUrlMin(int min_words, String tag_name, String custom = "") {
			return strbase + str1 + min_words + str3 + tag_name + custom;
		}
		
		public static String worksUrlMax(int max_words, String tag_name, String custom = "") {
			return strbase + str2 + max_words + str3 + tag_name + custom;
		}
		
		public static String worksUrlMinMax(int min_words, int max_words, String tag_name, String custom = "") {
			return strbase + str1 + min_words + str2 + max_words + str3 + tag_name + custom;
		}
	}
}
