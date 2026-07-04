using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.ProcessTechs;
using SIE.Wpf.Command;

namespace SIE.Wpf.Resources.ProcessTechs.Commands
{
    /// <summary>
    /// 制程工艺复制新增命令
    /// </summary>
    [Command(ImageName = "ContentCopy", Label = "复制添加", ToolTip = "复制并添加当前行数据", Gestures = "Ctrl+Shift+C", Location = CommandLocation.All, GroupType = 10)]
    public class CopyProcessTechCommand : ListCopyCommand
    {
        /// <summary>
        /// 创建编辑项
        /// </summary>
        /// <param name="entity">制程工艺</param>
        protected override void OnItemCreated(Entity entity)
        {
            base.OnItemCreated(entity);
            var item = entity as ProcessTech;
            item.Code = RT.Service.Resolve<ProcessTechBaseController>().GetProcessTechNo();
            item.Name = string.Format("{0}-副本", item.Name);
        }
    }
}
