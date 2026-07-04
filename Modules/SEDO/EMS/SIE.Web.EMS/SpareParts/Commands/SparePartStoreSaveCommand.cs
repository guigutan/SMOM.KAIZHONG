using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.SpareParts;
using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.SpareParts.Commands
{
    /// <summary>
    /// 保存
    /// </summary>
    [JsCommand("SIE.Web.EMS.SpareParts.Commands.SparePartStoreSaveCommand")]
    public class SparePartStoreSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存命令
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            foreach (SparePartStore parent in data)
            {
                var storeDetailList = parent.StoreDetailList;
                if (parent.StoreDetailList.Count == 0)
                    throw new ValidationException($"入库单号为[{parent.StoreCode}]入库明细不能为空".L10N());

                foreach (StoreDetail storeDetail in storeDetailList)
                {
                    if (storeDetail.SparePartId == 0)
                        throw new ValidationException($"请选择备件".L10N());
                    SparePart sparePart = RT.Service.Resolve<SparePartController>().GetSparePart(storeDetail.SparePartId);

                    if (sparePart.ControlMethod == SIE.EMS.SpareParts.Enums.ControlMethod.Sn)
                    {
                        if (storeDetail.Sn.IsNullOrEmpty())
                        {
                            throw new ValidationException("备件名称为[{0}]序列号管控模式，序列号列表不能为空".L10nFormat(sparePart.SparePartName));
                        }
                    }
                    else
                    {
                        if (storeDetail.Number == 0)
                        {
                            throw new ValidationException("备件名称为{0}数量不能为空".L10nFormat(sparePart.SparePartName));
                        }
                    }
                }
            }
            base.DoSave(data);
        }
    }
}
