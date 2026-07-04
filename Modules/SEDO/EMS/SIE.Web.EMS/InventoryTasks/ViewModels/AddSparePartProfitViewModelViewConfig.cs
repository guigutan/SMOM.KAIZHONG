using SIE.Domain;
using SIE.EMS.InventoryTasks;
using SIE.EMS.InventoryTasks.ViewModels;
using SIE.EMS.SpareParts.Enums;
using SIE.Warehouses;
using System.Collections.Generic;

namespace SIE.Web.EMS.InventoryTasks.ViewModels
{
    /// <summary>
    /// 工治具新增盘盈
    /// </summary>
    public class AddSparePartProfitViewModelViewConfig : WebViewConfig<AddSparePartProfitViewModel>
    {
        /// <summary>
        /// 配置明细
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(InventoryTask));
            View.UseDetail(2);
            View.Property(p => p.SparePartId).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.ControlMethod), nameof(e.SparePart.ControlMethod));
                keyValues.Add(nameof(e.SparePartCode), nameof(e.SparePart.SparePartCode));
                keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
                keyValues.Add(nameof(e.SpartType), nameof(e.SparePart.SpartType));
                keyValues.Add(nameof(e.ItemCategoryName), nameof(e.SparePart.ItemCategory.Name));
                keyValues.Add(nameof(e.Specification), nameof(e.SparePart.Specification));
                m.DicLinkField = keyValues;
            }).ShowInDetail();
            View.Property(p => p.SparePartName).Readonly();
            View.Property(p => p.ControlMethod).Readonly();
            View.Property(p => p.SpartType).Readonly();

            View.Property(p => p.ItemCategoryName).Readonly();
            View.Property(p => p.Specification).Readonly();            

            //批次管控
            View.Property(p => p.GenerateLotNo).Visibility(p => p.ControlMethod == ControlMethod.Batch).Cascade(x => x.LotNo, null);
            View.Property(p => p.LotNo).Visibility(p => p.ControlMethod == ControlMethod.Batch).Readonly(p => p.GenerateLotNo);

            //非序列号管控时显示
            View.Property(p => p.GoodQty).UseSpinEditor(p => p.MinValue = 0).Visibility(p => p.ControlMethod != ControlMethod.Sn);
            View.Property(p => p.NgQty).UseSpinEditor(p => p.MinValue = 0).Visibility(p => p.ControlMethod != ControlMethod.Sn);

            //序列号管控
            View.Property(p => p.GenerateSn).Visibility(p => p.ControlMethod == ControlMethod.Sn).Cascade(x => x.Sn, null);
            View.Property(p => p.Sn).Visibility(p => p.ControlMethod == ControlMethod.Sn).Readonly(p => p.GenerateSn);
            View.Property(p => p.IsGood).Visibility(p => p.ControlMethod == ControlMethod.Sn).Cascade(p => p.IsNg, p => !p.IsGood);
            View.Property(p => p.IsNg).Visibility(p => p.ControlMethod == ControlMethod.Sn).Cascade(p => p.IsGood, p => !p.IsNg);
        }
    }
}
