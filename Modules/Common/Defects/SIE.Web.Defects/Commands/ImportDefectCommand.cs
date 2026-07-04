using SIE.Defects.ImportHandle;
using SIE.Web.Common.Import.Commands;
using System;

namespace SIE.Web.Defects.Commands
{
    /// <summary>
    /// 数据导入 缺陷代码
    /// </summary>
    class ImportDefectCommand : ImportDataCommonCommand
    {
        /// <summary>
        /// 获取导入处理类型
        /// </summary>
        /// <returns></returns>
        protected override Type GetImportHandleType()
        {
            return typeof(DefectsHandle);
        }

        /// <summary>
        /// 设置模板文件信息
        /// </summary>
        /// <param name="importViewArgs"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override TemplatePathInfo SetTemplateFileName(ImportViewArgs importViewArgs, string scope)
        {
            const string templateFileName = "缺陷代码导入模板.xlsx";
            const string fileRelativePath = "Templates/" + templateFileName;
            return new TemplatePathInfo
            {
                FullPath = fileRelativePath,
                FileName = templateFileName
            };
        }
    }
}
