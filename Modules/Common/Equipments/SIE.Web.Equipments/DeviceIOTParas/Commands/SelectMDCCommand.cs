using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.DeviceIOTParas;
using SIE.Equipments.DeviceIOTParas.Details;
using SIE.Equipments.DeviceIOTParas.Enums;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Equipments.DeviceIOTParas.Commands
{
    /// <summary>
    /// 保存MDC接口数据
    /// </summary>
    [JsCommand("SIE.Web.Equipments.DeviceIOTParas.Commands.SelectMDCCommand")]
    public class SelectMDCCommand : ViewCommand
    {
        /// <summary>
        /// 执行选择
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var physicalUnionList = args.Data.ToJsonObject<List<PhysicalUnion>>();

            Check.NotNullOrEmpty(physicalUnionList, nameof(physicalUnionList));
            if (physicalUnionList == null || physicalUnionList.Count == 0)
            {
                throw new ValidationException("{0}数据参数不能为空".L10nFormat(nameof(physicalUnionList)));
            }

            var deviceIOTPara = RF.GetById<DeviceIOTPara>(physicalUnionList[0].DeviceIOTParaId, new EagerLoadOptions().LoadWith(DeviceIOTPara.PhysicalUnionProperty));
            foreach (var item in deviceIOTPara.PhysicalUnion)
            {
                if (item.From == FromType.Interface)
                {
                    item.PersistenceStatus = PersistenceStatus.Deleted;
                }
            }

            //DB.Delete<PhysicalUnion>().Where(p => p.DeviceIOTParaId == physicalUnionList[0].DeviceIOTParaId);


            EntityList<PhysicalUnion> physicalUnion = new EntityList<PhysicalUnion>();
            physicalUnionList.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.New;
                physicalUnion.Add(p);
            });

            //删除之前的接口参数
            RF.Save(deviceIOTPara);
            //保存新接口参数
            RF.Save(physicalUnion);
            return true;
        }
    }
}
