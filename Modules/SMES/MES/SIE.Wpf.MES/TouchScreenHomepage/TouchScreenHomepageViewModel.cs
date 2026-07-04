using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.Common;
using SIE.MES.Common.HomeMenusConfigs;
using SIE.ObjectModel;
using SIE.Rbac.Menus;
using SIE.Wpf.Common;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.TouchScreenHomepage
{
    /// <summary>
    /// 
    /// </summary>
    [RootEntity, Serializable]
    [Label("触摸屏首页")]
    public class TouchScreenHomepageViewModel : WorkCellViewModel
    {

        /// <summary>
        /// 加载
        /// </summary>
        public override void Onload()
        {
            base.Onload();
            AsyncExecute(() =>
            {
                GetMenuList();
            });
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        private void GetMenuList()
        {
            var res = RT.Service.Resolve<HomeMenusConfigsController>().GetCurUserMenus();
            for (int i = 0; i < res.Count; i++)
            {
                MenuList.Add(res[i]);
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public override void OnClose()
        {
            base.OnClose();

        }

        /// <summary>
        /// 重置界面数据
        /// </summary>
        public virtual void Reset()
        {

        }

        /// <summary>
        /// 菜单列表
        /// </summary>
        public EntityList<Menu> MenuList { get; set; } = new EntityList<Menu>();
    }
}
