using SIE.Common;
using SIE.Domain;
using SIE.MES.BarcodeProcesses;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BarcodeProcesses.Commands
{
    /// <summary>
    /// 条码工序指派明细保存命令
    /// </summary>
    public class WoProcessDetailSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前校验
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            // 新增、修改的数据
            var editDetail = data.CastTo<EntityList<BarcodeProDetail>>().Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged).AsEntityList();

            // 验证
            RT.Service.Resolve<BarcodeProcessController>().SaveWoProcessDetailValidate(editDetail);
        }

        /// <summary>
        /// 保存命令
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            // 删除的数据
            EntityList<BarcodeProDetail> delDetail = new EntityList<BarcodeProDetail>();
            if (data.DeletedList.Count > 0)
            {
                delDetail = data.DeletedList.OfType<BarcodeProDetail>().AsEntityList();
            }
            // 新增、 修改的数据
            var editDetail = data.CastTo<EntityList<BarcodeProDetail>>();

            RT.Service.Resolve<BarcodeProcessController>().SaveWoProcessDetail(delDetail, editDetail);
        }
    }
}
