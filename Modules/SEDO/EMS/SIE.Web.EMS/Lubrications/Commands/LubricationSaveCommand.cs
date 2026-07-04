using SIE.EMS.Lubrications;
using SIE.Web.Command;

namespace SIE.Web.EMS.Lubrications.Commands
{
    /// <summary>
    /// 添加保存
    /// </summary>
    internal class LubricationSaveCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存润滑记录
        /// </summary>
        /// <param name="stock">实体</param>
        protected void Saving(Lubrication stock)
        {
            RT.Service.Resolve<LubricationController>().SaveLubrication(stock);
            stock.MarkSaved();  // 保存修改状态为Unchanged，防止多次点击保存时违反唯一性约束    
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>命令执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            Lubrication order = args.Data.ToJsonObject<Lubrication>();
            Saving(order);
            return order;
        }
    }
}
