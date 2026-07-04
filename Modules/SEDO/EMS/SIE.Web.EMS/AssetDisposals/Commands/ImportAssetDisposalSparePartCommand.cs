using SIE.Common.Import;
using SIE.EMS.AssetDisposals;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetDisposals.Commands
{
    /// <summary>
    /// 导入设备回收
    /// </summary>
    public class ImportAssetDisposalSparePartCommand : ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            if (batch == null)
            {
                return;
            }
            var ImpMesResultList = RT.Service.Resolve<AssetDisposalController>().ImportOnSave(batch);
            importResult.MessageList.AddRange(ImpMesResultList);
        }
    }
}
