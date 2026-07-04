using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using StorageLocationData = SIE.ERPInterface.Common.Datas.StorageLocationData;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 库位下载控制器
    /// </summary>
    public class DownloadStorageLocationController : DomainController
    {
        /// <summary>
        /// 从API下载库位到业务表
        /// </summary>
        /// <param name="storageLocDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadStorageLocToBusiness(List<StorageLocationData> storageLocDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<StorageLocationData>(
                storageLocDatas,
                p => this.SaveStorageLocation(p.OrderByLastUpdateDate()),
                JobType.StorageLocation,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载库位到业务表
        /// </summary>
        public virtual ProcessResult DownloadStorageLocInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<StorageLocationInf>(
                () => ctl.GetUnprocessedDatas<StorageLocationInf>(),                   //库位中间表数据
                p =>
                {
                    var paras = this.GenerateStorageLocationPara(p);
                    return this.SaveStorageLocation(paras.OrderByLastUpdateDate());
                },
                JobType.StorageLocation, isManual);
        }

        /// <summary>
        /// 生成库位实体
        /// </summary>
        /// <param name="storageLocationInfs">中间表实体数据</param>
        /// <returns></returns>
        private List<StorageLocationData> GenerateStorageLocationPara(IEnumerable<StorageLocationInf> storageLocationInfs)
        {
            var paras = new List<StorageLocationData>();

            storageLocationInfs.ForEach(p =>
            {
                var data = new StorageLocationData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.Code = p.Code;
                data.Name = p.Name;
                data.LibraryType = (int)LibraryType.Entity;
                data.AreaCode = p.AreaCode;
                data.WarehouseCode = p.WhCode;

                paras.Add(data);
            });

            return paras;
        }


        /// <summary>
        /// 保存数据到库位
        /// </summary>
        /// <param name="datas">库位列表数据</param>
        /// <returns>错误列表信息</returns>
        public virtual List<ErpErrorData> SaveStorageLocation(List<StorageLocationData> datas)
        {
            var errors = new List<ErpErrorData>();

            var codeList = datas.Select(p => p.Code).Distinct().ToList();
            var locs = RT.Service.Resolve<WarehouseController>().GetStorageLocations(codeList);
            var locDic = locs.ToDictionary(p => p.Code);

            var areaList = datas.Select(p => p.AreaCode).Distinct().ToList();
            var areas = RT.Service.Resolve<WarehouseController>().GetAreaList(areaList);
            var areaDic = areas.ToDictionary(p => p.Code);

            var warehouseList = datas.Select(p => p.WarehouseCode).Distinct().ToList();
            var warehouses = RT.Service.Resolve<WarehouseController>().GetWarehouseList(warehouseList);
            var warehouseDic = warehouses.ToDictionary(p => p.Code);

            foreach (var p in datas)
            {
                var error = new ErpErrorData();
                error.Infkey = p.Infkey;
                try
                {
                    Warehouse warehouse = null;
                    warehouseDic.TryGetValue(p.WarehouseCode, out warehouse);
                    if (warehouse.Id == 0)
                    {
                        error.ErrMsg = "仓库{0}不存在".L10nFormat(p.WarehouseCode);
                        errors.Add(error);
                    }

                    StorageArea area = null;
                    areaDic.TryGetValue(p.AreaCode, out area);
                    if (area.Id == 0)
                    {
                        error.ErrMsg = "库区{0}不存在".L10nFormat(p.AreaCode);
                        errors.Add(error);
                    }

                    var loc = new StorageLocation();
                    if (locDic.ContainsKey(p.Code))
                        locDic.TryGetValue(p.Code, out loc);

                    if (p.IsDelete)
                    {
                        if (loc.Id != 0)
                        {
                            loc.PersistenceStatus = PersistenceStatus.Deleted;
                            locDic.Remove(p.Code);
                            RF.Save(loc);
                        }
                        else
                        {
                            error.ErrMsg = "库位{0}不存在，不能执行删除".L10nFormat(p.Code);
                            errors.Add(error);
                        }
                        continue;
                    }

                    loc.Code = p.Code;
                    loc.Name = p.Name;
                    loc.LibraryType = (LibraryType)p.LibraryType;
                    loc.AreaId = area.Id;
                    loc.WarehouseId = warehouse.Id;
                    loc.State = (State)p.State;
                    loc.ErpInvOrg = p.ErpInvOrg;
                    loc.ErpSubLibrary = p.ErpSubLibrary;
                    loc.ErpLocation = p.ErpLocation;
                    locDic.Remove(p.Code);
                    if (loc.Id == 0)
                    {
                        locDic.Add(p.Code, loc);
                    }
                    RF.Save(loc);
                }
                catch (Exception ex)
                {
                    error.ErrMsg = ex.Message;
                    errors.Add(error);
                }
            }
            return errors;
        }

    }
}
