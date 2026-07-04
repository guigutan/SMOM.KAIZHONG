using NPOI.OpenXmlFormats.Shared;
using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Common.Attachments;
using SIE.Common.Configs;
using SIE.Common.Discriminator;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Dispatchs;
using SIE.Items;
using SIE.MES.Engrave;
using SIE.MES.PackingQC.Configs;
using SIE.MES.PackingQC.Datas;
using SIE.MES.WIP.Pressure;
using SIE.MES.WorkOrders;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.PackingQC
{
    public partial class PackingQcController : DomainController
    {

        public virtual void PackingQcSave(ref PackingQc packingQc, ref WipPressureSn wipSn, ref WipBatch wipBatch, ref PackingDetail packingDetail, SIE.MES.BlueLable.BlueLable blueBable)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                if (packingQc != null && packingQc.PersistenceStatus != PersistenceStatus.Unchanged)
                    RF.Save(packingQc);

                if (packingDetail != null && packingDetail.PersistenceStatus != PersistenceStatus.Unchanged)
                    RF.Save(packingDetail);

                if (wipBatch != null && wipBatch.PersistenceStatus != PersistenceStatus.Unchanged)
                    RF.Save(wipBatch);

                if (wipSn != null && wipSn.PersistenceStatus != PersistenceStatus.Unchanged)
                    RF.Save(wipSn);

                if (blueBable != null && blueBable.PersistenceStatus != PersistenceStatus.Unchanged)
                    RF.Save(blueBable);
                tran.Complete();
            }
        }

        /// <summary>
        /// 查询包装采集
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<PackingQc> CriterialPackingQc(PackingQcCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("包装采集查询实体异常！".L10N());
            }
            var q = Query<PackingQc>();
            if (!criterial.BlueLabel.IsNullOrEmpty())
            {
                q.Where(m => m.BlueLabel.Contains("%" + criterial.BlueLabel + "%"));
            }
            if (criterial.Confirm.HasValue)
            {
                q.Where(p => p.Confirm == criterial.Confirm);
            }
            if (criterial.ReportsType.HasValue)
            {
                q.Where(p => p.ReportsType == criterial.ReportsType);
            }
            if (criterial.PackIdent.HasValue)
            {
                q.Where(p => p.PackIdent == criterial.PackIdent);
            }
            if (!criterial.ProductLabel.IsNullOrEmpty())
            {
                q.Exists<PackingDetail>((a, b) => b.Where(p => p.PackingQcId == a.Id && p.ProductLabel.Contains(criterial.ProductLabel)));
            }
            if (!criterial.BatchLabel.IsNullOrEmpty())
            {
                q.Exists<PackingDetail>((a, b) => b.Where(p => p.PackingQcId == a.Id && p.BatchLabel.Contains(criterial.BatchLabel)));
            }

            if (!criterial.WorkOrderNo.IsNullOrEmpty())
            {
                q.Exists<PackingDetail>((a, b) => b.Where(p => p.PackingQcId == a.Id && p.WorkOrderNo.Contains(criterial.WorkOrderNo)));
            }
            if (criterial.ItemCode.IsNotEmpty())
            {
                q.Where(p => p.Item.Code.Contains(criterial.ItemCode));
            }
            if (criterial.ResourceId != null)
                q.Where(p => p.ResourceId == criterial.ResourceId);

            if (criterial.CreateDate.BeginValue.HasValue)
                q.Where(p => p.CreateDate >= criterial.CreateDate.BeginValue);
            if (criterial.CreateDate.EndValue.HasValue)
                q.Where(p => p.CreateDate <= criterial.CreateDate.EndValue);

            return q.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询包装报工记录查询
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<PackingReportRecord> CriterialPackingReportRecord(PackingReportRecordCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("包装记录查询实体异常！".L10N());
            }
            var q = Query<PackingReportRecord>();
            if (!criterial.BlueLabel.IsNullOrEmpty())
            {
                q.Where(m => m.BlueLabel.Contains("%" + criterial.BlueLabel + "%"));
            }
            if (criterial.Report.HasValue)
            {
                q.Where(m => m.Report == criterial.Report);
            }
            return q.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取蓝标
        /// </summary>
        /// <param name="name">蓝标标签</param>
        /// <returns></returns>
        public virtual SIE.MES.BlueLable.BlueLable GetBlueLable(string name)
        {
            var q = Query<SIE.MES.BlueLable.BlueLable>().Where(p => p.BlueLableBox == name && p.CreateDeleteident == "创建").FirstOrDefault();
            if (q == null)
                return null;
            return q;
        }

        /// <summary>
        /// 获取所有蓝标
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual BlueLable.BlueLable AllBlueLable(string name)
        {
            var q = Query<SIE.MES.BlueLable.BlueLable>().Where(p => p.BlueLableBox == name).FirstOrDefault();
            if (q == null)
                return null;
            return q;
        }

        /// <summary>
        /// 根据物料获取蓝标层级
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual SIE.MES.BlueLable.BlueLableLevel GetBlueLableLevel(double itemId)
        {
            return Query<SIE.MES.BlueLable.BlueLableLevel>().Where(p => p.ItemId == itemId).FirstOrDefault();
        }

        /// <summary>
        /// 根据蓝标获取是否装箱的
        /// </summary>
        /// <param name="name">蓝标</param>
        /// <returns></returns>
        public virtual PackingQc GetPackingQc(string name)
        {
            var q = Query<PackingQc>().Where(p => p.BlueLabel == name).FirstOrDefault();
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<PackingDetail> GetPackingDetails(double resourId, string woNo)
        {
            var list = DB.Query<PackingDetail>().Where(p => p.PackingQc.ResourceId == resourId && p.WorkOrderNo == woNo && p.ReportsType == ReportsTypeEnum.NO).ToList();
            return list;
        }

        /// <summary>
        /// 根据SN获取包装QC确认明细
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public virtual PackingDetail GetPackingDetailByProductLabel(string label)
        {
            var detail = Query<PackingDetail>().Where(p => p.ProductLabel == label).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return detail;
        }

        /// <summary>
        /// 是否存在装箱明细
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public virtual PackingDetail IsExistPackingDetail(string label)
        {
            var detail = Query<PackingDetail>().Where(p => p.ProductLabel == label || p.BatchLabel == label).FirstOrDefault();
            return detail;
        }

        /// <summary>
        /// 判断工序标识是否重复使用
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool GetPackingDetail(string name)
        {
            var q = Query<PackingDetail>().Where(p => p.ProductLabel == name).FirstOrDefault();
            if (q == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据序号找二维码
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual EntityList<PackingDetail> GetPackingDetails(string name)
        {
            var query = Query<PackingDetail>().Where(p => p.ProductLabel.Contains(name)).ToList();
            return query;
        }

        /// <summary>
        /// 根据蓝标获取装箱明细
        /// </summary>
        /// <param name="blueLabel"></param>
        /// <returns></returns>
        public virtual EntityList<PackingDetail> GetPackingDetailsByBlueLabel(string blueLabel)
        {
            var list = Query<PackingDetail>().Where(p => p.PackingQc.BlueLabel == blueLabel).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 根据包装主表id查询从表数据
        /// </summary>
        /// <param name="packId"></param>
        /// <returns></returns>
        public virtual EntityList<PackingDetail> GetPackingDetailsByids(double packId, PagingInfo paging = null, IList<OrderInfo> SortInfo = null)
        {
            var list = Query<PackingDetail>().Where(p => p.PackingQcId == packId).OrderBy(SortInfo).ToList(paging, new EagerLoadOptions().LoadWithViewProperty());
            if (list.Count > 0 && list.FirstOrDefault().LabelType == LabelTypeEnum.SnLabel)
            {
                var sns = list.Select(p => p.ProductLabel).ToList();
                var snList = RT.Service.Resolve<WipPressureController>().GetWipPressureSns(sns);

                foreach (var item in list)
                {
                    item.TestValue = snList.FirstOrDefault(p => p.Sn == item.ProductLabel)?.RawData;
                }
            }
            return list;
        }

        /// <summary>
        /// 批次刻码装箱明细
        /// </summary>
        /// <param name="packId"></param>
        /// <param name="batchlabel"></param>
        /// <returns></returns>
        public virtual EntityList<PackingDetail> GetPackingDetailsBatchByids(double packId, string batchlabel)
        {
            var query = Query<PackingDetail>().Where(p => p.PackingQcId == packId && p.BatchLabel == batchlabel).ToList();
            return query;
        }

        /// <summary>
        /// 根据主表查询子表报工的数据
        /// </summary>
        /// <param name="packId"></param>
        /// <returns></returns>
        public virtual EntityList<PackingDetail> GetPackingDetailAByids(double packId)
        {
            var query = Query<PackingDetail>().Where(p => p.PackingQcId == packId && p.ReportsType == ReportsTypeEnum.YES).ToList();
            return query;
        }

        /// <summary>
        /// 获取所有蓝标
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<PackingQc> GetPackingQcs(List<string> blueBoxList)
        {
            return blueBoxList.SplitContains(nos =>
            {
                return Query<PackingQc>().Where(p => nos.Contains(p.BlueLabel)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }


        /// <summary>
        /// 获取所有不满箱,没有封箱的数据(排除已关闭的工单)
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<PackingQc> GetPackingQcByState(double resourceId)
        {
            var q = Query<PackingQc>()
                .Join<PackingDetail>((a, b) => a.Id == b.PackingQcId)
                .Join<PackingDetail, WorkOrder>((b, c) => b.WorkOrderNo == c.No && c.State != Core.WorkOrders.WorkOrderState.Close)
                .Where(p => p.ResourceId == resourceId && (p.BoxState != BoxStateEnum.NO || p.Confirm == ConfirmEnum.NO));
            return q.ToList();
        }

        /// <summary>
        /// 根据装箱记录获取工单id
        /// </summary>
        /// <param name="packDetailId"></param>
        /// <returns></returns>
        public virtual WorkOrder GetWoId(double packDetailId)
        {
            var packData = Query<PackingQc>().Where(p => p.Id == packDetailId).FirstOrDefault();
            var packDetailData = Query<PackingDetail>().Where(p => p.PackingQcId == packData.Id).FirstOrDefault();

            var woData = Query<WorkOrder>().Where(p => p.No == packDetailData.WorkOrderNo).FirstOrDefault();
            return woData;
        }

        /// <summary>
        /// 获取所有不满箱,没有封箱的数据
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<PackingQc> GetPackingQcDByState(double resourceId)
        {
            var q = Query<PackingQc>();
            return q.Where(p => p.ResourceId == resourceId && (p.BoxState != BoxStateEnum.NO)).ToList();//p.PackIdent == PackIdentEnum.NotFullTank ||

        }

        /// <summary>
        /// 装箱明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual EntityList<PackingDetail> GetPackingDetail(double id)
        {
            var q = Query<PackingDetail>().Where(p => p.PackingQcId == id).ToList();
            return q;
        }

        /// <summary>
        /// 获取待确认的QC清单列表
        /// </summary>
        /// <param name="keyWord">搜索关键词</param>
        /// <returns></returns>
        public virtual List<PackingQcData> GetPackingQcDatas(string keyWord)
        {
            List<PackingQcData> datas = new List<PackingQcData>();
            //待确认QC主表
            var packQcMaster = Query<PackingQc>()
                //排除工单关闭的
                .NotExists<PackingDetail>((x, y) => y.Join<WorkOrder>((x1, y1) => x1.WorkOrderNo == y1.No && y1.State == Core.WorkOrders.WorkOrderState.Close)
                .Where(p => p.PackingQcId == x.Id))
                .Where(p => p.Confirm == ConfirmEnum.NO).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (packQcMaster.Count == 0)
                return datas;
            if (packQcMaster == null)
                return datas;
            //蓝标
            var blueLabelArr = packQcMaster.Select(p => p.BlueLabel).ToList();
            var blueLabelList = Query<SIE.MES.BlueLable.BlueLable>().Where(p => blueLabelArr.Contains(p.BlueLableBox)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (blueLabelList.Count == 0)
                return datas;
            if (blueLabelList == null)
                return datas;
            var blueLabelDic = blueLabelList.ToDictionary(p => p.BlueLableBox);

            foreach (var p in packQcMaster)
            {
                PackingQcData data = new PackingQcData();
                data.Id = p.Id;
                data.BoxCode = p.BlueLabel;

                //待确认明细
                var dtls = p.PackingDetailList;
                var dtl = p.PackingDetailList.FirstOrDefault();
                //var workOrder = Query<WipBatch>().Where(p => p.BatchNo == dtl.ProductLabel).FirstOrDefault()?.WorkOrder;

                var res = Query<WipResource>().Where(x => x.Id == p.ResourceId).FirstOrDefault();

                SIE.MES.BlueLable.BlueLable label;
                if (blueLabelDic.TryGetValue(p.BlueLabel, out label))
                {
                    data.InstalledQty = dtls.Count.ToString() + "/" + label?.PackageNum.ToString();
                    data.ItemCode = label?.Item?.Code;
                    data.ItemOldCode = label?.Item?.ShortDescription;
                    data.WipResCode = res?.Code;
                    data.WipResName = res?.Name;
                    data.WipResId = res?.Id;
                }
                datas.Add(data);
            }

            if (!keyWord.IsNullOrEmpty())
            {
                datas = datas.Where(p => p.BoxCode.Contains(keyWord) || p.WipResCode.Contains(keyWord) || p.ItemCode.Contains(keyWord)).ToList();
            }

            return datas;
        }

        /// <summary>
        /// 提交待确认的QC清单明细
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool SubmitData(List<SubmitPackingQcData> data, double id)
        {
            var masterData = RF.GetById<PackingQc>(id);
            if (data != null)
            {
                foreach (var p in data)
                {
                    var attachment = GenerateAttachmentBase64StringContent(new PackingQcAttachment(), p.FileContent, p.FileName) as PackingQcAttachment;
                    attachment.OwnerId = id;
                    masterData.DocList.Add(attachment);
                }
            }
            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                if (masterData != null)
                {
                    //修改包装采集主表QC确认
                    masterData.Confirm = ConfirmEnum.YES;
                    RF.Save(masterData);
                    //修改包装采集明细表QC确认
                    DB.Update<PackingDetail>().Set(p => p.Confirm, ConfirmEnum.YES).Where(p => p.PackingQcId == masterData.Id && p.Confirm == ConfirmEnum.NO).Execute();
                }
                var detailList = Query<PackingDetail>().Where(p => p.PackingQcId == masterData.Id && p.Confirm == ConfirmEnum.YES && p.ReportsType == ReportsTypeEnum.NO).ToList();

                var detailData = detailList.FirstOrDefault();
                if (detailData != null)
                {
                    var labels = detailList.Select(p => p.ProductLabel).ToList();
                    var reportDatas = new List<ReportInfo>();
                    if (detailData.LabelType == LabelTypeEnum.BatchLabel)
                    {
                        //批次标签 生产批次:WipBatch
                        var batchs = RT.Service.Resolve<WipBatchController>().GetWipBatches(labels);
                        var woId = batchs.FirstOrDefault().WorkOrderId;
                        reportDatas = detailList
                            .Select(p => new ReportInfo
                            {
                                Sn = p.ProductLabel,
                                WorkOrderId = woId,
                                ResourceId = masterData.ResourceId,
                                GoodQty = p.PackingNum
                            }).ToList();
                    }
                    else
                    {
                        ////耐压标签 耐压测试批次数据WipPressure 耐压测试SN:WipPressureSn
                        var snList = RT.Service.Resolve<WipPressureController>().GetWipPressureSns(labels);
                        reportDatas = snList.GroupBy(p => new { p.WorkOrderId, p.BatchNo })
                            .Select(p => new ReportInfo
                            {
                                Sn = p.Key.BatchNo,
                                WorkOrderId = p.Key.WorkOrderId,
                                ResourceId = masterData.ResourceId,
                                GoodQty = p.Count()
                            }).ToList();
                    }
                    if (reportDatas.Count > 0)
                    {
                        //更新原标签数量
                        var batchNos = reportDatas.Select(p => p.Sn).ToList();
                        RT.Service.Resolve<WipPressureController>().UpdateBatchQty(batchNos);
                        //包装报工
                        RT.Service.Resolve<ITaskReportKZ>().PackingReport(reportDatas);
                        //更新装箱明细状态
                        DB.Update<PackingDetail>().Set(p => p.ReportsType, ReportsTypeEnum.YES).Where(p => p.PackingQcId == masterData.Id && labels.Contains(p.ProductLabel)).Execute();
                    }
                }

                trans.Complete();
            }
            return true;
        }

        /// <summary>
        /// 查询是否有需要报工的数据
        /// </summary>
        /// <param name="qcId"></param>
        /// <returns></returns>
        public virtual bool BoolUnreportedWork(double qcId)
        {
            var detailList = Query<PackingDetail>().Where(p => p.PackingQcId == qcId && p.Confirm == ConfirmEnum.YES && p.ReportsType == ReportsTypeEnum.NO).ToList();
            if (detailList.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 报工
        /// </summary>
        /// <param name="qc"></param>
        /// <param name="autoFeeding">自动上料</param>
        /// <returns></returns>
        public virtual string SubmitData(PackingQc qc, bool autoFeeding = false, bool IsTaskFinish = true, Dictionary<string, double> SnTask = null)
        {
            try
            {
                using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    var detailList = Query<PackingDetail>().Where(p => p.PackingQcId == qc.Id && p.Confirm == ConfirmEnum.YES && p.ReportsType == ReportsTypeEnum.NO).ToList();
                    var detailData = detailList.FirstOrDefault();
                    if (detailData != null)
                    {
                        var labels = detailList.Select(p => p.ProductLabel).ToList();
                        var reportDatas = new List<ReportInfo>();
                        if (detailData.LabelType == LabelTypeEnum.BatchLabel)
                        {
                            //批次标签 生产批次:WipBatch
                            var batchs = RT.Service.Resolve<WipBatchController>().GetWipBatches(labels);
                            var woId = batchs.FirstOrDefault().WorkOrderId;
                            reportDatas = detailList
                                .Select(p => new ReportInfo
                                {
                                    Sn = p.ProductLabel,
                                    TaskId = batchs.FirstOrDefault(f => f.BatchNo == p.ProductLabel)?.PackingTaskId,//SnTask != null ? SnTask[p.ProductLabel] : null,
                                    WorkOrderId = woId,
                                    ResourceId = qc.ResourceId,
                                    GoodQty = p.PackingNum,
                                    IsAutoFeeding = autoFeeding
                                }).ToList();
                        }
                        else if (detailData.LabelType == LabelTypeEnum.SnLabel)
                        {
                            ////耐压标签 耐压测试批次数据WipPressure 耐压测试SN:WipPressureSn
                            var snList = RT.Service.Resolve<WipPressureController>().GetWipPressureSns(labels);
                            reportDatas = snList.GroupBy(p => new { p.WorkOrderId, p.BatchNo })
                                .Select(p => new ReportInfo
                                {
                                    Sn = p.Key.BatchNo,
                                    WorkOrderId = p.Key.WorkOrderId,
                                    ResourceId = qc.ResourceId,
                                    GoodQty = p.Count(),
                                    IsAutoFeeding = autoFeeding
                                }).ToList();
                        }
                        else
                        {
                            //刻码标签
                            var snList = RT.Service.Resolve<EngraveLabelController>().GetEngraveSnSns(labels);
                            reportDatas = snList.GroupBy(p => new { p.WorkOrderId, p.BatchNo })
                                .Select(p => new ReportInfo
                                {
                                    Sn = p.Key.BatchNo,
                                    WorkOrderId = p.Key.WorkOrderId,
                                    ResourceId = qc.ResourceId,
                                    GoodQty = p.Count(),
                                    IsAutoFeeding = autoFeeding
                                }).ToList();
                        }
                        if (reportDatas.Count > 0)
                        {
                            RT.Service.Resolve<ITaskReportKZ>().PackingReport(reportDatas, IsTaskFinish);
                            DB.Update<PackingDetail>().Set(p => p.ReportsType, ReportsTypeEnum.YES).Where(p => p.PackingQcId == qc.Id && labels.Contains(p.ProductLabel)).Execute();
                            DB.Update<PackingQc>().Set(p => p.ReportsType, ReportsTypeEnum.YES).Where(p => p.Id == qc.Id).Execute();
                        }
                    }
                    trans.Complete();
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            return "";
        }

        /// <summary>
        /// 生成附件Base64格式上下文
        /// </summary>
        /// <param name="attachment">实体</param>
        /// <param name="content">上下文</param>
        /// <param name="fileName">文件名</param>
        /// <param name="persistenceStatus">持久化状态</param>
        /// <returns></returns>
        public virtual Attachment GenerateAttachmentBase64StringContent(Attachment attachment, string content, string fileName, PersistenceStatus persistenceStatus = SIE.Domain.PersistenceStatus.New)
        {

            if (string.IsNullOrWhiteSpace(content))
            {
                //没有文件内容则直接返回
                return attachment;
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                //空文件名则生成Guid作为文件名
                fileName = Guid.NewGuid().ToString("N");
            }

            var exts = new List<string> { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".webp", ".psd", ".svg", ".tiff", ".jfif" };
            if (!exts.Contains(System.IO.Path.GetExtension(fileName)))
            {
                throw new ValidationException("只能上传图片格式的文件".L10N());
            }

            // 解析出base64的字符，格式形如：data:application/zip;base64,UEsDBBQAAAAIAKJUU02Zjai********
            //content = content.Substring(content.IndexOf(",") + 1);

            attachment.Content = Convert.FromBase64String(content);
            attachment.FileName = fileName;
            attachment.FileExtesion = System.IO.Path.GetExtension(attachment.FileName);
            attachment.FileSize = FormatFileSize(attachment.Content.Length);
            attachment.FilePath = GenerateFilePath(attachment);
            attachment.PersistenceStatus = persistenceStatus;

            return attachment;
        }

        /// <summary>
        /// 计算附件大小
        /// </summary>
        /// <param name="fileSize">文件大小</param>
        /// <returns>计算后文件大小</returns>
        public static string FormatFileSize(long fileSize)
        {
            const long num = 1024L; //byte

            if (fileSize < num) return fileSize + "B"; //B
            if (fileSize < Math.Pow(num, 2)) return (fileSize / num).ToString("f2") + "KB"; //KB
            if (fileSize < Math.Pow(num, 3)) return (fileSize / Math.Pow(num, 2)).ToString("f2") + "MB"; //KB
            if (fileSize < Math.Pow(num, 4)) return (fileSize / Math.Pow(num, 3)).ToString("f2") + "GB"; //KB

            return (fileSize / Math.Pow(num, 4)).ToString("f2") + "TB"; //KB
        }

        /// <summary>
        /// 生成文件上传路径
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public virtual string GenerateFilePath(Attachment attachment)
        {
            var discriminator = RF.Find(attachment.GetType()).EntityMeta?.GetPropertyOrDefault<string>(DiscriminatorExtension.DiscriminatorProperty.Name);
            var path = discriminator + "/" + Guid.NewGuid().ToString("N");
            return path + "/" + attachment.FileName;
        }

        /// <summary>
        /// 验证码校验
        /// </summary>
        /// <param name="wipPressure"></param>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        public virtual bool VerifyCode(string verifyCode)
        {
            var config = ConfigService.GetConfig(new PackingQCVerifyCodeConfig(), typeof(PackingQc));
            if (verifyCode != config.VerifyCode)
                return false;
            return true;
        }

        /// <summary>
        /// 根据物料Id获取物料主表
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual Item GetItem(double itemId)
        {
            return Query<Item>().Where(p => p.Id == itemId).ToList().FirstOrDefault();
        }


        /// <summary>
        /// 上传的装箱标识到SAP
        /// </summary>
        /// <param name="days">多少天前</param>
        /// <returns></returns>
        public virtual EntityList<PackingQc> GetUnUploadSapDatas(int days = 1)
        {
            var date = DateTime.Now.AddDays(-days);
            //QC装箱 未推送记录
            var packingQcs = Query<PackingQc>()
                .Where(p => (p.IsUploadSap == null || p.IsUploadSap == false))
                .Exists<PackingDetail>((x, y) => y.Where(p => p.PackingQcId == x.Id && p.CreateDate > date))
                .ToList();
            //空箱数据,需要推送取消标识
            var packingQcs2 = Query<PackingQc>()
                .Where(p => (p.IsUploadSap == null || p.IsUploadSap == false) && p.PackingNum == 0 && p.UpdateDate > date)
                .ToList();

            var ids = packingQcs.Select(p => p.Id).ToList();
            packingQcs2.ForEach(p =>
            {
                if (!ids.Contains(p.Id))
                    packingQcs.Add(p);
            });

            return packingQcs;

        }

        /// <summary>
        /// A包装采集移除
        /// </summary>
        public virtual void DeleteSave(PackingDetail packingDetail, WipPressureSn wipPressureSn, PackingQc packingQc)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(packingDetail);
                RF.Save(wipPressureSn);
                RF.Save(packingQc);
                tran.Complete();
            }
        }

        /// <summary>
        /// B包装采集移除
        /// </summary>
        public virtual void DeleteCSave(PackingDetail packingDetail, WipBatch wipBatch, PackingQc packingQc)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(packingDetail);
                RF.Save(wipBatch);
                RF.Save(packingQc);
                tran.Complete();
            }
        }

        /// <summary>
        /// 连接器单体包装采集移除
        /// </summary>
        public virtual void DeleteConnectorSnSave(PackingDetail packingDetail, PackingQc packingQc, EngraveLabel engraveLabel, EngraveSn engraveSn)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(packingDetail);
                RF.Save(engraveLabel);
                RF.Save(packingQc);
                RF.Save(engraveSn);
                tran.Complete();
            }
        }

        /// <summary>
        /// 存入包装QC确认表并报工
        /// </summary>
        public virtual string SavePackingQcAndReport(PackingQc packingQc, BlueLable.BlueLable blueBable, string woNo, string barcode, string batchLable, bool isReport)
        {
            string submitMessage = "";
            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(packingQc);
                RF.Save(blueBable);
                //存入包装QC确认明细表
                PackingDetail packingDetail = new PackingDetail();
                packingDetail.PackingQcId = packingQc.Id;
                packingDetail.Confirm = ConfirmEnum.YES;
                packingDetail.BatchLabel = batchLable;
                packingDetail.WorkOrderNo = woNo;
                packingDetail.ProductLabel = barcode;
                packingDetail.LabelType = LabelTypeEnum.KmLabel;
                packingDetail.ReportsType = ReportsTypeEnum.NO;
                packingDetail.PackingNum = 1;
                RF.Save(packingDetail);
                //刻码标签
                var engrave = RT.Service.Resolve<EngraveLabelController>().BoolEngraveLabel(batchLable);
                if (engrave == null)
                    throw new ValidationException("刻码标签{0}不存在".L10nFormat(batchLable));
                //刻码SN
                EngraveSn engraveSn = new EngraveSn();
                engraveSn.EngraveLabelId = engrave.Id;
                engraveSn.Sn = barcode;
                RF.Save(engraveSn);
                //报工
                if (isReport)
                {
                    PackingReportRecord record = new PackingReportRecord();
                    record.BeginDate = DateTime.Now;
                    record.BlueLabel = packingQc.BlueLabel;
                    record.Report = ReportType.Connector;
                    packingQc.BoxState = BoxStateEnum.NO;
                    RF.Save(packingQc);
                    submitMessage = RT.Service.Resolve<PackingQcController>().SubmitData(packingQc, IsTaskFinish: false);
                    record.ReturnMessage = submitMessage;
                    RF.Save(record);
                    if (submitMessage.IsNullOrWhiteSpace())
                    {
                        packingQc.BoxState = BoxStateEnum.NO;
                        packingQc.PackIdent = PackIdentEnum.FullTank;
                        RF.Save(packingQc);
                    }
                }
                trans.Complete();
            }
            return submitMessage;
        }
    }
}
