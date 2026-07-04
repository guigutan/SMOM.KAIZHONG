using SIE.MES.Workbench.EmployeeManages;
using SIE.ObjectModel;
using System;
using System.Collections.ObjectModel;

namespace SIE.MES.Workbench.ProductingChecks
{
    /// <summary>
    /// 开班点检信息
    /// </summary>
    [Serializable]
    public class ProductingCheck : ObservableObject
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductingCheck()
        {
            ItemCheckList = new ObservableCollection<CheckItemResult>();
        }

        public double GroupId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 工位ID
        /// </summary>
        public double StationId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 是否关键工序
        /// </summary>
        public bool IsKeyStation
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public string Image
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public bool IsLastStation
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 当班人员
        /// </summary>
        public EmployeeInfo OnDuty
        {
            get { return GetProperty<EmployeeInfo>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 实际当班人员
        /// </summary>
        public EmployeeInfo ActualOnDuty
        {
            get { return GetProperty<EmployeeInfo>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 设备工具点检项目
        /// </summary>
        public CheckEquipmentResult CheckEquipment
        {
            get { return GetProperty<CheckEquipmentResult>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 物料点检结果
        /// </summary>
        public bool? ItemCheckResult
        {
            get { return GetProperty<bool?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 设备工具点检结果
        /// </summary>
        public bool? EquipmentCheckResult
        {
            get { return GetProperty<bool?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 物料点检项目
        /// </summary>
        public ObservableCollection<CheckItemResult> ItemCheckList { get; set; }
    }

    [Serializable]
    public class CheckItemResult
    {
        public double Id { get; set; }

        public int RowNo { get; set; }
        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId { get; set; }

        /// <summary>
        /// 物料号
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal DemandQty { get; set; }

        public decimal ArriveQty { get; set; }
        public decimal LackQty { get; set; }
        public decimal WarnQty { get; set; }
        public string State { get; set; }
        public decimal InRouteQty { get; set; }
    }

    [Serializable]
    public class CheckEquipmentResult : ObservableObject
    {
        public CheckEquipmentResult()
        {
            DetailList = new ObservableCollection<CheckEquipmentResultDetail>();
        }
        public double Id
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public string Code
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string State
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Period
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string LastUpkeepTime
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public ObservableCollection<CheckEquipmentResultDetail> DetailList { get; set; }
    }

    [Serializable]
    public class CheckEquipmentResultDetail : ObservableObject
    {
        public double Id
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }
        public string Code
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        public bool? Result
        {
            get { return GetProperty<bool?>(); }
            set { SetProperty(value); }
        }
    }
}
