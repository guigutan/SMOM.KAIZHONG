using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Kit.APS.FactoryPlanQtys
{
    /// <summary>
    /// 工厂计划数配置查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public  class FactoryPlanQtyCriteria : Criteria
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<FactoryPlanQtyCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id 
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)GetRefNullableId(FactoryIdProperty); }
            set { SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<FactoryPlanQtyCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>计划工艺模型列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<FactoryPlanQtyController>().GetFactoryPlanQtyList(this);
        }
    }
}
