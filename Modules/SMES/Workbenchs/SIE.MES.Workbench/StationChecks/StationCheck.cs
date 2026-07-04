using SIE.ObjectModel;
using System;

namespace SIE.MES.Workbench.StationChecks
{
    /// <summary>
    /// 工位点检
    /// </summary>
    [Serializable]
    public class StationCheck : ObservableObject
    {
        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 检验项目Id
        /// </summary>
        public double CheckItemId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string WorkType
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 点检类型
        /// </summary>
        public CheckType CheckType
        {
            get { return GetProperty<CheckType>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 编号
        /// </summary>
        public string Code
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal? DemandQty
        {
            get { return GetProperty<decimal?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 点检状态
        /// </summary>
        public bool State
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }
    }
}
