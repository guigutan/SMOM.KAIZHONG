using SIE.Domain;
using SIE.EMS.Equipments.Models;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 备件选择查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("备件选择查询")]
    public partial class SparePartSelCriteria : Criteria
    {
        #region 编码 SparePartCode
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> SparePartCodeProperty = P<SparePartSelCriteria>.Register(e => e.SparePartCode);

        /// <summary>
        /// 编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
            set { this.SetProperty(SparePartCodeProperty, value); }
        }
        #endregion

        #region 名称 SparePartName
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> SparePartNameProperty = P<SparePartSelCriteria>.Register(e => e.SparePartName);

        /// <summary>
        /// 名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
            set { this.SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 设备型号 ModelCode
        /// <summary>
        /// 设备型号
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> ModelCodeProperty = P<SparePartSelCriteria>.Register(e => e.ModelCode);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
            set { this.SetProperty(ModelCodeProperty, value); }
        }
        #endregion

        #region 是否只读 IsReadOnly
        /// <summary>
        /// 是否只读
        /// </summary>
        [Label("IsReadOnly")]
        public static readonly Property<bool> IsReadOnlyProperty = P<SparePartSelCriteria>.Register(e => e.IsReadOnly);

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly
        {
            get { return this.GetProperty(IsReadOnlyProperty); }
            set { this.SetProperty(IsReadOnlyProperty, value); }
        }
        #endregion


        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<SparePartController>().GetSparePartList(this);
        }
    }
}
