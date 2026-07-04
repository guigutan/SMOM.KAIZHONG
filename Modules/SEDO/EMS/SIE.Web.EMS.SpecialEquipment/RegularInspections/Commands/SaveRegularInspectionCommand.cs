using SIE.EMS.SpecialEquipment.RegularInspections;
using SIE.Web.Command;

namespace SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands
{
    /// <summary>
    /// 保存特种设备定检
    /// </summary>
    [JsCommand("SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands.SaveRegularInspectionCommand")]
    public class SaveRegularInspectionCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存特种设备定检
        /// </summary>
        /// <param name="stock">实体</param>
        protected void Saving(RegularInspection stock)
        {
            if (stock == null)
            {
                return;
            }
            RT.Service.Resolve<RegularInspectionController>().CreateOrEditRegularInspection(stock);
            stock.MarkSaved();  // 保存特种设备定检修改状态为Unchanged，防止多次点击保存时违反唯一性约束    
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>命令执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null)
            {
                return null;
            }
            RegularInspection order = args.Data.ToJsonObject<RegularInspection>();
            Saving(order);
            return order;
        }
    }
}
