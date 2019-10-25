using System;

namespace AO3scrape
{
	public class Exporter
	{
		public Exporter()
		{
			
		}
		
		public static String addColumn(String input, String label, String value) {
			input += Environment.NewLine;
			input += label;
			input += "\t\t";
			input += value;
			
			return input;
		}
	}
}
