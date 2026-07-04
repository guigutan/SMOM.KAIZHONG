using SIE.Domain;
using SIE.MES.TaskManagement.Specifications;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.MES.TaskManagement.Specifications.Commands
{
    /// <summary>
    /// 规格件选择
    /// </summary>
    public class SelectSpecificationCommand : ViewCommand
    {
        /// <summary>
        /// 执行选择
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find("SIE.MES.TaskManagement.Specifications.ProductSpecificationDetail");
            var savedData = RF.Find(meta.EntityType).NewList();
            var prodSpecificationDetails = args.Data.ToJsonObject<List<ProductSpecificationDetail>>();
            Check.NotNullOrEmpty(prodSpecificationDetails, nameof(prodSpecificationDetails));
            if (null == prodSpecificationDetails || prodSpecificationDetails.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(prodSpecificationDetails)));
            }
            foreach (var item in prodSpecificationDetails)
            {
                var prodSpecificationDetail = new ProductSpecificationDetail();
                prodSpecificationDetail.SpecificationId = item.SpecificationId;
                prodSpecificationDetail.ProductSpecificationId = item.ProductSpecificationId;
                prodSpecificationDetail.Qty = 1;
                savedData.Add(prodSpecificationDetail);
            }
            RF.Save(savedData);

            return true;
        }
    }
}
