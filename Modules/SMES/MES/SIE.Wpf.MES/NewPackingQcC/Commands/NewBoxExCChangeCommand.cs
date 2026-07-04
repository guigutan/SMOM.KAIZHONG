using SIE.Wpf.Command;
using SIE.Wpf.MES.NewPackingQC;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.NewPackingQcC.Commands
{
    /// <summary>
    /// 正常
    /// </summary>
    [Command(ImageName = "", Label = "换  箱", ToolTip = "换  箱", GroupType = CommandGroupType.Edit)]
    public class NewBoxExCChangeCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return base.CanExecute(view) && (view.Current as DataCollectionViewModel) != null && (view.Current as NewPackingQcCViewModel) != null;
        }

        public override void Execute(DetailLogicalView view)
        {
            //包装采集信息
            var vm1 = view.Current as NewPackingQcCViewModel;
            vm1.BoxExChange = 1;
            vm1.Tips = "请输入已装箱的蓝标!!!";
        }
    }
}
