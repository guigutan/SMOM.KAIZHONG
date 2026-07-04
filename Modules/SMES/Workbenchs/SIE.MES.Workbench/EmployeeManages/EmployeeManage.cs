using SIE.ObjectModel;
using System;
using System.Collections.ObjectModel;

namespace SIE.MES.Workbench.EmployeeManages
{
    /// <summary>
    /// 员工管理
    /// </summary>
    [Serializable]
    public class EmployeeManage : ObservableObject
    {
        /// <summary>
        /// 产线ID
        /// </summary>
        public double ResourceId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 班次ID
        /// </summary>
        public double ShiftId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 旧产线ID
        /// </summary>
        public double OldResourceId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 旧班次ID
        /// </summary>
        public double OldShiftId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 应到人数
        /// </summary>
        public int DueQty
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 已到人数
        /// </summary>
        public int ArrivedQty
        {
            get { return GetProperty<int>(); }
            set
            {
                if (ArrivedQty == DueQty)
                    return;
                AbsenteeismQty = DueQty - value;
                SetProperty(value);
            }
        }

        /// <summary>
        /// 缺勤人数
        /// </summary>
        public int AbsenteeismQty
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public string EmployeeType
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public ObservableCollection<EmployeeInfo> Employees { get; set; } = new ObservableCollection<EmployeeInfo>();

        public ObservableCollection<EmployeeInfo> OnLoanEmployees { get; set; } = new ObservableCollection<EmployeeInfo>();
    }

    [Serializable]
    public class EmployeeInfo : ObservableObject
    {
        public double Id
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 工号
        /// </summary>
        public string No
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Type
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string ShortType
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Aptitudes
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public byte[] Photo
        {
            get { return GetProperty<byte[]>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 是否借调
        /// </summary>
        public bool IsOnLoan
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 是否缺勤
        /// </summary>
        public bool IsAbsenteeism
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public bool AddEmployee
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }
    }
    [Serializable]
    public class EmplyeeType : ObservableObject
    {
        public string Type { get; set; }

        public string TypeName { get; set; }
    }
}
