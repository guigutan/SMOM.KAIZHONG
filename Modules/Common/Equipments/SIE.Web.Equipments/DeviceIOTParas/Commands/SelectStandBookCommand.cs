using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.DeviceIOTParas.Controllers;
using SIE.Equipments.DeviceIOTParas.Details;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Equipments.DeviceIOTParas.Commands
{
    /// <summary>
    /// 选择
    /// </summary>
    [JsCommand("SIE.Web.Equipments.DeviceIOTParas.Commands.SelectStandBookCommand")]
    
    public class SelectStandBookCommand : ViewCommand
    { /// <summary>
      /// 执行选择
      /// </summary>
      /// <param name="args">args</param>
      /// <param name="scope">scope</param>
      /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var facilityDetailList = args.Data.ToJsonObject<List<FacilityDetail>>();
            Check.NotNullOrEmpty(facilityDetailList, nameof(facilityDetailList));
            if (facilityDetailList == null || facilityDetailList.Count == 0)
            {
                throw new ValidationException("设备清单不能为空".L10N());
            }
            EntityList<FacilityDetail> facilityDetail = new EntityList<FacilityDetail>();
            facilityDetailList.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.New;
                facilityDetail.Add(p);
            });

            var ids= facilityDetailList.Select(p=>p.EquipAccountId).ToList();
            RT.Service.Resolve<DeviceIOTParaController>().ExistEquipAccountByEAIdAsFD(ids);
            RF.Save(facilityDetail);
            return true;
        }
    }
}