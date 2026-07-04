using SIE.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.Wpf.Barcodes.ViewModels
{
    /// <summary>
    /// 条码报废 ViewModel
    /// </summary>
    [Serializable, RootEntity]
    [Label("条码报废")]
    public class ScrapBarcodeViewModel : ViewModel
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="barcodeList">条码列表</param>
        public ScrapBarcodeViewModel(IEnumerable<Barcode> barcodeList)
        {
            BarcodeList.AddRange(barcodeList);
        }
        #endregion

        #region 报废原因 Reason
        /// <summary>
        /// 报废原因
        /// </summary>
        [Label("报废原因")]
        public static readonly Property<string> ReasonProperty = P<ScrapBarcodeViewModel>.Register(e => e.Reason);

        /// <summary>
        /// 报废原因
        /// </summary>
        public string Reason
        {
            get { return this.GetProperty(ReasonProperty); }
            set { this.SetProperty(ReasonProperty, value); }
        }
        #endregion

        #region 条码列表
        /// <summary>
        /// 条码列表
        /// </summary>
        public EntityList<Barcode> BarcodeList { get; set; } = new EntityList<Barcode>();
        #endregion
    }

    /// <summary>
    /// 条码报废 实体配置
    /// </summary>
    internal class ScrapBarcodeViewModelConfig : EntityConfig<ScrapBarcodeViewModel>
    {
        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.Add(ScrapBarcodeViewModel.ReasonProperty, new RequiredRule()
            {
                MessageBuilder = (e) => "原因 不能为空".L10N()
            });
        }
    }

    /// <summary>
    /// 条码报废 视图
    /// </summary>
    internal class ScrapBarcodeViewModelViewConfig : WPFViewConfig<ScrapBarcodeViewModel>
    {
        /// <summary>
        /// 明细视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(Barcode));
            View.ClearCommands();
            View.Property(p => p.Reason).UseMemoEditor();
        }
    }
}