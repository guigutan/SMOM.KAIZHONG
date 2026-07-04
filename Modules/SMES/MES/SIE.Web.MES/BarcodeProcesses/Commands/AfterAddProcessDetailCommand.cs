using SIE.MES.BarcodeProcesses;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BarcodeProcesses.Commands
{
    /// <summary>
    /// 添加工序
    /// </summary>
    public class AfterAddProcessDetailCommand : ViewCommand
    {
        /// <summary>
        /// 实现
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<AddProcessDetailArgs>();
            return RT.Service.Resolve<BarcodeProcessController>().AfterAddProDetails(data.BarcodeId, data.ProcessIds);
        }
    }

    /// <summary>
    /// 添加工序命令参数
    /// </summary>
    public class AddProcessDetailArgs
    {
        /// <summary>
        /// 主表id
        /// </summary>
        public double? BarcodeId { get; set; }

        /// <summary>
        /// 工序ids
        /// </summary>
        public List<double?> ProcessIds { get; set; }
    }
}
