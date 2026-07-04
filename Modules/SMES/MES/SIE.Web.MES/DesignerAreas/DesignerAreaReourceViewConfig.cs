using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Domain;
using SIE.MES.DesignerAreas;
using SIE.MetaModel.View;
using SIE.Web.MES.DesignerAreas.Commands;
using System;

namespace SIE.Web.MES.DesignerAreas
{
    public class DesignerAreaReourceViewConfig : WebViewConfig<DesignerAreaResources>
    {
        protected override void ConfigListView()
        {                

            View.UseCommands(typeof(DesignerAreaResourceSelectCommand).FullName, typeof(DesignerAreaResourceSelectCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.ResourceCode).ShowInList(150);
                View.Property(p => p.ResourceName).ShowInList(150);
            }
            View.UseCommands(WebCommandNames.Delete, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            //View.UseCommands(typeof(DesignerAreaReourceImportCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");



            View.Property(p => p.ResourceId).Readonly();
            View.Property(p => p.ResourceName).Readonly();

        }
        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.PropertyRef(p => p.Resource.Code).ShowInList(150).HasLabel("产线编码");
            }
        }


    }
}
