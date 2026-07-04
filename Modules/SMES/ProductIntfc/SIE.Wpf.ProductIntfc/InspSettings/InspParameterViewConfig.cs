using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.ProductIntfc.InspSettings;
using SIE.Tech.Processs;
using SIE.Wpf.InspSettings.ProductIntfcs.Commands;
using SIE.Wpf.ProductIntfc.InspSettings.Editors;

namespace SIE.Wpf.InspSettings.ProductIntfcs
{
    /// <summary>
    /// 报检参数视图类
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class InspParameterViewConfig : WPFViewConfig<InspParameter>
    {
        #region 产品族，工序是否只读
        /// <summary>
        /// 产品族，工序是否只读
        /// </summary>
        [Label("工序是否只读")]
        public static readonly Property<bool> ProcIsReadOnlyProperty = P<InspParameter>.RegisterExtensionReadOnly("ProcIsReadOnly", typeof(InspParameterViewConfig),
            GetProcIsReadOnly, InspParameter.ProcessTypeProperty);

        /// <summary>
        /// 产品族，工序是否只读
        /// </summary>
        /// <param name="me">实体InspParameter</param>
        /// <returns>bool</returns>
        public static bool GetProcIsReadOnly(InspParameter me)
        {
            if (me.ProcessType != InspProcess.Custom)
            {
                me.ProductFamily = null;
                me.InspProcess = null;
            }
            return me.ProcessType != InspProcess.Custom;
        }
        #endregion InspParameter

        #region 报检类型为成品时，工序类型默认首个工序 isProduct
        /// <summary>
        /// 报检类型为成品时，工序类型默认首个工序
        /// </summary> 
        public static readonly Property<bool> isProductProperty = P<InspParameter>.RegisterExtensionReadOnly("isProduct", typeof(InspParameterViewConfig),
            GetisProduct, InspParameter.InspTypeProperty);

        /// <summary>
        /// 报检类型为成品时，工序类型默认首个工序
        /// </summary>
        public static bool GetisProduct(InspParameter me)
        {
            if (me.InspType == InspType.Product)
                me.ProcessType = InspProcess.Last;
            if (me.InspType == InspType.FirstProduct)
            {
                me.ProcessType = InspProcess.First;
                me.InspDimension = InspDimension.BatchQty;
            }
            return false;
        }
        #endregion

        #region 报检类型为首件时，报检维度只能是数量 isFirstProduct
        /// <summary>
        /// 报检类型为首件时，报检维度只能是数量,默认选择首工序，为成品时默认选择最后工序
        /// </summary> 
        public static readonly Property<bool> isFirstProductProperty = P<InspParameter>.RegisterExtensionReadOnly("isFirstProduct", typeof(InspParameterViewConfig),
            GetisFirstProduct, InspParameter.InspTypeProperty);

        /// <summary>
        /// 报检类型为首件时，报检维度只能是数量
        /// </summary>
        public static bool GetisFirstProduct(InspParameter me)
        {
            return me.InspType == InspType.FirstProduct;
        }
        #endregion

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            //方法重写
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultBehaviors();
            View.UseCommands(typeof(InspParameterAddCommand));
            View.UseDefaultCommands();
            View.RemoveCommands(WPFCommandNames.ListAdd);

            using (View.OrderProperties())
            {
                View.Property(p => p.ProductId).HasLabel("产品编码").UseEditor(InspParamLookUpEditor.EditorName);
                View.Property(p => p.ProductName).HasLabel("产品名称").Readonly(true);
                View.Property(p => p.InspType).Readonly(isProductProperty);
                View.Property(p => p.ProcessType).Readonly(false);
                View.Property(p => p.ProductFamily).UsePagingLookUpEditor(p => p.DisplayMember = nameof(ProductFamily.Name)).Readonly(ProcIsReadOnlyProperty);
                View.Property(p => p.InspProcess).UseDataSource((e, p, s) =>
                {
                    var insp = e as InspParameter;
                    if (insp == null || !insp.ProductFamilyId.HasValue)
                        return new EntityList<Process>();
                    else
                        return RT.Service.Resolve<ProcessController>().GetProcessListByCategory(insp.ProductFamilyId.Value, s, p);
                }).UsePagingLookUpEditor().Readonly(ProcIsReadOnlyProperty);
                View.Property(p => p.InspDimension).Readonly(isFirstProductProperty);
                View.Property(p => p.InspParm).Readonly(true).Readonly(false).UseSpinEditor(e => { e.MinValue = 0; });
                View.Property(p => p.CreateDate).Readonly(true);
                View.Property(p => p.CreateByName).Readonly(true);
                View.Property(p => p.UpdateDate).Readonly(true);
                View.Property(p => p.UpdateByName).Readonly(true);
            }
        }

        /// <summary>
        /// 弹出窗体配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            base.ConfigSelectionView();
            View.Property(p => p.Product).Readonly(false);
            View.Property(p => p.InspType).Readonly(false);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            base.ConfigQueryView();
            View.Property(p => p.Product).Readonly(false);
            View.Property(p => p.InspType).Readonly(false);
        }
    }
}
