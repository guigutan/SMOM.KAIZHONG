using SIE.Common.Prints;
using SIE.Domain;
using SIE.MES.DataBarcode;
using SIE.MES.ItemEquipAccount;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemEquipAccount.ViewModels
{
    /// <summary>
    /// 模具条码化打印 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("模具条码化打印")]
    public class EquipAccountItemViewModels : ViewModel
    {
        #region 打印模板 Template
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty =
            P<EquipAccountItemViewModels>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

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
            P<EquipAccountItemViewModels>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
    #endregion
    }
    /// <summary>
    /// 模具条码化打印 视图
    /// </summary>
    internal class ItemLabelsViewModelConfig : WebViewConfig<EquipAccountItemViewModels>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(SIE.MES.DataBarcode.DataBarcode));
            View.Property(p => p.Template).UseDataSource((source, pagingInfo, keyword) =>
            {
                var model = source as EquipAccountItemViewModels;
                Check.NotNull(model, nameof(EquipAccountItemViewModels));
                var qualifiedName = typeof(EquipAccountItemPrintable).GetQualifiedName();
                return RT.Service.Resolve<PrintsController>().GetPrintTemplates(qualifiedName, true);
            });
        }
    }
}
