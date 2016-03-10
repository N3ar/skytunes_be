using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


using System.Text.RegularExpressions;

namespace CSharpUpload
{
	/// <summary>
	/// Helper class to identify file type by the file header, not file extension.
	/// </summary>
	public static class FileSniffer
	{
		#region Constants

		public readonly static FileType MP3 = new FileType (new byte?[] { 0x49, 0x44, 0x33 }, 0, "mp3", "audio/"); // 3 bytes
		public readonly static FileType M4A = new FileType (new byte?[] { 0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70, 0x4D, 0x34, 0x41, 0x20 }, 0, "m4a", "audio/"); // 12 bytes
//		public readonly static FileType WAV = new FileType (new byte?[] { 0x52, 0x49, 0x46, 0x46, xx, xx, xx, xx, 0x57, 0x41, 0x56, 0x45, 0x66, 0x6D, 0x74, 0x20 }, 0, "wav", "audio/"); // xx bytes

		// all the file types to be put into one list
		public readonly static List<FileType> FileTypes = new List<FileType> { MP3, M4A };

		// number of bytes to read from a file
		private const int MaxHeaderSize = 560;

		#endregion

		#region Main Methods
	
		public static FileType GetFileType(this FileInfo file)
		{
			// read first n-bytes from the file
			Byte[] fileHeader = ReadFileHeader (file, MaxHeaderSize); // some file formats have headers offset to 512 bytes

			string hex = BitConverter.ToString(fileHeader);
			string hexDash = hex.Replace("-","");
			string hexSpace = Regex.Replace(hex.Replace("-",""), ".{4}", "$0 ");

			Console.WriteLine("header: " + hexDash);
			Console.WriteLine("header: " + hexSpace);

			// compare the file header to the stored file headers
			foreach (FileType type in FileTypes)
			{
				int matchingBytes = 0;
				for (int i = 0; i < type.Header.Length; i++)
				{
					// if file offset is not set to zero, we need to take this into account when comparing.
					// if byte in type.header is set to null, means this byte is variable, ignore it
					if (type.Header [i] != null && type.Header [i] != fileHeader [i + type.HeaderOffest])
					{
						// if one of the bytes does not match, move on to the next type
						matchingBytes = 0;
						break;
					}
					else
					{
						matchingBytes++;
					}
				}

				if (matchingBytes == type.Header.Length)
				{
					// if all the bytes match, return the type
					return type;
				}
			}

			// if none of the types match, return null
			return null;
		}

		public static FileType GetFileType(this Stream fileStream)
		{
			// read first n-bytes from the file
			Byte[] fileHeader = ReadFileHeader (fileStream, MaxHeaderSize); // some file formats have headers offset to 512 bytes

			string hex = BitConverter.ToString(fileHeader);
//			string hexDash = hex.Replace("-","");
			string hexSpace = Regex.Replace(hex.Replace("-",""), ".{4}", "$0 ");

//			Console.WriteLine("header: " + hexDash);
			Console.WriteLine("header: " + hexSpace);

			// compare the file header to the stored file headers
			foreach (FileType type in FileTypes)
			{
				int matchingBytes = 0;
				for (int i = 0; i < type.Header.Length; i++)
				{
					// if file offset is not set to zero, we need to take this into account when comparing.
					// if byte in type.header is set to null, means this byte is variable, ignore it
					if (type.Header [i] != null && type.Header [i] != fileHeader [i + type.HeaderOffest])
					{
						// if one of the bytes does not match, move on to the next type
						matchingBytes = 0;
						break;
					}
					else
					{
						matchingBytes++;
					}
				}

				if (matchingBytes == type.Header.Length)
				{
					// if all the bytes match, return the type
					return type;
				}
			}

			// if none of the types match, return null
			return null;
		}

		/// <summary>
		/// Reads the file header - first (16) bytes from the file
		/// </summary>
		/// <param name="file">The file to work with</param>
		/// <returns>Array of bytes</returns>
		private static Byte[] ReadFileHeader(FileInfo file, int MaxHeaderSize) 
		{
			Byte[] header = new byte[MaxHeaderSize];
			try // read file
			{
				using (FileStream fsSource = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
				{
					// read first symbols from file into array of bytes
					fsSource.Read(header, 0, MaxHeaderSize);
					// close the file stream
				}
			}
			catch (Exception e) // file could not be found/read
			{
				throw new ApplicationException ("Could not read file: " + e.Message);
			}

			return header;
		}

		/// <summary>
		/// Reads the file header - first (16) bytes from the file
		/// </summary>
		/// <param name="file">The file to work with</param>
		/// <returns>Array of bytes</returns>
		private static Byte[] ReadFileHeader(Stream fileStream, int MaxHeaderSize) 
		{
			Byte[] header = new byte[MaxHeaderSize];
			try // read file
			{
				fileStream.Read(header, 0, MaxHeaderSize);
			}
			catch (Exception e) // file could not be found/read
			{
				throw new ApplicationException ("Could not read file: " + e.Message);
			}

			return header;
		}

		/// <summary>
		/// Determines whether provided file belongs to one of the provided list of files
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="requiredTypes">The required types.</param>
		/// <returns>
		///   <c>true</c> if file of the one of the provided types; otherwise, <c>false</c>.
		/// </returns>
		public static bool isFileOfTypes(this Stream fileStream, List<FileType> requiredTypes)
		{
			FileType currentType = fileStream.GetFileType();

			if (null == currentType )
			{
				return false;
			}

			return requiredTypes.Contains(currentType);
		}

		#endregion
	}
}

