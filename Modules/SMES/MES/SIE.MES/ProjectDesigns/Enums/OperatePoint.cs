using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.Enums
{
    /// <summary>
    /// 操作节点
    /// </summary>
    public enum OperatePoint
    {
        /// <summary>
        /// 创建
        /// </summary>
        [Label("创建")]
        Create = 0,

        /// <summary>
        /// 修改
        /// </summary>
        [Label("修改")]
        Edit = 1,

        /// <summary>
        /// 基本属性-更新
        /// </summary>
        [Label("基本属性-更新")]
        BasicUpdate = 2,

        /// <summary>
        /// 基本属性-删除
        /// </summary>
        [Label("基本属性-删除")]
        BasicDelete = 3,

        /// <summary>
        /// 工艺资料-引用标准BOM
        /// </summary>
        [Label("产品设计-引用标准BOM")]
        PTreeInit = 4,

        /// <summary>
        /// 工艺资料-产品BOM引用标准BOM
        /// </summary>
        [Label("工艺资料-产品BOM引用标准BOM")]
        PTreeBomInit = 5,

        /// <summary>
        /// 工艺资料-产品BOM更新版本
        /// </summary>
        [Label("工艺资料-产品BOM更新版本")]
        PTreeBomUpdate = 6,

        /// <summary>
        /// 工艺资料-产品工艺路线
        /// </summary>
        [Label("工艺资料-产品工艺路线")]
        PTreeRoutingInit = 7,

        /// <summary>
        /// 设计完成
        /// </summary>
        [Label("设计完成")]
        DesignComplete = 8,

        /// <summary>
        /// 审核
        /// </summary>
        [Label("审核")]
        DesignExamine = 9,

        /// <summary>
        /// 反审
        /// </summary>
        [Label("反审")]
        DesignAgainstExamine = 10,

        /// <summary>
        /// 启用
        /// </summary>
        [Label("启用")]
        Enable = 11,

        /// <summary>
        /// 禁用
        /// </summary>
        [Label("禁用")]
        Disable = 12,
    }
}
