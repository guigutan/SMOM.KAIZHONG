using SIE.Andon.Andons;
using SIE.Domain;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.Andon.Commands
{
    /// <summary>
    /// 安灯管理叫料生成备料单添加命令
    /// </summary>
    [Command(ImageName = "Add", Label = "添加", ToolTip = "添加", GroupType = CommandGroupType.Edit)]
    public class AndonManageMaterialCommand : ListViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Execute(ListLogicalView view)
        {
            var andonManage = view.Parent.Current as AndonManage;
            var callMaterial = new AndonManageCallMaterial
            {
                FactoryId = andonManage.FactoryId,
                WipId = (double)andonManage.WipResourceId,
                WorkShopId = andonManage.WorkShopId,
                WorkOrderId = andonManage.WorkOrderId,
                ProcessId = andonManage.ProcessId,
                AndonManageId = andonManage.Id,
            };
            callMaterial = RT.Service.Resolve<AndonManageController>().AddCallMaterial(callMaterial);
            view.IsReadOnly = MetaModel.ReadOnlyStatus.None;
            view.Data.Add(callMaterial);
        }
    }
}
