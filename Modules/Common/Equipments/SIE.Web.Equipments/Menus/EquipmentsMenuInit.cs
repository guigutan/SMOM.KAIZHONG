using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.Equipments.DeviceIOTParas;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipmentCards;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.Equipments.FinancialCategorys;

namespace SIE.Web.Equipments.Menus
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class EquipmentsMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();
            res.Add(new MenuDto()
            {
                Label = "EDO",
                Sort = 0,
                Icon = "edo icon-edo",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO",
                Label = "设备管理",
                IsLeafNode = false,
            });
            const string edoEquipmentManagement = "EDO.设备管理";
            res.Add(new MenuDto()
            {
                TreeKey = edoEquipmentManagement,
                Label = "设备类型维护",
                EntityType = typeof(EquipType)
            });
            res.Add(new MenuDto()
            {
                TreeKey = edoEquipmentManagement,
                Label = "设备型号维护",
                EntityType = typeof(EquipModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = edoEquipmentManagement,
                Label = "设备物联参数",
                EntityType = typeof(DeviceIOTPara)
            });
            res.Add(new MenuDto()
            {
                TreeKey = edoEquipmentManagement,
                Label = "设备台账维护",
                EntityType = typeof(EquipAccount)
            });
            res.Add(new MenuDto()
            {
                TreeKey = edoEquipmentManagement,
                Label = "设备立卡",
                EntityType = typeof(EquipmentCard)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "EDO",
                Label = "资产管理",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.资产管理",
                Label = "财务分类",
                EntityType = typeof(FinancialCategory)
            });

            return res;
        }

    }
}
