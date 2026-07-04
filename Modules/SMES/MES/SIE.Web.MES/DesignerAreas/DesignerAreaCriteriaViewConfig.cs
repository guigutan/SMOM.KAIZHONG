using DocumentFormat.OpenXml.Wordprocessing;
using SIE.MES.DesignerAreas;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;

namespace SIE.Web.MES.DesignerAreas
{
    internal class DesignerAreaCriteriaViewConfig : WebViewConfig<DesignerAreaCriteria>
    {
        protected override void ConfigView()
        {
            View.UseClientOrder();
           
        }
        protected override void ConfigQueryView()
        {
            View.Property(p => p.AreaCode).HasLabel("看板区域编码");          
            View.Property(p => p.AreaName).HasLabel("看板区域名称");             
       
           
        }
    }
}
