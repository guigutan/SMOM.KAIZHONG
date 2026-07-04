using SIE.Common.ImportHelper;
using SIE.MES.WorkOrders.ImportWorkOrders;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.Web.MES.WorkOrders.Commands
{
    /// <summary>
    /// 工单导入
    /// </summary>
    [JsCommand("SIE.Web.MES.WorkOrders.WorkOrderImportCommand")]
    public class WorkOrderImportCommand : ImportCommandBase
    {
        public const string FullName = "SIE.Web.MES.WorkOrders.WorkOrderImportCommand";

        /// <summary>
        /// 导入完成回调方法
        /// </summary>
        /// <returns>导入结果</returns>
        protected override ImportCompleted GetImportCompleted()
        {
            return (DataRow[] drSuccess, DataRow[] drFailed) =>
            {
            };
        }

        /// <summary>
        /// 获取导入处理类型
        /// </summary>
        /// <returns>工单导入处理类</returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ImportWorkOrderHandle);
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="importViewArgs">导入视图参数</param>
        /// <param name="scope">使用范围</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ImportViewArgs importViewArgs, string scope)
        {
            if (importViewArgs.BehaviorName == "Download")
            {
                return DownloadTemplate();
            }
            else
            {
                return ImportData(importViewArgs);
            }
        }

        /// <summary>
        /// 直接从服务器上下载模板
        /// </summary>
        /// <returns></returns>
        public virtual object DownloadTemplate()
        {
            const string templateFileName = "工单导入模板.xlsx";
            const string fileRelativePath = "Templates/" + templateFileName;

            return new
            {
                FilePath = fileRelativePath,
                FileName = templateFileName
            };
        }


        /// <summary>
        /// 获取导入模板数据
        /// </summary>
        /// <returns>模板数据</returns>
        protected override List<string> GetImportTempleData()
        {
            return new List<string> { "GD180404145", "Code001", "","100", "量产", "2018/4/4", "2018/4/28", "ShopCode", "B线", "", "", "", "", "100","py工厂" };
        }
    }
}