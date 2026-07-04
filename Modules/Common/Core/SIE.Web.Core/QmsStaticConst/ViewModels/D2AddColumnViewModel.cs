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
    public class D2AddColumnViewModel : ViewModel
    {
        #region TestQty TestQty

        /// <summary>
        /// TestQty
        /// </summary>
        [Label("测量次数（单值）")]
        [Required]
        public static readonly Property<string> TestQtyProperty = P<D2AddColumnViewModel>.Register(e => e.TestQty);

        /// <summary>
        /// 测量次数
        /// </summary>
        public string TestQty
        {
            get { return this.GetProperty(TestQtyProperty); }
            set { this.SetProperty(TestQtyProperty, value); }
        }

        #endregion

        #region D2 D2

        /// <summary>
        /// v
        /// </summary>
        [Label("d₂（单值）")]
        public static readonly Property<string> D2Property = P<D2AddColumnViewModel>.Register(e => e.D2);

        /// <summary>
        /// v
        /// </summary>
        public string D2
        {
            get { return this.GetProperty(D2Property); }
            set { this.SetProperty(D2Property, value); }
        }

        #endregion

        #region Cd Cd

        /// <summary>
        /// cd
        /// </summary>
        [Label("cd（单值）")]
        public static readonly Property<string> CdProperty = P<D2AddColumnViewModel>.Register(e => e.Cd);

        /// <summary>
        /// v
        /// </summary>
        public string Cd
        {
            get { return this.GetProperty(CdProperty); }
            set { this.SetProperty(CdProperty, value); }
        }

        #endregion

        #region V v

        /// <summary>
        /// V
        /// </summary>
        [Label("V（英文分号分隔）")]
        public static readonly Property<string> VProperty = P<D2AddColumnViewModel>.Register(e => e.V);

        /// <summary>
        /// V
        /// </summary>
        public string V
        {
            get { return this.GetProperty(VProperty); }
            set { this.SetProperty(VProperty, value); }
        }

        #endregion

        #region D2S D2S

        /// <summary>
        /// D2S
        /// </summary>
        [Label("d₂*（英文分号分隔）")]
        public static readonly Property<string> D2SProperty = P<D2AddColumnViewModel>.Register(e => e.D2S);

        /// <summary>
        /// D2S
        /// </summary>
        public string D2S
        {
            get { return this.GetProperty(D2SProperty); }
            set { this.SetProperty(D2SProperty, value); }
        }

        #endregion
    }

    internal class D2AddColumnViewModelViewConfig : WebViewConfig<D2AddColumnViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(1);
            View.Property(c => c.TestQty).UseSpinEditor(c => { c.AllowDecimals = false; c.AllowNegative = false; c.Step = 1; c.MinValue = 1; });
            View.Property(c => c.D2).UseSpinEditor(c => { c.AllowDecimals = true; c.AllowNegative = false; c.DecimalPrecision = 5; c.MinValue = 0.00001; });
            View.Property(c => c.Cd).UseSpinEditor(c => { c.AllowDecimals = true; c.AllowNegative = false; c.DecimalPrecision = 5; c.MinValue = 0.00001; });
            View.Property(c => c.V).UseMemoEditor(c => { c.Width = "400"; c.Height = "300"; });
            View.Property(c => c.D2S).UseMemoEditor(c => { c.Width = "400"; c.Height = "300"; });
        }
    }

}
