using SIE.DIST;
using SIE.Domain;
using SIE.Items.ViewModels;
using SIE.ManagedProperty;
using System.Linq;

namespace SIE.Wpf.DIST
{
    /// <summary>
    /// 配送管理视图配置 
    /// </summary>
    [CompiledPropertyDeclarer]
    internal class GoodsIssueViewConfig : WPFViewConfig<GoodsIssue>
    {
        #region 判断配送数量是否大于0 
        /// <summary>
        /// 配送数量是否大于0
        /// </summary>
        public static readonly Property<bool> IsHasDistributionQtyProperty = P<GoodsIssue>.RegisterExtensionReadOnly("IsHasDistributionQty", typeof(GoodsIssueViewConfig),
            GetIsHasDistributionQty, GoodsIssue.DistributionQtyProperty);

        /// <summary>
        /// 获取配送数量是否大于0
        /// </summary>
        /// <param name="me">工单发料信息</param>
        /// <returns>配送数据大于0返回true，否则返回false</returns>
        public static bool GetIsHasDistributionQty(GoodsIssue me)
        {
            return me.DistributionQty > 0;
        }
        #endregion

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(WPFCommandNames.ListAdd, typeof(GoodsIssueEditCommand), typeof(GoodsIssueDeleteCommand), WPFCommandNames.FormCopy, typeof(DistributionCommand));
            View.Property(p => p.ItemCode).HasLabel("物料编码");
            View.Property(p => p.ItemName).HasLabel("物料名称");
            View.Property(p => p.WorkOrderNo).HasLabel("工单");
            View.Property(p => p.SendNo);
            View.Property(p => p.BatchNo);
            View.Property(p => p.Unit);
            View.Property(p => p.Qty);
            View.Property(p => p.RemainderQty);
            View.Property(p => p.DistributionQty);
            View.Property(p => p.DefectQty);
            View.Property(p => p.NormalReturnQty);
            View.Property(p => p.DefectReturnQty);
            View.ChildrenProperty(p => p.PropertyValueList).Visible(false);
        }

        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.UseDefaultCommands().RemoveCommands(WPFCommandNames.FormDelete);
            View.ReplaceCommands(WPFCommandNames.FormSave, typeof(GoodsIssueSaveCommand));
            View.UseDetail(columnCount: 2);
            View.Property(p => p.Item);
            View.Property(p => p.ItemName).HasLabel("物料名称").Readonly(true);
            View.Property(p => p.WorkOrder).Readonly(IsHasDistributionQtyProperty);
            View.Property(p => p.SendNo).Readonly(IsHasDistributionQtyProperty);
            View.Property(p => p.BatchNo).Readonly(IsHasDistributionQtyProperty);
            View.Property(p => p.Unit).Readonly(false).Readonly(IsHasDistributionQtyProperty);
            View.Property(p => p.Qty).UseSpinEditor(e => e.MinValue = 1);
            View.ChildrenProperty(p => p.PropertyValueList).Visible(false);
            View.AttachChildrenProperty(typeof(PropertyValueViewModel), (o) =>
            {
                var goodsIssue = o.Parent as GoodsIssue;
                if (goodsIssue == null) return null;
                var list = goodsIssue.LocalContext.GetPropertyOrDefault<EntityList<PropertyValueViewModel>>("PropertyValueViewModel");
                if (list == null)
                {
                    var result = goodsIssue.PropertyValueList.GroupBy(p => p.DefinitionId)
                    .Select(f => new PropertyValueViewModel
                    {
                        DefinitionId = f.Key,
                        Values = f.Select(p => p.Value).ToList(),
                        Type = f.Select(p => p.GoodsIssue).FirstOrDefault().GetType(),
                        ParentId = f.Select(p => p.GoodsIssueId).FirstOrDefault()
                    });
                    list = new EntityList<PropertyValueViewModel>();
                    list.AddRange(result);
                    foreach (var value in list)
                        value.ItemId = goodsIssue.ItemId;
                    goodsIssue.LocalContext.SetExtendedProperty("PropertyValueViewModel", list);
                }

                list.MarkSaved();
                return list;
            }, GoodsIssuePropertyValueExtViewConfig.GoodsIssuePropertyValueExtView).Show(ChildShowInWhere.Detail).HasLabel("物料属性值");
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.WorkOrder.No).HasLabel("工单号").Readonly(false);
            View.Property(p => p.Item);
        }
    }
}
