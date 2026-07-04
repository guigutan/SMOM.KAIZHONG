using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    #region 接口返回：用户管理的单据数量
    /// <summary>
    /// 用户管理的单据数量
    /// </summary>
    [Serializable]
    public class UserManageBillCountInfo
    {
        /// <summary>
        /// 待处理和待确认单据信息
        /// </summary>
        public PendingBillInfo PendingBillInfo { get; set; }

        /// <summary>
        /// 处理中单据
        /// </summary>
        public PendingBillInfo ProcessingBillInfo { get; set; }
        /// <summary>
        /// 已完成单据
        /// </summary>
        public ComplateBillInfo CompleteBillInfo { get; set; }

    }
    /// <summary>
    /// 待处理和待确认单据信息
    /// </summary>
    public class PendingBillInfo
    {
        /// <summary>
        /// 单据总数
        /// </summary>
        public int BillCount { get; set; }
        /// <summary>
        /// 单据信息
        /// </summary>
        public List<BillInfo> BillInfoList { get; set; }

    }

    /// <summary>
    /// 处理中单据
    /// </summary>
    public class ProcessingBillInfo
    {
        /// <summary>
        /// 单据总数
        /// </summary>
        public int BillCount { get; set; }
        /// <summary>
        /// 单据信息
        /// </summary>
        public List<BillInfo> BillInfoList { get; set; }
    }
    /// <summary>
    /// 已完成单据
    /// </summary>
    public class ComplateBillInfo
    {
        /// <summary>
        /// 单据总数
        /// </summary>
        public int BillCount { get; set; }
        /// <summary>
        /// 单据信息
        /// </summary>
        public List<BillInfo> BillInfoList { get; set; }
    }
    /// <summary>
    /// 单据信息
    /// </summary>
    public class BillInfo
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
    }
    #endregion

    #region 参数：菜单信息
    /// <summary>
    /// 菜单信息
    /// </summary>
    [Serializable]
    public class MenuInfo
    {
        /// <summary>
        /// 待处理和待确认菜单信息
        /// </summary>
        public PendingMenuInfo PendingMenuInfo { get; set; }
        /// <summary>
        /// 处理中菜单信息
        /// </summary>
        public ProcessingMenuInfo ProcessingMenuInfo { get; set; }
        /// <summary>
        /// 已完成菜单信息
        /// </summary>
        public CompletedMenuInfo CompletedMenuInfo { get; set; }
    }

    /// <summary>
    /// 待处理和待确认菜单信息
    /// </summary>
    [Serializable]
    public class PendingMenuInfo
    {
        /// <summary>
        /// 点检待执行
        /// </summary>
        public string CheckNotPerformed { get; set; }
        /// <summary>
        /// 点检待确认
        /// </summary>
        public string CheckNotConfirm { get; set; }
        /// <summary>
        /// 保养待执行
        /// </summary>
        public string MaintainNotPerformed { get; set; }
        /// <summary>
        /// 保养待确认
        /// </summary>
        public string MaintainNotConfirm { get; set; }
        /// <summary>
        /// 润滑待执行
        /// </summary>
        public string LubricationNotPerformed { get; set; }
        /// <summary>
        /// 维修接单
        /// </summary>
        public string RepairReceive { get; set; }
        /// <summary>
        /// 维修待执行
        /// </summary>
        public string RepairApplyRepair { get; set; }
        /// <summary>
        /// 维修待确认
        /// </summary>
        public string RepairWaitConfirm { get; set; }
        /// <summary>
        /// 工程评分
        /// </summary>
        public string EngineerConfirm { get; set; }
    }

    /// <summary>
    /// 处理中菜单信息
    /// </summary>
    [Serializable]
    public class ProcessingMenuInfo
    {
        /// <summary>
        /// 点检中菜单编码
        /// </summary>
        public string Checking { get; set; }
        /// <summary>
        /// 保养中菜单编码
        /// </summary>
        public string Maintaining { get; set; }
        /// <summary>
        /// 润滑中菜单编码
        /// </summary>
        public string Lubricationing { get; set; }
        /// <summary>
        /// 维修中菜单编码
        /// </summary>
        public string Repairing { get; set; }
        /// <summary>
        /// 交机确认菜单编码
        /// </summary>
        public string HandoverConfirm { get; set; }
        /// <summary>
        /// 工程评分菜单编码
        /// </summary>
        public string EngineerConfirm { get; set; }
    }

    /// <summary>
    /// 已完成菜单信息
    /// </summary>
    public class CompletedMenuInfo
    {
        /// <summary>
        /// 点检已完成
        /// </summary>
        public string CheckPerformed { get; set; }
        /// <summary>
        /// 保养已完成
        /// </summary>
        public string MaintainPerformed { get; set; }
        /// <summary>
        /// 润滑已完成
        /// </summary>
        public string LubricationPerformed { get; set; }
        /// <summary>
        /// 维修已完成
        /// </summary>
        public string RepairCompleted { get; set; }

    }

    #endregion
}
