using SIE.LES.MaterialReceives;

namespace SIE.Web.LES.MaterialReceives
{
    /// <summary>
    /// 物料接收标签明细视图配置
    /// </summary>
    public class MaterialReceiveLabelViewConfig : WebViewConfig<MaterialReceiveLabel>
    {
        /// <summary>
        /// 部分接收视图
        /// </summary>
        public readonly string PartReceiveView = "PartReceiveView";

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MaterialReceive));
            View.DeclareExtendViewGroup(PartReceiveView);
            if (ViewGroup == PartReceiveView)
            {
                ConfigPartReceiveView();
            }
        }
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseClientOrder();
            View.DisableEditing();
            View.UseCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.LabelNo).HasLabel("序列号").ShowInList(width: 130);
                View.Property(p => p.ItemCode).ShowInList(width: 100);
                View.Property(p => p.ItemName).ShowInList(width: 150);
                View.Property(p => p.ItemExtPropName).Show();
                View.Property(p => p.ProjectNo).Show();
                View.Property(p => p.LotCode).ShowInList(width: 130);
                View.Property(p => p.IssuedQty).Show();
                //View.Property(p => p.ReceivedQty).Show();
                View.Property(p => p.ItemUnitName).Show();
                View.Property(p => p.IsSerialNumber).Show();
                View.Property(p => p.IsMerge).Show();
                //View.Property(p => p.State).Show();

                View.Property(p => p.CreateByName).Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).Show(ShowInWhere.All);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.All);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.All);
            }
        }

        /// <summary>
        /// 部分接收视图
        /// </summary>
        public void ConfigPartReceiveView()
        {
            View.WithoutPaging();
            View.UseClientOrder();
            View.UseGridSelectionModel();
            using (View.OrderProperties())
            {
                View.Property(p => p.LabelNo).ShowInList(width: 130).Readonly();
                View.Property(p => p.ItemCode).ShowInList(width: 100).Readonly();
                View.Property(p => p.ItemName).ShowInList(width: 150).Readonly();
                View.Property(p => p.ItemExtProp).Show().Readonly();
                View.Property(p => p.LotCode).ShowInList(width: 130).Readonly();
                View.Property(p => p.ReceivedQty).HasLabel("接收数").Show().UseSpinEditor(p => { p.MinValue = 0; }).Readonly(p => p.IsSerialNumber);
                View.Property(p => p.IssuedQty).HasLabel("发料数").Show().Readonly();
                View.Property(p => p.ItemUnitName).Show().Readonly();
                View.Property(p => p.IsSerialNumber).Show().Readonly();
                View.Property(p => p.IsMerge).Show().Readonly();
                View.Property(p => p.SoNo).ShowInList(width: 130).Readonly();
                View.Property(p => p.SoLineNo).Show().Readonly();
            }

        }
    }
}
