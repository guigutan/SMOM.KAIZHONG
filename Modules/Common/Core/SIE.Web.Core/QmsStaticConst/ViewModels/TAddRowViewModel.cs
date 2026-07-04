using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Core.QmsStaticConst.ViewModels
{
    /// <summary>
    /// 多值输入ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class TAddRowViewModel : ViewModel
    {
        #region Alpha Alpha

        /// <summary>
        /// n
        /// </summary>
        [Label("n（单值）")]
        public static readonly Property<string> SampleQtyProperty = P<TAddRowViewModel>.Register(e => e.SampleQty);

        /// <summary>
        /// n
        /// </summary>
        public string SampleQty
        {
            get { return this.GetProperty(SampleQtyProperty); }
            set { this.SetProperty(SampleQtyProperty, value); }
        }

        #endregion

        #region Alpha Alpha

        /// <summary>
        /// t
        /// </summary>
        [Label("t（英文分号分隔）")]
        public static readonly Property<string> AlphaProperty = P<TAddRowViewModel>.Register(e => e.Alpha);

        /// <summary>
        /// n
        /// </summary>
        public string Alpha
        {
            get { return this.GetProperty(AlphaProperty); }
            set { this.SetProperty(AlphaProperty, value); }
        }

        #endregion
    }

    internal class TAddRowViewModelViewConfig : WebViewConfig<TAddRowViewModel>
    {

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(1);
            View.Property(c => c.SampleQty).UseSpinEditor(c => { c.MinValue = 1; c.AllowDecimals = false; c.AllowNegative = false; });
            View.Property(c => c.Alpha).UseMemoEditor(c=> { c.Width = "400";c.Height = "300"; });
        }
    }

}
