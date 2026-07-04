using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using SIE.Common.Prints;

namespace SIE.MES.TaskManagement.Configs
{
    /// <summary>
    /// 派工任务单配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("派工任务单配置值")]
    public class DispatchConfigValue : ConfigValue
    {

        #region 是否强制校验产线对应任务单 IsCheckProductionLineTaskList
        /// <summary>
        /// 是否强制校验产线对应任务单
        /// </summary>
        [Label("是否强制校验产线对应任务单")]
        public static readonly Property<bool> IsCheckProductionLineTaskListProperty = P<DispatchConfigValue>.Register(e => e.IsCheckProductionLineTaskList);

        /// <summary>
        /// 是否强制校验产线对应任务单
        /// </summary>
        public bool IsCheckProductionLineTaskList
        {
            get { return this.GetProperty(IsCheckProductionLineTaskListProperty); }
            set { this.SetProperty(IsCheckProductionLineTaskListProperty, value); }
        }
        #endregion      

        #region 是否校验员工技能 IsCheckEmployeeSkill
        /// <summary>
        /// 是否校验员工技能
        /// </summary>
        [Label("是否校验员工技能")]
        public static readonly Property<bool> IsCheckEmployeeSkillProperty = P<DispatchConfigValue>.Register(e => e.IsCheckEmployeeSkill);

        /// <summary>
        /// 是否校验员工技能
        /// </summary>
        public bool IsCheckEmployeeSkill
        {
            get { return this.GetProperty(IsCheckEmployeeSkillProperty); }
            set { this.SetProperty(IsCheckEmployeeSkillProperty, value); }
        }
        #endregion      

        #region 是否校验人员权限 IsCheckPersonnelPermission
        /// <summary>
        /// 是否校验人员权限
        /// </summary>
        [Label("是否校验人员权限")]
        public static readonly Property<bool> IsCheckPersonnelPermissionProperty = P<DispatchConfigValue>.Register(e => e.IsCheckPersonnelPermission);

        /// <summary>
        /// 是否校验人员权限
        /// </summary>
        public bool IsCheckPersonnelPermission
        {
            get { return this.GetProperty(IsCheckPersonnelPermissionProperty); }
            set { this.SetProperty(IsCheckPersonnelPermissionProperty, value); }
        }
        #endregion

        #region 手动报工:当前工单已生成的第一个工序的任务单 IsFirstProcess
        /// <summary>
        /// 手动报工:当前工单已生成的第一个工序的任务单
        /// </summary>
        [Label("手动报工:当前工单已生成的第一个工序的任务单")]
        public static readonly Property<bool?> IsFirstProcessProperty = P<DispatchConfigValue>.Register(e => e.IsFirstProcess);

        /// <summary>
        /// 手动报工:当前工单已生成的第一个工序的任务单
        /// </summary>
        public bool? IsFirstProcess
        {
            get { return this.GetProperty(IsFirstProcessProperty); }
            set { this.SetProperty(IsFirstProcessProperty, value); }
        }
        #endregion


        #region 编码规则 NumberRule
        /// <summary>
        /// 编码规则Id
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
            P<DispatchConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 编码规则Id
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty =
            P<DispatchConfigValue>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region 允许报工数量超过当前标签批次数量 IsAllowOverBatchQty
        /// <summary>
        /// 允许报工数量超过当前标签批次数量
        /// </summary>
        [Label("允许报工数量超过当前标签批次数量")]
        public static readonly Property<bool> IsAllowOverBatchQtyProperty = P<DispatchConfigValue>.Register(e => e.IsAllowOverBatchQty);

        /// <summary>
        /// 允许报工数量超过当前标签批次数量
        /// </summary>
        public bool IsAllowOverBatchQty
        {
            get { return this.GetProperty(IsAllowOverBatchQtyProperty); }
            set { this.SetProperty(IsAllowOverBatchQtyProperty, value); }
        }
        #endregion

        #region 可超标签批次数量报工工序编码 IsAllowOverProcessCodes
        /// <summary>
        /// 可超标签批次数量报工工序编码
        /// </summary>
        [Label("可超标签批次数量报工工序编码")]
        public static readonly Property<string> IsAllowOverProcessCodesProperty = P<DispatchConfigValue>.Register(e => e.IsAllowOverProcessCodes);

        /// <summary>
        /// 可超标签批次数量报工工序编码
        /// </summary>
        public string IsAllowOverProcessCodes
        {
            get { return this.GetProperty(IsAllowOverProcessCodesProperty); }
            set { this.SetProperty(IsAllowOverProcessCodesProperty, value); }
        }
        #endregion

        
        #region 良品标签(打印模板) GoodLabelTemplate
        /// <summary>
        /// 良品标签Id
        /// </summary>
        [Label("良品标签(打印模板)")]
        public static readonly IRefIdProperty GoodLabelTemplateIdProperty =
            P<DispatchConfigValue>.RegisterRefId(e => e.GoodLabelTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 良品标签Id
        /// </summary>
        public double? GoodLabelTemplateId
        {
            get { return (double?)this.GetRefNullableId(GoodLabelTemplateIdProperty); }
            set { this.SetRefNullableId(GoodLabelTemplateIdProperty, value); }
        }

        /// <summary>
        /// 良品标签
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> GoodLabelTemplateProperty =
            P<DispatchConfigValue>.RegisterRef(e => e.GoodLabelTemplate, GoodLabelTemplateIdProperty);

        /// <summary>
        /// 良品标签
        /// </summary>
        public PrintTemplate GoodLabelTemplate
        {
            get { return this.GetRefEntity(GoodLabelTemplateProperty); }
            set { this.SetRefEntity(GoodLabelTemplateProperty, value); }
        }
        #endregion

        #region 可疑品标签(打印模板) SuspectLabelTemplate
        /// <summary>
        /// 可疑品标签Id
        /// </summary>
        [Label("可疑品标签(打印模板)")]
        public static readonly IRefIdProperty SuspectLabelTemplateIdProperty =
            P<DispatchConfigValue>.RegisterRefId(e => e.SuspectLabelTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 可疑品标签Id
        /// </summary>
        public double? SuspectLabelTemplateId
        {
            get { return (double?)this.GetRefNullableId(SuspectLabelTemplateIdProperty); }
            set { this.SetRefNullableId(SuspectLabelTemplateIdProperty, value); }
        }

        /// <summary>
        /// 可疑品标签
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> SuspectLabelTemplateProperty =
            P<DispatchConfigValue>.RegisterRef(e => e.SuspectLabelTemplate, SuspectLabelTemplateIdProperty);

        /// <summary>
        /// 可疑品标签
        /// </summary>
        public PrintTemplate SuspectLabelTemplate
        {
            get { return this.GetRefEntity(SuspectLabelTemplateProperty); }
            set { this.SetRefEntity(SuspectLabelTemplateProperty, value); }
        }
        #endregion

        #region 良品标签(蓝牙指令模板) GoodLabel
        /// <summary>
        /// 良品标签
        /// </summary>
        [Label("良品标签(蓝牙指令模板)")]
        public static readonly Property<string> GoodLabelProperty = P<DispatchConfigValue>.Register(e => e.GoodLabel);

        /// <summary>
        /// 良品标签
        /// </summary>
        public string GoodLabel
        {
            get { return this.GetProperty(GoodLabelProperty); }
            set { this.SetProperty(GoodLabelProperty, value); }
        }
        #endregion

        #region 可疑品标签(蓝牙指令模板) SuspectLabel
        /// <summary>
        /// 可疑品标签
        /// </summary>
        [Label("可疑品标签(蓝牙指令模板)")]
        public static readonly Property<string> SuspectLabelProperty = P<DispatchConfigValue>.Register(e => e.SuspectLabel);

        /// <summary>
        /// 可疑品标签
        /// </summary>
        public string SuspectLabel
        {
            get { return this.GetProperty(SuspectLabelProperty); }
            set { this.SetProperty(SuspectLabelProperty, value); }
        }
        #endregion

        #region 绕包线编码规则 EntangleNumberRule
        /// <summary>
        /// 绕包线编码规则Id
        /// </summary>
        [Label("绕包线编码规则")]
        public static readonly IRefIdProperty EntangleNumberRuleIdProperty =
            P<DispatchConfigValue>.RegisterRefId(e => e.EntangleNumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 绕包线编码规则Id
        /// </summary>
        public double? EntangleNumberRuleId
        {
            get { return (double?)this.GetRefNullableId(EntangleNumberRuleIdProperty); }
            set { this.SetRefNullableId(EntangleNumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 绕包线编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> EntangleNumberRuleProperty =
            P<DispatchConfigValue>.RegisterRef(e => e.EntangleNumberRule, EntangleNumberRuleIdProperty);

        /// <summary>
        /// 绕包线编码规则
        /// </summary>
        public NumberRule EntangleNumberRule
        {
            get { return this.GetRefEntity(EntangleNumberRuleProperty); }
            set { this.SetRefEntity(EntangleNumberRuleProperty, value); }
        }
        #endregion

        #region 绕包线打印模板 EntanglePrintTemplate
        /// <summary>
        /// 绕包线打印模板Id
        /// </summary>
        [Label("绕包线打印模板")]
        public static readonly IRefIdProperty EntanglePrintTemplateIdProperty =
            P<DispatchConfigValue>.RegisterRefId(e => e.EntanglePrintTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 绕包线打印模板Id
        /// </summary>
        public double? EntanglePrintTemplateId
        {
            get { return (double?)this.GetRefNullableId(EntanglePrintTemplateIdProperty); }
            set { this.SetRefNullableId(EntanglePrintTemplateIdProperty, value); }
        }

        /// <summary>
        /// 绕包线打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> EntanglePrintTemplateProperty =
            P<DispatchConfigValue>.RegisterRef(e => e.EntanglePrintTemplate, EntanglePrintTemplateIdProperty);

        /// <summary>
        /// 绕包线打印模板
        /// </summary>
        public PrintTemplate EntanglePrintTemplate
        {
            get { return this.GetRefEntity(EntanglePrintTemplateProperty); }
            set { this.SetRefEntity(EntanglePrintTemplateProperty, value); }
        }
        #endregion

        #region 非绕包线编码规则 UnEntangleNumberRule
        /// <summary>
        /// 非绕包线编码规则Id
        /// </summary>
        [Label("非绕包线编码规则")]
        public static readonly IRefIdProperty UnEntangleNumberRuleIdProperty =
            P<DispatchConfigValue>.RegisterRefId(e => e.UnEntangleNumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 非绕包线编码规则Id
        /// </summary>
        public double? UnEntangleNumberRuleId
        {
            get { return (double?)this.GetRefNullableId(UnEntangleNumberRuleIdProperty); }
            set { this.SetRefNullableId(UnEntangleNumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 非绕包线编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> UnEntangleNumberRuleProperty =
            P<DispatchConfigValue>.RegisterRef(e => e.UnEntangleNumberRule, UnEntangleNumberRuleIdProperty);

        /// <summary>
        /// 非绕包线编码规则
        /// </summary>
        public NumberRule UnEntangleNumberRule
        {
            get { return this.GetRefEntity(UnEntangleNumberRuleProperty); }
            set { this.SetRefEntity(UnEntangleNumberRuleProperty, value); }
        }
        #endregion

        #region 非绕包线打印模板 UnEntanglePrintTemplate
        /// <summary>
        /// 非绕包线打印模板Id
        /// </summary>
        [Label("非绕包线打印模板")]
        public static readonly IRefIdProperty UnEntanglePrintTemplateIdProperty =
            P<DispatchConfigValue>.RegisterRefId(e => e.UnEntanglePrintTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 非绕包线打印模板Id
        /// </summary>
        public double? UnEntanglePrintTemplateId
        {
            get { return (double?)this.GetRefNullableId(UnEntanglePrintTemplateIdProperty); }
            set { this.SetRefNullableId(UnEntanglePrintTemplateIdProperty, value); }
        }

        /// <summary>
        /// 非绕包线打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> UnEntanglePrintTemplateProperty =
            P<DispatchConfigValue>.RegisterRef(e => e.UnEntanglePrintTemplate, UnEntanglePrintTemplateIdProperty);

        /// <summary>
        /// 非绕包线打印模板
        /// </summary>
        public PrintTemplate UnEntanglePrintTemplate
        {
            get { return this.GetRefEntity(UnEntanglePrintTemplateProperty); }
            set { this.SetRefEntity(UnEntanglePrintTemplateProperty, value); }
        }
        #endregion

        #region 新材料工序校验 NewMaterialProValid
        /// <summary>
        /// 新材料工序校验
        /// </summary>
        [Label("新材料工序校验(多工序编码，用英文逗号隔开)")]
        public static readonly Property<string> NewMaterialProValidProperty = P<DispatchConfigValue>.Register(e => e.NewMaterialProValid);

        /// <summary>
        /// 新材料工序校验
        /// </summary>
        public string NewMaterialProValid
        {
            get { return this.GetProperty(NewMaterialProValidProperty); }
            set { this.SetProperty(NewMaterialProValidProperty, value); }
        }
        #endregion

        #region 是否启用扫码报工任务单数量校验 IsValidScanQty
        /// <summary>
        /// 是否启用扫码报工任务单数量校验
        /// </summary>
        [Label("是否启用扫码报工任务单数量校验")]
        public static readonly Property<bool?> IsValidScanQtyProperty = P<DispatchConfigValue>.Register(e => e.IsValidScanQty);

        /// <summary>
        /// 是否启用扫码报工任务单数量校验
        /// </summary>
        public bool? IsValidScanQty
        {
            get { return this.GetProperty(IsValidScanQtyProperty); }
            set { this.SetProperty(IsValidScanQtyProperty, value); }
        }
        #endregion


        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            return "是否校验员工技能:{0} | 是否校验人员权限：{1} | 编码规则：{2} 良品标签：{3} 可疑品标签：{3} 绕包线编码规则: {4} 绕包线打印模板: {5} 非绕包线编码规则: {6} 非绕包线打印模板: {7} 新材料工序校验: {8} 是否启用扫码报工任务单数量校验: {9}".L10nFormat(IsCheckEmployeeSkill, IsCheckPersonnelPermission, NumberRule?.Name, GoodLabelTemplate?.FileName, SuspectLabelTemplate?.FileName, EntangleNumberRule?.Name, EntanglePrintTemplate?.FileName, UnEntangleNumberRule?.Name, UnEntanglePrintTemplate?.FileName, NewMaterialProValid, IsValidScanQty);
        }
    }
}
