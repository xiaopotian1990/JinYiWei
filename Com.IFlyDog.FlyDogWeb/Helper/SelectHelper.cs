using Com.IFlyDog.APIDTO;
using Com.IFlyDog.Common;
using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;
using Com.JinYiWei.Common.Extensions;
using System.Text;
using System.Web.Mvc;
using System;
using Com.JinYiWei.Cache;

namespace Com.IFlyDog.FlyDogWeb.Helper
{
    public static class SelectHelper
    {
        private static RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();

        /// <summary>
        /// 下拉菜单（所有医院通用）
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="elementId"></param>
        /// <param name="className"></param>
        /// <param name="name"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        public static MvcHtmlString SelectCategory(this HtmlHelper helper, SelectType type, string elementId = "", string className = "", string name = "", string selected = "")
        {
            var statusList = new StringBuilder();

            var list = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + type);

            if (list == null)
            {
                var param = new Dictionary<string, string>();
                list = WebAPIHelper.GetResult<IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>>(string.Format(@"/api{0}", type.ToDescription()),
                       param).Data;
            }


            //param.Add("hospitalID", "1");
            //param.Add("userID", "1");

            statusList.AppendFormat("<select class=\"{0}\"  selected=\"selected\" id=\"{1}\" name=\"{2}\">", className, elementId, name);

            var t = "selected=\"selected\"";
            statusList.AppendFormat("<option value=\"-1\" {0} >请选择</option>", selected == "" ? t : "");

            foreach (var item in list)
            {
                statusList.AppendFormat("<option value={0} {1}>{2}</option>", item.ID, item.ID.Equals(selected) ? t : "", item.Name);
            }
            statusList.Append("</select>");

            //_redis.StringSet(RedisPreKey.CategoryHtmlSelect + type, statusList.ToString());

            return new MvcHtmlString(statusList.ToString());
        }

        /// <summary>
        /// 下拉菜单，根据医院查询类型
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="type"></param>
        /// <param name="elementId"></param>
        /// <param name="className"></param>
        /// <param name="name"></param>
        /// <param name="selected"></param>
        /// <returns></returns>

        public static MvcHtmlString SelectCategoryByHospital(this HtmlHelper helper, SelectType type, string elementId = "", string className = "", string name = "", string selected = "")
        {

            string hospitalID = IDHelper.GetHospitalID().ToString();

            if (type == SelectType.ALLHospital)
                hospitalID = "0";

            var tempCS = hospitalID;
            if (type == SelectType.WarehouseOfUser)
            {
                tempCS = IDHelper.GetUserID().ToString();
            }

            var list = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + type + ":" + tempCS);

            if (list == null)
            {
                var param = new Dictionary<string, string>();
                param.Add("hospitalID", hospitalID);
                if (type == SelectType.WarehouseOfUser)
                {
                    param.Add("userID", tempCS);
                }
                list = WebAPIHelper.GetResult<IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>>(string.Format(@"/api{0}", type.ToDescription()),
                       param).Data;
            }


            var statusList = new StringBuilder();

            statusList.AppendFormat("<select class=\"{0}\"  selected=\"selected\" id=\"{1}\" name=\"{2}\">", className, elementId, name);

            var t = "selected=\"selected\"";
            if (type == SelectType.Hospital)
            {
                selected = hospitalID;
            }
            else
            {
                statusList.AppendFormat("<option value=\"-1\" {0} >请选择</option>", selected == "" ? t : "");
            }

            foreach (var item in list)
            {
                statusList.AppendFormat("<option value={0} {1}>{2}</option>", item.ID, item.ID.Equals(selected) ? t : "", item.Name);
            }
            statusList.Append("</select>");

            return new MvcHtmlString(statusList.ToString());
        }

        /// <summary>
        /// 分诊下拉菜单
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="elementId"></param>
        /// <param name="className"></param>
        /// <param name="name"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        public static MvcHtmlString SelectFZUserByHospital(this HtmlHelper helper, string elementId = "", string className = "", string name = "", string selected = "")
        {

            string hospitalID = IDHelper.GetHospitalID().ToString();

            var list = _redis.StringGet<IEnumerable<FZUser>>(RedisPreKey.FZUser + hospitalID + ":" + DateTime.Today.ToShortDateString());

            if (list == null)
            {
                var param = new Dictionary<string, string>();
                param.Add("hospitalID", hospitalID);
                list = WebAPIHelper.GetResult<IFlyDogResult<IFlyDogResultType, IEnumerable<FZUser>>>(string.Format(@"/api/SmartUser/GetFZUsers"),
                       param).Data;
            }


            var statusList = new StringBuilder();

            statusList.AppendFormat("<select class=\"{0}\"  selected=\"selected\" id=\"{1}\" name=\"{2}\">", className, elementId, name);

            var t = "selected=\"selected\"";
            statusList.AppendFormat("<option value=\"-1\" {0} >请选择</option>", selected == "" ? t : "");

            foreach (var item in list)
            {
                statusList.AppendFormat("<option value={0} {1}>{2}</option>", item.ID, item.ID.Equals(selected) ? t : "", item.Name + " | " + item.Shift );
            }
            statusList.Append("</select>");

            return new MvcHtmlString(statusList.ToString());
        }
    }
}