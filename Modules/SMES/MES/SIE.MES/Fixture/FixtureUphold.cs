using NPOI.SS.Formula.Functions;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Checker;
using SIE.MES.ItemLine;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Fixture
{
    /// <summary>
    /// 工装维护
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(FixtureUpholdCriterial))]
    [DisplayMember(nameof(FixtureCode))]
    [Label("工装维护")]
    public partial class FixtureUphold : DataEntity
    {
        #region 快码
        /// <summary>
        /// 工装维护状态快码字符串
        /// </summary>
        public const string StateCatalogType = "FIXTURE_STATE";

        /// <summary>
        /// 工装维护类型快码字符串
        /// </summary>
        public const string TypeCatalogType = "FIXTURE_TYPE";
        #endregion

        #region 工装唯一码 FixtureCode
        /// <summary>
        /// 工装唯一码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("工装唯一码")]
        public static readonly Property<string> FixtureCodeProperty = P<FixtureUphold>.Register(e => e.FixtureCode);

        /// <summary>
        /// 工装唯一码
        /// </summary>
        public string FixtureCode
        {
            get { return this.GetProperty(FixtureCodeProperty); }
            set { this.SetProperty(FixtureCodeProperty, value); }
        }
        #endregion

        #region 工装物料描述 FixtureName
        /// <summary>
        /// 工装物料描述
        /// </summary>
        [Required]
        [Label("工装物料描述")]
        public static readonly Property<string> FixtureNameProperty = P<FixtureUphold>.Register(e => e.FixtureName);

        /// <summary>
        /// 工装物料描述
        /// </summary>
        public string FixtureName
        {
            get { return this.GetProperty(FixtureNameProperty); }
            set { this.SetProperty(FixtureNameProperty, value); }
        }
        #endregion

        #region 工装状态 FixtureState
        /// <summary>
        /// 工装状态
        /// </summary>
        [Required]
        [Label("工装状态")]
        public static readonly Property<string> FixtureStateProperty = P<FixtureUphold>.Register(e => e.FixtureState);

        /// <summary>
        /// 工装状态
        /// </summary>
        public string FixtureState
        {
            get { return this.GetProperty(FixtureStateProperty); }
            set { this.SetProperty(FixtureStateProperty, value); }
        }
        #endregion

        #region 工装类型 FixtureType
        /// <summary>
        /// 工装类型
        /// </summary>
        [Required]
        [Label("工装类型")]
        public static readonly Property<string> FixtureTypeProperty = P<FixtureUphold>.Register(e => e.FixtureType);

        /// <summary>
        /// 工装类型
        /// </summary>
        public string FixtureType
        {
            get { return this.GetProperty(FixtureTypeProperty); }
            set { this.SetProperty(FixtureTypeProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<FixtureUphold>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<FixtureUphold>.RegisterRef(e => e.Process, ProcessIdProperty);

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
            P<FixtureUphold>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
            P<FixtureUphold>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 图号 Drawn
        /// <summary>
        /// 图号
        /// </summary>
        [Label("图号")]
        public static readonly Property<string> DrawnProperty = P<FixtureUphold>.Register(e => e.Drawn);

        /// <summary>
        /// 图号
        /// </summary>
        public string Drawn
        {
            get { return this.GetProperty(DrawnProperty); }
            set { this.SetProperty(DrawnProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工厂编码 FactoryCode
        /// <summary>
        /// 工厂编码
        /// </summary>
        [Label("工厂编码")]
        public static readonly Property<string> FactoryCodeProperty = P<FixtureUphold>.RegisterView(e => e.FactoryCode, p => p.Factory.Code);

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
        public static readonly Property<string> FactoryNameProperty = P<FixtureUphold>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

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
    /// 工装维护 实体配置
    /// </summary>
    internal class FixtureUpholdConfig : EntityConfig<FixtureUphold>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(FixtureUphold.FixtureCodeProperty, new NotDuplicateRule());
            base.AddValidations(rules);
        }
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("FIXTURE_UPHOLD").MapAllProperties();
            Meta.Property(FixtureUphold.DrawnProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}
