using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Wpf.Items.ViewModels;
using System.Linq;

namespace SIE.Wpf.MES.WorkOrders
{
    /// <summary>
    /// 工单BOM视图配置
    /// </summary>
    internal class WorkOrderBomViewConfig : WPFViewConfig<WorkOrderBom>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(WorkOrderViewConfig.ReadonlyView);
            if (ViewGroup == WorkOrderViewConfig.ReadonlyView)
            {
                ReadOnlyView();
            }
            else
            {
                View.UseChildrenAsHorizontal();
                using (View.OrderProperties())
                {
                    View.Property(p => p.Item).HasLabel("物料编码").Show(ShowInWhere.All);
                    View.Property(p => p.ItemName).HasLabel("物料名称").Show(ShowInWhere.All);
                    View.Property(p => p.RequireQty).Show(ShowInWhere.All);
                    View.Property(p => p.SingleQty).Show(ShowInWhere.All);
                    View.Property(p => p.IsRecoilItem).Show(ShowInWhere.All);
                    View.Property(p => p.IsVritualItem).Show(ShowInWhere.All);
                    View.Property(p => p.IsByBill).Show(ShowInWhere.All);
                    View.Property(p => p.Remark).Show(ShowInWhere.All);
                    View.ChildrenProperty(p => p.PropertyValueList).Visible(false);
                    View.AttachChildrenProperty(typeof(PropertyValueViewModel), (o) =>
                    {
                        var bom = o.Parent as WorkOrderBom;
                        if (bom == null) return null;
                        var list = bom.LocalContext.GetPropertyOrDefault<EntityList<PropertyValueViewModel>>("WorkOrderBomValueList");
                        if (list == null)
                        {
                            var result = bom.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { DefinitionId = f.Key, Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.Bom).FirstOrDefault().GetType(), ParentId = f.Select(p => p.BomId).FirstOrDefault() });
                            list = new EntityList<PropertyValueViewModel>();
                            list.AddRange(result);
                            foreach (var value in list)
                                value.ItemId = bom.ItemId;
                            bom.LocalContext.SetExtendedProperty("WorkOrderBomValueList", list);
                        }

                        list.MarkSaved();
                        return list;
                    }, WorkOrderBomPropertyValueExtendViewConfig.WorkOrderBomPropertyValueExtendView).Show(ChildShowInWhere.All).HasLabel("工单BOM属性值");
                }
            }
        }

        /// <summary>
        /// 只读视图配置
        /// </summary>
        void ReadOnlyView()
        {
            View.DisableEditing();
            View.UseChildrenAsHorizontal();
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).Show(ShowInWhere.All);
                View.Property(p => p.ItemName).Show(ShowInWhere.All);
                View.Property(p => p.RequireQty).Show(ShowInWhere.All);
                View.Property(p => p.SingleQty).Show(ShowInWhere.All);
                View.Property(p => p.IsRecoilItem).Show(ShowInWhere.All);
                View.Property(p => p.IsVritualItem).Show(ShowInWhere.All);
                View.Property(p => p.IsByBill).Show(ShowInWhere.All);
                View.Property(p => p.Remark).Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.PropertyValueList).Visible(false);
                View.AttachChildrenProperty(typeof(PropertyValueViewModel), (o) =>
                {
                    var bom = o.Parent as WorkOrderBom;
                    if (bom == null) return null;
                    var list = bom.LocalContext.GetPropertyOrDefault<EntityList<PropertyValueViewModel>>("WorkOrderBomValueList");
                    if (list == null)
                    {
                        var result = bom.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { DefinitionId = f.Key, Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.Bom).FirstOrDefault().GetType(), ParentId = f.Select(p => p.BomId).FirstOrDefault() });
                        list = new EntityList<PropertyValueViewModel>();
                        list.AddRange(result);
                        foreach (var value in list)
                            value.ItemId = bom.ItemId;
                        bom.LocalContext.SetExtendedProperty("WorkOrderBomValueList", list);
                    }

                    list.MarkSaved();
                    return list;
                }, WorkOrderBomPropertyValueExtendViewConfig.WorkOrderBomPropertyValueReadonlyView).Show(ChildShowInWhere.All).HasLabel("工单BOM属性值");
            }
        }
    }
}
