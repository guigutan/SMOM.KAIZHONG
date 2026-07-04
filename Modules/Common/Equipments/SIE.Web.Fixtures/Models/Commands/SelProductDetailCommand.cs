using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Fixtures.Models;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Models.Commands
{
    /// <summary>
    /// 添加产品清单
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Models.Commands.SelProductDetailCommand")]
    public class SelProductDetailCommand : ViewCommand
    {
        /// <summary>
        /// 执行添加
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var productDetails = args.Data.ToJsonObject<List<FixtureEncodeProductDetail>>();
            Check.NotNullOrEmpty(productDetails, nameof(productDetails));
            if (null == productDetails || productDetails.Count == 0)
                throw new ValidationException("产品清单不能为空!".L10N());
            foreach (var item in productDetails)
            {
                var equipDetail = new FixtureEncodeProductDetail();
                equipDetail.FixtureEncodeId = item.FixtureEncodeId;
                equipDetail.ItemId = item.ItemId;
                savedData.Add(equipDetail);
            }
            RF.Save(savedData);
            return true;
        }
    }
}
