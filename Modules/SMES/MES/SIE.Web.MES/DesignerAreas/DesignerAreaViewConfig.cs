using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Domain;
using SIE.MES.DesignerAreas;
using SIE.MetaModel.View;
using SIE.Web.MES.BlueLable.Commands;
using SIE.Web.MES.DesignerAreas.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.MES.DesignerAreas
{
    public class DesignerAreaViewConfig : WebViewConfig<DesignerArea>
    {
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);

            View.UseCommands(typeof(DesignerAreaImportCommand).FullName, typeof(DesignerAreaDLTemplateCommand).FullName);

            using (View.OrderProperties())
            {
                View.Property(p => p.AreaCode).ShowInList(width: 100);
                View.Property(p => p.AreaName).ShowInList(width: 100);
            }
            View.Property(p => p.AreaCode).ShowInList(width: 100);
            View.Property(p => p.AreaName).ShowInList(width: 100); 

        }


        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.AreaCode).ShowInList(width: 150);
            View.Property(p => p.AreaName).ShowInList(width: 150);           
        }


    }
}
