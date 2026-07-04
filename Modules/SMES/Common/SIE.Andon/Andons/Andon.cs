using SIE.Andon.Andons.Configs;
using SIE.Andon.Andons.Enum;
using SIE.Common.Configs;
using SIE.Common.Organizations;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯维护
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(AndonPushPlugConfig))]
    //[CriteriaQuery]
    [ConditionQueryType(typeof(AndonCriteria))]
    [DisplayMember(nameof(AndonName))]
    [Label("安灯维护")]
    public class Andon : DataEntity, IStateEntity
    {
        #region 快码
        /// <summary>
        /// 安灯维护优先级快码字符串
        /// </summary>
        public const string PriorityCatalogType = "ANDON_PRIORITY";
        #endregion

        #region 安灯编码 AndonCode
        /// <summary>
        /// 安灯编码
        /// </summary>
        [Required]
        [Label("安灯编码")]
        public static readonly Property<string> AndonCodeProperty = P<Andon>.Register(e => e.AndonCode);

        /// <summary>
        /// 安灯编码
        /// </summary>
        public string AndonCode
        {
            get { return this.GetProperty(AndonCodeProperty); }
            set { this.SetProperty(AndonCodeProperty, value); }
        }
        #endregion

        #region 安灯名称 AndonName
        /// <summary>
        /// 安灯名称
        /// </summary>
        [Required]
        [Label("安灯名称")]
        public static readonly Property<string> AndonNameProperty = P<Andon>.Register(e => e.AndonName);

        /// <summary>
        /// 安灯名称
        /// </summary>
        public string AndonName
        {
            get { return this.GetProperty(AndonNameProperty); }
            set { this.SetProperty(AndonNameProperty, value); }
        }
        #endregion

        #region 描述 Desc
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescProperty = P<Andon>.Register(e => e.Desc);

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc
        {
            get { return this.GetProperty(DescProperty); }
            set { this.SetProperty(DescProperty, value); }
        }
        #endregion

        #region 安灯类型 AndonType
        /// <summary>
        /// 安灯类型Id
        /// </summary>
        [Label("安灯类型")]
        public static readonly IRefIdProperty AndonTypeIdProperty =
            P<Andon>.RegisterRefId(e => e.AndonTypeId, ReferenceType.Normal);

        /// <summary>
        /// 安灯类型Id
        /// </summary>
        public double? AndonTypeId
        {
            get { return (double?)this.GetRefNullableId(AndonTypeIdProperty); }
            set { this.SetRefNullableId(AndonTypeIdProperty, value); }
        }

        /// <summary>
        /// 安灯类型
        /// </summary>
        public static readonly RefEntityProperty<AndonType> AndonTypeProperty =
            P<Andon>.RegisterRef(e => e.AndonType, AndonTypeIdProperty);

        /// <summary>
        /// 安灯类型
        /// </summary>
        public AndonType AndonType
        {
            get { return this.GetRefEntity(AndonTypeProperty); }
            set { this.SetRefEntity(AndonTypeProperty, value); }
        }
        #endregion

        #region 安灯大类 AndonClass
        /// <summary>
        /// 安灯大类
        /// </summary>
        [Required]
        [Label("安灯大类")]
        public static readonly Property<AndonTypeClass> AndonClassProperty = P<Andon>.Register(e => e.AndonClass);

        /// <summary>
        /// 安灯大类
        /// </summary>
        public AndonTypeClass AndonClass
        {
            get { return this.GetProperty(AndonClassProperty); }
            set { this.SetProperty(AndonClassProperty, value); }
        }
        #endregion

        #region 通用解决方案 Solution
        /// <summary>
        /// 通用解决方案
        /// </summary>
        [MaxLength(2000)]
        [Label("通用解决方案")]
        public static readonly Property<string> SolutionProperty = P<Andon>.Register(e => e.Solution);

        /// <summary>
        /// 通用解决方案
        /// </summary>
        public string Solution
        {
            get { return this.GetProperty(SolutionProperty); }
            set { this.SetProperty(SolutionProperty, value); }
        }
        #endregion

        #region 优先级 Priority
        /// <summary>
        /// 优先级
        /// </summary>
        [Label("优先级")]
        public static readonly Property<string> PriorityProperty = P<Andon>.Register(e => e.Priority);

        /// <summary>
        /// 优先级
        /// </summary>
        public string Priority
        {
            get { return this.GetProperty(PriorityProperty); }
            set { this.SetProperty(PriorityProperty, value); }
        }
        #endregion

        #region 序号 OrderNo
        /// <summary>
        /// 序号
        /// </summary>
        [MinValue(1)]
        [Label("序号")]
        public static readonly Property<int> OrderNoProperty = P<Andon>.Register(e => e.OrderNo);

        /// <summary>
        /// 序号
        /// </summary>
        public int OrderNo
        {
            get { return this.GetProperty(OrderNoProperty); }
            set { this.SetProperty(OrderNoProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<Andon>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 限制重复触发 RepeatTrigger
        /// <summary>
        /// 限制重复触发
        /// </summary>
        [Label("限制重复触发")]
        public static readonly Property<bool> RepeatTriggerProperty = P<Andon>.Register(e => e.RepeatTrigger);

        /// <summary>
        /// 限制重复触发
        /// </summary>
        public bool RepeatTrigger
        {
            get { return this.GetProperty(RepeatTriggerProperty); }
            set { this.SetProperty(RepeatTriggerProperty, value); }
        }
        #endregion

        #region 是否停线 LineStop
        /// <summary>
        /// 停线
        /// </summary>
        [Label("是否停线")]
        public static readonly Property<AndonYesOrNo> LineStopProperty = P<Andon>.Register(e => e.LineStop);

        /// <summary>
        /// 停线
        /// </summary>
        public AndonYesOrNo LineStop
        {
            get { return this.GetProperty(LineStopProperty); }
            set { this.SetProperty(LineStopProperty, value); }
        }
        #endregion

        #region 异常类型 DefaultType
        /// <summary>
        /// 异常类型
        /// </summary>
        [Label("异常类型")]
        public static readonly Property<string> DefaultTypeProperty = P<Andon>.Register(e => e.DefaultType);

        /// <summary>
        /// 异常类型
        /// </summary>
        public string DefaultType
        {
            get { return this.GetProperty(DefaultTypeProperty); }
            set { this.SetProperty(DefaultTypeProperty, value); }
        }
        #endregion

        #region 是否叫料 AskMaterial
        /// <summary>
        /// 叫料
        /// </summary>
        [Label("是否叫料")]
        public static readonly Property<AndonYesOrNo> AskMaterialProperty = P<Andon>.Register(e => e.AskMaterial);

        /// <summary>
        /// 叫料
        /// </summary>
        public AndonYesOrNo AskMaterial
        {
            get { return this.GetProperty(AskMaterialProperty); }
            set { this.SetProperty(AskMaterialProperty, value); }
        }
        #endregion

        #region 负责部门 Department
        /// <summary>
        /// 负责部门Id
        /// </summary>
        [Label("负责部门")]
        public static readonly IRefIdProperty DepartmentIdProperty =
            P<Andon>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 负责部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return (double?)this.GetRefId(DepartmentIdProperty); }
            set { this.SetRefId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 负责部门
        /// </summary>
        public static readonly RefEntityProperty<Organization> DepartmentProperty =
            P<Andon>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 负责部门
        /// </summary>
        public Organization Department
        {
            get { return this.GetRefEntity(DepartmentProperty); }
            set { this.SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 负责人 Charger
        /// <summary>
        /// 负责人Id
        /// </summary>
        [Label("负责人")]
        public static readonly IRefIdProperty ChargerIdProperty =
            P<Andon>.RegisterRefId(e => e.ChargerId, ReferenceType.Normal);

        /// <summary>
        /// 负责人Id
        /// </summary>
        public double? ChargerId
        {
            get { return (double?)this.GetRefId(ChargerIdProperty); }
            set { this.SetRefId(ChargerIdProperty, value); }
        }

        /// <summary>
        /// 负责人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ChargerProperty =
            P<Andon>.RegisterRef(e => e.Charger, ChargerIdProperty);

        /// <summary>
        /// 负责人
        /// </summary>
        public Employee Charger
        {
            get { return this.GetRefEntity(ChargerProperty); }
            set { this.SetRefEntity(ChargerProperty, value); }
        }
        #endregion

        //#region 推送模块 PushPlug
        ///// <summary>
        ///// 推送模块Id
        ///// </summary>
        //[Label("推送模块")]
        //public static readonly IRefIdProperty PushPlugIdProperty =
        //    P<Andon>.RegisterRefId(e => e.PushPlugId, ReferenceType.Normal);

        ///// <summary>
        ///// 推送模块Id
        ///// </summary>
        //public double? PushPlugId
        //{
        //    get { return (double?)this.GetRefNullableId(PushPlugIdProperty); }
        //    set { this.SetRefNullableId(PushPlugIdProperty, value); }
        //}

        ///// <summary>
        ///// 推送模块
        ///// </summary>
        //public static readonly RefEntityProperty<PushPlug> PushPlugProperty =
        //    P<Andon>.RegisterRef(e => e.PushPlug, PushPlugIdProperty);

        ///// <summary>
        ///// 推送模块
        ///// </summary>
        //public PushPlug PushPlug
        //{
        //    get { return this.GetRefEntity(PushPlugProperty); }
        //    set { this.SetRefEntity(PushPlugProperty, value); }
        //}
        //#endregion

        //#region 消息模板 MessageTemplate
        ///// <summary>
        ///// 消息模板
        ///// </summary>
        //[MaxLength(2000)]
        //[Label("消息模板")]
        //public static readonly Property<string> MessageTemplateProperty = P<Andon>.Register(e => e.MessageTemplate);

        ///// <summary>
        ///// 消息模板
        ///// </summary>
        //public string MessageTemplate
        //{
        //    get { return this.GetProperty(MessageTemplateProperty); }
        //    set { this.SetProperty(MessageTemplateProperty, value); }
        //}
        //#endregion

        #region 可疑品触发安灯 SuspectAndon
        /// <summary>
        /// 可疑品触发安灯
        /// </summary>
        [Label("可疑品触发安灯")]
        public static readonly Property<bool?> SuspectAndonProperty = P<Andon>.Register(e => e.SuspectAndon);

        /// <summary>
        /// 可疑品触发安灯
        /// </summary>
        public bool? SuspectAndon
        {
            get { return this.GetProperty(SuspectAndonProperty); }
            set { this.SetProperty(SuspectAndonProperty, value); }
        }
        #endregion


        #region 消息推送 MessageSendList
        /// <summary>
        /// 消息推送
        /// </summary>
        [Label("消息推送")]
        public static readonly ListProperty<EntityList<AndonMessageSend>> MessageSendListProperty = P<Andon>.RegisterList(e => e.MessageSendList);

        /// <summary>
        /// 消息推送
        /// </summary>
        public EntityList<AndonMessageSend> MessageSendList
        {
            get { return this.GetLazyList(MessageSendListProperty); }
        }
        #endregion

        #region 安灯清单 AndonSespList
        /// <summary>
        /// 安灯清单
        /// </summary>
        [Label("安灯清单")]
        public static readonly ListProperty<EntityList<AndonSesp>> AndonSespListProperty = P<Andon>.RegisterList(e => e.AndonSespList);

        /// <summary>
        /// 安灯清单
        /// </summary>
        public EntityList<AndonSesp> AndonSespList
        {
            get { return this.GetLazyList(AndonSespListProperty); }
        }
        #endregion

        #region 产前准备项目与安灯对应关系 AndonPrepareProjectDetailList
        /// <summary>
        /// 产前准备项目与安灯对应关系
        /// </summary>
        [Label("产前准备项目与安灯对应关系")]
        public static readonly ListProperty<EntityList<AndonPrepareProjectDetail>> AndonPrepareProjectDetailListProperty = P<Andon>.RegisterList(e => e.AndonPrepareProjectDetailList);

        /// <summary>
        /// 产前准备项目与安灯对应关系
        /// </summary>
        public EntityList<AndonPrepareProjectDetail> AndonPrepareProjectDetailList
        {
            get { return this.GetLazyList(AndonPrepareProjectDetailListProperty); }
        }
        #endregion

        #region 安灯责任组 AndonResponseDetailList
        /// <summary>
        /// 安灯责任组
        /// </summary>
        [Label("安灯责任组")]
        public static readonly ListProperty<EntityList<AndonResponseDetail>> AndonResponseDetailListProperty = P<Andon>.RegisterList(e => e.AndonResponseDetailList);

        /// <summary>
        /// 安灯责任组
        /// </summary>
        public EntityList<AndonResponseDetail> AndonResponseDetailList
        {
            get { return this.GetLazyList(AndonResponseDetailListProperty); }
        }
        #endregion

        #region 通用问题描述 GeneralProbDtlList
        /// <summary>
        /// 通用问题描述
        /// </summary>
        [Label("通用问题描述")]
        public static readonly ListProperty<EntityList<GeneralProbDtl>> GeneralProbDtlListProperty = P<Andon>.RegisterList(e => e.GeneralProbDtlList);

        /// <summary>
        /// 通用问题描述
        /// </summary>
        public EntityList<GeneralProbDtl> GeneralProbDtlList
        {
            get { return this.GetLazyList(GeneralProbDtlListProperty); }
        }
        #endregion


        #region 视图属性
        //#region 推送模板名称 PushPlugName
        ///// <summary>
        ///// 推送模板名称
        ///// </summary>
        //[Label("推送模板名称")]
        //public static readonly Property<string> PushPlugNameProperty = P<Andon>.RegisterView(e => e.PushPlugName, p => p.PushPlug.Name);

        ///// <summary>
        ///// 推送模板名称
        ///// </summary>
        //public string PushPlugName
        //{
        //    get { return this.GetProperty(PushPlugNameProperty); }
        //    set { this.SetProperty(PushPlugNameProperty, value); }
        //}
        //#endregion

        #region 负责部门名称 DepartmentName
        /// <summary>
        /// 负责部门名称
        /// </summary>
        [Label("负责部门名称")]
        public static readonly Property<string> DepartmentNameProperty = P<Andon>.RegisterView(e => e.DepartmentName, p => p.Department.Name);

        /// <summary>
        /// 负责部门名称
        /// </summary>
        public string DepartmentName
        {
            get { return this.GetProperty(DepartmentNameProperty); }
        }
        #endregion

        #region 安灯类型名称 AndonTypeName
        /// <summary>
        /// 安灯类型名称
        /// </summary>
        [Label("安灯类型名称")]
        public static readonly Property<string> AndonTypeNameProperty = P<Andon>.RegisterView(e => e.AndonTypeName, p => p.AndonType.AndonTypeName);

        /// <summary>
        /// 安灯类型名称
        /// </summary>
        public string AndonTypeName
        {
            get { return this.GetProperty(AndonTypeNameProperty); }
        }
        #endregion

        #endregion

    }

    /// <summary>
    /// 安灯实体配置
    /// </summary>
    public class AndonConfig : EntityConfig<Andon>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_ANDON").MapAllProperties();
            Meta.Property(Andon.SolutionProperty).ColumnMeta.HasLength(4000);
            //Meta.Property(Andon.MessageTemplateProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
