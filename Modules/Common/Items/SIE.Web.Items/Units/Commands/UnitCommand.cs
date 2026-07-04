using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.Units;
using SIE.Web.Command;
using System;

namespace SIE.Web.Items.Units.Commands
{
    /// <summary>
    /// 保存
    /// </summary>
    public class SaveUnitCommand : SaveCommand
    {
        /// <summary>
        /// 保存前方法
        /// </summary>
        /// <param name="data">数据集合</param>
        protected override void OnSaving(EntityList data)
        {
            var units = data as EntityList<Unit>;
            foreach(var unit in units)
            {
                if (unit.Precision == null)
                {
                    unit.Precision = 0;
                }
            }
            base.OnSaving(units);
        }
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    public class DeleteUnitCommand : DeleteCommand
    {
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public class InitUnitCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {          
            if (RT.Service.Resolve<UnitsController>().CheckInitUnitList())
            {
                throw new ValidationException("已存在初始化单位,不能再初始化!".L10N());
            }
            else
            {
                RT.Service.Resolve<UnitsController>().InsertUnitList();
            }
            return true;
        }
    }
}
