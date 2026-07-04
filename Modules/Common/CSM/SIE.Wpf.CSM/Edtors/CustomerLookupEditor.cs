using SIE.CSM.Customers;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Wpf.Editors;

namespace SIE.Wpf.CSM.Edtors
{
    /// <summary>
    /// 客户编辑器
    /// </summary>
    public class CustomerLookupEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "CustomerLookupEditor";

        /// <summary>
        /// 重新数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">搜索关键词</param>
        /// <param name="titleProperty">IManagedProperty</param>
        /// <returns>EntityList</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            CustomerType customerType = CustomerType.CUSTOMER;
            var config = base.Config as CustomerLookUpEditorConfig;
            if (config!=null)
            {
                customerType = config.CustomerType;
            }

            return RT.Service.Resolve<CustomerController>().GetCustomer(customerType, keyword, pagingInfo);
        }
    }

    /// <summary>
    /// 客户编辑器配置类
    /// </summary>
    public class CustomerLookUpEditorConfig : PagingLookUpEditorConfig
    {
        /// <summary>
        /// 客户类型
        /// </summary>
        public CustomerType CustomerType
        {
            get { return GetProperty<CustomerType>(); }
            set { SetProperty(value); }
        }
    }
}
