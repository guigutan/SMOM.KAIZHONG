using SIE.Core.Prints;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;

namespace SIE.Web.Core.Prints
{
    /// <summary>
    /// KZ打印模板设置
    /// </summary>
    public class PrinterSettingTplViewConfig : WPFViewConfig<PrinterSettingTpl>
    {
        /// <summary>
        /// 视图函数
        /// </summary>
        protected override void ConfigView()
        {
            base.ConfigView();
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //View.UseCommands(WebCommandNames.ExportXlsAll, typeof(InitDefaultTplCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.TplName).ShowInList(200);
                //View.Property(p => p.PrinterName).Show();
                View.Property(p => p.PageWidth).Show();
                View.Property(p => p.PageHeight).Show();
                View.Property(p => p.MarginsLeft).Show();
                View.Property(p => p.MarginsTop).Show();
                View.Property(p => p.MarginsRight).Show();
                View.Property(p => p.MarginsBottom).Show();
                //View.Property(p => p.QrcodeX).Show();
                //View.Property(p => p.QrcodeY).Show();
                //View.Property(p => p.QrcodeWidth).Show();
                //View.Property(p => p.QrcodeHeight).Show();
                //View.Property(p => p.ProjectNameX).Show();
                //View.Property(p => p.ProjectNameY).Show();
                //View.Property(p => p.ProjectFontName).Show();
                //View.Property(p => p.ProjectFontSize).Show();
                //View.Property(p => p.ProjectFontBold).Show();
                //View.Property(p => p.CodeStrX).Show();
                //View.Property(p => p.CodeStrY).Show();
                //View.Property(p => p.CodeStrFontName).Show();
                //View.Property(p => p.CodeStrFontSize).Show();
                //View.Property(p => p.CodeStrFontBold).Show();
                //View.Property(p => p.CodeStrLineSize).Show();
                //View.Property(p => p.CodeStrLineHeight).Show();
            }
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            //View.UseCommands(WPFCommandNames.FormSave);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("页面设置".L10N(), 2))
                {
                    View.Property(p => p.TplName).ShowInDetail(columnSpan: 1).Readonly();
                    View.Property(p => p.Resolution).Show();

                    View.Property(p => p.PageWidth).Show().UseSpinEditor(p => { p.MinValue = 0; });
                    View.Property(p => p.PageHeight).Show().UseSpinEditor(p => { p.MinValue = 0; });

                    View.Property(p => p.MarginsLeft).Show().UseSpinEditor(p => {  });
                    View.Property(p => p.MarginsTop).Show().UseSpinEditor(p => {  });
                    View.Property(p => p.MarginsRight).Show().UseSpinEditor(p => {  });
                    View.Property(p => p.MarginsBottom).Show().UseSpinEditor(p => {  });
                }
                //using (View.DeclareGroup("二维码图片".L10N(), 4))
                //{
                //    View.Property(p => p.QrcodeX).Show().HasLabel("位置X").UseSpinEditor(p => { p.MinValue = 0; });
                //    View.Property(p => p.QrcodeY).Show().HasLabel("位置Y").UseSpinEditor(p => { p.MinValue = 0; });
                //    View.Property(p => p.QrcodeWidth).Show().HasLabel("宽").UseSpinEditor(p => { p.MinValue = 0; });
                //    View.Property(p => p.QrcodeHeight).Show().HasLabel("高").UseSpinEditor(p => { p.MinValue = 0; });
                //}
                //using (View.DeclareGroup("产品名称".L10N(), 5))
                //{
                //    View.Property(p => p.ProjectNameX).Show().HasLabel("位置X").UseSpinEditor(p => { p.MinValue = 0; });
                //    View.Property(p => p.ProjectNameY).Show().HasLabel("位置Y").UseSpinEditor(p => { p.MinValue = 0; });
                //    View.Property(p => p.ProjectFontName).Show().HasLabel("字体").UseDropDownEditor(() =>
                //    {
                //        return GetFontFamilys();
                //    });
                //    View.Property(p => p.ProjectFontSize).Show().HasLabel("字体大小").UseSpinEditor(p => { p.MinValue = 1; });
                //    View.Property(p => p.ProjectFontBold).Show().HasLabel("加粗");
                //}
                //using (View.DeclareGroup("二维码字符".L10N(), 5))
                //{
                //    View.Property(p => p.CodeStrX).Show().HasLabel("位置X").UseSpinEditor(p => { p.MinValue = 0; });
                //    View.Property(p => p.CodeStrY).Show().HasLabel("位置Y").UseSpinEditor(p => { p.MinValue = 0; });
                //    View.Property(p => p.CodeStrFontName).Show().HasLabel("字体").UseDropDownEditor(() =>
                //    {
                //        return GetFontFamilys();
                //    });
                //    View.Property(p => p.CodeStrFontSize).Show().HasLabel("字体大小").UseSpinEditor(p => { p.MinValue = 1; });
                //    View.Property(p => p.CodeStrFontBold).Show().HasLabel("加粗");
                //    View.Property(p => p.CodeStrLineSize).Show().HasLabel("每行长度").UseSpinEditor(p => { p.MinValue = 1; });
                //    View.Property(p => p.CodeStrLineHeight).Show().HasLabel("行高").UseSpinEditor(p => { p.MinValue = 1; });
                //}
            }
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.TplName).ShowInList(200).Readonly();
            }
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.TplName).ShowInList(200);
            }
        }

        /// <summary>
        /// 获取本机字体列表
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, string> GetFontFamilys()
        {
            var dic = new Dictionary<string, string>();
            // 创建一个字体集合
            InstalledFontCollection installedFonts = new InstalledFontCollection();

            // 获取字体族数组
            FontFamily[] fontFamilies = installedFonts.Families;

            // 遍历并打印每个字体族名称
            foreach (FontFamily fontFamily in fontFamilies)
            {
                dic.Add(fontFamily.Name, fontFamily.Name);
            }

            return dic;
        }
    }
}
