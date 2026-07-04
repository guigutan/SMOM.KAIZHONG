using SIE.Common.Catalogs;
using SIE.Common.Configs;
using SIE.Common.Import;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Equipments.EquipmentCards.ImportEquipmentCard
{
    /// <summary>
    /// 导入设备立卡帮助类
    /// </summary>

    public class ImportEquipmentCardHandle
    {

        /// <summary>
        /// 旧设备立卡数据
        /// </summary>
        private EntityList<EquipmentCard> OldEquipmentCardList { get; set; }

        /// <summary>
        /// ABC分类
        /// </summary>
        EntityList<Catalog> UseLevelList { get; set; }

        /// <summary>
        /// 企业模型
        /// </summary>
        private EntityList<Enterprise> EnterpriseList { get; set; }

        /// <summary>
        /// 扩展缓存
        /// </summary>
        private CacheData Cache { get; set; }
        
        //使用部门与工厂 key:工厂 value：使用部门
        Dictionary<double, List<double>> UseDepartAndFactoryDic { get; set; }

        //管理部门与工厂 key:工厂 value：管理部门
        Dictionary<double, List<double>> ManagementAndFactoryDic { get; set; }

        //产线与车间 key:车间id value：产线id
        Dictionary<double, List<double>> ResourceAndWorkShopDic { get; set; }

        /// <summary>
        /// 库位字段集合 key：仓库 value : 
        /// </summary>
        Dictionary<double, List<StorageLocation>> StorageLocationDic { get; set; }

        /// <summary>
        /// 当前用户的所有工厂权限
        /// </summary>
        List<double> FactoryIds { get; set; }
        /// <summary>
        /// 当前用户的所有仓库权限
        /// </summary>
        List<double> WarehouseIds { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ImportEquipmentCardHandle()
        {
            Cache = new CacheData();
            

            EnterpriseList = new EntityList<Enterprise>();
            OldEquipmentCardList = new EntityList<EquipmentCard>();
           
            UseLevelList = new EntityList<Catalog>();

            UseDepartAndFactoryDic = new Dictionary<double, List<double>>();
            ManagementAndFactoryDic = new Dictionary<double, List<double>>();
            ResourceAndWorkShopDic = new Dictionary<double, List<double>>();
            StorageLocationDic = new Dictionary<double, List<StorageLocation>>();

            FactoryIds = new List<double>();
            WarehouseIds = new List<double>();
        }

        /// <summary>
        /// 导入设备立卡主入口
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<ImportMessageResult> ImportEquipmentCard(IList<RowData> data)
        {
            List<ImportMessageResult> importMessageResults = new List<ImportMessageResult>();

            //加载数据
            LoadData(data, importMessageResults);

            List<ApprovalStatus> status = new List<ApprovalStatus>() { ApprovalStatus.Draft, ApprovalStatus.Reject, ApprovalStatus.Audited };

            var assetConfig = ConfigService.GetConfig<EquipAccountAssetConfigValue>(new EquipAccountAssetConfig(), typeof(EquipAccount));

            //加载数据
            foreach (var row in data)
            {
                try
                {
                    var card = row.Entity as EquipmentCard;
                    //校验设备编码存在设备立卡时，校验审核状态是否为【待提交】、【驳回】、【通过】
                    if (card.PersistenceStatus != PersistenceStatus.New && card.HasId && !status.Contains(card.ApprovalStatus))
                    {
                        throw new ValidationException("导入已存在数据的审核状态必须为【{0}】".L10nFormat(card.ApprovalStatus.ToLabel()));
                    }
                    //非空验证
                    NoNullCheckO(card);

                    //固定资产
                    AssetNullCheck(card, assetConfig);

                    //权限验证
                    PermisCheck(card);

                    using (var trans = DB.TransactionScope(EquipmentEntityDataProvider.ConnectionStringName))
                    {
                        // 判断设备编码是否存在，区分本条数据是新增还是更新
                        if (card.PersistenceStatus != PersistenceStatus.New && card.HasId)
                        {
                            //更新时：
                            //所有字段与现有数据无差时，不需要操作
                            //立卡日期为空时，保存修改的字段
                            //立卡日期不为空时，且审核状态为【待提交】、【驳回】时，保存修改的字段，并生成修改记录
                            //立卡日期不为空，且审核状态为【通过】时，
                            //①保存修改的字段，
                            //②清空原修改记录并生成新的修改记录，
                            //③审核状态更新为【待提交】并清空审核记录，
                            //④更新【修改标识】字段为【是】

                            //取旧的设备立卡的状态判断
                            var oldCard = OldEquipmentCardList.FirstOrDefault(p => p.Code == card.Code);
                            //立卡日期不为空时，
                            if (oldCard.CreateCardDateTime.HasValue)
                            {
                                //且审核状态为【待提交】、【驳回】时，保存修改的字段，并生成修改记录
                                //保存修改的字段，并生成修改记录， 
                                EntityLogHelper.CreateEntityLog(typeof(EquipmentCard), card, oldCard);
                                //且审核状态为【通过】时
                                if (oldCard.ApprovalStatus == ApprovalStatus.Audited)
                                {
                                    //①保存修改的字段，
                                    //②清空原修改记录并生成新的修改记录，
                                    //③审核状态更新为【待提交】并清空审核记录
                                    card.ApprovalStatus = ApprovalStatus.Draft;
                                    // ④更新【修改标识】字段为【是】
                                    card.IsChange = true;
                                }
                            }
                            //保存设备立卡
                            RF.Save(card);
                        }
                        else
                        {
                            //表示新增
                            card.ApprovalStatus = ApprovalStatus.Draft;
                            card.EquipmentCardSource = EquipmentCardSource.Manual;
                            card.NeedAcceptance = true;
                            //保存设备立卡
                            RF.Save(card);
                        }

                        trans.Complete();
                    }

                    importMessageResults.Add(new ImportMessageResult
                    {
                        RowNum = row.RowIndex + 1,
                        MsgType = ImportMessageType.SaveSucess,
                        Message = "保存成功！".L10N()
                    });
                }
                catch (Exception exc)
                {
                    importMessageResults.Add(new ImportMessageResult
                    {
                        RowNum = row.RowIndex + 1,
                        MsgType = ImportMessageType.SaveFail,
                        Message = exc.GetBaseException().Message
                    });
                }
            }

            return importMessageResults;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="importMessageResults"></param>
        private void LoadData(IList<RowData> data, List<ImportMessageResult> importMessageResults)
        {
            //获取所有的库存组织信息
            EnterpriseList = RT.Service.Resolve<EnterpriseController>().GetEnterpriseAll();

            //导入设备立卡所有的库位编码
            List<string> storageLocationCodes = new List<string>();

            //导入设备立卡的旧数据设备立卡编码(设备编码)
            List<string> equipmentCardCodes = new List<string>();

            List<double> outList;
            foreach (var rowData in data)
            {
                var card = rowData.Entity as EquipmentCard;


                storageLocationCodes.Add(card.StorageLocationCode);

                if (card.PersistenceStatus != PersistenceStatus.New && card.HasId)
                {
                    equipmentCardCodes.Add(card.Code);
                }

               

                //验证企业模型相关字段
                var isOk = ValidationEnterprise(importMessageResults, rowData, card);

                if (!isOk)
                {
                    continue;
                }

                //使用部门与工厂
                //使用部门不为空,当前设备立卡的使用部门在当前设备立卡的车间下,记录
                //不在车间下直接不记录，判断时不存在则不在此车间下。
                if (card.UseDepartment != null && card.UseDepartment.TreePId == card.FactoryId)
                {
                    //缓存中是否已包含此工厂
                    if (UseDepartAndFactoryDic.TryGetValue(card.FactoryId, out outList))
                    {
                        //此工厂的使用部门是否已缓存此使用部门
                        if (!outList.Any(p => p == card.UseDepartmentId))
                        {
                            outList.Add((double)card.UseDepartmentId);
                        }
                    }
                    else
                    {
                        UseDepartAndFactoryDic.Add(card.FactoryId, new List<double>() {
                            (double)card.UseDepartmentId
                        });
                    }
                }

                //管理部门与工厂
                if (card.Management != null && card.Management.TreePId == card.FactoryId)
                {
                    if (ManagementAndFactoryDic.TryGetValue(card.FactoryId, out outList))
                    {
                        if (!outList.Any(p => p == card.ManagementId))
                        {
                            outList.Add((double)card.ManagementId);
                        }
                    }
                    else
                    {
                        ManagementAndFactoryDic.Add(card.FactoryId, new List<double>() {
                            (double)card.ManagementId
                        });
                    }
                }

                //产线与车间
                if (card.WorkShopId != null && card.ResourceId != null && card.Resource.TreePId == card.WorkShopId)
                {
                    if (ResourceAndWorkShopDic.TryGetValue((double)card.WorkShopId, out outList))
                    {
                        if (!outList.Any(p => p == card.ResourceId))
                        {
                            outList.Add((double)card.ResourceId);
                        }
                    }
                    else
                    {
                        ResourceAndWorkShopDic.Add((double)card.WorkShopId, new List<double>() {
                            (double)card.ResourceId
                        });
                    }
                }

            }

            //当前用户所有的工厂权限
            FactoryIds = RT.Service.Resolve<EmployeeEnterpriseSelectController>().GetAuthorityFactoryId();


            //当前用户所有的仓库权限
            WarehouseIds = RT.Service.Resolve<WarehouseController>().GetAuthorityWarehouseId();

            //按照库位Code加载全部库位数据
            storageLocationCodes = storageLocationCodes.Distinct().ToList();

            StorageLocationDic = RT.Service.Resolve<WarehouseController>().GetStorageLocationByCodes(storageLocationCodes, null)
                .GroupBy(p => p.WarehouseId)
                .ToDictionary(p => p.Key, p => p.ToList());

            //取设备立卡已存在的旧数据集合
            equipmentCardCodes = equipmentCardCodes.Distinct().ToList();

            //设备台账数据更新集合
            OldEquipmentCardList = RT.Service.Resolve<EquipmentCardController>().GetEquipmentCardByCode(equipmentCardCodes);

            //加载ABC分类的数据
            UseLevelList = RT.Service.Resolve<CatalogController>().GetCatalogList(EquipAccount.EquipAccountUseLevel);
        }

        private bool ValidationEnterprise(List<ImportMessageResult> importMessageResults, RowData rowData, EquipmentCard card)
        {
            bool isOk = true;
            var factory = EnterpriseList.FirstOrDefault(x => x.Code == card.FactoryCode && x.LevelType == EnterpriseType.Plant);
            if (factory == null)
            {
                importMessageResults.Add(new ImportMessageResult
                {
                    RowNum = rowData.RowIndex + 1,
                    MsgType = ImportMessageType.SaveFail,
                    Message = "工厂编码不存在".L10N()
                });

                isOk = false;
            }
            else
            {
                card.Factory = factory;
                card.FactoryId = factory.Id;
            }

            //使用部门

            if (!card.UseDepartmentCode.IsNullOrEmpty())
            {
                var useDepartment = EnterpriseList.FirstOrDefault(x => x.Code == card.UseDepartmentCode && x.LevelType == EnterpriseType.Department);
                if (useDepartment == null)
                {
                    importMessageResults.Add(new ImportMessageResult
                    {
                        RowNum = rowData.RowIndex + 1,
                        MsgType = ImportMessageType.SaveFail,
                        Message = "使用部门编码不存在".L10N()
                    });

                    isOk = false;
                }
                else
                {
                    card.UseDepartment = useDepartment;
                    card.UseDepartmentId = useDepartment.Id;
                }
            }

            //管理部门
            if (!card.ManagementCode.IsNullOrEmpty())
            {
                var managementDepartment = EnterpriseList.FirstOrDefault(x => x.Code == card.ManagementCode && x.LevelType == EnterpriseType.Department);
                if (managementDepartment == null)
                {
                    importMessageResults.Add(new ImportMessageResult
                    {
                        RowNum = rowData.RowIndex + 1,
                        MsgType = ImportMessageType.SaveFail,
                        Message = "管理部门编码不存在".L10N()
                    });

                    isOk = false;
                }
                else
                {
                    card.Management = managementDepartment;
                    card.ManagementId = managementDepartment.Id;
                }
            }

            //车间
            if (!card.WorkShopCode.IsNullOrEmpty())
            {
                var workShop = EnterpriseList.FirstOrDefault(x => x.Code == card.WorkShopCode && x.LevelType == EnterpriseType.Shop);
                if (workShop == null)
                {
                    importMessageResults.Add(new ImportMessageResult
                    {
                        RowNum = rowData.RowIndex + 1,
                        MsgType = ImportMessageType.SaveFail,
                        Message = "车间编码不存在".L10N()
                    });

                    isOk = false;
                }
                else
                {
                    card.WorkShop = workShop;
                    card.WorkShopId = workShop.Id;
                }
            }


            //产线
            if (!card.ResourceCode.IsNullOrEmpty())
            {
                var resource = EnterpriseList.FirstOrDefault(x => x.Code == card.UseDepartmentCode && x.LevelType == EnterpriseType.Line);
                if (resource == null)
                {
                    importMessageResults.Add(new ImportMessageResult
                    {
                        RowNum = rowData.RowIndex + 1,
                        MsgType = ImportMessageType.SaveFail,
                        Message = "产线编码不存在".L10N()
                    });

                    isOk = false;
                }
                else
                {
                    card.Resource = resource;
                    card.ResourceId = resource.Id;
                }
            }

            return isOk;
        }

        /// <summary>
        /// 非空验证
        /// </summary>
        protected void NoNullCheckO(EquipmentCard card)
        {
            //设备台账启用固定资产配置

            if (card != null)
            {
                //ABC分类验证
                if (UseLevelList.FirstOrDefault(p => p.Code == card.UseLevel) == null)
                {
                    throw new ValidationException("ABC分类未在ABC分类快码中维护".L10N());
                }

                if (card.FactoryCode == null)
                {
                    throw new ValidationException("工厂不能为空".L10N());
                }
                if (card.ManagementCode == null)
                {
                    throw new ValidationException("管理部门不能为空".L10N());
                }
                if (!card.Code.IsNotEmpty())
                {
                    throw new ValidationException("设备编码不能为空".L10N());
                }
                if (!card.Name.IsNotEmpty())
                {
                    throw new ValidationException("设备名称不能为空".L10N());
                }
                if (card.EquipModel == null)
                {
                    throw new ValidationException("型号不能为空".L10N());
                }
                if (card.ApprovalStatus == ApprovalStatus.PendingReview && card.UseLevel.IsNotEmpty())
                {
                    throw new ValidationException("待审核,ABC分类不能为空".L10N());
                }
                if ((card.Warehouse != null && !card.StorageLocationCode.IsNotEmpty()) || (card.Warehouse == null && card.StorageLocationCode.IsNotEmpty()))
                {
                    throw new ValidationException("仓库和库位只能同时为空或不为空".L10N());
                }
            }
        }


        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="card"></param>
        protected void PermisCheck(EquipmentCard card)
        {
            #region 工厂权限
            // 验证工厂是否当前操作用户有权限操作的   
            if (!FactoryIds.Any(p => p == card.FactoryId))
            {
                throw new ValidationException("您没有工厂【{0}】的权限".L10nFormat(card.Factory?.Name));
            }
            #endregion

            #region 仓库权限

            if (card.WarehouseId.HasValue && !WarehouseIds.Any(p => p == card.WarehouseId))
            {
                // 验证仓库是否当前操作用户有权限操作的     
                throw new ValidationException("您没有仓库【{0}】的权限".L10nFormat(card.Warehouse?.Name));

            }
            #endregion

            #region 车间与工厂权限(递归)
            //找当前工厂所有的下级数据(车间)
            if (card.WorkShopId != null)
            {
                List<Enterprise> EnterprisesList = GetWorkShopByFactoryId(card.FactoryId, new List<Enterprise>());
                var enterids = EnterprisesList.Select(p => p.Id).ToList();
                if (!enterids.Contains((double)card.WorkShopId))
                {
                    throw new ValidationException("车间【{0}】不在工厂【{1}】下".L10nFormat(card.WorkShop?.Name, card.Factory?.Name));
                }
            }
            #endregion

            //输出集合(使用部门,责任部门)
            List<double> outList;

            #region 使用部门与工厂权限
            //使用部门
            //验证部门与工厂的上下级关系    
            if (card.UseDepartment != null)
            {
                if (UseDepartAndFactoryDic.TryGetValue(card.FactoryId, out outList))
                {
                    if (!outList.Any(p => p == card.UseDepartmentId))
                    {
                        throw new ValidationException("使用部门【{0}】不在工厂【{1}】下".L10nFormat(card.UseDepartment?.Name, card.Factory?.Name));
                    }
                }
                else
                {
                    throw new ValidationException("使用部门【{0}】不在工厂【{1}】下".L10nFormat(card.UseDepartment?.Name, card.Factory?.Name));
                }
            }

            #endregion

            #region 管理部门与工厂权限
            //验证管理部门与工厂的上下级关系    
            if (card.Management != null)
            {
                if (ManagementAndFactoryDic.TryGetValue(card.FactoryId, out outList))
                {
                    if (!outList.Any(p => p == card.ManagementId))
                    {
                        throw new ValidationException("管理部门【{0}】不在工厂【{1}】下".L10nFormat(card.Management?.Name, card.Factory?.Name));
                    }
                }
                else
                {
                    throw new ValidationException("管理部门【{0}】不在工厂【{1}】下".L10nFormat(card.Management?.Name, card.Factory?.Name));
                }
            }
            #endregion

            //车间不为空则验证产线
            #region 产线与车间权限
            //验证产线与车间的上下级关系     
            if (card.Resource != null)
            {
                //有产线无车间则提示错误
                if (card.WorkShop == null)
                {
                    throw new ValidationException("填写产线需同时填写车间".L10N());
                }

                if (ResourceAndWorkShopDic.TryGetValue((double)card.WorkShopId, out outList))
                {
                    if (!outList.Any(p => p == card.ResourceId))
                    {
                        throw new ValidationException("产线【{0}】不在车间【{1}】下".L10nFormat(card.Resource?.Name, card.WorkShop?.Name));
                    }
                }
                else
                {
                    throw new ValidationException("产线【{0}】不在车间【{1}】下".L10nFormat(card.Resource?.Name, card.WorkShop?.Name));
                }
            }
            #endregion


            #region 库位与仓库权限
            List<StorageLocation> StorageLocationList;
            //仓库不为空则验证库位
            if (card.StorageLocationCode.IsNotEmpty())
            {
                //已经在前面判断过有仓库必须有库位,有库位必须有仓库
                if (StorageLocationDic.TryGetValue((double)card.WarehouseId, out StorageLocationList))
                {
                    StorageLocation storage = StorageLocationList.FirstOrDefault(p => p.Code == card.Code);
                    if (storage != null)
                    {
                        card.StorageLocationId = storage.Id;
                    }
                    else
                    {
                        throw new ValidationException("未找到对应库位".L10N());
                    }
                }
                else
                {
                    throw new ValidationException("未找到对应库位".L10N());
                }
            }
            #endregion
        }


        /// <summary>
        /// 非空验证
        /// </summary>
        /// <param name="card">设备立卡</param>
        /// <param name="assetConfig">固定资产配置</param>
        protected void AssetNullCheck(EquipmentCard card, EquipAccountAssetConfigValue assetConfig)
        {
            if (card != null)
            {
                if (!assetConfig.Asset)
                {
                    if (card.IssAsset == null)
                    {
                        card.IssAsset = false;
                    }
                    if (card.IssAsset == true && !card.AssetCode.IsNotEmpty())
                    {
                        throw new ValidationException("【是否固定资产】为【是】时，固定资产编码必输".L10N());
                    }
                    if (card.IssAsset == false)
                    {
                        card.AssetCode = null;
                    }
                    if (card.IssAsset == true && !card.AssetName.IsNotEmpty())
                    {
                        throw new ValidationException("【是否固定资产】为【是】时，固定资产名称必输".L10N());
                    }
                    if (card.IssAsset == false)
                    {
                        card.AssetName = null;
                    }
                    if (card.IssAsset == false)
                    {
                        card.OriginalValue = 0;
                    }
                    if (card.OriginalValue < 0)
                    {
                        throw new ValidationException("【是否固定资产】为【是】时，原值不能小于0".L10N());
                    }
                }
                else
                {
                    //需要忽略该字段
                    if (card.IssAsset == null)
                    {
                        card.IssAsset = false;
                    }
                    if (card.IssAsset == false)
                    {
                        card.AssetCode = null;
                    }
                    if (card.IssAsset == false)
                    {
                        card.AssetName = null;
                    }
                    if (card.IssAsset == false)
                    {
                        card.OriginalValue = 0;
                    }
                }
                if (card.UsefulLife < 0)
                {
                    throw new ValidationException("使用年限不能小于0".L10N());
                }
            }
        }

        /// <summary>
        /// 根据工厂Id获取下属所有的车间
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="allWorkShop"></param>
        /// <returns></returns>
        public virtual List<Enterprise> GetWorkShopByFactoryId(double? parentId, List<Enterprise> allWorkShop)
        {
            if (allWorkShop == null)
            {
                return new List<Enterprise>();
            }
            List<Enterprise> childEnterprises = GetSubEnterprise(parentId);

            foreach (var item in childEnterprises)
            {
                if (item.Level.Type == EnterpriseType.Shop)
                {
                    allWorkShop.Add(item);
                }
                GetWorkShopByFactoryId(item.Id, allWorkShop);
            }
            return allWorkShop;
        }

        #region 递归获取企业模型数据
        /// <summary>
        /// 根据parentId获取企业模型下一层级的子数据
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns>返回下一层级的子数据</returns>
        public virtual List<Enterprise> GetSubEnterprise(double? parentId)
        {
            return EnterpriseList.Where(p => p.TreePId == parentId).ToList();
        }
        #endregion
    }
}

