using SIE.MetaModel;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.InventoryPlans
{
    [RootEntity,Serializable]
    [ConditionQueryType(typeof(WarehouseCriteria))]
    public class Warehouse : SIE.Warehouses.Warehouse
    {
    }
}
