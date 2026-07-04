using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.CSM.Customers
{
    /// <summary>
    /// 客户查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("客户查询实体")]
    public class CustomerCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<CustomerCriteria>.Register(e => e.Code);

        /// <summary>
        /// 生产部门
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
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<CustomerCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 类型 CustomerType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<CustomerType?> CustomerTypeProperty = P<CustomerCriteria>.Register(e => e.CustomerType);

        /// <summary>
        /// 类型
        /// </summary>
        public CustomerType? CustomerType
        {
            get { return GetProperty(CustomerTypeProperty); }
            set { SetProperty(CustomerTypeProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State?> StateProperty = P<CustomerCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 是否承运人 IsCarrier
        /// <summary>
        /// 是否承运人
        /// </summary>
        [Label("是否承运人")]
        public static readonly Property<bool> IsCarrierProperty = P<CustomerCriteria>.Register(e => e.IsCarrier);

        /// <summary>
        /// 是否承运人
        /// </summary>
        public bool IsCarrier
        {
            get { return GetProperty(IsCarrierProperty); }
            set { SetProperty(IsCarrierProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<CustomerController>().GetCustomerDatas(this);
        }
    }
}
