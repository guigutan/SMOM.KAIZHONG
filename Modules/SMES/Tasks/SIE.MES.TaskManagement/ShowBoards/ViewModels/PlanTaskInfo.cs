using SIE.Core.WorkOrders;
using SIE.MES.TaskManagement.Dispatchs;
using System;

namespace SIE.MES.TaskManagement.ShowBoards.ViewModels
{
    /// <summary>
    /// 任务单信息
    /// </summary>
    [Serializable]
    public class PlanTaskInfo
    {
        #region 任务单Id
        /// <summary>
        /// 任务单Id
        /// </summary>
        private double dispatchTaskId;

        /// <summary>
        /// 任务单Id
        /// </summary>
        public double DispatchTaskId
        {
            get
            {
                return dispatchTaskId;
            }

            set
            {
                dispatchTaskId = value;
            }
        }
        #endregion

        #region 任务单编号
        /// <summary>
        /// 任务单编号
        /// </summary>
        private string dispatchTaskNo;

        /// <summary>
        /// 任务单编号
        /// </summary>
        public string DispatchTaskNo
        {
            get
            {
                return dispatchTaskNo;
            }

            set
            {
                dispatchTaskNo = value;
            }
        }
        #endregion

        #region 计划开始时间
        /// <summary>
        /// 计划开始时间
        /// </summary>
        private DateTime planBeginTime;

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginTime
        {
            get
            {
                return planBeginTime;
            }

            set
            {
                planBeginTime = value;
            }
        }
        #endregion

        #region 计划结束时间
        /// <summary>
        /// 计划结束时间
        /// </summary>
        private DateTime planEndTime;

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEndTime
        {
            get
            {
                return planEndTime;
            }

            set
            {
                planEndTime = value;
            }
        }
        #endregion

        #region 任务数量
        /// <summary>
        /// 任务数量
        /// </summary>
        private decimal dispatchQty;

        /// <summary>
        /// 任务数量
        /// </summary>
        public decimal DispatchQty
        {
            get
            {
                return dispatchQty;
            }

            set
            {
                dispatchQty = value;
            }
        }
        #endregion

        #region 已报工数量
        /// <summary>
        /// 已报工数量
        /// </summary>
        private decimal reportQty;

        /// <summary>
        /// 已报工数量
        /// </summary>
        public decimal ReportQty
        {
            get
            {
                return reportQty;
            }

            set
            {
                reportQty = value;
            }
        }
        #endregion

        #region 任务状态
        /// <summary>
        /// 任务状态
        /// </summary>
        private DispatchTaskStatus taskStatus;

        /// <summary>
        /// 任务状态
        /// </summary>
        public DispatchTaskStatus TaskStatus
        {
            get
            {
                return taskStatus;
            }

            set
            {
                taskStatus = value;
            }
        }
        #endregion 

        #region 产品名称
        /// <summary>
        /// 产品名称
        /// </summary>
        private string productName;

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get
            {
                return productName;
            }

            set
            {
                productName = value;
            }
        }
        #endregion

        #region 工单编号
        /// <summary>
        /// 工单编号
        /// </summary>
        private string workOrderNo;

        /// <summary>
        /// 工单编号
        /// </summary>
        public string WorkOrderNo
        {
            get
            {
                return workOrderNo;
            }

            set
            {
                workOrderNo = value;
            }
        }
        #endregion

        #region 工单类型
        /// <summary>
        /// 工单类型
        /// </summary>
        private WorkOrderType workOrderType;

        /// <summary>
        /// 工单类型
        /// </summary>
        public WorkOrderType WorkOrderType
        {
            get
            {
                return workOrderType;
            }

            set
            {
                workOrderType = value;
            }
        }
        #endregion

        #region 对象名称（员工ID|班组ID|员工组ID)
        /// <summary>
        /// 对象名称（员工ID|班组ID|员工组ID)
        /// </summary>
        private double adoId;

        /// <summary>
        /// 对象名称（员工ID|班组ID|员工组ID)
        /// </summary>
        public double AdoId
        {
            get
            {
                return adoId;
            }

            set
            {
                adoId = value;
            }
        }
        #endregion

        #region 对象类型
        /// <summary>
        /// 对象类型
        /// </summary>
        private AdoType adoType;

        /// <summary>
        /// 对象类型
        /// </summary>
        public AdoType AdoType
        {
            get
            {
                return adoType;
            }

            set
            {
                adoType = value;
            }
        }
        #endregion

        #region 员工类型 AdoGroup(只针对员工对象，区分员工是否属于员工组或班组)
        /// <summary>
        /// 员工类型
        /// </summary>
        private AdoGroup? adoGroup;

        /// <summary>
        /// 员工类型
        /// </summary>
        public AdoGroup? AdoGroup
        {
            get
            {
                return adoGroup;
            }

            set
            {
                adoGroup = value;
            }
        }
        #endregion

        #region 班组名称
        /// <summary>
        /// 班组名称
        /// </summary>
        private string workGroupName;

        /// <summary>
        /// 班组名称
        /// </summary>
        public string WorkGroupName
        {
            get
            {
                return workGroupName;
            }

            set
            {
                workGroupName = value;
            }
        }
        #endregion

        #region 员工班组名称
        /// <summary>
        /// 员工班组名称
        /// </summary>
        private string workGroupNameOfEmployee;

        /// <summary>
        /// 员工班组名称
        /// </summary>
        public string WorkGroupNameOfEmployee
        {
            get
            {
                return workGroupNameOfEmployee;
            }

            set
            {
                workGroupNameOfEmployee = value;
            }
        }
        #endregion

        #region 员工组名称
        /// <summary>
        /// 员工组名称
        /// </summary>
        private string employeeGroupName;

        /// <summary>
        /// 员工组名称
        /// </summary>
        public string EmployeeGroupName
        {
            get
            {
                return employeeGroupName;
            }

            set
            {
                employeeGroupName = value;
            }
        }
        #endregion

        #region 员工员工组名称
        /// <summary>
        /// 员工员工组名称
        /// </summary>
        private string employeeGroupNameOfEmployee;

        /// <summary>
        /// 员工员工组名称
        /// </summary>
        public string EmployeeGroupNameOfEmployee
        {
            get
            {
                return employeeGroupNameOfEmployee;
            }

            set
            {
                employeeGroupNameOfEmployee = value;
            }
        }
        #endregion
    }
}
