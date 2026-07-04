using SIE.EMS.Equipments.RunningStates;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using SIE.Web.EMS.Equipments.RunngingStates.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Equipments.RunningStates
{
    /// <summary>
    /// 设备状态记录视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class EquipRunningStateRecordViewConfig : WebViewConfig<EquipRunningStateRecord>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.Edit, WebCommandNames.Save);
            View.UseCommands(typeof(ManualSyncEquipRunningStateRecordCommand).FullName);
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Copy, WebCommandNames.Delete);
            View.Property(p => p.EquipAccountId).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<EquipAccountController>().GetEquipAccounts(p, k);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.EquipAccountName), nameof(e.EquipAccount.Name));
                m.DicLinkField = keyValues;
            }).ShowInList(150);
            View.Property(p => p.EquipAccountName).ShowInList(150).Readonly();
            View.Property(p => p.EquipOnLineState);
            View.Property(p => p.EquipRunningState);
            View.Property(p => p.AtWhatTime).UseDateTimeEditor(m => m.DefaultValue = DateTime.Now).ShowInList(160);

            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.EquipAccount).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<EquipAccountController>().GetEquipAccounts(p, k);
            });
            View.Property(p => p.EquipOnLineState);
            View.Property(p => p.EquipRunningState);
        }

    }
}
