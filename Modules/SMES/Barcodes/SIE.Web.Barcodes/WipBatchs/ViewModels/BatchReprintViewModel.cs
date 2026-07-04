using SIE.Barcodes.Printables;
using SIE.Barcodes.WipBatchs;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Web.Barcodes.WipBatchs.ViewModels
{
    /// <summary>
    /// 生产批次补打 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("生产批次补打")]
    public class BatchReprintViewModel : ViewModel
    {
        #region 打印模板 Template
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty =
            P<BatchReprintViewModel>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

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
            P<BatchReprintViewModel>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion

        #region 是否子批次 IsSubBatch
        /// <summary>
        /// 是否子批次
        /// </summary>
        [Label("是否子批次")]
        public static readonly Property<bool> IsSubBatchProperty = P<BatchReprintViewModel>.Register(e => e.IsSubBatch);

        /// <summary>
        /// 是否子批次
        /// </summary>
        public bool IsSubBatch
        {
            get { return this.GetProperty(IsSubBatchProperty); }
            set { this.SetProperty(IsSubBatchProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 生产批次补打 视图
    /// </summary>
    internal class BatchReprintViewModelConfig : WebViewConfig<BatchReprintViewModel>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(WipBatch));
            View.Property(p => p.Template).UseDataSource((source, pagingInfo, keyword) =>
            {
                var model = source as BatchReprintViewModel;
                Check.NotNull(model, nameof(BatchReprintViewModel));
                var qualifiedName = model.IsSubBatch ? typeof(WipBatchPrintable).GetQualifiedName() : typeof(WipBatchPrintable).GetQualifiedName();
                return RT.Service.Resolve<PrintsController>().GetPrintTemplates(qualifiedName, true);
            });
        }
    }
}
