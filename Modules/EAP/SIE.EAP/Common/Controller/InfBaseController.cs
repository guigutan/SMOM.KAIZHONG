using SIE.Domain;
using SIE.EAP.Common.Enums;
using SIE.EAP.Common.Logs;
using SIE.ManagedProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.EAP.Common.Controller
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    public class InfBaseController : DomainController
    {
        #region 声明变量

        /// <summary>
        /// 最大重试次数(根据项目实际需要，可以做成配置项或配置文件)
        /// </summary>
        public const int MAX_RETYR_COUNT = 5;

        /// <summary>
        /// 最大批处理数量(根据项目实际需要，可以做成配置项或配置文件)
        /// </summary>
        public const int MAX_BATCH_QUANTITY = 1000;

        #endregion

        #region 通用

        /// <summary>
        /// 初始化API接口环境
        /// </summary>
        /// <param name="invOrg"></param>
        public virtual void InitEnvironment(int invOrg)
        {
            //库存组织赋值
            if (invOrg > 0)
                RT.InvOrg = invOrg;
        }

        /// <summary>
        /// 保存EAP接口记录
        /// </summary>
        /// <param name="direction">下载方向</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="remark">备注</param>
        /// <param name="requestContent">请求内容</param>
        /// <param name="responseContent">响应内容</param>
        public virtual void SaveEAPInfLog(string desc, JobDirection direction, DateTime beginDate, DateTime endDate, string remark = null, 
            string requestContent = null, string responseContent = null)
        {
            var downloadLog = new EAPInterfaceLog();
            downloadLog.Desc = desc;
            downloadLog.JobDirection = direction;
            downloadLog.BeginDate = beginDate;
            downloadLog.EndDate = endDate;
            downloadLog.Remark = remark;
            downloadLog.RequestContent = requestContent;
            downloadLog.ResponseContent = responseContent;
            RF.Save(downloadLog);
        }
        #endregion

    }
}
