using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.Web.Barcodes.WipBatchs.Commands;

namespace SIE.Web.Barcodes.WipBatchs
{
    /// <summary>
    /// 生产批次视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class WipBatchViewConfig : WebViewConfig<WipBatch>
    {

        /// <summary>
        /// 在制品查询视图
        /// </summary>
        public const string WipProgressBatchView = "WipProgressBatchView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(WipProgressBatchView);
            if (ViewGroup == WipProgressBatchView)
                ConfigWipProgressBatchView();
        }

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
            return $"{me.StartSn}-{me.EndSn}";
        }
        #endregion

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands().UseCommands(typeof(ReprintBatchCommand).FullName, "SIE.Web.Barcodes.WipBatchs.Commands.DeleteWipBatchCommand");

            using (View.OrderProperties())
            {
                View.UseGridSelectionModel();
                View.Property(p => p.BatchNo).ShowInList(150).HasLabel("批次编码").Readonly();
                View.Property(SnRangeProperty).HasLabel("条码范围").ShowInList(300).Readonly();
                View.Property(p => p.Qty).HasLabel("批次数量").Readonly();
                View.Property(p => p.ScrapQty).HasLabel("报废数量").Readonly();
                View.Property(p => p.IsSuspectProduct).Show().Readonly();
                View.Property(p => p.IsRework).Show().Readonly();
                View.Property(p => p.IsScraped).Readonly();
                View.Property(p => p.BatchState).HasLabel("批次状态").UseEnumEditor().Readonly();
                View.Property(p => p.BoxesQty).HasLabel("满批数量").Readonly();
                View.Property(p => p.IsMantissa).UseCheckDropDownEditor().Readonly();
                View.Property(p => p.PrintDate).ShowInList(150).HasLabel("生成时间").Readonly();
                //View.Property(p => p.PrintTimes).ShowInList(150).Readonly();
                //View.Property(p => p.PrintByName).HasLabel("打印人").Readonly();
                //View.Property(p => p.PrintedState).HasLabel("打印状态").UseEnumEditor().Readonly();
                View.Property(p => p.ResourceCode).Readonly();
                View.Property(p => p.GenerateProcessCode).Readonly();
                View.Property(p => p.ProcessCode).Readonly();
                View.Property(p => p.SourceNo).ShowInList(150).Readonly();
                View.Property(p => p.IsUploadIot).Readonly();
                View.Property(p => p.IsOutsourcing).Show().Readonly();
                View.Property(p => p.IsUpload).Show().Readonly();
                //View.Property(p => p.ReportRecordIds).Readonly();
                View.ChildrenProperty(p => p.BatchList).HasLabel("子批次信息").Show(ChildShowInWhere.Hide);
                //View.AttachChildrenProperty(typeof(SubWipBatch), (o) =>
                //{
                //    var arg = o as ChildPagingDataArgs;
                //    WipBatch batch = o.Parent as WipBatch;
                //    if (batch == null)
                //        return new EntityList<SubWipBatch>();
                //    return RT.Service.Resolve<WipBatchController>().GetGenerateSubWipBatches(batch.Id, arg.SortInfo, arg.PagingInfo);
                //}, SubWipBatchViewConfig.SubBatchView).HasLabel("子批次信息");
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.BatchNo);
            View.Property(p => p.WorkOrder).HasLabel("工单编号");
        }


        /// <summary>
        /// 配置在制品查询视图
        /// </summary>
        protected void ConfigWipProgressBatchView()
        {
            View.DisableEditing().UseClientOrder();

            using (View.OrderProperties())
            {
                View.Property(p => p.BatchNo).ShowInList(200).HasLabel("批次标签").Readonly();
                View.Property(p => p.Qty).HasLabel("批次数量").Show().Readonly();
                //View.Property(p => p.ScrapQty).HasLabel("报废数量").Show().Readonly();
                View.Property(p => p.IsSuspectProduct).Show().Readonly();
                View.Property(p => p.IsScraped).Show().Readonly();
                //View.Property(p => p.BatchState).HasLabel("批次状态").UseEnumEditor().Show().Readonly();
                View.Property(p => p.WorkOrderNo).ShowInList(150).Readonly();
                View.Property(p => p.InProcessQty).Show().Readonly();
                View.Property(p => p.ReportQty).Show().Readonly();
                View.Property(p => p.PreReportQty).Show().Readonly();
                View.Property(p => p.ProductCode).ShowInList(150);
                View.Property(p => p.ProductName).ShowInList(200);
                View.Property(p => p.ShortDescription).ShowInList(150);

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);

                //View.Property(p => p.ReportRecordIds).Readonly();
                View.ChildrenProperty(p => p.BatchList).HasLabel("子批次信息").Show(ChildShowInWhere.Hide);

            }
        }
    }
}