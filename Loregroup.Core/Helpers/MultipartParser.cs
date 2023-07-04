using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// MultipartParser http://multipartparser.codeplex.com
/// Reads a multipart form data stream and returns the filename, content type and contents as a stream.
/// 2009 Anthony Super http://antscode.blogspot.com
/// </summary>
namespace Loregroup.Core.Helpers
{
    public class MultipartParser
    {
        public MultipartParser(Stream stream)
        {
            FileInfos = new List<FilesInformation>();
            this.Parse(stream, Encoding.UTF8);
        }

        public MultipartParser(Stream stream, Encoding encoding)
        {
            FileInfos = new List<FilesInformation>();
            this.Parse(stream, encoding);
        }

        public MultipartParser(Stream stream, Type extraDataType) {
            FileInfos = new List<FilesInformation>();
            this.Parse(stream, Encoding.UTF8, extraDataType);
        }

        private void Parse(Stream stream, Encoding encoding, Type extraDataType = null)
        {
            this.Success = false;

            // Read the stream into a byte array
            byte[] data = ToByteArray(stream);
            int startIndex = 0;
            int endIndex = 0;
            // Copy to a string for header parsing
            string content = encoding.GetString(data);
            //File.WriteAllText(
            //    @"D:\postrequest\" + Guid.NewGuid().ToString() + ".txt",
            //    content);
            // The first line should contain the delimiter
            int delimiterEndIndex = content.IndexOf("\r\n");

            if (delimiterEndIndex > -1)
            {
                string delimiter = content.Substring(0, content.IndexOf("\r\n"));
                int i = 1;
                string[] filesData = content.Split(new string[] { delimiter }, StringSplitOptions.None);
                var obj = Activator.CreateInstance(extraDataType);
                foreach (var item in filesData) {
                    if (i < filesData.Length) {
                        string con = delimiter + item;
                        // Look for Content-Type
                        Regex re = new Regex(@"(?<=Content\-Type:)(.*?)(?=\r\n\r\n)");
                        Match contentTypeMatch = re.Match(con);

                        // Look for filename
                        re = new Regex(@"(?<=filename\=\"")(.*?)(?=\"")");
                        Match filenameMatch = re.Match(con);

                        if (extraDataType != null) {
                            
                            foreach (var prop in obj.GetType().GetProperties()) {
                                Regex propRegex = new Regex(@"(?<=name\=\"")(.*?)(?=\"")");
                                Match propMatch = propRegex.Match(con);
                                if (propMatch.Success && propMatch.Value.Trim().ToLower() == prop.Name.Trim().ToLower())
                                {
                                    prop.SetValue(obj, Convert.ChangeType(con.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None)[1].Replace("\r\n", ""), prop.PropertyType), null);
                                    this.Success = true;
                                }
                            }
                            
                        }
                        
                        // Did we find the required values?
                        if (contentTypeMatch.Success && filenameMatch.Success) {
                            FilesInformation fi = new FilesInformation();
                            // Set properties
                            fi.ContentType = contentTypeMatch.Value.Trim();
                            fi.Filename = filenameMatch.Value.Trim();

                            // Get the start & end indexes of the file contents
                            startIndex = endIndex + contentTypeMatch.Index + contentTypeMatch.Length + "\r\n\r\n".Length;

                            byte[] delimiterBytes = encoding.GetBytes(delimiter);
                            endIndex = IndexOf(data, delimiterBytes, startIndex);

                            int contentLength = endIndex - startIndex;

                            // Extract the file contents from the byte array
                            byte[] fileData = new byte[contentLength];
                            try {
                                Buffer.BlockCopy(data, startIndex, fileData, 0, contentLength);
                            } catch (Exception) {

                            }
                            
                            fi.FileContents = fileData;
                            this.Success = true;
                            FileInfos.Add(fi);
                        }
                        i++;
                    }
                    ExtraData = obj;
                }
            }
        }

        private int IndexOf(byte[] searchWithin, byte[] serachFor, int startIndex)
        {
            int index = 0;
            int startPos = Array.IndexOf(searchWithin, serachFor[0], startIndex);

            if (startPos != -1)
            {
                while ((startPos + index) < searchWithin.Length)
                {
                    if (searchWithin[startPos + index] == serachFor[index])
                    {
                        index++;
                        if (index == serachFor.Length)
                        {
                            return startPos;
                        }
                    }
                    else
                    {
                        startPos = Array.IndexOf<byte>(searchWithin, serachFor[0], startPos + index);
                        if (startPos == -1)
                        {
                            return -1;
                        }
                        index = 0;
                    }
                }
            }

            return -1;
        }

        private byte[] ToByteArray(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public bool Success
        {
            get;
            private set;
        }

        public List<FilesInformation> FileInfos { get; set; }

        public object ExtraData { get; set; }
    }

    public class FilesInformation {
        public string ContentType {
            get;
            set;
        }

        public string Filename {
            get;
            set;
        }

        public byte[] FileContents {
            get;
            set;
        }
    }
}