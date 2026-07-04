using IronPython.Runtime.Operations;
using Org.BouncyCastle.Crypto;
using SIE.Barcodes;
using SIE.Common;
using SIE.Common.NumberRules;
using SIE.Common.Sort;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.PackingPrints;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WIP.NewPackages
{
    /// <summary>
    /// 简化版包装采集
    /// </summary>
    public partial class NewPackageController : WipController
    {
        /// <summary>
        /// 获取包装记录
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public virtual EntityList<PackageSnRecord> GetPackageSnRecords(double resourceId, double processId, double stationId)
        {
            //return Query<PackageSnRecord>()
            //    .Where(p => p.ResourceId == resourceId && p.ProcessId == processId && p.StationId == stationId)
            //    .OrderBy(p => p.Sn).ToList(null, new EagerLoadOptions().LoadWith(PackageSnRecord.PackageUnitProperty)
            //    .LoadWith(PackageSnRecord.WorkOrderProperty)
            //    .LoadWith(PackageSnRecord.ProductProperty));
            return Query<PackageSnRecord>()
                .Where(p => p.ResourceId == resourceId && p.ProcessId == processId && p.StationId == stationId)
                .OrderBy(p => p.Sn).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取包装记录
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <param name="workOrderId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual EntityList<PackageSnRecord> GetPackageSnRecords(double resourceId, double processId, double stationId, double workOrderId, double productId)
        {
            return Query<PackageSnRecord>()
                .Where(p => p.ResourceId == resourceId && p.ProcessId == processId
                    && p.StationId == stationId && p.WorkOrderId == workOrderId && p.ProductId == productId).ToList();
        }

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="workcell"></param>
        /// <param name="packageNoList"></param>
        /// <param name="isAdvance"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public override string DoPackage(string sn, Workcell workcell, object packageNoList, object isAdvance)
        {
            List<string> packNos = new List<string>();

            // 预输入包装号
            var advanceNoList = packageNoList as Queue<string>;
            // 是否提前打印
            var isadvance = Convert.ToBoolean(isAdvance);

            var woId = Query<Barcode>().Where(p => p.Sn == sn).Select(p => p.WorkOrderId).FirstOrDefault<double>();
            var wo = Query<WorkOrder>().Where(p => p.Id == woId).Select(p => new { p.Id, p.No, p.ProductId }).FirstOrDefault<WoData>();

            var rules = Query<WorkOrderPackageRuleDetail>().Where(p => p.WorkOrderId == wo.Id).ToList().OrderBy(p => SortExtension.GetIndex(p)).ToArray();
            if (rules.Length <= 1)
            {
                throw new ValidationException("未维护工单[{0}]包装规则且包装规则最少有2层！".L10nFormat(wo.No));
            }

            var masterUnit = rules.FirstOrDefault();
            if (masterUnit == null || !masterUnit.PackageUnit.IsMasterUnit)
            {
                throw new ValidationException("请确保工单[{0}]主单位已经维护并且是第一个！".L10nFormat(wo.No));
            }

            var records = GetPackageSnRecords(workcell.ResourceId, workcell.ProcessId, workcell.StationId, woId, wo.ProductId);

            var record = GeneragePackageSnRecord(sn, masterUnit.PackageUnitId, wo.Id, wo.ProductId, "", workcell.ResourceId, workcell.ProcessId, workcell.StationId, 1, 1);
            records.Add(record);
            var itemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabels(records.Where(p => p.WoSn == "").Select(p => p.Sn).Distinct().ToList());

            //验证物流状态            
            if (itemLabels.Any(x => x.Qty != masterUnit.Qty))
            {
                throw new ValidationException("条码【{0}】的主单位数量与标签数量不一致"
                    .L10nFormat(itemLabels.Where(x => x.Qty != masterUnit.Qty).Select(x => x.Label).Concat(",")));
            }

            var packageRelations = RT.Service.Resolve<PackingRelationController>().GetAllPackingRelations(records.Where(p => p.WoSn != "").Select(p => p.Sn).Distinct().ToList());

            //级联打包
            List<PackingRelation> publicRelations = new List<PackingRelation>();
            for (int i = 1; i < rules.Length; i++)
            {
                //上层包装规则
                var upperRule = rules[i - 1];
                //当前包装规则
                var currentRule = rules[i];
                //上层的所有数据
                var allRecords = records.Where(p => p.PackageUnitId == upperRule.PackageUnitId && p.PersistenceStatus != PersistenceStatus.Deleted).OrderBy(p => p.CreateDate).ToList();
                bool isLastRult = (i == rules.Length - 1);
                while (allRecords.Count >= currentRule.LevelQty)
                {
                    var curRecords = allRecords.Take(Convert.ToInt32(currentRule.LevelQty)).ToList();
                    var curSns = curRecords.Select(p => p.Sn).ToList();

                    //生成包装
                    var packRelation = GeneratePackingRelation(advanceNoList, isadvance, currentRule.NumberRuleId.Value, wo.Id, currentRule.PackageUnitId, workcell.ProcessId, workcell.StationId, isLastRult, curRecords.Count, curRecords.Sum(p => p.ItemQty));
                    publicRelations.Add(packRelation);
                    packNos.Add(packRelation.PackageNo);
                    packageRelations.Add(packRelation);
                    //i==1的时候取物料标签   其他取包装条码
                    if (i == 1)
                    {
                        var curItemLabels = itemLabels.Where(p => curSns.Contains(p.Label)).ToList();
                        curItemLabels.ForEach(p => p.RelationId = packRelation.Id);
                    }
                    else
                    {
                        var curRelations = packageRelations.Where(p => curSns.Contains(p.PackageNo)).ToList();
                        curRelations.ForEach(p => { p.RootId = packRelation.Id; p.TreePId = packRelation.Id; p.ParentNo = packRelation.PackageNo; });
                        UpdateChildRelations(curRelations, packRelation.Id, ref packageRelations);
                    }
                    string woSn = string.Empty;
                    if (i == 1)
                        woSn = string.Join(",", curRecords.OrderBy(p => p.Sn).Select(p => p.Sn).ToList());
                    else
                        woSn = string.Join(",", curRecords.OrderBy(p => p.WoSn).Select(p => p.WoSn).ToList());
                    if (!isLastRult)
                    {
                        var parentRecord = GeneragePackageSnRecord(packRelation.PackageNo, currentRule.PackageUnitId, wo.Id, wo.ProductId, woSn, workcell.ResourceId, workcell.ProcessId, workcell.StationId, curRecords.Count, curRecords.Sum(p => p.ItemQty));
                        records.Add(parentRecord);
                    }

                    curRecords.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);

                    allRecords = records.Where(p => p.PackageUnitId == upperRule.PackageUnitId && p.PersistenceStatus != PersistenceStatus.Deleted).OrderBy(p => p.CreateDate).ToList();
                }
            }
            var saveRecords = records.Where(p => (p.PersistenceStatus == PersistenceStatus.Deleted && p.Id != 0) || p.PersistenceStatus == PersistenceStatus.New).AsEntityList();
            RF.Save(saveRecords);
            var insertRelations = packageRelations.Where(p => p.PersistenceStatus == PersistenceStatus.New).OrderByDescending(p => p.Id).AsEntityList();
            RF.Save(insertRelations);
            var otherRelations = packageRelations.Where(p => p.PersistenceStatus != PersistenceStatus.New).AsEntityList();
            RF.Save(otherRelations);
            RF.Save(itemLabels);
            RT.EventBus.Publish(new DoPackingEvent(DoPackingAction.Packed, "MesPacking1", publicRelations.ToArray()));
            return string.Join(",", packNos);
        }

        /// <summary>
        /// 手工打包
        /// </summary>
        /// <param name="records"></param>
        /// <param name="rules"></param>
        /// <param name="nextRule"></param>
        /// <param name="woId"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        public virtual Tuple<string,string> DoPackageMuanual(EntityList<PackageSnRecord> records, WorkOrderPackageRuleDetail[] rules, WorkOrderPackageRuleDetail nextRule, double woId, Workcell workcell)
        {
            var wo = Query<WorkOrder>().Where(p => p.Id == woId).Select(p => new { p.Id, p.No, p.ProductId }).FirstOrDefault<WoData>();
            string pkg = string.Empty;
            var lastRule = rules[rules.Length - 1];
            bool isLast = (lastRule.Id == nextRule.Id);
            if (!CheckRecordHasRelationIds(records,out string err))
            {
                return new Tuple<string, string>(err,"");

            }
            var relation = GeneratePackingRelation(nextRule.NumberRuleId.Value, woId, nextRule.PackageUnitId, workcell.ProcessId, workcell.StationId, isLast, records.Count, records.Sum(p => p.ItemQty));
            pkg = relation.PackageNo;

            EntityList<PackingRelation> packageRelations = new EntityList<PackingRelation>();
            packageRelations.Add(relation);
            EntityList<ItemLabel> itemLabels = new EntityList<ItemLabel>();
            if (records.Any(p => p.WoSn != ""))
            {
                var sns = records.Select(p => p.Sn).Distinct().ToList();
                packageRelations.AddRange(RT.Service.Resolve<PackingRelationController>().GetAllPackingRelations(sns));
                var curRelations = packageRelations.Where(p => sns.Contains(p.PackageNo)).ToList();
                curRelations.ForEach(p =>
                {
                    p.RootId = relation.Id; p.TreePId = relation.Id; p.ParentNo = relation.PackageNo;
                });
                UpdateChildRelations(curRelations, relation.Id, ref packageRelations);
            }
            else
            {
                itemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabels(records.Select(p => p.Sn).Distinct().ToList());
                itemLabels.ForEach(p => p.RelationId = relation.Id);
            }

            records.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
            string woSn = string.Empty;
            if (records.Any(p => p.WoSn != ""))
            {
                woSn = string.Join(",", records.OrderBy(p => p.WoSn).Select(p => p.WoSn).ToList());
            }
            else
            {
                woSn = string.Join(",", records.OrderBy(p => p.Sn).Select(p => p.Sn).ToList());
            }

            if (!isLast)
            {
                var parentRecord = GeneragePackageSnRecord(relation.PackageNo, nextRule.PackageUnitId, woId, wo.ProductId, woSn, workcell.ResourceId, workcell.ProcessId, workcell.StationId, records.Count, records.Sum(p => p.ItemQty));
                records.Add(parentRecord);
            }

            RF.Save(records);
            var insertRelations = packageRelations.Where(p => p.PersistenceStatus == PersistenceStatus.New).OrderByDescending(p => p.Id).AsEntityList();
            RF.Save(insertRelations);
            var otherRelations = packageRelations.Where(p => p.PersistenceStatus != PersistenceStatus.New).AsEntityList();
            RF.Save(otherRelations);
            if (itemLabels.Any())
            {
                RF.Save(itemLabels);
            }

            return new Tuple<string, string>("", pkg);
        }
        /// <summary>
        /// 检查SN是否已经打包过
        /// </summary>
        /// <param name="records"></param>
        /// <param name="erro"></param>
        /// <returns></returns>
        public virtual bool CheckRecordHasRelationIds(EntityList<PackageSnRecord> records,out string erro)
        {
            //检验包装是否已经生成过
            erro = "";
            var recordsSns = records.Select(m => m.Sn).ToList();
            var packedRelation = recordsSns.SplitContains(snlist =>
            {
                return Query<ItemLabel>().Where(m => recordsSns.Contains(m.Label) && m.RelationId > 0).ToList();
            });
            if (packedRelation.Any())
            {
                var packedsn = packedRelation.Select(m => m.Label);
                var ids = records.Select(m => m.Id).ToList();
                erro = "条码号【{0}】已被打包".L10nFormat(string.Join(',', packedsn));
                DB.Delete<PackageSnRecord>().Where(m => ids.Contains(m.Id) && packedsn.Contains(m.Sn)).Execute();
                return false;
            }
            return true;
        }

        private void UpdateChildRelations(List<PackingRelation> curRelations, double rootId, ref EntityList<PackingRelation> packageRelations)
        {
            if (curRelations.Count > 0)
            {
                var ids = curRelations.Select(p => (double?)p.Id).ToList();
                var curPackageRelations = packageRelations.Where(p => ids.Contains(p.TreePId)).ToList();
                if (curPackageRelations.Count > 0)
                {
                    curPackageRelations.ForEach(p => { p.RootId = rootId; });
                    UpdateChildRelations(curPackageRelations, rootId, ref packageRelations);
                }
            }
        }

        /// <summary>
        /// 生成一条记录
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="packageUnitId"></param>
        /// <param name="woId"></param>
        /// <param name="productId"></param>
        /// <param name="woSn"></param>
        /// <param name="resourceId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <param name="packedQty"></param>
        /// <param name="itemQty"></param>
        /// <returns></returns>
        public virtual PackageSnRecord GeneragePackageSnRecord(string sn, double packageUnitId, double woId, double productId, string woSn, double resourceId, double processId, double stationId, decimal packedQty, decimal itemQty)
        {
            var record = new PackageSnRecord()
            {
                Sn = sn,
                PackageUnitId = packageUnitId,
                WorkOrderId = woId,
                WoSn = woSn,
                ResourceId = resourceId,
                ProcessId = processId,
                StationId = stationId,
                PackedQty = packedQty,
                ItemQty = itemQty,
                ProductId = productId
            };
            return record;
        }

        /// <summary>
        /// 扫码生成包装关系(包含提前打印)
        /// </summary>
        /// <param name="packageNoList"></param>
        /// <param name="isAdvance"></param>
        /// <param name="numberRule"></param>
        /// <param name="woId"></param>
        /// <param name="packageUnitId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <param name="isProcessFinish"></param>
        /// <param name="packedQty"></param>
        /// <param name="itemQty"></param>
        /// <returns></returns>
        private PackingRelation GeneratePackingRelation(Queue<string> packageNoList, bool isAdvance, double numberRule, double woId, double packageUnitId, double processId, double stationId, bool isProcessFinish, decimal packedQty, decimal itemQty)
        {

            if (isAdvance && packageNoList.Count == 0)
            {

                throw new ValidationException("提前打印需传入至少一个包装号".L10N());
            }

            var no = isAdvance ? packageNoList.Dequeue() : RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRule, 1).FirstOrDefault();
            if (isAdvance)
            {
                DB.Update<PackingBarcode>()
                        .Set(x => x.IsUse, true)
                        .Where(x => x.Code == no)
                        .Execute();
            }
            var packingRelation = new PackingRelation()
            {
                PackageNo = no,
                ParentNo = "",
                FullPackedQty = 0,
                PackedQty = packedQty, //下一层的子数量
                ItemQty = itemQty, //包装产品SN的数量
                PackingBy = RT.IdentityId,
                PackedDate = DateTime.Now,
                PackageUnitId = packageUnitId,
                //RootId = rootId, //根ID
                TreePId = null, //父ID
                State = LogisticState.Printed,
                ProcessId = processId,
                StationId = stationId,
                IsProcessFinish = true,
                WorkOrderId = woId
            };
            packingRelation.GenerateId();
            packingRelation.RootId = packingRelation.Id;
            return packingRelation;
        }

        /// <summary>
        /// 手动打包生成包装关系
        /// </summary>
        /// <param name="numberRule"></param>
        /// <param name="woId"></param>
        /// <param name="packageUnitId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <param name="isProcessFinish"></param>
        /// <param name="packedQty"></param>
        /// <param name="itemQty"></param>
        /// <returns></returns>
        private PackingRelation GeneratePackingRelation(double numberRule, double woId, double packageUnitId, double processId, double stationId, bool isProcessFinish, decimal packedQty, decimal itemQty)
        {
            var no = RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRule, 1).FirstOrDefault();
            var packingRelation = new PackingRelation()
            {
                PackageNo = no,
                ParentNo = "",
                FullPackedQty = 0,
                PackedQty = packedQty, //下一层的子数量
                ItemQty = itemQty, //包装产品SN的数量
                PackingBy = RT.IdentityId,
                PackedDate = DateTime.Now,
                PackageUnitId = packageUnitId,
                //RootId = rootId, //根ID
                TreePId = null, //父ID
                State = LogisticState.Printed,
                ProcessId = processId,
                StationId = stationId,
                IsProcessFinish = true,
                WorkOrderId = woId
            };
            packingRelation.GenerateId();
            packingRelation.RootId = packingRelation.Id;
            return packingRelation;
        }


        /// <summary>
        /// 获取包装号
        /// </summary>
        /// <param name="packageNo">包装号</param>
        public virtual PackingBarcode GetPackingBarcode(string packageNo)
        {
            return Query<PackingBarcode>().Where(p => p.Code == packageNo)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工单包装规则
        /// </summary>
        /// <param name="woId"></param>
        /// <returns></returns>
        public virtual WorkOrderPackageRuleDetail[] GetWorkOrderRule(double woId)
        {
            return Query<WorkOrderPackageRuleDetail>().Where(p => p.WorkOrderId == woId).ToList(null, new EagerLoadOptions().LoadWithViewProperty()).OrderBy(p => SortExtension.GetIndex(p)).ToArray();
        }

        /// <summary>
        /// 获取工单产品id
        /// </summary>
        /// <param name="woId"></param>
        /// <returns></returns>
        public virtual double GetWorkOrderProductId(double woId)
        {
            var wo = Query<WorkOrder>().Where(p => p.Id == woId).FirstOrDefault();
            return wo != null ? wo.ProductId : 0;
        }
    }
}
