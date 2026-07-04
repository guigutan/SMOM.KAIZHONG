using SIE.Domain;
using SIE.EMS.MainenanceProjects;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpecialEquipment.RegularInspections.Criterias
{
    /// <summary>
    /// 选择检验规程点检项目
    /// </summary>
    [QueryEntity, Serializable]
    [Label("选择检验规程点检项目")]
    public class SelInspectionProjectCriteria: Criteria
    {
        #region 设备型号 SpecialEquipmentAccountId
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号Id")]
        public static readonly Property<double?> SpecialEquipmentAccountIdProperty = P<SelInspectionProjectCriteria>.Register(e => e.SpecialEquipmentAccountId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? SpecialEquipmentAccountId
        {
            get { return GetProperty(SpecialEquipmentAccountIdProperty); }
            set { SetProperty(SpecialEquipmentAccountIdProperty, value); }
        }
        #endregion

        #region 检验规程 InspectionRuleId 
        /// <summary>
        /// 检验规程Id
        /// </summary>
        [Label("检验规程Id")]
        public static readonly Property<double?> InspectionRuleIdProperty = P<SelInspectionProjectCriteria>.Register(e => e.InspectionRuleId);

        /// <summary>
        /// 检验规程Id
        /// </summary>
        public double? InspectionRuleId
        {
            get { return GetProperty(InspectionRuleIdProperty); }
            set { SetProperty(InspectionRuleIdProperty, value); }
        }
        #endregion

        #region 项目名称 Name
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<SelInspectionProjectCriteria>.Register(e => e.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 项目类型 ProjectType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目类型")]
        public static readonly Property<ProjectType?> ProjectTypeProperty = P<SelInspectionProjectCriteria>.Register(e => e.ProjectType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public ProjectType? ProjectType
        {
            get { return GetProperty(ProjectTypeProperty); }
            set { SetProperty(ProjectTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取工治具需求清单列表
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<RegularInspectionController>().GetInspectionProjects(this);
        }
    }
}
