using SIE.MES.WorkOrders.ImportWorkOrders;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using SIE.Web.MES.Common.Commands;
using System;

namespace SIE.Web.MES.WorkOrders.Commands
{
    /// <summary>
    /// 条码导入命令
    /// </summary>
    [JsCommand("SIE.Web.MES.WorkOrders.BarcodeDownLoadCommand")]
    public class BarcodeDownLoadCommand : ImportDataCommonCommand
    {
        /// <summary>
        /// 
        /// </summary>
        public const string FullName = "SIE.Web.MES.WorkOrders.BarcodeDownLoadCommand";

        /// <summary>
        /// 处理器
        /// </summary>
        /// <returns>类型</returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ImportBarcodeHandle);
        }

        /// <summary>
        /// 设置模板文件信息
        /// </summary>
        /// <param name="importViewArgs">importViewArgs</param>
        /// <param name="scope">scope</param>
        /// <returns>模板信息</returns>
        protected override TemplatePathInfo SetTemplateFileName(ImportViewArgs importViewArgs, string scope)
        {
           const string templateFileName = "工单条码导入模板.xls";
            const string fileRelativePath = "Templates/" + templateFileName;
            return new TemplatePathInfo
            {
                FullPath = fileRelativePath,
                FileName = templateFileName,
            };
        }
    }
}
