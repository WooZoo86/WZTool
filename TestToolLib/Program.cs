using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WZToolLib.Utility.FileCompress;
using WZToolLib.Utility.Transformer;

namespace TestToolLib
{
    class Program
    {
        static void Main(string[] args)
        {
            //ZipHelper.Password = "123456";
            //if (!ZipHelper.ZipDir("E:\\BaiduNetdiskDownload", null))
            //    Console.WriteLine(ZipHelper.ErrMsg);
            //else
            //    Console.WriteLine("Ok!");

            string s = TransferHelepr.HexStr2ASCII("31");
            Console.WriteLine(s);
            

            Console.ReadKey();
        }
    }
}
