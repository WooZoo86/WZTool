using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace WZToolLib.Utility.FileCompress
{
    public class ZipHelper
    {
        static ZipHelper()
        {
            ZipHelper.Password = null;
            ZipHelper.Block = 2048;
            ZipHelper.Level = 5;
            ZipHelper.OverWrite = true;
        }

        public static string Password { set; get; }

        public static int Level {
            set
            {
                if (0 == value) Level = 5;
            }
            get
            {
                return Level;
            }
        }

        public static int Block {
            set
            {
                if (0 == value) Block = 2048;
            }
            get
            {
                return Block;
            }
        }

        public static bool OverWrite { set; get; }

        public bool ZipFile(string FileToZip, string ZipedFile = null)
        {
            if (!File.Exists(FileToZip))
            {
                ZipHelper.ErrMsg = "[ZipFile] (" + FileToZip + ") can not be found!";
                return false;
            }

            if (string.IsNullOrEmpty(ZipedFile))
            {
                ZipedFile = Path.GetFileNameWithoutExtension(FileToZip) + ".zip";
            }

            if (0 != string.Compare(Path.GetExtension(ZipedFile), ".zip", StringComparison.OrdinalIgnoreCase))
            {
                ZipedFile = ZipedFile + ".zip";
            }

            bool Result = true;
            try
            {
                using (FileStream ZipFile = File.Create(ZipedFile))
                {
                    using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                    {
                        using (FileStream StreamToZip = new FileStream(FileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                        {
                            string filename = FileToZip.Substring(FileToZip.LastIndexOf('\\') + 1);
                            ZipEntry ZipEntry = new ZipEntry(filename);

                            ZipStream.PutNextEntry(ZipEntry);
                            ZipStream.SetLevel(ZipHelper.Level);
                            ZipStream.Password = ZipHelper.Password;

                            byte[] buffer = new byte[ZipHelper.Block];
                            int sizeRead = 0;
                            try
                            {
                                do
                                {
                                    sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                                    ZipStream.Write(buffer, 0, sizeRead);

                                } while (sizeRead > 0);
                            }
                            catch (Exception ex)
                            {
                                ZipHelper.ErrMsg = "[ZipFile] (" + ZipedFile + ") get exception{" + ex.Message + "}";
                                Result =  false;
                            }
                            finally
                            {
                                ZipStream.Finish();
                                ZipStream.Close();
                                StreamToZip.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ZipHelper.ErrMsg = "[ZipFile] (" + ZipedFile + ") get exception {"+ ex.Message +"}";
                Result =  false;
            }

            return Result;
        }

        public static bool ZipDirectory(string DirectoryToZip, string ZipedFile = null)
        {
            if (!System.IO.Directory.Exists(DirectoryToZip))
            {
                ZipHelper.ErrMsg = "[ZipDirectory] (" + DirectoryToZip + ") can not be found!";
                return false;
            }

            if (string.IsNullOrEmpty(ZipedFile))
            {
                ZipedFile = Path.GetFileNameWithoutExtension(DirectoryToZip) + ".zip";
            }

            if (0 != string.Compare(Path.GetExtension(ZipedFile), ".zip", StringComparison.OrdinalIgnoreCase))
            {
                ZipedFile = ZipedFile + ".zip";
            }

            //string ZipFileName = string.IsNullOrEmpty(ZipedFileName) ? ZipedPath + "\\" + new DirectoryInfo(DirectoryToZip).Name + ".zip" : ZipedPath + "\\" + ZipedFileName + ".zip";
            bool Result = true;
            try
            {
                using (System.IO.FileStream ZipFile = System.IO.File.Create(ZipedFile))
                {
                    using (ZipOutputStream ZipOut = new ZipOutputStream(ZipFile))
                    {
                        ZipOut.Password = ZipHelper.Password;
                        ZipSubDir(DirectoryToZip, ZipOut, "");
                    }
                }
            }
            catch (Exception ex)
            {
                ZipHelper.ErrMsg = "[ZipDirectory] (" + DirectoryToZip + ") get exception {" + ex.Message + "}";
                Result = false;
            }

            return Result;
        }

        private static void ZipSubDir(string strDirectory, ZipOutputStream ZipOut, string parentPath)
        {
            if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
            {
                strDirectory += Path.DirectorySeparatorChar;
            }

            Crc32 crc = new Crc32();

            string[] filenames = Directory.GetFileSystemEntries(strDirectory);
            foreach (string file in filenames)
            {
                if (Directory.Exists(file))
                {
                    string subPath = parentPath;
                    subPath += file.Substring(file.LastIndexOf("\\") + 1);
                    subPath += "\\";
                    ZipSubDir(file, ZipOut, subPath);
                }
                else 
                {
                    try
                    {
                        using (FileStream fs = File.OpenRead(file))
                        {
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);

                            string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);
                            ZipEntry entry = new ZipEntry(fileName);

                            entry.DateTime = DateTime.Now;
                            entry.Size = fs.Length;

                            fs.Close();

                            crc.Reset();
                            crc.Update(buffer);

                            entry.Crc = crc.Value;
                            ZipOut.PutNextEntry(entry);

                            ZipOut.Write(buffer, 0, buffer.Length);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public static bool UnZip(string ZipFile, string TargetDir = null)
        {
            if (!File.Exists(ZipFile))
            {
                ZipHelper.ErrMsg = "[UnZip] (" + ZipFile + ") can not be found!";
                return false;
            }
            
            string RootDir = string.Empty;
            if (string.IsNullOrEmpty(TargetDir))
            {
                RootDir = "\\";
            }
            else
            {
                RootDir = TargetDir.EndsWith("\\") ? TargetDir : TargetDir + "\\";
                bool flag = Directory.Exists(RootDir) || Directory.CreateDirectory(RootDir) != null;
                if(!flag)
                {
                    ZipHelper.ErrMsg = "[UnZip] (" + RootDir + ") can not be found or created!";
                    return false;
                }
            }

            bool Result = true;
            try
            {
                using (ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(ZipFile)))
                {
                    ZipHelper.FilesName.Clear();

                    zipInputStream.Password = ZipHelper.Password;
                    ZipEntry nextEntry;
                    while ((nextEntry = zipInputStream.GetNextEntry()) != null)
                    {
                        string path = RootDir + nextEntry.Name;
                        string fileName = Path.GetFileName(path);
                        string directoryName = Path.GetDirectoryName(path);
                        if (!string.IsNullOrEmpty(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            if ((File.Exists(path) && ZipHelper.OverWrite) || (!File.Exists(path)))
                            {
                                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                                {
                                    byte[] buffer = new byte[ZipHelper.Block];
                                    int count = 0;

                                    try
                                    {
                                        while ((count = zipInputStream.Read(buffer, 0, ZipHelper.Block)) > 0)
                                        {
                                            fileStream.Write(buffer, 0, count);
                                        }

                                        ZipHelper.FilesName.Add(fileName);
                                    }
                                    catch (Exception ex)
                                    {
                                        ZipHelper.ErrMsg = "[UnZip] (" + ZipFile + ") get exception {" + ex.Message + "}";
                                        Result = false;
                                    }
                                    finally
                                    {
                                        fileStream.Flush();
                                        fileStream.Close();
                                        zipInputStream.Close();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ZipHelper.ErrMsg = "[UnZip] (" + ZipFile + ") get exception {" + ex.Message + "}";
                Result = false;
            }

            return Result;
        }


        public static List<string> FilesName = new List<string>();
        public static string ErrMsg = "No Error";
    }
}
