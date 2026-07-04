using SIE.Common.Schdules;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.Checks.Plans
{
    /// <summary>
    /// 点检计划自动生成调度参数
    /// </summary>
    [RootEntity, Serializable]
    public class CheckPlanCrtJobParameter : JobParameter
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<CheckPlanCrtJobParameter>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<CheckPlanCrtJobParameter>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 使用部门 UseDpt
        /// <summary>
        /// 使用部门Id
        /// </summary>
        [Label("使用部门")]
        public static readonly IRefIdProperty UseDptIdProperty =
            P<CheckPlanCrtJobParameter>.RegisterRefId(e => e.UseDptId, ReferenceType.Normal);

        /// <summary>
        /// 使用部门Id
        /// </summary>
        public double? UseDptId
        {
            get { return (double?)this.GetRefNullableId(UseDptIdProperty); }
            set { this.SetRefNullableId(UseDptIdProperty, value); }
        }

        /// <summary>
        /// 使用部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> UseDptProperty =
            P<CheckPlanCrtJobParameter>.RegisterRef(e => e.UseDpt, UseDptIdProperty);

        /// <summary>
        /// 使用部门
        /// </summary>
        public Enterprise UseDpt
        {
            get { return this.GetRefEntity(UseDptProperty); }
            set { this.SetRefEntity(UseDptProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<CheckPlanCrtJobParameter>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<CheckPlanCrtJobParameter>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<CheckPlanCrtJobParameter>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<CheckPlanCrtJobParameter>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

    }
}
