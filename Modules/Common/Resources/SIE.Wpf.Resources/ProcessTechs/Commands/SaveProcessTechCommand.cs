using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Resources.ProcessTechs;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Resources.ProcessTechs.Commands
{
    /// <summary>
    /// 制程工艺保存命令
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存", ToolTip = "保存数据", Gestures = "Ctrl+S", GroupType = 10)]
    public class SaveProcessTechCommand : ListSaveCommand
    {
        /// <summary>
        /// 保存前验证
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        protected override void OnSaving(ListLogicalView view)
        {
            var current = view.Current as ProcessTech;
            if (current != null)
            {
                if (current.PersistenceStatus == PersistenceStatus.New || current.PersistenceStatus == PersistenceStatus.Modified)
                {
                    if (!current.IsScheduling)
                    {
                        if (current.OffsetTime == null)
                        {
                            throw new ValidationException("请维护偏移时间！".L10N());
                        }

                        current.TransferTime = null;
                        current.SAM = null;
                    }
                    else
                    {
                        current.OffsetTime = null;
                    }
                }
            }

            base.OnSaving(view);
        }
    }
}
