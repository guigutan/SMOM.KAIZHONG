using SIE.SO.SaleOrders.ImportSaleOrder;
using SIE.Web.Common.Import.Commands;
using SIE.Web.Pcb.SO.Common.Commands;
using System;

namespace SIE.Web.SO.SaleOrders.Commands
{
    /// <summary>
    /// 销售订单导入命令
    /// </summary>
    public class ImportSaleOrderCommand : ImportDataCommonCommand
    {
        /// <summary>
        /// 获取导入处理类型
        /// </summary>
        /// <returns></returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ImportSaleOrderHandle);
        }

        /// <summary>
        /// 设置模板文件信息
        /// </summary>
        /// <param name="importViewArgs"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override TemplatePathInfo SetTemplateFileName(ImportViewArgs importViewArgs, string scope)
        {
            string templateFileName = "销售订单导入模板.xlsx";
            var fileRelativePath = "Templates/" + templateFileName;
            return new TemplatePathInfo
            {
                FullPath = fileRelativePath,
                FileName = templateFileName
            };
        }
    }
}
