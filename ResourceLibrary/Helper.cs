namespace ResourceLibrary
{
	public static class Helper
	{
		public static string Encode(this string xmlValue)
		{
			return xmlValue.Replace("Ã¶", "ö").Replace("Ã¤", "ä").Replace("Ã¥", "å").Replace("Ã©", "é");
		}
		
		public static int ConvertMonth(string month)
		{
			int retVal;
			switch (month)
			{
				case "Jan":
					retVal = 1;
					break;
				case "Feb":
					retVal = 2;
					break;
				case "Mar":
					retVal = 3;
					break;
				case "Apr":
					retVal = 4;
					break;
				case "May":
					retVal = 5;
					break;
				case "Jun":
					retVal = 6;
					break;
				case "Jul":
					retVal = 7;
					break;
				case "Aug":
					retVal = 8;
					break;
				case "Sep":
					retVal = 9;
					break;
				case "Oct":
					retVal = 10;
					break;
				case "Nov":
					retVal = 11;
					break;
				case "Dec":
					retVal = 12;
					break;
				default:
					retVal = 0;
					break;
			}
			return retVal;
		}
	}
}