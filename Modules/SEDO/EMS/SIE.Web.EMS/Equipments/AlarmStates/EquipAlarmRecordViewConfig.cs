using SIE.Domain;
using SIE.EMS.Equipments.AlarmStates;
using SIE.MetaModel.View;
using SIE.Web.EMS.Equipments.AlarmStates.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.Equipments.AlarmStates
{
    /// <summary>
    /// 报警明细视图配置
    /// </summary>
    internal class EquipAlarmRecordViewConfig : WebViewConfig<EquipAlarmRecord>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
#if DEBUG
            //调试环境引用未 Echart的压缩版本
          //  View.RequireResource("~/lib/echarts.js", Domain.Enums.MIMEType.Script);
#else
          //  View.RequireResource("~/lib/echarts.min.js", Domain.Enums.MIMEType.Script);
#endif
            View.AssignAuthorize(typeof(EquipAlarmRecord));

            View.UseCommands(WebCommandNames.Add, WebCommandNames.Save, typeof(AlarmStateCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, "SIE.Web.EMS.Equipments.AlarmStates.Commands.OpenAlarmCountCommand", "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.AddEquipAlarmRecordRepairCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.EquipAccountId).UsePagingLookUpEditor((m, r) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(r.EquipAccountName), nameof(r.EquipAccount.Name));
                    keyValues.Add(nameof(r.EquipModelCode), nameof(r.EquipAccount.EquipModel.Code));
                    keyValues.Add(nameof(r.EquipModelName), nameof(r.EquipAccount.EquipModel.Name));

                    m.DicLinkField = keyValues;
                }).HasLabel("设备编码");
                View.Property(p => p.EquipAccountName);
                View.Property(p => p.EquipModelCode);
                View.Property(p => p.EquipModelName);
                View.Property(p => p.AlarmLevel);
                View.Property(p => p.AlarmContent);
                View.Property(p => p.AlarmType);
                View.Property(p => p.AlarmState).UseEnumEditor(w => w.ColumnXType = "setAlarmStateStyle_comboboxcolumn");
                View.Property(p => p.AlarmTime);
                View.Property(p => p.CloseTime);
                View.Property(p => p.Duration);
                View.Property(p => p.MdcUid);
                View.Property(p => p.TagName);
                View.Property(p => p.LinkTagFullName);
                View.Property(p => p.EquipRepairBillId).ShowInList( width:150).HasLabel("维修单号");
                View.Property(p => p.RepairState).HasLabel("维修状态");
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);

                View.ChildrenProperty(p => p.AlarmRecordValueList).Show(ChildShowInWhere.Hide);
                View.AttachChildrenProperty(typeof(AlarmRecordValueViewModel), (e) =>
                    {
                        var args = e as ChildPagingDataArgs;
                        var parent = args.Parent as EquipAlarmRecord;
                        if (parent == null)
                        {
                            return new EntityList<AlarmRecordValueViewModel>();
                        }

                        var roleUser = RT.Service.Resolve<AlarmController>().GetAlarmRecordValueViewModels(parent.Id, args.SortInfo, args.PagingInfo);
                        return roleUser;
                    }).OrderNo = 1;
            }
        }
    }
}