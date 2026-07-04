using SIE.Common.Catalogs;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Items;
using SIE.Items.Units;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 验证备件资料导入
    /// </summary>
    public class ValidSparePartDataHelper
    {
        private const string NOT_EXISTS_IN_SYSTEM = "不存在于系统";

        /// <summary>
        /// 构造函数
        /// </summary>
        protected ValidSparePartDataHelper() { }

        /// <summary>
        /// 验证物料
        /// </summary>
        /// <param name="itemCodeDic">物料范围</param>
        /// <param name="context">物料编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidItem(ref Dictionary<string, Item> itemCodeDic, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (itemCodeDic == null)
            {
                itemCodeDic = new Dictionary<string, Item>();
            }
            if (!itemCodeDic.ContainsKey(context))
            {
                Item item = RT.Service.Resolve<ItemController>().GetItemFromCode(context);
                if (item != null)
                {
                    itemCodeDic.Add(context, item);
                }
                else
                {
                    messageTip = NOT_EXISTS_IN_SYSTEM.L10N();
                    isValid = false;
                }
            }
            return isValid;
        }

        /// <summary>
        /// 验证客户
        /// </summary>
        /// <param name="CustomerNameDic">客户范围</param>
        /// <param name="context">客户编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidCustomer(ref Dictionary<string, Customer> CustomerNameDic, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (CustomerNameDic == null)
            {
                CustomerNameDic = new Dictionary<string, Customer>();
            }
            if (!CustomerNameDic.ContainsKey(context))
            {
                Customer customer = RT.Service.Resolve<CustomerController>().GetCustomerFromCode(context);
                if (customer != null)
                {
                    CustomerNameDic.Add(context, customer);
                }
                else
                {
                    messageTip = NOT_EXISTS_IN_SYSTEM.L10N();
                    isValid = false;
                }
            }
            return isValid;
        }

        /// <summary>
        /// 验证销售人员
        /// </summary>
        /// <param name="customerNameDic">销售范围</param>
        /// <param name="context">销售编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidEmployee(ref Dictionary<string, Employee> customerNameDic, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (customerNameDic == null)
            {
                customerNameDic = new Dictionary<string, Employee>();
            }
            if (!customerNameDic.ContainsKey(context))
            {
                Employee employee = RT.Service.Resolve<EmployeeController>().GetEmployeeByName(context);
                if (employee != null)
                {
                    customerNameDic.Add(context, employee);
                }
                else
                {
                    messageTip = NOT_EXISTS_IN_SYSTEM.L10N();
                    isValid = false;
                }
            }
            return isValid;
        }

        /// <summary>
        /// 验证库存组织
        /// </summary>
        /// <param name="EnterpriseCodeDic">库存组织范围</param>
        /// <param name="context">库存组织编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidEnterprise(ref Dictionary<string, Enterprise> EnterpriseCodeDic, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (EnterpriseCodeDic == null)
            {
                EnterpriseCodeDic = new Dictionary<string, Enterprise>();
            }
            if (!EnterpriseCodeDic.ContainsKey(context))
            {
                //只加载工厂类型的库存组织
                Enterprise enterprise = RT.Service.Resolve<EnterpriseController>().GetEnterprises(context, EnterpriseType.Plant);
                if (enterprise != null)
                {
                    EnterpriseCodeDic.Add(context, enterprise);
                }
                else
                {
                    messageTip = "不存在于系统或非工厂类型库存组织".L10N();
                    isValid = false;
                }
            }
            return isValid;
        }

        /// <summary>
        /// 验证单位
        /// </summary>
        /// <param name="UnitNameDic">单位范围</param>
        /// <param name="context">单位编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidUnit(ref Dictionary<string, Unit> UnitNameDic, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (UnitNameDic == null)
            {
                UnitNameDic = new Dictionary<string, Unit>();
            }
            if (!UnitNameDic.ContainsKey(context))
            {
                Unit employee = RT.Service.Resolve<UnitsController>().GetUnitFromName(context);
                if (employee != null)
                {
                    UnitNameDic.Add(context, employee);
                }
                else
                {
                    messageTip = NOT_EXISTS_IN_SYSTEM.L10N();
                    isValid = false;
                }
            }
            return isValid;
        }

        /// <summary>
        /// 验证快码类别
        /// </summary>
        /// <param name="categoryRange">检验类别范围</param>
        /// <param name="context">检验类别</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="categoryType">快码类型</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidCategory(ref Dictionary<string, string> categoryRange, string context, out string messageTip, string categoryType)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (categoryRange == null)
            {
                categoryRange = new Dictionary<string, string>();
                EntityList<Catalog> categoryList = RT.Service.Resolve<CatalogController>().GetCatalogList(categoryType);
                foreach (Catalog catalogItem in categoryList)
                {
                    categoryRange.Add(catalogItem.Name, catalogItem.Code);
                }
            }
            if (!categoryRange.ContainsKey(context))
            {
                messageTip = "只能选择".L10N() + string.Join("或", categoryRange.Keys);

                isValid = false;
            }
            return isValid;
        }
    }
}
