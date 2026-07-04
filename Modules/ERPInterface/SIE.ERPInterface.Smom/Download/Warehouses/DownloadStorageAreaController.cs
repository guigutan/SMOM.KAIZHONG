using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 库区下载控制器
    /// </summary>
    public class DownloadStorageAreaController : DomainController
    {
        /// <summary>
        /// 从API下载库区到业务表
        /// </summary>
        /// <param name="storageAreaDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadStorageAreaToBusiness(List<StorageAreaData> storageAreaDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<StorageAreaData>(
                storageAreaDatas,
                p => this.SaveStorageArea(p.OrderByLastUpdateDate()),
                JobType.StorageArea,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载库区到业务表
        /// </summary>
        public virtual ProcessResult DownloadStorageAreaInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<StorageAreaInf>(
                () => ctl.GetUnprocessedDatas<StorageAreaInf>(),               //库区中间表数据
                p =>
                {
                    var paras = this.GenerateStorageAreaPara(p);
                    return this.SaveStorageArea(paras.OrderByLastUpdateDate());
                },
                JobType.StorageArea, isManual);
        }

        /// <summary>
        /// 生成库区实体
        /// </summary>
        /// <param name="storageAreaInfs">中间表实体数据</param>
        /// <returns></returns>
        private List<StorageAreaData> GenerateStorageAreaPara(IEnumerable<StorageAreaInf> storageAreaInfs)
        {
            var paras = new List<StorageAreaData>();

            storageAreaInfs.ForEach(p =>
            {
                var data = new StorageAreaData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.Code = p.Code;
                data.Name = p.Name;
                data.LibraryType = (int)Warehouses.LibraryType.Entity;
                data.WarehouseCode = p.WarehouseCode;
                data.ErpKey = p.ErpKey;

                paras.Add(data);
            });

            return paras;
        }

        /// <summary>
        /// 保存数据到库区
        /// </summary>
        /// <param name="datas">库区列表数据</param>
        /// <returns>错误列表信息</returns>
        public virtual List<ErpErrorData> SaveStorageArea(List<StorageAreaData> datas)
        {
            var errors = new List<ErpErrorData>();
            var ctl = RT.Service.Resolve<Warehouses.WarehouseController>();

            var codeList = datas.Select(p => p.Code).Distinct().ToList();
            var areas = ctl.GetAreaList(codeList);
            var areaDic = areas.ToDictionary(p => p.Code);

            var warehouseList = datas.Select(p => p.WarehouseCode).Distinct().ToList();
            var warehouses = ctl.GetWarehouseList(warehouseList);
            var warehouseDic = warehouses.ToDictionary(p => p.Code);

            foreach (var p in datas)
            {
                var error = new ErpErrorData();
                error.Infkey = p.Infkey;

                try
                {
                    Warehouses.Warehouse warehouse = null;
                    warehouseDic.TryGetValue(p.WarehouseCode, out warehouse);
                    if (warehouse.Id == 0)
                    {
                        error.ErrMsg = "仓库{0}不存在".L10nFormat(p.WarehouseCode);
                        errors.Add(error);
                    }

                    var area = new Warehouses.StorageArea();
                    if (areaDic.ContainsKey(p.Code))
                        areaDic.TryGetValue(p.Code, out area);

                    if (p.IsDelete)
                    {
                        if (area.Id != 0)
                        {
                            area.PersistenceStatus = PersistenceStatus.Deleted;
                            areaDic.Remove(p.Code);
                            RF.Save(area);
                        }
                        else
                        {
                            error.ErrMsg = "库区{0}不存在，不能执行删除".L10nFormat(p.Code);
                            errors.Add(error);
                        }
                        continue;
                    }

                    area.Code = p.Code;
                    area.Name = p.Name;
                    area.LibraryType = (Warehouses.LibraryType)p.LibraryType;
                    area.WarehouseId = warehouse.Id;
                    area.State = (State)p.State;
                    if (area.Id == 0)
                    {
                        areaDic.Add(p.Code, area);
                    }
                    RF.Save(area);
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
