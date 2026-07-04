using SIE.LES.MaterialReceives;
using SIE.MetaModel.View;
using SIE.Web.LES.MaterialReceives.Commands;

namespace SIE.Web.LES.MaterialReceives
{
    /// <summary>
    /// 物料接收明细视图配置
    /// </summary>
    public class MaterialReceiveDetailViewConfig : WebViewConfig<MaterialReceiveDetail>
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
            View.UseCommands(typeof(ReceiveDetailCommand).FullName, typeof(RejectDetailCommand).FullName, typeof(PartReceiveDetailCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.State).ShowInList();
                View.Property(p => p.SoLineNo).ShowInList();
                View.Property(p => p.ItemCode).ShowInList(width: 100);
                View.Property(p => p.ItemName).ShowInList(width: 150);
                View.Property(p => p.ItemExtProp).Show();
                View.Property(p => p.ProjectNo).ShowInList();
                View.Property(p => p.IssuedQty).ShowInList();
                //View.Property(p => p.ReceivedQty).ShowInList();
                View.Property(p => p.ItemUnitName).Show();

                View.Property(p => p.CreateByName).Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).Show(ShowInWhere.All);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.All);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.LabelList).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 部分接收视图
        /// </summary>
        public void ConfigPartReceiveView()
        {

            View.Property(p => p.ReceivedQty).UseSpinEditor(p => { p.MinValue = 0; }).Show();

        }

    }
}
