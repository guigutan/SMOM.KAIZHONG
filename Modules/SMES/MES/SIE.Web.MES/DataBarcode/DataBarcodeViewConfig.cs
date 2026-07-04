using SIE.MES.DataBarcode;
using SIE.MetaModel.View;
using SIE.Web.Common.Configs.Commands;
using SIE.Web.MES.DataBarcode.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.DataBarcode
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class DataBarcodeViewConfig : WebViewConfig<SIE.MES.DataBarcode.DataBarcode>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            //View.HasDelegate(SIE.MES.DataBarcode.DataBarcode.NameProperty);
            //View.UseDefaultBehaviors();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(DataBarcodeDeleteCommand).FullName, typeof(DataBarcodeImportCommand).FullName,typeof(DataBarcodeLabelPrintCommand).FullName);
            View.Property(p => p.BarcodeType).ShowInList(width: 150);
            View.Property(p => p.BarcodeSite).ShowInList(width: 150);
            View.Property(p => p.BarcodeParam1).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam2).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam3).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam4).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam5).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam6).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam7).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam8).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam9).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam10).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam11).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam12).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam13).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam14).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam15).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam16).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam17).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam18).ShowInList(width: 150); 
            View.Property(p => p.BarcodeParam19).ShowInList(width: 150); 
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {

        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {

        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.BarcodeType);
            View.Property(p => p.BarcodeSite);
            View.Property(p => p.BarcodeParam1);
            View.Property(p => p.BarcodeParam2);
            View.Property(p => p.BarcodeParam3);
            View.Property(p => p.BarcodeParam4);
            View.Property(p => p.BarcodeParam5);
        }
    }
}
