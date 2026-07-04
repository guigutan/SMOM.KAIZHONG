using SIE.Dock.YardZones;
using SIE.Dock.YardZones.Service;
using SIE.Domain;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.Dock.YardZones.Commands
{
    /// <summary>
    /// 添加
    /// </summary>
    public class AddAddressCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    /// <summary>
    /// 编辑
    /// </summary>
    public class EditAddressCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    public class DeleteYardZoneCommand : DeleteCommand
    {
    }

    /// <summary>
    /// 保存命令
    /// </summary>
    public class SaveYardZoneCommand : SaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">实体列表</param>
        protected override void OnSaving(EntityList data)
        {
            var yardZoneService = RT.Service.Resolve<YardZoneService>();
            var list = data as EntityList<YardZone>;
            list.ForEach(bill =>
            {
                if (bill.PersistenceStatus != PersistenceStatus.New)
                {
                    var commitDtlIdList = bill.DockHandlingList.Select(p => p.Id).ToList();
                    var delIds = bill.DockHandlingList.DeletedList.Select(p => p.GetId()).ToList();
                    var oldDtlList = yardZoneService.GetDockHandlingList(bill.Id).Where(p => !commitDtlIdList.Contains(p.Id) && !delIds.Contains(p.Id)).ToList();
                    bill.DockHandlingList.AddRange(oldDtlList);
                    bill.DockHandlingList.Where(p => !commitDtlIdList.Contains(p.Id)).ForEach(p => p.PersistenceStatus = PersistenceStatus.Unchanged);
                    
                }

                yardZoneService.ValidateYardZoneData(bill);
            });

            base.OnSaving(data);
        }
    }
}
