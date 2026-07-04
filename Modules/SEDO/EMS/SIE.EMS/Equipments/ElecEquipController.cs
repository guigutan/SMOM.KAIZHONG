using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Models;
using SIE.EMS.RunStandards;
using SIE.Equipments;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccountLocations;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.EMS.Equipments
{
    /// <summary>
    /// 设备控制器
    /// </summary>
    public partial class ElecEquipController : CoreEquipController
    {
        #region 设备台账
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
            {
                query.Where(exp);
            }
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
        /// 获取站位类型Tray为设备Id集合
        /// </summary>
        /// <returns>位置列表</returns>
        public virtual IList<double> GetEquipIds()
        {
            var query = Query<EquipAccountLocation>().Where(p => p.StanceType == StanceType.Tray).ToList();
            return query.Select(w => w.EquipAccountId).Distinct().ToList();
        }

        /// <summary>
        /// 根据Ids获取设备台账
        /// </summary>
        /// <param name="ids">设备台账Id集合</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetEquipAccountsByIds(List<double> ids)
        {
            return Query<EquipAccount>().Where(w => ids.Contains(w.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据Id获取设备台账
        /// </summary>
        /// <param name="id">设备台账Id</param>
        /// <returns></returns>
        public virtual EquipAccount GetEquipAccountById(double id)
        {
            return Query<EquipAccount>().Where(w => w.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
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

        /// <summary>
        /// 获取设备台账
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetEquipAccount(string keyword, PagingInfo pagingInfo)
        {
            var query = Query<EquipAccount>();
            if (!keyword.IsNullOrEmpty())
            {
                query = query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty().LoadWith(EquipAccount.EquipModelProperty));
            return list;
        }
        #endregion

        #region 设备型号
        /// <summary>
        /// 获取设备型号位置列表
        /// </summary>
        /// <param name="equipModelId">设备型号ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="orderInfos">排序信息</param>
        /// <returns>位置列表</returns>
        public virtual EntityList<EquipModelLocation> GetEquipModelLocations(double equipModelId, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            return Query<EquipModelLocation>()
                .Where(p => p.EquipModelId == equipModelId)
                .OrderBy(orderInfos)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据设备型号编码和设备类型编码获取设备型号数据
        /// </summary>
        /// <param name="equipModelCode">设备型号编码</param>
        /// <param name="EquipTypeCode">设备类型编码</param>
        /// <returns></returns>
        public virtual EquipModel GetEquipModel(string equipModelCode, string EquipTypeCode)
        {
            return Query<EquipModel>().Exists<EquipType>((a, b) => b.Where(c => c.Id == a.EquipTypeId && a.Code == equipModelCode && c.TypeCode == EquipTypeCode)).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据台帐ID获取设备型号数据
        /// </summary>
        /// <param name="equipId">台帐Id</param>
        /// <returns>设备型号</returns>
        public new virtual EquipModel GetEquipModelByEquipId(double equipId)
        {
            return Query<EquipModel>().Exists<EquipAccount>((a, b) => b.Where(c => c.EquipModelId == a.Id && c.Id == equipId)).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion


        #region 维修定标
        /// <summary>
        /// 获取维修定标Id集合
        /// </summary>
        /// <returns></returns>
        public virtual List<double> GetRunStandardValueIds(double equipmentId)
        {
            var result= Query<RunStandardValue>().Join<RunStandard>(
                  (x, y) => x.RunStandardId == y.Id).Join<RunStandard, RunStandardEquipment>((k, l) => k.Id == l.RunStandardId
                       && l.EquipAccountId == equipmentId).Distinct().ToList(null).Select(m => m.Id).ToList();

            return result;
        }

        #endregion
    }
}