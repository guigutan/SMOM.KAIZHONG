using SIE.EMS.SpareParts;
using SIE.Equipments.Enums;
using SIE.Web.Command;

namespace SIE.Web.EMS.SpareParts.Commands
{
    /// <summary>
    /// 提交入库单
    /// </summary>
    public class SubmitSparePartStoreCommand : FormSaveCommand
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">命令参数</param>
        /// <returns>返回参数</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string jsonStr = args.Data.Replace("SIE.EMS.SpareParts.StoreDetail", "StoreDetailList");
            args.Data = jsonStr;
            var entity = GetDeserializeData(args, scope)[0] as SparePartStore;

            if (entity.InboundType == SparePartInboundType.Scene)
            {
                RT.Service.Resolve<SparePartController>().WholeBillInStorage(entity);
            }
            else 
            {
                RT.Service.Resolve<SparePartController>().PartBillInStorage(entity);
            }
            
            return true;
        }
    }
}
