using SIE.EMS.InventoryPlans;
using SIE.EMS.InventoryTasks;
using SIE.EMS.InventoryTasks.ViewModels;
using SIE.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.InventoryTasks.ViewModels
{
    /// <summary>
    /// 工治具新增盘盈
    /// </summary>
    public class AddFixtureProfitViewModelViewConfig : WebViewConfig<AddFixtureProfitViewModel>
    {
        /// <summary>
        /// 配置明细
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(InventoryTask));
            View.UseDetail(4);
            View.Property(p => p.FixtureEncodeId).UseDataSource((e, c, r) =>
            {
                var entity = e as AddFixtureProfitViewModel;
                return RT.Service.Resolve<InventoryPlanController>().GetTaskFixtureEncodeList(c, entity, r);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.ModelCode), nameof(e.FixtureEncode.ModelCode));
                keyValues.Add(nameof(e.ModelName), nameof(e.FixtureEncode.ModelName));
                keyValues.Add(nameof(e.FixtureType), nameof(e.FixtureEncode.FixtureType));
                keyValues.Add(nameof(e.ManageMode), nameof(e.FixtureEncode.ManageMode));
                m.DicLinkField = keyValues;
            }).ShowInDetail();
            View.Property(p => p.ModelCode).Readonly();
            View.Property(p => p.ModelName).Readonly();
            View.Property(p => p.FixtureType).Readonly();
            View.Property(p => p.ManageMode).Readonly();
            View.Property(p => p.Online).Visibility(p => p.ManageMode == ManageMode.Code).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; });
            View.Property(p => p.StockQty).Visibility(p => p.ManageMode == ManageMode.Code).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; });
            View.Property(p => p.FixtureStatus).Visibility(p => p.ManageMode == ManageMode.Number);
            View.Property(p => p.GenerateSn).Visibility(p => p.ManageMode == ManageMode.Number);
            View.Property(p => p.Sn).Readonly(p => p.GenerateSn).Visibility(p => p.ManageMode == ManageMode.Number);
        }
    }
}
