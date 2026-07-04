using SIE.Common.ImportHelper;
using SIE.Items.ProductModels;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Data;

namespace SIE.Web.Items.ProductModels.Commands
{
    /// <summary>
    /// 机型技能导入
    /// </summary>
    [JsCommand("SIE.Web.Items.ProductModels.Commands.ProductModelSkillImportCommand")]
    public class ProductModelSkillImportCommand : ImportCommandBase
    {
        /// <summary>
        /// 获取导入完成后逻辑
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
        protected override Type GetImportHandleType()
        {
            return typeof(ProductModelSkillImportHandle);
        }
    }
}
