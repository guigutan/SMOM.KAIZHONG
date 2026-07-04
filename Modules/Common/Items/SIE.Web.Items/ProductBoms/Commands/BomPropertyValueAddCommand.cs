using SIE.Web.Command;
using SIE.Web.Items.ViewModels;

namespace SIE.Web.Items.ProductBoms.Commands
{
    /// <summary>
    /// 产品BOM属性添加命令
    /// </summary>
    public class BomPropertyValueAddCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    /// <summary>
    /// 之前代码，暂时废弃
    /// </summary>
    public class BomPropertyValueAddCommandViewArgs
    {
        /// <summary>
        /// 产品BOM Id
        /// </summary>
        public int ProductBomId { get; set; }

        /// <summary>
        /// 物料属性ViewModel
        /// </summary>
        public PropertyValueViewModel PropertyValueViewModel { get; set; }
    }

}
