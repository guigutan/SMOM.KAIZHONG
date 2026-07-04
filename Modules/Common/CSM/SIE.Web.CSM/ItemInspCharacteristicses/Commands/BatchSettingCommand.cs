using SIE.Domain;
using SIE.CSM.ItemInspCharacteristicses;
using SIE.Web.Command;
using System;

namespace SIE.Web.CSM.ItemInspCharacteristicses.Commands
{
    /// <summary>
    /// 批量设置命令
    /// </summary>
    public class BatchSettingCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 重写执行方法
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<BatchSettingViewArgs>();
            for (int i = 0; i < data.EntityIdList.Length; i++)
            {
                var entity = RF.GetById<ItemInspCharacteristics>(data.EntityIdList[i]);
                Check.AssertNotNull(entity, "找不到物料检验特性".L10N());
                entity.RecurringInspection = data.RecurringInspection;
                entity.ConfirmInspection = data.ConfirmInspection;
                if (data.RecurringInspection)
                {
                    entity.IntervalPeriod = data.IntervalPeriod;
                    if (data.PeriodType == "Day")
                        entity.PeriodType = PeriodType.Day;
                    if (data.PeriodType == "Batch")
                        entity.PeriodType = PeriodType.Batch;
                }

                entity.SkipBatches = null;
                entity.InspectDateBegin = null;
                RepositoryFactory.Save(entity);
            }
            return true;
        }
    }
    /// <summary>
    /// 批量设置参数类
    /// </summary>
    public class BatchSettingViewArgs
    {
        /// <summary>
        /// 实体ID
        /// </summary>
        public double[] EntityIdList { get; set; }
        /// <summary>
        /// 物料周期检
        /// </summary>
        public bool RecurringInspection { get; set; }
        /// <summary>
        /// 确认检
        /// </summary>
        public bool ConfirmInspection { get; set; }
        /// <summary>
        /// 间隔周期
        /// </summary>
        public int? IntervalPeriod { get; set; }
        /// <summary>
        /// 周期类型
        /// </summary>
        public string PeriodType { get; set; }

    }
}
