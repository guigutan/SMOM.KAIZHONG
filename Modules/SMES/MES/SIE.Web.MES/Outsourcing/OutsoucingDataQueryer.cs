using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.Outsourcing;
using SIE.MES.Outsourcing.Model;
using SIE.MES.WorkOrders;
using SIE.Web.Data;
using System;
using System.Text.RegularExpressions;
using static IronPython.Modules._ast;

namespace SIE.Web.MES.Outsourcing
{
    public class OutsoucingDataQueryer : DataQueryer
    {
        /// <summary>
        /// 添加工序委外需求单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual string Add(AddModel data)
        {
            var entity = data;
            if (entity != null)
            {
                if (entity.WorkOrderId == 0)
                {
                    throw new ValidationException("工单不允许为空！".L10N());
                }
                if (entity.Qty <= 0)
                {
                    throw new ValidationException("委外需求数量必须大于0！".L10N());
                }
                CheckQty(entity);
                if (entity.SupplierId <= 0)
                {
                    throw new ValidationException("供应商不允许为空！".L10N());
                }
                RT.Service.Resolve<OutsourcingRequestController>().SaveAddModel(entity);
            }
            return "";
        }


        /// <summary>
        /// 检查数量
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ValidationException"></exception>
        private void CheckQty(AddModel entity)
        {
            var wo = RF.GetById<WorkOrder>(entity.WorkOrderId);
            if (wo != null)
            {
                var item = RF.GetById<Item>(wo.ProductId);
                if (item != null)
                {
                    var retrospectType = RT.Service.Resolve<ItemController>().GetRetrospectType(item.Id);
                    if (retrospectType == Core.Items.RetrospectType.Single && !IsIntergerNonZero(entity.Qty.ToString()))//单体追溯只能输入正整数
                    {
                        throw new ValidationException("工单产品为单体追溯只能输入正整数！".L10N());
                    }

                }
            }
        }

        /// <summary>
        /// 添加临时委外
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual string TemporaryAdd(TemporaryAddModel data)
        {
            var entity = data;
            var index = 0;
            if (entity != null)
            {
                if (entity.WorkOrderId == 0)
                {
                    throw new ValidationException("工单不允许为空！".L10N());
                }
                if (entity.Qty <= 0)
                {
                    throw new ValidationException("委外需求数量必须大于0！".L10N());
                }
                CheckQty(entity);

                if (entity.SupplierId <= 0)
                {
                    throw new ValidationException("供应商不允许为空！".L10N());
                }
                if (entity.BeginRoutingProcess == null)
                {
                    throw new ValidationException("开始工序不允许为空！".L10N());
                }
                if (entity.EndRoutingProcess == null)
                {
                    throw new ValidationException("结束工序不允许为空！".L10N());
                }
                if (entity.EndRoutingProcess.Index < entity.BeginRoutingProcess.Index)
                {
                    throw new ValidationException("结束工序不能早于起始工序！".L10N());
                }
                index = RT.Service.Resolve<OutsourcingRequestController>().SaveTemporaryAddModel(entity);
            }
            return "保存成功，本次共生成{0}条委外需求单".L10nFormat(index);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIntergerNonZero(string str)
        {
            return Regex.IsMatch(str, @"^[1-9]\d*$");
        }
    }

}
