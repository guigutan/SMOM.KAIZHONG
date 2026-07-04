using SIE.Common.ImportHelper;
using SIE.Warehouses;
using SIE.Warehouses.ImportHandles;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Data;

namespace SIE.Web.Warehouses.Commands
{
    /// <summary>
    /// 导入命令
    /// </summary>
    public class RoutewayImportCommand : ImportCommandBase
    {
        /// <summary>
        /// 导入命令
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
            return typeof(RoutewayImportHandle);
        }

    }

    /// <summary>
    /// 添加命令
    /// </summary>
    public class RoutewayAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<Routeway>();
            var code = RT.Service.Resolve<WarehouseController>().GetRoutewayCode();
            data.Code = code;          
            return data;
        }
    }     
}
