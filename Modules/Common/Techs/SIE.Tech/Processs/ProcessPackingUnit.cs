using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.Packages;
using System;

namespace SIE.Tech.Processs
{
    /// <summary>
    /// 工序与包装单位的关系配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("工序对应包装")]
    [DisplayMember(nameof(PackageUnitId))]
    public class ProcessPackingUnit : DataEntity
    {
        #region 工序
        /// <summary>
        /// 工序ID
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<ProcessPackingUnit>.RegisterRefId(e => e.ProcessId, ReferenceType.Parent);

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<ProcessPackingUnit>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 包装单位

        /// <summary>
        /// 包装单位ID
        /// </summary>
        [Label("包装单位")]
        public static readonly IRefIdProperty PackageUnitIdProperty =
            P<ProcessPackingUnit>.RegisterRefId(e => e.PackageUnitId, ReferenceType.Normal);

        /// <summary>
        /// 包装单位ID
        /// </summary>
        public double PackageUnitId
        {
            get { return (double)this.GetRefId(PackageUnitIdProperty); }
            set { this.SetRefId(PackageUnitIdProperty, value); }
        }

        /// <summary>
        /// 包装单位
        /// </summary>
        public static readonly RefEntityProperty<PackingUnit> PackageUnitProperty =
            P<ProcessPackingUnit>.RegisterRef(e => e.PackageUnit, PackageUnitIdProperty);

        /// <summary>
        /// 包装单位
        /// </summary>
        public PackingUnit PackageUnit
        {
            get { return this.GetRefEntity(PackageUnitProperty); }
            set { this.SetRefEntity(PackageUnitProperty, value); }
        }

        #endregion

        #region 代码 PackageUnitCode
        /// <summary>
        /// 代码
        /// </summary>
        [Label("代码")]
        public static readonly Property<string> PackageUnitCodeProperty = P<ProcessPackingUnit>.RegisterView(e => e.PackageUnitCode, p => p.PackageUnit.Code);

        /// <summary>
        /// 代码
        /// </summary>
        public string PackageUnitCode
        {
            get { return this.GetProperty(PackageUnitCodeProperty); }
        }
        #endregion

        #region 单位名称 PackageUnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> PackageUnitNameProperty = P<ProcessPackingUnit>.RegisterView(e => e.PackageUnitName, p => p.PackageUnit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string PackageUnitName
        {
            get { return this.GetProperty(PackageUnitNameProperty); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<ProcessPackingUnit>.RegisterView(e => e.Description, p => p.PackageUnit.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return this.GetProperty(DescriptionProperty); }
        }
        #endregion

        #region 是否主单位 IsMasterUnit
        /// <summary>
        /// 是否主单位
        /// </summary>
        [Label("是否主单位")]
        public static readonly Property<bool> IsMasterUnitProperty = P<ProcessPackingUnit>.RegisterView(e => e.IsMasterUnit, p => p.PackageUnit.IsMasterUnit);

        /// <summary>
        /// 描述
        /// </summary>
        public bool IsMasterUnit
        {
            get { return this.GetProperty(IsMasterUnitProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 工序与包装单位的关系数据库配置
    /// </summary>
    public class ProcessPackingUnitConfig : EntityConfig<ProcessPackingUnit>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_PROCESS_PKG_UNIT").MapAllProperties();
            Meta.DisablePhantoms();
            Meta.EnableInvOrg();
            base.ConfigMeta();
            Meta.Property(ProcessPackingUnit.ProcessIdProperty).ColumnMeta.HasIndex();
        }

        /// <summary>
        /// 增加实体的校验规则
        /// </summary>
        /// <param name="rules">实体校验规则集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule
            {
                Properties = { ProcessPackingUnit.PackageUnitIdProperty, ProcessPackingUnit.ProcessIdProperty }
            });
            rules.AddRule(new HandlerRule
            {
                Handler = (s, e) =>
                {
                    var unit = s as ProcessPackingUnit;
                    if (unit.Process.Type != ProcessType.Packing && unit.Process.Type != ProcessType.BatchPacking)
                        e.BrokenDescription = "包装单位只能在包装工序配置".L10N();
                }
            });
            base.AddValidations(rules);
        }
    }
}