using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialPreparations
{
    /// <summary>
    /// 备料需求单选择明细查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("备料需求单选择明细查询实体")]
    public class MaterialPreparationDtlSelCriteria : Criteria
    {
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<MaterialPreparationDtlSelCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 工单Id WoId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double> WoIdProperty = P<MaterialPreparationDtlSelCriteria>.Register(e => e.WoId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WoId
        {
            get { return this.GetProperty(WoIdProperty); }
            set { this.SetProperty(WoIdProperty, value); }
        }
        #endregion

        #region 备料类型 PreType
        /// <summary>
        /// 备料类型
        /// </summary>
        [Label("备料类型")]
        public static readonly Property<int> PreTypeProperty = P<MaterialPreparationDtlSelCriteria>.Register(e => e.PreType);

        /// <summary>
        /// 备料类型
        /// </summary>
        public int PreType
        {
            get { return this.GetProperty(PreTypeProperty); }
            set { this.SetProperty(PreTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<MaterialPreparationController>().GetMaterialPreparationDetailSelects(this);
        }

    }
}
