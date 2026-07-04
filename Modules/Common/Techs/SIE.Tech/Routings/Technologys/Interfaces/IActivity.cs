using SIE.Domain;
using SIE.EventMessages.EMS.Fixtures;
using SIE.Items;
using SIE.Tech.Processs;
using SIE.Tech.Routings.Technologys.Models;
using System;
using System.Collections.Generic;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
    /// 活动接口
    /// </summary>
    public interface IActivity : IChildElement
    {
        /// <summary>
        /// 是否委外
        /// </summary>
         bool IsOutsourcing { get; set; }
        /// <summary>
        /// 复制的Id
        /// </summary>
        string CopyId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        double ProcessId { get; set; }

        /// <summary>
        /// 是否可选
        /// </summary> 
        bool IsOptional { get; set; }

        /// <summary>
        /// 是否重复过站
        /// </summary> 
        bool IsRepeat { get; set; }

        /// <summary>
        /// 创建Sku
        /// </summary> 
        bool CreateSku { get; set; }

        /// <summary>
        /// 是否计产
        /// </summary>
        bool IsCalculate { get; set; }

        /// <summary>
        /// 是否生成任务单
        /// </summary> 
        bool IsGenerateTask { get; set; }

        /// <summary>
        /// 是否需求任务清单
        /// </summary>
        bool IsRequirementTask { get; set; }

        /// <summary>
        /// 是否扣料
        /// </summary> 
        bool IsBuckleMaterial { get; set; }

        /// <summary>
        /// 起始工序
        /// </summary> 
        double? StartProcess { get; set; }

        /// <summary>
        /// 正常胜制编码
        /// </summary>
        double? NormalVictory { get; set; }

        /// <summary>
        /// 维修胜制编码
        /// </summary>
        double? RepairVictory { get; set; }

        /// <summary>
        /// 是否加严
        /// </summary>
        bool IsStricter { get; set; }

        /// <summary>
        /// 超时时间（分钟）
        /// </summary> 
        int? Overtime { get; set; }

        /// <summary>
        /// 直通率取值
        /// </summary>
        bool IsPassRate { get; set; }

        /// <summary>
        /// 绑定
        /// </summary>
        bool IsBinding { get; set; }

        /// <summary>
        /// 解绑
        /// </summary>
        bool IsUnBinding { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        double Width { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        double Height { get; set; }

        /// <summary>
        /// 容器高
        /// </summary>
        double ContainerHeight { get; set; }

        /// <summary>
        /// 面板左边坐标
        /// </summary>
        double Left { get; set; }

        /// <summary>
        /// 面板顶坐标
        /// </summary>
        double Top { get; set; }

        /// <summary>
        /// 获取或设置为在此元素显示的工具提示对象 用户界面 (UI)。
        /// </summary>
        object ToolTip { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// 工序状态
        /// </summary>
        ProcessState ProcessState { get; set; }

        /// <summary>
        /// 工序类型
        /// </summary>
        ProcessType ProcessType { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        ActivityType Type { get; set; }

        /// <summary>
        /// 设置坐标点
        /// </summary> 
        /// <param name="point">位置</param>
        void SetPoint(Point point);

        /// <summary>
        /// 获取点
        /// </summary>
        /// <returns>位置</returns>
        Point GetPoint();

        /// <summary>
        /// 规则参数列表
        /// </summary>
        IList<IRule> Rules { get; set; }

        /// <summary>
        /// 开始规则列表
        /// </summary>
        IList<IRule> BeginRules { get; set; }

        /// <summary>
        /// 结束规则列表
        /// </summary>
        IList<IRule> EndRules { get; set; }

        /// <summary>
        /// 物料列表
        /// </summary>
        EntityList<ItemExtBom> Bom { get; set; }

        /// <summary>
        /// 工序BOM列表
        /// </summary>
        IList<ProcessBom> ProcessBoms { get; set; }

        /// <summary>
        /// 工治具ID
        /// </summary>
        IList<FixtureInfo> Fixtures { get; set; }

        /// <summary>
        /// 判断点是否在元素内部
        /// </summary>
        /// <param name="point">点</param>
        /// <returns>是否在元素内部</returns>
        bool PointIsInside(Point point);

        /// <summary>
        /// 移动活动的事件
        /// </summary> 
        event Action<IActivity> ActivityMove;

        /// <summary>
        /// 移动
        /// </summary>
        void Move();

        /// <summary>
        /// 最大过站次数
        /// </summary> 
        int? MaxPassNum { get; set; }

        /// <summary>
        /// 是否下工序入站
        /// </summary>
        bool? IsNextMoveIn { get; set; }

        /// <summary>
        /// 启用入站控制
        /// </summary>
        bool? EnableMoveInControl { get; set; }

        /// <summary>
        /// 交接类型
        /// </summary>
        TransferType? TransferType { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        string ParentNodeId { get; set; }

        /// <summary>
        /// 是否工序组
        /// </summary>
        bool IsGroup { get; set; }

        /// <summary>
        /// 工序组Id
        /// </summary>
        string GroupId { get; set; }

        /// <summary>
        /// 工序参数集合
        /// </summary>
        EntityList<ProcessParameter> ProcessParameter { get; set; }

        /// <summary>
        /// 获取底部中心位置
        /// </summary>
        /// <returns>位置</returns>
        Point GetBottomPoint();

        /// <summary>
        /// 获取底部中心位置
        /// </summary>
        /// <returns>位置</returns>
        Point GetTopPoint();

        /// <summary>
        /// 工艺路线信息sId
        /// </summary>
        public double? LayoutInfoId { get; set; }

        /// <summary>
        /// 工序流水码
        /// </summary>
        public string Vornr { get; set; }

        /// <summary>
        /// 控制码(工序控制码)
        /// </summary>
        public string Steus { get; set; }
    }
}
