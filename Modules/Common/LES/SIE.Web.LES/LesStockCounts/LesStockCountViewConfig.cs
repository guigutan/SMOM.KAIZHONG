using SIE.LES.LesStockCounts;
using SIE.MetaModel.View;
using SIE.Web.LES.LesStockCounts.Commands;

namespace SIE.Web.LES.LesStockCounts
{
    /// <summary>
    /// 盘点单视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class LesStockCountViewConfig : WebViewConfig<LesStockCount>
    {
        /// <summary>
        ///  扩展查看视图
        /// </summary>
        public const string ReadonlyView = "ReadOnlyView";

        /// <summary>
        /// 盘点状态等于创建可编辑
        /// </summary>
        private const string STATECREATECANEDIT = "盘点状态等于创建可编辑";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { ReadonlyView });
            View.AssignAuthorize(typeof(LesStockCount));
            if (ViewGroup == ReadonlyView)
            {
                ConfigReadOnlyView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(typeof(AddLesStockCountCommand).FullName, typeof(EditLesStockCountCommand).FullName, typeof(AduitLesCountFromCommand).FullName,
                typeof(FinishStockCountsCommand).FullName,typeof(DeleteLesStockCountCommand).FullName, typeof(CloseLesStockCountCommand).FullName, typeof(PrintLesStockCountCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.DefineFormChildSaveMode(FormChildSaveMode.Save);
            View.Property(p => p.No).ShowInList(width: 150).FixColumn()
                .UseListSetting(e => { e.HelpInfo = string.Format("根据{0}(配置项--{0})生成{1}单号", "单号生成规则", "盘点单"); });
            View.Property(p => p.OrderType);
            View.Property(p => p.State);
            View.Property(p => p.LesStockCountResult);
            View.Property(p => p.SourceType);
            View.Property(p => p.SourceBillNo);
            View.Property(p => p.AuditByName);
            View.Property(p => p.AuditDate).ShowInList(150);
            View.ChildrenProperty(p => p.LesStockCountRangeList).IsVisible(false);
            View.AttachDetailChildrenProperty(typeof(LesStockCountRange), c =>
            {
                var lesStockCount = c.Parent as LesStockCount;
                var range = RT.Service.Resolve<LesStockCountController>().GetLesStockCountRange(lesStockCount.Id);
                if (range == null)
                    range = new LesStockCountRange() { LesStockCountId = lesStockCount.Id };
                return range;
            }, LesStockCountRangeViewConfig.ReadonlyView).HasLabel("范围");
            View.ChildrenProperty(p => p.LesStockCountDetailList).UseViewGroup(LesStockCountDetailViewConfig.ReadonlyView).HasLabel("明细");
            View.ChildrenProperty(p => p.LesStockCountWorkOrderList).HasLabel("工单调账记录");
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(4);
            View.UseCommands(typeof(SaveLesCountFromCommand).FullName);
            View.UseCommands(typeof(AduitLesCountFromCommand).FullName);
            View.UseCommands(typeof(FinishCountCommand).FullName);
            View.UseCommands(typeof(CloseLesCountCommand).FullName);
            View.DefineFormChildSaveMode(FormChildSaveMode.NoSave);
            View.AddBehavior("SIE.Web.LES.LesStockCounts.Scripts.LesStockCountBehavior");
            View.Property(p => p.No).Readonly();
            View.Property(p => p.OrderType).Readonly();           
            View.Property(p => p.SourceType).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.SourceBillNo).Readonly(p => p.State != LesCountState.Create)
                .UseListSetting(e => { e.HelpInfo = STATECREATECANEDIT; });
            View.Property(p => p.LesStockCountResult).UseEnumEditor(p => p.AllowBlank = true).Readonly(true);
            View.ChildrenProperty(p => p.LesStockCountRangeList).IsVisible(false);
            View.AttachDetailChildrenProperty(typeof(LesStockCountRange), c =>
            {
                var stockCount = c.Parent as LesStockCount;
                var range = RT.Service.Resolve<LesStockCountController>().GetLesStockCountRange(stockCount.Id);
                return range;
            }, DetailsView, LesStockCount.LesStockCountRangeListProperty.Name).Show(ChildShowInWhere.All).HasLabel("范围");
            View.ChildrenProperty(p => p.LesStockCountDetailList).HasLabel("明细");
            View.ChildrenProperty(p => p.LesStockCountWorkOrderList).HasLabel("工单调账记录");
        }

        /// <summary>
        /// 查看视图
        /// </summary>
        private void ConfigReadOnlyView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly().Show();
                View.Property(p => p.OrderType).Readonly().Show();              
                View.Property(p => p.TaskLevel).Readonly().Show();               
                View.Property(p => p.State).Readonly().Show();
                View.Property(p => p.SourceBillNo).Readonly().Show();
                View.Property(p => p.LesStockCountResult).Readonly().Show();
                View.ChildrenProperty(p => p.LesStockCountRangeList).IsVisible(false);
                View.AttachDetailChildrenProperty(typeof(LesStockCountRange), c =>
                {
                    var lesStockCount = c.Parent as LesStockCount;
                    var range = RT.Service.Resolve<LesStockCountController>().GetLesStockCountRange(lesStockCount.Id);
                    return range;
                }, LesStockCountRangeViewConfig.ReadonlyView, LesStockCount.LesStockCountRangeListProperty.Name).Show(ChildShowInWhere.All).HasLabel("范围");
                View.ChildrenProperty(p => p.LesStockCountDetailList).UseViewGroup(LesStockCountDetailViewConfig.ReadonlyView).HasLabel("明细").Show(ChildShowInWhere.All);
            }
        }
    }
}
