using SIE.Domain;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using SIE.Tech.VictoryStandards;
using System;

namespace SIE.Tech.Routings.ViewModels
{
    /// <summary>
    /// 导入的工序明细
    /// </summary>
    [ChildEntity, Serializable]
    public partial class ProcessViewModel : ViewModel
    {
        #region 规则ID RuleId
        /// <summary>
        /// 规则ID
        /// </summary>
        [Label("规则ID")]
        public static readonly Property<string> RuleIdProperty = P<ProcessViewModel>.Register(e => e.RuleId);

        /// <summary>
        /// 规则ID
        /// </summary>
        public string RuleId
        {
            get { return this.GetProperty(RuleIdProperty); }
            set { this.SetProperty(RuleIdProperty, value); }
        }
        #endregion 

        #region 行号 RowNum
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<int> RowNumProperty = P<ProcessViewModel>.Register(e => e.RowNum);

        /// <summary>
        /// 行号
        /// </summary>
        public int RowNum
        {
            get { return this.GetProperty(RowNumProperty); }
            set { this.SetProperty(RowNumProperty, value); }
        }
        #endregion 

        #region 工序ID ProcessId
        /// <summary>
        /// 工序ID
        /// </summary>
        [Label("工序ID")]
        public static readonly Property<double> ProcessIdProperty = P<ProcessViewModel>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId
        {
            get { return this.GetProperty(ProcessIdProperty); }
            set { this.SetProperty(ProcessIdProperty, value); }
        }
        #endregion 

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("*工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ProcessViewModel>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 是否批次工序 IsBatch
        /// <summary>
        /// 是否批次工序
        /// </summary>
        [Label("是否批次工序")]
        public static readonly Property<bool> IsBatchProperty = P<ProcessViewModel>.Register(e => e.IsBatch);

        /// <summary>
        /// 是否批次工序
        /// </summary>
        public bool IsBatch
        {
            get { return this.GetProperty(IsBatchProperty); }
            set { this.SetProperty(IsBatchProperty, value); }
        }
        #endregion

        #region 工序类型 ProcessType
        /// <summary>
        /// 工序类型
        /// </summary>
        [Label("工序类型")]
        public static readonly Property<ProcessType> ProcessTypeProperty = P<ProcessViewModel>.Register(e => e.ProcessType);

        /// <summary>
        /// 工序类型
        /// </summary>
        public ProcessType ProcessType
        {
            get { return this.GetProperty(ProcessTypeProperty); }
            set { this.SetProperty(ProcessTypeProperty, value); }
        }
        #endregion

        #region 序列 SortOrder
        /// <summary>
        /// 序列
        /// </summary>
        [Label("序列")]
        public static readonly Property<int> SortOrderProperty = P<ProcessViewModel>.Register(e => e.SortOrder);

        /// <summary>
        /// 序列
        /// </summary>
        public int SortOrder
        {
            get { return this.GetProperty(SortOrderProperty); }
            set { this.SetProperty(SortOrderProperty, value); }
        }
        #endregion

        #region 返回序列 SortOrderBack
        /// <summary>
        /// 返回序列
        /// </summary>
        [Label("返回序列")]
        public static readonly Property<int> SortOrderBackProperty = P<ProcessViewModel>.Register(e => e.SortOrderBack);

        /// <summary>
        /// 返回序列
        /// </summary>
        public int SortOrderBack
        {
            get { return this.GetProperty(SortOrderBackProperty); }
            set { this.SetProperty(SortOrderBackProperty, value); }
        }
        #endregion

        #region 结果 Result
        /// <summary>
        /// 结果
        /// </summary>
        [Label("结果")]
        public static readonly Property<ResultTypeForDesign> ResultProperty = P<ProcessViewModel>.Register(e => e.Result);

        /// <summary>
        /// 结果
        /// </summary>
        public ResultTypeForDesign Result
        {
            get { return this.GetProperty(ResultProperty); }
            set { this.SetProperty(ResultProperty, value); }
        }
        #endregion 

        #region 结果描述 ResultDesc
        /// <summary>
        /// 结果描述
        /// </summary>
        [Label("结果描述")]
        public static readonly Property<string> ResultDescProperty = P<ProcessViewModel>.Register(e => e.ResultDesc);

        /// <summary>
        /// 结果描述
        /// </summary>
        public string ResultDesc
        {
            get { return this.GetProperty(ResultDescProperty); }
            set { this.SetProperty(ResultDescProperty, value); }
        }
        #endregion 

        #region 工序参数ID ParameterId
        /// <summary>
        /// 工序参数ID
        /// </summary>
        [Label("工序参数ID")]
        public static readonly Property<double> ParameterIdProperty = P<ProcessViewModel>.Register(e => e.ParameterId);

        /// <summary>
        /// 工序参数ID
        /// </summary>
        public double ParameterId
        {
            get { return this.GetProperty(ParameterIdProperty); }
            set { this.SetProperty(ParameterIdProperty, value); }
        }
        #endregion 

        #region 脚本 Script
        /// <summary>
        /// 脚本
        /// </summary>
        [Label("脚本")]
        public static readonly Property<string> FootScriptProperty = P<ProcessViewModel>.Register(e => e.Script);

        /// <summary>
        /// 脚本
        /// </summary>
        public string Script
        {
            get { return this.GetProperty(FootScriptProperty); }
            set { this.SetProperty(FootScriptProperty, value); }
        }
        #endregion

        #region 跳转条件 Condition
        /// <summary>
        /// 跳转条件
        /// </summary>
        [Label("跳转条件")]
        public static readonly Property<string> ConditionProperty = P<ProcessViewModel>.Register(e => e.Condition);

        /// <summary>
        /// 跳转条件
        /// </summary>
        public string Condition
        {
            get { return this.GetProperty(ConditionProperty); }
            set { this.SetProperty(ConditionProperty, value); }
        }
        #endregion


        #region 是否可选 CanChoose
        /// <summary>
        /// 是否可选
        /// </summary>
        [Label("是否可选")]
        public static readonly Property<bool?> CanChooseProperty = P<ProcessViewModel>.Register(e => e.CanChoose);

        /// <summary>
        /// 是否可选
        /// </summary>
        public bool? CanChoose
        {
            get { return this.GetProperty(CanChooseProperty); }
            set { this.SetProperty(CanChooseProperty, value); }
        }
        #endregion

        #region 是否重复 IsRepeat
        /// <summary>
        /// 是否重复
        /// </summary>
        [Label("是否重复")]
        public static readonly Property<bool?> IsRepeatProperty = P<ProcessViewModel>.Register(e => e.IsRepeat);

        /// <summary>
        /// 是否重复
        /// </summary>
        public bool? IsRepeat
        {
            get { return this.GetProperty(IsRepeatProperty); }
            set { this.SetProperty(IsRepeatProperty, value); }
        }
        #endregion

        #region 是否创建SKU IsCreateSKU 
        /// <summary>
        /// 是否创建SKU
        /// </summary>
        [Label("是否创建SKU")]
        public static readonly Property<bool?> IsCreateSkuProperty = P<ProcessViewModel>.Register(e => e.IsCreateSku);

        /// <summary>
        /// 是否创建SKU
        /// </summary>
        public bool? IsCreateSku
        {
            get { return this.GetProperty(IsCreateSkuProperty); }
            set { this.SetProperty(IsCreateSkuProperty, value); }
        }
        #endregion

        #region 是否计产工序 IsCalculate
        /// <summary>
        /// 是否计产工序
        /// </summary>
        [Label("是否计产")]
        public static readonly Property<bool?> IsCalculateProperty = P<ProcessViewModel>.Register(e => e.IsCalculate);

        /// <summary>
        /// 是否计产工序
        /// </summary>
        public bool? IsCalculate
        {
            get { return this.GetProperty(IsCalculateProperty); }
            set { this.SetProperty(IsCalculateProperty, value); }
        }
        #endregion

        #region 是否生成任务单 IsGenerateTask 
        /// <summary>
        /// 是否生成任务单
        /// </summary>
        [Label("是否生成任务单")]
        public static readonly Property<bool?> IsGenerateTaskProperty = P<ProcessViewModel>.Register(e => e.IsGenerateTask);

        /// <summary>
        /// 是否生成任务单
        /// </summary>
        public bool? IsGenerateTask
        {
            get { return this.GetProperty(IsGenerateTaskProperty); }
            set { this.SetProperty(IsGenerateTaskProperty, value); }
        }
        #endregion

        #region 是否需求任务清单 IsRequirementTask 
        /// <summary>
        /// 是否需求任务清单
        /// </summary>
        [Label("是否需求任务清单")]
        public static readonly Property<bool?> IsRequirementTaskProperty = P<ProcessViewModel>.Register(e => e.IsRequirementTask);

        /// <summary>
        /// 是否生成任务单
        /// </summary>
        public bool? IsRequirementTask
        {
            get { return this.GetProperty(IsRequirementTaskProperty); }
            set { this.SetProperty(IsRequirementTaskProperty, value); }
        }
        #endregion

        #region 是否扣料 IsBuckleMaterial
        /// <summary>
        /// 是否扣料
        /// </summary>
        [Label("是否扣料")]
        public static readonly Property<bool?> IsBuckleMaterialProperty = P<ProcessViewModel>.Register(e => e.IsBuckleMaterial);

        /// <summary>
        /// 是否扣料
        /// </summary>
        public bool? IsBuckleMaterial
        {
            get { return this.GetProperty(IsBuckleMaterialProperty); }
            set { this.SetProperty(IsBuckleMaterialProperty, value); }
        }
        #endregion

        #region 起始工序 StartProcess
        /// <summary>
        /// 起始工序
        /// </summary>
        [Label("起始工序")]
        public static readonly Property<double?> StartProcessProperty = P<ProcessViewModel>.Register(e => e.StartProcess);

        /// <summary>
        /// 起始工序
        /// </summary>
        public double? StartProcess
        {
            get { return this.GetProperty(StartProcessProperty); }
            set { this.SetProperty(StartProcessProperty, value); }
        }
        #endregion

        #region 正常胜制 NormalVictory
        /// <summary>
        /// 正常胜制Id
        /// </summary>
        [Label("正常胜制")]
        public static readonly IRefIdProperty NormalVictoryIdProperty =
            P<ProcessViewModel>.RegisterRefId(e => e.NormalVictoryId, ReferenceType.Normal);

        /// <summary>
        /// 正常胜制Id
        /// </summary>
        public double? NormalVictoryId
        {
            get { return (double?)this.GetRefNullableId(NormalVictoryIdProperty); }
            set { this.SetRefNullableId(NormalVictoryIdProperty, value); }
        }

        /// <summary>
        /// 正常胜制
        /// </summary>
        public static readonly RefEntityProperty<VictoryStandard> NormalVictoryProperty =
            P<ProcessViewModel>.RegisterRef(e => e.NormalVictory, NormalVictoryIdProperty);

        /// <summary>
        /// 正常胜制
        /// </summary>
        public VictoryStandard NormalVictory
        {
            get { return this.GetRefEntity(NormalVictoryProperty); }
            set { this.SetRefEntity(NormalVictoryProperty, value); }
        }
        #endregion 

        #region 维修胜制 RepairVictory
        /// <summary>
        /// 维修胜制Id
        /// </summary>
        [Label("维修胜制")]
        public static readonly IRefIdProperty RepairVictoryIdProperty =
            P<ProcessViewModel>.RegisterRefId(e => e.RepairVictoryId, ReferenceType.Normal);

        /// <summary>
        /// 维修胜制Id
        /// </summary>
        public double? RepairVictoryId
        {
            get { return (double?)this.GetRefNullableId(RepairVictoryIdProperty); }
            set { this.SetRefNullableId(RepairVictoryIdProperty, value); }
        }

        /// <summary>
        /// 维修胜制
        /// </summary>
        public static readonly RefEntityProperty<VictoryStandard> RepairVictoryProperty =
            P<ProcessViewModel>.RegisterRef(e => e.RepairVictory, RepairVictoryIdProperty);

        /// <summary>
        /// 维修胜制
        /// </summary>
        public VictoryStandard RepairVictory
        {
            get { return this.GetRefEntity(RepairVictoryProperty); }
            set { this.SetRefEntity(RepairVictoryProperty, value); }
        }
        #endregion

        #region 是否加严 IsStricter
        /// <summary>
        /// 是否加严
        /// </summary>
        [Label("加严")]
        public static readonly Property<bool> IsStricterProperty = P<ProcessViewModel>.Register(e => e.IsStricter);

        /// <summary>
        /// 是否加严
        /// </summary>
        public bool IsStricter
        {
            get { return this.GetProperty(IsStricterProperty); }
            set { this.SetProperty(IsStricterProperty, value); }
        }
        #endregion

        #region 超时时间（分钟） Overtime
        /// <summary>
        /// 超时时间（分钟）
        /// </summary>
        [Label("超时时间（分钟）")]
        public static readonly Property<int?> OvertimeProperty = P<ProcessViewModel>.Register(e => e.Overtime);

        /// <summary>
        /// 超时时间（分钟）
        /// </summary>
        public int? Overtime
        {
            get { return this.GetProperty(OvertimeProperty); }
            set { this.SetProperty(OvertimeProperty, value); }
        }
        #endregion
 
        #region 直通率取值 IsPassRate
        /// <summary>
        /// 直通率取值
        /// </summary>
        [Label("直通率取值")]
        public static readonly Property<bool?> IsPassRateProperty = P<ProcessViewModel>.Register(e => e.IsPassRate);

        /// <summary>
        /// 直通率取值
        /// </summary>
        public bool? IsPassRate
        {
            get { return this.GetProperty(IsPassRateProperty); }
            set { this.SetProperty(IsPassRateProperty, value); }
        }
        #endregion

        #region 绑定 IsBinding
        /// <summary>
        /// 绑定
        /// </summary>
        public static readonly Property<bool?> IsBindingProperty = P<ProcessViewModel>.Register(e => e.IsBinding);

        /// <summary>
        /// 绑定
        /// </summary>
        public bool? IsBinding
        {
            get { return this.GetProperty(IsBindingProperty); }
            set { this.SetProperty(IsBindingProperty, value); }
        }
        #endregion

        #region 解绑 IsUnBinding
        /// <summary>
        /// 解绑
        /// </summary>
        public static readonly Property<bool?> IsUnBindingProperty = P<ProcessViewModel>.Register(e => e.IsUnBinding);

        /// <summary>
        /// 解绑
        /// </summary>
        public bool? IsUnBinding
        {
            get { return this.GetProperty(IsUnBindingProperty); }
            set { this.SetProperty(IsUnBindingProperty, value); }
        }
        #endregion

        #region 层级 Level
        /// <summary>
        /// 层级
        /// </summary>
        [Label("层级")]
        public static readonly Property<int?> LevelProperty = P<ProcessViewModel>.Register(e => e.Level);

        /// <summary>
        /// 层级
        /// </summary>
        public int? Level
        {
            get { return this.GetProperty(LevelProperty); }
            set { this.SetProperty(LevelProperty, value); }
        }
        #endregion

        #region 已走过 IsPass
        /// <summary>
        /// 已走过
        /// </summary>
        [Label("已走过")]
        public static readonly Property<bool> IsPassProperty = P<ProcessViewModel>.Register(e => e.IsPass);

        /// <summary>
        /// 已走过
        /// </summary>
        public bool IsPass
        {
            get { return this.GetProperty(IsPassProperty); }
            set { this.SetProperty(IsPassProperty, value); }
        }
        #endregion

        #region 最大过站次数 MaxPassNum
        /// <summary>
        /// 最大过站次数
        /// </summary>
        [Label("最大过站次数")]
        public static readonly Property<int?> MaxPassNumProperty = P<ProcessViewModel>.Register(e => e.MaxPassNum);

        /// <summary>
        /// 最大过站次数
        /// </summary>
        public int? MaxPassNum
        {
            get { return this.GetProperty(MaxPassNumProperty); }
            set { this.SetProperty(MaxPassNumProperty, value); }
        }
        #endregion

        #region 是否下工序入站 IsNextMoveIn
        /// <summary>
        /// 是否下工序入站
        /// </summary>
        [Label("是否下工序入站")]
        public static readonly Property<bool?> IsNextMoveInProperty = P<ProcessViewModel>.Register(e => e.IsNextMoveIn);

        /// <summary>
        /// 是否下工序入站
        /// </summary>
        public bool? IsNextMoveIn
        {
            get { return this.GetProperty(IsNextMoveInProperty); }
            set { this.SetProperty(IsNextMoveInProperty, value); }
        }
        #endregion

        #region 是否委外 IsOutsourcing
        /// <summary>
        /// 是否委外
        /// </summary>
        [Label("是否委外")]
        public static readonly Property<bool> IsOutsourcingProperty = P<ProcessViewModel>.Register(e => e.IsOutsourcing);

        /// <summary>
        /// 是否委外
        /// </summary>
        public bool IsOutsourcing
        {
            get { return this.GetProperty(IsOutsourcingProperty); }
            set { this.SetProperty(IsOutsourcingProperty, value); }
        }
        #endregion

        #region 界面显示使用
        #region 序列-字符 StrSortOrder
        /// <summary>
        /// 序列-字符
        /// </summary>
        [Label("序列")]
        public static readonly Property<string> StrSortOrderProperty = P<ProcessViewModel>.Register(e => e.StrSortOrder);

        /// <summary>
        /// 序列-字符
        /// </summary>
        public string StrSortOrder
        {
            get { return this.GetProperty(StrSortOrderProperty); }
            set { this.SetProperty(StrSortOrderProperty, value); }
        }
        #endregion

        #region 返回序列-字符 StrSortOrderBack
        /// <summary>
        /// 返回序列-字符
        /// </summary>
        [Label("返回序列")]
        public static readonly Property<string> StrSortOrderBackProperty = P<ProcessViewModel>.Register(e => e.StrSortOrderBack);

        /// <summary>
        /// 返回序列-字符
        /// </summary>
        public string StrSortOrderBack
        {
            get { return this.GetProperty(StrSortOrderBackProperty); }
            set { this.SetProperty(StrSortOrderBackProperty, value); }
        }
        #endregion 

        #region 结果-字符 StrResult
        /// <summary>
        /// 结果-字符
        /// </summary>
        [Label("结果")]
        public static readonly Property<string> StrResultProperty = P<ProcessViewModel>.Register(e => e.StrResult);

        /// <summary>
        /// 结果-字符
        /// </summary>
        public string StrResult
        {
            get { return this.GetProperty(StrResultProperty); }
            set { this.SetProperty(StrResultProperty, value); }
        }
        #endregion

        #region 是否可选-字符 StrCanChoose
        /// <summary>
        /// 是否可选-字符
        /// </summary>
        [Label("是否可选")]
        public static readonly Property<string> StrCanChooseProperty = P<ProcessViewModel>.Register(e => e.StrCanChoose);

        /// <summary>
        /// 是否可选-字符
        /// </summary>
        public string StrCanChoose
        {
            get { return this.GetProperty(StrCanChooseProperty); }
            set { this.SetProperty(StrCanChooseProperty, value); }
        }
        #endregion

        #region 是否重复-字符 StrIsRepeat
        /// <summary>
        /// 是否重复-字符
        /// </summary>
        [Label("是否重复")]
        public static readonly Property<string> StrIsRepeatProperty = P<ProcessViewModel>.Register(e => e.StrIsRepeat);

        /// <summary>
        /// 是否重复-字符
        /// </summary>
        public string StrIsRepeat
        {
            get { return this.GetProperty(StrIsRepeatProperty); }
            set { this.SetProperty(StrIsRepeatProperty, value); }
        }
        #endregion

        #region 是否创建SKU StrIsSku
        /// <summary>
        /// 是否创建SKU
        /// </summary>
        [Label("是否创建SKU")]
        public static readonly Property<string> StrIsSkuProperty = P<ProcessViewModel>.Register(e => e.StrIsSku);

        /// <summary>
        /// 是否创建SKU
        /// </summary>
        public string StrIsSku
        {
            get { return this.GetProperty(StrIsSkuProperty); }
            set { this.SetProperty(StrIsSkuProperty, value); }
        }
        #endregion
        #endregion

        #region 工序并行组 ParallelGroup
        /// <summary>
        /// 工序并行组
        /// </summary>
        [Label("工序并行组")]
        public static readonly Property<string> ParallelGroupProperty = P<ProcessViewModel>.Register(e => e.ParallelGroup);

        /// <summary>
        /// 工序并行组
        /// </summary>
        public string ParallelGroup
        {
            get { return this.GetProperty(ParallelGroupProperty); }
            set { this.SetProperty(ParallelGroupProperty, value); }
        }
        #endregion

        #region 标识ID ActivityId
        /// <summary>
        /// 标识ID
        /// </summary>
        [Label("标识ID")]
        public static readonly Property<string> ActivityIdProperty = P<ProcessViewModel>.Register(e => e.ActivityId);

        /// <summary>
        /// 标识ID
        /// </summary>
        public string ActivityId
        {
            get { return GetProperty(ActivityIdProperty); }
            set { SetProperty(ActivityIdProperty, value); }
        }
        #endregion

        #region 工艺路线信息Id LayoutInfoId
        /// <summary>
        /// 工艺路线信息Id
        /// </summary>
        [Label("工艺路线信息Id")]
        public static readonly Property<double?> LayoutInfoIdProperty = P<ProcessViewModel>.Register(e => e.LayoutInfoId);

        /// <summary>
        /// 工艺路线信息Id
        /// </summary>
        public double? LayoutInfoId
        {
            get { return this.GetProperty(LayoutInfoIdProperty); }
            set { this.SetProperty(LayoutInfoIdProperty, value); }
        }
        #endregion

        #region 工序流水码 Vornr
        /// <summary>
        /// 工序流水码
        /// </summary>
        [Label("工序流水码")]
        public static readonly Property<string> VornrProperty = P<ProcessViewModel>.Register(e => e.Vornr);

        /// <summary>
        /// 工序流水码
        /// </summary>
        public string Vornr
        {
            get { return this.GetProperty(VornrProperty); }
            set { this.SetProperty(VornrProperty, value); }
        }
        #endregion

        #region 控制码(工序控制码) Steus
        /// <summary>
        /// 控制码(工序控制码)
        /// </summary>
        [Label("控制码(工序控制码)")]
        public static readonly Property<string> SteusProperty = P<ProcessViewModel>.Register(e => e.Steus);

        /// <summary>
        /// 控制码(工序控制码)
        /// </summary>
        public string Steus
        {
            get { return this.GetProperty(SteusProperty); }
            set { this.SetProperty(SteusProperty, value); }
        }
        #endregion

    }
}