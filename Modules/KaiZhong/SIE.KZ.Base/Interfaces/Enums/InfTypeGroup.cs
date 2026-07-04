using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Enums
{
    /// <summary>
    /// 多工厂接口类型
    /// </summary>
    public enum InfTypeGroup
    {
        //基础数据枚举值0-100

        #region 基础数据

        /// <summary>
        /// 参数项目
        /// </summary>
        [Label("参数项目")]
        CraftParameter = 0,

        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        Process = 1,

        /// <summary>
        /// 客户
        /// </summary>
        [Label("客户")]
        Customer = 2,

        /// <summary>
        /// 供应商
        /// </summary>
        [Label("供应商")]
        Supplier = 3,

        /// <summary>
        /// 物料分类
        /// </summary>
        [Label("物料分类")]
        ItemCategory = 5,

        /// <summary>
        /// 工艺参数
        /// </summary>
        [Label("工艺参数")]
        ProductParameter = 6,

        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        Item = 7,

        /// <summary>
        /// 设备台账
        /// </summary>
        [Label("设备台账")]
        EquipAccount = 9,

        /// <summary>
        /// 员工
        /// </summary>
        [Label("员工")]
        Employee = 11,

        /// <summary>
        /// 产品项目
        /// </summary>
        [Label("产品项目")]
        ProductProject = 14,

        /// <summary>
        /// 同步二级工序
        /// </summary>
        [Label("同步二级工序")]
        GradeProcessCompare = 18,

        /// <summary>
        /// 工序技能题库
        /// </summary>
        [Label("工序技能题库")]
        ProcessSkillQues = 19,

        /// <summary>
        /// 检验标准
        /// </summary>
        [Label("同步检验标准")]
        InspectionStandard = 27,

        /// <summary>
        /// 工作中心
        /// </summary>
        [Label("工作中心")]
        WorkCenter = 28,

        #endregion 基础数据
    }
}
