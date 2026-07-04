using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SIE.EMS.SpareParts;

namespace SIE.EMS.AssetDisposals
{
    /// <summary>
    /// 资产处置单保存前事件
    /// </summary>
    [System.ComponentModel.DisplayName("资产处置单保存前事件")]
    [System.ComponentModel.Description("资产处置单保存前校验备件的序列号是否已存在于库存表中")]
    public class AssetDisposalSubmitting : OnSubmitting<AssetDisposal>
    {
        /// <summary>
        /// 资产处置单保存前事件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Invoke(AssetDisposal entity, EntitySubmittingEventArgs e)
        {
            if (e == null || entity == null)
            {
                return;
            }
            if ((e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update) && entity.AssetDisposalSparePartList.Any())
            {
                var snSparePartList = entity.AssetDisposalSparePartList.Where(p => p.Sn.IsNotEmpty()).ToList();
                var snList = snSparePartList.Select(p => p.Sn).Distinct().ToList();

                if (snSparePartList.Count != snList.Count)
                {
                    throw new ValidationException("备件回收清单中的【序列号】须唯一".L10N());
                }

                var newSnList = snSparePartList.Where(p => p.IsNew).Select(p => p.Sn).Distinct().ToList();
                var otherSparePartList = newSnList.SplitContains(tempSns =>
                {
                    return DB.Query<AssetDisposalSparePart>().Where(p => tempSns.Contains(p.Sn)).ToList();
                });

                if (otherSparePartList.Any()) 
                {
                    throw new ValidationException("备件回收清单中的【序列号】已存在于本处置单或其他处置单的回收清单中".L10N());
                }

                var itemSparePartList = entity.AssetDisposalSparePartList.Where(p => p.LotNo.IsNullOrEmpty() && p.Sn.IsNullOrEmpty()).ToList();
                var distSparePartList = itemSparePartList.Select(p => new { p.SparePartId, p.QualityStatus }).Distinct().ToList();

                if (itemSparePartList.Count != distSparePartList.Count)
                {
                    throw new ValidationException("备件回收清单中的【备件编码+质量状态】须唯一".L10N());
                }

                var newSparePartList = itemSparePartList.Where(p => p.IsNew).Select(p => new { p.SparePartId, p.QualityStatus }).Distinct().ToList();
                var existSparePartList = DB.Query<AssetDisposalSparePart>().Where(p => p.AssetDisposalId == entity.Id).ToList();

                newSparePartList.ForEach(item=> {
                    if (existSparePartList.Any(p => p.SparePartId == item.SparePartId && p.QualityStatus == item.QualityStatus))
                    {
                        throw new ValidationException("备件回收清单中的【备件编码+质量状态】须唯一".L10N());
                    }
                });

                var storeDetailList = entity.AssetDisposalSparePartList.Select(p => p.Sn).Distinct().SplitContains(tempSns =>
                {
                    return DB.Query<StoreSummaryDetail>().Where(p => tempSns.Contains(p.OrderNumberCode)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });

                if (storeDetailList.Any())
                {
                    var disposalSparePartList = entity.AssetDisposalSparePartList.Where(p => storeDetailList.Select(s => s.OrderNumberCode).Contains(p.Sn));

                    foreach (var disposalSparePart in disposalSparePartList)
                    {
                        var detail = storeDetailList.First(p => p.OrderNumberCode == disposalSparePart.Sn);

                        if (detail.SparePartId != disposalSparePart.SparePartId)
                        {
                            throw new ValidationException("序列号【{0}】为备件编码【{1}】的序列号，与选择的备件编码不符合".L10nFormat(detail.OrderNumberCode, detail.SparePartCode));
                        }
                        else
                        {
                            if (detail.StoreStatus != OrdNumStoreStatus.Out)
                            {
                                throw new ValidationException("序列号【{0}】的库存状态不为【出库】，不能回收".L10nFormat(detail.OrderNumberCode));
                            }
                        }
                    }
                }
            }
        }
    }
}
