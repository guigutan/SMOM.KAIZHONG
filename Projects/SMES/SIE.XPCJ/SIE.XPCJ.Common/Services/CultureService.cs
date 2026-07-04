using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Common.Exceptions;
using SIE.XPCJ.Common.Settings;
using SIE.XPCJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Services
{
    /// <summary>
    /// 文化 本地化服务
    /// </summary>
    public static class CultureService
    {
        private static readonly string cultureController = "CultureController";

        /// <summary>
        /// 下载文化资源
        /// </summary>
        /// <param name="loadItemIds"></param>
        public static List<Resource> DownLoadCultureResource(string cultureCode)
        {
            object[] parameters = new object[2];
            parameters[0] = cultureCode;
            parameters[1] = ResourceType.Simplify;
            var result = ApiHelper.Post<List<Resource>>(cultureController, "GetCulture", parameters);
            if (!result.Success)
            {
                MessageBox.Show(result.Message);
            }
            return result.Result;
        }

        /// <summary>
        /// 上传资源文化
        /// </summary>
        /// <param name="resource"></param>

        public static bool UploadCultureResource(Resource resource)
        {
            object[] parameters = new object[1];
            parameters[0] = resource;
            var result = ApiHelper.Post<string>(cultureController, "SetResources", parameters);
            if (!result.Success)
            {
                Console.WriteLine(result.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 匿名获取所有文化
        /// </summary>
        /// <returns></returns>
        public static List<Culture> GetAllCulture()
        {
            object[] parameters = new object[0];
            var result = ApiHelper.Post<List<Culture>>(cultureController, "GetAllCulture", parameters);
            if (!result.Success)
            {
                MessageBox.Show(result.Message);
            }
            return result.Result;
        }


    }
}
