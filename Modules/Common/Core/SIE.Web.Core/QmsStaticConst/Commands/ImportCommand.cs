using SIE.Common.ImportHelper;
using SIE.Core.Import;
using SIE.Core.QmsStaticConst.Import;
using SIE.Web.Common.Import.Commands;
using SIE.Web.Core.Common.Commands.Import;
using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.Web.Core.QmsStaticConst.Commands
{
    /// <summary>
    /// Static常用参数导入命令
    /// </summary>
    public class ImportCommand : ImportMasterSubordinateDynamicCommand
    {
        
        /// <summary>
        /// 导入完成后处理
        /// </summary>
        /// <returns></returns>
        protected override ImportCompleted GetImportCompleted()
        {
            return (DataRow[] drSuccess, DataRow[] drFailed) =>
            {

            };
        }

        /// <summary>
        /// 获取导入处理者
        /// </summary>
        /// <returns></returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ImportStaticConstHandle);
        }

        /// <summary>
        /// 创建主从结构信息
        /// </summary>
        /// <returns></returns>
        protected override MasterSubordinateDynamicImportData CreateMasterSubordinateDynamicImportData()
        {
            List<string> subordinateSheetNameList = new List<string>() { "SPC常数表", "t分布数据表", "d2^常数表", "K1","K2","K3" };

            return new MasterSubordinateDynamicImportData() { MasterSheetName = ImportStaticConstHandle.ConstTabName, SubordinateSheetNameList = subordinateSheetNameList,AssociationColumnName="编码" };
        }


        /// <summary>
        /// 设置模板文件信息
        /// </summary>
        /// <param name="importViewArgs"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override TemplatePathInfo SetTemplateFileName(ImportViewArgs importViewArgs, string scope)
        {
            const string templateFileName = "统计系数表导入模板.xlsx";
            const string fileRelativePath = "Templates/" + templateFileName;
            return new TemplatePathInfo
            {
                FullPath = fileRelativePath,
                FileName = templateFileName
            };
        }
    }
}
