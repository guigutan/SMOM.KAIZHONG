using SIE.Common.Catalogs;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Equipments.Configs;
using SIE.Equipments.EquipTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Equipments.EquipModels
{
    /// <summary>
    /// 设备控制器
    /// </summary>
    public partial class EquipModelController : CoreEquipController
    {
        /// <summary>
        /// 根据设备型号编码和设备类型编码获取设备型号数据
        /// </summary>
        /// <param name="equipModelCode">设备型号编码</param>
        /// <param name="EquipTypeCode">设备类型编码</param>
        /// <returns></returns>
        public virtual EquipModel GetEquipModel(string equipModelCode, string EquipTypeCode)
        {
            return Query<EquipModel>()
                .Exists<EquipType>((a, b) => b.Where(c => c.Id == a.EquipTypeId
                    && a.Code == equipModelCode && c.TypeCode == EquipTypeCode))
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据设备型号Id列表获取设备型号列表
        /// </summary>
        /// <param name="modelIds">设备型号Id列表</param>
        /// <returns>设备型号列表</returns>
        public virtual EntityList<EquipModel> GetEquipModels(List<double> modelIds)
        {
            return Query<EquipModel>().Where(p => modelIds.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 根据设备型号编码列表获取设备型号列表
        /// </summary>
        /// <param name="codes">设备型号编码列表</param>
        /// <returns>设备型号列表</returns>
        public virtual EntityList<EquipModel> GetEquipModels(List<string> codes)
        {
            var result = codes.SplitContains(temps =>
            {
                return Query<EquipModel>().Where(p => temps.Contains(p.Code)).ToList();
            });
            return result;
        }

        /// <summary>
        /// 根据设备型号编码列表获取设备型号列表
        /// </summary>
        /// <param name="codes">设备型号编码列表</param>
        /// <returns>设备型号列表</returns>
        public virtual List<string> GetEquipModelCodes(List<string> codes)
        {
            List<string> modelCodes = new List<string>();
            codes.SplitDataExecute(temps =>
            {
                modelCodes.AddRange(Query<EquipModel>().Where(p => temps.Contains(p.Code)).Select(p => new {p.Code}).ToList<string>());
            });
            return modelCodes;
        }

        /// <summary>
        /// 获取设备型号列表
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<EquipModel> GetEquipModels(PagingInfo paging, string keyword)
        {
            var query = Query<EquipModel>();
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(paging, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 根据设备编码获取设备型号
        /// </summary>
        /// <param name="id">设备Id</param>
        /// <param name="code">设备编码</param>
        /// <returns></returns>
        public virtual EntityList<EquipModel> GetEquipModelByCode(double id, string code)
        {
            return Query<EquipModel>().Where(p => p.Id != id && p.Code == code).ToList();
        }

        /// <summary>
        /// 根据编码获取设备型号
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual EquipModel GetEquipModelByCode(string code)
        {
            return Query<EquipModel>().Where(p => p.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 查询设备型号列表
        /// </summary>
        /// <param name="criteria">设备型号查询对象</param>
        /// <returns>设备型号列表</returns>
        public virtual EntityList<EquipModel> GetEquipModelsByCriteria(EquipModelCriteria criteria)
        {
            var query = Query<EquipModel>();
            if (criteria.Code.IsNotEmpty())
                query.Where(w => w.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                query.Where(w => w.Name.Contains(criteria.Name));
            if (criteria.TypeCategory.IsNotEmpty())
                query.Where(p => p.TypeCategory == criteria.TypeCategory);
            if (criteria.EquipTypeId != 0)
                query.Where(w => w.EquipTypeId == criteria.EquipTypeId);
            if (criteria.CreateDate.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            if (criteria.CreateDate.EndValue.HasValue)
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);

            //匹配设备型号查询扩展条件
            MacthEquipModelCrieriaCondition(query, criteria);

            return query.OrderBy(criteria.OrderInfoList)
                .ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 匹配设备型号查询扩展条件
        /// </summary>
        /// <param name="query">实体查询器</param>
        /// <param name="criteria">设备模块查询实体</param>
        protected virtual void MacthEquipModelCrieriaCondition(IEntityQueryer<EquipModel> query, EquipModelCriteria criteria)
        {
        }

        /// <summary>
        /// 通过设备型号Id列表获取位置列表
        /// </summary>
        /// <param name="modelIds">设备型号Id列表</param>
        /// <returns>位置列表</returns>
        public virtual EntityList<EquipModelLocation> GetLocationsOfModels(List<double> modelIds)
        {
            return Query<EquipModelLocation>().Where(p => modelIds.Contains(p.EquipModelId)).ToList();
        }

        /// <summary>
        /// 通过设备型号Id获取型号
        /// </summary>
        /// <param name="Id">设备型号Id</param>
        /// <returns>型号</returns>
        public virtual EquipModel GetEquipModelsOfModel(double Id)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(EquipModel.EquipTypeProperty);
            elo.LoadWithViewProperty();
            return Query<EquipModel>().Where(p => p.Id == Id).FirstOrDefault(elo);
        }

        /// <summary>
        /// 获取设备型号的设备类型信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>

        public virtual Tuple<bool, bool> GetEquipmentTypeInfo(string code)
        {
            var config = ConfigService.GetConfig(new EquipModelEquipmentCategoryConfig(), typeof(EquipModel));
            var special = false;
            var metering = false;
            if (code.IsNullOrEmpty())
                return new Tuple<bool, bool>(metering, special);
            var catalogType = RT.Service.Resolve<CatalogController>().GetCatalog(EquipType.EquipTypeCatalogType, code);
            if (catalogType != null && !config.SpecialIds.IsNullOrEmpty())
            {
                var specialIds = config.SpecialIds.Split(',').ToList<string>();
                special = specialIds.Contains(catalogType.Id.ToString());
            }
            if (catalogType != null && !config.EquipmentMeteringIds.IsNullOrEmpty())
            {
                var meterings = config.EquipmentMeteringIds.Split(',').ToList<string>();
                metering = meterings.Contains(catalogType.Id.ToString());
            }
            return new Tuple<bool, bool>(metering, special);
        }

        /// <summary>
        /// 根据设备类型编码和设备类型名称获取设备类型
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<EquipType> GetEquipTypesByKeyword(PagingInfo paging,string keyword)
        {
            var q = Query<EquipType>();
            if (keyword.IsNotEmpty()) { 
                q.Where(p => p.TypeCode.Contains(keyword) || p.TypeName.Contains(keyword));
            }
            return q.ToList(paging);
        }

        /// <summary>
        /// 获取设备型号所有编码
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GetAllCode() {
            return Query<EquipModel>().Select(c=>c.Code).ToList<string>().ToList();
        }
    }
}
