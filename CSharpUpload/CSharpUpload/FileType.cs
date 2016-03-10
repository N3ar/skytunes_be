using System;


namespace CSharpUpload
{

	/// <summary>
	/// Little data structure to hold information about file types. 
	/// Holds information about binary header at the start of the file
	/// </summary>
	public class FileType
	{
		internal byte?[] Header { get; set; } // most of the times we only need first 8 bytes, but sometimes extend for 16
		internal int HeaderOffest { get; set; }
		internal string FileFormat { get; set; }
		internal string Mime { get; set; }

		public FileType (byte?[] header, string format, string mime)
		{
			Header = header;
			HeaderOffest = 0;
			FileFormat = format;
			Mime = mime;
		}

		public FileType (byte?[] header, int offset, string format, string mime)
		{
			Header = header;
			HeaderOffest = offset;
			FileFormat = format;
			Mime = mime;
		}
	}
}

