using SIE.Api;
using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.WorkOrders;
using SIE.Fixtures.ApiModels;
using SIE.Fixtures.FixtureRecords;
using SIE.Fixtures.Fixtures.Abnormals;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Models;
using SIE.Fixtures.Repairs;
using SIE.Resources.WipResources;
using SIE.Utils;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Fixtures
{
    public partial class ElecFixtureController : CoreFixtureController
    {
        #region 工治具报修接口列表

        /// <summary>
        /// 获取库位列表
        /// </summary>
        /// <returns></returns>
        [ApiService("获取库位列表")]
        [return: ApiReturn("仓库列表 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetRepairStorageLocation([ApiParameter("工治具ID")] string code, [ApiParameter("仓库Id")] double? warehouseId)
        {
            if (!code.IsNotEmpty())
            {
                throw new ValidationException(fixtureCodeIsNullOrEmpty.L10N());
            }
            if (!warehouseId.HasValue)
            {
                throw new ValidationException("请先选择仓库".L10N());
            }

            var account = GetFixtureAccountByCodeOrRFID(code);
            if (account == null)
            {
                throw new ValidationException(fixtureAccountNotExists.L10N());
            }

            var storageLocations = Query<FixtureAccountStock>().Where(m => m.FixtureAccountId == account.Id && m.FixtureWarehouseId == warehouseId).ToList(null, new EagerLoadOptions().LoadWithViewProperty()); //获取库存详情下的库存信息
            var infos = new List<BaseDataInfo>();
            storageLocations.ForEach(storageLocation =>
            {
                if (!infos.Any(m => m.Id == storageLocation.FixtureStorageLocationId) && storageLocation.FixtureStorageLocationId.HasValue)
                {
                    infos.Add(new BaseDataInfo()
                    {
                        Id = storageLocation.FixtureStorageLocationId.Value,
                        Code = storageLocation.LocationCode,
                        Name = storageLocation.LocationName,
                    });
                }
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = 1,
                PageSize = storageLocations.Count(),
                TotalCount = storageLocations.Count()
            };
            result.DataInfos.AddRange(infos);
            return result;
        }
        [ApiService("扫描库位")]
        [return: ApiReturn("扫描库位 PagingBaseDataInfo")]
        public virtual BaseDataInfo GetScanStorageLocation([ApiParameter("工治具ID")] string code, [ApiParameter("仓库Id")] double? warehouseId, [ApiParameter("库位编码")] string locationCode)
        {
            if (!code.IsNotEmpty())
            {
                throw new ValidationException(fixtureCodeIsNullOrEmpty.L10N());
            }
            if (!warehouseId.HasValue)
            {
                throw new ValidationException("请先选择仓库".L10N());
            }
            var account = GetFixtureAccountByCodeOrRFID(code);
            if (account == null)
            {
                throw new ValidationException(fixtureAccountNotExists.L10N());
            }

            var storageLocation = Query<FixtureAccountStock>().Where(m => m.FixtureAccountId == account.Id && m.FixtureWarehouseId == warehouseId && m.StorageLocation.Code == locationCode)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty()); //获取库存详情下的库存信息
            if (storageLocation == null)
            {
                throw new ValidationException("扫描条码在工治具{0}的库存仓库下不存在，请确认".L10nFormat(code));
            }
            return new BaseDataInfo()
            {
                Id = storageLocation.FixtureStorageLocationId.Value,
                Code = storageLocation.LocationCode,
                Name = storageLocation.LocationName,
            };
        }

        /// <summary>
        /// 扫描仓库
        /// </summary>
        /// <param name="code"></param>
        /// <param name="warehouseCode"></param>
        /// <returns></returns>
        [ApiService("扫描仓库")]
        [return: ApiReturn("扫描仓库 BaseDataInfo")]
        public virtual BaseDataInfo GetScanWarehouse([ApiParameter("工治具ID")] string code, [ApiParameter("仓库编码")] string warehouseCode)
        {
            if (!code.IsNotEmpty())
            {
                throw new ValidationException(fixtureCodeIsNullOrEmpty.L10N());
            }
            var account = GetFixtureAccountByCodeOrRFID(code);
            if (account == null)
            {
                throw new ValidationException(fixtureAccountNotExists.L10N());
            }

            var wareHouse = Query<FixtureAccountStock>().Where(m => m.FixtureAccountId == account.Id&&m.Warehouse.Code==warehouseCode).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (wareHouse == null)
            {
                throw new ValidationException("扫描在当前工治具{0}的库存仓库下不存在，请确认".L10nFormat(code));
            }
            return new BaseDataInfo()
            {
                Id = wareHouse.FixtureWarehouseId,
                Code = wareHouse.WarehouseCode,
                Name = wareHouse.WarehouseName,
            };
        }


        /// <summary>
        /// 获取仓库列表
        /// </summary>
        /// <returns></returns>
        [ApiService("获取仓库列表")]
        [return: ApiReturn("仓库列表 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetRepairWareHouseInfo([ApiParameter("工治具ID")] string code)
        {
            if (!code.IsNotEmpty())
            {
                throw new ValidationException(fixtureCodeIsNullOrEmpty.L10N());
            }
            var account = GetFixtureAccountByCodeOrRFID(code);
            if (account == null)
            {
                throw new ValidationException(fixtureAccountNotExists.L10N());
            }

            var wareHouses = Query<FixtureAccountStock>().Where(m => m.FixtureAccountId == account.Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty()); //获取库存详情下的库存信息
            var infos = new List<BaseDataInfo>();
            wareHouses.ForEach(warehouse =>
            {
                if (!infos.Any(m => m.Id == warehouse.FixtureWarehouseId))
                {
                    infos.Add(new BaseDataInfo()
                    {
                        Id = warehouse.FixtureWarehouseId,
                        Code = warehouse.WarehouseCode,
                        Name = warehouse.WarehouseName,
                    });
                }
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = 1,
                PageSize = wareHouses.Count(),
                TotalCount = wareHouses.Count()
            };
            result.DataInfos.AddRange(infos);
            return result;
        }



        #region 验证工治具报修ID编码 ValidateRepairIDCode        
        /// <summary>
        /// 验证工治具报修工治具ID
        /// </summary>
        /// <param name="code">工治具ID</param>
        /// <param name="repairBeforeState">报修前状态</param>
        /// <returns>工治具报修工治具台帐信息</returns>
        [ApiService("验证工治具报修工治具ID")]
        [return: ApiReturn("验证工治具报修工治具ID ValidateRepairIDCode")]
        public virtual RepairIDCodeInfo ValidateRepairIDCode([ApiParameter("工治具ID")] string code, [ApiParameter("报修前状态")] int repairBeforeState)
        {
            var repairInfo = new RepairIDCodeInfo();
            if (repairBeforeState != (int)RepairBeforeState.InStock && repairBeforeState != (int)RepairBeforeState.Online)
            {
                throw new ValidationException("报修前状态不存在！".L10N());
            }
            if (!code.IsNotEmpty())
            {
                throw new ValidationException(fixtureCodeIsNullOrEmpty.L10N());
            }
            var account = GetFixtureAccountByCodeOrRFID(code);
            if (account == null)
            {
                throw new ValidationException(fixtureAccountNotExists.L10N());
            }

            repairInfo.FixtureRepairDetailInfo = RT.Service.Resolve<CoreFixtureController>().GetFixtureRepairDetailInfo(account.Id);
            if (account.ManageMode == ManageMode.Number)
            {
                ValidateIDAccountOfRepair(repairBeforeState, repairInfo, account);
            }
            else
            {
                ValidateCodeAccount(repairBeforeState, repairInfo, account);
            }

            repairInfo.Code = account.Code;
            repairInfo.EncodeCode = account.EncodeCode;
            repairInfo.ModelName = account.ModelName;
            repairInfo.FixtureType = account.FixtureTypeId.HasValue ? account.FixtureTypeCode : "";
            repairInfo.ManageMode = account.ManageMode.ToLabel();
            if (repairInfo.FixtureRepairDetailInfo.WorkOrderId.HasValue)//取工单的产线
            {
                var workOrderInfo = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderResource(repairInfo.FixtureRepairDetailInfo.WorkOrderId.Value);
                var resource = RF.GetById<WipResource>(workOrderInfo.ResourceId);
                if (resource != null)
                {
                    repairInfo.ResourceId = resource.Id;
                    repairInfo.ResourceName = resource.Name;
                }
            }

            return repairInfo;
        }

        /// <summary>
        /// 验证编码类工治具台帐
        /// </summary>
        /// <param name="repairBeforeState">报修前状态</param>
        /// <param name="repairInfo">报修ID编码信息</param>
        /// <param name="account">工治具台帐</param>
        private static void ValidateCodeAccount(int repairBeforeState, RepairIDCodeInfo repairInfo, FixtureAccount account)
        {
            if (repairBeforeState == (int)RepairBeforeState.InStock)
            {
                if (account.InStockQty <= 0)
                    throw new ValidationException("编码类工治具台帐的在库数量小于等于零，不可报修！".L10N());
                repairInfo.Qty = account.InStockQty;
            }
            else
            {
                if (account.OnlineQty <= 0)
                    throw new ValidationException("编码类工治具台帐的在线数量小于等于零，不可报修！".L10N());
                repairInfo.Qty = account.OnlineQty;
            }
        }

        /// <summary>
        /// 验证ID类工治具台帐
        /// </summary>
        /// <param name="repairBeforeState">报修前状态</param>
        /// <param name="repairInfo">报修ID编码信息</param>
        /// <param name="account">工治具台帐</param>
        private void ValidateIDAccountOfRepair(int repairBeforeState, RepairIDCodeInfo repairInfo, FixtureAccount account)
        {
            if (!(account.AccountState == FixtureAccountState.Online || account.AccountState == FixtureAccountState.Using || account.AccountState == FixtureAccountState.InStorage))
            {
                throw new ValidationException("ID类工治具台帐的状态[{0}]不为在线、使用中和在库，不可报修！".L10nFormat(EnumViewModel.EnumToLabel(account.AccountState).L10N()));
            }
            if (repairBeforeState == (int)RepairBeforeState.InStock)
            {
                if (account.AccountState != FixtureAccountState.InStorage)
                {
                    throw new ValidationException("报修前状态为在库，ID类工治具台帐的状态[{0}]也必须为在库，不可报修！".L10nFormat(EnumViewModel.EnumToLabel(account.AccountState).L10N()));
                }
                var stock = GetTotalStock(account.Id);
                if (stock == null)
                {
                    throw new ValidationException("报修前状态为在库的ID类工治具台帐的库存台帐不存在，不可报修！".L10N());
                }
                repairInfo.WarehouseId = stock.FixtureWarehouseId;
                repairInfo.WarehouseName = stock.WarehouseName;
                repairInfo.LocationId = stock.FixtureStorageLocationId;
                repairInfo.StorageLocationName = stock.LocationName;
                repairInfo.QualityState = account.QualityState.HasValue ? (int)account.QualityState.Value : 0;
            }
            else
            {
                if (!(account.AccountState == FixtureAccountState.Using || account.AccountState == FixtureAccountState.Online))
                {
                    throw new ValidationException("报修前状态为在线，ID类工治具台帐的状态[{0}]也必须为在线/使用中，不可报修！".L10nFormat(EnumViewModel.EnumToLabel(account.AccountState).L10N()));
                }
            }

            repairInfo.Qty = 1;
        }
        #endregion

        #region 获取报修仓库库位信息列表 GetRepairWareLocationInfos   （已废弃）
        /// <summary>
        /// 根据工治具ID和库位编码获取报修仓库库位信息（已废弃）
        /// </summary>
        /// <param name="code">工治具ID</param>
        /// <param name="locationCode">库位编码</param>
        /// <param name="repairBeforeState">报修前状态</param>
        /// <returns>报修仓库库位信息</returns>
        [ApiService("获取报修仓库库位信息列表")]
        [return: ApiReturn("获取报修仓库库位信息列表 GetRepairWareLocationInfos")]
        public virtual WareLocationInfo GetRepairWareLocationInfos([ApiParameter("工治具ID")] string code, [ApiParameter("报修前状态")] int repairBeforeState, [ApiParameter("库位编码")] string locationCode)
        {
            if (!code.IsNotEmpty())
            {
                throw new ValidationException(fixtureCodeIsNullOrEmpty.L10N());
            }
            if (!locationCode.IsNotEmpty())
            {
                throw new ValidationException("输入/扫描的库位编码不能为空！".L10N());
            }
            if (repairBeforeState != (int)RepairBeforeState.InStock)
            {
                throw new ValidationException("报修前状态为在线，无需输入/扫描库位！".L10nFormat(EnumViewModel.EnumToLabel(RepairBeforeState.Online).L10N()));
            }
            var account = GetFixtureAccountByCodeOrRFID(code);
            if (account.ManageMode == ManageMode.Number)
            {
                if (account.AccountState != FixtureAccountState.InStorage)
                {
                    throw new ValidationException("ID类工治具台帐的状态[{0}]不为在库，无需输入/扫描库位！".L10nFormat(EnumViewModel.EnumToLabel(account.AccountState).L10N()));
                }
                if (account.FixedStorage == YesNo.Yes && account.LocationCode != locationCode)
                {
                    throw new ValidationException("ID类工治具台帐，固定储位的仓库[{0}]和输入/扫描的库位[{1}]不一致，请先确认！".L10nFormat(account.LocationCode, locationCode));
                }
            }
            if (account.ManageMode == ManageMode.Code && account.InStockQty <= 0)
            {
                throw new ValidationException("编码类工治具台帐的在库数量小于等于零，请先确认！".L10N());
            }
            var stock = GetTotalStock(account.Id, locationCode);
            if (stock == null)
            { throw new ValidationException("工治具台帐[{0}]的库位[{1}]不存在出库的库存台帐，请先入库！".L10nFormat(code, locationCode)); }
            var result = new WareLocationInfo();
            result.LocationId = stock.FixtureStorageLocationId.HasValue ? stock.FixtureStorageLocationId.Value : 0;
            result.Location = codeNameFormant.L10nFormat(stock.LocationCode, stock.LocationName);
            result.WarehouseId = stock.FixtureWarehouseId;
            result.Warehouse = codeNameFormant.L10nFormat(stock.WarehouseCode, stock.WarehouseName);
            result.Qty = stock.TotalQty;

            return result;
        }
        #endregion

        #region 获取异常现象信息列表 GetPagingAbnormalInfos        
        /// <summary>
        /// 获取异常现象信息列表
        /// </summary>
        /// <param name="abnormalQueryInfo">异常现象查询信息</param>
        /// <returns>异常现象信息列表</returns>
        [ApiService("获取异常现象信息列表")]
        [return: ApiReturn("获取异常现象信息列表 GetPagingAbnormalInfos")]
        public virtual AbnormalDataInfo GetPagingAbnormalInfos([ApiParameter("异常现象查询信息")] AbnormalQueryInfo abnormalQueryInfo)
        {
            if (abnormalQueryInfo == null)
            {
                throw new ValidationException(fixtureCodeIsNullOrEmpty.L10N());
            }
            var result = new AbnormalDataInfo();
            var pagingInfo = GetPagingInfo(abnormalQueryInfo.PageNumber, abnormalQueryInfo.PageSize);
            if (!abnormalQueryInfo.Code.IsNotEmpty())
            {
                throw new ValidationException(fixtureCodeIsNullOrEmpty.L10N());
            }
            var account = GetFixtureAccountByCodeOrRFID(abnormalQueryInfo.Code);
            if (account == null || account.FixtureEncode == null)
            {
                throw new ValidationException("工治具台帐{0}不存在！".L10nFormat(abnormalQueryInfo.Code));
            }
            var abnormals = GetFixtureAbnormals(account.FixtureEncode.FixtureModel.FixtureType, abnormalQueryInfo.Keyword, pagingInfo);
            abnormals.ForEach(a =>
            {
                var abnormalInfo = new AbnormalInfo
                {
                    AbnormalId = a.Id,
                    Code = a.Code,
                    Description = a.Description,
                    AbnormalDescription = codeNameFormant.L10nFormat(a.Code, a.Description)
                };

                result.AbnormalInfos.Add(abnormalInfo);
            });

            return result;
        }
        #endregion

        #region 提交工治具报修信息 SubmitRepairInfo       
        /// <summary>
        /// 提交工治具报修信息
        /// </summary>
        /// <param name="repairInfo">工治具报修信息</param> 
        [ApiService("提交工治具报修信息")]
        [return: ApiReturn("提交工治具报修信息 SubmitRepairInfo")]
        public virtual void SubmitRepairInfo([ApiParameter("工治具报修信息")] RepairInfo repairInfo)
        {
            if (repairInfo.RepairBeforeState != (int)RepairBeforeState.InStock && repairInfo.RepairBeforeState != (int)RepairBeforeState.Online)
            {
                throw new ValidationException("报修前状态不存在！".L10N());
            }

            //在线报修 校验工单产线必输
            if (repairInfo.RepairBeforeState == (int)RepairBeforeState.Online)
            {
                if (!repairInfo.WorkOrderId.HasValue)
                {
                    throw new ValidationException("在线报修,工单必输!".L10N());
                }
                if (!repairInfo.ResourceId.HasValue)
                {
                    throw new ValidationException("在线报修,产线必输!".L10N());
                }
            }
            if (!repairInfo.Code.IsNotEmpty())
            {
                throw new ValidationException(fixtureCodeIsNullOrEmpty.L10N());
            }
            var account = GetFixtureAccountByCodeOrRFID(repairInfo.Code);
            if (account == null)
            { throw new ValidationException(fixtureAccountNotExists.L10N()); }
            if (account.ManageMode == ManageMode.Number)//ID类
            {
                ValidateIDAccount(repairInfo, account);
            }
            else
            {
                ValidateCodeAccount(repairInfo, account);
            }
            var abnormal = ValidateAbnormal(repairInfo, account);
            SaveRepairInfo(repairInfo, account, abnormal);
        }

        /// <summary>
        /// 保存工治具报修信息及其相关信息
        /// </summary>
        /// <param name="repairInfo">工治具报修信息</param>
        /// <param name="account">工治具台帐</param>
        /// <param name="abnormal">工治具异常类型</param>
        private void SaveRepairInfo(RepairInfo repairInfo, FixtureAccount account, FixtureAbnormal abnormal)
        {
            using (var tran = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
            {
                var repair = CreateFixtureRepair();
                var repairDetail = CreateRepairDetail(repairInfo, account, abnormal);
                repairDetail.GenerateId();
                repair.Details.Add(repairDetail);
                RF.Save(repair);

                SaveRepairAttachment(repairInfo, repairDetail);
                UpdateAccount(repairInfo, account);
                tran.Complete();
            }
        }

        /// <summary>
        /// 验证工治具异常
        /// </summary>
        /// <param name="repairInfo">工治具报修信息</param>
        /// <param name="account">工治具台帐</param>
        /// <returns>工治具异常</returns>
        private FixtureAbnormal ValidateAbnormal(RepairInfo repairInfo, FixtureAccount account)
        {
            var abnormal = RF.GetById<FixtureAbnormal>(repairInfo.AbnormalId);
            if (abnormal == null)
            { throw new ValidationException("工治具异常类型不存在！".L10N()); }
            if (abnormal.AbnormalType != AbnormalType.Unusual)
            { throw new ValidationException("工治具异常类型不是异常现象！".L10N()); }
            if (abnormal.FixtureType.Id != account.FixtureEncode.FixtureModel.FixtureTypeId)
            {
                throw new ValidationException("异常现象的工治具类型与工治具台帐不一致！".L10N());
            }
            return abnormal;
        }

        /// <summary>
        /// 验证编码类工治具台帐
        /// </summary>
        /// <param name="repairInfo">工治具报修信息</param>
        /// <param name="account">工治具台帐</param>
        private void ValidateCodeAccount(RepairInfo repairInfo, FixtureAccount account)
        {
            if (account.InStockQty <= 0 && account.OnlineQty <= 0)
                throw new ValidationException("编码类工治具台帐的在库和在线数量都小于等于零，不可报修！".L10nFormat(EnumViewModel.EnumToLabel(account.AccountState).L10N()));
            if (repairInfo.RepairBeforeState == (int)RepairBeforeState.InStock)
                ValidateInStockCodeAccount(repairInfo, account);
            else
                ValidateOnlineCodeAccount(repairInfo, account);
        }

        /// <summary>
        /// 验证在线的编码类工治具台帐
        /// </summary>
        /// <param name="repairInfo">工治具报修信息</param>
        /// <param name="account">工治具台帐</param>
        private void ValidateOnlineCodeAccount(RepairInfo repairInfo, FixtureAccount account)
        {
            //在线报修的编码类时校验报修数量不能超过工单未归还数量
            var unloads = GetReturnUnloads(repairInfo.WorkOrderId.Value, repairInfo.ResourceId.Value, account.Id);
            if (!unloads.Any())
            { throw new ValidationException("报修数量不能大于工单的未归还数量！".L10N()); }
            var canRepairsQtys = unloads.Sum(p => p.UnloadQty) - unloads.Sum(p => p.ReturnQty) - unloads.Sum(p => p.NgQty);
            if (canRepairsQtys < repairInfo.Qty)
            { throw new ValidationException("报修数量不能超过工单未归还数量：{0}！".L10nFormat(canRepairsQtys)); }


            if (repairInfo.LocationId.HasValue)
                throw new ValidationException("报修前状态为在线的编码类工治具台帐库位不必填！".L10N());
            if (account.OnlineQty <= 0)
                throw new ValidationException("编码类工治具台帐的在线数量小于等于零，不可报修！".L10N());
            if (account.OnlineQty < repairInfo.Qty)
                throw new ValidationException("编码类工治具台帐的报修数量[{0}]大于在线数量，不可报修！".L10nFormat(repairInfo.Qty));
        }

        /// <summary>
        /// 验证在库的编码类工治具台帐
        /// </summary>
        /// <param name="repairInfo">工治具报修信息</param>
        /// <param name="account">工治具台帐</param>
        private void ValidateInStockCodeAccount(RepairInfo repairInfo, FixtureAccount account)
        {
            if (!repairInfo.QualityState.HasValue)
            {
                throw new ValidationException("报修前质量状态必填！".L10N());
            }
            if (!repairInfo.LocationId.HasValue)
            {
                throw new ValidationException("报修前状态为在库的编码类工治具台帐库位必填！".L10N());
            }
            var location = RF.GetById<StorageLocation>(repairInfo.LocationId);
            if (location == null)
                throw new ValidationException("报修前状态为在库的编码类工治具台帐库位不存在！".L10N());
            var stock = GetStockCodeAndLocation(account.Id, repairInfo.WarehouseId.Value, repairInfo.LocationId.Value);
            if (stock == null)
                throw new ValidationException("报修前状态为在库的编码类工治具台帐的库存台帐不存在，不可报修！".L10N());
            if (repairInfo.QualityState == FixtureQualityState.Pass && stock.PassQty < repairInfo.Qty)
            {
                throw new ValidationException("报修数量不能大于库存数，不可报修！".L10N());
            }
            if (repairInfo.QualityState == FixtureQualityState.Ng && stock.NgQty < repairInfo.Qty)
            {
                throw new ValidationException("报修数量不能大于库存数，不可报修！".L10N());
            }
        }

        /// <summary>
        /// 验证ID类工治具台帐
        /// </summary>
        /// <param name="repairInfo">工治具报修信息</param>
        /// <param name="account">工治具台帐</param>
        private void ValidateIDAccount(RepairInfo repairInfo, FixtureAccount account)
        {
            if (!(account.AccountState == FixtureAccountState.Online || account.AccountState == FixtureAccountState.Using || account.AccountState == FixtureAccountState.InStorage))
            {
                throw new ValidationException("ID类工治具台帐的状态[{0}]不为在线、使用中和在库，不可报修！".L10nFormat(EnumViewModel.EnumToLabel(account.AccountState).L10N()));
            }
            if (repairInfo.RepairBeforeState == (int)RepairBeforeState.InStock)
            { ValidateInStockIDAccount(repairInfo, account); }
            else
            { ValidateOnlineIDAccount(repairInfo, account); }
        }

        /// <summary>
        /// 根据【工治具编码 + 仓库 + 库位 + 质量状态】获取当前台账的库存
        /// </summary>
        /// <param name="fixtureAccountId"></param>
        /// <param name="whId"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        private FixtureAccountStock GetStockCodeAndLocation(double fixtureAccountId, double whId, double locationId)
        {
            return Query<FixtureAccountStock>().Where(p => p.FixtureAccountId == fixtureAccountId && p.FixtureStorageLocationId == locationId)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 更新在线ID类工治具台帐
        /// </summary>
        /// <param name="repairInfo">工治具报修信息</param>
        /// <param name="account">工治具台帐</param>
        private void ValidateOnlineIDAccount(RepairInfo repairInfo, FixtureAccount account)
        {
            if (!(account.AccountState == FixtureAccountState.Using || account.AccountState == FixtureAccountState.Online))
            {
                throw new ValidationException("报修前状态为在线，ID类工治具台帐的状态[{0}]也必须为在线/使用中，不可报修！".L10nFormat(EnumViewModel.EnumToLabel(account.AccountState).L10N()));
            }
            if (repairInfo.LocationId.HasValue)
            {
                throw new ValidationException("报修前状态为在线的ID类工治具台帐的库位不必填！".L10N());
            }
            if (repairInfo.Qty != 1)
            {
                throw new ValidationException("ID类工治具台帐的报修数量[{0}]不等于1，不可报修！".L10nFormat(repairInfo.Qty));
            }
        }

        /// <summary>
        /// 验证在库的ID类工治具台帐
        /// </summary>
        /// <param name="repairInfo">工治具报修信息</param>
        /// <param name="account">工治具台帐</param>
        private void ValidateInStockIDAccount(RepairInfo repairInfo, FixtureAccount account)
        {
            if (account.AccountState != FixtureAccountState.InStorage)
            {
                throw new ValidationException("报修前状态为在库，ID类工治具台帐的状态[{0}]也必须为在库，不可报修！".L10nFormat(EnumViewModel.EnumToLabel(account.AccountState).L10N()));
            }
            if (!repairInfo.LocationId.HasValue)
            {
                throw new ValidationException("报修前状态为在库的ID类工治具台帐库位必填！".L10N());
            }
            var location = RF.GetById<StorageLocation>(repairInfo.LocationId);
            if (location == null)
            {
                throw new ValidationException("报修前状态为在库的ID类工治具台帐库位不存在！".L10N());
            }
            var stock = GetStockByIdCodeAndLocation(account.Id, location.Id);
            if (stock == null)
            {
                throw new ValidationException("报修前状态为在库的ID类工治具台帐的库存台帐不存在，不可报修！".L10N());
            }
            if (stock.TotalQty <= 0)
            {
                throw new ValidationException("报修前状态为在库的ID类工治具台帐的在库数量小于等于零，不可报修！".L10N());
            }
            if (stock.TotalQty < repairInfo.Qty)
            {
                throw new ValidationException("报修前状态为在库的ID类工治具台帐的报修数量[{0}]大于在库数量[{1}]，不可报修！".L10nFormat(repairInfo.Qty, stock.TotalQty));
            }
            if (repairInfo.Qty != 1)
            {
                throw new ValidationException("ID类工治具台帐的报修数量[{0}]不等于1，不可报修！".L10nFormat(repairInfo.Qty));
            }
        }

        /// <summary>
        /// 创建工治具出入库记录
        /// </summary>
        /// <param name="qty">工治具报修数量</param>
        /// <param name="account">工治具台账</param>
        /// <param name="stock">库存台帐</param>
        private void CreateFixtureRecordByAccountStock(int qty, FixtureAccount account, FixtureAccountStock stock)
        {
            var now = RF.Find<FixtureRecord>().GetDbTime();
            var record = new FixtureRecord()
            {
                RecordType = RecordType.Out,
                BusinessType = BusinessType.RepairOut,
                Code = string.Empty,
                FixtureAccountId = account.Id,
                FixtureWarehouseId = stock.FixtureWarehouseId,
                FixtureStorageLocationId = stock.FixtureStorageLocationId,
                Qty = qty,
                ApplyById = RT.IdentityId,
                ApplyDate = now,
                ComplyById = RT.IdentityId,
                ComplyDate = now
            };
            RF.Save(record);
        }

        /// <summary>
        /// 保存工治具报修上传附件
        /// </summary>
        /// <param name="repairInfo">工治具报修信息</param>
        /// <param name="repairDetail">工治具异常详情</param>
        private void SaveRepairAttachment(RepairInfo repairInfo, FixtureRepairDetail repairDetail)
        {
            if (repairInfo.RepairFileInfos.Count > 0)
            {
                foreach (var repairFileInfo in repairInfo.RepairFileInfos)
                {
                    var fileName = repairFileInfo.FileName;
                    var dataUrl = repairFileInfo.DataURL;
                    if (dataUrl.IndexOf(",") != -1)
                    {
                        var attachment = new FixtureRepairAttachment();
                        dataUrl = dataUrl.Substring(dataUrl.IndexOf(",") + 1);
                        var content = Convert.FromBase64String(dataUrl);
                        attachment.FileExtesion = ".png";
                        attachment.FileName = fileName;
                        var equalIndex = dataUrl.IndexOf('=');
                        if (dataUrl.IndexOf('=') != -1)
                        { dataUrl = dataUrl.Substring(0, equalIndex); }
                        var strLength = dataUrl.Length;
                        var fileLength = Math.Round((strLength - (strLength / 8) * 2) / 1024.0, 2);
                        attachment.FileSize = fileLength + "kb";
                        attachment.Content = content;
                        attachment.OwnerId = repairDetail.Id;
                        attachment.PersistenceStatus = PersistenceStatus.New;
                        RF.Save(attachment);
                    }
                }
            }
        }

        /// <summary>
        /// 更新工治具台帐
        /// </summary>
        /// <param name="repairInfo">工治具报修信息</param>
        /// <param name="account">工治具台帐</param>
        private void UpdateAccount(RepairInfo repairInfo, FixtureAccount account)
        {
            if (account.ManageMode == ManageMode.Number)
            {
                UpdateIDAccountAndStockOfRepair(repairInfo, account);
            }
            else
            {
                UpdateCodeAccountAndStockOfRepair(repairInfo, account);
            }
        }

        /// <summary>
        /// 更新编码类工治具台帐和库存台帐
        /// </summary>
        /// <param name="repairInfo">报修前状态</param>
        /// <param name="account">工治具台帐</param>
        private void UpdateCodeAccountAndStockOfRepair(RepairInfo repairInfo, FixtureAccount account)
        {
            var repairBeforeState = (RepairBeforeState)repairInfo.RepairBeforeState;//报修前状态
            if (repairBeforeState == RepairBeforeState.Online)
            {
                //编码类的更新编码台账：【在线、合格】减少，【待维修、不合格】增加
                account.OnlineQty -= repairInfo.Qty;
                account.PassQty -= repairInfo.Qty;
                account.NgQty += repairInfo.Qty;
            }
            else if (repairBeforeState == RepairBeforeState.InStock)
            {
                UpdateInStockAccount(repairInfo, account);
            }
            account.WaitRepair += repairInfo.Qty;
            RF.Save(account);
        }

        /// <summary>
        /// 更新在库编码类工治具台帐
        /// </summary>
        /// <param name="repairInfo">工治具报修数量</param>
        /// <param name="account">工治具台帐</param>
        private void UpdateInStockAccount(RepairInfo repairInfo, FixtureAccount account)
        {
            var stock = GetTotalStock(account.Id, repairInfo.LocationId.Value);
            account.InStockQty -= repairInfo.Qty;
            /*  俊杰提出优化
            在库+合格报修：【在库、合格】减少，【待维修、不合格】增加
            在库+不合格报修：【在库】减少，【待维修】增加
            */

            if (repairInfo.QualityState == FixtureQualityState.Pass)
            {
                account.PassQty -= repairInfo.Qty;
                account.NgQty += repairInfo.Qty;
                stock.PassQty -= repairInfo.Qty;
            }
            if (repairInfo.QualityState == FixtureQualityState.Ng)
            {
                stock.NgQty -= repairInfo.Qty;
            }
            stock.TotalQty -= repairInfo.Qty;
            stock.PersistenceStatus = PersistenceStatus.Modified;
            if (stock.TotalQty == 0)
                stock.PersistenceStatus = PersistenceStatus.Deleted;
            RF.Save(stock);

            CreateFixtureRecordByAccountStock(repairInfo.Qty, account, stock);
        }

        /// <summary>
        /// 更新ID类台帐和台帐库存
        /// </summary>
        /// <param name="repairInfo">工治具报修数量</param>
        /// <param name="account">工治具台帐</param>
        private void UpdateIDAccountAndStockOfRepair(RepairInfo repairInfo, FixtureAccount account)
        {
            if (account.AccountState == FixtureAccountState.InStorage)
            {
                var stock = GetTotalStock(account.Id);
                stock.PersistenceStatus = PersistenceStatus.Deleted;
                RF.Save(stock);
                CreateFixtureRecordByAccountStock(repairInfo.Qty, account, stock);
            }

            account.AccountState = FixtureAccountState.WaitRepair;
            account.QualityState = FixtureQualityState.Ng;
            RF.Save(account);
        }

        /// <summary>
        /// 创建工治具异常详情
        /// </summary>
        /// <param name="repairInfo">工治具报修信息</param>
        /// <param name="account">工治具台帐</param>
        /// <param name="abnormal">异常现象</param>
        /// <returns>工治具异常详情</returns>
        private FixtureRepairDetail CreateRepairDetail(RepairInfo repairInfo, FixtureAccount account, FixtureAbnormal abnormal)
        {
            var repairDetail = new FixtureRepairDetail()
            {
                FixtureAccountId = account.Id,
                AbnormalId = abnormal.Id,
                Qty = repairInfo.Qty,
                RepairBeforeState = (RepairBeforeState)repairInfo.RepairBeforeState,
                RepairBeforeQualityStatus = FixtureQualityState.Pass,
            };

            if (repairDetail.RepairBeforeState == RepairBeforeState.InStock)
            {
                repairDetail.FixtureWarehouseId = repairInfo.WarehouseId;
                repairDetail.FixtureStorageLocationId = repairInfo.LocationId;
                repairDetail.RepairBeforeQualityStatus = repairInfo.QualityState;
            }
            else
            {
                repairDetail.WorkOrderId = repairInfo.WorkOrderId;

            }
            return repairDetail;
        }

        /// <summary>
        /// 创建工治具报修
        /// </summary>
        /// <returns>工治具报修</returns>
        private FixtureRepair CreateFixtureRepair()
        {
            return new FixtureRepair()
            {
                No = GetFixtureRepairNo(),
                RepairState = RepairState.Wait,
                ApplyById = RT.IdentityId,
                ApplyDate = RF.Find<FixtureRepair>().GetDbTime()
            };
        }
        #endregion

        #endregion
    }
}
