using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Packages.QrCodeParseRules
{
    /// <summary>
    /// 二维码解析规则控制器
    /// </summary>
    public partial class QrCodeParseRuleController : DomainController
    {
        /// <summary>
        /// 获取二维码解析规则数据
        /// </summary>
        /// <param name="qrCodeIds">二维码解析规则ID集合</param>
        /// <returns>二维码解析规则数据</returns>
        public virtual EntityList<QrCodeParseRule> GetQrCodeParseRules(List<double> qrCodeIds)
        {
            if (qrCodeIds == null || qrCodeIds.Count == 0)
            {
                return new EntityList<QrCodeParseRule>();
            }
            return qrCodeIds.SplitContains(sons =>
            {
                return Query<QrCodeParseRule>().Where(p=>sons.Contains(p.Id)).ToList();
            });
        }

        /// <summary>
        /// 获取非当前ID集合二维码数据列表
        /// </summary>
        /// <returns>二维码数据列表</returns>
        public virtual EntityList<QrCodeParseRule> GetNotContainsIdQrCodeParseRules(List<double> qrCodeIds)
        {
            return Query<QrCodeParseRule>().Where(p => !qrCodeIds.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 获取启用的二维码解析规则
        /// </summary>
        /// <returns>二维码解析规则</returns>
        public virtual EntityList<QrCodeParseRule> GetEnabelQrCodeParseRuleData()
        {
            return Query<QrCodeParseRule>().Where(p => p.State == State.Enable).ToList();
        }

        /// <summary>
        /// 获取二维码解析规则明细
        /// </summary>
        /// <param name="qrCodeRuleId">二维码解析规则规则Id</param>
        /// <returns>二维码解析规则明细</returns>
        public virtual EntityList<QrCodeParseRuleDetail> GetQrCodeParseRuleDetails(double qrCodeRuleId)
        {
            return Query<QrCodeParseRuleDetail>().Where(p => p.QrCodeParseRuleId == qrCodeRuleId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 验证二维码解析规则
        /// </summary>
        /// <param name="qrCodeRule">二维码解析规则</param>
        public virtual void ValidQrCodeRule(QrCodeParseRule qrCodeRule)
        {
            // 1.截取方式为“分隔符“，分隔符类型不能为空
            if (qrCodeRule.InterceptWay == InterceptWay.Separator && qrCodeRule.SeparatorType.IsNullOrEmpty())
                throw new ValidationException("二维码解析规则:[{0}]截取方式为分隔符时,分隔符类型不能为空".L10nFormat(qrCodeRule.Code));

            if (qrCodeRule.InterceptWay == InterceptWay.InterceptDigit)
            {
                int tempEnd = 0;
                qrCodeRule.QrCodeParseRuleDetailList.OrderBy(q => int.Parse(q.LineNo)).ForEach(p =>
                 {
                     if (!p.InterceptStart.HasValue || !p.InterceptEnd.HasValue)
                         throw new ValidationException("二维码解析规则:[{0}]截取方式为截取位时,明细行:[{1}]的截取开始位、截取结束位必须有值".L10nFormat(qrCodeRule.Code, p.LineNo));

                     if (p.InterceptStart.HasValue && p.InterceptStart.Value <= 0)
                         throw new ValidationException("二维码解析规则:[{0}]截取方式为截取位时,明细行:[{1}]的截取开始位不能小于等于0".L10nFormat(qrCodeRule.Code, p.LineNo));

                     if (p.InterceptEnd.HasValue && p.InterceptEnd.Value <= 0)
                         throw new ValidationException("二维码解析规则:[{0}]截取方式为截取位时,明细行:[{1}]的截取结束位不能小于等于0".L10nFormat(qrCodeRule.Code, p.LineNo));

                     if (p.InterceptStart.HasValue && p.InterceptEnd.HasValue && p.InterceptStart > p.InterceptEnd)
                         throw new ValidationException("二维码解析规则:[{0}]截取方式为截取位时,明细行:[{1}]的截取开始位大于截取结束位".L10nFormat(qrCodeRule.Code, p.LineNo));

                     if (p.InterceptStart.HasValue && p.InterceptStart <= tempEnd)
                         throw new ValidationException("二维码解析规则:[{0}]截取方式为截取位时,明细行:[{1}]的截取开始位必须大于上一行的截取结束位".L10nFormat(qrCodeRule.Code, p.LineNo));

                     tempEnd = p.InterceptEnd ?? 0;
                 });
            }
        }

        /// <summary>
        /// 启用二维码解析规则
        /// </summary>
        /// <param name="qrCodeIds">二维码解析规则ID集合</param>
        public virtual void EnableQrCodeParseRuleData(List<double> qrCodeIds)
        {
            EntityList<QrCodeParseRule> qrCodeRuleList = GetQrCodeParseRules(qrCodeIds);

            if (qrCodeRuleList.Any(p => p.QrCodeParseRuleDetailList.Count == 0))
                throw new ValidationException("二维码解析规则:[{0}]缺少规则明细，无法启用!".L10nFormat(qrCodeRuleList.Select(p => p.Code).FirstOrDefault()));

            if (qrCodeRuleList.Any(p => p.State == State.Enable))
                throw new ValidationException("二维码解析规则:[{0}]已启用，无法再启用!".L10nFormat(qrCodeRuleList.Select(p => p.Code).FirstOrDefault()));

            using (var tran = DB.TransactionScope(PackageEntityDataProvider.ConnectionStringName))
            {
                //当前行变成启用
                qrCodeRuleList.ForEach(p => p.State = State.Enable);
                RF.Save(qrCodeRuleList);

                //其他数据变成禁用
                EntityList<QrCodeParseRule> notContainsQrCodeRuleList = GetNotContainsIdQrCodeParseRules(qrCodeIds);
                notContainsQrCodeRuleList.ForEach(p => p.State = State.Disable);
                RF.Save(notContainsQrCodeRuleList);

                tran.Complete();
            }
        }
    }
}
