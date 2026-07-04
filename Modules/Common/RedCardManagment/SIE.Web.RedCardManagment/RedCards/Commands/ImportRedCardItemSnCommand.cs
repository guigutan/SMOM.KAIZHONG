using SIE.Common.ImportHelper;
using SIE.Web.Common.Import.Commands;
using System;
using System.Data;

namespace SIE.Web.RedCardManagment.RedCards.Commands
{
    /// <summary>
    /// 数据导入-红牌物料SN
    /// </summary>
    class ImportRedCardItemSnCommand : ImportCommandBase
    {
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
            return typeof(SIE.RedCardManagment.RedCards.ImportRedCardItemSnHandle);
        }

    }
}
