using System;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.Equipments.AlarmStates;
using SIE.Web.Command;

namespace SIE.Web.EMS.Equipments.AlarmStates.Commands
{
    /// <summary>
    /// 关闭
    /// </summary>
    [JsCommand("SIE.Web.EMS.Equipments.AlarmStates.Commands.AlarmStateCommand")]
    public class AlarmStateCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var alarmDetail = entity as EquipAlarmRecord;
            alarmDetail.AlarmState =AlarmState.Close;
            alarmDetail.CloseTime = DateTime.Now;
            alarmDetail.Duration=RT.Service.Resolve<AlarmController>().CalculateDate(alarmDetail.AlarmTime, Convert.ToDateTime(alarmDetail.CloseTime));
            RF.Save(alarmDetail);
        }
    }
}
