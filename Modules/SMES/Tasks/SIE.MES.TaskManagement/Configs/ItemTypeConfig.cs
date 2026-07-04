using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.Packings.Configs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Configs
{
    /// <summary>
    /// 派工任务单物料类型配置
    /// </summary>
    [DisplayName("派工任务单物料类型配置")]
    [Description("用于派工任务单物料类型配置具体规则")]
    public class ItemTypeConfig : ModuleCategoryConfig<TypeConfigValue, ItemTypeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override ItemTypeConfigValue DefaultValue
        {
            get { return new ItemTypeConfigValue(); }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(TypeConfigValue.Type))]
    public class TypeConfigValue : StringEntity
    {
        #region 物料类型 Type
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<string> TypeProperty = P<TypeConfigValue>.Register(e => e.Type);

        /// <summary>
        /// 物料类型
        /// </summary>
        public string Type
        {
            get { return this.GetProperty(TypeProperty); }
            set { this.SetProperty(TypeProperty, value); }
        }
        #endregion
    }

    internal class TypeConfigValueConfig : EntityConfig<TypeConfigValue>
    {
        protected override void ConfigMeta()
        {
            Meta.MapView("(select Mtart Id,Mtart Type_,'' Create_By,'' Update_By,'' Create_Date,'' Update_Date from item where Mtart is not null group by Mtart)").MapAllProperties();
            Meta.DisableInvOrg();
            Meta.DisablePhantoms();
        }
    }


    [RootEntity, Serializable]
    public class ItemTypeConfigValue : ConfigValue
    {

        #region 物料类型工序编码规则 ProcessNumberRule
        /// <summary>
        /// 物料类型工序编码规则Id
        /// </summary>
        [Label("物料类型工序编码规则")]
        public static readonly IRefIdProperty ProcessNumberRuleIdProperty =
            P<ItemTypeConfigValue>.RegisterRefId(e => e.ProcessNumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 物料类型工序编码规则Id
        /// </summary>
        public double? ProcessNumberRuleId
        {
            get { return (double?)this.GetRefNullableId(ProcessNumberRuleIdProperty); }
            set { this.SetRefNullableId(ProcessNumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 物料类型工序编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> ProcessNumberRuleProperty =
            P<ItemTypeConfigValue>.RegisterRef(e => e.ProcessNumberRule, ProcessNumberRuleIdProperty);

        /// <summary>
        /// 物料类型工序编码规则
        /// </summary>
        public NumberRule ProcessNumberRule
        {
            get { return this.GetRefEntity(ProcessNumberRuleProperty); }
            set { this.SetRefEntity(ProcessNumberRuleProperty, value); }
        }
        #endregion

        #region 物料类型工序模板 ProcessPrintTemplate
        /// <summary>
        /// 物料类型工序模板Id
        /// </summary>
        [Label("物料类型工序模板")]
        public static readonly IRefIdProperty ProcessPrintTemplateIdProperty =
            P<ItemTypeConfigValue>.RegisterRefId(e => e.ProcessPrintTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 物料类型工序模板Id
        /// </summary>
        public double? ProcessPrintTemplateId
        {
            get { return (double?)this.GetRefNullableId(ProcessPrintTemplateIdProperty); }
            set { this.SetRefNullableId(ProcessPrintTemplateIdProperty, value); }
        }

        /// <summary>
        /// 物料类型工序模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> ProcessPrintTemplateProperty =
            P<ItemTypeConfigValue>.RegisterRef(e => e.ProcessPrintTemplate, ProcessPrintTemplateIdProperty);

        /// <summary>
        /// 物料类型工序模板
        /// </summary>
        public PrintTemplate ProcessPrintTemplate
        {
            get { return this.GetRefEntity(ProcessPrintTemplateProperty); }
            set { this.SetRefEntity(ProcessPrintTemplateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            return "物料类型工序编码规则:{0} | 物料类型工序模板：{1} ".L10nFormat(ProcessNumberRule?.Name, ProcessPrintTemplate?.FileName);
        }
    }
}
