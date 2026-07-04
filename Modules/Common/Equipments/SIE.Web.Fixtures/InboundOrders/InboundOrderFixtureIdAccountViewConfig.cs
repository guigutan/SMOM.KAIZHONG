using SIE.Domain;
using SIE.Fixtures.InboundOrders;
using SIE.Warehouses;
using SIE.Web.Fixtures.InboundOrders.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Fixtures.InboundOrders
{
    /// <summary>
    /// ID类工治具台账视图
    /// </summary>
    public class InboundOrderFixtureIdAccountViewConfig : WebViewConfig<InboundOrderFixtureIdAccount>
    {
        /// <summary>
        /// 弹出页配置
        /// </summary>
        public const string IDLsitView = "IDLsitView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(InboundOrderFixtureIdAccountViewConfig.IDLsitView);
            if (ViewGroup == IDLsitView)
            {
                ConfigIDLsitView();
            }
        }

        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.FixtureIDAccount).Show().HasLabel("工治具ID");
            View.Property(p => p.Rfid).Show();
            View.Property(p => p.Qty).Show();
            View.Property(p => p.StorageLocation).Show().HasLabel("库位");
            View.Property(p => p.PoNo).Show().HasLabel("采购订单号");
            View.Property(p => p.Price).Show();
            View.Property(p => p.PoLineNo).Show().HasLabel("行号");
            View.Property(p => p.OriginalSerialNumber).Show();
            View.Property(p => p.AssetCode).Show();
            View.Property(p => p.ProductionDate).Show();
            View.Property(p => p.Manufacturer).Show();
            View.Property(p => p.MaintainTask).Show();
            View.Property(p => p.MaintainState).Show();
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置ID类列表
        /// </summary>

        protected void ConfigIDLsitView()
        {
            View.AddBehavior("SIE.Web.Fixtures.InboundOrders.Scripts.InboundOrdersChildListBehavior");
            View.UseCommand(typeof(OneKeyPassCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.IDCode).Show().Readonly();
                View.Property(p => p.Rfid).Show().Readonly();
                View.Property(p => p.Qty).Show().Readonly();
                View.Property(p => p.StorageLocation).UseDataSource((e, pagingInfo, keyword)=>
                {

                    var inboundOrderFixtureIdAccount = e as InboundOrderFixtureIdAccount;
                    if (inboundOrderFixtureIdAccount == null)
                    {
                        return new EntityList<StorageLocation>();
                    }
                    return RT.Service.Resolve<InboundOrderController>().GetStorageLocationList(inboundOrderFixtureIdAccount.InboundOrderId, pagingInfo, keyword);

                }).HasLabel("库位").Show();
                View.Property(p => p.PoNo).Show().HasLabel("采购订单号").Readonly();
                View.Property(p => p.Price).Show().Readonly();
                View.Property(p => p.PoLineNo).Show().HasLabel("行号").Readonly();
                View.Property(p => p.OriginalSerialNumber).Show().Readonly();
                View.Property(p => p.AssetCode).Show().Readonly();
                View.Property(p => p.ProductionDate).Show().Readonly();
                View.Property(p => p.Manufacturer).Show().Readonly();
                View.Property(p => p.MaintainTask).Show().Readonly();
                View.Property(p => p.MaintainState).Show().Readonly();
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
