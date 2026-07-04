using SIE.Core.Enums;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.InspectionRules
{
    /// <summary>
    /// 检验规程查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class InspectionRuleCriteria : Criteria
    {
        #region 规则编码 Code
        /// <summary>
        /// 规则编码
        /// </summary>
        [Label("规则编码")]
        public static readonly Property<string> CodeProperty = P<InspectionRuleCriteria>.Register(e => e.Code);

        /// <summary>
        /// 规则编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 规则名称 Name
        /// <summary>
        /// 规则名称
        /// </summary>
        [Label("规则名称")]
        public static readonly Property<string> NameProperty = P<InspectionRuleCriteria>.Register(e => e.Name);

        /// <summary>
        /// 规则名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 项目类型 InspectionRuleType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目类型")]
        public static readonly Property<InspectionRuleType?> InspectionRuleTypeProperty = P<InspectionRuleCriteria>.Register(e => e.InspectionRuleType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public InspectionRuleType? InspectionRuleType
        {
            get { return GetProperty(InspectionRuleTypeProperty); }
            set { SetProperty(InspectionRuleTypeProperty, value); }
        }
        #endregion

        #region 校验类别 CheckCategory
        /// <summary>
        /// 校验类别
        /// </summary>
        [Label("校验类别")]
        public static readonly Property<CheckCategory?> CheckCategoryProperty = P<InspectionRuleCriteria>.Register(e => e.CheckCategory);

        /// <summary>
        /// 校验类别
        /// </summary>
        public CheckCategory? CheckCategory
        {
            get { return GetProperty(CheckCategoryProperty); }
            set { SetProperty(CheckCategoryProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<InspectionRuleCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 客户交期
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>检验规程列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<InspectionRuleController>().GetInspectionRuleList(this);
        }
    }
}
