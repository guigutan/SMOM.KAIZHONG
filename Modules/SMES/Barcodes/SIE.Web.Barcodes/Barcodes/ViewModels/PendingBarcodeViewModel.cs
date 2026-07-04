using SIE.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.Web.Barcodes.ViewModels
{
    /// <summary>
    /// 条码挂起 ViewModel
    /// </summary>
    [Serializable, RootEntity]
    [Label("条码挂起")]
    public class PendingBarcodeViewModel : ViewModel
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="barcodeList">条码列表</param>
        public PendingBarcodeViewModel(IEnumerable<Barcode> barcodeList)
        {
            BarcodeList.AddRange(barcodeList);
        }
        #endregion

        #region 挂起原因 Reason
        /// <summary>
        /// 挂起原因
        /// </summary>
        [Label("挂起原因")]
        public static readonly Property<string> ReasonProperty = P<PendingBarcodeViewModel>.Register(e => e.Reason);

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
        EntityList<Barcode> BarcodeList = new EntityList<Barcode>();
        #endregion
    }

    /// <summary>
    /// 条码挂起 实体配置
    /// </summary>
    internal class PendingBarcodeViewModelConfig : EntityConfig<PendingBarcodeViewModel>
    {
        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.Add(PendingBarcodeViewModel.ReasonProperty, new RequiredRule()
            {
                MessageBuilder = (e) => "原因 不能为空".L10N()
            });
        }
    }

    /// <summary>
    /// 条码挂起 视图
    /// </summary>
    internal class PendingBarcodeViewModelViewConfig : WebViewConfig<PendingBarcodeViewModel>
    {
        /// <summary>
        /// 明细视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(Barcode));
            View.ClearCommands();
            View.Property(p => p.Reason);
        }
    }
}