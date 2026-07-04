using SIE.Domain;

namespace SIE.WorkBenchCommon.Workbench.Concerns
{
    /// <summary>
    /// 关注控制器
    /// </summary>
    public class ConcernsController : DomainController
    {
        /// <summary>
        /// 我的关注
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<ConcernsInfo> GetMyConcerns()
        {
            return Query<ConcernsInfo>().Where(p => p.CreateBy == RT.IdentityId).ToList();
        }

        /// <summary>
        /// 是否关注
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool IsConcerns(string name)
        {
            return Query<ConcernsInfo>().Where(p => p.CreateBy == RT.IdentityId && p.Name == name).Count() > 0;
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="name"></param>
        public virtual void CancelConcerns(string name)
        {
            var concerns = Query<ConcernsInfo>().Where(p => p.CreateBy == RT.IdentityId && p.Name == name).FirstOrDefault();
            if(concerns != null)
            {
                concerns.PersistenceStatus = PersistenceStatus.Deleted;
                RF.Save(concerns);
            }
        }

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="name"></param>
        public virtual void Concerns(string name)
        {
            var concerns = new ConcernsInfo();
            concerns.Name = name;
            RF.Save(concerns);
        }
    }
}
