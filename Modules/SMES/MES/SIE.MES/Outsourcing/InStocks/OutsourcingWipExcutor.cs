using Newtonsoft.Json;
using SIE.Barcodes;
using SIE.Common;
using SIE.Core.Barcodes;
using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.LoadItems;
using SIE.MES.WIP;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Outsourcing.InStocks
{
    /// <summary>
    /// 工序委外在制控制器
    /// </summary>
    public class OutsourcingWipExcutor
    {
        /// <summary>
        /// 运行时控制器
        /// </summary>
        private readonly RuntimeController runtimeController = AppRuntime.Service.Resolve<RuntimeController>();

        /// <summary>
        /// 生产产品版本列表
        /// </summary>
        private readonly IList<WipProductVersion> wipProductVersions;

        /// <summary>
        /// 采集运行时产品
        /// </summary>
        private readonly Dictionary<double, product> productsDictionary = new Dictionary<double, product>();

        /// <summary>
        /// 工序跳转日志
        /// </summary>
        private readonly EntityList<WipGotoLog> wipGotoLogs = new EntityList<WipGotoLog>();

        /// <summary>
        /// 采集运行时产品信息
        /// </summary>
        private readonly EntityList<ProductEntity> productEntities;

        /// <summary>
        /// 当前数据库时间
        /// </summary>
        private readonly DateTime dateTimeOfDataBase;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wipProductVersions"></param>        
        /// <exception cref="ArgumentNullException"></exception>
        public OutsourcingWipExcutor(IList<WipProductVersion> wipProductVersions)
        {
            if (wipProductVersions is null)
            {
                throw new ArgumentNullException(nameof(wipProductVersions));
            }

            var puidKeys = wipProductVersions.Select(x =>
            {
                return CreatePuidKey(x);
            });

            var puidMaps = puidKeys.SplitContains(tempPuidKeys =>
            {
                return DB.Query<PuidMap>().Where(x => tempPuidKeys.Contains(x.Id))
                    .ToList();
            });

            var puids = puidMaps.Select(x => x.Puid).Distinct().ToList();

            productEntities = puids.SplitContains(tempPuids =>
            {
                return DB.Query<ProductEntity>().Where(x => tempPuids.Contains(x.Id)).ToList();
            });

            //采集运行时产品字典表            
            GetProducts(wipProductVersions, puidMaps, productEntities, productsDictionary);

            this.wipProductVersions = wipProductVersions;

            dateTimeOfDataBase = RF.Find<WipProductVersion>().GetDbTime();
        }

        /// <summary>
        /// 跳到指定站
        /// </summary>        
        /// <param name="lastOutsourcingRoutingProcessId">委外最后工序（工单工序清单中的工序ID）</param>
        /// <param name="woId">工单ID</param>
        /// <exception cref="ValidationException">产品未上线，已下线、产品工艺路线不存在跳站工序</exception>
        /// <exception cref="EntityNotFoundException">工序不存在</exception>
        public virtual void ResumeProduction(double lastOutsourcingRoutingProcessId, double woId)
        {
            var wipProductVersionOfFirst = wipProductVersions.First();
            productsDictionary.TryGetValue(wipProductVersionOfFirst.Id, out var fistProduct);

            if (fistProduct == null)
            {
                throw new ValidationException("在制品的生产采集运行时产品找不到，委外入库重新上线失败！".L10N());
            }

            var lastOutsouringProcess = fistProduct.Routing.Processes
                    .FirstOrDefault(p => p.Id == lastOutsourcingRoutingProcessId);

            if (lastOutsouringProcess == null)
            {
                throw new ValidationException("委外入库重新上线失败，当前产品的工艺路线中找不到工序[{0}]!"
                    .L10nFormat(lastOutsourcingRoutingProcessId));
            }

            if (lastOutsouringProcess.IsEnd)
            {
                var wipProductVersionIds = wipProductVersions.Select(x => x.Id).Distinct().ToList();
                var productIds = wipProductVersions.Select(x => x.ProductId).Distinct().ToList();

                //完工处理
                wipProductVersionIds.SplitDataExecute(tempIds =>
                {
                    DB.Update<WipProductVersion>()
                        .Set(x => x.IsFinish, true)
                        .Set(x => x.FinishDateTime, dateTimeOfDataBase)
                        .Set(x => x.NextProcessId, (double?)null)
                        .Set(x => x.IsOutsourcing, false)
                        .Where(x => tempIds.Contains(x.Id))
                        .Execute();
                });

                productIds.SplitDataExecute(tempIds =>
                {
                    DB.Update<WipProduct>()
                       .Set(x => x.State, WipProductState.Finish)
                       .Where(x => tempIds.Contains(x.Id))
                       .Execute();
                });

                var puids = productsDictionary.Values.Select(x => x.Puid).Distinct().ToList();

                puids.SplitDataExecute(tempIds =>
                {
                    //# runtimeController.RemoveProduct(product);#
                    DB.Delete<PuidMap>().Where(p => tempIds.Contains(p.Puid)).Execute();
                    DB.Delete<ProductEntity>().Where(p => tempIds.Contains(p.Id)).Execute();
                });


                var wo = RF.GetById<WorkOrder>(woId);
                if (wo == null)
                {
                    throw new ValidationException("倒扣料执行失败，生产采集运行时产品【product】的工单为空！".L10N());
                }


                if (!wo.FactoryId.HasValue)
                {
                    throw new ValidationException("倒扣料执行失败，工单【{0}】的工厂为空！".L10nFormat(wo.No));
                }

                //获取工单BOM且不在工序BOM中的物料进行扣料
                var allProcessBoms = wo.ProcessBomList;
                var itemIdsOfProcessBom = allProcessBoms.Select(x => x.ItemId).Distinct().ToList();

                //工单BOM,没有出现在工序BOM中的
                var workOrderBoms = RT.Service.Resolve<WorkOrderBomController>()
                    .GetWorkOrderBoms(woId, itemIdsOfProcessBom);

                EntityList<WoCostItem> deductItems = new EntityList<WoCostItem>();
                foreach (var sn in from wipProductVersion in wipProductVersions
                                   let sn = (wipProductVersion.Sn.IsNullOrEmpty() ? wipProductVersionOfFirst.KeyLabel : wipProductVersion.Sn)
                                   select sn)
                {
                    productsDictionary.TryGetValue(wipProductVersionOfFirst.Id, out var product);
                    if (product == null)
                    {
                        throw new ValidationException("在制品【{0}】的生产采集运行时产品找不到，委外入库重新上线失败！".L10nFormat(sn));
                    }

                    RT.Service.Resolve<WipController>().CompleteProduct(product, sn);

                    RT.EventBus.Publish(new WipFinishedEvent(product, sn, dateTimeOfDataBase, null));

                    //写入倒扣料需求
                    if (workOrderBoms.Any())
                    {
                        var costItems = RT.Service.Resolve<BackflushMaterialController>().
                            CreateDeductItems(sn, wo.ResourceId, lastOutsouringProcess.ProcessId, stationId: null,
                                wo.FactoryId.Value, product.WorkOrderId, product.Qty, workOrderBoms, RetrospectType.Single);
                        deductItems.AddRange(costItems);
                    }
                }

                if (deductItems.Any())
                {
                    RF.BatchInsert(deductItems);
                    deductItems.MarkSaved();
                }
            }
            else
            {
                foreach (var wipProductVersion in wipProductVersions)
                {
                    var sn = wipProductVersion.Sn;
                    if (sn.IsNullOrEmpty())
                    {
                        sn = wipProductVersion.KeyLabel;
                    }

                    productsDictionary.TryGetValue(wipProductVersion.Id, out var product);


                    if (product == null)
                    {
                        throw new ValidationException("在制品【{0}】的生产采集运行时产品找不到，委外入库重新上线失败！".L10nFormat(sn));
                    }

                    var fromProcessId = product.Routing.Current.ProcessId.Value;


                    GoToRoutingProccess(productEntities, sn, product, fromProcessId, lastOutsouringProcess,
                        wipProductVersion);
                }
            }

            if (wipGotoLogs.Any())
            {
                //工序跳转日志
                RepositoryFactory.BatchInsert(wipGotoLogs);
            }
        }

        private void GoToRoutingProccess(EntityList<ProductEntity> productEntities,
            string sn, product product, double fromProcessId, process lastOutsouringProcess, WipProductVersion wipProductVersion)
        {
            ComputeNextProcess(lastOutsouringProcess, product, fromProcessId);

            var entity = productEntities.FirstOrDefault(x => x.Id == product.Puid);
            if (entity == null)
            {
                throw new ValidationException("委外入库重新上线失败，当前产品【{0}】找不到【采集运行时产品信息/ProductEntity】!"
                    .L10nFormat(sn));
            }

            var jsonProduct = JsonConvert.SerializeObject(product);
            if (!jsonProduct.IsNullOrEmpty())
            {
                var products = runtimeController.SplitProduct(jsonProduct, 2000);
                runtimeController.SetProduct(entity, products);
            }

            entity.PersistenceStatus = PersistenceStatus.Modified;

            RF.Save(entity);

            //更新生产通用报表的下一工序
            if (product.Routing.GetNext().Any()
                && product.Routing.GetNext().First() != null
                && product.Routing.GetNext().First().ProcessId.HasValue)
            {
                //更新委外中为否
                //更新下一工序Id
                DB.Update<WipProductVersion>()
                    .Set(x => x.IsOutsourcing, false)
                    .Set(x => x.NextProcessId, product.Routing.GetNext().First().ProcessId.Value)
                    .Set(x => x.NextProcessIndex, product.Routing.GetNext().First().Index)
                    .Set(x => x.CurrenrProcessIndex, product.Routing.Current.Index)
                    .Where(x => wipProductVersion.Id == x.Id)
                    .Execute();
            }
            else
            {
                //更新委外中为否
                DB.Update<WipProductVersion>()
                    .Set(x => x.IsOutsourcing, false)
                    .Where(x => wipProductVersion.Id == x.Id)
                    .Execute();
            }
        }

        /// <summary>
        /// 递归计算后工序
        /// </summary>
        /// <param name="process">工序</param>
        /// <param name="product">运行时产品</param>
        /// <param name="fromProcessId"></param> 
        /// <exception cref="ValidationException">采集结果无效</exception>
        private void ComputeNextProcess(process process, product product, double fromProcessId)
        {
            product.Routing.Next.Clear();

            List<double> nextIds;

            if (!process.Next.TryGetValue(ResultType.Pass, out nextIds))
            {
                throw new ValidationException("工序[{0}]未配置采集结果为[{1}]的工序参数"
                    .L10nFormat(process.Name, ResultType.Pass.ToLabel()));
            }

            foreach (var nextId in nextIds)
            {
                var nexts = product.Routing.Processes.Where(q => q.Id == nextId).ToList();

                foreach (var next in nexts)
                {
                    next.IsPass = false;

                    if (next.IsGroup == true)
                    {
                        //工序组下的所有工序都加入后工序列表
                        //工序组，则取组下的所有工序
                        var processesOfGroup = product.Routing.Processes
                            .Where(x => x.GroupId == next.GroupId && x.IsGroup != true).ToList();

                        foreach (var processUnderGroup in processesOfGroup)
                        {
                            processUnderGroup.IsPass = false;

                            if (!product.Routing.Next.Contains(processUnderGroup.Id))
                            {
                                product.Routing.Next.Add(processUnderGroup.Id);
                                AddWipGotoLog(wipGotoLogs, fromProcessId, processUnderGroup);
                            }
                        }
                    }
                    else
                    {
                        if (!product.Routing.Next.Contains(next.Id))
                        {
                            product.Routing.Next.Add(next.Id);
                            AddWipGotoLog(wipGotoLogs, fromProcessId, next);
                        }

                        if (next.Optional)
                        {
                            ComputeNextProcess(next, product, fromProcessId);
                        }
                    }
                }
            }
        }

        private void GetProducts(IList<WipProductVersion> wipProductVersions, EntityList<PuidMap> puidMaps,
            EntityList<ProductEntity> productEntities, Dictionary<double, product> productsDictionary)
        {
            foreach (var wipProductVersion in wipProductVersions)
            {
                var puidKey = CreatePuidKey(wipProductVersion);

                var puidMap = puidMaps.FirstOrDefault(x => x.Id == puidKey);

                if (puidMap == null)
                {
                    continue;
                }

                var productEntity = productEntities.FirstOrDefault(x => x.Id == puidMap.Puid);
                if (productEntity == null)
                {
                    continue;
                }

                string jsonProduct = runtimeController.CombineProduct(productEntity);

                productsDictionary.Add(wipProductVersion.Id, JsonConvert.DeserializeObject<product>(jsonProduct));
            }
        }

        private static void AddWipGotoLog(EntityList<WipGotoLog> wipGotoLogs, double fromProcessId, process toProcess)
        {
            if (toProcess is null)
            {
                throw new ArgumentNullException(nameof(toProcess));
            }

            var log = new WipGotoLog()
            {
                FromProcessId = fromProcessId,
                ToProcessId = toProcess.ProcessId.Value,
                UserId = AppRuntime.IdentityId,
                LogDate = DateTime.Now
            };

            wipGotoLogs.Add(log);
        }

        private string CreatePuidKey(WipProductVersion x)
        {
            if (!x.Sn.IsNullOrEmpty())
            {
                return runtimeController.CreatePuidKey(x.Sn, BarcodeType.SN);
            }
            else
            {
                if (!x.KeyLabel.IsNullOrEmpty())
                {
                    return runtimeController.CreatePuidKey(x.KeyLabel, BarcodeType.KeyLabel);
                }
                else
                {
                    throw new ValidationException("【条码】和【组件条码】都为空!".L10N());
                }
            }
        }
    }
}
