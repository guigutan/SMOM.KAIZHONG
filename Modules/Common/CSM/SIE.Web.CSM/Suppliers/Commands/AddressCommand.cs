using SIE.CSM.Suppliers;
using SIE.CSM.Suppliers.Datas;
using SIE.Web.Command;

namespace SIE.Web.CSM.Suppliers.Commands
{
    /// <summary>
    /// 添加
    /// </summary>
    public class AddAddressCommand : ViewCommand
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
    /// 编辑
    /// </summary>
    public class EditAddressCommand : ViewCommand
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
    /// 添加供应商
    /// </summary>
    public class AddSupplierCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var rst = new SupplierConfigValue();
            rst = RT.Service.Resolve<SupplierController>().SetSupplierConfigValue();
            return rst;
        }
    }
}
