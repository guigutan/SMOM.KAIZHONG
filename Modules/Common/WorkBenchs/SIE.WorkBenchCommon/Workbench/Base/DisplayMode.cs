using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.WorkBenchCommon.Workbench.Base
{
    /// <summary>
    /// 工作台显视模式
    /// </summary>
    public enum DisplayMode
    {
        /// <summary>
        /// 弹出窗口
        /// </summary>
        [Label("弹出窗口")]
        PopupWindow = 0,
        /// <summary>
        /// 卡片式显视
        /// </summary>
        [Label("卡片式显视")]
        TabScreen = 1,
        /// <summary>
        /// 模态窗口
        /// </summary>
        [Label("模态窗口")]
        Dialog =2
    }
}
