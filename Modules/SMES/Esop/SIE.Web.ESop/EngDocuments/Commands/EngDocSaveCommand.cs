using SIE.Domain;
using SIE.ESop.EngDocuments.Services;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ESop.EngDocuments.Commands
{
    /// <summary>
    /// 工程文件保存命令
    /// </summary>
    public class EngDocSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前校验
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            RT.Service.Resolve<EngDocumentService>().EngDocSave(data);
            base.OnSaving(data);
        }
    }
}
