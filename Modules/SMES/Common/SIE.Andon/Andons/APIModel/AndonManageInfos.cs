using SIE.Andon.Andons.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.APIModel
{
    /// <summary>
    /// 安灯管理信息
    /// </summary>
    [Serializable]
    public class AndonManageInfo
    {
        /// <summary>
        /// 构造
        /// </summary>
        public AndonManageInfo()
        {
            this.AttachmentInfos = new List<AttachmentInfo>();
        }

        /// <summary>
        /// 安灯管理Id
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// 安灯编码
        /// </summary>
        public string AndonManageCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public AndonManageState State { get; set; }

        /// <summary>
        /// 触发者Id
        /// </summary>
        public double? TriggerId { get; set; }

        /// <summary>
        /// 触发
        /// </summary>
        public DateTime TriggerTime { get; set; }

        /// <summary>
        /// 发生时间
        /// </summary>
        public string FaultTime { get; set; }

        /// <summary>
        /// 班组
        /// </summary>
        public string WorkGroup { get; set; }

        /// <summary>
        /// 班组Id
        /// </summary>
        public double? WorkGroupId { get; set; }

        /// <summary>
        /// 安灯大类
        /// </summary>
        public string AndonManageClass { get; set; }

        /// <summary>
        /// 安灯ID
        /// </summary>
        public double AndonId { get; set; }

        /// <summary>
        /// 解决方案
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public double DepartmentId { get; set; }

        /// <summary>
        /// 安灯名称
        /// </summary>
        public string AndonName { get; set; }

        /// <summary>
        ///安灯类型名称
        /// </summary>
        public string AndonTypeName { get; set; }

        /// <summary>
        /// 安灯类型Id
        /// </summary>
        public double AndonTypeId { get; set; }

        /// <summary>
        /// 停线标志
        /// </summary>
        public AndonYesOrNo LineStopFlag { get; set; }

        /// <summary>
        /// 叫料标志
        /// </summary>
        public AndonYesOrNo AskMaterialFlag { get; set; }

        /// <summary>
        /// 是否 叫料
        /// </summary>
        public bool AskMaterial { get; set; }

        /// <summary>
        /// 是否停线
        /// </summary>

        public bool LineStop { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double woId { get; set; }

        /// <summary>
        ///工单编号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId { get; set; }

        /// <summary>
        ///车间
        /// </summary>
        public double WorkShopId { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShop { get; set; }

        /// <summary>
        /// 产线
        /// </summary>
        public double WipResourceId { get; set; }

       

        /// <summary>
        /// 产线
        /// </summary>
        public string WipResource { get; set; }

        /// <summary>
        /// 工位
        /// </summary>
        public double? StationId { get; set; }

        /// <summary>
        /// 工位
        /// </summary>
        public string Station { get; set; }

        /// <summary>
        /// 设备台账
        /// </summary>
        public double? EquipAccountId { get; set; }

       /// <summary>
       /// 设备名称
       /// </summary>
        public string EquipAccountName { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode { get; set; }

        /// <summary>
        /// 工序
        /// </summary>

        public double? ProcessId { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public string Defect { get; set; }

        /// <summary>
        /// 缺陷代码Id列表
        /// </summary>
        public string DefectIds { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>

        public string ProblemDesc { get; set; }

        /// <summary>
        /// 附件信息
        /// </summary>
        public List<AttachmentInfo> AttachmentInfos { get; set; }

        /// <summary>
        ///处理人
        /// </summary>
        public double? HandleId { get; set; }
    }

    /// <summary>
    /// 附件信息
    /// </summary>
    [Serializable]
    public class AttachmentInfo
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FliePath { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }

    /// <summary>
    /// 物料、仓库、库位信息
    /// </summary>
    public class CallMaterialInfo
    {
        /// <summary>
        /// Id
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
    }

    /// <summary>
    /// 生成备料单提交信息
    /// </summary>
    public class SubmitCallMaterialInfo
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 本次备料量
        /// </summary>
        public decimal Qty {  get; set; }

        /// <summary>
        /// 需求时间
        /// </summary>
        public string NeedTime { get; set; }

        /// <summary>
        /// 线边仓仓库ID
        /// </summary>
        public double? LineWareId { get; set; }

        /// <summary>
        /// 线边仓仓库编码
        /// </summary>
        public string LineWareCode { get; set; }

        /// <summary>
        /// 线边仓库位Id
        /// </summary>
        public double? LineStorageId { get; set; }

        /// <summary>
        /// 线边仓库位编码
        /// </summary>
        public string LineStorageCode { get; set; }
    }
}
