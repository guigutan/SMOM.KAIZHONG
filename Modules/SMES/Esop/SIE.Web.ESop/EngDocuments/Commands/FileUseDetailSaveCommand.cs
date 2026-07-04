using SIE.Domain;
using SIE.ESop.EngDocuments.Services;
using SIE.Web.Command;

namespace SIE.Web.ESop.EngDocuments.Commands
{
    /// <summary>
    /// 使用类型关联保存命令
    /// </summary>
    public class FileUseDetailSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前校验
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            RT.Service.Resolve<FileUseDetailService>().FileUseSave(data);
            base.OnSaving(data);
        }
    }
}
