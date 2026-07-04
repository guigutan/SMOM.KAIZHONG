using SIE.Domain;
using SIE.Items;
using SIE.Wpf.Command;

namespace SIE.Wpf.Items.ProductModels.Commands
{
    [Command(Label = "添加", ImageName = "AddEntity", GroupType = CommandGroupType.Edit)]
    class AddProductModelCapacityCommand : ListAddCommand
    {
        protected override Entity CreateNewItem()
        {
            var productModelLineCapacity = base.CreateNewItem() as ProductModelLineCapacity;
            var parent = View.Parent.Current as ProductModel;
            productModelLineCapacity.ProductModelId = parent.Id;
            return productModelLineCapacity;
        }
    }
}
