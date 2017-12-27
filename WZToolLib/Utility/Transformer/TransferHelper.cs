using System;
using System.Collections.Generic;
using System.Text;

namespace WZToolLib.Utility.Transformer
{
    public class ValueTransformer
    {
        public static uint HexStrToUInt(string strValue)
        {
            char[] array = strValue.ToCharArray();
            uint num = 0u;
            try
            {
                int i = 0;
                while (i < array.Length)
                {
                    string text = array[i].ToString().ToUpper();
                    string text2 = text;
                    switch (text2)
                    {
                        case "A":
                            text = "10";
                            break;
                        case "B":
                            text = "11";
                            break;
                        case "C":
                            text = "12";
                            break;
                        case "D":
                            text = "13";
                            break;
                        case "E":
                            text = "14";
                            break;
                        case "F":
                            text = "15";
                            break;
                    }
               
                    double value = Math.Pow(16.0, Convert.ToDouble(array.Length - i - 1));
                    num += Convert.ToUInt32(text) * Convert.ToUInt32(value);
                    i++;
                }
            }
            catch (Exception ex)
            {
                string text3 = ex.ToString();
                return 0u;
            }
            return num;
        }

        public static int HexStr2Dec(string strValue)
        {
            char[] array = strValue.ToCharArray();
            int num = 0;
            try
            {
                int i = 0;
                while (i < array.Length)
                {
                    string text = array[i].ToString().ToUpper();
                    string text2 = text;
                    switch (text2)
                    {
                        case "A":
                            text = "10";
                            break;
                        case "B":
                            text = "11";
                            break;
                        case "C":
                            text = "12";
                            break;
                        case "D":
                            text = "13";
                            break;
                        case "E":
                            text = "14";
                            break;
                        case "F":
                            text = "15";
                            break;
                    }

                    double value = Math.Pow(16.0, Convert.ToDouble(array.Length - i - 1));
                    num += Convert.ToInt32(text) * Convert.ToInt32(value);
                    i++;
                }
            }
            catch (Exception ex)
            {
                string text3 = ex.ToString();
                return 0;
            }
            return num;
        }

        public static string HexStr2ASCII(string strValue)
        {
            int num = strValue.Length / 2;
            List<byte> list = new List<byte>();
            for (int i = 0; i < num; i++)
            {
                byte item = (byte)Convert.ToInt32(strValue.Substring(i * 2, 2), 16);
                list.Add(item);
            }

            byte[] bytes = list.ToArray();
            return Encoding.ASCII.GetString(bytes);
        }

        public static byte[] ASCII2HexStr(string strValue)
        {
            int length = strValue.Length;
            char[] chars = strValue.ToCharArray();
            return Encoding.ASCII.GetBytes(chars);
        }

        public static string Reverse(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("Invalid Argument");
            }
            StringBuilder stringBuilder = new StringBuilder(str.Length);
            for (int i = str.Length - 1; i >= 0; i--)
            {
                stringBuilder.Append(str[i]);
            }
            return stringBuilder.ToString();
        }

        public static string Reverse(string str, int mode)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("Invalid Argument");
            }
            StringBuilder stringBuilder = new StringBuilder(str.Length);
            for (int i = str.Length - 1; i >= 0; i -= mode)
            {
                for (int j = mode - 1; j >= 0; j--)
                {
                    stringBuilder.Append(str[i - j]);
                }
            }
            return stringBuilder.ToString();
        }

        public static string ToHexString(byte[] bytes)
        {
            string result = string.Empty;
            if (bytes != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringBuilder.Append(bytes[i].ToString("X2"));
                }
                result = stringBuilder.ToString();
            }
            return result;
        }

        public static string To2HexString(string HexStr)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < HexStr.Length; i++)
            {
                string text = HexStr.Substring(i, 1);
                string text2 = text.ToUpper();
                switch (text2)
                {
                    case "0":
                        text = "0000";
                        break;
                    case "1":
                        text = "0001";
                        break;
                    case "2":
                        text = "0010";
                        break;
                    case "3":
                        text = "0011";
                        break;
                    case "4":
                        text = "0100";
                        break;
                    case "5":
                        text = "0101";
                        break;
                    case "6":
                        text = "0110";
                        break;
                    case "7":
                        text = "0111";
                        break;
                    case "8":
                        text = "1000";
                        break;
                    case "9":
                        text = "1001";
                        break;
                    case "A":
                        text = "1010";
                        break;
                    case "B":
                        text = "1011";
                        break;
                    case "C":
                        text = "1100";
                        break;
                    case "D":
                        text = "1101";
                        break;
                    case "E":
                        text = "1110";
                        break;
                    case "F":
                        text = "1111";
                        break;
                }
                stringBuilder.Append(text);
            }
            return stringBuilder.ToString();
        }

        public static string HexStr2BCD(string retValue, int length)
        {
            List<byte> list = new List<byte>();
            long num = Convert.ToInt64(retValue, 16);
            for (int i = length - 1; i >= 0; i--)
            {
                long num2 = num % 10L;
                list.Add(Convert.ToByte((num2 / 10L << 4) + (num2 % 10L & 15L)));
                num /= 100L;
            }
            Console.WriteLine(list.ToArray().ToString());
            return ValueTransformer.ToHexString(list.ToArray());
        }

        public static string HexStr2BCD_Standard(string retValue, int length)
        {
            string text = string.Empty;
            string value = string.Empty;
            for (int i = 0; i < length; i++)
            {
                value = retValue.Substring(i, 1);
                int num = Convert.ToInt32(value, 16);
                text += (num / 10 * 10 + num % 10).ToString();
            }
            Console.WriteLine(text);
            return text;
        }

        public static byte[] HexStr2Byte(string strValue)
        {
            if (strValue.IndexOf("x") == 1)
            {
                strValue = strValue.Substring(2);
            }
            int num = strValue.Length / 2;
            List<byte> list = new List<byte>();
            for (int i = 0; i < num; i++)
            {
                byte item = (byte)Convert.ToInt32(strValue.Substring(i * 2, 2), 16);
                list.Add(item);
            }
            return list.ToArray();
        }

        public static byte[] HexStr2Byte(string strValue, int maxLength)
        {
            if (strValue.IndexOf("x") == 1)
            {
                strValue = strValue.Substring(2);
            }
            int length = strValue.Length;
            byte[] result;
            if (length > maxLength)
            {
                result = null;
            }
            else
            {
                for (int i = 0; i < maxLength - length; i++)
                {
                    strValue = "0" + strValue;
                }
                int num = strValue.Length / 2;
                List<byte> list = new List<byte>();
                for (int i = 0; i < num; i++)
                {
                    byte item = (byte)Convert.ToInt32(strValue.Substring(i * 2, 2), 16);
                    list.Add(item);
                }
                byte[] array = list.ToArray();
                result = array;
            }
            return result;
        }

        public static double HexToFloat(string byteStr)
        {
            double num = 0.0;
            int value = Convert.ToInt32(byteStr, 16);
            string text = Convert.ToString(value, 2);
            for (int i = 0; i < 32; i++)
            {
                if (text.Length < i + 1)
                {
                    text = "0" + text;
                }
            }
            double y = (text.Substring(0, 1) == "0") ? 0.0 : 1.0;
            string value2 = text.Substring(1, 8);
            double y2 = (double)(Convert.ToInt32(value2, 2) - 127);
            double num2 = -1.0;
            for (int i = 9; i < 32; i++)
            {
                if (text.Substring(i, 1) == "1")
                {
                    double num3 = 1.0;
                    num += num3 * Math.Pow(2.0, num2);
                }
                num2 -= 1.0;
            }
            double value3 = Math.Pow(-1.0, y) * (1.0 + num) * Math.Pow(2.0, y2);
            return Math.Round(value3, 2);
        }

        private string IntegerToHexstr(int intaddr)
        {
            string[] array = new string[4];
            array[0] = (intaddr & 255).ToString("X2");
            for (int i = 1; i < 4; i++)
            {
                intaddr >>= 8;
                string[] array2;
                IntPtr intPtr;
                (array2 = array)[(int)(intPtr = (IntPtr)i)] = array2[(int)intPtr] + (intaddr & 255).ToString("X2");
            }
            return array[3] + array[2] + array[1] + array[0];
        }

        public static string The2HexstrTo16Hexstr(string str)
        {
            int num = str.Length % 8;
            if (num != 0)
            {
                for (int i = 0; i < 8 - num; i++)
                {
                    str = "0" + str;
                }
            }
            string text = "";
            for (int i = 0; i < str.Length; i += 4)
            {
                string text2 = str.Substring(i, 4);
                string text3 = text2;
                switch (text3)
                {
                    case "0000":
                        text += "0";
                        break;
                    case "0001":
                        text += "1";
                        break;
                    case "0010":
                        text += "2";
                        break;
                    case "0011":
                        text += "3";
                        break;
                    case "0100":
                        text += "4";
                        break;
                    case "0101":
                        text += "5";
                        break;
                    case "0110":
                        text += "6";
                        break;
                    case "0111":
                        text += "7";
                        break;
                    case "1000":
                        text += "8";
                        break;
                    case "1001":
                        text += "9";
                        break;
                    case "1010":
                        text += "A";
                        break;
                    case "1011":
                        text += "B";
                        break;
                    case "1100":
                        text += "C";
                        break;
                    case "1101":
                        text += "D";
                        break;
                    case "1110":
                        text += "E";
                        break;
                    case "1111":
                        text += "F";
                        break;
                }
            }
            return text;
        }

        public static string IntegerToStr(int length)
        {
            string[] array = new string[3];
            array[0] = (length & 255).ToString("X2");
            for (int i = 1; i < 3; i++)
            {
                length >>= 8;
                array[i] = (length & 255).ToString("X2");
            }
            return array[2] + array[1] + array[0];
        }

        public static string StrTo16Hex(string str)
        {
            string text = "";
            string result;
            if (str == "")
            {
                result = "";
            }
            else
            {
                byte[] bytes = Encoding.Default.GetBytes(str);
                for (int i = 0; i < bytes.Length; i++)
                {
                    text += bytes[i].ToString("X");
                }
                result = text;
            }
            return result;
        }

        public static string IntegerToStr(ulong intaddr)
        {
            string text = (intaddr & 255UL).ToString("X2");
            for (int i = 1; i < 8; i++)
            {
                intaddr >>= 8;
                text = (intaddr & 255UL).ToString("X2") + text;
            }
            return text;
        }
    }
}
