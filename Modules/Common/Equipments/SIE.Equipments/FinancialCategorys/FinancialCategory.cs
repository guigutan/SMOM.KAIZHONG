using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Equipments.FinancialCategorys
{
    /// <summary>
    /// 财务分类
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(FinancialCategoryCriteria))]
    [DisplayMember(nameof(Name))]
    [Label("财务分类")]
    public class FinancialCategory : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> CodeProperty = P<FinancialCategory>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion


        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> NameProperty = P<FinancialCategory>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion


        #region 描述 Desc
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        [MaxLength(1000)]
        public static readonly Property<string> DescProperty = P<FinancialCategory>.Register(e => e.Desc);

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc
        {
            get { return this.GetProperty(DescProperty); }
            set { this.SetProperty(DescProperty, value); }
        }
        #endregion


        #region 折旧年限 Depreciation
        /// <summary>
        /// 折旧年限
        /// </summary>
        [Label("折旧年限")]
        public static readonly Property<int> DepreciationProperty = P<FinancialCategory>.Register(e => e.Depreciation);

        /// <summary>
        /// 折旧年限
        /// </summary>
        public int Depreciation
        {
            get { return this.GetProperty(DepreciationProperty); }
            set { this.SetProperty(DepreciationProperty, value); }
        }
        #endregion


    }
    internal class FinancialCategoryConfig : EntityConfig<FinancialCategory>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FINANCIAL_CATEGOYR").MapAllProperties();
            Meta.Property(FinancialCategory.DescProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
