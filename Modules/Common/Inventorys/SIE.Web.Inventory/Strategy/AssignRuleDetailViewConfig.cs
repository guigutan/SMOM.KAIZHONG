using SIE.Domain;
using SIE.Inventory.Strategy;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Inventory.Strategy.Commands;
using System.Collections.Generic;

namespace SIE.Web.Inventory.Strategy
{
    /// <summary>
    /// 分配规则明细视图配置
    /// </summary>
    internal class AssignRuleDetailViewConfig : WebViewConfig<AssignRuleDetail>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(AddAssignRuleDetailCommand).FullName, typeof(EditAssignRuleDetailCommand).FullName, typeof(DeleteAssignRuleDetailCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.LineNo).Readonly();
            View.Property(p => p.StorageArea).UseDataSource((e, c, r) =>
            {
                var assignRuleDetail = e as AssignRuleDetail;
                if (assignRuleDetail == null)
                    return new EntityList<StorageArea>();
                return RT.Service.Resolve<WarehouseController>().GetEnableStorageAreas(null, r, c);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(e.WarehouseName), nameof(e.StorageArea.WarehouseName));
                m.DicLinkField = dic;
            }).Readonly(p => p.IsDefault).HasLabel("库区");
            View.Property(p => p.WarehouseName).Readonly();
            View.Property(p => p.PickProcessType).Readonly(p => p.IsDefault).HasLabel("库位拣货处理");
            View.Property(p => p.WithLpnType).Readonly(p => p.IsDefault);
            View.Property(p => p.AutomatedStock).Readonly(p => p.IsDefault);
            View.Property(p => p.SpecialBasisStock).Readonly(p => p.IsDefault);
            View.Property(p => p.State).Readonly(p => p.IsDefault);
            View.Property(p => p.Sort1).Readonly(p => p.IsDefault).HasLabel("排序1");
            View.Property(p => p.Sort2).Readonly(p => p.IsDefault).HasLabel("排序2");
            View.Property(p => p.Sort3).Readonly(p => p.IsDefault).HasLabel("排序3");
            View.Property(p => p.LpnQtyMatchType).Readonly(p => p.IsDefault);
            View.Property(p => p.AssignBase).Cascade(p => p.PackageLevel, null).ShowInList();
            View.Property(p => p.PackageLevel).Readonly(p => p.AssignBase == AssignBase.AllowSplit);
        }
    }
}
