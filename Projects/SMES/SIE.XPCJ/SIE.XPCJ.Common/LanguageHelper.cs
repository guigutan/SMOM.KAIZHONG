using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Services;
using SIE.XPCJ.Common.Settings;
using SIE.XPCJ.Models;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIE.XPCJ.Common
{
    /// <summary>
    /// 该类用于获取语言包资料
    /// </summary>
    public static class LanguageHelper
    {
        /// <summary>
        /// 收集资源的队列
        /// </summary>
        private static ConcurrentQueue<Resource> concurrentQueue = new ConcurrentQueue<Resource>();

        public static void InQueue(Resource resource)
        {
            concurrentQueue.Enqueue(resource);
        }

        /// <summary>
        /// 程序启动后开启线程不停的从队列中上传语言资源到SMOM 不影响界面
        /// </summary>
        public static void StartUploadCultureResoure()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    //要求已登录 且启用收集文化 队列不为空 出列成功后执行上传 
                    if (LoginInfo.Instance.UserId > 0 
                    && AppSettings.Instance.CollectionCulture 
                    && !LanguageHelper.concurrentQueue.IsEmpty 
                    && LanguageHelper.concurrentQueue.TryDequeue(out Resource uploadResource) 
                    && CultureService.UploadCultureResource(uploadResource))
                    {
                        Global.Language.AddLocalResource(uploadResource);
                    }
                    Thread.Sleep(200);
                }
            });
        }

        public static void SetLanguage(Control control, string formName = "")
        {
            var special = formName == "FormLogin";//特殊处理
            if (control is TextBox && special)
            {
                return;
            }
            if (control is ComboBox)
            {
                return;
            }
            if (control is SIE.XPCJ.Common.Controls.XPBottomBar)//状态栏不收集
            {
                return;
            }

            if (control is DataGridView dgv && control.Controls.Count > 0)
            {
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    column.HeaderText = column.HeaderText.L10N();
                }
            }
            if (control is Controls.XPWatermarkTextBox xpwtext)
            {
                xpwtext.WaterText = xpwtext.WaterText.L10N();
            }
            if (Global.Language != null && control.Tag == null && special && Global.Language.Code == AppSettings.Instance.DevCulture)//这两个界面特殊处理
            {
                control.Tag = control.Text;
            }
            var languageText = control.Tag == null ? control.Text.L10N() : control.Tag.ToString().L10N();
            control.Text = special ? languageText : control.Text.L10N();
            foreach (Control subControl in control.Controls)
            {
                SetLanguage(subControl, formName); // 递归调用
            }
        }
    }
}
