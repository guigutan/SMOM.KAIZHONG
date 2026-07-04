using SIE.Domain;
using SIE.ERPInterface.Common.UploadTransactionRules;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.ERPInterface.UploadTransactionRules.Commands
{
    /// <summary>
    /// 交易上传规则选择仓库
    /// </summary>
    public class TransactionWarehouseLookupCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var transcWarehouseList = args.Data.ToJsonObject<List<UploadTransactionExclWh>>();
            Check.NotNullOrEmpty(transcWarehouseList, nameof(transcWarehouseList));
            if (transcWarehouseList == null || transcWarehouseList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(transcWarehouseList)));
            }
            foreach (var item in transcWarehouseList)
            {
                var transcWarehouse = new UploadTransactionExclWh();
                transcWarehouse.WarehouseId = item.WarehouseId;
                transcWarehouse.UploadTransactionRuleId = item.UploadTransactionRuleId;
                savedData.Add(transcWarehouse);
            }
            RF.Save(savedData);
            return true;
        }
    }
}
