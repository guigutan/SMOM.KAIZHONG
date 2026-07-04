using SIE.MetaModel.View;
using SIE.Web.MES.Outsourcing.Commands;

namespace SIE.MES.Outsourcing
{
    /// <summary>
    /// 在制品委外出库
    /// </summary>
    public partial class ProcessingOutboundViewConfig : WebViewConfig<ProcessingOutbound>
    {
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(
                typeof(SyncToOtherFactoryCommand).FullName);
            View.Property(p => p.SN).ShowInList(width: 150).Readonly();
            View.Property(p => p.LotNo).ShowInList(width: 150).Readonly();
            View.Property(p => p.Qty).ShowInList(width: 100).UseSpinEditor(p => p.MinValue = 0).Readonly(p => p.SN != "" || p.LotNo != ""||p.State!= OutsourcingDetailState.Created);
            View.Property(p => p.State).ShowInList(width: 100).Readonly();
            View.Property(p => p.IsConfirm).Show().Readonly();
            View.Property(p => p.IsUpload).Show().Readonly();
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.SN).ShowInList(width: 150);
        }
    }

}