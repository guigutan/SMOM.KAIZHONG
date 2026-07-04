using SIE.Andon.Andons;
using SIE.Domain;
using SIE.Wpf.MES.WIP.Reworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.Andon
{
    /// <summary>
    /// 安灯管理触发行为
    /// </summary>
    public class AndonManageTriggerBehavior : ViewBehavior
    {
        /// <summary>
        /// 附加视图行为
        /// </summary>
        protected override void OnAttach()
        {
            View.DataChanged += (o, e) =>
            {
                var andonManage = View.Current as AndonManage;
                andonManage.PropertyChanged += AndonManage_PropertyChanged;
            };
        }

        private void AndonManage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AndonManage.AskMaterial))
            {
                var dialog = CRT.Workbench.DialogContents.FirstOrDefault(p => p.ViewId == View.ViewId);
                if (dialog != null)
                {
                    var currentAndonManage = sender as AndonManage;
                    View.ChildrenViews[0].RefreshControl();
                    View.ChildrenViews[0].IsVisible = currentAndonManage.AskMaterial;
                    // 清空子表
                    View.ChildrenViews[0].Data = new EntityList<AndonManageCallMaterial>();
                    if (currentAndonManage.AskMaterial)
                    {
                        dialog.Height += 150;
                    }
                    else
                    {
                        dialog.Height -= 150;
                    }
                }
            }
        }
    }
}
