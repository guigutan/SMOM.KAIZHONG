using SIE.Barcodes.Panels;
using SIE.Web.Barcodes.Barcodes.Commands;
using SIE.Web.Command;

namespace SIE.Web.Barcodes.Panels.Commands
{
    /// <summary>
    /// 拼板码报废命令
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.Panels.Commands.ScarpCommand")]
    public class ScarpCommand : ListViewCommand
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args">实体参数</param>
        /// <param name="scope">scope</param>
        /// <returns>返回结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var scarpDatas = args.Data.ToJsonObject<DataModel>();
            if (scarpDatas.BarCodeIds.Count > 0)
            {
                var panels = RT.Service.Resolve<PanelController>().GetPanelsByIds(scarpDatas.BarCodeIds);
                RT.Service.Resolve<PanelController>().PanelScrap(panels, scarpDatas.Reason);
            }
            return true;
        }
    }
}
