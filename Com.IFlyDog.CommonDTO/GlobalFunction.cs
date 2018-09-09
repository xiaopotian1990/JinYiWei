using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 公共方法
    /// </summary>
    public class GlobalFunction
    {

        /// <summary>
        /// 系统内置账号
        /// </summary>
        /// <returns></returns>
        public static List<int> GetSystemAccount()
        {
            List<int> acounts = new List<int>();
            acounts.Add(GlobalConstants.SuperAccount);
            acounts.Add(GlobalConstants.BlankManagerAccount);

            return acounts;
        }
        public static string GetAppointmentCode()
        {
            StringBuilder sb = new StringBuilder();
            string RandomString_09AZ = "0123456789ABCDEFGHIJKMLNOPQRSTUVWXYZ";
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                int n = random.Next(RandomString_09AZ.Length - 1);
                sb.Append(RandomString_09AZ[n]);
            }
            return sb.ToString();
        }

        public static decimal ToFixed(decimal d, int s)
        {
            decimal sp = Convert.ToDecimal(Math.Pow(10, s));

            if (d < 0)
                return Math.Truncate(d) + Math.Ceiling((d - Math.Truncate(d)) * sp) / sp;
            else
                return Math.Truncate(d) + Math.Floor((d - Math.Truncate(d)) * sp) / sp;
        }

        public static string GetPurchaseNo(int ID)
        {
            string s = string.Format("{0:d8}", ID);
            return "JH" + s;
        }
        public static string GetReturnNo(int ID)
        {
            string s = string.Format("{0:d8}", ID);
            return "TH" + s;
        }
        public static string GetOffsetNo(int ID)
        {
            string s = string.Format("{0:d8}", ID);
            return "CJ" + s;
        }
        public static string GetCheckNo(int ID)
        {
            string s = string.Format("{0:d8}", ID);
            return "PD" + s;
        }
        public static string GetAllocateNo(int ID)
        {
            string s = string.Format("{0:d8}", ID);
            return "DB" + s;
        }

        public static string GetUseNo(int ID)
        {
            string s = string.Format("{0:d8}", ID);
            return "SY" + s;
        }
        public static string GetStockInNo(int ID)
        {
            string s = string.Format("{0:d8}", ID);
            return "RK" + s;
        }
        public static string GetStockOutNo(int ID)
        {
            string s = string.Format("{0:d8}", ID);
            return "CK" + s;
        }

        public static string GetCashierNo(int ID)
        {
            string s = string.Format("{0:d8}", ID);
            return "PN" + s;
        }


        public static bool isValidMobile(string str)
        {
            string dianxin = @"^1[3578][01379]\d{8}$";
            Regex dReg = new Regex(dianxin);

            string liantong = @"^1[34578][01256]\d{8}$";
            Regex tReg = new Regex(liantong);

            string yidong = @"^(134[012345678]\d{7}|1[34578][012356789]\d{8})$";
            Regex yReg = new Regex(yidong);

            return dReg.IsMatch(str) || tReg.IsMatch(str) || yReg.IsMatch(str);
        }

        public static bool IsValidTel(string str)
        {
            return Regex.IsMatch(str, @"^(\d{3,4}-)?\d{6,8}$");
        }

        public static string WholeHidePhone(string str)
        {
            if (str == null || str.Equals(""))
            {
                return "";
            }
            str = Regex.Replace(str, "0", "*");
            str = Regex.Replace(str, "1", "*");
            str = Regex.Replace(str, "2", "*");
            str = Regex.Replace(str, "3", "*");
            str = Regex.Replace(str, "4", "*");
            str = Regex.Replace(str, "5", "*");
            str = Regex.Replace(str, "6", "*");
            str = Regex.Replace(str, "7", "*");
            str = Regex.Replace(str, "8", "*");
            str = Regex.Replace(str, "9", "*");

            return str;
        }

        public static string PartHidePhone(string str)
        {
            if (str == null || str.Equals(""))
            {
                return "";
            }
            if (str.Length == 11)
            {
                string str1 = str.Substring(0, 3);
                string str2 = str.Substring(3, str.Length - 7);
                string str3 = str.Substring(str.Length - 4, 4);
                return str1 + WholeHidePhone(str2) + str3;
            }
            else
            {
                if (str.Length < 6)
                {
                    return str;
                }
                else
                {
                    string str1 = str.Substring(0, 5);
                    string str2 = str.Substring(5);
                    return WholeHidePhone(str1) + str2;
                }
            }
        }



        public static string NoZeroNum(decimal s)
        {
            string re = s.ToString();

            string last = re.Substring(re.Length - 1);

            while (last.Equals("0"))
            {
                int len = re.Length;
                re = re.Substring(0, len - 1);
                last = re.Substring(re.Length - 1);
            }

            last = re.Substring(re.Length - 1);
            if (last.Equals("."))
            {
                int len = re.Length;
                re = re.Substring(0, len - 1);
                last = re.Substring(re.Length - 1);
            }
            return re;
        }


        public static string Percent(decimal d)
        {
            string s = (d * 100).ToString("F2") + "%";
            return s;
        }

        public static string F2(decimal d)
        {
            string s = d.ToString("#0.00");
            return s;
        }
    }
}
