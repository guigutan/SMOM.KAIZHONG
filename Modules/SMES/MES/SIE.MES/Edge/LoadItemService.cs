using SIE.Domain.Validation;
using SIE.MES.Edge.Models;
using SIE.MES.LoadItems;
using SIE.MES.SingleLabels;
using SIE.MES.WIP;
using SIE.MES.WIP.Repairs;
using SIE.MES.WIP.Runtime;
using SIE.Packages.ItemLabels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Edge
{
    /// <summary>
    /// 边缘上料服务
    /// </summary>
    public class LoadItemService : ILoadItemService
    {
        /// <summary>
        /// ID 格式字符串
        /// </summary>
        private const string ID_FORMAT = "#.###";

        private readonly LoadItemController loadItemCtr;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="loadItemCtr"></param>
        public LoadItemService(LoadItemController loadItemCtr)
        {
            this.loadItemCtr = loadItemCtr;
        }

        /// <summary>
        /// 获取条码物料信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<MaterialInfo> GetBarcodeInfo(EdgeCollectData data)
        {
            List<MaterialInfo> materialInfos = new List<MaterialInfo>();
            if (data == null)
            {
                return materialInfos;
            }
            if (data.MaterialLabels != null)
            {
                foreach (var barcode in data.MaterialLabels.Select(p => p.Barcode))
                {
                    var info = GetMaterialInfoByBarcode(barcode);
                    if (info.ItemId.IsNullOrEmpty())
                    {
                        throw new ValidationException("未找到可上料的配送单或标签条码、产品条码[{0}]".L10nFormat(barcode));
                    }
                    materialInfos.Add(info);
                }
            }
            return materialInfos;
        }


        /// <summary>
        /// 上料
        /// </summary>
        public List<MaterialInfo> LoadItem(EdgeCollectData data)
        {
            List<MaterialInfo> materialInfos = new List<MaterialInfo>();
            if (data == null)
            {
                return materialInfos;
            }

            if (data.Barcode.IsNotEmpty())
            {
                data.MaterialLabels = new List<EdgeMaterial>() {
                    new EdgeMaterial() { Barcode = data.Barcode}
                };
            }
            foreach (var label in data.MaterialLabels)
            {
                var info = data.Barcode.IsNotEmpty() ? GetMaterialInfoByBarcode(data.Barcode)
                    : new MaterialInfo()
                    {
                        Barcode = label.Barcode,
                        ItemId = label.ItemId,
                        ItemCode = label.ItemCode,
                        ItemName = label.ItemName,
                        Type = (SingleLabels.LoadItemSourceType)label.SourceType,
                        SourceId = double.Parse(label.SourceId),
                        Qty = label.Qty,
                        RemainQty = label.RemainQty
                    };

                if (info.ItemId.IsNullOrEmpty())
                {
                    throw new ValidationException("未找到可上料的配送单或标签条码、产品条码[{0}]".L10nFormat(info.Barcode));
                }

                if (data.Barcode.IsNotEmpty() && (info.Type == LoadItemSourceType.SN))
                {
                    throw new ValidationException("半成品[{0}]不能提前上料".L10nFormat(info.Barcode));
                }

                switch (info.Type)
                {
                    case LoadItemSourceType.ItemLabel:
                        loadItemCtr.LoadEdgeItemLabel(label.Barcode);
                        break;
                    case LoadItemSourceType.SN://创建了SKU标签的生产条码需更新物料标签的状态
                        loadItemCtr.LoadEdgeItemLabel(label.Barcode);
                        break;
                    default:
                        break;
                }
                materialInfos.Add(info);
            }
            return materialInfos;
        }

        /// <summary>
        /// 获取物料信息
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>物料信息</returns>
        public MaterialInfo GetMaterialInfoByBarcode(string barcode)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }
            var mateInfo = new MaterialInfo() { Barcode = barcode };
            decimal qty = 0m;
            decimal remainQty = 0m;
            string propertyValues = "";
            string propertyValuesName = "";
            var itemLabel = loadItemCtr.GetItemLabel(barcode);  //获取物料标签
            if (itemLabel == null)
            {
                //查找产品条码
                var version = loadItemCtr.GetProductVersion(barcode);
                if (version == null)
                {
                    return mateInfo;
                }
                mateInfo.Type = LoadItemSourceType.SN;

                propertyValues = version.Product.ItemExtProp;
                propertyValuesName= version.Product.ItemExtPropName;
                mateInfo.ItemId = version.Product.ItemId.ToString(ID_FORMAT);
                mateInfo.SourceId = version.Id;
                mateInfo.ItemCode = version.ProductCode;
                mateInfo.ItemName = version.ProductName;
                qty = 1;
                remainQty = 1;
            }
            else
            {
                if (itemLabel.SourceType == LabelSource.Wip)
                {
                    var version = loadItemCtr.GetProductVersion(barcode);
                    propertyValues = version.Product.ItemExtProp;
                    propertyValuesName = version.Product.ItemExtPropName;
                    mateInfo.Type = LoadItemSourceType.SN;
                    mateInfo.ItemId = version.Product.ItemId.ToString(ID_FORMAT);
                    mateInfo.SourceId = version.Id;
                    mateInfo.ItemCode = version.ProductCode;
                    mateInfo.ItemName = version.ProductName;
                    qty = 1;
                    remainQty = 1;
                }
            }

            mateInfo.ItemExtProp = propertyValues;
            mateInfo.ItemExtPropName = propertyValuesName;
            mateInfo.Qty = qty;
            mateInfo.RemainQty = remainQty;
            return mateInfo;
        }
    }
}
