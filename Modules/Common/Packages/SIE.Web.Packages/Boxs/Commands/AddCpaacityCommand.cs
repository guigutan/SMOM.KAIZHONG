using SIE.Domain;
using SIE.Packages.Boxs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Packages.Boxs.Commands
{
    /// <summary>
    /// 周转箱选择物料 命令
    /// </summary>
    [JsCommand("SIE.Web.Packages.Boxs.Commands.AddCpaacityCommand")]
    public class AddCpaacityCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">SIE.MetaModel.View.Block.EntityType</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var productCapacityList = args.Data.ToJsonObject<List<ProductCapacity>>();
            Check.NotNullOrEmpty(productCapacityList, nameof(productCapacityList));
            if (null == productCapacityList || productCapacityList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(productCapacityList)));
            }
            foreach (var item in productCapacityList)
            {
                var productCapacity = new ProductCapacity();
                productCapacity.ItemId = item.ItemId;
                productCapacity.TurnoverBoxId = item.TurnoverBoxId;
                productCapacity.Capacity = item.Capacity;
                savedData.Add(productCapacity);
            }
            RF.Save(savedData);
            return true;
        }


    }
}