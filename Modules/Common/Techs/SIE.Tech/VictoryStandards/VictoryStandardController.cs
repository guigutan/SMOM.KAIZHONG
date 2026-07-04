using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SIE.Tech.VictoryStandards
{
    /// <summary>
    /// 胜制方案控制器
    /// </summary>
    public partial class VictoryStandardController : DomainController
    {
        /// <summary>
        /// 更新最大测试次数
        /// </summary>
        /// <param name="victoryStandardId">胜制方案id</param>
        public virtual void UpdateMaxTestQty(double victoryStandardId)
        {
            var victoryStandard = RF.GetById<VictoryStandard>(victoryStandardId);
            var standards = GetStandards(victoryStandardId);
            victoryStandard.MaxTestQty = !standards.Any() ? 1 : standards.Max(p => p.Length);
            RF.Save(victoryStandard);
        }

        /// <summary>
        /// 验证胜局标准
        /// </summary>
        /// <param name="victoryStandardId">胜制方案id</param>
        public virtual void ValidationStandard(double victoryStandardId)
        {
            var standards = GetStandards(victoryStandardId);
            standards = standards.OrderBy(p => p.Length).ToList();
            var passStandard = new List<string>();
            foreach (var standard in standards)
            {
                if (!Regex.IsMatch(standard, "^[0-1]*1$"))
                {
                    throw new ValidationException("胜局标准:必须为【0、1】组成的字符串,最后一位必须为：1".L10nFormat(standard));
                }

                for (var i = 1; i <= standard.Length; i++)
                {
                    var str = standard.Substring(0, i);
                    if (passStandard.Contains(str))
                    {
                        throw new ValidationException("胜局标准：【{0}】无效".L10nFormat(standard));
                    }
                }
                passStandard.Add(standard);
            }
        }

        /// <summary>
        /// 根据胜制方案id获取胜局标准
        /// </summary>
        /// <param name="victoryStandardId">胜制方案id</param>
        /// <returns>胜局标准</returns>
        public virtual IList<string> GetStandards(double victoryStandardId)
        {
            return Query<VictoryStandardDetail>().Where(p => p.VictoryStandardId == victoryStandardId).Select(p => p.Standard).ToList<string>();
        }

        /// <summary>
        /// 获取启用的胜制方案
        /// </summary>
        /// <returns>胜制方案列表</returns>
        public virtual EntityList<VictoryStandard> GetEnableVictoryStandards()
        {
            return Query<VictoryStandard>().Where(p => p.State == State.Enable).ToList(new PagingInfo() { PageNumber = 1, PageSize = int.MaxValue - 1 });
        }

        /// <summary>
        /// 获取启用的胜制方案 带分页
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<VictoryStandard> GetEnableVictoryStandards(PagingInfo pageInfo, string keyword)
        {
            var query = Query<VictoryStandard>().Where(p => p.State == State.Enable);
            if (!string.IsNullOrEmpty(keyword))
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(pageInfo);
        }




        /// <summary>
        /// 获取胜制标准
        /// </summary>
        /// <param name="victoryId">胜制方案ID</param>
        /// <returns>胜制标准列表</returns>
        public virtual IList<string> GetVictoryStandardDetails(double victoryId)
        {
            return Query<VictoryStandardDetail>().Where(p => p.VictoryStandardId == victoryId).Select(p => p.Standard).ToList<string>();
        }

        /// <summary>
        /// 匹配胜制
        /// </summary>
        /// <param name="victoryId">胜制方案ID</param>
        /// <param name="strProcessRecord">胜制过站记录</param>
        /// <returns>匹配结果</returns>
        public virtual MatchResult VictoryMatch(double victoryId, string strProcessRecord)
        {
            var standards = GetVictoryStandardDetails(victoryId);

            if (standards.Any(p => p == strProcessRecord))
            {
                return MatchResult.ExactMatch;
            }

            if (standards.Any(p => p.StartsWith(strProcessRecord)))
            {
                return MatchResult.PartialMatch;
            }

            return MatchResult.UnMatch;
        }
    }
}