using SIE.Domain;
using SIE.MES.TaskManagement.Specifications;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.MES.TaskManagement.Specifications.Commands
{
    /// <summary>
    /// 产品规格件物料选择
    /// </summary>
    public class SelectItemCommand : ViewCommand
    {
        /// <summary>
        /// 执行选择
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find("SIE.MES.TaskManagement.Specifications.ProductSpecification");
            var savedData = RF.Find(meta.EntityType).NewList();
            var productSpecifications = args.Data.ToJsonObject<List<ProductSpecification>>();
            Check.NotNullOrEmpty(productSpecifications, nameof(productSpecifications));
            if (null == productSpecifications || productSpecifications.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(productSpecifications)));
            }
            foreach (var item in productSpecifications)
            {
                var productSpecification = new ProductSpecification();
                productSpecification.ProductId = item.ProductId;
                savedData.Add(productSpecification);
            }

            RF.Save(savedData);
            return true;
        }
    }
}
