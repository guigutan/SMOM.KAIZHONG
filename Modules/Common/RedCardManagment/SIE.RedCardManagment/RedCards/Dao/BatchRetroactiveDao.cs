using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Linq;

namespace SIE.RedCardManagment.RedCards.Dao
{
	/// <summary>
	/// 异常判定规则 AbnormalDecisionRuleDao DAO
	/// </summary>
	public class BatchRetroactiveDao : BaseDao<BatchRetroactive>
    {

        /// <summary>
        /// 获取监控实体的，实体仓储
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public EntityList<BatchRetroactive> GetList(double redCardId)
        {
            return DB.Query<BatchRetroactive>().Where(c=>c.RedCardId== redCardId).ToList();
        }

        /// <summary>
        /// 获取监控实体的，实体仓储
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public EntityList<BatchRetroactive> GetList(double redCardId, PagingInfo pagingInfo = null)
        {
            return DB.Query<BatchRetroactive>().Where(c => c.RedCardId == redCardId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据红牌ID获取物料批次表数据
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public EntityList<BatchRetroactive> GetBatchRetroactiveInventory(double redCardId)
        {
            return DB.Query<BatchRetroactive>().Where(c => c.RedCardId == redCardId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 修改红牌状态
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public bool SetBatchRedCardStatus(double batchId, RedCardState state)
        {
            var result = DB.Update<BatchRetroactive>()
                .Set(c => c.Status, state)
                .Set(c => c.ApplicantId, RT.IdentityId)
                .Set(c => c.ApplyTime, DateTime.Now)
                .Where(c => c.Id == batchId).Execute();
            return result > 0;
        }

        /// <summary>
        /// 根据红牌ID修改所有红牌状态
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public void SetAllBatchRedCardStatus(double redCardId, RedCardState state)
        {
            var result = DB.Query<BatchRetroactive>().Where(c => c.RedCardId == redCardId).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Status = state;
                result[i].ApplicantId = RT.IdentityId;
                result[i].ApplyTime = DateTime.Now;
            }
            RF.Save(result);
        }

    }
}
