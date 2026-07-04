using SIE.Domain;
using SIE.ProductIntfc.InspSettings;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.InspSettings.ProductIntfcs.Commands
{
    /// <summary>
    /// 报检参数新增命令类
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加数据", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    public class InspParameterAddCommand : ListAddCommand
    {
        /// <summary>
        /// 新增命令
        /// </summary>
        /// <returns>ScheduingRouting实体</returns>
        protected override Entity CreateNewItem()
        {
            var inspParameter = base.CreateNewItem() as InspParameter;
            inspParameter.InspParm = 1;
            inspParameter.CreateDate = DateTime.Now;
            inspParameter.UpdateDate = DateTime.Now;
            if (inspParameter.InspType == InspType.Product)
                inspParameter.ProcessType = InspProcess.Last;
            return inspParameter;
        }
    }
}
