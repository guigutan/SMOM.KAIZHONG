using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Domain;
using SIE.Wpf.Barcodes.WipBatchs.Commands;
using System.Linq;

namespace SIE.Wpf.Barcodes.WipBatchs
{
    /// <summary>
    /// 生产批次视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class WipBatchViewConfig : WPFViewConfig<WipBatch>
    {
        #region 条码范围 SnRange
        /// <summary>
        /// 条码范围
        /// </summary>
        public static readonly Property<string> SnRangeProperty = P<WipBatch>.RegisterExtensionReadOnly("SnRange", typeof(WipBatchViewConfig),
            GetSnRangeProperty, WipBatch.RangeIdProperty);

        /// <summary>
        /// 条码范围
        /// </summary>
        /// <param name="me">条码</param>
        /// <returns>string</returns>
        public static string GetSnRangeProperty(WipBatch me)
        {
            if (me == null || me.Range == null)
                return string.Empty;
            return me.Range.StartSn + "-" + me.Range.EndSn;
        }
        #endregion

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands().UseCommands(typeof(ReprintBatchCommand));
            View.UseChildrenAsHorizontal();
            using (View.OrderProperties())
            {
                View.Property(p => p.BatchNo).HasLabel("批次编码").Show(ShowInWhere.All).FixColumn(MetaModel.View.ColumnFixedStyle.Left);
                View.Property(SnRangeProperty).HasLabel("条码范围").UseListSetting(e => e.ListGridWidth = 250).Show(ShowInWhere.All).FixColumn(MetaModel.View.ColumnFixedStyle.Left);
                View.Property(p => p.Qty).HasLabel("批次数量").Show(ShowInWhere.All);
                View.Property(p => p.BatchState).HasLabel("批次状态").UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.BoxesQty).HasLabel("满批数量").Show(ShowInWhere.All);
                View.Property(p => p.IsMantissa).UseCheckDropDownEditor().Show(ShowInWhere.All);
                View.Property(p => p.PrintDate).HasLabel("生成时间").Show(ShowInWhere.All);
                View.Property(p => p.PrintTimes).Show(ShowInWhere.All);
                View.Property(p => p.PrintByName).HasLabel("打印人").Show(ShowInWhere.All);
                View.Property(p => p.PrintedState).HasLabel("打印状态").UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.IsScraped).Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.BatchList).HasLabel("子批次信息").Show(ChildShowInWhere.Hide);
                View.AttachChildrenProperty(typeof(SubWipBatch), (o) =>
                {
                    WipBatch batch = o.Parent as WipBatch;
                    return batch.BatchList.Where(p => p.IsGenerate).AsEntityList();
                }, SubWipBatchViewConfig.SubBatchView).HasLabel("子批次信息");
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.BatchNo);
            View.Property(p => p.WorkOrder.No).HasLabel("工单编号");
        }
    }
}