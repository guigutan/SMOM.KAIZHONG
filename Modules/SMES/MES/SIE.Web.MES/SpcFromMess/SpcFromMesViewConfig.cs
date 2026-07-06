using DocumentFormat.OpenXml.Wordprocessing;
using SIE.MES.SpcFromMess;
using SIE.MetaModel.View;
using SIE.Web.MES.SpcFromMesDetails;
using SIE.Web.MES.SpcFromMess.Commands;
using System;


namespace SIE.Web.MES.SpcFromMess
{
    /// <summary>
    /// 统计过程控制-视图配置
    /// </summary>
    public class SpcFromMesViewConfig : WebViewConfig<SpcFromMes>
    {

        /// <summary>
        /// ViewGroup视图---记录测量数据
        /// </summary>
        public const string RecordDetailView = nameof(RecordDetailView);

        /// <summary>
        /// ViewGroup视图---查看预览
        /// </summary>
        public const string ReadonlyView = nameof(ReadonlyView);



        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { RecordDetailView, ReadonlyView });
            switch (ViewGroup)
            {
                case RecordDetailView:
                    EditConfigView();
                    break;
                case ReadonlyView:
                    ReadonlyConfigView();
                    break;
                default:
                    // 默认视图配置
                    ConfigListView();
                    break;
            }
        }

        /// <summary>
        /// 视图配置---默认
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(RecordDetailViewSpcFromMesCommand).FullName);
            View.UseCommands(typeof(ReadonlyViewSpcFromMesCommand).FullName);
            View.UseDefaultCommands();
            View.UseCommands(WebCommandNames.Edit, WebCommandNames.Save);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);


            View.Property(p => p.No).ShowInList(width: 150);
            View.Property(p => p.Name).ShowInList(width: 150);
            View.Property(p => p.ItemId).ShowInList(width: 120);
            View.Property(p => p.ItemName).Readonly().Show();
            View.Property(p => p.Project).ShowInList(width: 150);
            View.Property(p => p.UnitId).ShowInList(width: 120);
            View.Property(p => p.UnitCode).Readonly().Show();
            View.Property(p => p.StandardUsl).ShowInList(width: 120);
            View.Property(p => p.StandardSl).ShowInList(width: 120);
            View.Property(p => p.StandardLsl).ShowInList(width: 120);
            View.Property(p => p.GroupCount).ShowInList(width: 100);
            View.Property(p => p.AverageUsl).ShowInList(width: 120);
            View.Property(p => p.AverageSl).ShowInList(width: 120);
            View.Property(p => p.AverageLsl).ShowInList(width: 120);
            View.Property(p => p.RangeUsl).ShowInList(width: 120);
            View.Property(p => p.RangeSl).ShowInList(width: 120);
            View.Property(p => p.RangeLsl).ShowInList(width: 120);
            View.Property(p => p.ProductionDept).ShowInList(width: 150);
            View.Property(p => p.ResourceId).ShowInList(width: 120);
            View.Property(p => p.ResourceName).Readonly().Show();
            View.Property(p => p.InspectorId).ShowInList(width: 120);
            View.Property(p => p.InspectorName).Readonly().Show();
            View.Property(p => p.ProcessAnalysis).ShowInList(width: 300);
            View.Property(p => p.ReasonsAndSolutions).ShowInList(width: 500);

            View.ChildrenProperty(p => p.SpcDetailList).Show(ChildShowInWhere.Hide);

        }

        /// <summary>
        /// 视图配置---记录测量数据
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void EditConfigView()
        {
            View.ClearCommands();

            View.UseDetail(6);

            //注意排序
            using (View.DeclareGroup("基础"))
            {
                View.Property(p => p.ItemName).Show(ShowInWhere.Detail).Readonly().HasOrderNo(10);
                View.Property(p => p.Project).Show(ShowInWhere.Detail).Readonly().HasOrderNo(20);
                View.Property(p => p.UnitCode).Show(ShowInWhere.Detail).Readonly().HasOrderNo(30);
            }
            using (View.DeclareGroup("标准"))
            {
                View.Property(p => p.StandardUsl).Show(ShowInWhere.Detail).Readonly().HasOrderNo(11);
                View.Property(p => p.StandardSl).Show(ShowInWhere.Detail).Readonly().HasOrderNo(21);
                View.Property(p => p.StandardLsl).Show(ShowInWhere.Detail).Readonly().HasOrderNo(31);

            }
            using (View.DeclareGroup("群组"))
            {
                View.Property(p => p.GroupCount).Show(ShowInWhere.Detail).Readonly().HasOrderNo(12);
                View.Property(p => p.Qty).Show(ShowInWhere.Detail).Readonly().HasOrderNo(22);
                View.Property(p => p.TotalQty).Show(ShowInWhere.Detail).Readonly().HasOrderNo(32);
            }
            using (View.DeclareGroup("均值"))
            {
                View.Property(p => p.AverageUsl).Show(ShowInWhere.Detail).Readonly().HasOrderNo(13);
                View.Property(p => p.AverageSl).Show(ShowInWhere.Detail).Readonly().HasOrderNo(23);
                View.Property(p => p.AverageLsl).Show(ShowInWhere.Detail).Readonly().HasOrderNo(33);

            }
            using (View.DeclareGroup("极差"))
            {
                View.Property(p => p.RangeUsl).Show(ShowInWhere.Detail).Readonly().HasOrderNo(14);
                View.Property(p => p.RangeSl).Show(ShowInWhere.Detail).Readonly().HasOrderNo(24);
                View.Property(p => p.RangeLsl).Show(ShowInWhere.Detail).Readonly().HasOrderNo(34);
            }
            using (View.DeclareGroup("对象"))
            {
                View.Property(p => p.ProductionDept).Show(ShowInWhere.Detail).Readonly().HasOrderNo(15);
                View.Property(p => p.ResourceName).Show(ShowInWhere.Detail).Readonly().HasOrderNo(25);
                View.Property(p => p.InspectorName).Show(ShowInWhere.Detail).Readonly().HasOrderNo(35);
            }

            //View.ChildrenProperty(p => p.SpcDetailList).Show(ChildShowInWhere.Hide);

            EditConfigViewChildren();
        }

        private void EditConfigViewChildren()
        {
            View.ChildrenProperty(p => p.SpcDetailList).Show(ChildShowInWhere.All).UseViewGroup(SpcFromMesDetailViewConfig.EditView).HasOrderNo(1000);            
        }

        /// <summary>
        /// 视图配置---查看预览
        /// </summary>
        private void ReadonlyConfigView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.GroupCount).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.StandardUsl).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.AverageUsl).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.RangeUsl).Show(ShowInWhere.Detail).Readonly();

                View.Property(p => p.Qty).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.StandardSl).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.AverageSl).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.RangeSl).Show(ShowInWhere.Detail).Readonly();

                View.Property(p => p.TotalQty).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.StandardLsl).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.AverageLsl).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.RangeLsl).Show(ShowInWhere.Detail).Readonly();


                View.Property(p => p.No).ShowInDetail( width:"20%", columnSpan:4).Readonly();
                View.Property(p => p.ItemName).ShowInDetail(width: "20%", columnSpan:4).Readonly();


                View.Property(p => p.Project).ShowInDetail(width: "50%", columnSpan:2).Readonly();
                View.Property(p => p.TotalSumX).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.Cpu).Show(ShowInWhere.Detail).Readonly();


                View.Property(p => p.ProductionDept).ShowInDetail(width: "50%", columnSpan: 2).Readonly();
                View.Property(p => p.TotalSumR).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.Cpl).Show(ShowInWhere.Detail).Readonly();


                View.Property(p => p.ResourceName).ShowInDetail(width: "50%", columnSpan: 2).Readonly();
                View.Property(p => p.TotalAvgX).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.Stdev).Show(ShowInWhere.Detail).Readonly();


                View.Property(p => p.InspectorName).ShowInDetail(width: "50%", columnSpan: 2).Readonly();
                View.Property(p => p.TotalAvgR).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.Cpk).Show(ShowInWhere.Detail).Readonly();


                View.Property(p => p.UnitCode).ShowInDetail(width: "50%", columnSpan: 2).Readonly();
                View.Property(p => p.Sigma).Show(ShowInWhere.Detail).Readonly();  
                View.Property(p => p.Ppk).Show(ShowInWhere.Detail).Readonly();

            }



            View.ChildrenProperty(p => p.SpcDetailList).Show(ChildShowInWhere.All).UseViewGroup(SpcFromMesDetailViewConfig.ReadonlyView).HasOrderNo(1000);
        }




















      
    }
}
