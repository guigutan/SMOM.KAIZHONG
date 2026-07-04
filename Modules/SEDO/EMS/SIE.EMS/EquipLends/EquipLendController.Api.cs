using SIE.Api;
using SIE.Common.Configs;
using SIE.Core.ApiModels;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Common.Utils;
using SIE.EMS.EquipLends.ApiModels;
using SIE.EMS.EquipLends.Configs;
using SIE.EMS.EquipLends.Enums;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.ApiModels;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SIE.EMS.EquipLends
{
    /// <summary>
    /// 设备借还管理控制器API
    /// </summary>
    public partial class EquipLendController : DomainController
    {
        /// <summary>
        /// 获取工厂、车间、部门、产线基础信息
        /// </summary>
        /// <param name="key">借机部门关键字</param>
        /// <returns></returns>
        [ApiService]
        public virtual List<BaseDataInfo> GetLendEnterpriseBaseData([ApiParameter("借机部门关键字")] string key)
        {
            return RT.Service.Resolve<EnterpriseController>().GetDepartmentsBaseInfo(key);
        }

        /// <summary>
        /// 获取借机人基础信息
        /// </summary>
        /// <param name="key">借机人关键字</param>
        /// <returns></returns>
        [ApiService]
        public virtual List<BaseDataInfo> GetLendEmployeeBaseData([ApiParameter("借机人关键字")] string key)
        {
            return RT.Service.Resolve<EmployeeController>().GetBaseEmployeeInfo(key);
        }

        /// <summary>
        /// 获取供应商基础信息
        /// </summary>
        /// <param name="key">供应商关键字</param>
        /// <returns></returns>
        [ApiService]
        public virtual List<BaseDataInfo> GetLendSupplierBaseData([ApiParameter("供应商关键字")] string key)
        {
            return RT.Service.Resolve<SupplierController>().GetSuppliers(key);
        }

        /// <summary>
        /// 获取设备基础数据
        /// </summary>
        /// <param name="key">设备关键字</param>
        /// <returns></returns>
        [ApiService]
        public virtual EquipInfo GetEquipAccountInfo([ApiParameter("设备关键字")] string key)
        {
            return RT.Service.Resolve<EquipController>().GetEquipAccountInfo(key);
        }

        /// <summary>
        /// 提交借机单
        /// </summary>
        /// <param name="subData">提交数据</param>
        [ApiService]
        public virtual void SubmitEquipLend([ApiParameter("提交数据")] EquipLendSubmitInfo subData)
        {
            // 借机单主表信息
            EquipLendManage equipLendManage = new EquipLendManage
            {
                LendState = (LendState)subData.LendState,
                EquipAccountId = subData.EquipAccountId,
                LendEnterpriseId = subData.LendEnterpriseId,
                LendEmployeeId = subData.LendEmployeeId,
                SupplierId = subData.SupplierId,
                LendObject = (LendObject)subData.LendObject,
                Reason = subData.Reason,
                Remark = subData.Remark,
            };

            // 校验
            EquipLendOnSavingValidate(equipLendManage);
            EquipLendSubmitValidate(new List<EquipLendSubmitInfo> { subData });

            //验证通过
            equipLendManage.No = GetLendNos().FirstOrDefault();
            equipLendManage.GenerateId();
            subData.Id = equipLendManage.Id;

            bool lendExamine = false; // 是否启用借出审核
            var config = ConfigService.GetConfig(new EquipLendManageConfig(), typeof(EquipLendManage));
            if (config != null)
            {
                lendExamine = config.LendExamine;
            }

            // 图片信息
            var hepler = new FileUrlHelper();
            EquipLendAttachment attachment = null;
            if (subData.Content != null && subData.Content.Length > 0)
            {
                attachment = hepler.GenerateAttachmentBase64StringContent(new EquipLendAttachment(), subData.Content, subData.FileName) as EquipLendAttachment;
                attachment.OwnerId = equipLendManage.Id;
            }

            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                // 保存
                RF.Save(equipLendManage);
                // 提交单据
                EquipLendSubmitOrder(new List<EquipLendSubmitInfo> { subData }, lendExamine);
                // 提交附件图片
                if (attachment != null)
                {
                    RF.Save(attachment);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 查询设备归还数据
        /// </summary>
        /// <param name="departmentId">部门Id</param>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [ApiService]
        public virtual EquipReturnQueryInfo GetEquipReturnInfos([ApiParameter("借机部门")] double? departmentId, [ApiParameter("分页信息")] PagingInfo pagingInfo, [ApiParameter("关键字")] string key)
        {
            List<EquipReturnInfo> equipReturnInfos = new List<EquipReturnInfo>();
            var list = Query<EquipLendManage>()
                .LeftJoin<EquipAccount>((em, ea) => em.EquipAccountId == ea.Id)
                .WhereIf<EquipAccount>(key.IsNotEmpty(), (em, ea) => ea.Code.Contains(key) || ea.Name.Contains(key) || ea.RFID.Contains(key))
                .WhereIf(departmentId != null, em => em.LendEnterpriseId == departmentId)
                .Where(em => em.LendState == LendState.HasLended)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var totalCount = list.TotalCount;
            if (list.Count > 0)
            {
                foreach (var i in list)
                {
                    EquipReturnInfo equipReturnInfo = new EquipReturnInfo
                    {
                        Id = i.Id,
                        No = i.No,
                        EquipCode = i.EquipAccountCode,
                        EquipName = i.EquipAccountName,
                        EquipModelCode = i.ModelCode,
                        RFID = i.RFID,
                        EnterpriseName = i.EnterpriseName,
                        EmployeeName = i.EmployeeName,
                        LendTime = i.CreateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                        LendObject = (int)i.LendObject,
                        SupplierCode = i.SupplierCode,
                        SupplierName = i.SupplierName,
                        Reason = i.Reason,
                        Remark = i.Remark,
                    };
                    equipReturnInfos.Add(equipReturnInfo);
                }
            }
            EquipReturnQueryInfo equipQueryInfo = new EquipReturnQueryInfo
            {
                TotalCount = totalCount,
                EquipReturnInfos = equipReturnInfos
            };
            return equipQueryInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lendIds"></param>
        /// <param name="returnRemark"></param>
        [ApiService]
        public virtual void SubmitEquipReturn([ApiParameter("借还单Ids")] List<double> lendIds, [ApiParameter("归还说明")] string returnRemark)
        {
            RT.Service.Resolve<EquipLendController>().EquipLendReturn(returnRemark, lendIds);
        }
    }
}
