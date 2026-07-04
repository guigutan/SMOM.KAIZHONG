using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.EMS.Purchases.EquipmentAcceptances;
using SIE.EMS.Purchases.EquipmentInbounds;
using SIE.EMS.Purchases.EquipmentReceives;
using SIE.EMS.Purchases.EquipmentSetups;
using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.EMS.Purchases.FixtureReceives;
using SIE.EMS.Purchases.PaymentPlans;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.EMS.Purchases.SparePartAcceptances;
using SIE.EMS.Purchases.SparePartReceives;

namespace SIE.Web.EMS.Purchases
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class PurchasesMenuInit : IWebMenuInit
    {
        private const string edoPurchaseManagement = "EDO.采购管理";

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();

            res.Add(new MenuDto()
            {
                TreeKey = "EDO",
                Label = "采购管理",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = edoPurchaseManagement,
                Label = "资产采购申请",
                EntityType = typeof(PurchaseRequisition)
            });
            res.Add(new MenuDto()
            {
                TreeKey = edoPurchaseManagement,
                Label = "资产采购订单",
                EntityType = typeof(PurchaseOrder)
            });
            res.Add(new MenuDto()
            {
                TreeKey = edoPurchaseManagement,
                Label = "付款计划",
                EntityType = typeof(PaymentPlan)
            });
            res.Add(new MenuDto()
            {
                TreeKey = edoPurchaseManagement,
                Label = "设备接收",
                EntityType = typeof(EquipmentReceive)
            });
            res.Add(new MenuDto()
            {
                TreeKey = edoPurchaseManagement,
                Label = "设备开箱验收",
                EntityType = typeof(EquipmentAcceptance)
            });
            res.Add(new MenuDto()
            {
                TreeKey = edoPurchaseManagement,
                Label = "设备入库",
                EntityType = typeof(EquipmentInbound)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "EDO.备件管理",
                Label = "备件接收",
                EntityType = typeof(SparePartReceive)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.备件管理",
                Label = "备件验收",
                EntityType = typeof(SparePartAcceptance)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.备件管理",
                Label = "安装调试",
                EntityType = typeof(EquipmentSetup)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "EDO.工装管理",
                Label = "工治具接收",
                EntityType = typeof(FixtureReceive)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.工装管理",
                Label = "工治具验收",
                EntityType = typeof(FixtureAcceptance)
            });

            return res;
        }

    }
}
