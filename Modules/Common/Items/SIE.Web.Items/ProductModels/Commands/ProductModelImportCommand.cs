using SIE.Items.ProductModels.ImportProductModels;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;

namespace SIE.Web.Items.ProductModels.Commands
{
    /// <summary>
    /// 产品机型导入
    /// </summary>
    [JsCommand("SIE.Web.Items.ProductModels.Commands.ProductModelImportCommand")]
    public class ProductModelImportCommand : ImportDataCommonCommand
    {
        /// <summary>
        /// 导入命令
        /// </summary>
        public const string FullName = "SIE.Web.Items.ProductModels.Commands.ProductModelImportCommand";

        /// <summary>
        /// 获取导入处理类型
        /// </summary>
        /// <returns>工单导入处理类</returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ImportProductModelHandel);
        }

        /// <summary>
        /// 设置模板文件信息
        /// </summary>
        /// <param name="importViewArgs">importViewArgs</param>
        /// <param name="scope">scope</param>
        /// <returns>模板信息</returns>
        protected override TemplatePathInfo SetTemplateFileName(ImportViewArgs importViewArgs, string scope)
        {
            const string templateFileName = "产品机型导入模板.xlsx";
            const string fileRelativePath = "Templates/" + templateFileName;
            return new TemplatePathInfo
            {
                FullPath = fileRelativePath,
                FileName = templateFileName,
            };
        }
    }
}
