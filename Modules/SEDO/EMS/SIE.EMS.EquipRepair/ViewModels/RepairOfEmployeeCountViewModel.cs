using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ViewModels
{
    /// <summary>
    /// 维修员参与单据数量VM
    /// </summary>
    [RootEntity, Serializable]
    public class RepairOfEmployeeCountViewModel : ViewModel
    {
        #region 维修员ID RepairerId
        /// <summary>
        /// 维修员ID
        /// </summary>
        [Label("维修员ID")]
        public static readonly Property<double> RepairerIdProperty = P<RepairOfEmployeeCountViewModel>.Register(e => e.RepairerId);

        /// <summary>
        /// 维修员ID
        /// </summary>
        public double RepairerId
        {
            get { return this.GetProperty(RepairerIdProperty); }
            set { this.SetProperty(RepairerIdProperty, value); }
        }
        #endregion

        #region 单据数量 Count
        /// <summary>
        /// 单据数量
        /// </summary>
        [Label("单据数量")]
        public static readonly Property<int> CountProperty = P<RepairOfEmployeeCountViewModel>.Register(e => e.Count);

        /// <summary>
        /// 单据数量
        /// </summary>
        public int Count
        {
            get { return this.GetProperty(CountProperty); }
            set { this.SetProperty(CountProperty, value); }
        }
        #endregion

    }
}
