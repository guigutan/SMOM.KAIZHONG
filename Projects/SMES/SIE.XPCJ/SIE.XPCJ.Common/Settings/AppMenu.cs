using Newtonsoft.Json;
using SIE.XPCJ.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Settings
{
    public class AppMenu
    {
        /// <summary>
        /// 名称，如：新包装采集
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 资源文件名称，如：SIE.XPCJ.NewPackage.Properties.Resources
        /// </summary>
        public string ResourceFileName { get; set; }
        /// <summary>
        /// 图标资源，如：AppMenuIcon
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 权限关键字，如：SIE.Wpf.MES.WIP.NewPackages.NewPackageViewModel,SIE.Wpf.MES
        /// </summary>
        public string Privilege { get; set; }

        private string _dll;

        /// <summary>
        /// dll文件名称，如：SIE.XPCJ.NewPackage.dll
        /// </summary>
        public string Dll
        {
            get
            {
                return string.IsNullOrEmpty(_dll) ? "" : Path.Combine(Global.ExecutingPath, _dll);

            }
            set
            {
                _dll = value;
            }
        }
        /// <summary>
        /// 主Form名称，如：NewPackageForm
        /// </summary>
        public string MainForm { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 是否在界面上显示此菜单按钮
        /// </summary>
        public bool IsShowMenuButton
        {
            get
            {
                return this.Visible && !string.IsNullOrEmpty(this.Icon) && !string.IsNullOrEmpty(this.Dll) && !string.IsNullOrEmpty(this.ResourceFileName) && File.Exists(this.Dll);
            }
        }

        public Image GetIconImage()
        {
            if (string.IsNullOrEmpty(this.Dll) || string.IsNullOrEmpty(this.Icon) || !File.Exists(this.Dll))
                return null;

            try
            {
                Assembly assembly = Assembly.LoadFrom(this.Dll); // 加载 DLL 文件            
                ResourceManager resourceManager = new ResourceManager(this.ResourceFileName, assembly); // 创建 ResourceManager 对象

                object resource = resourceManager.GetObject(this.Icon);// 获取图像资源

                if (resource != null && resource is Image)
                {
                    return (Image)resource;
                }
                else
                {
                    MessageBox.Show("无法找到指定的资源{0}.{1}".L10nFormat(this.ResourceFileName, this.Icon));
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public bool OpenMainForm(Form formMain)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(this.Dll); // 加载 DLL 文件            
                Type formType = assembly.GetType(this.MainForm); // 获取窗体类型

                if (formType != null && formType.IsSubclassOf(typeof(Form)))
                {
                    Form form = (Form)Activator.CreateInstance(formType, formMain); // 创建窗体实例
                    form.Show(); // 显示窗体
                    return true;
                }
                else
                {
                    MessageBox.Show("{0}中无法找到指定的窗体类型：{1}".L10nFormat(this.Dll, this.MainForm));
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }



        private static List<AppMenu> _all;
        public static List<AppMenu> All
        {
            get
            {
                if (_all == null)
                {
                    _all = AppMenu.LoadFromFile();
                }
                return _all;
            }
        }

        public static List<AppMenu> LoadFromFile()
        {
            var appMenusPath = Path.Combine(Global.ExecutingPath, "AppMenus.json");

            if (File.Exists(appMenusPath))
            {
                string text = File.ReadAllText(appMenusPath);
                return JsonConvert.DeserializeObject<List<AppMenu>>(text);
            }
            return new List<AppMenu>();
        }
    }
}
