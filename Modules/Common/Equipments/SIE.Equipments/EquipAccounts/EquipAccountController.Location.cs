using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccountLocations;
using SIE.Equipments.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.Equipments.EquipAccounts
{
    /// <summary>
    /// 设备台账位置管理控制器
    /// </summary>
    public partial class EquipAccountController : DomainController
    {
        /// <summary>
        /// 获取设备型号位置列表
        /// </summary>
        /// <param name="Id">设备台账ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="orderInfos">排序信息</param>
        /// <returns>位置列表</returns>
        public virtual EntityList<EquipAccountLocation> GetEquipAccountLocations(double Id, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            return Query<EquipAccountLocation>()
                .Where(p => p.EquipAccountId == Id)
                .OrderBy(orderInfos)
                .ToList(pagingInfo);
        }

        /// <summary>
        /// 根据设备台账获取位置列表
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <returns>位置列表</returns>
        public virtual EntityList<EquipAccountLocation> GetEquipLocationsByAccountId(double equipId, PagingInfo pagingInfo)
        {
            return Query<EquipAccountLocation>().Where(p => p.EquipAccountId == equipId).ToList(pagingInfo);
        }

        /// <summary>
        /// 获取是否立卡
        /// </summary>
        /// <returns></returns>
        public virtual bool GetUseCard()
        {
            var config = ConfigService.GetConfig(new EquipAccountAssetConfig(), typeof(EquipAccount));
            if (config == null)
                throw new ValidationException("未找到固定资产相关配置项,请检查配置".L10N());
            return config.UseCard;
        }

        /// <summary>
        /// 根据设备台账、分区获取位置列表
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <param name="subarea">分区</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <returns>位置列表</returns>
        public virtual EntityList<EquipAccountLocation> GetEquipLocationsByAccountIdSubarea(double equipId, string subarea, PagingInfo pagingInfo)
        {
            return Query<EquipAccountLocation>().Where(p => p.EquipAccountId == equipId && p.Subarea == subarea).ToList(pagingInfo);
        }

        /// <summary>
        /// 根据设备台账、分区、站位获取位置
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <param name="subarea">分区</param>
        /// <param name="stance">站位</param>
        /// <returns>位置</returns>
        public virtual EquipAccountLocation GetEquipLocation(double equipId, string subarea, string stance)
        {
            return Query<EquipAccountLocation>().Where(p => p.EquipAccountId == equipId && p.Subarea == subarea && p.Stance == stance).FirstOrDefault();
        }


        /// <summary>
        ///  根据获取位置列表
        /// </summary>
        /// <param name="exp">条件</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>位置列表</returns>
        public virtual EntityList<EquipAccountLocation> GetEquipLocations(Expression<Func<EquipAccountLocation, bool>> exp, PagingInfo pagingInfo)
        {
            var query = Query<EquipAccountLocation>();
            if (exp != null)
                query.Where(exp);
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取设备位置分区列表
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <returns>分区列表</returns>
        public virtual IList<string> GetEquipStance(double equipId)
        {
            return Query<EquipAccountLocation>()
                .Where(p => p.EquipAccountId == equipId)
                .Select(p => p.Subarea).Distinct().ToList<string>();
        }

        /// <summary>
        /// 获取设备位置站位列表
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <returns>站位列表</returns>
        public virtual IList<string> GetEquipSubarea(double equipId)
        {
            return Query<EquipAccountLocation>()
                .Where(p => p.EquipAccountId == equipId)
                .Select(p => p.Stance).Distinct().ToList<string>();
        }

        /// <summary>
        /// 获取设备位置列表
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <returns>位置列表</returns>
        public virtual IList<EquipLocationInfo> GetLocationInfos(double equipId)
        {
            return Query<EquipAccountLocation>()
                .Where(p => p.EquipAccountId == equipId)
                .Select(p => new
                {
                    Subarea = p.Subarea,
                    Stance = p.Stance
                }).ToList<EquipLocationInfo>();
        }
        /// <summary>
        /// 获取站位类型Tray为设备Id集合
        /// </summary>
        /// <returns>位置列表</returns>
        public virtual IList<double> GetEquipIds()
        {
            var query = Query<EquipAccountLocation>().Where(p => p.StanceType == StanceType.Tray).ToList();
            return query.Select(w => w.EquipAccountId).Distinct().ToList();
        }

        /// <summary>
        /// 查询设备位置信息
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <param name="subarea">分区</param>
        /// <param name="stance">站位</param>
        /// <returns></returns>
        public virtual EquipAccountLocation GetEquipAccountLocation(double equipId, string subarea, string stance)
        {
            var query = Query<EquipAccountLocation>()
                .Where(x => x.EquipAccountId == equipId && x.Subarea == subarea && x.Stance == stance);

            return query.FirstOrDefault();
        }
    }
}
