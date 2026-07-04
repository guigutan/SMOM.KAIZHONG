using SIE.Domain;
using SIE.ProductIntfc.ProductStorages;
using SIE.Web.Command;
using System.Linq;
using System.Collections.Generic;
using SIE.ProductIntfc.OutputProducts;
using SIE.Tech.Routings.ViewModels;
using SIE.Domain.Validation;
using System;

namespace SIE.Web.ProductIntfc.OutputProducts.Commands
{
    /// <summary>
    /// 入库命令
    /// </summary>
    [JsCommand("SIE.Web.ProductIntfc.OutputProducts.Commands.ToStorageCommand")]
    public class ToStorageCommand : SaveCommand
    {
        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>入库结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {

            EntityList<OutputProductDetail> postDatas = args.Data.ToJsonObject<EntityList<OutputProductDetail>>();
            if (!postDatas.Any())
            {
                throw new ValidationException("请选择入库数据!".L10N());
            }
            RT.Service.Resolve<OutputProductController>().ToStorageIn(postDatas);
            return "入库成功".L10N();
        }

        protected override void DoSave(EntityList data)
        {
            base.DoSave(data);
        }
    }
}
