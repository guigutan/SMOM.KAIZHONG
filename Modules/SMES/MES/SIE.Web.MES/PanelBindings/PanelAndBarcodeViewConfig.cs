using SIE.MES.PanelBindings;

namespace SIE.Web.MES.PanelBindings
{
    /// <summary>
    /// 拼板码与条码关系视图配置
    /// </summary>
    internal class PanelAndBarcodeViewConfig : WebViewConfig<PanelAndBarcode>
    {
        /// <summary>
        /// MES工单条码绑定记录-绑定记录
        /// </summary>
        public const string BindingView = "BindingView";

        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(PanelBindingRecord));
            
            View.DeclareExtendViewGroup(BindingView);

            if (ViewGroup == BindingView)
                ConfigBindingView();

        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.PanelCode);
            View.Property(p => p.SN);
            View.Property(p => p.Qty);
            View.Property(p => p.PanelQty);
            View.Property(p => p.ForkPlateQty);
            View.Property(p => p.BindingQty);
            View.Property(p => p.BindingDate);
            View.Property(p => p.IsSplitPanel);
            View.Property(p => p.IsBindComplete);
            View.Property(p => p.Panel);
            View.Property(p => p.Operator);
            View.Property(p => p.WorkOrder);
            View.Property(p => p.Barcode);
        }

        /// <summary>
        /// MES工单条码绑定记录-绑定记录
        /// </summary>
        void ConfigBindingView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.SN).ShowInList(width: 150).Readonly();
                View.Property(p => p.Qty).Readonly().ShowInList();
                View.Property(p => p.PanelCode).ShowInList(width: 150).Readonly();
                View.Property(p => p.PanelQty).Readonly().ShowInList();
                View.Property(p => p.ForkPlateQty).Readonly().ShowInList();
                View.Property(p => p.BindingQty).Readonly().ShowInList();
                View.Property(p => p.BindingDate).ShowInList(width: 150).Readonly();
                View.Property(p => p.IsSplitPanel).ShowInList().Readonly();
                View.Property(p => p.IsBindComplete).ShowInList().Readonly();
                View.Property(p => p.Operator).HasLabel("操作人").Readonly().ShowInList();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}