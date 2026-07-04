using SIE.Common.Menus;
using SIE.MES.PackingPrints;
using SIE.Wpf.MES.BatchWIP.Assemblys;
using SIE.Wpf.MES.BatchWIP.Inspects;
using SIE.Wpf.MES.BatchWIP.Moves;
using SIE.Wpf.MES.BatchWIP.Packings;
using SIE.Wpf.MES.BatchWIP.PackRecombine;
using SIE.Wpf.MES.BatchWIP.Repairs;
using SIE.Wpf.MES.OnOffDuty;
using SIE.Wpf.MES.TouchScreenHomepage;
using SIE.Wpf.MES.WIP.Assemblys;
using SIE.Wpf.MES.WIP.Inspects;
using SIE.Wpf.MES.WIP.Moves;
using SIE.Wpf.MES.WIP.NewPackages;
using SIE.Wpf.MES.WIP.Packings;
using SIE.Wpf.MES.WIP.PackRecombine;
using SIE.Wpf.MES.WIP.Repairs;
using SIE.Wpf.MES.WIP.Reworks;
using SIE.Wpf.MES.WIP.TemporaryRepairs;
using System.Collections.Generic;

namespace SIE.Wpf.MES
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class MesMenuInit : IWpfMenuInit
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
                Label = "MES(CS)",
                Sort = 0,
                Icon = "mes icon-mes",
                IsLeafNode = false,
            });

            res.Add(new MenuDto()
            {
                TreeKey = "MES(CS)",
                Label = "包装管理",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES(CS).包装管理",
                Label = "包装号打印",
                EntityType = typeof(PackingWorkOrder)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES(CS).包装管理",
                Label = "包装管理",
                EntityType = typeof(PackRecombineViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES(CS).包装管理",
                Label = "批次包装管理",
                EntityType = typeof(BatchPackRecombineViewModel)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "MES(CS)",
                Label = "生产采集",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES(CS)",
                Label = "触摸屏首页",
                EntityType = typeof(TouchScreenHomepageViewModel),
            });
            AddMesCollectionMenu(res);

            return res;
        }

        private static void AddMesCollectionMenu(List<MenuDto> res)
        {
            const string mesCollection = "MES(CS).生产采集";
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "过站采集",
                EntityType = typeof(MoveViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "检验采集",
                EntityType = typeof(InspectViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "检验项目采集",
                EntityType = typeof(InspectByItemViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "上料采集",
                EntityType = typeof(AssemblyViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "包装采集",
                EntityType = typeof(PackingViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "包装采集(正常)",
                EntityType = typeof(NewPackingViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "直接包装采集",
                EntityType = typeof(DirectPackingViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "新包装采集",
                EntityType = typeof(NewPackageViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "维修采集",
                EntityType = typeof(RepairViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "返工采集",
                EntityType = typeof(ReworkViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "批次过站采集",
                EntityType = typeof(BatchMoveViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "批次上料采集",
                EntityType = typeof(BatchAssemblyViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "批次检验采集",
                EntityType = typeof(BatchInspectViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "批次包装采集",
                EntityType = typeof(BatchPackingViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "批次维修采集",
                EntityType = typeof(BatchRepairViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "临时维修采集",
                EntityType = typeof(TemporaryRepairViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "上岗/下岗",
                EntityType = typeof(OnOffDutyViewModel)
            });
        }
    }
}
