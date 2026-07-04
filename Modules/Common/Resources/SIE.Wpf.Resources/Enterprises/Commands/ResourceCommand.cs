using SIE.Domain;
using SIE.Resources.Enterprises;
using SIE.Wpf.Command;
using System;
using System.Linq;
using SIE.Resources.WipResources;

namespace SIE.Wpf.Resources.Enterprises.Commands
{
    /// <summary>
    /// 设置组织层级为资源
    /// </summary>
    [Command(Label = "设为资源")]
    public class EnableResourceCommand : ListViewCommand
    {
        /// <summary>
        /// 控制命令是否可执行的逻辑
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>只有组织层级不是资源时此命令才可以执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.Any() && view.SelectedEntities.OfType<EnterpriseLevel>().All(p => !p.IsResource && p.PersistenceStatus != PersistenceStatus.New);
        }

        /// <summary>
        /// 执行逻辑
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var selected = view?.SelectedEntities.OfType<EnterpriseLevel>().ToList();
            var result = CRT.MessageService.AskQuestion("确定设定当前选中的{0}行为资源吗?".L10nFormat(selected?.Count));
            if (result && selected != null)
            {
                foreach (EnterpriseLevel resourceLevel in selected)
                {
                    var ctl = RT.Service.Resolve<EnterpriseController>();
                    var rtn = ctl.SetResource(resourceLevel.Id, true);
                    resourceLevel.Clone(rtn, CloneOptions.ReadDbRow());    //克隆对象
                    resourceLevel.NotifyAllPropertiesChanged();
                }
            }
        }
    }

    /// <summary>
    /// 取消组织层级资源
    /// </summary>
    [Command(Label = "取消资源")]
    public class DisableResourceCommand : ListViewCommand
    {
        /// <summary>
        /// 控制命令是否可执行的逻辑
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>只有组织层级是资源时此命令才可以执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view == null)
            {
                return false;
            }
            else
            {
               return  view.SelectedEntities.Any() && view.SelectedEntities.OfType<EnterpriseLevel>().All(p => p.IsResource && p.PersistenceStatus != PersistenceStatus.New);
            }
        }

        /// <summary>
        /// 执行逻辑
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var ctl = RT.Service.Resolve<EnterpriseController>();
            var selected = view?.SelectedEntities.OfType<EnterpriseLevel>().ToList();
            if (selected == null)
            {
                CRT.MessageService.ShowMessage("不允许取消资源，企业层级数据异常".L10N());
                return;
            }
            if (ctl.IsExistRourceEnterprise(selected.Select(p => (double?)p.Id).ToList()))
            {
                CRT.MessageService.ShowMessage("不允许取消资源，企业层级下存在是资源的企业模型".L10N());
                return;
            }

            var result = CRT.MessageService.AskQuestion("确定取消当前选择的{0}行资源吗?".L10nFormat(selected.Count));
            if (result)
            {
                foreach (EnterpriseLevel resourceLevel in selected)
                {
                    var rtn = ctl.SetResource(resourceLevel.Id, false);
                    RT.Service.Resolve<WipResourceController>().StopSchResourse(resourceLevel.Id, SyncSourceType.Enterprise);
                    resourceLevel.Clone(rtn, CloneOptions.ReadDbRow());    //克隆对象
                    resourceLevel.NotifyAllPropertiesChanged();
                    resourceLevel.MarkSaved();
                }
            }
        }
    }
}
