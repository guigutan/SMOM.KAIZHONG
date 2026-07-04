using SIE.Andon.Andons.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.ForWinform.ApiModels
{
    /// <summary>
    /// 安灯管理
    /// </summary>
    [Serializable]
    public class XPAndonManage
    {
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// 安灯事件编码
        /// </summary>
        public string AndonManageCode { get; set; }

        /// <summary>
        /// 安灯大类
        /// </summary>
        public AndonTypeClass AndonManageClass { get; set; }

        /// <summary>
        /// 安灯类型Id
        /// </summary>
        public double AndonTypeId { get; set; }

        /// <summary>
        /// 安灯类型
        /// </summary>
        //public AndonType AndonType { get; set; }

        /// <summary>
        /// 安灯名称Id
        /// </summary>
        public double AndonId { get; set; }

        /// <summary>
        /// 安灯名称
        /// </summary>
        //public Andon Andon { get; set; }

        /// <summary>
        /// 解决方案
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        public string ProblemDesc { get; set; }

        /// <summary>
        /// 负责部门
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public AndonManageState State { get; set; }

        /// <summary>
        /// 缺陷代码
        /// </summary>

        public string Defect { get; set; }

        /// <summary>
        /// 缺陷代码Id列表
        /// </summary>
        public string DefectIds { get; set; }

        /// <summary>
        /// 故障发生时间
        /// </summary>
        public DateTime FaultTime { get; set; }

        /// <summary>
        /// 触发人Id
        /// </summary>
        public double TriggerId { get; set; }

        /// <summary>
        /// 触发人
        /// </summary>
        //public Employee Trigger { get; set; }

        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime TriggerTime { get; set; }

        /// <summary>
        /// 处理人Id
        /// </summary>
        public double? HandlerId { get; set; }

        /// 处理人
        /// </summary>
        //public Employee Handler { get; set; }

        /// <summary>
        /// 关闭时间
        /// </summary>
        public DateTime? CloseTime { get; set; }

        /// <summary>(小时)
        /// 持续时间
        /// </summary>
        public double? LastTime { get; set; }

        /// <summary>
        /// 实际影响时间(小时)
        /// </summary>
        public double? ActualTime { get; set; }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        //public Enterprise Factory { get; set; }

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        //public Enterprise WorkShop { get; set; }

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? WipResourceId { get; set; }

        /// <summary>
        /// 产线
        /// </summary>
        //public WipResource WipResource { get; set; }

        /// <summary>
        /// 工位Id
        /// </summary>
        public double? StationId { get; set; }

        /// <summary>
        /// 工位
        /// </summary>
        //public Station Station { get; set; }

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double? EquipAccountId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        //public EquipAccount EquipAccount { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        //public Process Process { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 班组
        /// </summary>
        public string WorkGroup { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        //public WorkOrder WorkOrder { get; set; }

        /// <summary>
        /// 是否停线
        /// </summary>
        public bool LineStop { get; set; }

        /// <summary>
        /// 是否叫料
        /// </summary>
        public bool AskMaterial { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string PhotoFile { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string Attachment { get; set; }

        /// <summary>
        /// 停线信息Id
        /// </summary>
        public double? AbnormalCauseId { get; set; }

        /// <summary>
        /// 停线信息
        /// </summary>
        //public AbnormalCause AbnormalCause { get; set; }

        #region 事件报告
        /// <summary>
        /// 事件原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 处理方式
        /// </summary>
        public string HandleMethod { get; set; }

        /// <summary>
        /// 预防措施
        /// </summary>
        public string Measures { get; set; }

        /// <summary>
        /// 安灯管理经验库标识
        /// </summary>
        public bool ExperienceFlag { get; set; }
        #endregion

        /// <summary>
        /// 操作记录
        /// </summary>
        //public List<AndonManageOperateLog> OperateLogList { get; set; }

        /// <summary>
        /// 物料明细
        /// </summary>
        //public List<AndonManageCallMaterial> ItemDetail { get; set; }

        /// <summary>
        /// 消息推送
        /// </summary>
        //public List<AndonManageMessageSend> MessageSendList { get; set; }

        #region 视图属性
        /// <summary>
        /// 优先级
        /// </summary>
        public string Priority { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountCode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public string WipResourceName { get; set; }

        /// 停线只读标识
        /// </summary>
        public AndonYesOrNo LineStopFlag { get; set; }

        /// <summary>
        /// 叫料只读标识
        /// </summary>
        public AndonYesOrNo AskMaterialFlag { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string OrderNum { get; set; }

        /// <summary>
        /// 安灯类型名称
        /// </summary>
        public string AndonTypeName { get; set; }

        /// <summary>
        /// 安灯名称
        /// </summary>
        public string AndonName { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 触发人
        /// </summary>
        public string TriggerByName { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public string HandlerName { get; set; }

        /// <summary>
        /// 安灯编码
        /// </summary>
        public string AndonCode { get; set; }


        #endregion


        /// <summary>
        /// 状态描述
        /// </summary>

        public string StateDesc { get; set; }

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 根据AndonManage创建
        /// </summary>
        /// <param name="am"></param>
        /// <returns></returns>
        public static XPAndonManage Gen(AndonManage am)
        {
            return new XPAndonManage() {
                Id = am.Id,
                AndonManageCode = am.AndonManageCode,
                AndonManageClass = am.AndonManageClass,
                AndonTypeId = am.AndonTypeId,
                //AndonType = am.AndonType,
                AndonId = am.AndonId,
                //Andon = am.Andon,
                Solution = am.Solution,
                ProblemDesc = am.ProblemDesc,
                Department = am.Department,
                State = am.State,
                Defect = am.Defect,
                DefectIds = am.DefectIds,
                FaultTime = am.FaultTime,
                TriggerId = am.TriggerId,
                //Trigger = am.Trigger,
                TriggerTime = am.TriggerTime,
                HandlerId = am.HandlerId,
                //Handler = am.Handler,
                CloseTime = am.CloseTime,
                LastTime = am.LastTime,
                ActualTime = am.ActualTime,
                FactoryId = am.FactoryId,
                //Factory = am.Factory,
                WorkShopId = am.WorkShopId,
                //WorkShop = am.WorkShop,
                WipResourceId = am.WipResourceId,
                //WipResource = am.WipResource,
                StationId = am.StationId,
                //Station = am.Station,
                EquipAccountId = am.EquipAccountId,
                //EquipAccount = am.EquipAccount,
                ProcessId = am.ProcessId,
                //Process = am.Process,
                BarCode = am.BarCode,
                WorkGroup = am.WorkGroup,
                WorkOrderId = am.WorkOrderId,
                //WorkOrder = am.WorkOrder,
                LineStop = am.LineStop,
                AskMaterial = am.AskMaterial,
                PhotoFile = am.PhotoFile,
                Attachment = am.Attachment,
                AbnormalCauseId = am.AbnormalCauseId,
                //AbnormalCause = am.AbnormalCause,
                Reason = am.Reason,
                HandleMethod = am.HandleMethod,
                Measures = am.Measures,
                ExperienceFlag = am.ExperienceFlag,
                //OperateLogList = am.OperateLogList,
                //ItemDetail = am.ItemDetail,
                //MessageSendList = am.MessageSendList,
                Priority = am.Priority,
                EquipAccountName = am.EquipAccountName,
                EquipAccountCode = am.EquipAccountCode,
                ProductCode = am.ProductCode,
                ProductName = am.ProductName,
                WipResourceName = am.WipResourceName,
                LineStopFlag = am.LineStopFlag,
                AskMaterialFlag = am.AskMaterialFlag,
                WoNo = am.WoNo,
                OrderNum = am.OrderNum,
                AndonTypeName = am.AndonTypeName,
                AndonName = am.AndonName,
                WorkShopName = am.WorkShopName,
                ProcessName = am.ProcessName,
                StationName = am.StationName,
                TriggerByName = am.TriggerByName,
                HandlerName = am.HandlerName,
                AndonCode = am.AndonCode,
                StateDesc = am.StateDesc,
                FactoryName = am.FactoryName
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="am"></param>
        /// <returns></returns>
        public AndonManage ToAndonManage()
        {
            return new AndonManage() {
                Id = this.Id,
                AndonManageCode = this.AndonManageCode,
                AndonManageClass = this.AndonManageClass,
                AndonTypeId = this.AndonTypeId,
                //AndonType = this.AndonType,
                AndonId = this.AndonId,
                //Andon = this.Andon,
                Solution = this.Solution,
                ProblemDesc = this.ProblemDesc,
                Department = this.Department,
                State = this.State,
                Defect = this.Defect,
                DefectIds = this.DefectIds,
                FaultTime = this.FaultTime,
                TriggerId = this.TriggerId,
                //Trigger = this.Trigger,
                TriggerTime = this.TriggerTime,
                HandlerId = this.HandlerId,
                //Handler = this.Handler,
                CloseTime = this.CloseTime,
                LastTime = this.LastTime,
                ActualTime = this.ActualTime,
                FactoryId = this.FactoryId,
                //Factory = this.Factory,
                WorkShopId = this.WorkShopId,
                //WorkShop = this.WorkShop,
                WipResourceId = (double)this.WipResourceId,
                //WipResource = this.WipResource,
                StationId = this.StationId,
                //Station = this.Station,
                EquipAccountId = this.EquipAccountId,
                //EquipAccount = this.EquipAccount,
                ProcessId = this.ProcessId,
                //Process = this.Process,
                BarCode = this.BarCode,
                WorkGroup = this.WorkGroup,
                WorkOrderId = this.WorkOrderId,
                //WorkOrder = this.WorkOrder,
                LineStop = this.LineStop,
                AskMaterial = this.AskMaterial,
                PhotoFile = this.PhotoFile,
                Attachment = this.Attachment,
                AbnormalCauseId = this.AbnormalCauseId,
                //AbnormalCause = this.AbnormalCause,
                Reason = this.Reason,
                HandleMethod = this.HandleMethod,
                Measures = this.Measures,
                ExperienceFlag = this.ExperienceFlag,
                //OperateLogList = this.OperateLogList,
                //ItemDetail = this.ItemDetail,
                //MessageSendList = this.MessageSendList,
                //Priority = this.Priority,
                //EquipAccountName = this.EquipAccountName,
                //EquipAccountCode = this.EquipAccountCode,
                //ProductCode = this.ProductCode,
                //ProductName = this.ProductName,
                //WipResourceName = this.WipResourceName,
                LineStopFlag = this.LineStopFlag,
                AskMaterialFlag = this.AskMaterialFlag,
                WoNo = this.WoNo,
                //OrderNum = this.OrderNum,
                AndonTypeName = this.AndonTypeName,
                AndonName = this.AndonName,
                //WorkShopName = this.WorkShopName,
                //ProcessName = this.ProcessName,
                //StationName = this.StationName,
                //TriggerByName = this.TriggerByName,
                //HandlerName = this.HandlerName,
                //AndonCode = this.AndonCode,
                //StateDesc = this.StateDesc,
                //FactoryName = this.FactoryName
            };
        }
    }
}
