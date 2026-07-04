using SIE.Domain.Validation;
using SIE.Web.Command;
using SIE.WorkBenchCommon.Workbench.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.WorkBenchCommon.Workbench.Base.Commands
{
    /// <summary>
    /// 工作台定义添加保存命令
    /// </summary>
    public class AddWorkbenchCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var workbenchViewModel = args.Data.ToJsonObject<WorkbenchViewModel>();

            if(workbenchViewModel.IsNewData)
                RT.Service.Resolve<WorkbenchController>().InsertWorkbenchDefinition(workbenchViewModel);
            else
                RT.Service.Resolve<WorkbenchController>().AddUpdateWorkbenchDefinition(workbenchViewModel);
            return true;
        }
    }

    /// <summary>
    /// 工作台定义设计保存命令
    /// </summary>
    public class DesignWorkbenchCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var workbenchViewModel = args.Data.ToJsonObject<WorkbenchViewModel>();

             RT.Service.Resolve<WorkbenchController>().UpdateWorkbenchDefinition(workbenchViewModel);
           
            return true;
        }
    }

    /// <summary>
    /// 工作台定义发布命令
    /// </summary>
    public class PublishWorkbenchCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var publishInfo = args.Data.ToJsonObject<PublishInfo>();

            RT.Service.Resolve<WorkbenchController>().PublishWorkbench(publishInfo.Code);

            return true;
        }

        class PublishInfo
        {
            public string Code { get; set; }
            public DisplayMode DisplayMode { get; set; }
        }
    }

    /// <summary>
    /// 工作台定义取消发布命令
    /// </summary>
    public class UnPublishWorkbenchCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var workbenchCode = args.Data.ToJsonObject<string>();

            RT.Service.Resolve<WorkbenchController>().UnPublishWorkbench(workbenchCode);

            return true;
        }

       
    }

    public class WorkbenchPreviewCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var entity = args.Data.ToJsonObject<WorkbenchDefinition>();

            var layout=RT.Service.Resolve<WorkbenchController>().ExitLayout(entity.LayoutCode);
            if(layout== null)
                throw new ValidationException("[布局管理]未查询到布局编码[{0}],".L10nFormat(entity.LayoutCode));
            return true;
        }
    }
}
