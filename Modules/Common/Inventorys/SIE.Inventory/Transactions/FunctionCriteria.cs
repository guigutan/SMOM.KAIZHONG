using SIE.Common;
using SIE.Domain;
using SIE.Inventory.Commom;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Packages.Boxs;
using SIE.Warehouses;
using System;

namespace SIE.Inventory.Transactions
{
    /// <summary>
    /// 垛查询实体
    /// </summary>
    [RootEntity, Serializable]
    public class FunctionCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<FunctionCriteria>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<FunctionCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<FunctionCriteria>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return this.GetProperty(DescriptionProperty); }
            set { this.SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 数据来源类型 SourceType
        /// <summary>
        /// 数据来源类型
        /// </summary>
        [Label("数据来源类型")]
        public static readonly Property<SourceType?> SourceTypeProperty = P<FunctionCriteria>.Register(e => e.SourceType);

        /// <summary>
        /// 数据来源类型
        /// </summary>
        public SourceType? SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State?> StateProperty = P<FunctionCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 类别 FunctionType
        /// <summary>
        /// 类别
        /// </summary>
        [Label("类别")]
        public static readonly Property<FunctionType?> FunctionTypeProperty = P<FunctionCriteria>.Register(e => e.FunctionType);

        /// <summary>
        /// 类别
        /// </summary>
        public FunctionType? FunctionType
        {
            get { return this.GetProperty(FunctionTypeProperty); }
            set { this.SetProperty(FunctionTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<TransactionController>().GetFunctions(this);
        }
    }
}
