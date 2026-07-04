using SIE.Domain;
using SIE.Resources.ProcessTechs;
using SIE.Web.Command;

namespace SIE.Web.Resources.ProcessTechs.Commands
{

    /// <summary>
    /// 制程工艺保存命令
    /// </summary>
    public class ProcessTechSaveCommand : SaveCommand
    {
        /// <summary>
        /// 制程工艺保存命令
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            foreach (ProcessTech item in data)
            {
                if (item.IsScheduling)
                {
                    item.OffsetTime = null;
                }
                else
                {
                    item.TransferTime = null;
                }
            }
            base.DoSave(data);
        }
    }
}
