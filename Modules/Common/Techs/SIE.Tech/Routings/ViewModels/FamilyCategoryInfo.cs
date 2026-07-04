using SIE.Domain;
using SIE.ObjectModel;
using SIE.Tech.VictoryStandards;
using System;
using System.Collections.Generic;

namespace SIE.Tech.Routings.ViewModels
{
    /// <summary>
    /// 产品族分类信息
    /// </summary>
    [Serializable]
    public class FamilyCategoryInfo
    {
        /// <summary>
        /// 产品族分类id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品族列表
        /// </summary>
        public List<FamilyInfo> FamilyList { get; } = new List<FamilyInfo>();

        /// <summary>
        /// 工艺路线列表
        /// </summary>
        public List<RoutingInfo> RoutingList { get; } = new List<RoutingInfo>();
    }

    /// <summary>
    /// 产品族信息
    /// </summary>
    [Serializable]
    public class FamilyInfo
    {
        /// <summary>
        /// 产品族id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工序信息列表
        /// </summary>
        public List<ProcessInfo> ProcessList { get; } = new List<ProcessInfo>();
    }

    /// <summary>
    /// 工序信息
    /// </summary>
    [Serializable]
    public class ProcessInfo
    {
        /// <summary>
        /// 工序ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工序类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 工序类型显示值
        /// </summary>

        public string TypeDisplay { get; set; }

        /// <summary>
        /// 引用次数
        /// </summary>
        public int ReferenceTime { get; set; }

        /// <summary>
        /// 是否批次工序
        /// </summary>
        public string IsBatch { get; set; }

        /// <summary>
        /// 启用入站控制
        /// </summary>
        public bool? EnableMoveInControl { get; set; }

        /// <summary>
        /// 交接类型
        /// </summary>
        public int? TransferType { get; set; }

        /// <summary>
        /// 是否委外
        /// </summary>
        public bool IsOutsourcing { get; set; }

        /// <summary>
        /// 是否可选
        /// </summary>

        public bool? CanChoose { get; set; }

        /// <summary>
        /// 重复过站
        /// </summary>
        public bool? IsRepeat { get; set; }

        /// <summary>
        /// 创建SKU
        /// </summary>
        public bool? IsCreateSku { get; set; }

        /// <summary>
        /// 是否计产
        /// </summary>
        public bool? IsCalculate { get; set; }
        /// <summary>
        /// 是否生成任务单
        /// </summary>
        public bool? IsGenerateTask { get; set; }

        /// <summary>
        /// 是否需求任务清单
        /// </summary>
        public bool? IsRequirementTask { get; set; }

        /// <summary>
        /// 是否扣料
        /// </summary>
        public bool? IsBuckleMaterial { get; set; }


        /// <summary>
        /// 正常胜制Id
        /// </summary>
        public double? VictoryStandardId { get; set; }

        /// <summary>
        /// 正常胜制
        /// </summary>
        public string VictoryStandard_Display { get; set; }

        /// <summary>
        /// 维修胜制Id
        /// </summary>
        public double? RepairVictoryId { get; set; }

        /// <summary>
        /// 维修胜制
        /// </summary>
        public string RepairVictory_Display { get; set; }

        /// <summary>
        /// 是否加严
        /// </summary>
        public bool? IsStricter { get; set; }

        /// <summary>
        /// 超时时间（分钟）
        /// </summary>
        public int? Overtime { get; set; }

        /// <summary>
        /// 直通率取值
        /// </summary>
        public bool? IsPassRate { get; set; }

        /// <summary>
        /// 绑定
        /// </summary>
        public bool? IsBinding { get; set; }

        /// <summary>
        /// 解绑
        /// </summary>
        public bool? IsUnBinding { get; set; }

        /// <summary>
        /// 最大过站次数
        /// </summary>
        public int? MaxPassNum { get; set; }

        /// <summary>
        /// 是否下工序入站
        /// </summary>
        public bool? IsNextMoveIn { get; set; }
    }

        /// <summary>
        /// 工艺路线信息
        /// </summary>
        [Serializable]
    public class RoutingInfo
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 默认版本id
        /// </summary>
        public double? DefaultVersionId { get; set; }

        /// <summary>
        /// 产品族分类ID
        /// 用于工艺路线过滤
        /// </summary>
        public double CategoryId { get; set; }

        /// <summary>
        /// 最大版本数
        /// </summary>
        public int MaxVersionNum { get; set; }

        /// <summary>
        /// 工艺路线版本集合
        /// </summary>
        public List<RoutingVersoinInfo> VersionList { get; } = new List<RoutingVersoinInfo>();
    }

    /// <summary>
    /// 工艺路线版本信息
    /// </summary>
    [Serializable]
    public class RoutingVersoinInfo
    {
        /// <summary>
        /// 版本id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 版本名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 引用次数
        /// </summary>
        public int ReferenceTime { get; set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        public YesNo IsDefault { get; set; }

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// 流程状态（保存/发布）
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 布局ID
        /// </summary>
        public double? LayoutId { get; set; }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public double RoutingId { get; set; }
    }

    /// <summary>
    /// 复制粘贴工艺路线版本信息
    /// </summary>
    [Serializable]
    public class CopyVersionInfo
    {
        /// <summary>
        /// 工艺路线Id
        /// </summary>
        public double RoutingId { get; set; }

        /// <summary>
        /// 工艺路线版本Id
        /// </summary>
        public double VersionId { get; set; }

        /// <summary>
        /// 复制流程属性
        /// </summary>
        public bool IsCopyActivityProperty { get; set; }

        /// <summary>
        /// 复制工序BOM
        /// </summary>
        public bool IsCopyBom { get; set; }

        /// <summary>
        /// 复制工治具需求
        /// </summary>
        public bool IsCopyFixture { get; set; }
    }
}
