using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 分配规则
    /// </summary>
    [QueryEntity, Serializable]
    [Label("分配规则查询")]
    public partial class AssignRuleCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [MaxLength(20)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<AssignRuleCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(20)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<AssignRuleCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State?> StateProperty = P<AssignRuleCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        /// <returns>返回结果</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<RuleController>().GetAssignRule(this);
        }
    }
}
