using SIE.EMS.Equipments.Boms;
using SIE.Equipments.EquipModels;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Equipments.Boms
{
    /// <summary>
    /// 设备BOM查询实体视图
    /// </summary>
    internal class EquipBomCriteriaViewConfig : WebViewConfig<EquipBomCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code).HasLabel("设备型号编码");
            View.Property(p => p.Name).HasLabel("设备型号名称");
            View.Property(p => p.EquipType).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<EquipModelController>().GetEquipTypes(p, k, true);
            });
            View.Property(p => p.CreateDateTime).UseDateRangeEditor(p => { p.DateRangeType = DateRangeType.All; p.Format = "Y-m-d"; }).Show(ShowInWhere.All);
        }
    }
}
