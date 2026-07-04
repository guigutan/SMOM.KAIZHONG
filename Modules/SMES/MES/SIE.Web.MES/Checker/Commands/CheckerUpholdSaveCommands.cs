using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Checker;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Checker.Commands
{
    /// <summary>
    /// 检具保存命令
    /// </summary>
    public class CheckerUpholdSaveCommands : SaveCommand
    {
        /// <summary>
        /// 进行保存
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {

            //if (data?.Count > 0)
            //{
            //    foreach (var entity in data)
            //    {
            //        if (entity is CheckerUphold charac)
            //        {
            //            if (RT.Service.Resolve<CheckerUpholdController>().GetCheckerUpholdBool(charac.CheckerCode))
            //            {
            //                throw new ValidationException("检具编码已存在!");
            //            }
            //        }
            //    }
            //}
            base.DoSave(data);
        }
    }
}
