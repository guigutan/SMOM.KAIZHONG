using SIE.Api;
using SIE.Common.Configs;
using SIE.Core.ApiModels;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.ApiModel;
using SIE.EMS.Common.Utils;
using SIE.EMS.Equipments.ApiModels;
using SIE.EMS.Equipments.Boms;
using SIE.EMS.SpareParts.ApiModels;
using SIE.EMS.SpareParts.Applys;
using SIE.EMS.SpareParts.Applys.Details;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Items;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 备件控制器API
    /// </summary>
    public partial class SparePartController : DomainController
    {
        /// <summary>
        /// 根据设备台账编码，获取备件BOM
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="equipCode">设备编码</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        [ApiService("根据设备台账编码，获取备件BOM")]
        [return: ApiReturn("备件BOM列表 List<EquipBomInfo>")]
        public virtual SparePartData GetSparePartBomInfos([ApiParameter("每页数据量")] int pageSize, [ApiParameter("页码")] int pageNumber
            , [ApiParameter("设备编码")] string equipCode, [ApiParameter("关键字")] string key)
        {
            if (pageSize <= 0)
                throw new ValidationException("[每页数据量]必须大于0".L10N());
            if (pageNumber <= 0)
                throw new ValidationException("[页码]必须大于0".L10N());

            //构建分页实体
            var pageInfo = new PagingInfo() { PageSize = pageSize, PageNumber = pageNumber, IsNeedCount = true };

            var data = new SparePartData();
            var infos = new List<SparePartInfo>();
            Dictionary<double, int> storeSummaryDic = new Dictionary<double, int>();
            Dictionary<double, SpareOutInfo> outDetails = new Dictionary<double, SpareOutInfo>();

            IList<SpareBomInfo> boms;
            //通过设备编码获取备件BOM
            (data.TotalCount, boms) = RT.Service.Resolve<EquipBomController>().GetEquipBomDetails(equipCode, pageInfo, key);

            //查询、赋值备件库存、申请单剩余数
            if (boms.Count > 0)
            {
                var partIds = boms.Select(p => p.SparePartId).Distinct().ToList();
                storeSummaryDic = RT.Service.Resolve<SparePartController>().CountSpareStoreSummary(partIds);
                outDetails = RT.Service.Resolve<OutDepotController>().GetSpareOutDepotDetailInfo((IList<double>)partIds);
            }

            //构建返回数据 
            boms.ForEach(p =>
            {
                storeSummaryDic.TryGetValue(p.SparePartId, out var goodNumber);
                outDetails.TryGetValue(p.SparePartId, out var outDetail);
                var useCount = outDetail?.UseCount ?? 0;
                var outDepotCount = outDetail?.OutDepotCount ?? 0;

                infos.Add(new SparePartInfo()
                {
                    SparePartId = p.SparePartId,
                    SparePartCode = p.SparePartCode,
                    SparePartName = p.SparePartName,
                    SparePartUnit = p.UnitName,
                    SparePartSpecification = p.Specification,
                    SparePartTypeName = p.SparePartTypeName,
                    EquipModelCode = p.SpEquipModelCode,
                    EquipModelName = p.SpEquipModelName,
                    Manufacturer = p.Manufacturer,
                    StoreQty = goodNumber,
                    RemainingQty = outDepotCount - useCount
                });
                
            });
            data.Data = infos;

            return data;
        }

        /// <summary>
        /// 获取备件基础数据
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="key"></param>
        ///  <param name="modelId"></param>
        /// <returns></returns>
        [ApiService("获取备件基础数据")]
        [return: ApiReturn("备件基础数据列表 SparePartData")]
        public virtual SparePartData GetSparePartData([ApiParameter("每页数据量")] int pageSize, [ApiParameter("页码")] int pageNumber,
            [ApiParameter("关键字")] string key, [ApiParameter("型号Id")] double? modelId = null)
        {
            if (pageSize <= 0)
                throw new ValidationException("[每页数据量]必须大于0".L10N());
            if (pageNumber <= 0)
                throw new ValidationException("[页码]必须大于0".L10N());

            //构建分页实体
            var pageInfo = new PagingInfo() { PageSize = pageSize, PageNumber = pageNumber, IsNeedCount = true };
            //控制器
            var ctl = RT.Service.Resolve<SparePartController>();
            //通过设备编码获取备件基础数据


            var spareParts = modelId.HasValue ? ctl.GetSparePartsByModelCode(pageInfo, key, modelId) : ctl.GetEnableSpareParts(pageInfo, key);
            //查询、赋值备件库存,备件图片
            var storeSummarys = new EntityList<StoreSummary>();
            IList<EmsAttachmentInfo> pics = new List<EmsAttachmentInfo>();
            if (spareParts.Count > 0)
            {
                var partIds = spareParts.Select(p => p.Id).Distinct().ToList();
                storeSummarys = ctl.GetStoreSummarys(partIds);
                pics = ctl.GetSparePartPictureAttachments(partIds);
            }

            //构建返回数据            
            var infos = new SparePartData();
            infos.TotalCount = spareParts.TotalCount;
            spareParts.ForEach(p =>
            {
                var goodNumber = storeSummarys.Where(x => x.SparePartId == p.Id).Sum(x => x.GoodNumber);
                var pic = pics.FirstOrDefault(x => x.OwnerId == p.Id);
                var picBase64Str = FileUrlHelper.GetAttachmentBase64StringData(pic?.FilePath, pic?.FileName);

                infos.Data.Add(new SparePartInfo()
                {
                    SparePartId = p.Id,
                    SparePartCode = p.SparePartCode,
                    SparePartName = p.SparePartName,
                    SparePartUnit = p.Unit?.Name,
                    SparePartSpecification = p.Specification,
                    SparePartTypeName = p.SparePartTypeName,
                    EquipModelCode = p.EquipModelCode,
                    EquipModelName = p.EquipModelName,
                    Manufacturer = p.Manufacturer,
                    StoreQty = goodNumber,
                    PhotoBase64 = picBase64Str,
                    FileExtension = pic?.FileExtension
                });
            });

            return infos;
        }

        /// <summary>
        /// 根据备件ID或者备件出库单
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        [ApiService("获取备件出库单数据")]
        [return: ApiReturn("备件出库单 SpOutDtlPagingResultInfo")]
        public virtual SpOutDtlPagingResultInfo GetSparePartOutInfo([ApiParameter("备件出库单分页参数")] SparePartOutQueryInfo queryInfo)
        {
            //构建分页实体
            var pageInfo = this.GeneratePagingInfo(queryInfo);

            //查询数据
            var ctl = RT.Service.Resolve<OutDepotController>();
            var outDtlList = ctl.GetPartOutDepotDtls(queryInfo.SparePartId, queryInfo.EquipAccountId, queryInfo.ModelId, queryInfo.SourceNo, pageInfo, queryInfo.Keyword);

            //构建返回数据
            var infos = new SpOutDtlPagingResultInfo();
            infos.TotalCount = outDtlList.TotalCount;
            infos.PageSize = pageInfo.PageSize;
            infos.PageNumber = pageInfo.PageNumber;
            outDtlList.ForEach(p =>
            {
                infos.SparePartOutDtlInfos.Add(new SparePartOutInfo()
                {
                    OutDepotNo = p.OutDepotNoView,
                    EquipId = p.EquipAccountId,
                    EquipCode = p.EquipAccountCode,
                    EquipName = p.EquipAccountName,
                    OutDtlId = p.Id,
                    OutStockWarehouseId = p.WarehouseId,
                    OutStockWarehouseName = p.WarehouseName,
                    SparePartId = p.SparePartId,
                    SparePartCode = p.SparePartCode,
                    SparePartName = p.SparePartName,
                    SparePartUnit = p.SparePartUnitName,
                    SparePartSpecification = p.SpecificationView,
                    SparePartTypeName = p.SparePartTypeName,
                    EquipModelCode = p.EquipModelCode,
                    EquipModelName = p.EquipModelName,
                    Manufacturer = p.Manufacturer,
                    RemainingQty = p.OutDepotCount - p.UseCount,
                    OutDepotState = (int)p.OutDepotState,
                    OutDepotStateName = p.OutDepotState.ToLabel(),
                    SeriaNo = p.SeriaNo,
                    BatchNo = p.BatchNoRef?.BatchNumber
                });
            });

            return infos;
        }

        /// <summary>
        /// 获取备件仓库数据
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        [ApiService("获取备件仓库数据")]
        [return: ApiReturn("备件仓库数据 SparePartWhPagingResultInfo")]
        public virtual SparePartWhPagingResultInfo GetSparePartWarehouseInfos([ApiParameter("备件仓库分页参数")] SparePartDepotQueryInfo queryInfo)
        {
            //构建分页实体
            var pageInfo = this.GeneratePagingInfo(queryInfo);

            //获取备件仓库
            var whs = RT.Service.Resolve<WarehouseController>().GetWarehouseDataList(LibraryType.Entity,
                 queryInfo.Keyword, pageInfo);

            //构建返回数据
            var infos = new SparePartWhPagingResultInfo();
            infos.TotalCount = whs.TotalCount;
            infos.PageSize = pageInfo.PageSize;
            infos.PageNumber = pageInfo.PageNumber;

            //获取库存明细
            var storeSummary = Query<StoreSummary>().Where(m => m.SparePartId == queryInfo.SparePartId).FirstOrDefault();

            if (storeSummary != null)
            {
                var storeSummaryWarehouses = RT.Service.Resolve<SparePartController>()
                    .GetStoreSummaryWarehouseList(null, null, storeSummary);

                whs.ForEach(p =>
                {
                    var storeSummaryWarehouse = storeSummaryWarehouses
                        .FirstOrDefault(x => x.WarehouseId == p.Id);

                    if (storeSummaryWarehouse != null)
                    {
                        infos.SparePartWarehouseInfos.Add(new SparePartWarehouseInfo()
                        {
                            SparePartId = queryInfo.SparePartId,
                            WarehouseId = storeSummaryWarehouse.WarehouseId,
                            WarehouseCode = storeSummaryWarehouse.WarehouseCode,
                            WarehouseName = storeSummaryWarehouse.WarehouseName,
                            StoreQty = storeSummaryWarehouse.GoodNumber
                        });
                    }
                });
            }

            return infos;
        }

        /// <summary>
        /// 获取备件库存数据
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        [ApiService("获取备件库存数据")]
        [return: ApiReturn("备件库存数据 SparePartStorePagingResultInfo")]
        public virtual SparePartStorePagingResultInfo GetSparePartStoreData([ApiParameter("备件仓库分页参数")] SparePartDepotQueryInfo queryInfo)
        {
            //构建分页实体
            var pageInfo = this.GeneratePagingInfo(queryInfo);
            //控制器
            var ctl = RT.Service.Resolve<SparePartController>();
            //查询备件信息
            var sparePart = ctl.GetSparePart(queryInfo.SparePartId);

            var storeSummary = Query<StoreSummary>().Where(m => m.SparePartId == queryInfo.SparePartId).FirstOrDefault();

            int goodNumber = 0;
            if (storeSummary != null)
            {
                goodNumber = storeSummary.GoodNumber;
            }

            //构建返回数据
            var info = new SparePartStorePagingResultInfo()
            {
                TotalCount = -1,
                PageSize = pageInfo.PageSize,
                PageNumber = pageInfo.PageNumber,
                SparePartId = sparePart?.Id ?? 0,
                SparePartCode = sparePart?.SparePartCode,
                SparePartName = sparePart?.SparePartName,
                SparePartUnit = sparePart?.Unit?.Name,
                SparePartSpecification = sparePart?.Specification,
                SparePartTypeName = sparePart?.ItemCategory?.Name,
                EquipModelCode = sparePart?.SpartEquipModel?.Code,
                EquipModelName = sparePart?.SpartEquipModel?.Name,
                Manufacturer = sparePart?.Manufacturer,
                StoreTotalQty = goodNumber
            };

            if (storeSummary != null)
            {
                var storeSummaryWarehouses = RT.Service.Resolve<SparePartController>()
                    .GetStoreSummaryWarehouseList(null, null, storeSummary, sparePart);

                foreach (var storeSummaryWarehouse in storeSummaryWarehouses)
                {
                    info.SparePartWarehouseInfos.Add(new SparePartWarehouseInfo()
                    {
                        SparePartId = queryInfo.SparePartId,
                        WarehouseId = storeSummaryWarehouse.WarehouseId,
                        WarehouseCode = storeSummaryWarehouse.WarehouseCode,
                        WarehouseName = storeSummaryWarehouse.WarehouseName,
                        StoreQty = storeSummaryWarehouse.GoodNumber
                    });
                }

            }
            return info;
        }

        #region 备件信息接口
        /// <summary>
        /// 获取所有备件类型
        /// </summary>      
        /// <returns>备件类型列表</returns>
        [ApiService("获取所有备件类型")]
        [return: ApiReturn("备件类型列表")]
        public virtual List<BaseDataInfo> GetAllSparePartTypes()
        {
            List<BaseDataInfo> infos = new List<BaseDataInfo>();

            var list = RT.Service.Resolve<ItemController>()
                .GetItemSmallCategory(SIE.Items.Items.CategoryType.Item, ItemType.SparePart, String.Empty, null);

            list.ForEach(p =>
            {
                p.TreePId = null;
                infos.Add(new BaseDataInfo { Id = p.Id, Code = p.Code, Name = p.Name });
            });

            return infos;
        }

        /// <summary>
        /// 获取所有备件仓库
        /// </summary>      
        /// <returns>备件仓库列表</returns>
        [ApiService("获取所有备件仓库")]
        [return: ApiReturn("备件仓库列表")]
        public virtual List<BaseDataInfo> GetAllSparePartDepots()
        {
            List<BaseDataInfo> infos = new List<BaseDataInfo>();
            var depots = GetAll<Warehouse>();
            depots.ForEach(p => infos.Add(new BaseDataInfo { Id = p.Id, Code = p.Code, Name = p.Name }));
            return infos;
        }

        /// <summary>
        /// 根据类型和仓库，获取备件基础数据
        /// </summary>
        /// <param name="criteria">备件查询信息</param>
        /// <returns>备件基础数据</returns>
        [ApiService("根据类型和仓库，获取备件基础数据")]
        [return: ApiReturn("备件基础数据 TypeDepotSparePartData")]
        public virtual TypeDepotSparePartData GetTypeDepotSparePartData([ApiParameter("备件查询信息")] TypeDepotQueryInfo criteria)
        {
            var data = new TypeDepotSparePartData();
            var pagingInfo = new PagingInfo()
            {
                PageNumber = criteria.PageNumber.HasValue ? criteria.PageNumber.Value : 1,
                PageSize = criteria.PageSize.HasValue ? criteria.PageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            //获取备件
            var spareParts = GetSparePartByTypeDepot(pagingInfo, criteria);

            var partIds = spareParts.Select(p => p.Id).Distinct().ToList();

            //获取备件库存
            var allStoreSummarys = GetStoreSummarys(partIds);

            IList<SparePartWarehouseInfo> sparePartWarehouseInfos = new List<SparePartWarehouseInfo>();

            if (criteria.DepotId.HasValue)
            {
                sparePartWarehouseInfos = GetStoreSummaryDepots(partIds, new List<double> { criteria.DepotId.Value });
            }

            foreach (var sparePart in spareParts)
            {
                var info = new TypeDepotSparePartInfo();

                info.SparePartId = sparePart.Id;
                info.SparePartCode = sparePart.SparePartCode;
                info.SparePartName = sparePart.SparePartName;
                info.SparePartSpecification = sparePart.Specification;
                info.SparePartTypeName = sparePart.SparePartTypeName;

                if (criteria.DepotId.HasValue && sparePartWarehouseInfos != null && sparePartWarehouseInfos.Any())
                {
                    var depots = sparePartWarehouseInfos.Where(p => p.SparePartId == sparePart.Id
                            && p.WarehouseId == criteria.DepotId.Value).ToList();

                    info.StoreQty = depots.Sum(p => p.StoreQty);
                }
                else
                {
                    var goodNumber = allStoreSummarys
                        .Where(x => x.SparePartId == sparePart.Id)
                        .Sum(x => x.GoodNumber);

                    info.StoreQty = goodNumber;
                }

                info.IsLowerThan = info.StoreQty < sparePart.SafeStock;

                data.Data.Add(info);
            }

            if (criteria.DepotId.HasValue)
            {
                data.TotalCount = spareParts.Count;
            }
            else
            {
                data.TotalCount = spareParts.TotalCount;
            }

            return data;
        }

        /// <summary>
        /// 获取备件图片信息
        /// </summary>
        /// <param name="partIds"></param>
        /// <returns></returns>
        [ApiService("获取备件图片信息")]
        public virtual List<TypeDepotSparePartInfo> GetTypeDepotSparePartDataPic([ApiParameter("备件Id")] List<double> partIds)
        {
            List<TypeDepotSparePartInfo> typeDepotSparePartInfos = new List<TypeDepotSparePartInfo>();
            //获取图片
            var pics = GetSparePartPictureAttachments(partIds);
            foreach(var partId in partIds)
            {
                TypeDepotSparePartInfo typeDepotSparePartInfo = new TypeDepotSparePartInfo
                {
                    SparePartId = partId,
                };
                var pic = pics.FirstOrDefault(x => x.OwnerId == partId);

                if (pic != null)
                {
                    var picBase64Str = FileUrlHelper.GetAttachmentBase64StringData(pic.FilePath, pic.FileName);
                    typeDepotSparePartInfo.PhotoBase64 = picBase64Str;
                }
                typeDepotSparePartInfos.Add(typeDepotSparePartInfo);
            }
            return typeDepotSparePartInfos;
        }

        /// <summary>
        /// 根据备件id，获取备件详细信息
        /// </summary>
        /// <param name="sparePartId">备件id</param>
        /// <returns>备件详细信息</returns>
        [ApiService("根据备件id，获取备件详细信息")]
        [return: ApiReturn("备件详细信息 SparePartDetailInfo")]
        public virtual SparePartDetailInfo GetSparePartDetailInfo([ApiParameter("备件id")] double sparePartId)
        {
            var info = new SparePartDetailInfo();
            //获取备件
            var sparePart = GetSparePartById(sparePartId);

            if (sparePart == null)
                return info;
                        
            info.SparePartId = sparePart.Id;
            info.SparePartCode = sparePart.SparePartCode;
            info.SparePartName = sparePart.SparePartName;
            info.SparePartSpecification = sparePart.Specification;

            StringBuilder stringBuilder = new StringBuilder();
            List<ItemCategory> itemCategories = new List<ItemCategory>();
            GetItemCategoryRecursive(sparePart.ItemCategory, itemCategories);
            foreach (var itemCategory in itemCategories.Reverse<ItemCategory>())
            {
                stringBuilder.Append(itemCategory.Name);
                stringBuilder.Append("-");
            }

            info.SparePartTypeName = stringBuilder.ToString().TrimEnd('-');

            info.PartType = sparePart.SpartType.ToLabel().L10N();
            info.EquipType = sparePart.EquipTypeCode;
            info.EquipModel = sparePart.EquipModelCode;
            info.OriginalItemCode = sparePart.OriginalItemCode;
            info.Manufacturer = sparePart.Manufacturer;
            info.Supplier = sparePart.Supplier;

            info.SafeStock = sparePart.SafeStock;
            info.IsLowerThan = info.StoreQty < sparePart.SafeStock;

            info.LifeTime = sparePart.LifeTime;
            info.UseTime = sparePart.UseTime;
            info.UnitPrice = sparePart.UnitPrice;
            info.IsReplacement = sparePart.IsReplacement ? "是".L10N() : "否".L10N();
            info.UnitCode = sparePart.UnitCode;
            info.UnitName = sparePart.UnitName;
            info.IsSeqNoCharge = sparePart.ControlMethod.ToLabel().L10N();

            //获取备件库存
            var storeSummary = Query<StoreSummary>().Where(m => m.SparePartId == sparePartId).FirstOrDefault();

            if (storeSummary != null)
            {
                var storeSummaryStocks = RT.Service.Resolve<SparePartController>()
                    .GetStoreSummaryStockLotList(null, null, storeSummary, sparePart);

                var goodNumber = storeSummaryStocks.Sum(p => p.GoodNumber);
                info.StoreQty = goodNumber;

                foreach (var depot in storeSummaryStocks)
                {
                    var depotInfo = new SparePartDetailDepotInfo();
                    depotInfo.DepotCode = depot.WarehouseCode;
                    depotInfo.DepotName = depot.WarehouseName;
                    depotInfo.SiteCode = depot.StorageLocationCode;
                    depotInfo.SiteName = depot.StorageLocationName;
                    depotInfo.BatchNumber = depot.LotName;
                    depotInfo.GoodNumber = depot.GoodNumber;
                    depotInfo.RotNumber = depot.RotNumber;
                    info.Depots.Add(depotInfo);
                }
            }

            return info;
        }

        /// <summary>
        /// 获取备件Id
        /// </summary>
        /// <param name="sparePartId">备件Id</param>
        /// <returns></returns>
        [ApiService("根据备件id，获取备件图片信息")]
        public virtual List<string> GetSparePartDetailPics([ApiParameter("备件id")] double sparePartId)
        {
            List<string> base64Info = new List<string>();
            //获取图片
            var pics = GetSparePartPictureAttachments(new List<double> { sparePartId });
            foreach (var pic in pics)
            {
                var picBase64Str = FileUrlHelper.GetAttachmentBase64StringData(pic.FilePath, pic.FileName);
                base64Info.Add(picBase64Str);
            }
            return base64Info;
        }

        /// <summary>
        /// 提交备件申请单
        /// </summary>
        /// <param name="equipInfo">设备信息</param>
        /// <param name="spareList">备件明细</param>
        [ApiService("提交备件申请单")]
        public virtual void SubmitSpareApply([ApiParameter("设备信息")] EquipInfo equipInfo, [ApiParameter("备件明细")] List<SpareApplyInfo> spareList)
        {
            if (spareList.Count <= 0)
            {
                throw new ValidationException("备件明细不能为空".L10N());
            }
            else if (spareList.Any(p => p.WarehouseId == null))
            {
                throw new ValidationException("备件明细出库仓库必填".L10N());                
            }
            else if (spareList.Any(p => p.ApplyQty == null || p.ApplyQty <= 0))
            {
                throw new ValidationException("备件明细申请数量不能小于0".L10N());
            }

            //是否启用审核配置项
            bool enableAudit = false;
            var config = ConfigService.GetConfig(new ApprovalConfig(), typeof(SparePartApp));
            if (config != null)
            {
                enableAudit = config.EnableAudit;
            }

            // 创建申请单
            SparePartApp sparePartApp = new SparePartApp
            {
                No = RT.Service.Resolve<CommonController>().GetNo<SparePartApp>("备件申请单号".L10N()),
                FromType = Applys.Enums.FromType.Hand,
                DemandDate = DateTime.Now,
                AuditState = enableAudit ? Applys.Enums.AuditState.StandAudit : Applys.Enums.AuditState.Butbound,
                QualityStatus = QualityStatus.Good,
                EquipAccountId = equipInfo.EquipId,
                EquipModelId = equipInfo.EquipModelId,
                GetDepartmentId = equipInfo.UseDepartmentId,
            };
            // 创建申请单明细
            EntityList<ApplyDetail> applyDetails = new EntityList<ApplyDetail>();
            foreach(var spare in spareList)
            {
                ApplyDetail applyDetail = new ApplyDetail
                {
                    SparePartApp = sparePartApp,
                    SparePartId = spare.SptId,
                    ApplyAmount = spare.ApplyQty.Value,
                    WarehouseId = spare.WarehouseId.Value,
                };
                applyDetails.Add(applyDetail);
            }

            using(var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RT.Service.Resolve<CommonController>().BatchInsertSave(new EntityList<SparePartApp> { sparePartApp });
                applyDetails.ForEach(p => { p.SparePartAppId = p.SparePartApp.Id; });
                RT.Service.Resolve<CommonController>().BatchInsertSave(applyDetails);
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取上级分类
        /// </summary>
        /// <param name="itemCategory"></param>
        /// <param name="itemCategories">分类列表</param>
        /// <returns></returns>
        private void GetItemCategoryRecursive(ItemCategory itemCategory, List<ItemCategory> itemCategories)
        {
            if (itemCategory != null)
            {
                itemCategories.Add(itemCategory);

                if (itemCategory.TreePId != null && itemCategory.TreePId != 0)
                {
                    var prevItemCategory = RT.Service.Resolve<ItemController>().GetItemCategory(itemCategory.TreePId);

                    if (prevItemCategory != null)
                    {
                        GetItemCategoryRecursive(prevItemCategory, itemCategories);
                    }
                }
            }
        }
        #endregion

        #region Private

        /// <summary>
        /// 生成查询实体
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        private PagingInfo GeneratePagingInfo(PagingKeywordQueryInfo queryInfo)
        {
            if (queryInfo.PageSize <= 0)
                throw new ValidationException("[每页数据量]必须大于0".L10N());
            if (queryInfo.PageNumber <= 0)
                throw new ValidationException("[页码]必须大于0".L10N());

            //构建分页实体
            var pageInfo = new PagingInfo()
            {
                PageSize = queryInfo.PageSize.Value,
                PageNumber = queryInfo.PageNumber.Value,
                IsNeedCount = true
            };

            return pageInfo;
        }

        #endregion
    }
}
