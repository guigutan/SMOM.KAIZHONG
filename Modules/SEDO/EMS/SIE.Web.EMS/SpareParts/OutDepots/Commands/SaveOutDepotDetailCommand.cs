using SIE.EMS.SpareParts.OutDepots;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.Web.Command;

namespace SIE.Web.EMS.SpareParts.OutDepots.Commands
{
    /// <summary>
    /// 保存出库单
    /// </summary>
    public class SaveOutDepotDetailCommand : FormSaveCommand
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">命令参数</param>
        /// <returns>返回参数</returns>
        protected override object Excute(ViewArgs args, string scope) 
        {
            string jsonStr = args.Data.Replace("SIE.EMS.SpareParts.OutDepots.Details.OutDepotDetail", "OutDepotDetailList")
                                      .Replace("SIE.EMS.SpareParts.OutDepots.Details.PartOutDepotDetail", "PartOutDepotDetailList");
            args.Data = jsonStr;
            var entity = GetDeserializeData(args, scope)[0] as OutDepot;
            RT.Service.Resolve<OutDepotController>().SaveOutDepotBill(entity);
            return true;
        }
    }
}
