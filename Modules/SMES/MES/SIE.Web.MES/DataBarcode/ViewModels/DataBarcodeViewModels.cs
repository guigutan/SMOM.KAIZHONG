using SIE.Common.Prints;
using SIE.Domain;
using SIE.MES.DataBarcode;
using SIE.ObjectModel;
using SIE.Packages.ItemLabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.DataBarcode.ViewModels
{
    /// <summary>
    /// 数据条码化打印 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("数据条码化打印")]
    public class DataBarcodeViewModels : ViewModel
    {
        #region 打印模板 Template
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty =
            P<DataBarcodeViewModels>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double TemplateId
        {
            get { return (double)this.GetRefId(TemplateIdProperty); }
            set { this.SetRefId(TemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> TemplateProperty =
            P<DataBarcodeViewModels>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion

        //#region 是否子批次 IsSubBatch
        ///// <summary>
        ///// 是否子批次
        ///// </summary>
        //[Label("是否子批次")]
        //public static readonly Property<bool> IsSubBatchProperty = P<ItemLabelsViewModels>.Register(e => e.IsSubBatch);

        ///// <summary>
        ///// 是否子批次
        ///// </summary>
        //public bool IsSubBatch
        //{
        //    get { return this.GetProperty(IsSubBatchProperty); }
        //    set { this.SetProperty(IsSubBatchProperty, value); }
        //}
        //#endregion
    }

    /// <summary>
    /// 物料批次打印 视图
    /// </summary>
    internal class ItemLabelsViewModelConfig : WebViewConfig<DataBarcodeViewModels>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(SIE.MES.DataBarcode.DataBarcode));
            View.Property(p => p.Template).UseDataSource((source, pagingInfo, keyword) =>
            {
                var model = source as DataBarcodeViewModels;
                Check.NotNull(model, nameof(DataBarcodeViewModels));
                //var qualifiedName = model.IsSubBatch ? typeof(ItemLabelPrintable).GetQualifiedName() : typeof(ItemLabelsViewModels).GetQualifiedName();
                var qualifiedName = typeof(DataBarcodePrintable).GetQualifiedName();
                return RT.Service.Resolve<PrintsController>().GetPrintTemplates(qualifiedName, true);
            });
        }
    }
}
