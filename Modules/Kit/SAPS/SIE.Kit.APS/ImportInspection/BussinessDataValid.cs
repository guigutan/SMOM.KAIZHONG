using SIE.Common.Catalogs;
using SIE.Common.ImportHelper;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Items;
using SIE.Items.Units;
using SIE.Kit.APS.Common;
using SIE.Kit.APS.ProductLocations;
using SIE.SO.SaleOrders;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace SIE.Kit.APS.ImportInspection
{
    /// <summary>
    /// 导入功能业务数据的验证(公共)
    /// </summary>
    public class BussinessDataValid
    {
        #region 业务数据验证
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
                    messageTip = "不存在于系统".L10N();
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
                    messageTip = "不存在于系统".L10N();
                    isValid = false;
                }
            }
            return isValid;
        }

        /// <summary>
        /// 验证销售人员
        /// </summary>
        /// <param name="EmployeeNameDic">销售范围</param>
        /// <param name="context">销售编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidEmployee(ref Dictionary<string, Employee> EmployeeNameDic, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (EmployeeNameDic == null)
            {
                EmployeeNameDic = new Dictionary<string, Employee>();
            }

            if (!EmployeeNameDic.ContainsKey(context))
            {
                Employee employee = RT.Service.Resolve<EmployeeController>().GetEmployeeByName(context);
                if (employee != null)
                {
                    EmployeeNameDic.Add(context, employee);
                }
                else
                {
                    messageTip = "不存在于系统".L10N();
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
                Enterprise enterprise = RT.Service.Resolve<EnterpriseController>().GetEnterprises(context, null);
                if (enterprise != null)
                {
                    EnterpriseCodeDic.Add(context, enterprise);
                }
                else
                {
                    messageTip = "不存在于系统".L10N();
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
                    messageTip = "不存在于系统".L10N();
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


        /// <summary>
        /// 验证数量
        /// </summary>
        /// <param name="qty">数量</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidInt(String qty, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (!string.IsNullOrEmpty(qty))
            {
                var reg = new Regex(@"^[0-9]\d*$");
                var result = reg.IsMatch(qty);
                if (!result)
                {
                    messageTip = "必须是正整数".L10N();
                    isValid = false;
                }
            }
            return isValid;
        }

        /// <summary>
        /// 验证分类(产品定位)
        /// </summary>
        /// <param name="ClassificationDic">检验分类范围</param>
        /// <param name="context">标准类型</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidClassification(ref Dictionary<string, Enum> ClassificationDic, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (ClassificationDic == null)
            {
                ClassificationDic = ImportExtension.GetEnumLabel(typeof(Classification), string.Empty);
            }
            if (!ClassificationDic.ContainsKey(context))
            {
                messageTip = "只能选择".L10N() + string.Join("、", ClassificationDic.Keys);
                isValid = false;
            }
            return isValid;
        }
        /// <summary>
        /// 验证分类值(产品定位)
        /// </summary>
        /// <param name="ClassificationDic"></param>
        /// <param name="context"></param>
        /// <param name="messageTip"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool ValidTypeValue(ref Dictionary<Classification, List<string>> ClassificationDic, string context, out string messageTip, Classification c)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (ClassificationDic == null)
                ClassificationDic = new Dictionary<Classification, List<string>>();

            List<string> tmpValues = null;
            if (!ClassificationDic.TryGetValue(c, out tmpValues))
            {
                EntityList<ClassificationInfo> list = RT.Service.Resolve<ClassificationInfoController>().GetClassificationInfoList(c, null, null);
                ClassificationDic.Add(c, list.Select(p => p.Value).ToList());
            }
            tmpValues = ClassificationDic[c];

            if (!tmpValues.Contains(context))
            {
                isValid = false;
                messageTip = "只能选择".L10N() + string.Join("、", tmpValues);
            }
            return isValid;
        }
        #endregion

        /// <summary>
        /// 验证特殊工艺枚举
        /// </summary>
        /// <param name="ProcessDic">检验特殊工艺类型</param>
        /// <param name="context">标准类型</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidProcess(ref Dictionary<string, Enum> ProcessDic, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (ProcessDic == null)
            {
                ProcessDic = ImportExtension.GetEnumLabel(typeof(Process), string.Empty);
            }
            if (!ProcessDic.ContainsKey(context))
            {
                messageTip = "只能选择".L10N() + string.Join("、", ProcessDic.Keys);
                isValid = false;
            }
            return isValid;
        }
    }
}

