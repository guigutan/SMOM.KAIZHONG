using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.CSM.Suppliers.Commands
{
    /// <summary>
    /// 供应商选择物料关系
    /// </summary>
    public class SelectUserCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args != null)
            {
                var meta = ClientEntities.Find(args.Type);//获取元数据
                var savedData = RF.Find(meta.EntityType).NewList();//新的数据集
                var supplierUserList = args.Data.ToJsonObject<List<SupplierUser>>();
                Check.NotNullOrEmpty(supplierUserList, nameof(supplierUserList));//检查空
                if (null == supplierUserList || supplierUserList.Count == 0)
                {
                    throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(supplierUserList)));
                }
                foreach (var item in supplierUserList)
                {
                    var supplierUser = new SupplierUser();
                    supplierUser.UserId = item.UserId;//用户
                    supplierUser.SupplierId = item.SupplierId;//供应商
                    savedData.Add(supplierUser);
                }
                RF.Save(savedData);//保存数据
            }
            return true;
        }
    }
}
