using SIE.Domain;
using SIE.EMS.AssetTransfers;
using SIE.Web.Command;

namespace SIE.Web.EMS.AssetTransfers.Commands
{
    /// <summary>
    /// 保存入库单
    /// </summary>
    [JsCommand("SIE.Web.EMS.AssetTransfers.Commands.SaveAssetTransfer")]
    public class SaveAssetTransfer : FormSaveCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var model = entity as AssetTransfer;
            RT.Service.Resolve<AssetTransfersController>().SaveAssetTransfer(model);
        }
    }
}
