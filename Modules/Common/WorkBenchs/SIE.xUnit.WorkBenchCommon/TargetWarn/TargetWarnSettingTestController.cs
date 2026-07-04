using SIE.Domain;
using SIE.WorkBenchCommon.Workbench.TargetWarn;

namespace SIE.xUnit.WorkBenchCommon.TargetWarn
{
    /// <summary>
    /// 目标测试数据控制器
    /// </summary>
    public class TargetWarnSettingTestController : DomainController
    {
        /// <summary>
        /// 获取或者创建目标
        /// </summary>
        /// <param name="rateCode">目标编码</param>
        /// <param name="rateName">目标名称</param>
        /// <returns></returns>
        public virtual TargetWarnSetting GetOrCreateTargetWarnSetting(string rateCode, string rateName)
        {
            var qTarget = Query<TargetWarnSetting>().Where(p => p.Name == rateName).FirstOrDefault();
            if (qTarget?.TargetWarnDetailList?.Count > 0)
                return qTarget;
            else
            {
                TargetWarnSetting set = new TargetWarnSetting()
                {
                    Code = rateCode,
                    Name = rateName
                };
                set.GenerateId();
                var detail1 = new TargetWarnDetail()
                {
                    TargetWarnSettingId = set.Id,
                    TargetColor = TargetColor.Green,
                    MaxValue = 90,
                    TargetOpetators = TargetOpetators.GreaterOrEqual
                };
                set.TargetWarnDetailList.Add(detail1);
                var detail2 = new TargetWarnDetail()
                {
                    TargetWarnSettingId = set.Id,
                    TargetColor = TargetColor.Yellow,
                    MaxValue = 90,
                    MinValue = 80,
                    TargetOpetators = TargetOpetators.Between
                };
                set.TargetWarnDetailList.Add(detail2);
                var detail3 = new TargetWarnDetail()
                {
                    TargetWarnSettingId = set.Id,
                    TargetColor = TargetColor.Red,
                    MinValue = 80,
                    TargetOpetators = TargetOpetators.LessOrEqual
                };
                set.TargetWarnDetailList.Add(detail3);

                RF.Save(set);
                return set;
            }
        }
    }
}
