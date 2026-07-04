using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Fixture;
using SIE.MES.ProcessProperty;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProcessProperty.Commands
{
    /// <summary>
    /// 工序属性保存命令
    /// </summary>
    public class ProcessPtySaveCommands:SaveCommand
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
            //        if (entity is ProcessPty charac)
            //        {
            //            if (RT.Service.Resolve<ProcessPtyController>().GetProcessPtyBool(charac.ProductLine,charac.Type,charac.ProductType,charac.ProcessId))
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
