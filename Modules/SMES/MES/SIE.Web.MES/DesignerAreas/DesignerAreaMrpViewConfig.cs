using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Domain;
using SIE.MES.DesignerAreas;
using SIE.MetaModel.View;
using System;

namespace SIE.Web.MES.DesignerAreas
{
    public class DesignerAreaMrpViewConfig : WebViewConfig<DesignerAreaMrp>
    {
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();        

            View.Property(p => p.MrpController).ShowInList(width:100);
           
        }
    }
}
