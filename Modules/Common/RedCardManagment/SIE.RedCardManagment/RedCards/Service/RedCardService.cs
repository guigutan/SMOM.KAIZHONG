using NPOI.HSSF.Record;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.PPSNs;
using SIE.EventMessages.WMS.TraceableItem;
using SIE.Items;
using SIE.RedCardManagment.RedcardEvent;
using SIE.RedCardManagment.RedCards.Config;
using SIE.RedCardManagment.RedCards.Dao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.RedCardManagment.RedCards
{
    /// <summary>
    /// 红牌管理服务
    /// </summary>
    public class RedCardService : DomainService
    {
        private readonly ItemSnRetroactiveDao _itemSnRetroactiveDao;
        private readonly BatchRetroactiveDao _batchRetroactiveDao;
        private readonly RedCardDao _redCardDao;
        private readonly ProductRetroactiveDao _productRetroactiveDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="itemSnRetroactiveDao"></param>
        public RedCardService(ItemSnRetroactiveDao itemSnRetroactiveDao, BatchRetroactiveDao batchRetroactiveDao, ProductRetroactiveDao productRetroactiveDao, RedCardDao redCardDao)
        {
            this._itemSnRetroactiveDao = itemSnRetroactiveDao;
            this._batchRetroactiveDao = batchRetroactiveDao;
            this._productRetroactiveDao = productRetroactiveDao;
            this._redCardDao = redCardDao;
        }

        /// <summary>
        /// 获取红牌管理编码配置项的值
        /// </summary>
        /// <returns></returns>
        public virtual string GetRedCardCode()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(RedCard));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到编码生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                    .GenerateSegment(config.BacodeRule, 1)
                    .FirstOrDefault();

        }
        /// <summary>
        /// 检查红牌管理编码生成规则
        /// </summary>
        public virtual void CheckRedCardCodeConfig()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(RedCard));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到红牌管理编码生成规则,请检查".L10N());
        }


        #region 查询 Get
        /// <summary>
        /// 生成新的红牌单号
        /// </summary>
        /// <returns></returns>
        public virtual string GetNewRedCardNo()
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(RedCard));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到红牌单号生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<RedCard> GetRedCards(RedCardCriteria criteria)
        {
            return _redCardDao.GetList(criteria);
        }

        #endregion

        #region 追溯
        /// <summary>
        /// 同步ReelID
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns>同步成功数量</returns>
        public virtual int SyncRedCardTraceable(double redCardId)
        {
            var redCard = RF.GetById<RedCard>(redCardId);
            if (redCard == null)
                throw new EntityNotFoundException("红牌记录不存在。".L10N());
            int amount = 0;//数据追溯数目
            var snList = _itemSnRetroactiveDao.GetList(redCard.Id);
            var batchList = _batchRetroactiveDao.GetList(redCard.Id);
            using (var trans = DB.TransactionScope(RedCardManagmentDataProvider.ConnectionStringName))
            {
                //物料追溯
                amount = SyncRedCardTraceableItem(redCard, snList, batchList);

                //产品追溯
                var sns = snList.Select(c => c.SN).ToList();
                var batchs = batchList.Select(c => c.ItemBatch).ToList();
                var queryInfo = new PPSNQueryInfo()
                {
                    SnList = sns,
                    ItemBatchList = batchs,
                    ItemId = redCard.ItemId
                };
                GetTraceableProductList(queryInfo, redCardId);
                trans.Complete();
            }
            return amount;
        }

        /// <summary>
        /// 追溯物料
        /// </summary>
        /// <param name="redCard"></param>
        /// <param name="snList"></param>
        /// <param name="batchList"></param>
        /// <returns></returns>

        public virtual int SyncRedCardTraceableItem(RedCard redCard, EntityList<ItemSnRetroactive> snList, EntityList<BatchRetroactive> batchList)
        {
            int amount = 0;
            //物料追溯
            DateTime? createTimeStart = null;
            if (redCard.ItemBatch.IsNullOrEmpty() && redCard.ItemSN.IsNullOrEmpty() && redCard.ProductDateStart == null && redCard.ProductDateEnd == null)
            {
                var config = ConfigService.GetConfig(new SyncDaysConfig(), typeof(RedCard));
                if (config != null)
                {
                    var syncDays = config.Days;
                    if (syncDays.HasValue)
                        createTimeStart = DateTime.Today.AddDays(-1 * syncDays.Value);
                }
            }
            var criteria = GetCriteriaInfo(redCard, createTimeStart);
            var traceablinfos = GetTraceableItems(criteria);//从WMS同步数据
            traceablinfos.ForEach(info =>
            {
                if (info.SN.IsNullOrEmpty())
                {
                    var batchQuery = batchList.FirstOrDefault(p => p.WmsKey == info.WmsKey);
                    if (batchQuery == null)
                    {
                        amount++;
                        batchList.Add(new BatchRetroactive
                        {
                            WmsKey= info.WmsKey,
                            ItemId = info.ItemId,
                            SupplierId = info.SupplierId,
                            ItemBatch = info.ItemBatch,
                            Batch = info.Batch,
                            ProductDate = info.ProductDate,
                            Quannity = info.Quannity,
                            RedCardId = redCard.Id,
                        });
                    }

                }
                else
                {
                    var reelQuery = snList.FirstOrDefault(p => p.WmsKey == info.WmsKey);
                    if (reelQuery == null)
                    {
                        amount++;
                        info.RedCardId = redCard.Id;
                        snList.Add(info);
                    }
                }
            });
            RF.Save(batchList);
            RF.Save(snList);
            return amount;
        }
        #endregion

        /// <summary>
        /// 生成查询类
        /// </summary>
        /// <param name="card">红牌</param>
        /// <param name="createTimeStart">开始时间</param>
        /// <returns></returns>
        private TraceableItemCriteriaInfo GetCriteriaInfo(RedCard card, DateTime? createTimeStart)
        {
            var criteriaInfo = new TraceableItemCriteriaInfo()
            {
                ItemId = card.ItemId,
                SupplierId = card.SupplierId,
                ProductBatch = card.Batch,
                SN = card.ItemSN,
                ItemBatch = card.ItemBatch,
                ProductDateStart = card.ProductDateStart,
                ProductDateEnd = card.ProductDateEnd,
                CreateTimeStart = createTimeStart
            };
            //if (criteriaInfo.CreateTimeStart.HasValue)
            //    criteriaInfo.CreateTimeStart.Value.AddMilliseconds(1);  //避免获取重复数据
            return criteriaInfo;
        }


        #region 追溯Wms物料信息

        /// <summary>
        /// 查询ReelIDs
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        private List<ItemSnRetroactive> GetTraceableItems(TraceableItemCriteriaInfo criteria)
        {
            var reelIDs = GetRedCardReelIDsFromWMS(criteria);
            var reelIDEntities = TransferRedCardInfo(reelIDs);
            return reelIDEntities;
        }

        #region 调用WMS接口
        /// <summary>
        /// 从WMS获取ReelID数据
        /// </summary>
        /// <param name="criteriaInfos"></param>
        public virtual List<TraceableInfo> GetRedCardReelIDsFromWMS(TraceableItemCriteriaInfo criteriaInfos)
        {
            try
            {
                return RT.Service.Resolve<ITraceableItem>().GetTraceableItemInfos(criteriaInfos);
            }
            catch (Exception ex)
            {
                throw new ValidationException("调用获取追溯物理信息数据接口失败。失败信息：{0}".L10nFormat(ex.Message));
            }
        }

        /// <summary>
        /// 转换WMS的ReelID成QMS的ReelID实体
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        private List<ItemSnRetroactive> TransferRedCardInfo(List<TraceableInfo> infos)
        {
            List<ItemSnRetroactive> result = new List<ItemSnRetroactive>();

            foreach (var info in infos)
            {
                var entity = new ItemSnRetroactive()
                {
                    WmsKey = info.WmsKey,
                    ItemId = info.ItemId,
                    SupplierId = info.SupplierId,
                    SN = info.SN,
                    ItemBatch = info.ItemBatch,
                    Batch = info.ProductBatch,
                    ProductDate = info.ProductDate,
                    Quannity = info.Qty
                };
                result.Add(entity);
            }
            return result;
        }
        #endregion

        #endregion

        #region 物料SN追溯清单
        /// <summary>
        /// 获取物料SN清单
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public virtual EntityList<ItemSnRetroactive> GetItemSnRetroactives(double redCardId, PagingInfo pagingInfo)
        {
            return _itemSnRetroactiveDao.GetList(redCardId, pagingInfo);
        }
        #endregion

        #region 物料批次清单
        /// <summary>
        /// 获取物料批次清单
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public virtual EntityList<BatchRetroactive> GetBatchRetroactives(double redCardId, PagingInfo pagingInfo)
        {
            return _batchRetroactiveDao.GetList(redCardId, pagingInfo);
        }
        #endregion

        #region 产品追溯
        /// <summary>
        /// 实体转换
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        private void GetTraceableProductList(PPSNQueryInfo queryInfo, double recardId)
        {
            var ppsns = GetPPSNs(queryInfo);
            var result = new EntityList<ProductRetroactive>();

            if (ppsns == null || !ppsns.Any())
                return;

            var productList = _productRetroactiveDao.GetList(recardId);

            foreach (var item in ppsns)
            {
                var model = new ProductRetroactive()
                {
                    WorkNo = item.WorkOrderNo,
                    ProductSn = item.ProductSN,
                    RedCardId = recardId,
                    ProductId = item.ProductId,
                    Status = RedCardState.Disable
                };

                if (queryInfo.SnList.Contains(item.PPSN))
                {
                    model.SN = item.PPSN;
                }
                else if (queryInfo.ItemBatchList.Contains(item.PPSN))
                {
                    model.ItemBatch = item.PPSN;
                }

                var batchQuery = productList.Any(p => p.SN == model.SN && p.ItemBatch == model.ItemBatch && p.RedCardId == recardId && p.WorkNo == model.WorkNo);

                if (!batchQuery)
                    result.Add(model);
            }
            RF.Save(result);
        }

        /// <summary>
        /// 获取PPID
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        public virtual List<PPSNInfo> GetPPSNs(PPSNQueryInfo queryInfo)
        {
            try
            {
                var list = RT.Service.Resolve<IPPSNQuery>().GetPPSNInfos(queryInfo);
                //去重
                var distinctPersons = list.Distinct(new PPSNInfoComparer()).ToList();
                return distinctPersons;
            }
            catch (Exception ex)
            {
                throw new ValidationException("调用获取产品追溯数据接口失败。失败信息：{0}".L10nFormat(ex.Message));
            }
        }


        public virtual EntityList<ProductRetroactive> GetProductRetroactives(ProductRetroactiveCriteria criteria)
        {
            return _productRetroactiveDao.GetList(criteria);
        }
        #endregion

        #region 修改红牌状态（主表）
        /// <summary>
        /// 修改红牌状态
        /// </summary>
        /// <returns></returns>
        public virtual bool AlterRedCardStatus(double redCardId, RedCardState state)
        {
            var redCard = UpdateRedCardState(redCardId, state);
            var config = ConfigService.GetConfig<AutoLockProductSNConfigValue>(new AutoLockProductSNConfig(), typeof(RedCard));
            if (state == RedCardState.Enable)
            {
                if (config.IsAuto)
                {
                    _productRetroactiveDao.SetProductRedCardStatusAll(state, redCardId);  //根据配置项锁定产品SN，启用禁用产品红牌
                }
                PubRedCardTask(redCard, redCard.ApplyTime, RT.Identity.Name);
            }
            else if (state == RedCardState.Disable && config.IsAuto)
            {
                _productRetroactiveDao.SetProductRedCardStatusAll(state, redCardId);  //根据配置项锁定产品SN，启用禁用产品红牌

            }

            AddRedCardLog(redCard, state);//填写日志
            return true;
        }

        /// <summary>
        /// 更新红牌状态
        /// </summary>
        /// <param name="redCardId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual RedCard UpdateRedCardState(double redCardId, RedCardState state)
        {
            var reard = _redCardDao.GetById(redCardId, new EagerLoadOptions().LoadWithViewProperty());
            reard.Status = state;
            reard.ApplyTime = DateTime.Now;
            reard.ApplicantId = RT.IdentityId;
            RF.Save(reard);
            if (state != RedCardState.PartialEnable)
            {
                _itemSnRetroactiveDao.SetAllItemSnRedCardStatus(redCardId, state);
                _batchRetroactiveDao.SetAllBatchRedCardStatus(redCardId, state);
                //_productRetroactiveDao.SetAllProductRedCardStatus(redCardId, state);  //红牌关联界面启用红牌时，不启用产品红牌
            }
            return reard;
        }

        /// <summary>
        /// 金庸红牌
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public virtual RedCard DisabledRedCardState(double redCardId)
        {
            var reard = _redCardDao.GetById(redCardId, new EagerLoadOptions().LoadWithViewProperty());
            reard.Status = RedCardState.Disable;
            reard.ApplyTime = DateTime.Now;
            reard.ApplicantId = RT.IdentityId;
            using (var trans = DB.TransactionScope(RedCardManagmentDataProvider.ConnectionStringName))
            {
                RF.Save(reard);
                _itemSnRetroactiveDao.SetAllItemSnRedCardStatus(redCardId, RedCardState.Disable);
                _batchRetroactiveDao.SetAllBatchRedCardStatus(redCardId, RedCardState.Disable);
                _productRetroactiveDao.SetAllProductRedCardStatus(redCardId, RedCardState.Disable);
                trans.Complete();
            }
            return reard;
        }
        #endregion

        #region 修改ItemSN红牌状态
        /// <summary>
        /// 修改红牌状态
        /// </summary>
        /// <returns></returns>
        public virtual bool SetItemSnRedCardStatus(double[] ids, RedCardState state)
        {
            EntityList<ItemSnRetroactive> itemSnRetroactiveList = new EntityList<ItemSnRetroactive>();
            List<string> itemSnList = new List<string>();
            double redCardId = 0;
            foreach (var id in ids)
            {
                var reard = _itemSnRetroactiveDao.GetById(id, new EagerLoadOptions().LoadWithViewProperty());
                reard.Status = state;
                reard.ApplicantId = RT.IdentityId;
                itemSnRetroactiveList.Add(reard);
                itemSnList.Add(reard.SN);
                redCardId = reard.RedCardId;
            }
            RF.Save(itemSnRetroactiveList);
            var config = ConfigService.GetConfig<AutoLockProductSNConfigValue>(new AutoLockProductSNConfig(), typeof(RedCard));
            if (config.IsAuto)
            {
                _productRetroactiveDao.SetProductRedCardStatusForItemSN(itemSnList, state, redCardId);  //根据配置项锁定产品SN，启用禁用产品红牌

            }
            if (state == RedCardState.Enable)
            {
                PubRedCardTask(itemSnRetroactiveList[0].RedCard, DateTime.Now, RT.Identity.Name);
            }
            AddItemSnRedCardLog(itemSnRetroactiveList, state);//填写日志
            return SetMasterRedCardStatus(itemSnRetroactiveList[0].RedCardId);
        }
        #endregion

        #region 修改物料追溯红牌状态
        /// <summary>
        /// 修改红牌状态
        /// </summary>
        /// <returns></returns>
        public virtual object SetBatchRedCardStatus(double[] ids, RedCardState state)
        {
            EntityList<BatchRetroactive> batchRetroactivesList = new EntityList<BatchRetroactive>();
            List<string> itemBatchList = new List<string>();
            double redCardId = 0;
            foreach (var id in ids)
            {
                var reard = _batchRetroactiveDao.GetById(id, new EagerLoadOptions().LoadWithViewProperty());
                reard.Status = state;
                reard.ApplicantId = RT.IdentityId;
                batchRetroactivesList.Add(reard);
                itemBatchList.Add(reard.ItemBatch);
                redCardId = reard.RedCardId;
            }
            RF.Save(batchRetroactivesList);
            var config = ConfigService.GetConfig<AutoLockProductSNConfigValue>(new AutoLockProductSNConfig(), typeof(RedCard));
            if (config.IsAuto)
            {
                _productRetroactiveDao.SetProductRedCardStatusForItemBatch(itemBatchList, state, redCardId);  //根据配置项锁定产品SN，启用禁用产品红牌

            }
            if (state == RedCardState.Enable)
                PubRedCardTask(batchRetroactivesList[0].RedCard, DateTime.Now, RT.Identity.Name);
            AddBatchRedCardLog(batchRetroactivesList, state);//填写日志
            return SetMasterRedCardStatus(batchRetroactivesList[0].RedCardId);
        }
        #endregion

        #region 修改关联产品红牌状态
        /// <summary>
        /// 修改红牌状态
        /// </summary>
        /// <returns></returns>
        public virtual bool SetProductRedCardStatus(double[] ids, RedCardState state)
        {
            var reard = _productRetroactiveDao.GetProductListFroIds(ids, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var item in reard)
            {
                item.Status = state;
                item.ApplicantId = RT.IdentityId;
                item.ApplyTime = DateTime.Now;
            }
            RF.Save(reard);
            if (state == RedCardState.Enable)
            {
                PubRedCardTask(reard[0].RedCard, DateTime.Now, RT.Identity.Name);
            }
            AddProductRedCardLog(reard, state);//填写日志
            return true;
        }
        #endregion

        #region 根据红牌ID查询物料SN清单
        /// <summary>
        /// 修改红牌状态
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<ItemSnRetroactive> GetItemSnInventory(double redCardId)
        {
            return _itemSnRetroactiveDao.GetItemSnInventory(redCardId);
        }
        #endregion

        #region 根据红牌ID查询物料SN清单
        /// <summary>
        /// 根据红牌ID查询物料SN清单
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<BatchRetroactive> GetBatchRetroactiveInventory(double redCardId)
        {
            return _batchRetroactiveDao.GetBatchRetroactiveInventory(redCardId);
        }
        #endregion

        #region 根据红牌ID查询关联产品清单
        /// <summary>
        /// 根据红牌ID查询关联产品清单
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<ProductRetroactive> GetProductRetroactiveInventory(double redCardId)
        {
            return _productRetroactiveDao.GetProductRetroactiveInventory(redCardId);
        }
        #endregion

        #region 生成单据

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applyBill"></param>
        /// <returns></returns>
        public virtual RedCard GenerateRedCard(RedCardApplyBills.RedCardApplyBill applyBill)
        {
            RedCard redCard = new RedCard();
            redCard.GenerateId();
            redCard.No = this.GetNewRedCardNo();
            redCard.ItemBatch = applyBill.JoinProductBatchs;
            redCard.ItemSN = applyBill.JoinBarcodes;
            redCard.ProductDateStart = applyBill.ProductDateStart;
            redCard.ProductDateEnd = applyBill.ProductDateEnd;
            redCard.ApplyBillNo = applyBill.No;
            redCard.SupplierId = applyBill.SupplierId;
            redCard.Status = RedCardState.Disable;
            redCard.AddWay = RecordAddWay.Auto;
            redCard.ItemId = applyBill.ItemId.HasValue ? applyBill.ItemId.Value : 0;//前面已控制必填
            redCard.Status = RedCardState.Disable;
            return redCard;
        }

        #endregion

        #region 生成红牌任务
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redCard"></param>
        /// <param name="ApplyTime"></param>
        /// <param name="ApplicantName"></param>
        public virtual void PubRedCardTask(RedCard redCard, DateTime? ApplyTime, string ApplicantName)
        {
            var config = ConfigService.GetConfig<GeneralAbnormalTaskConfigValue>(new GeneralAbnormalTaskConfig(), typeof(RedCard));
            if (config == null) return;
            if (!config.IsAuto) return;
            var products = _productRetroactiveDao.GetList(redCard.Id, new EagerLoadOptions().LoadWithViewProperty());
            RT.Service.Resolve<RedcardEventsController>().OnRedCardStatusChange(redCard, products, ApplyTime, ApplicantName);
        }
        #endregion

        #region 子表禁用按钮修改主表红牌管理红牌状态
        /// <summary>
        /// 修改红牌状态
        /// </summary>
        /// <returns></returns>
        public virtual bool SetMasterRedCardStatus(double redCardId)
        {
            var EnableCount = 0;
            var itemSnLisr = GetItemSnInventory(redCardId);
            //遍历物料SN表，记录红牌状态为启用状态的个数
            foreach (var item in itemSnLisr)
            {
                if (item.Status == RedCardState.Enable)
                {
                    EnableCount++;
                }
            }
            //如果红牌状态为启用状态的个数已经为部分启用的情况，直接设置红牌管理为部分启用
            if (EnableCount > 0 && EnableCount < itemSnLisr.Count)
            {
                return AlterRedCardStatus(redCardId, RedCardState.PartialEnable);
            }
            //遍历物料追溯表，记录红牌状态为启用状态的个数
            var batchRetroactiveLisr = GetBatchRetroactiveInventory(redCardId);
            foreach (var item in batchRetroactiveLisr)
            {
                if (item.Status == RedCardState.Enable)
                {
                    EnableCount++;
                }
            }
            //如果红牌状态为启用状态的个数已经为部分启用的情况，直接设置红牌管理为部分启用
            if (EnableCount > 0 && EnableCount < itemSnLisr.Count + batchRetroactiveLisr.Count)
            {
                return AlterRedCardStatus(redCardId, RedCardState.PartialEnable);
            }
            //结合2个表的数据，红牌状态为启用状态的个数是部分启用的情况，把主表的红牌状态设置为部分启用
            if (EnableCount > 0 && EnableCount < itemSnLisr.Count + batchRetroactiveLisr.Count)
            {
                return AlterRedCardStatus(redCardId, RedCardState.PartialEnable);
            }
            //结合2个表的数据，红牌状态为启用状态的个数是全部启用的情况，把主表的红牌状态设置为启用
            if (EnableCount == itemSnLisr.Count + batchRetroactiveLisr.Count)
            {
                return AlterRedCardStatus(redCardId, RedCardState.Enable);
            }
            //结合2个表的数据，红牌状态为启用状态的个数是0的情况，把主表的红牌状态设置为禁用
            else if (EnableCount == 0)
            {
                return AlterRedCardStatus(redCardId, RedCardState.Disable);
            }
            //只有发生错误才会到此处
            return false;
        }
        #endregion

        #region 添加红牌操作日志
        /// <summary>
        /// 添加红牌操作日志（主表）
        /// </summary>
        /// <returns></returns>
        public virtual void AddRedCardLog(RedCard redCard, RedCardState redCardState)
        {
            EntityList<RedCardLog> redCardLog = new EntityList<RedCardLog>();
            redCardLog.Add(new RedCardLog()
            {
                SN = redCard.ItemSN,
                Supplier = redCard.Supplier,
                Item = redCard.Item,
                ApplicantId = RT.IdentityId,
                RedCardNo = redCard.No,
                Batch = redCard.Batch,
                ItemBatch = redCard.ItemBatch,
                Status = redCardState,
                ApplyTime = DateTime.Now,
                ProductDateStart = redCard.ProductDateStart,
                ProductDateEnd = redCard.ProductDateEnd,
            });
            RF.Save(redCardLog);
        }

        /// <summary>
        /// 添加红牌操作日志（物料SN追溯表）
        /// </summary>
        /// <returns></returns>
        public virtual void AddItemSnRedCardLog(EntityList<ItemSnRetroactive> itemSnRetroactiveList, RedCardState redCardState)
        {
            EntityList<RedCardLog> redCardLog = new EntityList<RedCardLog>();
            foreach (var itemSnRetroactive in itemSnRetroactiveList)
            {
                redCardLog.Add(new RedCardLog()
                {
                    SN = itemSnRetroactive.SN,
                    Supplier = itemSnRetroactive.RedCard.Supplier,
                    Item = itemSnRetroactive.RedCard.Item,
                    ApplicantId = RT.IdentityId,
                    RedCardNo = itemSnRetroactive.RedCard.No,
                    Batch = "",
                    ItemBatch = "",
                    Status = redCardState,
                    ApplyTime = DateTime.Now,
                    ProductDateStart = itemSnRetroactive.RedCard.ProductDateStart,
                    ProductDateEnd = itemSnRetroactive.RedCard.ProductDateEnd,
                });
            }
            RF.Save(redCardLog);
        }

        /// <summary>
        /// 添加红牌操作日志（物料批次追溯表）
        /// </summary>
        /// <returns></returns>
        public virtual void AddBatchRedCardLog(EntityList<BatchRetroactive> batchRetroactiveList, RedCardState redCardState)
        {
            EntityList<RedCardLog> redCardLog = new EntityList<RedCardLog>();
            foreach (var batchRetroactive in batchRetroactiveList)
            {
                redCardLog.Add(new RedCardLog()
                {
                    SN = "",
                    Supplier = batchRetroactive.RedCard.Supplier,
                    Item = batchRetroactive.RedCard.Item,
                    ApplicantId = RT.IdentityId,
                    RedCardNo = batchRetroactive.RedCard.No,
                    Batch = batchRetroactive.Batch,
                    ItemBatch = batchRetroactive.ItemBatch,
                    Status = redCardState,
                    ApplyTime = DateTime.Now,
                    ProductDateStart = batchRetroactive.RedCard.ProductDateStart,
                    ProductDateEnd = batchRetroactive.RedCard.ProductDateEnd,
                });
            }
            RF.Save(redCardLog);
        }

        /// <summary>
        /// 添加红牌操作日志（产品追溯表）
        /// </summary>
        /// <returns></returns>
        public virtual void AddProductRedCardLog(EntityList<ProductRetroactive> productRetroactivesList, RedCardState redCardState)
        {
            EntityList<RedCardLog> redCardLog = new EntityList<RedCardLog>();
            foreach (var productRetroactive in productRetroactivesList)
            {
                redCardLog.Add(new RedCardLog()
                {
                    SN = productRetroactive.SN,
                    Supplier = productRetroactive.RedCard.Supplier,
                    Item = productRetroactive.RedCard.Item,
                    ApplicantId = RT.IdentityId,
                    ProductSn = productRetroactive.ProductSn,
                    RedCardNo = productRetroactive.RedCard.No,
                    Batch = productRetroactive.RedCard.Batch,
                    ItemBatch = productRetroactive.ItemBatch,
                    Status = redCardState,
                    ApplyTime = DateTime.Now,
                    ProductDateStart = productRetroactive.RedCard.ProductDateStart,
                    ProductDateEnd = productRetroactive.RedCard.ProductDateEnd,
                });
            }
            RF.Save(redCardLog);
        }
        #endregion

    }
}
