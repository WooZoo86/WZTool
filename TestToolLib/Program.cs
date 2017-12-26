using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WZToolLib.Utility.FileCompress;

namespace TestToolLib
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!ZipHelper.UnZip("E:\\BaiduNetdiskDownload\\GoodSync-v10-Setup-Portable.zip"))
                Console.WriteLine(ZipHelper.ErrMsg);
            else
                Console.WriteLine("Ok!");


            Console.ReadKey();
        }
    }
}
