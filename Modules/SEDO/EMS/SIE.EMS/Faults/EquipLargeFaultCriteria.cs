using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Faults
{
    /// <summary>
    /// 故障大类查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("故障大类查询实体")]
    public partial class EquipLargeFaultCriteria : Criteria
    {
        #region 大类编码 LargeCode
        /// <summary>
        /// 大类编码
        /// </summary>
        [Label("大类编码")]
        public static readonly Property<string> LargeCodeProperty = P<EquipLargeFaultCriteria>.Register(e => e.LargeCode);

        /// <summary>
        /// 大类编码
        /// </summary>
        public string LargeCode
        {
            get { return GetProperty(LargeCodeProperty); }
            set { SetProperty(LargeCodeProperty, value); }
        }
        #endregion

        #region 大类名称 LargeName
        /// <summary>
        /// 大类名称
        /// </summary>
        [Label("大类名称")]
        public static readonly Property<string> LargeNameProperty = P<EquipLargeFaultCriteria>.Register(e => e.LargeName);

        /// <summary>
        /// 大类名称
        /// </summary>
        public string LargeName
        {
            get { return GetProperty(LargeNameProperty); }
            set { SetProperty(LargeNameProperty, value); }
        }
        #endregion

        #region 中类编码 MiddleCode
        /// <summary>
        /// 中类编码
        /// </summary>
        [Label("中类编码")]
        public static readonly Property<string> MiddleCodeProperty = P<EquipLargeFaultCriteria>.Register(e => e.MiddleCode);

        /// <summary>
        /// 中类编码
        /// </summary>
        public string MiddleCode
        {
            get { return GetProperty(MiddleCodeProperty); }
            set { SetProperty(MiddleCodeProperty, value); }
        }
        #endregion

        #region 中类名称 MiddleName
        /// <summary>
        /// 中类名称
        /// </summary>
        [Label("中类名称")]
        public static readonly Property<string> MiddleNameProperty = P<EquipLargeFaultCriteria>.Register(e => e.MiddleName);

        /// <summary>
        /// 中类名称
        /// </summary>
        public string MiddleName
        {
            get { return GetProperty(MiddleNameProperty); }
            set { SetProperty(MiddleNameProperty, value); }
        }
        #endregion

        #region 小类编码 SmallCode
        /// <summary>
        /// 小类编码
        /// </summary>
        [Label("小类编码")]
        public static readonly Property<string> SmallCodeProperty = P<EquipLargeFaultCriteria>.Register(e => e.SmallCode);

        /// <summary>
        /// 小类编码
        /// </summary>
        public string SmallCode
        {
            get { return GetProperty(SmallCodeProperty); }
            set { SetProperty(SmallCodeProperty, value); }
        }
        #endregion

        #region 小类名称 SmallName
        /// <summary>
        /// 小类名称
        /// </summary>
        [Label("小类名称")]
        public static readonly Property<string> SmallNameProperty = P<EquipLargeFaultCriteria>.Register(e => e.SmallName);

        /// <summary>
        /// 小类名称
        /// </summary>
        public string SmallName
        {
            get { return GetProperty(SmallNameProperty); }
            set { SetProperty(SmallNameProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EquipFaultController>().QueryEquipLargeFault(this);
        }
    }
}
