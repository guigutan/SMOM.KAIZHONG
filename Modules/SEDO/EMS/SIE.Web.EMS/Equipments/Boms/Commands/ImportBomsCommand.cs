using SIE.EMS.Equipments.Boms.ImportEquipBom;
using SIE.Web.Common.Import.Commands;
using SIE.Web.EMS.Common.Commands;
using System;

namespace SIE.Web.EMS.Equipments.Boms.Commands
{
    /// <summary>
    /// 资料导入命令
    /// </summary>
    public class ImportBomsCommand : ImportDataCommonCommand
    {
        /// <summary>
        /// 获取导入处理类型
        /// </summary>
        /// <returns></returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ImportEquipBomHandle);
        }

        /// <summary>
        /// 设置模板文件信息
        /// </summary>
        /// <param name="importViewArgs"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override TemplatePathInfo SetTemplateFileName(ImportViewArgs importViewArgs, string scope)
        {
            const string templateFileName = "设备Bom导入模板.xlsx";
            const string fileRelativePath = "Templates/" + templateFileName;
            return new TemplatePathInfo
            {
                FullPath = fileRelativePath,
                FileName = templateFileName
            };
        }
    }
}
