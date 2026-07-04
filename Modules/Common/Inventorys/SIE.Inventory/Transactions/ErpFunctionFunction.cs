using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Transactions
{
    /// <summary>
    /// ERP功能与功能关系
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("ERP功能与大类关系")]
    public partial class ErpFunctionFunction : DataEntity
    {
        #region ERP功能 ErpFunction
        /// <summary>
        /// ERP功能Id
        /// </summary>
        public static readonly IRefIdProperty ErpFunctionIdProperty = P<ErpFunctionFunction>.RegisterRefId(e => e.ErpFunctionId, ReferenceType.Normal);

        /// <summary>
        /// ERP功能Id
        /// </summary>
        public double ErpFunctionId
        {
            get { return (double)GetRefId(ErpFunctionIdProperty); }
            set { SetRefId(ErpFunctionIdProperty, value); }
        }

        /// <summary>
        /// ERP功能
        /// </summary>
        public static readonly RefEntityProperty<Function> ErpFunctionProperty = P<ErpFunctionFunction>.RegisterRef(e => e.ErpFunction, ErpFunctionIdProperty);

        /// <summary>
        /// ERP功能
        /// </summary>
        public Function ErpFunction
        {
            get { return GetRefEntity(ErpFunctionProperty); }
            set { SetRefEntity(ErpFunctionProperty, value); }
        }
        #endregion

        #region 单据大类 Function
        /// <summary>
        /// 单据大类
        /// </summary>
        public static readonly IRefIdProperty FunctionIdProperty = P<ErpFunctionFunction>.RegisterRefId(e => e.FunctionId, ReferenceType.Normal);

        /// <summary>
        /// 单据大类
        /// </summary>
        public double FunctionId
        {
            get { return (double)GetRefId(FunctionIdProperty); }
            set { SetRefId(FunctionIdProperty, value); }
        }

        /// <summary>
        /// 单据大类
        /// </summary>
        public static readonly RefEntityProperty<Function> FunctionProperty = P<ErpFunctionFunction>.RegisterRef(e => e.Function, FunctionIdProperty);

        /// <summary>
        /// 单据大类
        /// </summary>
        public Function Function
        {
            get { return GetRefEntity(FunctionProperty); }
            set { SetRefEntity(FunctionProperty, value); }
        }
        #endregion

        #region 单据大类编码 FunctionCode
        /// <summary>
        /// 单据大类编码
        /// </summary>
        [Label("单据大类编码")]
        public static readonly Property<string> FunctionCodeProperty = P<ErpFunctionFunction>.RegisterView(e => e.FunctionCode, p => p.Function.Code);

        /// <summary>
        /// 单据大类编码
        /// </summary>
        public string FunctionCode
        {
            get { return this.GetProperty(FunctionCodeProperty); }
        }
        #endregion

        #region ERP编码 ErpFunctionCode
        /// <summary>
        /// ERP编码
        /// </summary>
        [Label("ERP编码")]
        public static readonly Property<string> ErpFunctionCodeProperty = P<ErpFunctionFunction>.RegisterView(e => e.ErpFunctionCode, p => p.ErpFunction.Code);

        /// <summary>
        /// ERP编码
        /// </summary>
        public string ErpFunctionCode
        {
            get { return this.GetProperty(ErpFunctionCodeProperty); }
        }
        #endregion
    }

    /// <summary>
    /// ERP功能与功能关系 实体配置
    /// </summary>
    internal class ErpFunctionFunctionConfig : EntityConfig<ErpFunctionFunction>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("TRANS_ERP_FUNC_FUNC").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}