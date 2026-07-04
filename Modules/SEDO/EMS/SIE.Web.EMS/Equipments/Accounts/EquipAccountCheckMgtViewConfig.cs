using SIE.Domain;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.SpareParts;
using SIE.Equipments.EquipAccounts;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Equipments.Accounts
{
    /// <summary>
	/// 设备台账 点检管理 视图配置
	/// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class EquipAccountCheckMgtViewConfig : WebViewConfig<EquipAccountCheckMgt>
    {
        /// <summary>
        /// 表单配置视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.EMS.Equipments.Scripts.EquipAccountTabBehavior");
            View.AttachChildrenProperty(typeof(EquipAccountCheckProject), (w) =>
                {
                    var args = w as ChildPagingDataArgs;
                    var parent = args.Parent.CastTo<EquipAccountCheckMgt>();
                    if (parent == null)
                        return new EntityList<EquipAccountCheckProject>();

                    var list = RT.Service.Resolve<EquipController>()
                        .GetEquipAccountCheckProjects(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                    return list;
                }, ViewConfig.ListView, directionParentPropertyName: EquipAccountExtension.CheckProjectListProperty.Name)
                .HasLabel("点检项目").HasOrderNo(1).Show(ChildShowInWhere.All);

            View.AttachChildrenProperty(typeof(EquipAccountPhysicalUnion), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<EquipAccountCheckMgt>();
                if (parent == null)
                    return new EntityList<EquipAccountPhysicalUnion>();

                var list = RT.Service.Resolve<EquipController>().GetEquipAccountPhysicalUnions(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return list;
            }, ViewConfig.ListView, directionParentPropertyName: EquipAccount.EquipAccountPhysicalUnionListProperty.Name)
                .HasLabel("物联参数").HasOrderNo(2).Show(ChildShowInWhere.All);
        }
    }
}
