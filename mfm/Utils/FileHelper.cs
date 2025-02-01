namespace Utils;

public static class FileHelper
{
	public static async Task<string> ReadFileContents(string filePath)
	{
		if (string.IsNullOrEmpty(filePath))
		{
			throw new ArgumentNullException();
		}

		using var reader = new StreamReader(filePath);
		return await reader.ReadToEndAsync();
	}

	//@https://stackoverflow.com/questions/4744890/c-sharp-check-if-file-is-text-based
	public static bool IsBinary(string filePath, int requiredConsecutiveNul = 1)
	{
	    const int charsToCheck = 8000;
	    const char nulChar = '\0';

	    int nulCount = 0;

	    using (var streamReader = new StreamReader(filePath))
	    {
	        for (var i = 0; i < charsToCheck; i++)
	        {
	            if (streamReader.EndOfStream)
	                return false;

	            if ((char) streamReader.Read() == nulChar)
	            {
	                nulCount++;

	                if (nulCount >= requiredConsecutiveNul)
	                    return true;
	            }
	            else
	            {
	                nulCount = 0;
	            }
	        }
	    }

	    return false;
	}
}
