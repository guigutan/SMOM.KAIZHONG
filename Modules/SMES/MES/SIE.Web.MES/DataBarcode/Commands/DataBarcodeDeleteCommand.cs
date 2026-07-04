using SIE.Domain;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.DataBarcode.Commands
{
    /// <summary>
    /// 物料标签删除
    /// </summary>
    public class DataBarcodeDeleteCommand : ViewCommand<double>
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>命令执行结果</returns>
        protected override object Excute(double args, string scope)
        {
            var line = RF.GetById<SIE.MES.DataBarcode.DataBarcode>(args);
            if (line == null) return false;
            line.PersistenceStatus = PersistenceStatus.Deleted;
            RF.Save(line);
            return true;
        }
    }
}
