using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.Common
{
    /// <summary>
    /// 缓存清除策略
    /// </summary>
    public static class CacheDelete
    {
        private static RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();

        #region 部门
        /// <summary>
        /// 部门更新引起缓存清除
        /// </summary>
        public static void DeptUpdate(long deptID, long hospitalID)
        {
            _redis.KeyDelete(RedisPreKey.UserInfo);
            CategoryChange(SelectType.Dept, hospitalID);
        }
        public static void DeptAdd(long hospitalID)
        {
            CategoryChange(SelectType.Dept, hospitalID);
        }
        public static void DeptDelete(long deptID, long hospitalID)
        {
            CategoryChange(SelectType.Dept, hospitalID);
        }
        #endregion

        #region 角色
        /// <summary>
        /// 角色更新引起缓存清除
        /// </summary>
        public static void RoleUpdate(long roleID, long hospitalID, IEnumerable<long> userIDS)
        {
            _redis.KeyDelete(RedisPreKey.RolesMenu + roleID);
            //_redis.KeyDelete(RedisPreKey.RolesOfHospital + hospitalID);
            _redis.KeyDelete(RedisPreKey.UserInfo);
            _redis.KeyDelete(RedisPreKey.FZUser + hospitalID + ":" + DateTime.Today.ToShortDateString());

            foreach (var u in userIDS)
                _redis.KeyDelete(RedisPreKey.UserLogin + u);
        }
        /// <summary>
        /// 角色添加引起缓存清除
        /// </summary>
        public static void RoleAdd(long hospitalID)
        {
            //_redis.KeyDelete(RedisPreKey.RolesOfHospital + hospitalID);
        }
        /// <summary>
        /// 角色删除引起缓存清除
        /// </summary>
        public static void RoleDelete(long roleID, long hospitalID)
        {
            _redis.KeyDelete(RedisPreKey.RolesMenu + roleID);
            //_redis.KeyDelete(RedisPreKey.RolesOfHospital + hospitalID);
            //_redis.KeyDelete(RedisPreKey.UserInfo);
        }
        #endregion

        #region 用户
        /// <summary>
        /// 用户添加引起缓存清除
        /// </summary>
        public static void UserAdd(long userID, long hospitalID = 0)
        {
            _redis.KeyDelete(RedisPreKey.UserInfo);
            _redis.KeyDelete(RedisPreKey.FZUser + hospitalID + ":" + DateTime.Today.ToShortDateString());
        }
        /// <summary>
        /// 用户更新引起缓存清除
        /// </summary>
        public static void UserUpdate(long userID, long hospitalID = 0)
        {
            _redis.KeyDelete(RedisPreKey.UserInfo);
            _redis.KeyDelete(RedisPreKey.UserLogin + userID);
            _redis.KeyDelete(RedisPreKey.FZUser + hospitalID + ":" + DateTime.Today.ToShortDateString());
        }
        /// <summary>
        /// 用户使用停用引起缓存清除
        /// </summary>
        public static void UserStopOrUse(long userID, long hospitalID = 0)
        {
            _redis.KeyDelete(RedisPreKey.UserInfo);
            _redis.KeyDelete(RedisPreKey.UserLogin + userID);
            _redis.KeyDelete(RedisPreKey.FZUser + hospitalID + ":" + DateTime.Today.ToShortDateString());
        }
        #endregion

        #region 仓库
        /// <summary>
        /// 仓库更新引起缓存清除
        /// </summary>     
        public static void WarehouseChange(long warehouseID, long hospitalID, IEnumerable<long> userIDS)
        {
            CategoryChange(SelectType.Warehouse, hospitalID);
            foreach (var u in userIDS)
                CategoryChange(SelectType.WarehouseOfUser, null, u);
        }
        #endregion

        #region 下拉菜单
        /// <summary>
        /// 下拉菜单改变
        /// </summary>
        /// <param name="type"></param>
        /// <param name="hospitalID"></param>
        /// <param name="userID"></param>
        public static void CategoryChange(SelectType type, long? hospitalID = null, long? userID = null)
        {

            if (hospitalID == -1)
            {
                for (int i = 1; i <= Key.HospitalCount; i++)
                {
                    _redis.KeyDelete(RedisPreKey.Category + type + ":" + i);
                }
            }
            else
            {
                var tempHospitalID = hospitalID == null ? "" : ":" + hospitalID;
                var tempUserID = userID == null ? "" : ":" + userID;
                string key = RedisPreKey.Category + type + tempHospitalID + tempUserID;
                //string keyHtml = RedisPreKey.CategoryHtmlSelect + type + tempHospitalID + tempUserID;
                _redis.KeyDelete(key);
            }

            //_redis.KeyDelete(RedisPreKey.CategoryHtmlSelect + type);
        }
        #endregion

        #region Option
        public static void OptionUpdate(string key)
        {
            _redis.KeyDelete(key);
        }
        #endregion
    }
}
