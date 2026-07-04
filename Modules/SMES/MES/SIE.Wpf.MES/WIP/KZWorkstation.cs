using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;

namespace SIE.Wpf.MES.WIP
{
    /// <summary>
    /// 工作站信息(KZ)
    /// </summary>
    [RootEntity, Serializable]
    public class KZWorkstation : ViewModel
    {

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public KZWorkstation(WorkCellViewModel workCellViewModel)
        {
            ProcessTypes = new List<ProcessType>();
            WorkCellViewModel = workCellViewModel;
        }
        #endregion



        #region User 人员
        /// <summary>
        /// 人员ID
        /// </summary>
        [Label("人员")]
        public static readonly IRefIdProperty EmployeeIdProperty =
            P<KZWorkstation>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 人员ID
        /// </summary>
        public double? EmployeeId
        {
            get { return (double?)this.GetRefNullableId(EmployeeIdProperty); }
            set { this.SetRefNullableId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 人员
        /// </summary>
        public static readonly RefEntityProperty<Employee> UserProperty =
            P<KZWorkstation>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 人员
        /// </summary>
        public Employee Employee
        {
            get { return this.GetRefEntity(UserProperty); }
            set { this.SetRefEntity(UserProperty, value); }
        }
        #endregion

        #region Resource 资源
        /// <summary>
        /// 资源ID
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<KZWorkstation>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源ID
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<KZWorkstation>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region Process 工序
        /// <summary>
        /// 工序ID
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<KZWorkstation>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序ID
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<KZWorkstation>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region Station 工位
        /// <summary>
        /// 工位ID
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<KZWorkstation>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位ID
        /// </summary>
        public double? StationId
        {
            get { return (double?)this.GetRefNullableId(StationIdProperty); }
            set { this.SetRefNullableId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty =
            P<KZWorkstation>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        ///  工位 
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 切换工作中心 ChangeWorkstation
        /// <summary>
        /// 切换工作中心(此属性是为了绑定【切换工作中心按钮】)
        /// </summary>
        [Label("切换工作中心")]
        public static readonly Property<string> ChangeWorkstationProperty = P<KZWorkstation>.Register(e => e.ChangeWorkstation);

        /// <summary>
        /// 切换工作中心
        /// </summary>
        public string ChangeWorkstation
        {
            get { return this.GetProperty(ChangeWorkstationProperty); }
            set { this.SetProperty(ChangeWorkstationProperty, value); }
        }
        #endregion

        /// <summary>
        /// 工序类型
        /// </summary>
        public List<ProcessType> ProcessTypes { get; private set; }

        /// <summary>
        /// 切换工作中心
        /// </summary>
        public bool ChangeWorkstationEvent()
        {
            return KZWorkstationSelector.SelectOperation(this, WorkCellViewModel.Workstation);
        }

        /// <summary>
        /// 采集功能基类
        /// </summary>
        public WorkCellViewModel WorkCellViewModel { get; set; }
    }

    /// <summary>
    /// 工作站 实体配置
    /// </summary>
    class KZWorkstationConfig : EntityConfig<KZWorkstation>
    {
        /// <summary>
        /// 增加实体验证规则
        /// </summary>
        /// <param name="rules">实体验证规则集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            //rules.Add(Workstation.UserProperty, new RequiredRule()
            //{
            //    MessageBuilder = e => "人员不能为空。".L10N()
            //});
            //rules.Add(Workstation.ProcessProperty, new RequiredRule()
            //{
            //    MessageBuilder = e => "工序不能为空。".L10N()
            //});
            //rules.Add(Workstation.StationProperty, new RequiredRule()
            //{
            //    MessageBuilder = e => "工位不能为空。".L10N()
            //});
            //rules.Add(Workstation.ResourceProperty, new RequiredRule()
            //{
            //    MessageBuilder = e => "资源不能为空。".L10N()
            //});
        }
    }
}
