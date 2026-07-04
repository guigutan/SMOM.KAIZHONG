using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.IOT
{
    public class IotTokenData
    {
        /// <summary>
        /// 
        /// </summary>
        public ResultIotTokenData result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string jsonrpc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string error { get; set; }
    }
    public class ResultIotTokenData
    {
        /// <summary>
        /// 
        /// </summary>
        public IotTokenDataData data { get; set; }
    }
    public class IotTokenDataData
    {
        /// <summary>
        /// 
        /// </summary>
        public string languageFavorite { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<RolesItemIotTokenData> roles { get; set; }
        /// <summary>
        /// test租户管理员
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pwd_expire_day { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string login { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string token { get; set; }
    }

    public class RolesItemIotTokenData
    {
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// test租户管理员角色
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 负责test租户的管理及维护工作
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
    }
}
