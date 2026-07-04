using NPOI.POIFS.Crypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.RedisUtil
{
    public class RedisUtilController : DomainController
    {
        /// <summary>
        /// 主要是针对wpf前端
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireMinutes"></param>
        public virtual void RedisUtilSet(string key, string value, int expireMinutes = 1)
        {
            RedisUtil.SetCache(key, key, value, expireMinutes);
        }

        /// <summary>
        /// 主要是针对wpf前端
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireMinutes"></param>
        public virtual string RedisUtilGet(string key)
        {
            var result = RedisUtil.GetCache<string>(key, key);
            return result;
        }

        public virtual (bool,string) Lock(string redisKey,int expireSeconds = 1800)
        {
            string lockId = null;
            var locked = RT.Redis.Lock(redisKey, out lockId, expireSeconds);
            if (!locked)
                return (false, lockId);
            return (true, lockId);
        }

        public virtual bool UnLock(string redisKey, string lockId)
        {
            return RT.Redis.UnLock(redisKey, lockId);
        }
    }
}
