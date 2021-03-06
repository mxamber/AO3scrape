# AO3scrape

Command-line tool for scraping and exporting fandom numbers from AO3.

## Usage

Run via the console. Open *cmd.exe*, navigate to the folder you saved *AO3scrape.exe* in, and run the file with any desired flags. Example: `C:\Users\example\Desktop>AO3scrape.exe -tag "Harry Potter - J. K. Rowling" -min 5000 -max 25000 -search "&work_search[query]=-Dramione"`

**-h, -help**
Displays the help text, overruling any other flags.

**-work [int]**
Requests stats for a specific work, with the ID passed as argument. Overrides any flag except `-help`.

**-tag [string]**
Sets the tag to search. Required, omitting this flag will fail the process and shut down the software.

**-min [int]**
Sets the minimum word count filter. Negative values equal not setting the flag.

**-max [int]**
Sets the maximum word count filter. Negative values equal not setting the flag. Large numbers will be capped at 5,000,000.

**-search [string]**
Allows adding custom search parameters to the search results URL used internally, such as *&work_search[query]=-"High School AU"*

**-simple**
Replaces the usual "there are x works" output with a numbers-only version listing minimum and maximum word count and number of fics, separated by tabulators and fit for import into spreadsheet software.

**-range [int,int,int,...]**
Range of word counts to be queried. Ex: 0,500,1000,5000, result: 0-500, 500-1000, 1000-5000.

**-export [path]**
Export results to specified text file instead of printing to console. Will overwrite without warning.

## Notes

Following the removal of all reusable code in order to separate the scraping code and the use case specific code to be placed here, [mxamber/ScrapingFromOurOwn](https://github.com/mxamber/ScrapingFromOurOwn) is now a requirement. DLL included with releases.

## Installation

### Compile it yourself

Download the source code. Set up a C# console application project in your IDE of choice. Either download a compiled version of SFO2 or the current source code from the Github repo (recommended). If you download the DLL, add it to your project's assembly references. If you download the source, place it in a second project in your first porject's solution and add said second project to your AO3scrape project's references. Compile.

### Download

Download AO3scrape.exe and SFO2.dll from the releases page. You can also download a current SFO2 release from the Github repo, but I can't guarantee it'll be compatible with AO3scrape, since I might update SFO2 without updating AO3scrape. Place both files in the same directory and run AO3scrape (no installation required).
