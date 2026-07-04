using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Resources.Employees.ViewModels
{
    /// <summary>
    /// 人员条码化打印 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("人员条码化打印")]
    public class EmployeeViewModels : ViewModel
    {
        #region 打印模板 Template
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty =
            P<EmployeeViewModels>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

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
            P<EmployeeViewModels>.RegisterRef(e => e.Template, TemplateIdProperty);

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
    /// 物料批次打印 视图
    /// </summary>
    internal class ItemLabelsViewModelConfig : WebViewConfig<EmployeeViewModels>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(Employee));
            View.Property(p => p.Template).UseDataSource((source, pagingInfo, keyword) =>
            {
                var model = source as EmployeeViewModels;
                Check.NotNull(model, nameof(EmployeeViewModels));
                //var qualifiedName = model.IsSubBatch ? typeof(ItemLabelPrintable).GetQualifiedName() : typeof(ItemLabelsViewModels).GetQualifiedName();
                var qualifiedName = typeof(EmployeePrintable).GetQualifiedName();
                return RT.Service.Resolve<PrintsController>().GetPrintTemplates(qualifiedName, true);
            });
        }
    }
}
