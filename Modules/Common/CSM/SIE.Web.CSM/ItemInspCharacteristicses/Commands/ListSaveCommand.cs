using SIE.Domain;
using SIE.CSM.ItemInspCharacteristicses;
using SIE.Web.Command;

namespace SIE.Web.CSM.ItemInspCharacteristicses.Commands
{
    /// <summary>
    /// 列表保存命令
    /// </summary>
    public class ListSaveCommand:SaveCommand
    {
        protected override void OnSaving(EntityList data)
        {
            if(data?.Count > 0)
            {
                foreach(var entity in data)
                {
                    if(entity is ItemInspCharacteristics charac)
                    {
                        //保存时重置时间和已执行批次数量
                        charac.InspectDateBegin = null;
                        charac.SkipBatches = null;
                    }
                }
            }
        }
    }
}
