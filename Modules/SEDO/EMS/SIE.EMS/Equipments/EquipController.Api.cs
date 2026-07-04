using SIE.Api;
using SIE.Common.Catalogs;
using SIE.Core.ApiModels;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Common.Utils;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.ApiModels;
using SIE.EMS.Equipments.Boms;
using SIE.Equipments;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.Resources.WipResources;
using SIE.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Equipments
{
    /// <summary>
    /// 设备控制器API
    /// </summary>
    public partial class EquipController : DomainController
    {
        #region PCB套件新接口

        /// <summary>
        /// 获取设备台账信息
        /// </summary>
        /// <param name="queryInfo">设备查询信息</param>
        /// <returns></returns>
        [ApiService("获取设备台账信息")]
        [return: ApiReturn("分页设备信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetEquipAccountInfo([ApiParameter("设备查询信息")] EquipAccountQueryInfo queryInfo)
        {
            //构建分页实体
            var pageInfo = this.GeneratePagingInfo(queryInfo);

            //查询维修单数据
            var equips = RT.Service.Resolve<EquipController>().GetEquipAccounts(pageInfo, queryInfo.EquipModelId, queryInfo.EquipTypeId, queryInfo.Keyword);

            //构建返回实体
            var info = new PagingBaseDataInfo();
            info.PageNumber = queryInfo.PageNumber;
            info.PageSize = queryInfo.PageSize;
            info.TotalCount = equips.TotalCount;
            equips.ForEach(p =>
            {
                info.DataInfos.Add(new BaseDataInfo()
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name
                });
            });

            return info;
        }

        /// <summary>
        /// 获取设备型号信息
        /// </summary>
        /// <param name="queryInfo">型号查询信息</param>
        /// <returns></returns>
        [ApiService("获取设备型号信息")]
        [return: ApiReturn("分页设备型号信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetEquipModelInfo([ApiParameter("型号查询信息")] EquipModelQueryInfo queryInfo)
        {
            //构建分页实体
            var pageInfo = this.GeneratePagingInfo(queryInfo);

            //查询维修单数据
            var models = RT.Service.Resolve<EquipController>().GetEquipModels(pageInfo, queryInfo.EquipTypeId, queryInfo.Keyword);

            //构建返回实体
            var info = new PagingBaseDataInfo();
            info.PageNumber = queryInfo.PageNumber;
            info.PageSize = queryInfo.PageSize;
            info.TotalCount = models.TotalCount;
            models.ForEach(p =>
            {
                info.DataInfos.Add(new BaseDataInfo()
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name
                });
            });

            return info;
        }

        /// <summary>
        /// 获取设备类型信息
        /// </summary>
        /// <param name="queryInfo">类型查询信息</param>
        /// <returns></returns>
        [ApiService("获取设备类型信息")]
        [return: ApiReturn("分页设备型号信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetEquipTypeInfo([ApiParameter("类型查询信息")] PagingKeywordQueryInfo queryInfo)
        {
            //构建分页实体
            var pageInfo = this.GeneratePagingInfo(queryInfo);

            //查询维修单数据
            var models = RT.Service.Resolve<SIE.Equipments.CoreEquipController>().GetEquipTypes(pageInfo, queryInfo.Keyword);

            //构建返回实体
            var info = new PagingBaseDataInfo();
            info.PageNumber = queryInfo.PageNumber;
            info.PageSize = queryInfo.PageSize;
            info.TotalCount = models.TotalCount;
            models.ForEach(p =>
            {
                info.DataInfos.Add(new BaseDataInfo()
                {
                    Id = p.Id,
                    Code = p.TypeCode,
                    Name = p.TypeName
                });
            });

            return info;
        }

        #endregion


        /// <summary>
        /// 根据产线获取设备台账列表
        /// </summary>
        /// <param name="queryInfo">设备查询信息</param>
        /// <returns>分页设备信息</returns>
        [ApiService("根据产线获取设备台账列表")]
        [return: ApiReturn("分页设备信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetPagingEquipAccountInfos([ApiParameter("设备查询信息")] EquipQueryInfo queryInfo)
        {
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            var equips = GetEquipAccountsByResourceId(queryInfo.ResourceId, queryInfo.Keyword, pagingInfo);
            var infos = new List<BaseDataInfo>();
            equips.ForEach(workshop =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = workshop.Id,
                    Code = workshop.Code,
                    Name = workshop.Name
                });
            });
            //取对应父产线的设备
            var resource = RF.GetById<WipResource>(queryInfo.ResourceId);
            if (resource != null && resource.ParentResourceId.HasValue)
            {
                var parentEquips = GetEquipAccountsByResourceId(resource.ParentResourceId.Value, queryInfo.Keyword, pagingInfo);
                parentEquips.ForEach(workshop =>
                {
                    infos.Add(new BaseDataInfo()
                    {
                        Id = workshop.Id,
                        Code = workshop.Code,
                        Name = workshop.Name
                    });
                });
            }
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = equips.TotalCount
            };
            result.DataInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 根据产线获取设备台账列表
        /// </summary>
        /// <param name="queryInfo">设备查询信息</param>
        /// <returns>分页设备信息</returns>
        [ApiService("根据产线获取设备台账列表")]
        [return: ApiReturn("分页设备信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetEquipAccountInfos([ApiParameter("设备查询信息")] EquipQueryInfo queryInfo)
        {
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            var equips = GetEquipAccountsByResourceId(queryInfo.ResourceId, queryInfo.Keyword, pagingInfo);
            if (equips.Count <= 0)
                equips = GetEquipAccounts(pagingInfo, queryInfo.Keyword);

            var infos = new List<BaseDataInfo>();
            equips.ForEach(workshop =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = workshop.Id,
                    Code = workshop.Code,
                    Name = workshop.Name
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = equips.TotalCount
            };
            result.DataInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 获取SCADA设备列表
        /// </summary> 
        /// <returns>SCADA设备列表</returns>
        [ApiService("获取SCADA设备列表")]
        [return: ApiReturn("SCADA设备列表 List<ScadaEquipInfo>", SampleValueProvider = typeof(ScadaEquipInfoValueProvider))]
        [AllowAnonymous]
        public virtual List<ScadaEquipInfo> GetScadaEquipInfos()
        {
            var equips = RT.Service.Resolve<CommonController>().GetDatas<ScadaEquipAccount>(null);
            List<ScadaEquipInfo> result = new List<ScadaEquipInfo>();
            equips.ForEach(e =>
            {
                result.Add(new ScadaEquipInfo()
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    EquipModelId = e.EquipModelId,
                    EquipModelCode = e.ModelCode,
                    EquipModelName = e.ModelName,
                    ResourceId = e.ResourceId,
                    InvOrgId = e.InvOrgId
                });
            });
            return result;
        }

        /// <summary>
        /// 根据用户获取设备信息列表
        /// </summary>
        /// <returns>用户设备信息列表</returns>
        [ApiService("根据用户获取设备信息列表")]
        [return: ApiReturn("用户设备信息列表 List<EquipTabInfo>")]
        public virtual List<EquipTabInfo> GetEquipTabInfos([ApiParameter("设备查询信息")] EquipTabQueryInfo criteria)
        {
            var query = Query<EquipAccount>();
            if (criteria.Key.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(criteria.Key) || p.Name.Contains(criteria.Key));
            }

            if (criteria.EquipTypeId.HasValue)
            {
                query.Join<EquipModel>((d, e) => d.EquipModelId == e.Id && e.EquipTypeId == criteria.EquipTypeId);
            }

            if (criteria.WipResource.IsNotEmpty())
            {
                query.Where(p => p.WorkShop.Code == criteria.WipResource);
            }

            if (criteria.State.HasValue)
            {
                query.Where(x =>   x.State == criteria.State.Value);
            }

            //获取设备
            var equips = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var infos = new List<EquipTabInfo>();

            var equipIds = equips.Select(p => (double?)p.Id).ToList();

            foreach (var equip in equips)
            {
                var info = new EquipTabInfo();
                info.Id = equip.Id;
                info.Code = equip.Code;
                info.Name = equip.Name;
                info.AssetCode = equip.AssetCode;
                info.UseDepartment = equip.UseDepartmentName;

                if (equip.WorkShopName.IsNotEmpty())
                {
                    info.WorkShop = "(" + equip.WorkShopName + ")" + equip.ResourceName;
                }
                else
                {
                    info.WorkShop = equip.ResourceName;
                }

                //去掉存储位置，只保留安装位置，并重命名为“位置”
                info.Location = equip.InstallationLocation;

                info.AccountState = equip.State.ToLabel().L10N();
                info.AccountStateValue = equip.State;
                info.IOTState = equip.EquipOnLineState.ToLabel().L10N();
                info.IOTStateValue = equip.EquipOnLineState;

                infos.Add(info);
            }
            return infos;
        }

        /// <summary>
        /// 获取设备logo图片
        /// </summary>
        /// <param name="equipIds">设备Ids</param>
        /// <returns></returns>
        [ApiService("根据用户获取设备信息列表图片")]
        [return: ApiReturn("用户设备信息列表图片 List<EquipTabInfo>")]
        public virtual List<EquipTabInfo> GetEquipTabInfoPics(IEnumerable<double> equipIds)
        {
            List<EquipTabInfo> equipTabInfos = new List<EquipTabInfo>();
            var exts = new List<string> { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".webp", ".psd", ".svg", ".tiff", ".jfif" };
            // 设备logo
            var equipLogos = equipIds.SplitContains(ids =>
            {
                return Query<EquipAccountAttachment>().Where(p => p.OwnerId != null && ids.Contains((double)p.OwnerId) && exts.Contains(p.FileExtesion) && p.IsEquipLogo == true).ToList();
            });
            foreach (var logo in equipLogos)
            {
                EquipTabInfo equipTabInfo = new EquipTabInfo
                {
                    Id = logo.OwnerId.Value,
                };
                equipTabInfo.Picture = FileUrlHelper.GetAttachmentBase64StringData(logo.FilePath, logo.FileName);
                equipTabInfos.Add(equipTabInfo);
            }
            return equipTabInfos;
        }

        /// <summary>
        /// 根据id获取设备详细信息
        /// </summary>
        /// <param name="equipId">设备id</param>
        /// <returns>设备详细信息</returns>
        [ApiService("根据id获取设备台账详细信息")]
        [return: ApiReturn("设备详细信息 EquipAccountTabInfo")]
        public virtual EquipAccountTabInfo GetEquipAccountTabInfo([ApiParameter("设备id")] double equipId)
        {
            var info = new EquipAccountTabInfo();
            var equip = RT.Service.Resolve<EquipController>().GetEquipAccount(p => p.Id == equipId);
            if (equip != null)
            {
                info.EquipModelCode = equip.ModelCode;
                info.EquipTypeCode = equip.EquipTypeCode;
                info.UseLevel = equip.UseLevel;
                //info.KeyEquip = equip.IsKeyEquip.ToLabel();
                info.IsVirtual = equip.IsVirtual.ToLabel();
                info.IndustryCategory = equip.IndustryCategory.ToLabel();
                info.UseState = equip.UseState.ToLabel();
                info.Proprietorship = equip.Proprietorship.ToLabel();
                if (equip.EnterDate.HasValue)
                    info.EnterDate = equip.EnterDate.ToString();
                info.SupplierName = equip.SupplierName;
                info.PurchaseUnit = equip.PurchaseUnit;
                info.Manufacturer = equip.Manufacturer;
                // info.AssetOriginalValue = equip.AssetOriginalValue;
                // info.AssetNetValue = equip.AssetNetValue;
                info.UsefulLife = equip.UsefulLife != null ? equip.UsefulLife.Value : 0;
                if (equip.WarrantyPeriod.HasValue)
                    info.WarrantyPeriod = equip.WarrantyPeriod.ToString();
                info.InstallationLocation = equip.InstallationLocation;
                info.Location = equip.InstallationLocation;
            }
            return info;
        }

        /// <summary>
        /// 获取设备类型列表
        /// </summary>      
        /// <returns>设备类型列表</returns>
        [ApiService("获取设备类型列表")]
        [return: ApiReturn("设备类型列表")]
        public virtual List<BaseDataInfo> GetAllEquipTypes()
        {
            List<BaseDataInfo> infos = new List<BaseDataInfo>();
            var equipTypes = GetAll<EquipType>();
            equipTypes.ForEach(p => infos.Add(new BaseDataInfo { Id = p.Id, Code = p.TypeCode, Name = p.TypeName }));
            return infos;
        }

        /// <summary>
        /// 获取设备类别列表
        /// </summary>      
        /// <returns>设备类别列表</returns>
        [ApiService("获取设备类别列表")]
        [return: ApiReturn("设备类别列表")]
        public virtual List<BaseDataInfo> GetEquipTypeCatalogTypes([ApiParameter("查询信息")] PagingKeywordQueryInfo queryInfo)
        {
            var pagingInfo = new PagingInfo()
            {
                PageNumber = queryInfo.PageNumber.HasValue ? queryInfo.PageNumber.Value : 1,
                PageSize = queryInfo.PageSize.HasValue ? queryInfo.PageSize.Value : int.MaxValue - 1
            };
            List<BaseDataInfo> infos = new List<BaseDataInfo>();
            var catalogType = RT.Service.Resolve<CatalogController>().GetCatalogList(EquipType.EquipTypeCatalogType, pagingInfo, queryInfo.Keyword);
            catalogType.ForEach(p => infos.Add(new BaseDataInfo { Id = p.Id, Code = p.Code, Name = p.Name }));
            return infos;
        }

        /// <summary>
        /// 获取设备ABC分类列表
        /// </summary>      
        /// <returns>设备ABC分类列表</returns>
        [ApiService("获取设备ABC分类列表")]
        [return: ApiReturn("设备ABC分类列表")]
        public virtual List<BaseDataInfo> GetEquipUseLevels([ApiParameter("查询信息")] PagingKeywordQueryInfo queryInfo)
        {
            var pagingInfo = new PagingInfo()
            {
                PageNumber = queryInfo.PageNumber.HasValue ? queryInfo.PageNumber.Value : 1,
                PageSize = queryInfo.PageSize.HasValue ? queryInfo.PageSize.Value : int.MaxValue - 1
            };
            List<BaseDataInfo> infos = new List<BaseDataInfo>();
            var catalogType = RT.Service.Resolve<CatalogController>().GetCatalogList(EquipAccount.EquipAccountUseLevel, pagingInfo, queryInfo.Keyword);
            catalogType.ForEach(p => infos.Add(new BaseDataInfo { Id = p.Id, Code = p.Code, Name = p.Name }));
            return infos;
        }

        /// <summary>
        /// 获取设备类别下的设备类型
        /// </summary>      
        /// <returns>设备类别下的设备类型</returns>
        [ApiService("获取设备类别下的设备类型")]
        [return: ApiReturn("设备类别下的设备类型")]
        public virtual EntityList<EquipType> GetCategoryEquipTypes([ApiParameter("查询信息")] PagingKeywordQueryInfo queryInfo, [ApiParameter("设备类别")] string category)
        {
            var pagingInfo = new PagingInfo()
            {
                PageNumber = queryInfo.PageNumber.HasValue ? queryInfo.PageNumber.Value : 1,
                PageSize = queryInfo.PageSize.HasValue ? queryInfo.PageSize.Value : int.MaxValue - 1
            };
            if (!category.IsNullOrWhiteSpace())
            {
                return RT.Service.Resolve<CoreEquipController>().GetEquipTypes(category, pagingInfo, queryInfo.Keyword);
            }
            else
            {
                return RT.Service.Resolve<CoreEquipController>().GetEquipTypes(pagingInfo, queryInfo.Keyword);
            }
        }

        /// <summary>
        /// 获取设备类型和设备类别下的设备型号
        /// </summary>      
        /// <returns>设备类型和设备类别下的设备型号</returns>
        [ApiService("获取设备类型和设备类别下的设备型号")]
        [return: ApiReturn("设备类型和设备类别下的设备型号")]
        public virtual EntityList<EquipModel> GetEquipModelsByTypeAndCategory([ApiParameter("查询信息")] PagingKeywordQueryInfo queryInfo
            , [ApiParameter("设备类别")] string category, [ApiParameter("设备类型id")] double? equipTypeId)
        {
            var pagingInfo = new PagingInfo()
            {
                PageNumber = queryInfo.PageNumber.HasValue ? queryInfo.PageNumber.Value : 1,
                PageSize = queryInfo.PageSize.HasValue ? queryInfo.PageSize.Value : int.MaxValue - 1
            };
            if (equipTypeId.HasValue)
            {
                return RT.Service.Resolve<EquipController>().GetEquipModels(pagingInfo, equipTypeId, queryInfo.Keyword);
            }
            if (!category.IsNullOrWhiteSpace())
            {
                return RT.Service.Resolve<EquipController>().GetEquipModelsOfType(category, queryInfo.Keyword, pagingInfo);
            }
            return RT.Service.Resolve<EquipController>().GetEquipModels(pagingInfo, null, queryInfo.Keyword);
        }

        /// <summary>
        /// 根据设备编码获取设备BOM详细信息
        /// </summary>
        /// <param name="code">设备编码</param>
        /// <returns>设备BOM详细信息</returns>
        [ApiService("根据设备编码获取设备BOM详细信息")]
        [return: ApiReturn("设备BOM详细信息 List<EquipTabBomInfo>")]
        public virtual List<EquipTabBomInfo> GetEquipTabBomInfos([ApiParameter("设备编码")] string code)
        {
            var boms = new List<EquipTabBomInfo>();
            var equip = RT.Service.Resolve<EquipController>().GetEquipAccountsByCode(code);
            if (equip != null)
            {
                var equipBomDetails = RT.Service.Resolve<EquipBomController>().GetEquipBomDetailsByModelId(equip.EquipModelId, null, null);
                var rootDetails = equipBomDetails.Where(p => p.TreePId == null || p.TreePId == 0).ToList();
                foreach (var equipBomDetail in rootDetails)
                {
                    var bom = new EquipTabBomInfo();
                    bom.BomId = equipBomDetail.Id;
                    bom.SparePartCode = equipBomDetail.SparePartCode;
                    bom.SparePartName = equipBomDetail.SparePartName;
                    bom.Specification = equipBomDetail.Specification;
                    bom.SparePartQty = equipBomDetail.SparePartQty;
                    bom.StockQty = equipBomDetail.StockQty;
                    boms.Add(bom);
                    CalculateChildren(bom, equipBomDetails);
                }
            }
            return boms;
        }

        /// <summary>
        /// 计算子bom
        /// </summary>
        /// <param name="parentInfo">父bom</param>
        /// <param name="details">设备bom明细</param>
        private void CalculateChildren(EquipTabBomInfo parentInfo, EntityList<EquipBomDetail> details)
        {
            var children = details.Where(p => p.TreePId == parentInfo.BomId).ToList();
            foreach (var equipBomDetail in children)
            {
                var bom = new EquipTabBomInfo();
                bom.BomId = equipBomDetail.Id;
                bom.SparePartCode = equipBomDetail.SparePartCode;
                bom.SparePartName = equipBomDetail.SparePartName;
                bom.Specification = equipBomDetail.Specification;
                bom.SparePartQty = equipBomDetail.SparePartQty;
                bom.StockQty = equipBomDetail.StockQty;
                parentInfo.Children.Add(bom);
                CalculateChildren(bom, details);
            }
        }

        /// <summary>
        /// 根据设备编码或型号编码返回信息
        /// </summary>
        /// <param name="key">查询关键字</param>
        /// <returns></returns>
        [ApiService("根据设备编码或型号编码返回信息")]
        [return: ApiReturn("设备信息或型号信息")]
        public virtual EquipInfo GetEquipInfoByKey([ApiParameter("查询关键字")] string key)
        {
            var equipList = Query<EquipAccount>().LeftJoin<EquipModel>((ea, em) => ea.EquipModelId == em.Id).Where(p => p.Code.Contains(key)).Select<EquipModel>((ea, em) => new
            {
                EquipId = ea.Id,
                Code = ea.Code,
                Name = ea.Name,
                EquipModelId = ea.EquipModelId,
                EquipModelCode = em.Code,
                UseDepartmentId = ea.UseDepartmentId,
            }).ToList<EquipInfo>();
            if (equipList.Count <= 0)
            {
                var equipModelList = Query<EquipModel>().Where(p => p.Code.Contains(key)).Select(p => new
                {
                    EquipModelId = p.Id,
                    EquipModelCode = p.Code,
                }).ToList<EquipInfo>();
                if (equipModelList.Count <= 0)
                {
                    throw new ValidationException("未找到对应设备或设备型号".L10N());
                }
                else
                {
                    return equipModelList.FirstOrDefault();
                }
            }
            else
            {
                return equipList.FirstOrDefault();
            }
        }

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

    /// <summary>
    /// SCADA设备值提供器
    /// </summary>
    class ScadaEquipInfoValueProvider : IApiSampleValueProvider
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns>值</returns>
        public object GetValue()
        {
            return new ScadaEquipInfo();
        }
    }
}