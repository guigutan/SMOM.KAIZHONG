using SIE.Common.ImportHelper;
using SIE.Items;
using SIE.Web.Common.Import.Commands;
using System;
using System.Data;

namespace SIE.Web.Items.ProductBoms.Commands
{
    /// <summary>
    /// 产品BOM明细导入命令
    /// </summary>
    public class ProductBomDetailImportCommand : ImportCommandBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ImportCompleted GetImportCompleted()
        {
            return (DataRow[] drSuccess, DataRow[] drFailed) =>
            {
            };
        }

        /// <summary>
        /// 获取导入处理类型
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override Type GetImportHandleType()
        {
            return typeof(ProductBomDetailHandle);
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
            const string templateFileName = "产品BOM明细导入模板.xlsx";
            const string fileRelativePath = "Templates/" + templateFileName;

            return new
            {
                FilePath = fileRelativePath,
                FileName = templateFileName
            };
        }
    }


}
