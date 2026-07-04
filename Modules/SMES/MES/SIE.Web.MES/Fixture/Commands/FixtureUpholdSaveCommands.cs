using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Fixture;
using SIE.MES.ItemLine;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Fixture.Commands
{
    /// <summary>
    /// 工装保存命令
    /// </summary>
    public class FixtureUpholdSaveCommands : SaveCommand
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
            //        if (entity is FixtureUphold charac)
            //        {
            //            if (RT.Service.Resolve<FixtureUpholdController>().GetFixtureUpholdBool(charac.FixtureCode))
            //            {
            //                throw new ValidationException("工装编码已存在!");
            //            }
            //        }
            //    }
            //}
            base.DoSave(data);
        }
    }
}
