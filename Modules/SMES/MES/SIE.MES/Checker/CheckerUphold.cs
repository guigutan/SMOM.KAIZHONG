using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Checker;
using SIE.MES.Fixture;
using SIE.MES.ItemChecker;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Checker
{
    /// <summary>
    /// 检具维护
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(CheckerUpholdCriterial))]
    [Label("检具维护")]
    [DisplayMember(nameof(CheckerCode))]
    public partial class CheckerUphold : DataEntity
    {
        #region 快码
        /// <summary>
        /// 检具维护状态快码字符串
        /// </summary>
        public const string StateCatalogType = "CHECKER_STATE";

        /// <summary>
        /// 检具维护类型快码字符串
        /// </summary>
        public const string TypeCatalogType = "CHECKER_TYPE";
        #endregion

        #region 检具编码 CheckerCode
        /// <summary>
        /// 检具编码
        /// </summary>
        [Required]
        [Label("检具编码")]
        public static readonly Property<string> CheckerCodeProperty = P<CheckerUphold>.Register(e => e.CheckerCode);

        /// <summary>
        /// 检具编码
        /// </summary>
        public string CheckerCode
        {
            get { return this.GetProperty(CheckerCodeProperty); }
            set { this.SetProperty(CheckerCodeProperty, value); }
        }
        #endregion

        #region 检具名称 CheckerName
        /// <summary>
        /// 检具名称
        /// </summary>
        [Required]
        [Label("检具名称")]
        public static readonly Property<string> CheckerNameProperty = P<CheckerUphold>.Register(e => e.CheckerName);

        /// <summary>
        /// 检具名称
        /// </summary>
        public string CheckerName
        {
            get { return this.GetProperty(CheckerNameProperty); }
            set { this.SetProperty(CheckerNameProperty, value); }
        }
        #endregion

        #region 有效日期 EffectiveDate
        /// <summary>
        /// 有效日期
        /// </summary>
        [Label("有效日期")]
        public static readonly Property<DateTime?> EffectiveDateProperty = P<CheckerUphold>.Register(e => e.EffectiveDate);

        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? EffectiveDate
        {
            get { return this.GetProperty(EffectiveDateProperty); }
            set { this.SetProperty(EffectiveDateProperty, value); }
        }
        #endregion

        #region 检具类型 CheckerType
        /// <summary>
        /// 检具类型
        /// </summary>
        [Required]
        [Label("检具类型")]
        public static readonly Property<string> CheckerTypeProperty = P<CheckerUphold>.Register(e => e.CheckerType);

        /// <summary>
        /// 检具状态
        /// </summary>
        public string CheckerType
        {
            get { return this.GetProperty(CheckerTypeProperty); }
            set { this.SetProperty(CheckerTypeProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<CheckerUphold>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<CheckerUphold>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        [Required]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<CheckerUphold>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<CheckerUphold>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 图号 DrawingNo
        /// <summary>
        /// 图号
        /// </summary>
        [Label("图号")]
        public static readonly Property<string> DrawingNoProperty = P<CheckerUphold>.Register(e => e.DrawingNo);

        /// <summary>
        /// 图号
        /// </summary>
        public string DrawingNo
        {
            get { return this.GetProperty(DrawingNoProperty); }
            set { this.SetProperty(DrawingNoProperty, value); }
        }
        #endregion


        #region 视图属性

        #region 工厂编码 FactoryCode
        /// <summary>
        /// 工厂编码
        /// </summary>
        [Label("工厂编码")]
        public static readonly Property<string> FactoryCodeProperty = P<CheckerUphold>.RegisterView(e => e.FactoryCode, p => p.Factory.Code);

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode
        {
            get { return this.GetProperty(FactoryCodeProperty); }
        }

        #endregion

        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<CheckerUphold>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// 检具维护 实体配置
    /// </summary>
    internal class CheckerUpholdConfig : EntityConfig<CheckerUphold>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            //rules.AddRule(CheckerUphold.CheckerCodeProperty, new NotDuplicateRule());
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = {
                CheckerUphold.CheckerCodeProperty,
                CheckerUphold.DrawingNoProperty
                },
                MessageBuilder = (e) => {
                    return "已存在相同检具编码和图号的数据".L10N();
                }
            });
            base.AddValidations(rules);
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CHECKER_UPHOLD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
