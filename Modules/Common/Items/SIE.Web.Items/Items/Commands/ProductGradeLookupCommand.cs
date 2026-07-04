using SIE.Domain;
using SIE.Items.Items;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Items.Items.Commands
{
    /// <summary>
    /// 选择
    /// </summary>
    [JsCommand("SIE.Web.Items.Items.Commands.ProductGradeLookupCommand")]
    public class ProductGradeLookupCommand : ViewCommand
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
            var productGradeList = args.Data.ToJsonObject<List<ProductGrade>>();
            Check.NotNullOrEmpty(productGradeList, nameof(productGradeList));
            if (null == productGradeList || productGradeList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(productGradeList)));
            }
            foreach (var item in productGradeList)
            {
                var productGrade = new ProductGrade();
                productGrade.ItemId = item.ItemId;
                productGrade.Code = item.Code;
                productGrade.Name = item.Name;
                productGrade.Describe = item.Describe;
                savedData.Add(productGrade);
            }

            RF.Save(savedData);
            return true;
        }
    }
}
