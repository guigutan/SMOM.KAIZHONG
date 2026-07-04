using SIE.CSM.Customers;
using SIE.Domain;

namespace SIE.Wpf.CSM.Customers.ViewBehaviors
{
    /// <summary>
    /// 顾客变更行为
    /// </summary>
    public class CustomerChangeBehavior : ViewBehavior
    {
        /// <summary>
        /// 是否正在更改
        /// </summary>
        private bool isRun;

        /// <summary>
        /// 当前实体
        /// </summary>
        private Entity Current;

        /// <summary>
        /// 附加
        /// </summary>
        protected override void OnAttach()
        {
            var view = View as ListLogicalView;
            if (view != null)
            {
                view.CurrentChanged -= Customer_CurrentChanged;
                view.CurrentChanged += Customer_CurrentChanged;
            }
        }

        /// <summary>
        /// 当前顾客对象变更
        /// </summary>
        /// <param name="sender">当前变更的视图对象</param>
        /// <param name="e">事件参数</param>
        private void Customer_CurrentChanged(object sender, System.EventArgs e)
        {
            ListLogicalView logicalView = sender as ListLogicalView;
            if (logicalView != null)
            {
                if (Current != null)
                {
                    Current.PropertyChanged -= Customer_PropertyChanged;
                }

                Customer customer = logicalView.Current as Customer;
                Current = customer;
                if (customer != null && (customer.PersistenceStatus == Domain.PersistenceStatus.New || customer.PersistenceStatus == Domain.PersistenceStatus.Modified))
                {
                    customer.PropertyChanged -= Customer_PropertyChanged;
                    customer.PropertyChanged += Customer_PropertyChanged;
                }
            }
        }

        /// <summary>
        /// 顾客属性变更
        /// </summary>
        /// <param name="sender">变更对象</param>
        /// <param name="e">变更事件</param>
        private void Customer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!isRun)
            {
                isRun = true;
                try
                {
                    if (e.PropertyName == Customer.CustomerTypeProperty.Name)
                    {
                        Customer customer = sender as Customer;
                        if (customer != null)
                        {
                            customer.SupplierId = null;
                            customer.Supplier = null;
                        }
                    }
                }
                finally
                {
                    isRun = false;
                }
            }
        }
    }
}
