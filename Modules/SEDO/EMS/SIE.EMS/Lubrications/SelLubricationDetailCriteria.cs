using SIE.Domain;
using SIE.ObjectModel;
using System;
namespace SIE.EMS.Lubrications
{
    /// <summary>
    /// 选择润滑项目
    /// </summary>
    [QueryEntity, Serializable]
    [Label("选择润滑项目")]
    public class SelLubricationDetailCriteria : Criteria
    {
        #region 设备台账Id EquipAccountId
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账Id")]
        public static readonly Property<double?> EquipAccountIdProperty = P<SelLubricationDetailCriteria>.Register(e => e.EquipAccountId);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return GetProperty(EquipAccountIdProperty); }
            set { SetProperty(EquipAccountIdProperty, value); }
        }
        #endregion

        #region 责任部门Id DepartmentId 
        /// <summary>
        /// 责任部门Id
        /// </summary>
        [Label("责任部门Id")]
        public static readonly Property<double?> DepartmentIdProperty = P<SelLubricationDetailCriteria>.Register(e => e.DepartmentId);

        /// <summary>
        /// 责任部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return GetProperty(DepartmentIdProperty); }
            set { SetProperty(DepartmentIdProperty, value); }
        }
        #endregion

        #region 项目名称 Name
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<SelLubricationDetailCriteria>.Register(e => e.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        /// <summary>
        ///
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<LubricationController>().GetLubricationDetail(this);
        }
    }
}
