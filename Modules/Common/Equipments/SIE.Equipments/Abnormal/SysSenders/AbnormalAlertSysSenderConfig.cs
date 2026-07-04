using Newtonsoft.Json;
using SIE.Common.Sender.SystemSender;
using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.Equipments.Abnormal.SysSenders
{
    /// <summary>
    /// 停机管理触发任务配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("停机管理触发任务配置")]
    public class AbnormalAlertSysSenderConfig : AlertSystemSenderConfig
    {
        #region 是否自动恢复停线 IsAutoRestore
        /// <summary>
        /// 是否自动恢复停线
        /// </summary>
        [Label("是否自动恢复停线")]
        public static readonly Property<bool> IsAutoRestoreProperty = P<AbnormalAlertSysSenderConfig>.Register(e => e.IsAutoRestore);

        /// <summary>
        /// 是否自动恢复停线
        /// </summary>
        public bool IsAutoRestore
        {
            get { return GetProperty(IsAutoRestoreProperty); }
            set { SetProperty(IsAutoRestoreProperty, value); }
        }
        #endregion


        #region 产线 Line
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty LineIdProperty =
            P<AbnormalAlertSysSenderConfig>.RegisterRefId(e => e.LineId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? LineId
        {
            get { return (double?)GetRefNullableId(LineIdProperty); }
            set { SetRefNullableId(LineIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> LineProperty =
            P<AbnormalAlertSysSenderConfig>.RegisterRef(e => e.Line, LineIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource Line
        {
            get { return GetRefEntity(LineProperty); }
            set { SetRefEntity(LineProperty, value); }
        }
        #endregion

        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<AbnormalAlertSysSenderConfig>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)GetRefNullableId(EquipAccountIdProperty); }
            set { SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<AbnormalAlertSysSenderConfig>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 产线名称 LineName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("产线名称")]
        public static readonly Property<string> LineNameProperty = P<AbnormalAlertSysSenderConfig>.RegisterView(e => e.LineName, p => p.Line.Name);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string LineName
        {
            get { return GetProperty(LineNameProperty); }
        }
        #endregion

        #region 设备名称 EquipName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipNameProperty = P<AbnormalAlertSysSenderConfig>.RegisterView(e => e.EquipName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName
        {
            get { return GetProperty(EquipNameProperty); }
        }
        #endregion

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="value"></param>
        public override void Initialize(string value)
        {
            if (value.IsNullOrWhiteSpace())
                return;

            var config = JsonConvert.DeserializeObject<AbnormalAlertSysSenderConfig>(value);

            LineId = config.LineId;
            EquipAccountId = config.EquipAccountId;
            LoadProperty(LineNameProperty, config.LineName);
            LoadProperty(EquipNameProperty, config.EquipName);
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            var o = new
            {
                LineId,
                LineId_Display = "",
                EquipAccountId,
                EquipAccountId_Display = "",
                LineName,
                EquipName,
                IsAutoRestore
            };
            string ret = JsonConvert.SerializeObject(o);
            return ret;
        }
    }
}
