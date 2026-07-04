using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.MainenanceProjects;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.ApiModels;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SIE.EMS.Equipments.Boms
{
    /// <summary>
    /// 设备BOM控制器
    /// </summary>
    public partial class EquipBomController : DomainController
    {
        /// <summary>
        /// 查询设备BOM
        /// </summary>
        /// <param name="criteria">设备BOM查询实体</param>
        /// <returns>设备BOM列表</returns>
        public virtual EntityList<EquipBom> CriteriaEquipBoms(EquipBomCriteria criteria)
        {
            var q = Query<EquipBom>();
            if (criteria.Code.IsNotEmpty())
                q.Where(p => p.EquipModel.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                q.Where(p => p.EquipModel.Name.Contains(criteria.Name));
            if (criteria.EquipTypeId != 0 && criteria.EquipTypeId != null)
                q.Where(p => p.EquipModel.EquipTypeId == criteria.EquipTypeId);
            if (criteria.CreateDateTime.BeginValue.HasValue)
                q.Where(p => p.CreateDate >= criteria.CreateDateTime.BeginValue);
            if (criteria.CreateDateTime.EndValue.HasValue)
                q.Where(p => p.CreateDate <= criteria.CreateDateTime.EndValue);

            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询设备BOM明细
        /// </summary>
        /// <param name="criteria">设备BOM明细查询实体</param>
        /// <returns>设备BOM明细列表</returns>
        public virtual EntityList<EquipBomDetail> CriteriaEquipBomDtls(EquipBomDetailSelCriteria criteria)
        {
            var q = Query<EquipBomDetail>().Where(p => p.SparePart.State == State.Enable);

            if (criteria.SparePartCode.IsNotEmpty())
            {
                q.Where(p => p.SparePart.SparePartCode.Contains(criteria.SparePartCode));
            }

            if (criteria.SparePartName.IsNotEmpty())
            {
                q.Where(p => p.SparePart.SparePartName.Contains(criteria.SparePartName));
            }

            if (criteria.ModelCode.IsNotEmpty())
            {
                q.Where(p => p.EquipBom.EquipModel.Code.Contains(criteria.ModelCode));
            }

            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var storeSummaryList = list.Select(s => s.SparePartId).SplitContains(tempIds =>
            {
                return Query<StoreSummary>().Where(p => tempIds.Contains(p.SparePartId)).ToList();
            });

            list.ForEach(x =>
            {
                x.StockQty = storeSummaryList.FirstOrDefault(p => p.SparePartId == x.SparePartId)?.GoodNumber;
            });

            return list;
        }

        /// <summary>
        /// 获取设备BOM明细
        /// </summary>
        /// <param name="Id">主表Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="orderInfos">排序</param>
        /// <returns>设备BOM明细</returns>
        public virtual EntityList<EquipBomDetail> GetEquipBomDetails(double Id, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            var query = Query<EquipBomDetail>().Where(c => c.EquipBomId == Id);
            
            if (orderInfos != null && orderInfos.Count > 0)
            {
                query.OrderBy(orderInfos);
            }

            var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var storeSummaryList = list.Select(s => s.SparePartId).SplitContains(tempIds =>
            {
                return Query<StoreSummary>().Where(p => tempIds.Contains(p.SparePartId)).ToList();
            });

            list.ForEach(x =>
            {
                x.StockQty = storeSummaryList.FirstOrDefault(p => p.SparePartId == x.SparePartId)?.SumNumber;
            });

            return list;
        }


        /// <summary>
        /// 根据设备台账编码，获取备件BOM
        /// </summary>
        /// <param name="equipCode"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual Tuple<int, IList<SpareBomInfo>> GetEquipBomDetails(string equipCode, PagingInfo pagingInfo, string key)
        {
            var q = Query<EquipBomDetail>()
                .LeftJoin<EquipBom>((ebd, eb) => ebd.EquipBomId == eb.Id)
                .Exists<EquipBom, EquipAccount>((ebd, eb ,ea) => ea.Where(p => p.EquipModelId == eb.EquipModelId && ebd.EquipBomId == eb.Id && p.Code == equipCode))
                .LeftJoin<SparePart>((ebd, sp) => ebd.SparePartId == sp.Id)
                .WhereIf<SparePart>(key.IsNotEmpty(), (ebd, sp) => sp.SparePartCode.Contains(key) || sp.SparePartName.Contains(key))
                .LeftJoin<SparePart, Unit>((sp, u) => sp.UnitId == u.Id)
                .LeftJoin<SparePart, ItemCategory>((sp, ic) => sp.ItemCategoryId == ic.Id)
                .LeftJoin<SparePart, EquipModel>((sp, em) => sp.SpartEquipModelId == em.Id)
                .Select<EquipBom, EquipAccount, SparePart, Unit, ItemCategory, EquipModel>((ebd, eb, ea, sp, u, ic, em) => new
                {
                    SparePartId = sp.Id,
                    SparePartCode = sp.SparePartCode,
                    SparePartName = sp.SparePartName,
                    UnitName = u.Name,
                    Specification = sp.Specification,
                    SparePartTypeName = ic.Name,
                    SpEquipModelCode = em.Code,
                    SpEquipModelName = em.Name,
                    Manufacturer = sp.Manufacturer
                }).Distinct();
            var totalCount = q.Count();
            var queryList = q.ToList<SpareBomInfo>(pagingInfo);

            return Tuple.Create<int, IList<SpareBomInfo>>(totalCount, queryList);
        }

        /// <summary>
        /// 获取设备型号
        /// </summary>
        /// <param name="keyword">下拉列表过滤条件</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>设备型号列表</returns>
        public virtual EntityList<EquipModel> GetEquipModels(string keyword, PagingInfo pagingInfo)
        {
            var query = Query<EquipModel>();
            if (!keyword.IsNullOrEmpty())
                query = query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            list.ForEach(x =>
            {
                x.Specifications = x.EquipType?.TypeName;//设备类型
            });
            return list;
        }

        /// <summary>
        /// 获取过滤后的设备BOM明细
        /// </summary>
        /// <param name="detail">选中的设备BOM明细</param>
        /// <param name="keyword">下拉列表过滤条件</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>设备BOM明细列表</returns>
        public virtual EntityList<EquipBomDetail> GetDowngradeEquipBomDetails(EquipBomDetail detail, string keyword, PagingInfo pagingInfo)
        {
            EntityList<EquipBomDetail> detailList = new EntityList<EquipBomDetail>();

            //查询出非自身的同级BOM明细备件
            var list = Query<EquipBomDetail>()
                .WhereIf(keyword.IsNotEmpty(), p => p.SparePart.SparePartCode.Contains(keyword) || p.SparePart.SparePartName.Contains(keyword))
                .Where(p => p.TreePId == detail.TreePId && p.Id != detail.Id && p.EquipBomId == detail.EquipBomId)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var item in list)
            {
                detailList.Add(item);

                GetEquipBomDetailsByRecursion(item.Id, keyword, pagingInfo, detailList);
            }
            foreach( var item in detailList)
            {
                if (!detailList.Any(p => p.Id == item.TreePId))
                {
                    item.TreePId = null;
                }
            }
            return detailList;
        }

        private void GetEquipBomDetailsByRecursion(double detailId, string keyword, PagingInfo pagingInfo, EntityList<EquipBomDetail> detailList)
        {
            var list = Query<EquipBomDetail>().Where(p => p.TreePId == detailId)
                .WhereIf(keyword.IsNotEmpty(), p => p.SparePart.SparePartCode.Contains(keyword) || p.SparePart.SparePartName.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var item in list)
            {
                detailList.Add(item);

                GetEquipBomDetailsByRecursion(item.Id, keyword, pagingInfo, detailList);
            }
        }

        /// <summary>
        /// 根据设备BOMID移除所有备件列表
        /// </summary>
        /// <param name="deleteIds"></param>
        public virtual void RemoveByEquipBomIds(List<double> deleteIds)
        {
            DB.Delete<EquipBomDetail>().Where(m => deleteIds.Contains(m.EquipBomId)).Execute();
        }

        /// <summary>
        /// 获取备件基础数据
        /// </summary>
        /// <param name="keyword">下拉列表过滤条件</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>备件基础数据列表</returns>
        public virtual EntityList<SparePart> GetSpareParts(string keyword, PagingInfo pagingInfo)
        {
            var query = Query<SparePart>();
            if (!keyword.IsNullOrEmpty())
                query = query.Where(p => p.SparePartCode.Contains(keyword) || p.SparePartName.Contains(keyword));
            var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var stockList = list.Select(p => p.Id).SplitContains(tempIds =>
            {
                return Query<StoreSummary>().Where(p => tempIds.Contains(p.SparePartId)).ToList();
            });
            list.ForEach(x =>
            {
                x.OriginalItemCode = x.ItemCategory.Code;//备件类型


                if (stockList.Any(p => p.SparePartId == x.Id))
                {
                    x.GoodNumber = stockList.First(p => p.SparePartId == x.Id).GoodNumber;  
                }
            });
            return list;
        }

        /// <summary>
        /// 获取备件基础数据
        /// </summary>
        /// <param name="code">备件编码</param>
        /// <returns>备件基础数据</returns>
        public virtual SparePart GetSparePart(string code)
        {
            var query = Query<SparePart>();
            if (!code.IsNullOrEmpty())
                query = query.Where(p => p.SparePartCode == code);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 升级设备BOM明细
        /// </summary>
        /// <param name="detailId">备件ID</param>
        public virtual void UpgradeEquipBomDetail(double detailId)
        {
            var detail = RF.GetById<EquipBomDetail>(detailId);
            if (detail == null)
                throw new ValidationException("升级失败，该备件不存在！".L10N());
            if (detail.TreePId == null)
                throw new ValidationException("升级失败，该备件已经是最高级！".L10N());
            var parentDetail = RF.GetById<EquipBomDetail>(detail.TreePId);
            if (parentDetail == null)
                throw new ValidationException("升级失败，该备件的父级不存在，请检查！".L10N());
            var parents = Query<EquipBomDetail>().Where(p => p.TreePId == parentDetail.TreePId && p.SparePartId == detail.SparePartId && p.EquipBomId == detail.EquipBomId).ToList();
            if (parents.Any())
                throw new ValidationException("升级失败，该备件的父层级里已存在该备件，请检查！".L10N());
            else
                DB.Update<EquipBomDetail>().Set(p => p.TreePId, parentDetail.TreePId).Where(p => p.Id == detailId).Execute();
        }

        /// <summary>
        /// 降级设备BOM明细
        /// </summary>
        /// <param name="detailId">设备台账ID</param>
        /// <param name="parentDetailId">目标父台账ID</param>
        public virtual void DowngradeEquipBomDetail(double detailId, double parentDetailId)
        {
            var detail = RF.GetById<EquipBomDetail>(detailId);
            var parentDetail = RF.GetById<EquipBomDetail>(parentDetailId);
            if (parentDetail == null)
                throw new ValidationException("降级失败，目标父备件不存在，请检查！".L10N());
            var childs = Query<EquipBomDetail>().Where(p => p.TreePId == parentDetailId && p.SparePartId == detail.SparePartId).ToList();
            if (childs.Any())
                throw new ValidationException("降级失败，降级的层级里已存在该备件，请检查！".L10N());
            else
                DB.Update<EquipBomDetail>().Set(p => p.TreePId, parentDetailId).Where(p => p.Id == detailId).Execute();
        }

        /// <summary>
        /// 验证设备BOM（设备型号不可重复）
        /// </summary>
        /// <param name="entity">设备BOM实体</param> 
        /// <returns>bool</returns>
        public virtual bool VerifyEquipModelIsRepeat(EquipBom entity)
        {
            return Query<EquipBom>()
                .Where(p => p.Id != entity.Id)
                .Where(p => p.EquipModelId == entity.EquipModelId).ToList().Count > 0;
        }

        /// <summary>
        /// 验证设备BOM明细（备件编码不可重复）
        /// </summary>
        /// <param name="entity">设备BOM明细实体</param> 
        /// <returns>bool</returns>
        public virtual bool VerifySparePartIsRepeat(EquipBomDetail entity)
        {
            return Query<EquipBomDetail>()
                .Where(p => p.Id != entity.Id)
                .Where(p => p.SparePartId == entity.SparePartId && p.TreePId == entity.TreePId && p.EquipBomId == entity.EquipBomId).ToList().Count > 0;
        }

        /// <summary>
        /// 通过设备型号ID获取设备BOM明细
        /// </summary>
        /// <param name="modelId">设备型号ID</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="orderInfos">排序</param>
        /// <returns>设备BOM明细</returns>
        public virtual EntityList<EquipBomDetail> GetEquipBomDetailsByModelId(double modelId, IList<OrderInfo> orderInfos, PagingInfo pagingInfo)
        {
            var query = Query<EquipBomDetail>();

            query.Where(p => p.EquipBom.EquipModelId == modelId);
            if (orderInfos != null && orderInfos.Count > 0)
                query.OrderBy(orderInfos);

            var elo = new EagerLoadOptions();
            elo.LoadWith(EquipBomDetail.SparePartProperty);
            elo.LoadWithViewProperty();

            var list = query.ToList(pagingInfo, elo);
            return list;
        }

        /// <summary>
        /// 根据设备型号获取设备BOM
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public virtual EquipBom GetEquipBomByModelId(double modelId)
        {
            return Query<EquipBom>().Where(m => m.EquipModelId == modelId).FirstOrDefault();
        }

        /// <summary>
        /// 获取除选择外且有备件清单的数据
        /// </summary>
        /// <param name="ecpIds"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EntityList<EquipBomSelect> GetEquipBomsExceptIds(List<double> ecpIds)
        {
            return ecpIds.SplitContains(tempIds =>
            {
                return Query<EquipBomSelect>().Where(p => !tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 复制选择设备bom的明细
        /// </summary>
        /// <param name="copyIds"></param>
        /// <param name="sourceId"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void CopyDetailCommand(List<double> copyIds, double? sourceId)
        {
            if (sourceId == null || sourceId == 0)
            {
                throw new ValidationException("没有可提交的数据".L10N());
            }
            var bomDetail = Query<EquipBomDetail>().Where(p => p.EquipBomId == sourceId || copyIds.Contains(p.EquipBomId)).ToList();
            // 被复制的明细
            var copyDetailList = bomDetail.Where(p => p.EquipBomId == sourceId).ToList();
            // 删除原明细
            var delDetailList = bomDetail.Where(p => copyIds.Contains(p.EquipBomId)).AsEntityList();
            delDetailList.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);

            EntityList<EquipBomDetail> equipBomDetails = new EntityList<EquipBomDetail>();
            foreach (var mainId in copyIds)
            {
                foreach (var detail in copyDetailList)
                {
                    var copyDetail = new EquipBomDetail {
                        SparePartId = detail.SparePartId,
                        SparePartSite = detail.SparePartSite,
                        SparePartQty = detail.SparePartQty,
                        StockQty = detail.StockQty,
                    };
                    copyDetail.EquipBomId = mainId;
                    copyDetail.GenerateId();
                    copyDetail.CopyFromId = detail.Id;
                    if (detail.TreePId == null)
                    {
                        copyDetail.TreePId = null;
                    }
                    else
                    {
                        var newTreeP = equipBomDetails.FirstOrDefault(p => p.EquipBomId == mainId && p.CopyFromId == detail.TreePId);
                        copyDetail.TreePId = newTreeP.Id;
                    }
                    equipBomDetails.Add(copyDetail);
                }
            }
            using(var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(delDetailList);
                RF.BatchInsert(equipBomDetails);
                tran.Complete();
            }
        }

        /// <summary>
        /// 查询选择设备BOM
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EntityList<EquipBomSelect> QuerySelectEquipBom(EquipBomSelectCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<EquipBomSelect>();
            }
            var query = Query<EquipBomSelect>();
            if (criteria.Code.IsNotEmpty())
            {
                query.Where(p => p.EquipModel.Code.Contains(criteria.Code));
            }
            if (criteria.Name.IsNotEmpty())
            {
                query.Where(p => p.EquipModel.Name.Contains(criteria.Name));
            }
            if (criteria.ExceptIds.IsNotEmpty())
            {
                var ids = new List<double>();
                criteria.ExceptIds.Split(',').ForEach(p =>
                {
                    double.TryParse(p, out var val);
                    ids.Add(val);
                });
                query.Where(p => !ids.Contains(p.Id));
            }
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
