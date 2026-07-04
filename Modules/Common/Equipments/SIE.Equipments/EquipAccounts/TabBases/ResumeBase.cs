using SIE.Core.Enums;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Equipments.EquipAccounts.TabBases
{
    /// <summary>
    /// 台账履历基类
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("台账履历基类")]
    public partial class ResumeBase : DataEntity
    {
        #region 设备状态 State
        /// <summary>
        /// 设备状态
        /// </summary>
        [Label("设备状态")]
        public static readonly Property<AccountState> StateProperty = P<ResumeBase>.Register(e => e.State);

        /// <summary>
        /// 设备状态
        /// </summary>
        public AccountState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 类型 ResumeType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<ResumeType?> ResumeTypeProperty = P<ResumeBase>.Register(e => e.ResumeType);

        /// <summary>
        /// 类型
        /// </summary>
        public ResumeType? ResumeType
        {
            get { return GetProperty(ResumeTypeProperty); }
            set { SetProperty(ResumeTypeProperty, value); }
        }
        #endregion

        #region 单据编号 No
        /// <summary>
        /// 单据编号
        /// </summary>
        [Label("单据编号")]
        public static readonly Property<string> NoProperty = P<ResumeBase>.Register(e => e.No);

        /// <summary>
        /// 单据编号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 变更 Changed
        /// <summary>
        /// 变更
        /// </summary>
        [Label("变更")]
        public static readonly Property<string> ChangedProperty = P<ResumeBase>.Register(e => e.Changed);

        /// <summary>
        /// 变更
        /// </summary>
        public string Changed
        {
            get { return GetProperty(ChangedProperty); }
            set { SetProperty(ChangedProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ResumeBase>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 设备履历 实体配置
    /// </summary>
    internal class ResumeBaseConfig : EntityConfig<ResumeBase>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_RESUME").MapAllProperties();
            Meta.Property(ResumeBase.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
