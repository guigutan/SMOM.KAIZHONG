namespace SIE.EventMessages
{
    [Services.Service(FallbackType = typeof(CustomerCreated))]
    public interface ICustomerCreated
    {
        void CreateCustLevelSetting(double CustomerId);
    }

    public class CustomerCreated : ICustomerCreated
    {
        public void CreateCustLevelSetting(double CustomerId)
        {
            return;
        }
    }


}
