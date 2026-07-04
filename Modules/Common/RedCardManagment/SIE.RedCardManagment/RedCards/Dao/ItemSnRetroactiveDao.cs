using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Linq;

namespace SIE.RedCardManagment.RedCards.Dao
{
	/// <summary>
	/// 异常判定规则 AbnormalDecisionRuleDao DAO
	/// </summary>
	public class ItemSnRetroactiveDao : BaseDao<ItemSnRetroactive>
    {

        /// <summary>
        /// 获取监控实体的，实体仓储
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public EntityList<ItemSnRetroactive> GetList(double redCardId, PagingInfo pagingInfo = null)
        {
            return DB.Query<ItemSnRetroactive>().Where(c=>c.RedCardId== redCardId).ToList(pagingInfo,new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据红牌ID获取物料SN表数据
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public EntityList<ItemSnRetroactive> GetItemSnInventory(double redCardId)
        {
            return  DB.Query<ItemSnRetroactive>().Where(c => c.RedCardId == redCardId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
        
        /// <summary>
        /// 修改红牌状态
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public bool SetItemSnRedCardStatus(double itemSnId, RedCardState state)
        {
            var result = DB.Update<ItemSnRetroactive>()
                .Set(c => c.Status, state)
                .Set(c => c.ApplicantId, RT.IdentityId)
                .Set(c => c.ApplyTime, DateTime.Now)
                .Where(c => c.Id == itemSnId).Execute();
            return result > 0;
        }
        
        
        /// <summary>
        /// 根据红牌ID修改所有红牌状态
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public void SetAllItemSnRedCardStatus(double redCardId, RedCardState state)
        {
            var result = DB.Query<ItemSnRetroactive>().Where(c => c.RedCardId == redCardId).ToList();
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
