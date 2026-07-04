using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Packages.ItemLabels;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Packages.ItemLabels.Commands
{
    /// <summary>
    /// 物料标签删除
    /// </summary>
    public class ItemLabelDeleteCommand : ViewCommand<List<double>>
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>命令执行结果</returns>
        protected override object Excute(List<double> args, string scope)
        {
            if (args.Count == 0)
                throw new ValidationException("请选择要删除的数据".L10N());
            //var line = RF.GetById<ItemLabel>(args);
            //if (line == null) return false;
            ////if (line.State == State.Enable)
            ////{
            ////    throw new ValidationException("可用状态不允许删除!!!");
            ////}
            //line.PersistenceStatus = PersistenceStatus.Deleted;
            //RF.Save(line);
            var list = RT.Service.Resolve<ItemLabelController>().GetItemLabelsByIds(args);
            foreach (var item in list)
            {
                item.PersistenceStatus = PersistenceStatus.Deleted;
            }
            RF.Save(list);
            return true;
        }
    }
}
