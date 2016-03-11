using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.IO;
using System.Text;

using System.Security.Cryptography;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

namespace CSharpUpload
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public partial class WebForm1 : System.Web.UI.Page
	{
		private readonly static List<FileType> VALID_MUSIC_FORMATS = FileSniffer.FileTypes;
		private readonly static int kB = 1024;
		private readonly static int MB = 1000 * kB;
		private readonly static int MAX_FILE_SIZE = 20 * MB;

		private readonly static int MUSIC_PATH_DEPTH = 3;

		protected System.Web.UI.HtmlControls.HtmlInputFile musicFilesToUpload;
		protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			// 
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			// 
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			// Override default MaxRequestLength from 4MB to 20MB
			HttpRuntimeSection section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
			section.MaxRequestLength = MAX_FILE_SIZE;
			Console.WriteLine (section.MaxRequestLength);

			this.Submit1.ServerClick += new System.EventHandler(this.Submit1_ServerClick);
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion


		// Convert the hash byte array into a string representing the directories that will be used 
		// to store the music files. (Returns string with leading with "/" and ending with "/")
		public static string DirectoryPath(byte[] hashArray)
		{
			string str = "/";
			int i;
			int subDirs = 0;
			for (i = 0; i < hashArray.Length; i++)
			{
				str += String.Format("{0:X2}", hashArray[i]);
				if ((i % 2) == 1) 
				{
					str += "/";
					subDirs++;
					if (subDirs >= MUSIC_PATH_DEPTH)
						return str;
				}
			}
			Console.WriteLine (str);
			return str;
		}

		private static string GetSaveLocation(Stream fileStream, string fileName)
		{
			// Initialize a SHA256 hash object.
			SHA256 mySHA256 = SHA256Managed.Create();
			byte[] hashValue;

			// Be sure it's positioned to the beginning of the stream.
			fileStream.Position = 0;
			// Compute the hash of the fileStream.
			hashValue = mySHA256.ComputeHash(fileStream);
			// Write the name of the file to the Console.
			Console.Write(fileName + ": ");
			// Write the hash value to the Console.
			string saveLocation = DirectoryPath(hashValue);
			// Reset the filestream.
			fileStream.Position = 0;

			return saveLocation;
		}

		private void Submit1_ServerClick(object sender, System.EventArgs e)
		{
			
			HttpFileCollection fileCollection = Request.Files;
			for (int i = 0; i < fileCollection.Count; i++)
			{
				HttpPostedFile uploadedFile = fileCollection[i];
				string subDirs = GetSaveLocation (uploadedFile.InputStream, uploadedFile.FileName);
				string SaveLocation = Server.MapPath("Data") + subDirs +  uploadedFile.FileName;

				if( ( uploadedFile != null ) && ( uploadedFile.ContentLength > 0 ) )
				{
//					Console.WriteLine ("File: " + fileCollection[i].FileName);
//					Console.WriteLine ("Size: " + fileCollection[i].ContentLength);
//					Console.WriteLine ("Content Type: " + fileCollection[i].ContentType);
					string fn = System.IO.Path.GetFileName(uploadedFile.FileName);
					bool uploadOk = true;

					// Allow only predefined file formats to be uploaded
					bool valid = uploadedFile.InputStream.isFileOfTypes(VALID_MUSIC_FORMATS);
					if (!valid)
					{
						uploadOk = false;
						Response.Write ("Sorry" + uploadedFile.FileName + ", is not a valid format. Only MP3, MP4, WAV & M4A files are allowed. <br />");
					}

					// Check file size (limit 20MB)
					if (uploadedFile.ContentLength > MAX_FILE_SIZE) 
					{
						uploadOk = false;
						Response.Write ("Sorry " + uploadedFile.FileName + ", is too large. <br />");
					}

					// Check if file already exists
					// TODO: might need to check if it exists only for current user, other people may have same file.
					if (File.Exists (SaveLocation)) 
					{
						uploadOk = false;
						Response.Write ("Sorry " + uploadedFile.FileName + ", already exists. <br />");
					}

					if (uploadOk) // if everything is ok, try to upload file
					{
						try {
							// attempt to create dirs of non existant
							Directory.CreateDirectory (Server.MapPath("Data") + subDirs);

							uploadedFile.SaveAs (SaveLocation);
							mendUserLibrary("usr", SaveLocation);
							Response.Write ("The file " + uploadedFile.FileName + " has been uploaded. <br />");
						} catch (Exception ex) {
							Response.Write ("Error: " + ex.Message);
							//Note: Exception.Message returns a detailed message that describes the current exception. 
							//For security reasons, we do not recommend that you return Exception.Message to end users in 
							//production environments. It would be better to return a generic error message. 
						}
					} else 
					{
						Response.Write ("Sorry " + uploadedFile.FileName + ", was not uploaded. <br />");
					}
				}
				else
				{
					Response.Write("Please select a file to upload.");
				}
			}
		}

		private void mendUserLibrary(string user, string file) {
			TagLib.File mp3file = TagLib.File.Create (file);
			string artist = mp3file.Tag.FirstPerformer;
			string album = mp3file.Tag.Album;
			string track = mp3file.Tag.Title;
//			IPicture cover = mp3file.Tag.Pictures [0].Data.Data;

			string userrepo = Server.MapPath ("Data") + "/" + user;
			if (!Directory.Exists (userrepo)) {
				Directory.CreateDirectory (userrepo);
			}

			string artistdir = userrepo + "/" + artist;
			if (!Directory.Exists (artistdir)) {
				Directory.CreateDirectory (artistdir);
			}

			string albumdir = artistdir + "/" + album;
			if (!Directory.Exists (albumdir)) {
				Directory.CreateDirectory (albumdir);
			}
				
			string newtrackpath = albumdir + "/" + track;
			File.Move (file, newtrackpath);
			mp3file.Dispose ();
		}

		// Assuming the following directory structure: User -> Artists -> Albums -> Tracks
		private String generateJSON(string username) {
			string userrepo = Server.MapPath (username);
			if (!Directory.Exists(userrepo)) {
				Directory.CreateDirectory (userrepo);
			}

			Boolean firstArtist = true;
			Boolean firstAlbum = true;
			Boolean firstTrack = true;

			StringBuilder sb = new StringBuilder ();
			sb.Append ("[");
			foreach (string artist in Directory.GetDirectories(userrepo)) {
				if (!firstArtist) sb.Append (", ");
				sb.Append ("{\"artist\": " + "\"" + artist + "\", ");
				sb.Append ("\"albums\": [");

				foreach (string album in Directory.GetDirectories(artist)) {
					if (!firstAlbum) sb.Append (", ");
					sb.Append ("{\"title\": " + "\"" + album + "\", ");
					sb.Append ("\"cover\": " + "\"" + album + "/cover.png" + "\", ");
					sb.Append ("\"tracks\": [");

					foreach (string track in Directory.GetFiles(album)) {
						if (!firstTrack) sb.Append(", ");
						sb.Append ("{\"title\": " + "\"" + track + "\", ");
						sb.Append ("\"url\": " + "\"" + track + "\"}");
						firstTrack = false;
					}
					firstTrack = true;
					sb.Append ("]}");
					firstAlbum = false;
				}
				firstAlbum = true;
				sb.Append ("]}");
				firstArtist = false;
			}
			firstArtist = true;
			sb.Append ("]");

			return sb.ToString();
		}
	}
}
