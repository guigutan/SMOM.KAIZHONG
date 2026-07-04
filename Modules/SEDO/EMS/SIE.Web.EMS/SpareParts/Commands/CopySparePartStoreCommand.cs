using SIE.EMS.SpareParts;
using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.SpareParts.Commands
{
    /// <summary>
    /// 备件入库复制命令
    /// </summary>
    [JsCommand("SIE.Web.EMS.SpareParts.Commands.CopySparePartStoreCommand")]
    public class CopySparePartStoreCommand : ViewCommand
    {
        /// <summary>
        /// 执行复制操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>备件入库单号</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var para = args.Data.ToJsonObject<Para>();
            if (para.IsStoreCode)
            {
                return RT.Service.Resolve<SparePartController>().GetStoreCode();
            }
            else
            {
                return RT.Service.Resolve<SparePartController>().GetDetailCodes(para.DtlCount);
            }

        }
    }

    /// <summary>
    /// 参数
    /// </summary>
    [Serializable]
    public class Para
    {
        /// <summary>
        /// 是否入库单编码
        /// </summary>
        public bool IsStoreCode { get; set; }

        /// <summary>
        /// 明细数量
        /// </summary>
        public int DtlCount { get; set; }
    }
}
