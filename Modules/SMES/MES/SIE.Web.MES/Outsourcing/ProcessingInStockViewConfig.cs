using SIE.MES.Outsourcing;
using SIE.Web.MES.Outsourcing.Commands;

namespace SIE.Web.MES.Outsourcing
{
    /// <summary>
    /// 在制品委外入库视图配置
    /// </summary>
    public partial class ProcessingInStockViewConfig : WebViewConfig<ProcessingInStock>
    {
        const int SingleCharWidth = 10;
        /// <summary>
        /// 配置列表
        /// </summary>

        protected override void ConfigListView()
        {
            View.ClearCommands();
            //View.UseCommands(typeof(InStockAddCommand).FullName,
            //    typeof(InStockRecordAddCommand).FullName,
            //    "SIE.Web.MES.Outsourcing.Commands.InstockEditCommand",
            //    typeof(InStockSaveCommand).FullName,
            //    typeof(InStockDeleteCommand).FullName,
            //    typeof(InStockSubmitCommand).FullName);
            View.Property(p => p.SN).ShowInList(width: SingleCharWidth * 15).Readonly();
            View.Property(p => p.LotNo).ShowInList(width: SingleCharWidth * 15).Readonly();
            View.Property(p => p.Qty).UseSpinEditor(m=>m.MinValue=0).ShowInList(width: SingleCharWidth * 8)
                .Readonly(p => p.State == OutsourcingDetailState.Submitted);
            View.Property(p => p.ProcessingType).Show().Readonly();
            //View.Property(p => p.PassQty).UseSpinEditor(m => m.MinValue = 0).ShowInList(width: SingleCharWidth * 8)
            //    .Readonly(p => p.State == OutsourcingDetailState.Submitted);
            //View.Property(p => p.NgQty).UseSpinEditor(m => m.MinValue = 0).ShowInList(width: SingleCharWidth * 8)
            //    .Readonly(p => p.State == OutsourcingDetailState.Submitted);
            View.Property(p => p.State).ShowInList(width: SingleCharWidth * 8).Readonly();
        }

    }
}