using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.Web.Command;
using System;
using System.Text;

namespace SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Commands
{
    /// <summary>
    /// 保存
    /// </summary>
    public class MeterEquipAccountLubricationProjectSaveCommand : SaveCommand
    {

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            if (data == null)
            {
                throw new ValidationException("没有数据可以保存。".L10N());
            }

            StringBuilder sb;

            foreach (MeteringEquipAccountLubricationProject lub in data)
            {
                sb = new StringBuilder();
                if (lub.ProjectDetailId == 0)
                {
                    sb.Append("润滑项目不能为空".L10N());
                }
                if (!lub.ProjectCycle.HasValue)
                {
                    IsAppend(sb);
                    sb.Append("周期不能为空".L10N());
                }
                if (!lub.WarningPeriod.HasValue)
                {
                    IsAppend(sb);
                    sb.Append("预警期不能为空".L10N());
                }
                if (lub.LubricatingType == null)
                {
                    IsAppend(sb);
                    sb.Append("润滑方式不能为空".L10N());
                }
                if (!lub.DepartmentId.HasValue)
                {
                    IsAppend(sb);
                    sb.Append("责任部门不能为空".L10N());
                }
                if (!lub.LastDate.HasValue)
                {
                    IsAppend(sb);
                    sb.Append("上次润滑日期不能为空".L10N());
                }
                if (!lub.NextDate.HasValue)
                {
                    IsAppend(sb);
                    sb.Append("下次润滑日期不能为空".L10N());
                }
                if (!lub.MinValue.HasValue)
                {
                    IsAppend(sb);
                    sb.Append("加油量下限不能为空".L10N());
                }
                if (!lub.MaxValue.HasValue)
                {
                    IsAppend(sb);
                    sb.Append("加油量上限不能为空".L10N());
                }
                if ((lub.MinValue.HasValue && lub.MaxValue.HasValue) && lub.MinValue.Value > lub.MaxValue.Value)
                {
                    IsAppend(sb);
                    sb.Append("加油量下限大于加油量上限".L10N());
                }
                if (sb.Length > 0)
                {
                    //sb.Insert(0, string.Format("设备台账【{0}】", lub.EquipAccount.Code));
                    throw new ValidationException("设备台账".L10N()+string.Format("【{0}】", lub.EquipAccount.Code)+sb.ToString());

                    throw new ValidationException("设备台账【{0}】".L10nFormat(lub.EquipAccount.Code) + sb.ToString());
                }
                if (lub.IsNew && lub.Id > 0)
                {
                    lub.NotSubmit = false;
                }
            }

            base.DoSave(data);
        }

        /// <summary>
        /// 是否追加分隔符
        /// </summary>
        /// <param name="sb"></param>
        public void IsAppend(StringBuilder sb)
        {
            if (sb.Length > 0)
            {
                sb.Append(",");
            }
        }
    }
}
