using SIE.Common.Prints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 人员条码化打印
    /// </summary>
    [Serializable]
    //[System.ComponentModel.DisplayName("人员条码化打印")]
    [DisplayName("人员条码化打印")]
    public class EmployeePrintable : LabelPrintable<Employee>
    {
        public override IEnumerable<String> GetPropertys(Type type = null)
        {
            var propertys = new List<String>();
            propertys.Add("员工工号");
            propertys.Add("员工姓名");
            return propertys;
        }
        public override string ConverterData(object data)
        {
            var content = string.Empty;
            var employee = data as Employee;
            if (employee != null)
            {
                //var catalog = RT.Service.Resolve<CatalogController>().GetCatalog();
                content += employee.Code + Separator
                    + employee.Name + Separator
                    ;
                //content = itemLabel.Label + itemLabel.Lot + itemLabel.Qty;
            }
            return content;
        }
    }
}
