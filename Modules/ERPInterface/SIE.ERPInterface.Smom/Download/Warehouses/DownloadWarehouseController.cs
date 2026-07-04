using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Download.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 仓库下载控制器
    /// </summary>
    public class DownloadWarehouseController : DomainController
    {
        /// <summary>
        /// 从API下载仓库到业务表
        /// </summary>
        /// <param name="warehouseDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadWarehouseToBusiness(List<WarehouseData> warehouseDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<WarehouseData>(
                warehouseDatas,
                p => this.SaveWarehouse(p.OrderByLastUpdateDate()),
                JobType.Warehouse,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载仓库到业务表
        /// </summary>
        public virtual ProcessResult DownloadWarehouseInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<WarehouseInf>(
                () => ctl.GetUnprocessedDatas<WarehouseInf>(),             //仓库中间表数据
                p =>
                {
                    var paras = this.GenerateWarehousePara(p);
                    return this.SaveWarehouse(paras.OrderByLastUpdateDate());
                },
                JobType.Warehouse, isManual);
        }

        /// <summary>
        /// 生成仓库实体
        /// </summary>
        /// <param name="warehouseInfs">中间表实体数据</param>
        /// <returns></returns>
        private List<WarehouseData> GenerateWarehousePara(IEnumerable<WarehouseInf> warehouseInfs)
        {
            var paras = new List<WarehouseData>();

            warehouseInfs.ForEach(p =>
            {
                var data = new WarehouseData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.Code = p.Code;
                data.Name = p.Name;
                data.LibraryType = (int)SIE.Warehouses.LibraryType.Entity;
                ////warehouseData.Category = warehouseInf.Category;
                data.SimpleCode = p.SimpleCode;

                paras.Add(data);
            });

            return paras;
        }

        /// <summary>
        /// 手动下载
        /// </summary>
        /// <param name="keyWord">查询关键字</param>
        public virtual string DownloadManual(string keyWord)
        {
            ProcessResult result = new ProcessResult();
            string resultMsg = string.Empty;

            try
            {
                if (keyWord.IsNullOrEmpty())
                    throw new ValidationException("唯一主键不能为空".L10N());
                using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
                {
                    RT.Service.Resolve<SoapWarehouseController>().DownloadToInf(true, keyWord);                     //执行中间表下载
                    result = DownloadWarehouseInfToBusiness(true);           //执行业务表下载
                    trans.Complete();
                }
            }
            catch (Exception ex)
            {
                result.AddFailMsg(ex.GetBaseException());
            }

            if (!result.Result) resultMsg = result.FailMsg.FirstOrDefault();
            return resultMsg;

        }


        /// <summary>
        /// 保存数据到仓库
        /// </summary>
        /// <param name="datas">仓库列表数据</param>
        /// <returns>错误列表信息</returns>
        public virtual List<ErpErrorData> SaveWarehouse(List<WarehouseData> datas)
        {
            var errors = new List<ErpErrorData>();

            var codeList = datas.Select(p => p.Code).Distinct().ToList();
            var warehouses = RT.Service.Resolve<SIE.Warehouses.WarehouseController>().GetWarehouseList(codeList);
            var warehouseDic = warehouses.ToDictionary(p => p.Code);

            foreach (var p in datas)
            {
                var error = new ErpErrorData();
                error.Infkey = p.Infkey;

                try
                {
                    var warehouse = new SIE.Warehouses.Warehouse();
                    if (warehouseDic.ContainsKey(p.Code))
                        warehouseDic.TryGetValue(p.Code, out warehouse);

                    if (p.IsDelete)
                    {
                        if (warehouse.Id != 0)
                        {
                            warehouse.PersistenceStatus = PersistenceStatus.Deleted;
                            warehouseDic.Remove(p.Code);
                            RF.Save(warehouse);
                        }
                        else
                        {
                            error.ErrMsg = "仓库{0}不存在，不能执行删除".L10nFormat(p.Code);
                            errors.Add(error);
                        }
                        continue;
                    }

                    warehouse.Code = p.Code;
                    warehouse.Name = p.Name;
                    warehouse.LibraryType = (SIE.Warehouses.LibraryType)p.LibraryType;
                    warehouse.SimpleCode = p.SimpleCode;
                    warehouse.State = (State)p.State;
                    if (warehouse.Id == 0)
                    {
                        warehouseDic.Add(p.Code, warehouse);
                    }
                    RF.Save(warehouse);
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
