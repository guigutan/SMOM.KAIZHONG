using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.FixtureRecords;
using SIE.Fixtures.Fixtures.Abnormals;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.InboundOrders;
using SIE.Fixtures.MaintainTasks;
using SIE.Fixtures.Models;
using SIE.Fixtures.Projects;
using SIE.Fixtures.Querys.ViewModels;
using SIE.Fixtures.Repairs;
using SIE.Fixtures.Warns;

namespace SIE.Web.Fixtures.Menus
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class FixturesMenuInit : IWebMenuInit
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
                TreeKey = "EDO",
                Label = "工装管理",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.工装管理",
                Label = "工治具异常类型",
                EntityType = typeof(FixtureAbnormal)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.工装管理",
                Label = "工治具类型",
                EntityType = typeof(SIE.Fixtures.FixtureTypes.FixtureType)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.工装管理",
                Label = "工治具保养项目",
                EntityType = typeof(MaintainProject)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.工装管理",
                Label = "工治具型号",
                EntityType = typeof(FixtureModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.工装管理",
                Label = "工治具库存台账",
                EntityType = typeof(FixtureAccountStock)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.工装管理",
                Label = "工治具编码",
                EntityType = typeof(FixtureEncode)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.工装管理",
                Label = "工治具台账",
                EntityType = typeof(FixtureAccountModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.工装管理",
                Label = "工治具入库",
                EntityType = typeof(InboundOrder)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.工装管理",
                Label = "工治具需求清单",
                EntityType = typeof(FixtureDemand)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.工装管理",
                Label = "工治具报修",
                EntityType = typeof(FixtureRepair)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.工装管理",
                Label = "工治具查询",
                EntityType = typeof(FixtureQueryViewModel)
            });
            //res.Add(new MenuDto()
            //{
            //    TreeKey = "EDO.工装管理",
            //    Label = "工治具保养预警",
            //    EntityType = typeof(FixtureWarn)
            //});
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.工装管理",
                Label = "工治具保养任务",
                EntityType = typeof(MaintainTask)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.工装管理",
                Label = "工治具出入库记录",
                EntityType = typeof(FixtureRecord)
            });

            return res;
        }

    }
}
