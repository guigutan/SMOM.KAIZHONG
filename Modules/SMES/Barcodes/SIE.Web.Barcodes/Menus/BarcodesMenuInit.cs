using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.Barcodes;
using SIE.Barcodes.Panels;
using SIE.Barcodes.WipBatchs;

namespace SIE.Web.Barcodes
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class BarcodesMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();

            res.Add(new MenuDto()
            {
                TreeKey = "MES",
                Label = "条码管理",
                IsLeafNode = false,
            });

            const string mesBarcode = "MES.条码管理";

            res.Add(new MenuDto()
            {
                TreeKey = mesBarcode,
                Label = "条码打印",
                EntityType = typeof(PrintWorkOrder)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesBarcode,
                Label = "条码补打",
                EntityType = typeof(BarcodeReprint)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesBarcode,
                Label = "条码报废",
                EntityType = typeof(Barcode)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesBarcode,
                Label = "条码挂起",
                EntityType = typeof(BarcodePending)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesBarcode,
                Label = "条码打印日志",
                EntityType = typeof(BarcodeLog)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesBarcode,
                Label = "批次生成",
                EntityType = typeof(BatchWorkOrder)
            });
            //res.Add(new MenuDto()
            //{
            //    TreeKey = mesBarcode,
            //    Label = "拼板码打印",
            //    EntityType = typeof(PanelWorkOrder)
            //});

            return res;
        }

    }
}
