using SIE.Core.WorkOrders;
using SIE.Core.WorkOrders.Models;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ESop.EngDocuments.Services;
using SIE.Items;
using SIE.Tech.Processs;
using SIE.Tech.Processs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ESop.EngDocuments.Handles
{
    /// <summary>
    /// 
    /// </summary>
    public class EngDocSaveHandle
    {
        #region 属性
        
        /// <summary>
        /// 主表ids
        /// </summary>
        private List<double> EngDocumentIds { get; set; }

        /// <summary>
        /// 子表Ids
        /// </summary>
        private List<double> EngDocumentDetailIds { get; set; }

        /// <summary>
        /// 主表
        /// </summary>
        private EntityList<EngDocument> EngDocuments {  get; set; }

        /// <summary>
        /// 子表
        /// </summary>
        private EntityList<EngDocumentDetail> EngDocumentDetails { get; set; }

        /// <summary>
        /// 主表产品编码
        /// </summary>
        private List<double> EngDocumentProductIds { get; set; }

        /// <summary>
        /// 主表工单编码
        /// </summary>
        private List<double> EngDocumentWoIds { get; set; }

        /// <summary>
        /// 子表文件编码
        /// </summary>
        private List<string> EngDocumentDetailCodes { get; set; }

        /// <summary>
        /// 主表服务
        /// </summary>
        private EngDocumentService EngDocumentService { get; set; }

        /// <summary>
        /// 子表服务
        /// </summary>
        /// <returns></returns>
        private EngDocumentDetailService EngDocumentDetailService { get; set; }

        /// <summary>
        /// 工序名称集合
        /// </summary>
        private List<ProcessIdName> ProcessIdNames { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        private List<ItemAndCode> ItemCodes { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        private List<WoBaseInfo> WoNos { get; set; }

        #endregion

        #region 方法
        /// <summary>
        /// 主表必填校验
        /// </summary>
        /// <returns></returns>
        public bool ParentNotRequired()
        {
            if (EngDocuments == null) 
            {
                return false;
            } 
            if (EngDocuments.Any(p => p.Type == Enums.EngDocType.Product && p.ProductId == null))
            {
                throw new ValidationException("类型为产品时，产品编码必填！".L10N());
            }
            if (EngDocuments.Any(p => p.Type == Enums.EngDocType.WorkOrder && p.WorkOrderId == null))
            {
                throw new ValidationException("类型为工单时，工单必填！".L10N());
            }
            return false;
        }

        /// <summary>
        /// 子表必填校验
        /// </summary>
        /// <returns></returns>
        public bool ChildrenNotRequired()
        {
            if (EngDocumentDetails == null)
            {
                return false;
            }
            if (EngDocumentDetails.Any(p => p.DocCode.IsNullOrEmpty()))
            {
                throw new ValidationException("文件编码必填！".L10N());
            }
            else if (EngDocumentDetails.Any(p => p.DocName.IsNullOrEmpty()))
            {
                throw new ValidationException("文件名称必填！".L10N());
            }
            else if (EngDocumentDetails.Any(p => p.UseType.IsNullOrEmpty()))
            {
                throw new ValidationException("文件使用类型必填！".L10N());
            }
            else if (EngDocumentDetails.Any(p => p.FId == null || p.FId == 0))
            {
                throw new ValidationException("文件路径必填！".L10N());
            }
            return false;
        }

        /// <summary>
        /// 前端新增修改的数据产品编码或工单重复
        /// </summary>
        /// <returns></returns>
        public bool WebProductIsRepeat()
        {
            foreach(var productId in EngDocumentProductIds)
            {
                var web = EngDocuments.Where(p => p.ProductId == productId).ToList();
                // 产品
                var product = ItemCodes.FirstOrDefault(p => p.ItemId == productId);
                if (web != null && web.Count > 1)
                {
                    throw new ValidationException("已经存在[产品编码]是{0}的数据".L10nFormat(product.ItemCode));
                }
            }
            foreach (var woId in EngDocumentWoIds)
            {
                var web = EngDocuments.Where(p => p.WorkOrderId == woId).ToList();
                var wo = WoNos.FirstOrDefault(p => p.Id == woId);
                if (web != null && web.Count > 1)
                {
                    throw new ValidationException("已经存在[工单]是{0}的数据".L10nFormat(wo.No));
                }
            }
            return false;
        }

        /// <summary>
        /// 验证新增修改的数据产品编码与数据库重复
        /// </summary>
        /// <returns></returns>
        public bool DBProductIsRepeat()
        {
            var dbProductList = EngDocumentService.GetEngDocumentByProductIds(EngDocumentProductIds, EngDocumentIds);
            foreach(var productId in EngDocumentProductIds)
            {
                var db = dbProductList.Where(p => p.ProductId == productId).ToList();
                // 产品
                var product = ItemCodes.FirstOrDefault(p => p.ItemId == productId);
                if (db != null && db.Count > 0)
                {
                    throw new ValidationException("已经存在[产品编码]是{0}的数据".L10nFormat(product.ItemCode));
                }
            }
            var dbWoList = EngDocumentService.GetEngDocumentByWoIds(EngDocumentWoIds, EngDocumentIds);
            foreach (var woId in EngDocumentWoIds)
            {
                var db = dbWoList.Where(p => p.WorkOrderId == woId).ToList();
                var wo = WoNos.FirstOrDefault(p => p.Id == woId);
                if (db != null && db.Count > 0)
                {
                    throw new ValidationException("已经存在[工单]是{0}的数据".L10nFormat(wo.No));
                }
            }
            return false;
        }

        /// <summary>
        /// 验证前端子表重复添加
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public bool WebChildIsRepeat()
        {
            foreach(var item in EngDocumentDetails)
            {
                var same = EngDocumentDetails.Where(p => p.EngDocumentId == item.EngDocumentId
                && p.DocCode == item.DocCode && p.UseType == item.UseType && p.ProcessId == item.ProcessId).ToList();
                // 主表
                var engDocument = EngDocuments.FirstOrDefault(p => p.Id == item.EngDocumentId);
                // 产品
                var product = ItemCodes.FirstOrDefault(p => p.ItemId == engDocument.ProductId);
                // 工单
                var wo = WoNos.FirstOrDefault(p => p.Id == engDocument.WorkOrderId);
                // 工序
                var process = ProcessIdNames.FirstOrDefault(p => p.ProcessId == item.ProcessId);
                if (same != null && same.Count > 1)
                {
                    throw new ValidationException("[{0}][{1}]已维护文件[{2}]工序[{3}]使用类型[{4}]的数据".L10nFormat(engDocument.Type.ToLabel(), product?.ItemCode ?? wo?.No, item.DocCode, process.ProcessName, item.UseType));
                }
            }
            return false;
        }

        /// <summary>
        /// 验证数据库子表重复添加
        /// </summary>
        /// <returns></returns>
        public bool DBChildIsRepeat()
        {
            var dbList = EngDocumentDetailService.GetEngDocumentDetailByDocCodes(EngDocumentDetailCodes, EngDocumentDetailIds);
            foreach(var item in EngDocumentDetails)
            {
                var same = dbList.Where(p => p.EngDocumentId == item.EngDocumentId
                && p.DocCode == item.DocCode && p.UseType == item.UseType && p.ProcessId == item.ProcessId).ToList();
                // 主表
                var engDocument = EngDocuments.FirstOrDefault(p => p.Id == item.EngDocumentId);
                // 产品
                var product = ItemCodes.FirstOrDefault(p => p.ItemId == engDocument.ProductId);
                // 工单
                var wo = WoNos.FirstOrDefault(p => p.Id == engDocument.WorkOrderId);
                // 工序
                var process = ProcessIdNames.FirstOrDefault(p => p.ProcessId == item.ProcessId);
                if (same != null && same.Count > 0)
                {
                    throw new ValidationException("[{0}][{1}]已维护文件[{2}]工序[{3}]使用类型[{4}]的数据".L10nFormat(engDocument.Type.ToLabel(), product?.ItemCode ?? wo?.No, item.DocCode, process.ProcessName, item.UseType));
                }
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 构造
        /// </summary>
        public EngDocSaveHandle(EntityList data)
        {
            if (data == null || (data.Count == 0 && data.DeletedList.Count == 0))
            {
                throw new ValidationException("保存数据异常！".L10N());
            }
            EngDocuments = data as EntityList<EngDocument>;
            EngDocumentDetails = new EntityList<EngDocumentDetail>();
            EngDocuments.ForEach(p =>
            {
                EngDocumentDetails.AddRange(p.EngDocumentDetailList.Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged).ToList());
            });
            EngDocumentIds = new List<double>();
            EngDocumentDetailIds = new List<double>();
            EngDocumentProductIds = new List<double>();
            EngDocumentWoIds = new List<double>();
            EngDocumentDetailCodes = new List<string>();
            // 主表ids
            EngDocumentIds.AddRange(EngDocuments.Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged).Select(p => p.Id).ToList());
            // 子表ids
            EngDocumentDetailIds.AddRange(EngDocumentDetails.Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged).Select(p => p.Id).ToList());
            // 产品Id
            EngDocumentProductIds.AddRange(EngDocuments.Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged && p.ProductId != null).Select(p => (double)p.ProductId).ToList());
            // 工单Id
            EngDocumentWoIds.AddRange(EngDocuments.Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged && p.WorkOrderId != null).Select(p => (double)p.WorkOrderId).ToList());
            // 文件编码
            EngDocumentDetailCodes.AddRange(EngDocumentDetails.Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged).Select(p => p.DocCode).ToList());

        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            // 主表服务
            EngDocumentService = RT.Service.Resolve<EngDocumentService>();

            // 子表服务
            EngDocumentDetailService = RT.Service.Resolve<EngDocumentDetailService>();

            // 工序名称集合
            ProcessIdNames = RT.Service.Resolve<ProcessController>().GetProcessIdNames(EngDocumentDetails.Select(p => p.ProcessId).ToList());

            // 产品编码
            ItemCodes = RT.Service.Resolve<ItemController>().GetItemCodes(EngDocumentProductIds);

            // 工单
            WoNos = RT.Service.Resolve<WorkOrderController>().GetWorkOrderBaseInfos(EngDocumentWoIds);
        }
    }
}
