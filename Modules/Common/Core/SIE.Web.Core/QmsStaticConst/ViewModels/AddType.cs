using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Core.QmsStaticConst.ViewModels
{
    /// <summary>
    /// 子表添加操作的类型
    /// </summary>
    public enum AddType
    {
        /// <summary>
        /// 添加列
        /// </summary>
        [Label("添加列")]
        Column,
        /// <summary>
        /// 添加行
        /// </summary>
        [Label("添加行")]
        Row,
    }
}
