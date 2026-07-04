using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace Resources.Controls
{
    /// <summary>
    /// VersionInfo.xaml 的交互逻辑
    /// </summary>
    public partial class VersionInfo : UserControl
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public VersionInfo()
        {
            InitializeComponent();
            var version = Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyFileVersionAttribute>().FirstOrDefault().Version.ToString().Split('.');
            versionText.Text = "Version：" + version[0] + "." + version[1];
        }
    }
}
