using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.DeviceControls.Configs;
using System;

namespace SIE.Equipments.DeviceControls
{
    /// <summary>
    /// 物料控制器
    /// </summary>
    public partial class DeviceControlController : DomainController
    {
        #region 设备控制记录 
        /// <summary>
        /// 查询设备控制记录列表
        /// </summary>
        /// <param name="criteria">设备控制记录查询实体</param>
        /// <returns>设备控制记录列表</returns>
        public virtual EntityList<DeviceControl> GetDeviceControls(DeviceControlCriteria criteria)
        {
            var query = Query<DeviceControl>();
            if (criteria.EquipAccountId.HasValue)
                query.Where(p => p.EquipAccountId == criteria.EquipAccountId);
            if (criteria.SourceControlId.HasValue)
                query.Where(p => p.Source == criteria.SourceControl.Source);
            if (criteria.IsStop != null)
                query.Where(p => p.IsStop == criteria.IsStop);
            if (criteria.IsEffective != null)
                query.Where(p => p.IsEffective == criteria.IsEffective);
            if (criteria.OpearDateTime.BeginValue.HasValue)
                query.Where(p => p.OpearDateTime >= criteria.OpearDateTime.BeginValue);
            if (criteria.OpearDateTime.EndValue.HasValue)
                query.Where(p => p.OpearDateTime <= criteria.OpearDateTime.EndValue);

            return query.OrderBy(criteria.OrderInfoList)
                .ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        /// <summary>
        /// 获取设备控制来源清单
        /// </summary>
        /// <param name="source">来源 可空,空时获取全部</param>
        /// <returns>控制来源清单列表</returns>
        public virtual EntityList<SourceControl> GetSourceControls(string source)
        {
            var q = Query<SourceControl>();
            if (!source.IsNullOrEmpty())
                q.Where(p => p.Source == source);

            return q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过来源获取来源的最后开停机记录
        /// </summary>
        /// <param name="source">来源 不可空</param>
        /// <returns>设备控制记录 可空</returns>
        public virtual DeviceControl GetDeviceControlBySource(string source)
        {
            return Query<DeviceControl>().Where(p => p.Source == source).OrderByDescending(p => p.CreateDate).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 验证来源是否存在，不存在则新增
        /// </summary>
        /// <param name="source">来源 不可空</param>
        public virtual void ValidationSource(string source)
        {
            if (source.IsNullOrEmpty())
            {
                throw new ValidationException("来源编码不能为空！".L10N());
            }
            var sourceControl = Query<SourceControl>().Where(p => p.Source == source).FirstOrDefault();

            if (sourceControl == null)
            {
                sourceControl = new SourceControl();
                sourceControl.Source = source;
                RF.Save(sourceControl);
            }
        }

        /// <summary>
        /// 保存控制记录
        /// </summary>
        /// <param name="deviceControl">控制记录 不可空</param>
        public virtual void SaveDeviceControl(DeviceControl deviceControl)
        {
            RF.Save(deviceControl);
        }

        /// <summary>
        /// 获取SMDC的Api请求地址
        /// </summary>
        /// <param name="methodName">Api地址后面的全路径名称</param>
        /// <returns></returns>
        public virtual  string GetSmdcApiUrl(string methodName)
        {
            // 从全局配置项中取数据源配置
            var config = ConfigService.GetConfig(new SmdcApiConfig());

            if (config == null || string.IsNullOrWhiteSpace(config.ApiUrl))
            {
                throw new ValidationException("请配置SMDC接口的Api访问地址.".L10N());
            }

            string url = config.ApiUrl;
            if (!url.EndsWith("/"))
            {
                url += "/";
            }

            if (methodName.StartsWith("/"))
            {
                methodName = methodName.Substring(1);
            }

            url += methodName;

            return url;
        }

    }

}