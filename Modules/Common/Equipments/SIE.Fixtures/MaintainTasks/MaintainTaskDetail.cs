using SIE.Common;
using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.Fixtures.Projects;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.Fixtures.MaintainTasks
{
    /// <summary>
	/// 保养执行详情
	/// </summary>
	[ChildEntity, Serializable]
    [Label("保养执行详情")]
    public partial class MaintainTaskDetail : DataEntity
    {
        #region 检测值 CheckValue
        /// <summary>
        /// 检测值
        /// </summary>
        [Label("检测值")]
        public static readonly Property<decimal?> CheckValueProperty = P<MaintainTaskDetail>.Register(e => e.CheckValue);

        /// <summary>
        /// 检测值
        /// </summary>
        public decimal? CheckValue
        {
            get { return GetProperty(CheckValueProperty); }
            set { SetProperty(CheckValueProperty, value); }
        }
        #endregion

        #region 保养完成时间 FinishDate
        /// <summary>
        /// 保养完成时间
        /// </summary>
        [Label("保养完成时间")]
        public static readonly Property<DateTime?> FinishDateProperty = P<MaintainTaskDetail>.Register(e => e.FinishDate);

        /// <summary>
        /// 保养完成时间
        /// </summary>
        public DateTime? FinishDate
        {
            get { return GetProperty(FinishDateProperty); }
            set { SetProperty(FinishDateProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<MaintainTaskDetail>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 保养项目 MaintainProject
        /// <summary>
        /// 保养项目Id
        /// </summary>
        [Label("保养项目")]
        public static readonly IRefIdProperty MaintainProjectIdProperty = P<MaintainTaskDetail>.RegisterRefId(e => e.MaintainProjectId, ReferenceType.Normal);

        /// <summary>
        /// 保养项目Id
        /// </summary>
        public double MaintainProjectId
        {
            get { return (double)GetRefId(MaintainProjectIdProperty); }
            set { SetRefId(MaintainProjectIdProperty, value); }
        }

        /// <summary>
        /// 保养项目
        /// </summary>
        public static readonly RefEntityProperty<MaintainProject> MaintainProjectProperty = P<MaintainTaskDetail>.RegisterRef(e => e.MaintainProject, MaintainProjectIdProperty);

        /// <summary>
        /// 保养项目
        /// </summary>
        public MaintainProject MaintainProject
        {
            get { return GetRefEntity(MaintainProjectProperty); }
            set { SetRefEntity(MaintainProjectProperty, value); }
        }
        #endregion

        #region 保养人员 MaintainBy
        /// <summary>
        /// 保养人员Id
        /// </summary>
        [Label("保养人员")]
        public static readonly IRefIdProperty MaintainByIdProperty = P<MaintainTaskDetail>.RegisterRefId(e => e.MaintainById, ReferenceType.Normal);

        /// <summary>
        /// 保养人员Id
        /// </summary>
        public double? MaintainById
        {
            get { return (double?)GetRefNullableId(MaintainByIdProperty); }
            set { SetRefNullableId(MaintainByIdProperty, value); }
        }

        /// <summary>
        /// 保养人员
        /// </summary>
        public static readonly RefEntityProperty<Employee> MaintainByProperty = P<MaintainTaskDetail>.RegisterRef(e => e.MaintainBy, MaintainByIdProperty);

        /// <summary>
        /// 保养人员
        /// </summary>
        public Employee MaintainBy
        {
            get { return GetRefEntity(MaintainByProperty); }
            set { SetRefEntity(MaintainByProperty, value); }
        }
        #endregion

        #region 项目保养结果 MaintainResult
        /// <summary>
        /// 项目保养结果
        /// </summary>
        [Label("项目保养结论")]
        public static readonly Property<InspectionResult?> MaintainResultProperty = P<MaintainTaskDetail>.Register(e => e.MaintainResult);

        /// <summary>
        /// 项目保养结果
        /// </summary>
        public InspectionResult? MaintainResult
        {
            get { return GetProperty(MaintainResultProperty); }
            set { SetProperty(MaintainResultProperty, value); }
        }
        #endregion

        #region 保养任务 MaintainTask
        /// <summary>
        /// 保养任务Id
        /// </summary>
        public static readonly IRefIdProperty MaintainTaskIdProperty = P<MaintainTaskDetail>.RegisterRefId(e => e.MaintainTaskId, ReferenceType.Parent);

        /// <summary>
        /// 保养任务Id
        /// </summary>
        public double MaintainTaskId
        {
            get { return (double)GetRefId(MaintainTaskIdProperty); }
            set { SetRefId(MaintainTaskIdProperty, value); }
        }

        /// <summary>
        /// 保养任务
        /// </summary>
        public static readonly RefEntityProperty<MaintainTask> MaintainTaskProperty = P<MaintainTaskDetail>.RegisterRef(e => e.MaintainTask, MaintainTaskIdProperty);

        /// <summary>
        /// 保养任务
        /// </summary>
        public MaintainTask MaintainTask
        {
            get { return GetRefEntity(MaintainTaskProperty); }
            set { SetRefEntity(MaintainTaskProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 保养耗材 Consumable
        /// <summary>
        /// 保养耗材
        /// </summary>
        [Label("保养耗材")]
        public static readonly Property<string> ConsumableProperty = P<MaintainTaskDetail>.RegisterView(e => e.Consumable, p => p.MaintainProject.Consumable);

        /// <summary>
        /// 保养耗材
        /// </summary>
        public string Consumable
        {
            get { return this.GetProperty(ConsumableProperty); }
        }
        #endregion

        #region 耗材用量 ConsumableQty
        /// <summary>
        /// 耗材用量
        /// </summary>
        [Label("耗材用量")]
        public static readonly Property<decimal> ConsumableQtyProperty = P<MaintainTaskDetail>.RegisterView(e => e.ConsumableQty, p => p.MaintainProject.ConsumableQty);

        /// <summary>
        /// 耗材用量
        /// </summary>
        public decimal ConsumableQty
        {
            get { return this.GetProperty(ConsumableQtyProperty); }
        }
        #endregion

        #region 保养方法 Method
        /// <summary>
        /// 保养方法
        /// </summary>
        [Label("保养方法")]
        public static readonly Property<string> MethodProperty = P<MaintainTaskDetail>.RegisterView(e => e.Method, p => p.MaintainProject.Method);

        /// <summary>
        /// 保养方法
        /// </summary>
        public string Method
        {
            get { return this.GetProperty(MethodProperty); }
        }
        #endregion

        #region 保养工具 Tool
        /// <summary>
        /// 保养工具
        /// </summary>
        [Label("保养工具")]
        public static readonly Property<string> ToolProperty = P<MaintainTaskDetail>.RegisterView(e => e.Tool, p => p.MaintainProject.Tool);

        /// <summary>
        /// 保养工具
        /// </summary>
        public string Tool
        {
            get { return this.GetProperty(ToolProperty); }
        }
        #endregion

        #region 检测合格最小值 MinValue
        /// <summary>
        /// 检测合格最小值
        /// </summary>
        [Label("检测合格最小值")]
        public static readonly Property<decimal?> MinValueProperty = P<MaintainTaskDetail>.Register(e => e.MinValue);

        /// <summary>
        /// 检测合格最小值
        /// </summary>
        public decimal? MinValue
        {
            get { return this.GetProperty(MinValueProperty); }
            set { SetProperty(MinValueProperty, value); }
        }
        #endregion

        #region 检测合格最大值 MaxValue
        /// <summary>
        /// 检测合格最大值
        /// </summary>
        [Label("检测合格最大值")]
        public static readonly Property<decimal?> MaxValueProperty = P<MaintainTaskDetail>.Register(e => e.MaxValue);

        /// <summary>
        /// 检测合格最大值
        /// </summary>
        public decimal? MaxValue
        {
            get { return this.GetProperty(MaxValueProperty); }
            set { SetProperty(MaxValueProperty, value); }
        }
        #endregion

        #region 检验标识 CheckTag
        /// <summary>
        /// 检验标识
        /// </summary>
        [Label("检验标识")]
        public static readonly Property<CheckTag> CheckTagProperty = P<MaintainTaskDetail>.RegisterView(e => e.CheckTag, p => p.MaintainProject.CheckTag);

        /// <summary>
        /// 检验标识
        /// </summary>
        public CheckTag CheckTag
        {
            get { return this.GetProperty(CheckTagProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 保养执行详情 实体配置
    /// </summary>
    internal class MaintainTaskDetailConfig : EntityConfig<MaintainTaskDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_MAINTAIN_TASK_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
