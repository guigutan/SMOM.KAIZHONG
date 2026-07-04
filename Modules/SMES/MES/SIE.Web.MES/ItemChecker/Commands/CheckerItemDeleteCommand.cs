using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.ItemChecker;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemChecker.Commands
{
    /// <summary>
    /// 检具与产品关系删除
    /// </summary>
    public class CheckerItemDeleteCommand : ViewCommand<double>
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>命令执行结果</returns>
        protected override object Excute(double args, string scope)
        {
            var line = RF.GetById<CheckerItem>(args);
            if (line == null) return false;
            //if (line.State == State.Enable)
            //{
            //    throw new ValidationException("可用状态不允许删除!!!");
            //}
            line.PersistenceStatus = PersistenceStatus.Deleted;
            RF.Save(line);
            return true;
        }
    }
}
