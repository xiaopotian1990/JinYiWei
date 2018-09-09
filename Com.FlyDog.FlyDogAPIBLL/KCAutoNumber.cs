using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class KCAutoNumber : BaseService
    {
        private static readonly object _object = new object();

        private static int _workID;
        public static DateTime _today = DateTime.Today;
        public static readonly object _todayObject = new object();


        private static int _rkNumber = 1;
        private static readonly object _rkobject = new object();

        private static int _ckNumber = 1;
        private static readonly object _ckobject = new object();

        private static int _dbNumber = 1;
        private static readonly object _dbobject = new object();

        private static int _pdNumber = 1;
        private static readonly object _pdobject = new object();

        private static int _lyNumber = 1;
        private static readonly object _lyobject = new object();

        private static int _yyNumber = 1;
        private static readonly object _yyobject = new object();


        private static KCAutoNumber _instance = null;
        private KCAutoNumber()
        {
        }

        public static KCAutoNumber Instance()
        {
            if (_instance == null)
            {
                lock (_object)
                {
                    if (null == _instance)
                    {
                        _instance = new KCAutoNumber();
                        _instance.Init(Key.WorkID);
                    }
                }
            }

            return _instance;
        }


        /// <summary>
        /// 库存相关单号生成
        /// </summary>
        /// <param name="workID">机器ID</param>
        /// <param name="qz">标志： JH代表入库，TH代表出库，DB代表调拨，PD代表盘点,SY代表领用</param>
        /// <returns></returns>
        public string CKNumber(string qz)
        {
            Reset();
            DateTime time = DateTime.Today;
            string month = time.Month < 10 ? "0" + time.Month : time.Month.ToString();
            string day = time.Day < 10 ? "0" + time.Day : time.Day.ToString();
            string workID_temp = _workID < 10 ? "0" + _workID : _workID.ToString();
            //string hour = time.Hour < 10 ? "0" + time.Hour : time.Hour.ToString();
            //string minute = time.Minute < 10 ? "0" + time.Minute : time.Minute.ToString();
            //string secend = time.Second < 10 ? "0" + time.Second : time.Second.ToString();
            string temp = qz + time.Year.ToString().Substring(2) + month + day + workID_temp;
            int number = 0;

            #region switch
            switch (qz)
            {
                case "JH":
                    lock (_rkobject)
                    {
                        _rkNumber++;
                        number = _rkNumber;
                    }
                    break;

                case "TH":
                    lock (_ckobject)
                    {
                        _ckNumber = 1;
                        number = _rkNumber;
                    }
                    break;

                case "DB":
                    lock (_dbobject)
                    {
                        _dbNumber++;
                        number = _dbNumber;
                    }
                    break;

                case "PD":
                    lock (_pdobject)
                    {
                        _pdNumber = 1;
                        number = _pdNumber;
                    }
                    break;

                case "SY":
                    lock (_lyobject)
                    {
                        _lyNumber = 1;
                        number = _lyNumber;
                    }
                    break;
            }
            #endregion
            if (number < 10)
                temp += "00" + number;
            else if (number < 100)
                temp += "0" + number;
            else
                temp += number;

            return temp;
        }

        /// <summary>
        /// 重置
        /// </summary>
        private void Reset()
        {
            lock (_todayObject)
            {
                if (DateTime.Today > _today)
                {
                    _rkNumber = 1;
                    _ckNumber = 1;
                    _dbNumber = 1;
                    _pdNumber = 1;
                    _lyNumber = 1;
                    _today = DateTime.Today;
                }
            }
        }

        /// <summary>
        /// 生成预约码
        /// </summary>
        /// <returns></returns>
        public string AppointmentCode()
        {
            DateTime now = DateTime.Today;
            Reset();
            string temp = NumberToChar(now.Year % 100) + NumberToChar(now.Month) + NumberToChar(now.Day);
            string workID_temp = _workID < 10 ? "0" + _workID : _workID.ToString();
            int number = 0;
            lock (_yyobject)
            {
                _yyNumber++;
                number = _yyNumber;
            }

            temp += workID_temp + number;

            return temp;
        }

        /// <summary>
        /// 把1,2,3,...,35,36转换成A,B,C,...,Y,Z
        /// </summary>
        /// <param name="number">数字</param>
        /// <returns></returns>
        private string NumberToChar(int number)
        {
            if (1 <= number && 36 >= number)
            {
                int num = number + 64;
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] btNumber = new byte[] { (byte)num };
                return asciiEncoding.GetString(btNumber);
            }
            return "";
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="workID"></param>
        public void Init(int workID)
        {
            _workID = workID;
            TryExecute(() =>
            {
                var param = new { CreateTime = DateTime.Today };
                string rkNO = _connection.Query<string>("SELECT TOP 1 [No] FROM [SmartPurchase] where CreateTime> @CreateTime order by CreateTime desc", param).FirstOrDefault();

                _rkNumber = rkNO.IsNullOrEmpty() ? 0 : rkNO.Length == 13 ? Convert.ToInt32(rkNO.Substring(rkNO.Length - 3)) : 0;

                string ckNO = _connection.Query<string>("SELECT TOP 1 [No] FROM [SmartReturn] where CreateTime> @CreateTime order by CreateTime desc", param).FirstOrDefault();

                _ckNumber = ckNO.IsNullOrEmpty() ? 0 : ckNO.Length == 13 ? Convert.ToInt32(ckNO.Substring(ckNO.Length - 3)) : 0;

                string dbNO = _connection.Query<string>("SELECT TOP 1 [No] FROM [SmartAllocate] where CreateTime> @CreateTime order by CreateTime desc", param).FirstOrDefault();

                _dbNumber = dbNO.IsNullOrEmpty() ? 0 : dbNO.Length == 13 ? Convert.ToInt32(dbNO.Substring(dbNO.Length - 3)) : 0;

                string pdNO = _connection.Query<string>("SELECT TOP 1 [No] FROM [SmartCheck] where CreateTime> @CreateTime order by CreateTime desc", param).FirstOrDefault();

                _pdNumber = pdNO.IsNullOrEmpty() ? 0 : pdNO.Length == 13 ? Convert.ToInt32(pdNO.Substring(pdNO.Length - 3)) : 0;

                string lyNO = _connection.Query<string>("SELECT TOP 1 [No] FROM [SmartUse] where CreateTime> @CreateTime order by CreateTime desc", param).FirstOrDefault();

                _lyNumber = lyNO.IsNullOrEmpty() ? 0 : lyNO.Length == 13 ? Convert.ToInt32(lyNO.Substring(lyNO.Length - 3)) : 0;

                //预约
                string yyNO = _connection.Query<string>("SELECT TOP 1 [Code] FROM [SmartAppointment] where CreateTime> @CreateTime order by CreateTime desc", param).FirstOrDefault();

                _yyNumber = yyNO.IsNullOrEmpty() ? 0 : yyNO.Length > 5 ? Convert.ToInt32(yyNO.Substring(5)) : 0;
            });
        }
    }
}
