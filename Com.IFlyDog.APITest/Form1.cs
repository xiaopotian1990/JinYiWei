using Com.JinYiWei.Common.Data;
using Com.JinYiWei.Common.Extensions;
using Com.JinYiWei.Common.Logging;
using Com.JinYiWei.Common.Secutiry;
using Com.JinYiWei.Common.WebAPI;
using Com.JinYiWei.Log4Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Com.IFlyDog.APITest
{
    public partial class Form1 : Form
    {
        string sign_key_param_name = "sign_key";
        string sign_param_name = "sign";

        string appid = "583c28f0721e2a6dd4f39f98";
        string appsecred = "90b2b5dd00bc436fb3b8343ca61758fc";
        string sign_key = "a80ab3f2628a410994a6f217c4c133d1";
        //private const string baseAddress = "http://localhost:45445/";
        public Form1()
        {
            InitializeComponent();
            Log4NetLoggerAdapter adapter = new Log4NetLoggerAdapter();
            LogManager.AddLoggerAdapter(adapter);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var token = await WebAPIHelper.GetToken("http://localhost:45445/api/NewToken/Token", appid, appsecred, sign_key);
        }




        private async void button2_Click(object sender, EventArgs e)
        {

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var result2 = await WebAPIHelper.Post("http://localhost:35489/api/Dept/Add", "http://localhost:45445/api/NewToken/Token",
           appid, appsecred, sign_key, new { CreateUserID = 1, Name = "信息开发部", Remark = "信息开发部", SortNo = 1 });

            this.textBox1.Text = result2;
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            var result2 = await WebAPIHelper.Post("http://localhost:35489/api/Dept/Update", "http://localhost:45445/api/NewToken/Token",
           appid, appsecred, sign_key, new { CreateUserID = 1, Name = "信息开发部", Remark = "信息开发部", SortNo = 1, ID = 1 });
            this.textBox1.Text = result2;
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            var result2 = await WebAPIHelper.Post("http://localhost:35489/api/Dept/StopOrUse", "http://localhost:45445/api/NewToken/Token",
           appid, appsecred, sign_key, new { CreateUserID = "super", DeptID = "super", OpenStatus = 1 });
            this.textBox1.Text = result2;
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();

            var result = await WebAPIHelper.Get("http://192.168.11.220:8099/api/Dept/Get", "http://192.168.11.220:8098/api/NewToken/Token", appid, appsecred, sign_key, param);
            this.textBox1.Text = result;
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            this.button7.Enabled = false;
            BatchSubmit submit = new BatchSubmit();
            submit.Appid = "583c28f0721e2a6dd4f39f98";
            submit.Data = new List<BatchTemp>();

            string temp = this.textBox1.Text;
            if (temp.IsNullOrEmpty())
            {
                MessageBox.Show("请输入电话号码！");
                return;
            }

            string[] ss = temp.Split(',');

            for (int i = 0; i < ss.Length; i++)
            {
                submit.Data.Add(new BatchTemp() { phones = ss[i], content = this.textBox2.Text + "回复TD退订！" });
            }
            var result2 = await WebAPIHelper.Post("http://101.200.228.225:8044/api/SSM/BatchSubmit", "http://101.200.228.225:8033/api/NewToken/Token",
          appid, appsecred, sign_key, submit);
            MessageBox.Show(result2);
            this.textBox1.Text = "";
        }
    }

    /// <summary>
    /// 批量提交
    /// </summary>
    public class BatchSubmit
    {
        /// <summary>
        /// appid
        /// </summary>
        public string Appid { get; set; }
        /// <summary>
        /// 批量提交的数据
        /// </summary>
        public List<BatchTemp> Data { get; set; }
    }

    public class BatchTemp
    {
        public string phones { get; set; }
        public string content { get; set; }
        public string sign { get; set; }
    }
}
