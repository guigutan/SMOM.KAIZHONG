using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Applys;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.SpareParts.Applys
{
    /// <summary>
    /// 备件申请单
    /// </summary>
    public class SparePartAppViewConfig : WebViewConfig<SparePartApp>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.AddBehavior("SIE.Web.EMS.Common.Script.ApprovalBehavior");//行为
            View.AddBehavior("SIE.Web.EMS.SpareParts.Applys.Behaviors.AppBehavior");//行为
            View.UseCommands(
                   "SIE.Web.EMS.SpareParts.Applys.Commands.AddAppCommand",//添加
                   "SIE.Web.EMS.SpareParts.Applys.Commands.EditAppCommand",//修改
                   "SIE.Web.EMS.SpareParts.Applys.Commands.DelAppCommand",//删除
                   "SIE.Web.EMS.SpareParts.Applys.Commands.SaveAppCommand",//保存
                   "SIE.Web.EMS.SpareParts.Applys.Commands.SubmitAppCommand",//提交
                    "SIE.Web.EMS.SpareParts.Applys.Commands.AuditAppCommand",//审核
                   "SIE.Web.EMS.SpareParts.Applys.Commands.CancelAppCommand"//取消
               );

            View.Property(p => p.No).Readonly().ShowInList(width: 120);
            View.Property(p => p.FromNo).Readonly().ShowInList(width: 120);
            View.Property(p => p.FromType).DefaultValue(0).Readonly();
            View.Property(p => p.DemandDate).UseDateEditor(p =>
                {
                    p.Format = "Y/m/d";
                    p.MinValue = DateTime.Now.ToString("yyyy/MM/dd");

                }).Readonly(p => p.AuditState != AuditState.Create && p.AuditState != AuditState.Returned);
            View.Property(p => p.AuditState).DefaultValue(0).Readonly();
            View.Property(p => p.QualityStatus).Readonly(p => p.AuditState != AuditState.Create && p.AuditState != AuditState.Returned);
            View.Property(p => p.EquipAccount)
                .UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EquipAccountSelectController>().GetEquipAccounts(keyword,pagingInfo);
                })
                .UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.EquipAccountName), nameof(e.EquipAccount.Name));
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.AuditState != AuditState.Create && p.AuditState != AuditState.Returned);

            View.Property(p => p.EquipAccountName).Readonly();
            View.Property(p => p.EquipModel).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EquipModelController>().GetEquipModels(pagingInfo, keyword);
            }).Readonly(p => ((p.AuditState != AuditState.Create && p.AuditState != AuditState.Returned) || p.EquipAccountId != null));

            View.Property(p => p.GetDepartment).UseDataSource((e, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EnterpriseController>().GetDepartmentsWithParent(pagingInfo, keyword);
                }).Readonly(p => p.AuditState != AuditState.Create && p.AuditState != AuditState.Returned);
            View.Property(p => p.Remark).HasLabel("审核意见").Readonly(p => p.AuditState != AuditState.Create && p.AuditState != AuditState.Returned);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No);
            View.Property(p => p.FromNo);
            View.Property(p => p.EquipAccount);
            View.Property(p => p.AuditState);
            View.Property(p => p.No);
            View.Property(p => p.CreateBy).HasLabel("创建人");
        }
    }
}
