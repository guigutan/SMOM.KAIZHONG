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
    public class D2AddRowViewModel : ViewModel
    {
        #region N n

        /// <summary>
        /// n
        /// </summary>
        [Label("n（单值）")]
        public static readonly Property<string> NProperty = P<D2AddRowViewModel>.Register(e => e.N);

        /// <summary>
        /// n
        /// </summary>
        public string N
        {
            get { return this.GetProperty(NProperty); }
            set { this.SetProperty(NProperty, value); }
        }

        #endregion

        #region V v

        /// <summary>
        /// v
        /// </summary>
        [Label("V（英文分号分隔）")]
        public static readonly Property<string> VProperty = P<D2AddRowViewModel>.Register(e => e.V);

        /// <summary>
        /// v
        /// </summary>
        public string V
        {
            get { return this.GetProperty(VProperty); }
            set { this.SetProperty(VProperty, value); }
        }

        #endregion

        #region D2 D2

        /// <summary>
        /// D2
        /// </summary>
        [Label("d₂*（英文分号分隔）")]
        public static readonly Property<string> D2Property = P<D2AddRowViewModel>.Register(e => e.D2);

        /// <summary>
        /// D2
        /// </summary>
        public string D2
        {
            get { return this.GetProperty(D2Property); }
            set { this.SetProperty(D2Property, value); }
        }

        #endregion
    }

    internal class D2AddRowViewModelViewConfig : WebViewConfig<D2AddRowViewModel>
    {

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(1);
            View.Property(c => c.N).UseSpinEditor(c => { c.AllowDecimals = false; c.AllowNegative = false; c.Step = 1; c.MinValue = 1; });
            View.Property(c => c.V).UseMemoEditor(c=> { c.Width = "400";c.Height = "300"; });
            View.Property(c => c.D2).UseMemoEditor(c => { c.Width = "400"; c.Height = "300"; });
        }
    }

}
