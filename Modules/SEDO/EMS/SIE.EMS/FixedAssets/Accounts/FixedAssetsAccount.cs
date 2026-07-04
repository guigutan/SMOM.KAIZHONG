using SIE.Common.Configs;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Equipments.FinancialCategorys;
using SIE.EMS.FixedAssets.Accounts.Config;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;
using SIE.Domain.Validation;
using System.Text;
using SIE.Equipments.Configs;

namespace SIE.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 固定资产台账
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(FixedAssetsAccountCriteria))]
    [EntityWithConfig(typeof(FixedAssetAccountConfig))]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [Label("固定资产台账")]
    public partial class FixedAssetsAccount : Core.Equipments.FixedAssets.FixedAssetsAccount
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public FixedAssetsAccount()
        {
            this.ReviewStatus = ApprovalStatus.Draft;
        }

        

        #region 合同编码 ContractCode
        /// <summary>
        /// 合同编码
        /// </summary>
        [Label("合同编码")]
        public static readonly Property<string> ContractCodeProperty = P<FixedAssetsAccount>.Register(e => e.ContractCode);

        /// <summary>
        /// 合同编码
        /// </summary>
        public string ContractCode
        {
            get { return GetProperty(ContractCodeProperty); }
            set { SetProperty(ContractCodeProperty, value); }
        }
        #endregion

        #region 采购单价 PurchaseUnitPrice
        /// <summary>
        /// 采购单价
        /// </summary>
        [Label("采购单价")]
        public static readonly Property<decimal?> PurchaseUnitPriceProperty = P<FixedAssetsAccount>.Register(e => e.PurchaseUnitPrice);

        /// <summary>
        /// 采购单价
        /// </summary>
        public decimal? PurchaseUnitPrice
        {
            get { return GetProperty(PurchaseUnitPriceProperty); }
            set { SetProperty(PurchaseUnitPriceProperty, value); }
        }
        #endregion

        #region 成本中心 CostCenter
        /// <summary>
        /// 成本中心
        /// </summary>
        [Label("成本中心")]
        public static readonly Property<string> CostCenterProperty = P<FixedAssetsAccount>.Register(e => e.CostCenter);

        /// <summary>
        /// 成本中心
        /// </summary>
        public string CostCenter
        {
            get { return GetProperty(CostCenterProperty); }
            set { SetProperty(CostCenterProperty, value); }
        }
        #endregion

        #region 转固日期 FixedAssetsTransferDate
        /// <summary>
        /// 转固日期
        /// </summary>
        [Label("转固日期")]
        [Required]
        public static readonly Property<DateTime> FixedAssetsTransferDateProperty = P<FixedAssetsAccount>.Register(e => e.FixedAssetsTransferDate);

        /// <summary>
        /// 转固日期
        /// </summary>
        public DateTime FixedAssetsTransferDate
        {
            get { return GetProperty(FixedAssetsTransferDateProperty); }
            set { SetProperty(FixedAssetsTransferDateProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<FixedAssetsAccount>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return (double)GetRefId(FactoryIdProperty); }
            set { SetRefId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<FixedAssetsAccount>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 管理部门 MangeDepartment
        /// <summary>
        /// 管理部门Id
        /// </summary>
        [Label("管理部门")]
        public static readonly IRefIdProperty MangeDepartmentIdProperty = P<FixedAssetsAccount>.RegisterRefId(e => e.MangeDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 管理部门Id
        /// </summary>
        public double MangeDepartmentId
        {
            get { return (double)GetRefId(MangeDepartmentIdProperty); }
            set { SetRefId(MangeDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 管理部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> MangeDepartmentProperty = P<FixedAssetsAccount>.RegisterRef(e => e.MangeDepartment, MangeDepartmentIdProperty);

        /// <summary>
        /// 管理部门
        /// </summary>
        public Enterprise MangeDepartment
        {
            get { return GetRefEntity(MangeDepartmentProperty); }
            set { SetRefEntity(MangeDepartmentProperty, value); }
        }
        #endregion

        #region 管理状态 ManageStatus
        /// <summary>
        /// 管理状态
        /// </summary>
        [Label("管理状态")]
        public static readonly Property<ManageState> ManageStatusProperty = P<FixedAssetsAccount>.Register(e => e.ManageStatus);

        /// <summary>
        /// 管理状态
        /// </summary>
        public ManageState ManageStatus
        {
            get { return GetProperty(ManageStatusProperty); }
            set { SetProperty(ManageStatusProperty, value); }
        }
        #endregion

        #region 资产来源 AssetsSource
        /// <summary>
        /// 资产来源
        /// </summary>
        [Label("资产来源")]
        [Required]
        public static readonly Property<AssetsSource> AssetsSourceProperty = P<FixedAssetsAccount>.Register(e => e.AssetsSource);

        /// <summary>
        /// 资产来源
        /// </summary>
        public AssetsSource AssetsSource
        {
            get { return GetProperty(AssetsSourceProperty); }
            set { SetProperty(AssetsSourceProperty, value); }
        }
        #endregion

        #region 审核状态 ReviewStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        [Required]
        public static readonly Property<ApprovalStatus> ReviewStatusProperty
            = P<FixedAssetsAccount>.Register(e => e.ReviewStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ReviewStatus
        {
            get { return GetProperty(ReviewStatusProperty); }
            set { SetProperty(ReviewStatusProperty, value); }
        }
        #endregion

        #region 资产类型 AssetsType
        /// <summary>
        /// 资产类型
        /// </summary>
        [Label("资产类型")]
        [Required]
        public static readonly Property<AssetsType> AssetsTypeProperty = P<FixedAssetsAccount>.Register(e => e.AssetsType);

        /// <summary>
        /// 资产类型
        /// </summary>
        public AssetsType AssetsType
        {
            get { return GetProperty(AssetsTypeProperty); }
            set { SetProperty(AssetsTypeProperty, value); }
        }
        #endregion

        #region 类型 AssetsClass
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        [Required]
        public static readonly Property<AssetsClass> AssetsClassProperty = P<FixedAssetsAccount>.Register(e => e.AssetsClass);

        /// <summary>
        /// 类型
        /// </summary>
        public AssetsClass AssetsClass
        {
            get { return this.GetProperty(AssetsClassProperty); }
            set { this.SetProperty(AssetsClassProperty, value); }
        }
        #endregion

        #region 财务分类 FinancialCategory
        /// <summary>
        /// 财务分类Id
        /// </summary>
        [Label("财务分类")]
        public static readonly IRefIdProperty FinancialCategoryIdProperty =
            P<FixedAssetsAccount>.RegisterRefId(e => e.FinancialCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 财务分类Id
        /// </summary>
        public double FinancialCategoryId
        {
            get { return (double)this.GetRefId(FinancialCategoryIdProperty); }
            set { this.SetRefId(FinancialCategoryIdProperty, value); }
        }

        /// <summary>
        /// 财务分类
        /// </summary>
        public static readonly RefEntityProperty<FinancialCategory> FinancialCategoryProperty =
            P<FixedAssetsAccount>.RegisterRef(e => e.FinancialCategory, FinancialCategoryIdProperty);

        /// <summary>
        /// 财务分类
        /// </summary>
        public FinancialCategory FinancialCategory
        {
            get { return this.GetRefEntity(FinancialCategoryProperty); }
            set { this.SetRefEntity(FinancialCategoryProperty, value); }
        }
        #endregion

        #region 年初净值 BeginNetWorth
        /// <summary>
        /// 年初净值
        /// </summary>
        [Label("年初净值")]
        public static readonly Property<decimal?> BeginNetWorthValueProperty = P<FixedAssetsAccount>.Register(e => e.BeginNetWorthValue);

        /// <summary>
        /// 年初净值
        /// </summary>
        public decimal? BeginNetWorthValue
        {
            get { return this.GetProperty(BeginNetWorthValueProperty); }
            set { this.SetProperty(BeginNetWorthValueProperty, value); }
        }
        #endregion

        #region 残值比例 ResidualValueRatio
        /// <summary>
        /// 残值比例
        /// </summary>
        [Label("残值比例(%)")]
        public static readonly Property<decimal> ResidualValueRatioProperty = P<FixedAssetsAccount>.Register(e => e.ResidualValueRatio);

        /// <summary>
        /// 残值比例
        /// </summary>
        public decimal ResidualValueRatio
        {
            get { return this.GetProperty(ResidualValueRatioProperty); }
            set { this.SetProperty(ResidualValueRatioProperty, value); }
        }
        #endregion



        #region 设备清单列表 DeviceBillList
        /// <summary>
        /// 设备清单列表
        /// </summary>
        [Label("设备清单")]
        public static readonly ListProperty<EntityList<FixedAssetDeviceBill>> DeviceBillListProperty = P<FixedAssetsAccount>.RegisterList(e => e.DeviceBillList);

        /// <summary>
        /// 设备清单列表
        /// </summary>
        public EntityList<FixedAssetDeviceBill> DeviceBillList
        {
            get { return this.GetLazyList(DeviceBillListProperty); }
        }
        #endregion

        #region 资产履历 ResumeList
        /// <summary>
        /// 资产履历
        /// </summary>
        [Label("资产履历")]
        public static readonly ListProperty<EntityList<FixedAssetResume>> ResumeListProperty = P<FixedAssetsAccount>.RegisterList(e => e.ResumeList);

        /// <summary>
        /// 资产履历
        /// </summary>
        public EntityList<FixedAssetResume> ResumeList
        {
            get { return this.GetLazyList(ResumeListProperty); }
        }
        #endregion

        #region 履历类型 ResumeState
        /// <summary>
        /// 履历类型
        /// </summary>
        [Label("履历类型")]
        public static readonly Property<int?> ResumeStateProperty = P<FixedAssetsAccount>.Register(e => e.ResumeState);

        /// <summary>
        /// 履历类型
        /// </summary>
        public int? ResumeState
        {
            get { return this.GetProperty(ResumeStateProperty); }
            set { this.SetProperty(ResumeStateProperty, value); }
        }
        #endregion

        #region 上次折旧日期 LastDepreciartion
        /// <summary>
        /// 上次折旧日期
        /// </summary>
        [Label("上次折旧日期")]
        public static readonly Property<DateTime?> LastDepreciartionProperty = P<FixedAssetsAccount>.Register(e => e.LastDepreciartion);

        /// <summary>
        /// 上次折旧日期
        /// </summary>
        public DateTime? LastDepreciartion
        {
            get { return this.GetProperty(LastDepreciartionProperty); }
            set { this.SetProperty(LastDepreciartionProperty, value); }
        }
        #endregion

        #region 折旧年限 DepreciationYear
        /// <summary>
        /// 折旧年限
        /// </summary>
        [Label("折旧年限")]
        public static readonly Property<int> DepreciationYearProperty = P<FixedAssetsAccount>.RegisterView(e => e.DepreciationYear, p => p.FinancialCategory.Depreciation);

        /// <summary>
        /// 折旧年限
        /// </summary>
        public int DepreciationYear
        {
            get { return this.GetProperty(DepreciationYearProperty); }
        }
        #endregion


        #region 工治具清单 FixedAssetFixtureBillList
        /// <summary>
        /// 工治具清单
        /// </summary>
        [Label("工治具清单")]
        public static readonly ListProperty<EntityList<FixedAssetFixtureBill>> FixedAssetFixtureBillListProperty = P<FixedAssetsAccount>.RegisterList(e => e.FixedAssetFixtureBillList);
        /// <summary>
        /// 工治具清单
        /// </summary>
        public EntityList<FixedAssetFixtureBill> FixedAssetFixtureBillList
        {
            get { return this.GetLazyList(FixedAssetFixtureBillListProperty); }
        }
        #endregion

        #region 备件清单 FixedAssetSparePartList
        /// <summary>
        /// 备件清单
        /// </summary>
        [Label("备件清单")]
        public static readonly ListProperty<EntityList<FixedAssetSparePart>> FixedAssetSparePartListProperty = P<FixedAssetsAccount>.RegisterList(e => e.FixedAssetSparePartList);
        /// <summary>
        /// 备件清单
        /// </summary>
        public EntityList<FixedAssetSparePart> FixedAssetSparePartList
        {
            get { return this.GetLazyList(FixedAssetSparePartListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 固定资产台账 实体配置
    /// </summary>
    internal class FixedAssetsAccountConfig : EntityConfig<FixedAssetsAccount>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FIXED_ASSETS_ACCOUNT").MapAllPropertiesExcept(FixedAssetsAccount.ResumeStateProperty);
            Meta.EnablePhantoms();
        }

        /// <summary>
        /// 校验规则
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var para = o.CastTo<FixedAssetsAccount>();
                    StringBuilder sb = new StringBuilder();

                    if (para.DepreciationResidualValue >= para.OriginalAssetsValue)
                    {
                        sb.AppendLine("【折旧残值】须小于【资产原值】！".L10N());
                    }

                    e.BrokenDescription = sb.ToString();
                }
            }, new RuleMeta() { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
        }
    }
}