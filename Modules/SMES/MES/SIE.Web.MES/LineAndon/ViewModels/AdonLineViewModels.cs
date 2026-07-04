using SIE.Common.Prints;
using SIE.Domain;
using SIE.MES.LineAndon;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.LineAndon.ViewModels
{
    /// <summary>
    /// 产线条码化打印 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("产线条码化打印")]
    public class AdonLineViewModels : ViewModel
    {
        #region 打印模板 Template
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty =
            P<AdonLineViewModels>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

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
            P<AdonLineViewModels>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 打印 视图
        /// </summary>
        internal class AdonLineViewModelConfig : WebViewConfig<AdonLineViewModels>
        {
            /// <summary>
            /// 配置明细视图
            /// </summary>
            protected override void ConfigDetailsView()
            {
                View.AssignAuthorize(typeof(SIE.MES.LineAndon.AndonLine));
                View.Property(p => p.Template).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var model = source as AdonLineViewModels;
                    Check.NotNull(model, nameof(AdonLineViewModels));
                    var qualifiedName = typeof(AndonLinePrintable).GetQualifiedName();
                    return RT.Service.Resolve<PrintsController>().GetPrintTemplates(qualifiedName, true);
                });
            }
        }
    }
}
