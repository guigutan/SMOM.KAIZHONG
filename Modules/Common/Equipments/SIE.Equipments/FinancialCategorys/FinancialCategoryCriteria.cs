using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.FinancialCategorys
{

    /// <summary>
    /// 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("查询实体")]
    public  class FinancialCategoryCriteria:Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<FinancialCategoryCriteria>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<FinancialCategoryCriteria>.Register(e => e.Name);

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
        public static readonly Property<string> DescProperty = P<FinancialCategoryCriteria>.Register(e => e.Desc);

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc
        {
            get { return this.GetProperty(DescProperty); }
            set { this.SetProperty(DescProperty, value); }
        }
        #endregion


        #region 创建日期 CreationDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreationDateProperty = P<FinancialCategoryCriteria>.Register(e => e.CreationDate,
            new PropertyMetadata<DateRange>() { DateTimePart = DateTimePart.DateTime, });

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreationDate
        {
            get { return GetProperty(CreationDateProperty); }
            set { SetProperty(CreationDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<FinancialCategoryController>().Fetch(this);
        }
    }
}
