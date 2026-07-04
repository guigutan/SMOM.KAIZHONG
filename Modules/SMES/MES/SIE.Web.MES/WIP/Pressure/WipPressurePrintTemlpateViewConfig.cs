using SIE.MES.WIP.Pressure;
using SIE.Web.MES.WIP.Pressure.Commands;
using System.Collections.Generic;

namespace SIE.Web.MES.WIP.Pressure
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class WipPressurePrintTemlpateViewConfig : WebViewConfig<WipPressurePrintTemlpate>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseCommands(typeof(WipPressurePrintTPLImportCommand).FullName, typeof(WipPressurePrintTPLDLTemplateCommand).FullName);
            using (View.OrderProperties())
            {

                View.Property(p => p.Customer).ShowInList(150);
                View.Property(p => p.Product).ShowInList(150).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProductName), nameof(e.Product.Name));
                    m.DicLinkField = keyValues;
                });
                View.Property(p => p.ProductName).ShowInList(200).Readonly();
                View.Property(p => p.NumberRule).ShowInList(150);
                //View.Property(p => p.NumberRule2).ShowInList(150);
                View.Property(p => p.PrintTemplate).ShowInList(150);
                //View.Property(p => p.PrinterSettingTpl).ShowInList(150);


            }
        }
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Customer).Show();
            View.Property(p => p.ProductCode).Show().Readonly(false);
            View.Property(p => p.ProductName).Show().Readonly(false);
        }

        /// <summary>
        /// 配置数据导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Customer).Show();
            View.PropertyRef(p => p.Product.Code).Show().HasLabel("产品编码");
            View.PropertyRef(p => p.NumberRule.Name).Show().HasLabel("条码规则");
            View.PropertyRef(p => p.PrintTemplate.FileName).Show().HasLabel("打印模板");
        }
    }
}