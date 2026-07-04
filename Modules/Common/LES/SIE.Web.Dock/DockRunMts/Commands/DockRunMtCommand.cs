using SIE.Dock.DockRunMts;
using SIE.Dock.DockRunMts.Service;
using SIE.Domain;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.Dock.DockRunMts.Commands
{
    /// <summary>
    /// 保存
    /// </summary>
    public class SaveDockRunMtCommand : SaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">实体列表</param>
        protected override void OnSaving(EntityList data)
        {
            var dockRunMtService = RT.Service.Resolve<DockRunMtService>();
            var list = data as EntityList<DockRunMt>;
            list.ForEach(bill =>
            {
                if (bill.PersistenceStatus != PersistenceStatus.New)
                {
                    var commitDtlIdList = bill.WorkTimeList.Select(p => p.Id).ToList();
                    var delworklistIds = bill.WorkTimeList.DeletedList.Select(p => p.GetId()).ToList();
                    var oldDtlList = dockRunMtService.GetWorkTimeList(bill.Id).Where(p => !commitDtlIdList.Contains(p.Id) && !delworklistIds.Contains(p.Id)).ToList();
                    bill.WorkTimeList.AddRange(oldDtlList);
                    bill.WorkTimeList.Where(p => !commitDtlIdList.Contains(p.Id)).ForEach(p => p.PersistenceStatus = PersistenceStatus.Unchanged);

                    var curExcepTimeIds = bill.ExcepTimeList.Select(p => p.Id).ToList();
                    var delexceplistIds = bill.ExcepTimeList.DeletedList.Select(p => p.GetId()).ToList();
                    var oldExcepTimeList = dockRunMtService.GetExcepTimeList(bill.Id).Where(p => !curExcepTimeIds.Contains(p.Id) && !delexceplistIds.Contains(p.Id)).ToList();
                    bill.ExcepTimeList.AddRange(oldExcepTimeList);
                    bill.ExcepTimeList.Where(p => !curExcepTimeIds.Contains(p.Id)).ForEach(p => p.PersistenceStatus = PersistenceStatus.Unchanged);
                }

                dockRunMtService.ValidateDockRunMtData(bill);
            });

            base.OnSaving(data);
        }
    }
}
