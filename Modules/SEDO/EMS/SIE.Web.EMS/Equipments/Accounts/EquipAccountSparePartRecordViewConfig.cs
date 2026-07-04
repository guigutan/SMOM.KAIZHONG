using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.SpareParts;
using SIE.Web.EMS.SpareParts;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Equipments.Accounts
{
    /// <summary>
	/// 设备台账 备件记录 视图配置
	/// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class EquipAccountSparePartRecordViewConfig : WebViewConfig<EquipAccountSpPartRecord>
    {
        /// <summary>
        /// 表单配置视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.EMS.Equipments.Scripts.EquipAccountTabBehavior");
            View.AttachChildrenProperty(typeof(SparePartChangedRecord), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<EquipAccountSpPartRecord>();
                if (parent == null)
                    return new EntityList<SparePartChangedRecord>();

                var list = RT.Service.Resolve<SparePartController>().GetSparePartChangedRecords(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return list;
            }, ViewConfig.ListView).HasLabel("备件更换记录").HasOrderNo(1).Show(ChildShowInWhere.All);

            View.AttachChildrenProperty(typeof(SparePartChangedRecord), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<EquipAccountSpPartRecord>();
                if (parent == null)
                    return new EntityList<SparePartChangedRecord>();

                var list = RT.Service.Resolve<SparePartController>().GetSparePartChangedRecords(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo, true);
                return list;
            }, SparePartChangedRecViewConfig.SparePartAgeViewGroup).HasLabel("备件寿命监控").HasOrderNo(2).Show(ChildShowInWhere.All);
        }
    }
}
