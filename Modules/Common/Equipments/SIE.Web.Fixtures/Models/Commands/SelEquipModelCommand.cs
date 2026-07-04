using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Fixtures.Models;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Models.Commands
{
    /// <summary>
    /// 添加设备型号
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Models.Commands.SelEquipModelCommand")]
    public class SelEquipModelCommand : ViewCommand
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
            var equipDetails = args.Data.ToJsonObject<List<FixtureModelEquipDetail>>();
            Check.NotNullOrEmpty(equipDetails, nameof(equipDetails));
            if (null == equipDetails || equipDetails.Count == 0)
                throw new ValidationException("设备清单不能为空!".L10N());
            foreach (var item in equipDetails)
            {
                var equipDetail = new FixtureModelEquipDetail();
                equipDetail.FixtureModelId = item.FixtureModelId;
                equipDetail.EquipModelId = item.EquipModelId;
                savedData.Add(equipDetail);
            }
            RF.Save(savedData);
            return true;
        }
    }
}
