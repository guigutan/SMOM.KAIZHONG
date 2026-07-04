using SIE.Items;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 设置相同物料产品bom缺省
    /// </summary>
    [Command(ImageName = "SetDefault", Label = "设为缺省", GroupType = 60)]
    public class ProductBomDefaultCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var bom = view.Current as ProductBom;
            return bom != null && !bom.IsDefault;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var bom = view.Current as ProductBom;
            if (bom == null) return;
            if (CRT.MessageService.AskQuestion("是否将产品BOM[{0}]设置为缺省?".L10nFormat(bom.Name)))
            {
                RT.Service.Resolve<ItemController>().SetDefaultProductBom(bom.ProductId, bom.Id);
                if (view.DataLoader.AnyLoaded)
                    view.DataLoader.ReloadDataAsync();
            }
        }
    }
}