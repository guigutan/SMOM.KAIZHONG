using Newtonsoft.Json;
using SIE.Domain.Validation;
using SIE.Logging;
using SIE.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.RedisUtil
{
    /// <summary>
    /// Redis工具类
    /// </summary>
    public static class RedisUtil
    {
        /// <summary>
        /// 检查Redis配置
        /// </summary>
        private static void CheckRedisConfig()
        {
            // 主动检查配置。因没有配置时，RT.Redis.Lock不会主动报错
            var redisConfigKey = "RedisConnectionStrings";
            var redisConfig = RT.Config.GetSection(redisConfigKey);
            if (redisConfig.Keys.Count == 0)
            {
                throw new ValidationException("未配置appsettings.json【{0}】节点，无法使用Redis锁".L10nFormat(redisConfigKey));
            }
        }

        /// <summary>
        /// 获取Redis键
        /// </summary>
        /// <param name="id">调用者身份</param>
        /// <param name="key">调用者身份下的Redis键对象（序列化后生成md5校验码）</param>
        /// <param name="prefix">前缀</param>
        /// <returns>Redis键</returns>
        private static string GetRedisKey(string id, object key, string prefix)
        {
            var keyStr = JsonConvert.SerializeObject(key);
            var md5 = CryptographyHelper.MD5(keyStr).Replace("-", "");
            var redisKey = $"proj_{prefix}_{id}_{md5}";
            return redisKey;
        }

        #region 缓存处理

        /// <summary>
        /// 获取Redis缓存键
        /// </summary>
        /// <param name="id">调用者身份</param>
        /// <param name="key">调用者身份下的Redis键对象（序列化后生成md5校验码）</param>
        /// <returns>Redis键</returns>
        private static string GetCacheKey(string id, object key)
        {
            return GetRedisKey(id, key, "cache");
        }

        /// <summary>
        /// 获取Redis缓存
        /// </summary>
        /// <typeparam name="TValue">缓存值类型</typeparam>
        /// <param name="id">调用者身份</param>
        /// <param name="key">调用者身份下的Redis键对象（放置能识别身份的数据实例）</param>
        /// <returns>Redis值</returns>
        public static TValue GetCache<TValue>(string id, object key)
        {
            var redisKey = GetCacheKey(id, key);
            RT.Redis.TryGet<TValue>(redisKey, out var redisValue);
            return redisValue;
        }

        /// <summary>
        /// 设置Redis缓存
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="id">调用者身份</param>
        /// <param name="key">调用者身份下的Redis键对象</param>
        /// <param name="value">Redis值对象</param>
        /// <param name="expireMinutes">过期分钟数</param>
        public static void SetCache<TValue>(string id, object key, TValue value, int expireMinutes = 1)
        {
            var redisKey = GetCacheKey(id, key);
            var expire = DateTime.Now.AddMinutes(expireMinutes);
            RT.Redis.Add(redisKey, value, expire);
        }

        #endregion

        #region 锁处理

        /// <summary>
        /// 获取Redis锁键
        /// </summary>
        /// <param name="id">调用者身份</param>
        /// <param name="key">调用者身份下的Redis键对象（序列化后生成md5校验码）</param>
        /// <returns>Redis键</returns>
        private static string GetLockKey(string id, object key)
        {
            return GetRedisKey(id, key, "lock");
        }

        /// <summary>
        /// 使用Redis进行防止并发的操作
        /// </summary>
        /// <param name="id">调用者身份</param>
        /// <param name="key">调用者身份下的Redis键对象（放置能识别数据身份的数据实例）</param>
        /// <param name="action">需要防止并发的行为</param>
        /// <param name="actionDesc">行为描述</param>
        /// <param name="lockSeconds">锁定秒数，默认300，5分钟</param>
        public static void LockToDo(string id, object key, string actionDesc, Action action, int lockSeconds = 300)
        {
            bool locked = false;        // 锁定是否成功
            string lockId = null;       // 锁定成功后，锁id
            var redisKey = GetLockKey(id, key);

            CheckRedisConfig();

            // 获取锁
            try
            {
                locked = RT.Redis.Lock(redisKey, out lockId, lockSeconds);
            }
            catch (Exception ex)
            {
                // 删除换行后内容。因密码错误时，换行内容中会返回配置的密码，风险较高。
                var error = ex.Message.Split('\n')[0].Trim('\r');
                throw new ValidationException("获取Redis锁异常：{0}".L10nFormat(error));
            }

            // 执行锁定后操作
            if (locked)
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception ex)
                {
                    throw new ValidationException(ex.Message);
                }
                finally
                {
                    RT.Redis.UnLock(redisKey, lockId);
                }
            }
            else
            {
                var error = "{0}产生并发，请稍后重试".L10nFormat(actionDesc?.L10N());
                LogManager.Logger.Error(error);
                throw new ValidationException(error);
            }
        }

        /// <summary>
        /// 使用Redis进行防止并发的批量操作
        /// </summary>
        /// <param name="id">调用者身份</param>
        /// <param name="keys">调用者身份下的Redis键对象集合（放置能识别数据身份的数据实例）</param>
        /// <param name="action">需要防止并发的行为</param>
        /// <param name="actionDesc">行为描述</param>
        /// <param name="lockSeconds">锁定秒数，默认600，10分钟</param>
        public static void LockBatchToDo<T>(string id, IList<T> keys, string actionDesc, Action action, int lockSeconds = 600)
        {
            List<string> redisKeys = new List<string>();    // 锁定成功的Redis键集合
            List<string> lockIds = new List<string>();      // 锁定成功的Redis值集合
            var now = DateTime.Now;                         // 当前时间
            var deadline = now.AddSeconds(lockSeconds);     // 锁定过期期限

            CheckRedisConfig();

            try
            {
                // 获取锁
                try
                {
                    foreach (var key in keys)
                    {
                        var redisKey = GetLockKey(id, key);
                        var keyLockSeconds = (int)((deadline - now).TotalSeconds);
                        var redisKeyLocked = RT.Redis.Lock(redisKey, out string lockId, keyLockSeconds);
                        if (!redisKeyLocked)
                        {
                            // 锁定不成功时立即中断
                            break;
                        }
                        redisKeys.Add(redisKey);
                        lockIds.Add(lockId);
                    }
                }
                catch (Exception ex)
                {
                    // 删除换行后内容。因密码错误时，换行内容中会返回配置的密码，风险较高。
                    var error = ex.Message.Split('\n')[0].Trim('\r');
                    throw new ValidationException("获取Redis锁异常：{0}".L10nFormat(error));
                }

                // 执行锁定后操作
                if (redisKeys.Count == keys.Count)
                {
                    action?.Invoke();
                }
                else
                {
                    var error = "{0}产生并发，请稍后重试".L10nFormat(actionDesc?.L10N());
                    LogManager.Logger.Error(error);
                    throw new ValidationException(error);
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
            finally
            {
                // 释放成功的锁
                for (int i = 0; i < redisKeys.Count; i++)
                {
                    try
                    {
                        RT.Redis.UnLock(redisKeys[i], lockIds[i]);
                    }
                    catch { }
                }
            }
        }

        #endregion
    }
}
