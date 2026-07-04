using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.DeviceIOTParas.Criterias;
using SIE.Equipments.DeviceIOTParas.Details;
using SIE.Equipments.DeviceIOTParas.ViewModles;
using SIE.SMDC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Equipments.DeviceIOTParas.Controllers
{
    /// <summary>
    /// 设备物联参数控制器
    /// </summary>
    public class DeviceIOTParaController : DomainController
    {
        /// <summary>
        /// 根据查询条件获取实体
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<DeviceIOTPara> SelectByCriteria(DeviceIOTParaCirteria criteria)
        {
            var query = DB.Query<DeviceIOTPara>();

            if (!criteria.Code.IsNullOrWhiteSpace())
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }
            if (criteria.EquipModelId != null && criteria.EquipModelId != 0)
            {
                query.Where(p => p.EquipModelId == criteria.EquipModelId);
            }
            if (criteria.TypeCategory.IsNotEmpty())
            {
                query.Where(p => p.DeviceType ==  criteria.TypeCategory);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate > criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate < criteria.CreateDate.EndValue);
            }
            var listDevice = query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return listDevice;
        }

        /// <summary>
        /// 根据查询条件获取实体
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<PhysicalUnion> SelectByCriteria(PhysicalUnionSelCriteria criteria)
        {
            var query = DB.Query<PhysicalUnion>();
            query.Where(p => p.Enable);
            if (criteria.EquipAccountId > 0)
                query.Exists<DeviceIOTPara>((x, y) => y.Join<FacilityDetail>((a, b) => b.EquipAccountId == criteria.EquipAccountId && a.Id == b.DeviceIOTParaId).Where(p => x.DeviceIOTParaId == p.Id));

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物联参数列表
        /// </summary>
        /// <param name="equipmentId">设备台账ID</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns>物联参数列表</returns>
        public virtual EntityList<PhysicalUnion> GetPhysicalUnions(double equipmentId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var query = Query<PhysicalUnion>();
            query.Exists<DeviceIOTPara>((x, y) => y.Join<FacilityDetail>((a, b) => b.EquipAccountId == equipmentId && a.Id == b.DeviceIOTParaId).Where(p => x.DeviceIOTParaId == p.Id));

            return query.OrderBy(orderInfoList)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取MDC接口
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<MDCDetailViewModle> GetMDCDetail(MDCDetailViewModleCriteria criteria)
        {
            try
            {
                EntityList<MDCDetailViewModle> list = RT.Service.Resolve<EquipmentSmdcController>()
                    .GetMDCDetailByEquipModel(criteria);
                return list;
            }
            catch (Exception)
            {
                throw new ValidationException("接口异常：从MDC接口中查找明细数据失败!".L10N());
            }
        }

        /// <summary>
        /// 是否已经存在台账
        /// </summary>
        public virtual void ExistEquipAccountByEAIdAsFD(IEnumerable<double> ids)
        {
            var fd = DB.Query<FacilityDetail>().Where(p => ids.Contains(p.EquipAccountId)).FirstOrDefault(
                new EagerLoadOptions().LoadWith(FacilityDetail.EquipAccountProperty));
            if (fd != null)
            {
                throw new ValidationException("已经存在设备台账编码是{0}的设备清单".L10nFormat(fd.EquipAccount.Code));
            }
        }
    }
}
