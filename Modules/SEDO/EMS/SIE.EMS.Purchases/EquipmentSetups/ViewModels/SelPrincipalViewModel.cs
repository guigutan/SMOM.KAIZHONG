using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.Purchases.EquipmentSetups.ViewModels
{
    /// <summary>
    /// 选择员工 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("选择员工")]
    public class SelPrincipalViewModel : Entity<double>
    {
        #region 负责人 Principal
        /// <summary>
        /// 负责人Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty PrincipalIdProperty = P<SelPrincipalViewModel>.RegisterRefId(e => e.PrincipalId, ReferenceType.Normal);

        /// <summary>
        /// 负责人Id
        /// </summary>
        public double PrincipalId
        {
            get { return (double)GetRefId(PrincipalIdProperty); }
            set { SetRefId(PrincipalIdProperty, value); }
        }

        /// <summary>
        /// 负责人
        /// </summary>
        public static readonly RefEntityProperty<Employee> PrincipalProperty = P<SelPrincipalViewModel>.RegisterRef(e => e.Principal, PrincipalIdProperty);

        /// <summary>
        /// 负责人
        /// </summary>
        public Employee Principal
        {
            get { return GetRefEntity(PrincipalProperty); }
            set { SetRefEntity(PrincipalProperty, value); }
        }
        #endregion
    }
}
