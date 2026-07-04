using SIE.Security;
using SIE.Tech.Routings.ImportRoutings;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using SIE.Web.Tech.Common.Commands;
using System;

namespace SIE.Web.MES.WorkOrders.Commands
{
    /// <summary>
    /// 工艺路线导入
    /// </summary>
    [JsCommand("SIE.Web.Tech.Routings.Commands.ImportRouting")]
    [AllowAnonymous]
    public class ImportRouting : ImportDataCommonCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="importViewArgs"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ImportViewArgs importViewArgs, string scope)
        {
            return base.Excute(importViewArgs, "SIE.Tech.Routings.Routing,SIE.Tech");
        }

        /// <summary>
        /// 处理器
        /// </summary>
        /// <returns>类型</returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ImportRoutingHandle);
        }

        /// <summary>
        /// 设置模板文件信息
        /// </summary>
        /// <param name="importViewArgs">importViewArgs</param>
        /// <param name="scope">scope</param>
        /// <returns>模板信息</returns>
        protected override TemplatePathInfo SetTemplateFileName(ImportViewArgs importViewArgs, string scope)
        {
            const string templateFileName = "工艺路线导入模板.xlsx";
            const string fileRelativePath = "Templates/" + templateFileName;
            return new TemplatePathInfo
            {
                FullPath = fileRelativePath,
                FileName = templateFileName
            };
        }
    }
}