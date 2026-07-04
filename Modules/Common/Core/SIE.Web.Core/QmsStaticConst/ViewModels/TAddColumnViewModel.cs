using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Core.QmsStaticConst.ViewModels
{
    /// <summary>
    /// D2添加列ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class TAddColumnViewModel : ViewModel
    {


        #region Alpha Alpha

        /// <summary>
        /// n
        /// </summary>
        [Label("α（单值）")]
        [Required]
        public static readonly Property<string> AlphaProperty = P<TAddColumnViewModel>.Register(e => e.Alpha);

        /// <summary>
        /// n
        /// </summary>
        public string Alpha
        {
            get { return this.GetProperty(AlphaProperty); }
            set { this.SetProperty(AlphaProperty, value); }
        }

        #endregion

        #region SampleQty SampleQty

        /// <summary>
        /// SampleQty
        /// </summary>
        [Label("t（英文分号分隔）")]
        public static readonly Property<string> SampleQtyProperty = P<TAddColumnViewModel>.Register(e => e.SampleQty);

        /// <summary>
        /// SampleQty
        /// </summary>
        public string SampleQty
        {
            get { return this.GetProperty(SampleQtyProperty); }
            set { this.SetProperty(SampleQtyProperty, value); }
        }

        #endregion
    }

    internal class TAddColumnViewModelViewConfig : WebViewConfig<TAddColumnViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(1);
            View.Property(c => c.Alpha).UseSpinEditor(c=> { c.MinValue = 0.001;c.DecimalPrecision = 3;c.AllowDecimals = true; });
            View.Property(c => c.SampleQty).UseMemoEditor(c => { c.Width = "400"; c.Height = "300"; });
        }
    }

}
