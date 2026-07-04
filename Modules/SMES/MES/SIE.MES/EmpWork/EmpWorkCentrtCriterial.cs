using SIE.Domain;
using SIE.MES.Fixture;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WorkCenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.EmpWork
{
    /// <summary>
    /// 人员与工作中心查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("人员与工作中心查询实体")]
    public class EmpWorkCentrtCriterial : Criteria
    {
        #region 姓名 Employee
        /// <summary>
        /// 姓名Id
        /// </summary>
        [Label("姓名")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<EmpWorkCentrtCriterial>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 姓名Id
        /// </summary>
        public double? EmployeeId
        {
            get { return (double?)GetRefNullableId(EmployeeIdProperty); }
            set { SetRefNullableId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 姓名
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<EmpWorkCentrtCriterial>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 姓名
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 工号 EmpNo
        /// <summary>
        /// 工号
        /// </summary>
        [Label("工号")]
        public static readonly Property<string> EmpNoProperty = P<EmpWorkCentrtCriterial>.Register(e => e.EmpNo);

        /// <summary>
        /// 工号
        /// </summary>
        public string EmpNo
        {
            get { return this.GetProperty(EmpNoProperty); }
            set { this.SetProperty(EmpNoProperty, value); }
        }
        #endregion

        #region 工作中心编码 WorkCenter
        /// <summary>
        /// 工作中心编码Id
        /// </summary>
        [Label("工作中心编码")]
        public static readonly IRefIdProperty WorkCenterIdProperty = P<EmpWorkCentrtCriterial>.RegisterRefId(e => e.WorkCenterId, ReferenceType.Normal);

        /// <summary>
        /// 工作中心编码Id
        /// </summary>
        public double? WorkCenterId
        {
            get { return (double?)GetRefNullableId(WorkCenterIdProperty); }
            set { SetRefNullableId(WorkCenterIdProperty, value); }
        }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public static readonly RefEntityProperty<WorkCenter> WorkCenterProperty = P<EmpWorkCentrtCriterial>.RegisterRef(e => e.WorkCenter, WorkCenterIdProperty);

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public WorkCenter WorkCenter
        {
            get { return GetRefEntity(WorkCenterProperty); }
            set { SetRefEntity(WorkCenterProperty, value); }
        }
        #endregion

        #region 工作中心名称 WorkName
        /// <summary>
        /// 工作中心名称
        /// </summary>
        [Label("工作中心名称")]
        public static readonly Property<string> WorkNameProperty = P<EmpWorkCentrtCriterial>.Register(e => e.WorkName);

        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string WorkName
        {
            get { return this.GetProperty(WorkNameProperty); }
            set { this.SetProperty(WorkNameProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EmpWorkCentrtController>().CriterialEmpWorkCentrt(this);
        }
    }
}
