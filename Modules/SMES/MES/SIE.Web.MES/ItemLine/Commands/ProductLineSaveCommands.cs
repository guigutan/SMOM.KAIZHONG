using IronPython.Compiler.Ast;
using Minio;
using SIE.CSM.ItemInspCharacteristicses;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.ItemLine;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemLine.Commands
{
    /// <summary>
    /// 产品与产线关系保存命令
    /// </summary>
    public class ProductLineSaveCommands : SaveCommand
    {
        /// <summary>
        /// 进行保存
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {

            if (data?.Count > 0)
            {
                foreach (var entity in data)
                {
                    if (entity is ProductLine charac)
                    {
                        //保存时重置时间和已执行批次数量
                        if (charac.PersistenceStatus == PersistenceStatus.Modified)
                        {
                            //if (charac.State == State.Enable)
                            //{
                            //    throw new ValidationException("可用状态不允许修改!!!");
                            //}
                        }

                        //double processId = 0;
                        //if (charac.ProcessId != null)
                        //    processId = (double)charac.ProcessId;

                        //if (RT.Service.Resolve<ProductLineController>().GetProductLineBool(charac.ItemId, processId, charac.WipResourceId))
                        //{
                        //    throw new ValidationException("相同的数据不允许保存!!!");
                        //}
                    }
                }
            }
            base.DoSave(data);
        }
    }
}
