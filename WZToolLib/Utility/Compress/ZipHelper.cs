using System;
using System.Collections.Generic;
using System.IO;


namespace WZToolLib.Utility.FileCompress
{
    using ICSharpCode.SharpZipLib.Zip;
    using ICSharpCode.SharpZipLib.Checksums;

    public class ZipHelper
    {
        static ZipHelper()
        {

        }

        public static string Password { set; get; } = null;

        public static int Level {
            set
            {
                if (0 == value) ZipHelper.CompressionLevel = 5;
            }
            get
            {
                return ZipHelper.CompressionLevel;
            }
        }

        public static int Block {
            set
            {
                if (0 == value) ZipHelper.BlockSize = 2048;
            }
            get
            {
                return ZipHelper.BlockSize;
            }
        }

        public static bool OverWrite { set; get; } = true;

        public static bool ZipFile(string FileToZip, string ZipedFile = null)
        {
            if (!File.Exists(FileToZip))
            {
                ZipHelper.ErrMsg = "[ZipFile] (" + FileToZip + ") can not be found!";
                return false;
            }

            if (string.IsNullOrEmpty(ZipedFile))
            {
                ZipedFile = Path.GetDirectoryName(FileToZip) + "\\" + Path.GetFileNameWithoutExtension(FileToZip) + ".zip";
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

                            ZipStream.SetLevel(ZipHelper.Level);
                            ZipStream.Password = ZipHelper.Password;
                            ZipStream.PutNextEntry(ZipEntry);

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

        public static bool ZipKind(string DirToZip, string ZipedFile = null, string Pattern = "*.*", SearchOption Option = SearchOption.AllDirectories)
        {
            DirToZip = DirToZip.EndsWith("\\") ? DirToZip : DirToZip + "\\";
            if (!System.IO.Directory.Exists(DirToZip))
            {
                ZipHelper.ErrMsg = "[ZipKind] (" + DirToZip + ") can not be found!";
                return false;
            }

            if (string.IsNullOrEmpty(ZipedFile))
            {
                ZipedFile = Path.GetDirectoryName(DirToZip) + ".zip";
            }

            if (0 != string.Compare(Path.GetExtension(ZipedFile), ".zip", StringComparison.OrdinalIgnoreCase))
            {
                ZipedFile = ZipedFile + ".zip";
            }

            bool Result = true;
            try
            {
                using (System.IO.FileStream ZipFile = System.IO.File.Create(ZipedFile))
                {
                    using (ZipOutputStream zipStream = new ZipOutputStream(ZipFile))
                    {
                        zipStream.Password = ZipHelper.Password;
                        zipStream.SetLevel(ZipHelper.Level);
                        
                        foreach (string file in Directory.GetFiles(DirToZip, Pattern, Option))
                        {
                            if (File.Exists(file))
                            {
                                FileInfo item = new FileInfo(file);
                                FileStream fs = File.OpenRead(item.FullName);
                                byte[] buffer = new byte[fs.Length];
                                fs.Read(buffer, 0, buffer.Length);

                                ZipEntry entry = new ZipEntry(item.Name);
                                zipStream.PutNextEntry(entry);
                                zipStream.Write(buffer, 0, buffer.Length);
                            }
                        }

                        zipStream.Finish();
                        zipStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ZipHelper.ErrMsg = "[ZipKind] (" + DirToZip + ") get exception {" + ex.Message + "}";
                Result = false;
            }

            return Result;
        }

        public static bool ZipDir(string DirToZip, string ZipedFile = null)
        {
            DirToZip = DirToZip.EndsWith("\\") ? DirToZip : DirToZip + "\\";
            if (!System.IO.Directory.Exists(DirToZip))
            {
                ZipHelper.ErrMsg = "[ZipDir] (" + DirToZip + ") can not be found!";
                return false;
            }

            if (string.IsNullOrEmpty(ZipedFile))
            {
                ZipedFile = Path.GetDirectoryName(DirToZip) + ".zip";
            }

            if (0 != string.Compare(Path.GetExtension(ZipedFile), ".zip", StringComparison.OrdinalIgnoreCase))
            {
                ZipedFile = ZipedFile + ".zip";
            }

            bool Result = true;
            try
            {
                using (System.IO.FileStream ZipFile = System.IO.File.Create(ZipedFile))
                {
                    using (ZipOutputStream zipStream = new ZipOutputStream(ZipFile))
                    {
                        zipStream.Password = ZipHelper.Password;
                        zipStream.SetLevel(ZipHelper.Level);

                        ZipSubDir(DirToZip, zipStream, DirToZip);
                        
                        zipStream.Finish();
                        zipStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ZipHelper.ErrMsg = "[ZipDirectory] (" + DirToZip + ") get exception {" + ex.Message + "}";
                Result = false;
            }

            return Result;
        }

        private static void ZipSubDir(string strDirectory, ZipOutputStream ZipOut, string parentPath)
        {
            Crc32 crc = new Crc32();
            ZipEntry entry = null;

            string[] filenames = Directory.GetFileSystemEntries(strDirectory);
            foreach (string file in filenames)
            {
                if (Directory.Exists(file))
                {
                    ZipSubDir(file, ZipOut, parentPath);
                }
                else 
                {
                    try
                    {
                        using (FileStream fs = File.OpenRead(file))
                        {
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);

                            string fileName = file.Replace(parentPath, "");
                            entry = new ZipEntry(fileName);

                            ZipOut.PutNextEntry(entry);
                            ZipOut.Write(buffer, 0, buffer.Length);
                            fs.Close();
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
                RootDir = Path.GetDirectoryName(ZipFile) + "\\"; 
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
        private static int BlockSize = 2048;
        private static int CompressionLevel = 5;
    }
}
