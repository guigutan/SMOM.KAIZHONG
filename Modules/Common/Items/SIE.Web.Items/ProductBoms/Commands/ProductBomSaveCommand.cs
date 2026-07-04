using SIE.Domain;
using SIE.Items;
using SIE.Items.ProductBoms;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Items.ProductBoms.Commands
{
    /// <summary>
    /// 产品BOM保存命令
    /// </summary>
    [JsCommand("SIE.Web.Items.ProductBoms.Commands.ProductBomSaveCommand")]
    public class ProductBomSaveCommand : SaveCommand
    {
        /// <summary>
        /// 产品BOM保存命令
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            EntityList<ProductBom> proBomList = data as EntityList<ProductBom>;
            proBomList.ForEach(p => p.ExtValues.Clear());//产品属性不在这里保存
            RT.Service.Resolve<ProductBomController>().ValidateSumit(data);
            ProcessBomData(proBomList);

            base.OnSaving(data);
        }

        /// <summary>
        /// 处理产品BOM下的所有物料属性
        /// </summary>
        /// <param name="proBomList">产品BOM列表</param>
        private void ProcessBomData(EntityList<ProductBom> proBomList)
        {
            foreach (var poBom in proBomList)
            {
                //// 处理产品BOM明细下的物料属性
                //List<ProductBomDetail> details = poBom.DetailList.Where(p => !string.IsNullOrEmpty(p.PropertyValueJson)).ToList();
                //foreach (var detail in details)
                //{
                //    List<ProductBomDetailPropertyValue> proValues = detail.PropertyValueJson.ToJsonObject<List<ProductBomDetailPropertyValue>>();
                //    MergeItemPropertyValue(detail, proValues);
                //}

                // 处理组合替代下的物料属性
                List<CombinationReplate> replates = poBom.CombinationReplateList.Where(p => !string.IsNullOrEmpty(p.PropertyValueJson)).ToList();
                foreach (var replate in replates)
                {
                    List<CombinationReplatePropertyValue> proValues = replate.PropertyValueJson.ToJsonObject<List<CombinationReplatePropertyValue>>();
                    MergeReplateItemPropertyValue(replate, proValues);
                }
            }
        }

        ///// <summary>
        ///// 处理产品BOM明细下的物料属性
        ///// </summary>
        ///// <param name="detail">产品BOM明细</param>
        ///// <param name="uiDataList">新勾选的值</param>
        //private void MergeItemPropertyValue(ProductBomDetail detail, List<ProductBomDetailPropertyValue> uiDataList)
        //{
        //    EntityList<ProductBomDetailPropertyValue> dbDataList = detail.PropertyValueList;
        //    List<ProductBomDetailPropertyValue> oldDbDataList = new List<ProductBomDetailPropertyValue>();
        //    List<ProductBomDetailPropertyValue> newDbDataList = new List<ProductBomDetailPropertyValue>();
        //    foreach (var uiData in uiDataList)
        //    {
        //        var dbData = dbDataList.FirstOrDefault(p => p.DefinitionId == uiData.DefinitionId && p.Value == uiData.Value && p.PropertyGroup == uiData.PropertyGroup);
        //        if (dbData != null)
        //        {
        //            oldDbDataList.Add(dbData);
        //        }
        //        else
        //        {
        //            ProductBomDetailPropertyValue newValue = new ProductBomDetailPropertyValue();
        //            newValue.DefinitionId = uiData.DefinitionId;
        //            newValue.PropertyGroup = uiData.PropertyGroup;
        //            newValue.Value = uiData.Value;
        //            newValue.DetailId = detail.Id;
        //            newDbDataList.Add(newValue);
        //        }
        //    }

        //    var delDataList = dbDataList.Where(p => !oldDbDataList.Contains(p)).ToList();
        //    delDataList.ForEach(p => dbDataList.Remove(p));
        //    dbDataList.AddRange(newDbDataList);
        //}

        /// <summary>
        /// 处理组合替代下的物料属性
        /// </summary>
        /// <param name="comReplate">组合替代</param>
        /// <param name="uiDataList">新勾选的值</param>
        private void MergeReplateItemPropertyValue(CombinationReplate comReplate, List<CombinationReplatePropertyValue> uiDataList)
        {
            EntityList<CombinationReplatePropertyValue> dbDataList = comReplate.PropertyValueList;
            List<CombinationReplatePropertyValue> oldDbDataList = new List<CombinationReplatePropertyValue>();
            List<CombinationReplatePropertyValue> newDbDataList = new List<CombinationReplatePropertyValue>();
            foreach (var uiData in uiDataList)
            {
                var dbData = dbDataList.FirstOrDefault(p => p.DefinitionId == uiData.DefinitionId && p.Value == uiData.Value && p.PropertyGroup == uiData.PropertyGroup);
                if (dbData != null)
                {
                    oldDbDataList.Add(dbData);
                }
                else
                {
                    CombinationReplatePropertyValue newValue = new CombinationReplatePropertyValue();
                    newValue.DefinitionId = uiData.DefinitionId;
                    newValue.PropertyGroup = uiData.PropertyGroup;
                    newValue.Value = uiData.Value;
                    newValue.CombinationReplateId = comReplate.Id;
                    newDbDataList.Add(newValue);
                }
            }

            var delDataList = dbDataList.Where(p => !oldDbDataList.Contains(p)).ToList();
            delDataList.ForEach(p => dbDataList.Remove(p));
            dbDataList.AddRange(newDbDataList);
        }
    }
}