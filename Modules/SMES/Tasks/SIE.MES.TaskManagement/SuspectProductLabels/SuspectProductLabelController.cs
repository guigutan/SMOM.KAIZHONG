using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Data;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.SuspectProductLabel;
using SIE.Items;
using SIE.MES.TaskManagement.SuspectProductLabels.ApiModels;
using SIE.MES.TaskManagement.SuspectProductLabels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.TaskManagement.SuspectProductLabels
{
    /// <summary>
    /// 可疑品标签 控制器
    /// </summary>
    public partial class SuspectProductLabelController : DomainController, ISuspectProductLabel
    {
        #region 可疑品处理(BS)

        public virtual void SuspectProductLabelProcessing(SuspectLabelProcessingCommandData data)
        {
            if (data == null)
                throw new ValidationException("没有可提交数据".L10N());
            if (data.details.Count > 0)
            {
                if (data.details.Any(p => p.Qty == 0))
                    throw new ValidationException("处理数量必须大于0".L10N());
                if (data.details.Any(p => p.DefectId == null || p.DefectId == 0))
                    throw new ValidationException("缺陷必填!".L10N());
                if (data.details.Any(p => p.SuspectJudgeResult == null))
                    throw new ValidationException("判定结果必填!".L10N());
                if (data.details.Sum(p => p.Qty) + (data.viewModel.GoodQty ?? 0) != data.viewModel.Qty)
                    throw new ValidationException("良品数+报废数+返工数必须等于可疑品标签数".L10N());
            }
            SuspectProductLabelData labelData = new SuspectProductLabelData();
            labelData.SuspectProductLabelId = data.viewModel.SuspectProductLabelId;
            labelData.GoodQty = (data.viewModel.GoodQty ?? 0);
            labelData.AttachmentIdList = new System.Collections.Generic.List<double>();
            //报废
            labelData.ScrapQty = 0;
            var scraps = data.details.Where(p => p.SuspectJudgeResult == ProcessingDefectDtlViewModelType.Scrap).ToList();
            labelData.ScrapDetailList = new System.Collections.Generic.List<SuspectProductLabelDetailData>();
            if (scraps.Count > 0)
            {
                foreach (var scrap in scraps)
                {
                    labelData.ScrapDetailList.Add(new SuspectProductLabelDetailData()
                    {
                        DefectId = scrap.DefectId,
                        Qty = scrap.Qty
                    });
                }
                labelData.ScrapQty = labelData.ScrapDetailList.Sum(p => p.Qty);
            }

            //返工
            labelData.RepairQty = 0;
            var repairs = data.details.Where(p => p.SuspectJudgeResult == ProcessingDefectDtlViewModelType.Repair).ToList();
            labelData.RepairDetailList = new System.Collections.Generic.List<SuspectProductLabelDetailData>();
            if (repairs.Count > 0)
            {
                foreach (var repair in repairs)
                {
                    labelData.RepairDetailList.Add(new SuspectProductLabelDetailData()
                    {
                        DefectId = repair.DefectId,
                        Qty = repair.Qty
                    });
                }
                labelData.RepairQty = labelData.RepairDetailList.Sum(p => p.Qty);
            }
            labelData.IsTaskFinish = true;

            //调用PDA可疑品处理逻辑
            SubmitSuspectProductHandleResult(labelData);
        }

        #endregion

        #region 查询可疑品标签

        /// <summary>
        /// 根据工单号获取可疑品标签
        /// </summary>
        /// <param name="woNos"></param>
        /// <returns></returns>
        public virtual EntityList<SuspectProductLabel> GetSuspectProductLabelsByWoNos(List<string> woNos)
        {
            var list = woNos.SplitContains(temps =>
            {
                return Query<SuspectProductLabel>().Where(p => temps.Contains(p.WorkOrder.No)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 查询可疑品标签
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SuspectProductLabel> GetSuspectProductLabels(SuspectProductLabelCriteria criteria)
        {
            var query = Query<SuspectProductLabel>()
                .WhereIf(criteria.DispatchTaskNo.IsNotEmpty(), p => criteria.DispatchTaskNo.Contains(p.DispatchTask.No))
                .WhereIf(criteria.BatchNo.IsNotEmpty(), p => p.BatchNo.Contains(criteria.BatchNo))
                .WhereIf(criteria.ProcessBatchNo.IsNotEmpty(), p => p.ProcessBatchNo.Contains(criteria.ProcessBatchNo))
                .WhereIf(criteria.ProcessId.HasValue, p => p.ProcessId == criteria.ProcessId)
                .WhereIf(criteria.WorkOrderId.HasValue, p => p.WorkOrderId == criteria.WorkOrderId)
                .WhereIf(criteria.ProductCode.IsNotEmpty(), p => p.WorkOrder.Product.Code.Contains(criteria.ProductCode))
                .WhereIf(criteria.OldProductCode.IsNotEmpty(), p => p.WorkOrder.Product.ShortDescription.Contains(criteria.OldProductCode))
                .WhereIf(criteria.CreateById.HasValue, p => p.CreateBy == criteria.CreateById)
                .WhereIf(criteria.CreateDate != null && criteria.CreateDate.BeginValue.HasValue, p => p.CreateDate >= criteria.CreateDate.BeginValue)
                .WhereIf(criteria.CreateDate != null && criteria.CreateDate.EndValue.HasValue, p => p.CreateDate <= criteria.CreateDate.EndValue)
                .WhereIf(!criteria.WorkShop.IsNullOrEmpty(), p => p.DispatchTask.WorkShop.Code.Contains(criteria.WorkShop) || p.DispatchTask.WorkShop.Name.Contains(criteria.WorkShop))
                .WhereIf(criteria.HandleState != null, p => p.HandleState == criteria.HandleState);
            //工单状态
            if (criteria.WoState.IsNotEmpty())
            {
                var stateList = new List<int>();
                criteria.WoState.Split(',').ForEach(s =>
                {
                    stateList.Add(int.Parse(s));
                });
                query.Where(p => stateList.Contains((int)p.WorkOrder.State));
            }
            if (!criteria.ShowCloseWo)
            {
                query.Where(p => p.WorkOrder.State != Core.WorkOrders.WorkOrderState.Close);
            }

            if (criteria.SubBatchNo.IsNotEmpty() || criteria.DefectId.HasValue || criteria.HandleById.HasValue
                || (criteria.HandleDate != null && (criteria.HandleDate.BeginValue.HasValue || criteria.HandleDate.EndValue.HasValue)))
            {
                query.Exists<SuspectProductLabelDetail>((l, d) => d.Where(p => p.SuspectProductLabelId == l.Id)
                .WhereIf(criteria.SubBatchNo.IsNotEmpty(), p => p.SubBatchNo.Contains(criteria.SubBatchNo))
                .WhereIf(criteria.DefectId.HasValue, p => p.DefectId == criteria.DefectId)
                .WhereIf(criteria.HandleById.HasValue, p => p.HandleById == criteria.HandleById)
                .WhereIf(criteria.HandleDate != null && criteria.HandleDate.BeginValue.HasValue, p => p.HandleDate >= criteria.HandleDate.BeginValue)
                .WhereIf(criteria.HandleDate != null && criteria.HandleDate.EndValue.HasValue, p => p.HandleDate <= criteria.HandleDate.EndValue));
            }

            var list = query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var itemIds = list.Select(p => p.ProductId).Distinct().ToList();
            var parentItems = RT.Service.Resolve<ItemController>().GetParentItemsByItemIds(itemIds);
            foreach (var l in list)
            {
                var parentItem = parentItems.FirstOrDefault(p => p.ItemId == l.ProductId);
                if (parentItem != null)
                {
                    l.Bismt = parentItem.Bismt;
                }
            }

            return list;
        }
        #endregion

        #region 根据Id获取可疑品标签
        /// <summary>
        /// 根据Id获取可疑品标签
        /// </summary>
        /// <param name="id"></param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual SuspectProductLabel GetSuspectProductLabel(double id, EagerLoadOptions elo = null)
        {
            return Query<SuspectProductLabel>().Where(p => p.Id == id).FirstOrDefault(elo);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual SuspectProductLabelDetail GetSuspectProductLabelDetail(string label, EagerLoadOptions elo = null)
        {
            return Query<SuspectProductLabelDetail>().Where(p => p.SubBatchNo == label).FirstOrDefault(elo);
        }

        /// <summary>
        /// 根据批次号获取可疑品标签
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual SuspectProductLabel GetSuspectProductLabel(string batchNo, EagerLoadOptions elo = null)
        {
            return Query<SuspectProductLabel>().Where(p => p.BatchNo == batchNo).FirstOrDefault(elo);
        }
        /// <summary>
        /// 获取标签处理结果
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public string GetSuspectProductLabelDetailResult(string label)
        {
            var detail = GetSuspectProductLabelDetail(label, new EagerLoadOptions().LoadWithViewProperty());
            if (detail != null)
                return detail.SuspectJudgeResult.ToLabel();
            return "";
        }

        /// <summary>
        /// 报废明细报表查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<ScrapDetailViewModel> QueryScrapDetailReports(ScrapDetailCriteria criteria)
        {
            EntityList<ScrapDetailViewModel> datas = new EntityList<ScrapDetailViewModel>();

            List<object> idList = new List<object>();

            string sqlWhere = "  ";
            if (criteria != null)
            {
                if (!criteria.BatchNo.IsNullOrEmpty())
                {
                    sqlWhere += " and a.batch_no like '%" + criteria.BatchNo + "%' ";
                }

                if (!criteria.ProductName.IsNullOrEmpty())
                {
                    sqlWhere += " and f.name like '%" + criteria.ProductName + "%' ";
                }

                if (!criteria.ItemName.IsNullOrEmpty())
                {
                    sqlWhere += " and f.code like '%" + criteria.ItemName + "%' ";
                }

                if (!criteria.ProcessName.IsNullOrEmpty())
                {
                    sqlWhere += " and g.name like '%" + criteria.ProcessName + "%' ";
                }

                if (!criteria.LineType.IsNullOrEmpty())
                {
                    sqlWhere += " and e.name like '%" + criteria.LineType + "%' ";
                }

                if (!criteria.BadCode.IsNullOrEmpty())
                {
                    sqlWhere += " and c.code like '%" + criteria.BadCode + "%' ";
                }
                if (criteria.ScrapDate != null && criteria.ScrapDate.BeginValue.HasValue)
                {
                    sqlWhere += " and a.Create_Date>=to_date('" + criteria.ScrapDate.BeginValue + "','yyyy-mm-dd hh24:mi:ss' )";
                }
                if (criteria.ScrapDate != null && criteria.ScrapDate.EndValue.HasValue)
                {
                    sqlWhere += " and a.Create_Date<=to_date('" + criteria.ScrapDate.EndValue + "','yyyy-mm-dd hh24:mi:ss')";
                }
                if (!criteria.HandleName.IsNullOrEmpty())
                {
                    sqlWhere += " and emp.name like'%" + criteria.HandleName + "%' ";
                }

                if (criteria.HandleDate != null && criteria.HandleDate.BeginValue.HasValue)
                {
                    sqlWhere += " and b.HANDLE_DATE>=to_date('" + criteria.ScrapDate.BeginValue + "','yyyy-mm-dd hh24:mi:ss' )";
                }
                if (criteria.HandleDate != null && criteria.HandleDate.EndValue.HasValue)
                {
                    sqlWhere += " and b.HANDLE_DATE<=to_date('" + criteria.ScrapDate.EndValue + "','yyyy-mm-dd hh24:mi:ss')";
                }
                //to_date('2025/12/1 上午12:00:00','yyyy-mm-dd hh24:mi:ss')
                if (criteria.ClassType.HasValue)
                {
                    if (criteria.ClassType == ClassesScrapType.Day)
                    {
                        sqlWhere += " and d.Classes=1 ";
                    }
                    if (criteria.ClassType == ClassesScrapType.Night)
                    {
                        sqlWhere += " and d.Classes=2 ";
                    }

                    if (criteria.ClassType == ClassesScrapType.kb)
                    {
                        sqlWhere += " and d.Classes IS NULL ";
                    }
                }
            }

            var sql = string.Format(@"SELECT a.batch_no as batchNo,emp.NAME AS HandleName,b.HANDLE_DATE AS handleDate, e.NAME AS WipName,e.CODE AS WipCode,g.NAME AS ProcessName,g.CODE AS ProcessCode,CASE WHEN d.CLASSES=1 THEN '白班' WHEN d.CLASSES=2 THEN '晚班' WHEN d.CLASSES IS NULL THEN '' ELSE '' END as className  ,f.NAME as ItemName,f.CODE AS ItemCode,CASE WHEN f.TYPE_=0 THEN '成品' WHEN f.TYPE_=1 THEN '原材料' WHEN f.TYPE_=2 THEN '半成品' WHEN f.TYPE_=3 THEN '备件' WHEN f.TYPE_ IS NULL THEN '' ELSE '其他' END as ItemType , f.MTART ,f.Mrp_Controller as MrpController,h.CODE UnitCode,c.CODE as DefectCode,c.DESCRIPTION AS DefectDes,b.QTY,a.CREATE_DATE as dates,emp2.Name as CreateName FROM WIP_SUSPECT_PROD_LABEL a "
                     + " INNER JOIN WIP_SUSPECT_PROD_LABEL_DTL b " +
                     " ON a.id=b.SUSPECT_PRODUCT_LABEL_ID " +
                     " LEFT JOIN RES_EMP emp " +
                     " ON b.HANDLE_BY_ID =emp.id " +
                      " LEFT JOIN RES_EMP emp2 " +
                     " ON a.create_by =emp2.id " +
                     " LEFT JOIN DEF_DEFECT c " +
                     " ON c.id=b.DEFECT_ID " +
                     " LEFT JOIN TM_DISP_TASK d " +
                     " ON a.DISPATCH_TASK_ID =d.ID " +
                     " LEFT JOIN  RES_WIP_SCHE e " +
                     " ON a.WIP_RESOURCE_ID =e.id" +
                     " LEFT JOIN WO w " +
                     "ON w.id=a.WORK_ORDER_ID " +
                     " LEFT JOIN ITEM f " +
                     " ON f.ID =w.product_id" +
                     " LEFT JOIN BD_UNIT h " +
                     " ON f.UNIT_ID =h.id " +
                     " LEFT JOIN TECH_PROCESS g " +
                     " ON a.PROCESS_ID =g.ID " +
                     " WHERE b.SUSPECT_JUDGE_RESULT =1 and a.inv_org_id={0} {1}", RT.InvOrg, sqlWhere);

            using (var db = DbAccesserFactory.Create(TaskManagementEntityDataProvider.ConnectionStringName))
            {
                using (System.Data.IDataReader dr = db.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        ScrapDetailViewModel model = new ScrapDetailViewModel();
                        if (dr["BatchNo"] == null)
                            model.BatchNo = "";
                        else
                            model.BatchNo = dr["BatchNo"].ToString();

                        if (dr["MTART"] == null)
                            model.Mtart = "";
                        else
                            model.Mtart = dr["MTART"].ToString();

                        if (dr["MrpController"] == null)
                            model.MrpController = "";
                        else
                            model.MrpController = dr["MrpController"].ToString();
                        if (dr["UnitCode"] == null)
                            model.UnitCode = "";
                        else
                            model.UnitCode = dr["UnitCode"].ToString();

                        if (dr["ItemName"] == null)
                            model.ProductName = "";
                        else
                            model.ProductName = dr["ItemName"].ToString();

                        if (dr["ItemCode"] == null)
                            model.ItemCode = "";
                        else
                            model.ItemCode = dr["ItemCode"].ToString();

                        if (dr["ItemType"] == null)
                            model.ItemType = "";
                        else
                            model.ItemType = dr["ItemType"].ToString();

                        if (dr["WipName"] == null)
                            model.LineTypeName = "";
                        else
                            model.LineTypeName = dr["WipName"].ToString();

                        if (dr["WipCode"] == null)
                            model.LineTypeCode = "";
                        else
                            model.LineTypeCode = dr["WipCode"].ToString();

                        if (dr["ProcessName"] == null)
                            model.ProcessName = "";
                        else
                            model.ProcessName = dr["ProcessName"].ToString();

                        if (dr["ProcessCode"] == null)
                            model.ProcessCode = "";
                        else
                            model.ProcessCode = dr["ProcessCode"].ToString();

                        if (dr["className"] == null)
                            model.ClassType = "";
                        else
                            model.ClassType = dr["className"].ToString();

                        model.BadCode = dr["DefectCode"]?.ToString();
                        model.BadName = dr["DefectDes"]?.ToString();
                        model.ScrapNum = dr["QTY"].ToString();
                        model.ScrapDate = dr["dates"]?.ToString();
                        model.HandleDate = dr["HandleDate"]?.ToString();
                        model.HandleName = dr["HandleName"]?.ToString();
                        model.CreateName = dr["CreateName"]?.ToString();
                        datas.Add(model);
                    }
                }
            }
            return datas;
        }

        /*  /// <summary>
          /// 获取报废明细数据 - 使用DateTime?类型的日期字段
          /// </summary>
          /// <param name="criteria">查询条件</param>
          public virtual EntityList<ScrapDetail> GetScrapDetail(ScrapDetailCriteria criteria)
          {
              // 直接查询ScrapDetail视图实体
              var q = Query<ScrapDetail>();

              // 先添加组织过滤条件（根据实际情况调整）
              //q.Where(p => p.InvOrgId == 2); // 这里假设当前组织ID是2

              // 应用查询条件
              if (criteria != null)
              {
                  // 1. 批次号查询
                  if (!criteria.BatchNo.IsNullOrEmpty())
                      q.Where(p => p.BatchNo.Contains(criteria.BatchNo));

                  // 2. 产品名称查询
                  if (!criteria.ProductName.IsNullOrEmpty())
                      q.Where(p => p.ItemName.Contains(criteria.ProductName));

                  // 3. 物料编码查询
                  if (!criteria.ItemName.IsNullOrEmpty())
                      q.Where(p => p.ItemCode.Contains(criteria.ItemName));

                  // 4. 工序名称查询
                  if (!criteria.ProcessName.IsNullOrEmpty())
                      q.Where(p => p.ProcessName.Contains(criteria.ProcessName));

                  // 5. 线别查询
                  if (!criteria.LineType.IsNullOrEmpty())
                      q.Where(p => p.WipName.Contains(criteria.LineType));

                  // 6. 不良代码查询
                  if (!criteria.BadCode.IsNullOrEmpty())
                      q.Where(p => p.DefectCode.Contains(criteria.BadCode));

                  // 7. MRP控制者查询
                  if (!criteria.MrpController.IsNullOrEmpty())
                      q.Where(p => p.MrpController.Contains(criteria.MrpController));

                  // 8. 旧物料号查询
                  if (!criteria.ShortDescription.IsNullOrEmpty())
                      q.Where(p => p.ShortDescription.Contains(criteria.ShortDescription));

                  // 9. 处理人查询
                  if (!criteria.HandleName.IsNullOrEmpty())
                      q.Where(p => p.HandleName.Contains(criteria.HandleName));

                  // 10. 处理时间查询 - 现在可以使用DateTime直接比较
                  if (criteria.HandleDate != null)
                  {
                      // 开始时间
                      if (criteria.HandleDate.BeginValue.HasValue)
                      {
                          q.Where(p => p.HandleDate >= criteria.HandleDate.BeginValue.Value);
                      }

                      // 结束时间（包含当天的23:59:59）
                      if (criteria.HandleDate.EndValue.HasValue)
                      {
                          var endDate = criteria.HandleDate.EndValue.Value.AddDays(1).AddSeconds(-1);
                          q.Where(p => p.HandleDate <= endDate);
                      }
                  }

                  // 11. 创建时间查询 - 同样使用DateTime直接比较
                  if (criteria.ScrapDate != null)
                  {
                      // 开始时间
                      if (criteria.ScrapDate.BeginValue.HasValue)
                      {
                          q.Where(p => p.ScrapDate >= criteria.ScrapDate.BeginValue.Value);
                      }

                      // 结束时间（包含当天的23:59:59）
                      if (criteria.ScrapDate.EndValue.HasValue)
                      {
                          var endDate = criteria.ScrapDate.EndValue.Value.AddDays(1).AddSeconds(-1);
                          q.Where(p => p.ScrapDate <= endDate);
                      }
                  }

                  // 12. 班别查询
  *//*                if (criteria.ClassType.HasValue)
                  {
                      ApplyClassTypeCriteria(q, criteria.ClassType.Value);
                  }*//*

                  // 13. 数量范围查询（如果需要）
  *//*                if (criteria.ScrapNum != null)
                  {
                      if (criteria.ScrapNum.BeginValue.HasValue)
                          q.Where(p => p.ScrapNum >= criteria.ScrapNum.BeginValue.Value);

                      if (criteria.ScrapNum.EndValue.HasValue)
                          q.Where(p => p.ScrapNum <= criteria.ScrapNum.EndValue.Value);
                  }*//*
              }

              // 排序 - 按处理时间降序排列
              //q = q.OrderByDescending(p => p.HandleDate);

              // 执行分页查询
              //var list = q.ToList(criteria?.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
              var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
              return list;
          }*/

        /// <summary>
        /// 获取报废明细数据
        /// </summary>
        /// <param name="criteria">查询条件</param>
        public virtual EntityList<ScrapDetail> GetScrapDetail(ScrapDetailCriteria criteria)
        {
            // 直接查询ScrapDetail视图实体
            var q = Query<ScrapDetail>();

            // 先添加组织过滤条件（根据实际情况调整）
            // q.Where(p => p.InvOrgId == 2); // 这里假设当前组织ID是2

            // 应用查询条件
            if (criteria != null)
            {
                // 1. 批次号查询
                if (!criteria.BatchNo.IsNullOrEmpty())
                    q.Where(p => p.BatchNo.Contains(criteria.BatchNo));

                // 1. 子批次号查询
                if (!criteria.SubBatchNo.IsNullOrEmpty())
                    q.Where(p => p.SubBatchNo.Contains(criteria.SubBatchNo));


                // 2. 产品名称查询
                if (!criteria.ProductName.IsNullOrEmpty())
                    q.Where(p => p.ItemName.Contains(criteria.ProductName));

                // 3. 物料编码查询
                if (!criteria.ItemName.IsNullOrEmpty())
                    q.Where(p => p.ItemCode.Contains(criteria.ItemName));

                // 4. 工序名称查询
                if (!criteria.ProcessName.IsNullOrEmpty())
                    q.Where(p => p.ProcessName.Contains(criteria.ProcessName));

                // 5. 线别查询
                if (!criteria.LineType.IsNullOrEmpty())
                    q.Where(p => p.WipName.Contains(criteria.LineType));

                // 6. 不良代码查询
                if (!criteria.BadCode.IsNullOrEmpty())
                    q.Where(p => p.DefectCode.Contains(criteria.BadCode));

                // 7. MRP控制者查询
                if (!criteria.MrpController.IsNullOrEmpty())
                    q.Where(p => p.MrpController.Contains(criteria.MrpController));

                // 8. 旧物料号查询
                if (!criteria.ShortDescription.IsNullOrEmpty())
                    q.Where(p => p.ShortDescription.Contains(criteria.ShortDescription));

                // 9. 处理人查询
                if (!criteria.HandleName.IsNullOrEmpty())
                    q.Where(p => p.HandleName.Contains(criteria.HandleName));

                // 10. 处理时间查询 - 现在可以使用DateTime直接比较
                if (criteria.HandleDate != null)
                {
                    // 开始时间
                    if (criteria.HandleDate.BeginValue.HasValue)
                    {
                        q.Where(p => p.HandleDate >= criteria.HandleDate.BeginValue.Value);
                    }

                    // 结束时间（包含当天的23:59:59）
                    if (criteria.HandleDate.EndValue.HasValue)
                    {
                        //var endDate = criteria.HandleDate.EndValue.Value.AddDays(1).AddSeconds(-1);
                        var endDate = criteria.HandleDate.EndValue.Value;
                        q.Where(p => p.HandleDate <= endDate);
                    }
                }

                // 11. 创建时间查询 - 同样使用DateTime直接比较
                if (criteria.ScrapDate != null)
                {
                    // 开始时间
                    if (criteria.ScrapDate.BeginValue.HasValue)
                    {
                        q.Where(p => p.ScrapDate >= criteria.ScrapDate.BeginValue.Value);
                    }


                    // 结束时间（包含当天的23:59:59）
                    if (criteria.ScrapDate.EndValue.HasValue)
                    {
                        //var endDate = criteria.ScrapDate.EndValue.Value.AddDays(1).AddSeconds(-1);
                        var endDate = criteria.ScrapDate.EndValue.Value;
                        q.Where(p => p.ScrapDate <= endDate);
                    }
                }

                // 12. 班别查询 - 直接内联处理
                if (criteria.ClassType.HasValue)
                {
                    switch (criteria.ClassType.Value)
                    {
                        case ClassesScrapType.Day:
                            q.Where(p => p.ClassName == "白班");
                            break;
                        case ClassesScrapType.Night:
                            q.Where(p => p.ClassName == "晚班");
                            break;
                        case ClassesScrapType.kb:
                            q.Where(p => string.IsNullOrEmpty(p.ClassName) || p.ClassName == "");
                            break;
                    }
                }

                // 13. 数量范围查询（如果需要）
                /* if (criteria.ScrapNum != null)
                {
                    if (criteria.ScrapNum.BeginValue.HasValue)
                        q.Where(p => p.ScrapNum >= criteria.ScrapNum.BeginValue.Value);

                    if (criteria.ScrapNum.EndValue.HasValue)
                        q.Where(p => p.ScrapNum <= criteria.ScrapNum.EndValue.Value);
                } */
            }

            // 执行分页查询 - 使用传入的排序信息
            var list = q.OrderBy(criteria?.OrderInfoList).ToList(criteria?.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 应用班别查询条件
        /// </summary>
        private void ApplyClassTypeCriteria(IQueryable<ScrapDetail> q, ClassesScrapType classType)
        {
            switch (classType)
            {
                case ClassesScrapType.Day:
                    q.Where(p => p.ClassName == "白班");
                    break;
                case ClassesScrapType.Night:
                    q.Where(p => p.ClassName == "晚班");
                    break;
                case ClassesScrapType.kb:
                    q.Where(p => string.IsNullOrEmpty(p.ClassName) || p.ClassName == "");
                    break;
            }
        }

    }
}
