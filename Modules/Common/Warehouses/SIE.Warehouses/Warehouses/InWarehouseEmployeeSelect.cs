using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.WMS.Inventory;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Warehouses.Warehouses
{
    /// <summary>
    /// 仓库视图 （只为做选择视图）
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(InWarehouseEmployeeSelectCriteria))]
    [Label("仓库")]
    [DisplayMember(nameof(Name))]
    public class InWarehouseEmployeeSelect:Warehouse
    {

    }

    /// <summary>
    /// 仓库 实体配置
    /// </summary>
    internal class InWarehouseEmployeeSelectConfig : EntityConfig<InWarehouseEmployeeSelect>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var wareHouse = o.CastTo<Warehouse>();
                    var oldWareHouse = RF.GetById<Warehouse>(wareHouse.Id);
                    if (oldWareHouse != null)
                    {
                        if (wareHouse.IsLineWarehouse != oldWareHouse.IsLineWarehouse)
                        {
                            //说明已经更改了线边仓选项 判断该仓库是否有库存
                            var flag = RT.Service.Resolve<IGetLotLpnOnhand>().IsWareHouseHasOnHand(wareHouse.Id);
                            if (flag)
                            {
                                e.BrokenDescription = "当前仓库[{0}]已存在现有量大于0的库存数据，无法更改其线边仓属性".L10nFormat(wareHouse.Code);
                            }
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WH").MapAllProperties();
            Meta.Property(Warehouse.InvOrgNameProperty).DontMapColumn();
            Meta.Property(Warehouse.InvOrgCodeProperty).DontMapColumn();
            Meta.EnablePhantoms();
            //  Meta.EnableSort();
        }
    }
}
