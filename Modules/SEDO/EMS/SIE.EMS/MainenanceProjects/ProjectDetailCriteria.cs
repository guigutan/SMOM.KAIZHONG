using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MainenanceProjects
{
    /// <summary>
    /// 点检保养项目查询实体
    /// </summary>
    [Label("点检保养项目查询实体")]
    [QueryEntity, Serializable]
    public class ProjectDetailCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectDetailCriteria()
        {
            ProjectType = null;
        }

        #region 项目名称 Name
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ProjectDetailCriteria>.Register(e => e.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<ProjectDetailCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 项目类型 ProjectType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目类型")]
        public static readonly Property<ProjectType?> ProjectTypeProperty = P<ProjectDetailCriteria>.Register(e => e.ProjectType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public ProjectType? ProjectType
        {
            get { return GetProperty(ProjectTypeProperty); }
            set { SetProperty(ProjectTypeProperty, value); }
        }
        #endregion

        #region 项目类型是否只读 IsReadonly
        /// <summary>
        /// 项目类型是否只读
        /// </summary>
        public static readonly Property<bool> IsReadonlyProperty = P<ProjectDetailCriteria>.Register(e => e.IsReadonly);

        /// <summary>
        /// 项目类型是否只读
        /// </summary>
        public bool IsReadonly
        {
            get { return GetProperty(IsReadonlyProperty); }
            set { SetProperty(IsReadonlyProperty, value); }
        }
        #endregion

        #region 周期类型 CycleType
        /// <summary>
        /// 周期类型
        /// </summary>        
        [Label("周期类型")]
        public static readonly Property<CycleType?> CycleTypeProperty = P<ProjectDetailCriteria>.Register(e => e.CycleType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public CycleType? CycleType
        {
            get { return GetProperty(CycleTypeProperty); }
            set { SetProperty(CycleTypeProperty, value); }
        }
        #endregion

        #region 部位 Part
        /// <summary>
        /// 部位
        /// </summary>
        [Label("部位")]
        public static readonly Property<string> PartProperty = P<ProjectDetailCriteria>.Register(e => e.Part);

        /// <summary>
        /// 部位
        /// </summary>
        public string Part
        {
            get { return GetProperty(PartProperty); }
            set { SetProperty(PartProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>点检保养项目列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProjectDetailController>().GetProjectDetails(this);
        }
    }
}
