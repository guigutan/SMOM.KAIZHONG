using SIE.Wpf.MES.WIP;
using SIE.Wpf.MES.Workbench.EmployeeManages;
using SIE.Wpf.MES.Workbench.Properties;
using System;
using System.Linq;

namespace SIE.Wpf.MES.Workbench.Helper
{
    /// <summary>
    /// 本地设置帮助类
    /// </summary>
    public static class SettingsHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ResourceShift GetResourceShift()
        {
            var resourceShift = new ResourceShift();
            var setting = Settings.Default.ResourceShift;
            if (setting.IsNullOrEmpty()) return resourceShift;
            var result = setting.Split(new char[] { ';' });
            if (result.Count() != 2) return resourceShift;
            resourceShift.ResourceId = double.Parse(result[0]);
            resourceShift.ShiftId = double.Parse(result[1]);
            return resourceShift;
        }
    }
}
