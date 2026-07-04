using SIE.Domain.Validation;
using SIE.ESop.Displays;
using SIE.MES.WIP;
using SIE.MetaModel;
using SIE.Security;
using SIE.View;
using SIE.Wpf.Command;
using SIE.Wpf.MES.WIP;
using System;
using System.Linq;

namespace SIE.Wpf.ESop.Displays.Command
{
    /// <summary>
    /// 作业指导书
    /// </summary>
    [Command(ImageName = "PlaylistPlay", Label = "作业指导书", ToolTip = "作业指导书", Gestures = "Ctrl+Shift+N", GroupType = CommandGroupType.Edit)]
    public class PlayESopCommand : DetailViewCommand
    {
        /// <summary>
        /// 执行具体的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            if (!PermissionService.CanShowModule("#看板定义.ESOP"))
            {
                throw new ValidationException("没有找到ESOP功能,可能没有配置权限，或者名称不符合".L10N());
            }

            var data = (view.Current as DataCollectionViewModel);
            ValidationEsop(data.Workstation);
            data.Workstation.PropertyChanged -= Workstation_PropertyChanged;
            data.Workstation.PropertyChanged += Workstation_PropertyChanged;
            ModuleMeta moduleMeta = CommonModel.Modules.FindModule("#看板定义.ESOP");
            if (moduleMeta != null)
            {
                SIE.Context.AppContext.Items["ESop.EnableProces"] = true;
                SIE.Context.AppContext.Items["ESop.Workstation"] = data.Workstation;
                RT.Service.Resolve<IMenuService>().OpenModule(moduleMeta.Key);
            }
        }

        /// <summary>
        /// 验证ESOP
        /// </summary>
        /// <param name="workcell">工作站信息</param>
        private void ValidationEsop(Workstation workcell)
        {
            if (workcell == null)
                throw new ValidationException("工作区丢失".L10N());
            if (workcell.ProcessId == 0 || workcell.ResourceId == 0)
                throw new ValidationException("工序与产线,未找到匹配的显示点".L10N());
            var displayerPoint = RT.Service.Resolve<DisplayPointController>().GetDisplayPointList(new DisplayPointCriteria { ResourceId = workcell.ResourceId, ProcessId = workcell.ProcessId }).FirstOrDefault();
            if (displayerPoint == null)
                throw new ValidationException("工序与产线,未找到匹配的显示点".L10N());
        }

        /// <summary>
        /// 工作站信息变更时触发
        /// </summary>
        /// <param name="sender">变更工作站对象</param>
        /// <param name="e">事件参数</param>
        private void Workstation_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var data = (View.Current as DataCollectionViewModel);
            var workstation = data.Workstation;
            if (e.PropertyName == nameof(workstation.EmployeeId) || e.PropertyName == nameof(workstation.ResourceId) || e.PropertyName == nameof(workstation.ProcessId) || e.PropertyName == nameof(workstation.StationId))
                return; //防止重复触发
            PublishWorkcellChanged(workstation);
        }

        /// <summary>
        /// 发布工作站变更到EventBus
        /// </summary>
        /// <param name="workstation">工作站</param>
        private void PublishWorkcellChanged(Workstation workstation)
        {
            var workcell = new Workcell();
            workcell.EmployeeId = workstation.EmployeeId.HasValue ? workstation.EmployeeId.Value : 0;
            workcell.ProcessId = workstation.ProcessId.HasValue ? workstation.ProcessId.Value : 0;
            workcell.StationId = workstation.StationId.HasValue ? workstation.StationId.Value : 0;
            workcell.ResourceId = workstation.ResourceId.HasValue ? workstation.ResourceId.Value : 0;
            RT.EventBus.Publish<Workcell>(workcell);
        }
    }
}