using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Andon;
using SIE.MES.Fixture;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Andon.Commands
{
    /// <summary>
    /// 安灯区域保存命令
    /// </summary>
    public class AndonUpholdSaveCommands : SaveCommand
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
            //        if (entity is AndonUphold charac)
            //        {
            //            if (RT.Service.Resolve<AndonUpholdController>().GetAndonUpholdBool(charac.AndonDesc,charac.AndonCode))
            //            {
            //                throw new ValidationException("数据已存在!");
            //            }
            //        }
            //    }
            //}
            base.DoSave(data);
        }
    }
}
