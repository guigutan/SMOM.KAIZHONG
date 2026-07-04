using SIE.Domain;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.ProductIntfc.InspSettings;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SIE.Web.ProductIntfc.InspSettings
{
    /// <summary>
    /// 报检参数视图类
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class InspParameterViewConfig : WebViewConfig<InspParameter>
    {
        #region 产品族，工序是否只读
        /// <summary>
        /// 产品族，工序是否只读
        /// </summary>
        public static Expression<Func<InspParameter, bool>> ProcIsReadOnlyProperty { get; } =
            e => e.ProcessType != InspProcess.Custom;

        #endregion

        #region 报检类型为首件时，报检维度只能是数量 isFirstProduct
        /// <summary>
        /// 报检类型为首件时，报检维度只能是数量,默认选择首工序，为成品时默认选择最后工序
        /// </summary>
        public static Expression<Func<InspParameter, bool>> isFirstProductProperty { get; } =
            e => e.InspType == InspType.FirstProduct;
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
            View.UseDefaultCommands().UseImportCommands();
            View.ReplaceCommands(WebCommandNames.Add, "SIE.Web.ProductIntfc.InspSettings.Commands.InspParameterAddCommand");
            View.ReplaceCommands(WebCommandNames.Edit, "SIE.Web.ProductIntfc.InspSettings.Commands.InspParameterEditCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.Product).HasLabel("产品编码").UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProductName), nameof(e.Product.Name));
                    m.DicLinkField = keyValues;
                }).HasOrderNo(10);
                View.Property(p => p.ProductName).HasLabel("产品名称").Readonly(true).HasOrderNo(20);
                View.Property(p => p.InspType).HasOrderNo(30);
                View.Property(p => p.ProcessType).Readonly(false).HasOrderNo(40);
                View.Property(p => p.ProductFamily).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProductFamilyName), nameof(e.ProductFamily.Name));
                    m.DicLinkField = keyValues;
                })
                    .Readonly(ProcIsReadOnlyProperty).Cascade(p => p.InspProcess, null)
                    .UseListSetting(e => { e.HelpInfo = "工序类型等于自定义工序可编辑"; }).HasOrderNo(50);
                View.Property(p => p.ProductFamilyName).Readonly(true).HasOrderNo(55);
                View.Property(p => p.InspProcess).UseDataSource((e, p, s) =>
                {
                    var insp = e as InspParameter;
                    if (insp == null || !insp.ProductFamilyId.HasValue)
                        return new EntityList<Process>();
                    else
                        return RT.Service.Resolve<ProcessController>().GetProcessListByCategory(insp.ProductFamilyId.Value, s, p);
                }).UsePagingLookUpEditor().Readonly(ProcIsReadOnlyProperty).HasOrderNo(60)
                        .UseListSetting(e => { e.HelpInfo = "显示当前产品族下的工序列表,工序类型等于自定义工序可编辑"; });
                View.Property(p => p.InspDimension).Readonly(isFirstProductProperty).HasOrderNo(70)
                        .UseListSetting(e => { e.HelpInfo = "报检类型等于首件不可编辑"; });
                View.Property(p => p.InspParm).Readonly(true).Readonly(false).UseSpinEditor(e => { e.MinValue = 0; }).HasOrderNo(80);
                View.Property(p => p.CreateDate).HasOrderNo(90).Readonly(true);
                View.Property(p => p.CreateByName).HasOrderNo(100).Readonly(true);
                View.Property(p => p.UpdateDate).HasOrderNo(110).Readonly(true);
                View.Property(p => p.UpdateByName).HasOrderNo(120).Readonly(true);
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

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Product.Code).HasLabel("产品编码");
            View.Property(p => p.InspType);
            View.Property(p => p.ProcessType);
            View.PropertyRef(p => p.ProductFamily.Code).HasLabel("产品族编码");
            View.PropertyRef(p => p.InspProcess.Name).HasLabel("报检工序");
            View.Property(p => p.InspDimension);
            View.Property(p => p.InspParm);
        }
    }
}
