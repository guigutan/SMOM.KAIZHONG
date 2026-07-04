using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.TurnoverTools.TurnoverTools
{
    /// <summary>
    /// 前加工参数验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("周转箱型号列表验证")]
    [System.ComponentModel.Description("周转箱型号列表验证")]
    public class TurnoverToolModelRule : EntityRule<TurnoverToolModel>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public TurnoverToolModelRule()
        {
            Scope = MetaModel.EntityStatusScopes.Add | MetaModel.EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(Domain.IEntity entity, MetaModel.RuleArgs e)
        {
            var turnoverToolModel = entity as TurnoverToolModel;
            if (turnoverToolModel != null)
            {
                var dbToolModel = RF.GetById<TurnoverToolModel>(turnoverToolModel.Id, new EagerLoadOptions().LoadWithViewProperty());
                if (dbToolModel != null)
                {
                    var items = new List<TurnoverToolModelInProduct>();
                    var orgItemsList = dbToolModel.ProductList;
                    var deleteItmes = turnoverToolModel.ProductList.DeletedList.Cast<TurnoverToolModelInProduct>();
                    var deleteItmeIds = deleteItmes.Select(p => p.Id).Distinct();
                    items.AddRange(orgItemsList.Where(p => !deleteItmeIds.Contains(p.Id)));
                    foreach (var item in turnoverToolModel.ProductList)
                    {
                        if (!items.Any(p => p.Id == item.Id))
                            items.Add(item);
                        else
                            items[items.FindIndex((p => p.Id == item.Id))] = item;
                    }
                    var duplicateItems = items.GroupBy(x => x.ProductId).Where(x => x.Count() > 1).ToList();
                    if (duplicateItems.Count > 0)
                        e.BrokenDescription = "产品容量的物料维护了多笔物料数据，物料编码必须唯一！".L10N();
                }

            }

        }
    }
}
