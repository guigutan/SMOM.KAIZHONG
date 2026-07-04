using SIE.Core.Equipments;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.ItemEquipAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ItemEquipAccount
{
    /// <summary>
    /// 模具与产品关系控制器
    /// </summary>
    public class EquipAccountItemController : DomainController
    {
        #region 查询模具与产品关系

        /// <summary>
        /// 根据产品编码和模具编码、工序编码获取模具与产品的关系
        /// </summary>
        /// <param name="processCodes"></param>
        /// <param name="equipIds"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountItem> GetEquipAccountItemsByItemCodesEquipCodesProcessCodes(List<string> itemCodes, List<string> equipCodes, List<string> processCodes)
        {
            var equipAccountItems = processCodes.SplitContains(pcs =>
                {
                    return itemCodes.SplitContains(ics =>
                    {
                        return equipCodes.SplitContains(ecs =>
                        {
                            return Query<EquipAccountItem>().Where(p => pcs.Contains(p.Process.Code) && ics.Contains(p.Item.Code) && ecs.Contains(p.EquipAccount.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                        });
                    });
                });
            return equipAccountItems;
        }

        /// <summary>
        /// 根据产品编码和模具Id获取模具与产品的关系
        /// </summary>
        /// <param name="processCodes"></param>
        /// <param name="equipIds"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountItem> GetEquipAccountItemsByItemCodesEquipCodes(List<string> itemCodes, List<string> equipCodes)
        {
            var equipAccountItems = itemCodes.SplitContains(ics =>
            {
                return equipCodes.SplitContains(ecs =>
                {
                    return Query<EquipAccountItem>().Where(p => ics.Contains(p.Item.Code) && ecs.Contains(p.EquipAccount.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
            });
            return equipAccountItems;
        }

        /// <summary>
        /// 根据工序编码和模具Id获取模具与产品的关系
        /// </summary>
        /// <param name="processCodes"></param>
        /// <param name="equipIds"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountItem> GetEquipAccountItemsByprocessCodesEquipCodes(List<string> processCodes,List<string> equipCodes)
        {
            var equipAccountItems = processCodes.SplitContains(pcs =>
            {
                return equipCodes.SplitContains(ecs =>
                {
                    return Query<EquipAccountItem>().Where(p => pcs.Contains(p.Process.Code) && ecs.Contains(p.EquipAccount.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
            });
            return equipAccountItems;
        }

        /// <summary>
        /// 根据工序编码和物料编码获取模具与产品的关系
        /// </summary>
        /// <param name="processCodes"></param>
        /// <param name="itemCodes"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountItem> GetEquipAccountItemsByProCodesItems(List<string> processCodes, List<string> itemCodes)
        {
            var equipAccountItems = processCodes.SplitContains(pcs =>
            {
                return itemCodes.SplitContains(ics =>
                {
                    return Query<EquipAccountItem>().Where(p => pcs.Contains(p.Process.Code) && ics.Contains(p.Item.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
            });
            return equipAccountItems;
        }

        /// <summary>
        /// 查询模具与产品关系
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<EquipAccountItem> CriterialEquipAccountItem(EquipAccountItemCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("模具与产品关系查询实体异常！".L10N());
            }
            var query = Query<EquipAccountItem>();
            if (criterial.ItemId.HasValue)
            {
                if (criterial.ItemId > 0)
                {
                    query.Where(p => p.ItemId == criterial.ItemId);
                }
            }
            if (criterial.ProcessId.HasValue)
            {
                query.Where(p => p.ProcessId == criterial.ProcessId);
            }
            //if (criterial.EquipAccountId.HasValue)
            //{
            //    query.Where(p => p.EquipAccountId == criterial.EquipAccountId);
            //}
            if (!criterial.ItemName.IsNullOrEmpty())
            {
                query.Where(m => m.Item.Name.Contains("%" + criterial.ItemName + "%"));
            }
            if (!criterial.OldItem.IsNullOrEmpty())
            {
                query.Where(m => m.Item.ShortDescription.Contains("%" + criterial.OldItem + "%"));
            }
            if (!criterial.ItemMtart.IsNullOrEmpty())
            {
                query.Where(p => p.Item.Mtart.Contains(criterial.ItemMtart));
            }
            if (!criterial.ProcessCode.IsNullOrEmpty())
            {
                query.Where(p => p.Process.Code.Contains(criterial.ProcessCode));
            }
            if (!criterial.UniqueCode.IsNullOrEmpty())
            {
                query.Where(p => p.UniqueCode.Contains(criterial.UniqueCode));
            }
            if (!criterial.EquipAccountName.IsNullOrEmpty())
            {
                query.Where(p => p.EquipAccount.Name.Contains(criterial.EquipAccountName));
            }
            if (!criterial.EquipAccountCode.IsNullOrEmpty())
            {
                query.Where(p => p.EquipAccount.Code.Contains(criterial.EquipAccountCode));
            }
            if (!criterial.Drawn.IsNullOrEmpty())
                query.Where(p => p.EquipAccount.Drawn.Contains(criterial.Drawn));
            return query.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 启用模具与产品关系
        /// <summary>
        /// 启用模具与产品关系
        /// </summary>
        /// <returns></returns>
        public virtual void EnableEquipAccountItem(List<double> LineIds)
        {

        }
        #endregion

        #region 禁用模具与产品关系
        /// <summary>
        /// 禁用模具与产品关系
        /// </summary>
        /// <param name="LineIds"></param>
        public virtual void DisableEquipAccountItem(List<double> LineIds)
        {

        }
        #endregion

        #region 是否有相同的数据
        /// <summary>
        /// 是否有相同的数据
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="processId"></param>
        /// <param name="wipId"></param>
        /// <returns></returns>
        public virtual bool GetEquipAccountItemBool(double itemId, double processId, double EquipAccountId)
        {
            var query = Query<EquipAccountItem>().Where(p => p.ItemId == itemId && p.ProcessId == processId && p.EquipAccountId == EquipAccountId).ToList();
            if (query.Count > 0)
                return true;
            else
                return false;
        }
        #endregion

        /// <summary>
        /// 按照生产设备获取设备台账维护
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetEquipAccounts(PagingInfo info, string code)
        {
            var equipType = Query<EquipType>().Where(p => p.TypeName == "模具设备").FirstOrDefault();
            if (equipType == null)
            {
                throw new ValidationException("请先在设备类型维护,维护模具设备类型!");
            }
            var equipTypeId = equipType.Id;
            var q = Query<EquipAccount>();
            q.Join<EquipModel>((x, y) => x.EquipModelId == y.Id)
            .Where<EquipModel>((x, y) => y.EquipTypeId == equipTypeId);
            if (code != null)
            {
                q.Where(p => p.Code.Contains(code));
            }
            return q.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 查询模具条码化数据
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public virtual List<SIE.MES.ItemEquipAccount.EquipAccountItem> GetEquipAccountItemPrintDatas(List<double> Ids)
        {
            List<SIE.MES.ItemEquipAccount.EquipAccountItem> equipAccountItemDatas = new List<SIE.MES.ItemEquipAccount.EquipAccountItem>();
            Ids.SplitDataExecute(DataBarcodeIds =>
            {
                var list = Query<SIE.MES.ItemEquipAccount.EquipAccountItem>()
                    .Where(x => DataBarcodeIds.Contains(x.Id))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                equipAccountItemDatas.AddRange(list);
            });
            return equipAccountItemDatas;
        }

        /// <summary>
        /// 校验产品是否与产品组模具匹配
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual bool ValidateModelGroup(double itemId, List<double> itemIds)
        {
            var list = Query<EquipAccountItem>().Where(p => p.ItemId == itemId).ToList();
            if (list.All(p => p.UniqueCode.IsNullOrEmpty()))
                throw new ValidationException("当前产品未维护模具组");

            var codes = list.Where(p => p.UniqueCode.IsNotEmpty()).Select(p => p.UniqueCode).ToList();
            if (itemIds.Count > 0)
            {
                var list2 = Query<EquipAccountItem>().Where(p => itemIds.Contains(p.ItemId)).ToList();
                var codes2 = list2.Where(p => p.UniqueCode.IsNotEmpty()).Select(p => p.UniqueCode).ToList();
                if (codes.Intersect(codes2).Count() == 0)
                    throw new ValidationException("当前产品与生产队列中的产品没有相同的模具组");
            }
            return true;
        }
    }
}
