using SIE.Domain;
using SIE.EMS.Equipments;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;
using System.Linq;

namespace SIE.Web.EMS.Equipments.Accounts.ViewModels
{
    /// <summary>
    /// 设备台账降级ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class DowngradeAccountViewModel : ViewModel
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<DowngradeAccountViewModel>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefNullableId(EquipAccountIdProperty); }
            set { this.SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<DowngradeAccountViewModel>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 当前台账ID CurrentAccountId
        /// <summary>
        /// 当前台账ID
        /// </summary>
        [Label("当前台账ID")]
        public static readonly Property<double> CurrentAccountIdProperty = P<DowngradeAccountViewModel>.Register(e => e.CurrentAccountId);

        /// <summary>
        /// 当前台账ID
        /// </summary>
        public double CurrentAccountId
        {
            get { return this.GetProperty(CurrentAccountIdProperty); }
            set { this.SetProperty(CurrentAccountIdProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 设备台账降级ViewModel ViewConfig
    /// </summary>
    public class DowngradeAccountViewModelViewConfig : WebViewConfig<DowngradeAccountViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.EquipAccountId).HasLabel("目标父台账").Show(ShowInWhere.All).UseDataSource((e, p, k) =>
            {
                var entity = e as DowngradeAccountViewModel;
                var list = RT.Service.Resolve<EquipController>().GetDowngradeEquipAccounts(entity.CurrentAccountId, k, p);

                //有过滤的话可能Parent没有在查询结果中，则将查询结果的Tree Parent Id 清空，否则会报错
                if (!k.IsNullOrEmpty())
                {
                    list.ForEach(x => x.TreePId = null);
                }

                return list;
            });
        }
    }
}
