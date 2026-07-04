using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WIP;
using SIE.Packages.Boxs;
using SIE.Packages.Boxs.Configs;
using SIE.Tech.Processs;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.MES.BatchWIP.Inspects
{
    /// <summary>
    /// DefectiveKeyInControl.xaml 的交互逻辑
    /// </summary>
    public partial class BatchDefectiveKeyInControl : UserControl
    {
        /// <summary>
        /// 批次检验不良集合
        /// </summary>
        private BatchDefectiveSetViewModel _batchDefectSetVmdl;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="defectControlInfo">缺陷信息</param>
        /// <param name="defectInfos">不良记录信息</param>
        /// <param name="batchDefectSetVmdl">批次检验不良集合</param>
        public BatchDefectiveKeyInControl(FrameworkElement defectControlInfo, FrameworkElement defectInfos,
            BatchDefectiveSetViewModel batchDefectSetVmdl)
        {
            InitializeComponent();
            _batchDefectSetVmdl = batchDefectSetVmdl;

            DefectContent.Content = defectControlInfo; ////--批次检验缺陷选择
            //DefectInfos.Content = defectInfos; ////批次检验不良记录

            gdBatchDefectSet.DataContext = _batchDefectSetVmdl;
            GetDefects(_batchDefectSetVmdl); ////获取缺陷信息
        }

        /// <summary>
        /// 获取缺陷信息
        /// </summary>
        /// <param name="batchDefectSetVmdl">批次检验不良集合</param>
        private void GetDefects(BatchDefectiveSetViewModel batchDefectSetVmdl)
        {
            var process = RF.GetById<Process>(batchDefectSetVmdl.Workstation.ProcessId.Value);
            batchDefectSetVmdl.DefectList.Clear();
            batchDefectSetVmdl.DefectList.AddRange(process.DefectList.Select(p => p.Defect));
        }

        /// <summary>
        /// "提交"按钮事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            SubmitValidation();  ////不良记录提交的验证方法

            BatchDefectiveViewModel batchDefectiveVmdl = SubmitCreateBatchDefectiveViewModel(); ////创建批次检验不良明细

            SubmitUpdateInputBatch(batchDefectiveVmdl);  ////不良记录提交更新对应InputBatch的数量
            _batchDefectSetVmdl.batchInspectvm.RefreshInputBatch();
            _batchDefectSetVmdl.batchInspectvm.BatchDefectSetVmdl.Clear(); // 清楚暂存的缺陷信息
            CloseMe(sender, "SimpleButton");
            _batchDefectSetVmdl.batchInspectvm.ShowTips("生成不良批次成功".L10N());
            SubmitRefresh();  ////不良记录提交提交后的刷新方法
        }

        /// <summary>
        /// 不良记录提交的验证方法
        /// </summary>
        private void SubmitValidation()
        {
            // 240726 lyp 不良拆分允许输入小数
            //_batchDefectSetVmdl.NgQty = Math.Round(_batchDefectSetVmdl.NgQty, 0);
            //if (_batchDefectSetVmdl.NgQty < 1)
            //{
            //    throw new ValidationException("不良数量不能小于1! ".L10N());
            //}
            if (_batchDefectSetVmdl.DefectItemList.Count <= 0)
            {
                throw new ValidationException("未选择缺陷代码".L10N());
            }
            if (_batchDefectSetVmdl.NgQty > _batchDefectSetVmdl.RemainQty)
            {
                throw new ValidationException("不良数量不能大于剩余数量!".L10N());
            }
            if (_batchDefectSetVmdl.NgQty <= 0)
            {
                throw new ValidationException("不良数量必须大于0! ".L10N());
            }
        }

        /// <summary>
        /// 不良记录提交，创建批次检验不良明细
        /// </summary>
        /// <returns>批次检验不良明细</returns>
        private BatchDefectiveViewModel SubmitCreateBatchDefectiveViewModel()
        {
            BatchDefectiveViewModel batchDefectiveVmdl = new BatchDefectiveViewModel();
            var selectDefects = _batchDefectSetVmdl.DefectItemList;

            batchDefectiveVmdl.Barcode = _batchDefectSetVmdl.Barcode;
            batchDefectiveVmdl.ChildBarcode = _batchDefectSetVmdl.ChildBarcode;
            batchDefectiveVmdl.NgQty = _batchDefectSetVmdl.NgQty;
            batchDefectiveVmdl.InspectDate = RF.Find<BatchWipProductDefectDetail>().GetDbTime();
            StringBuilder sb = new StringBuilder();
            StringBuilder st= new StringBuilder();
            foreach (var defectItem in selectDefects)
            {
                var curBatchPrdDftDtl = new BatchWipProductDefectDetail();
                curBatchPrdDftDtl.Defect = defectItem.Defect;
                batchDefectiveVmdl.BatchWipPrdDrtDetails.Add(curBatchPrdDftDtl);
                sb.Append(defectItem.Defect.Code + ";");
                st.Append(defectItem.Defect.Description + ";");
              
            }
            batchDefectiveVmdl.Defects = sb.ToString();
            batchDefectiveVmdl.Descriptions += st;

            int curLength = batchDefectiveVmdl.Defects.Length;
            int descLength = batchDefectiveVmdl.Descriptions.Length;
            if (curLength > 0)
            {
                batchDefectiveVmdl.Defects = batchDefectiveVmdl.Defects.Substring(0, curLength - 1);
                batchDefectiveVmdl.Descriptions = batchDefectiveVmdl.Descriptions.Substring(0, descLength - 1);
            }

            _batchDefectSetVmdl.BatchDefectiveViewModels.Add(batchDefectiveVmdl);
            return batchDefectiveVmdl;
        }

        /// <summary>
        /// 不良记录提交更新对应InputBatch的数量
        /// </summary>
        /// <param name="batchDefectiveVmdl">批次检验不良记录</param>
        private void SubmitUpdateInputBatch(BatchDefectiveViewModel batchDefectiveVmdl)
        {
            var inputBatchItem = _batchDefectSetVmdl.InputBatchs.FirstOrDefault(x => x.BatchNo == batchDefectiveVmdl.Barcode && x.SubBatchNo == batchDefectiveVmdl.ChildBarcode);
            Workcell workCell = new Workcell
            {
                ResourceId = inputBatchItem.ResourceId,
                StationId = inputBatchItem.StationId,
                ProcessId = inputBatchItem.ProcessId,
            };
            CreateWipPrdDefects(_batchDefectSetVmdl, null);
            // 缺陷信息
            var defect = _batchDefectSetVmdl.DefectItemList.Select(p => p.Defect).AsEntityList();

            RT.Service.Resolve<WipController>().NewGenerateSplitInput(inputBatchItem, workCell, batchDefectiveVmdl.NgQty, BarcodeType.BatchBarocde, defect ,true);

        }

        /// <summary>
        /// 不良记录提交提交后的刷新方法
        /// </summary>
        private void SubmitRefresh()
        {
            _batchDefectSetVmdl.RemainQty -= _batchDefectSetVmdl.NgQty;
            _batchDefectSetVmdl.NgQty = 0;
            _batchDefectSetVmdl.DefectItemList.Clear();
        }

        /// <summary>
        /// 转出载具条码的KeyDown事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void TxtOutVehicle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            //txtOutVehicle.Focus();
            e.Handled = true;
            if (_batchDefectSetVmdl.BatchDefectiveViewModels != null && _batchDefectSetVmdl.BatchDefectiveViewModels.Count > 0)
            {
                CheckContainerValidation();
                SubWipBatch childBarcode = CreateChildBatch();
                CreateWipPrdDefects(_batchDefectSetVmdl, childBarcode);
                OutputBatch curOutputBatch = CreateOutputBatch( childBarcode);
                _batchDefectSetVmdl.OutputBatchs.Add(curOutputBatch);
                _batchDefectSetVmdl.Clear();
                CloseMe(sender, "TextEdit");
            }
            else
            {
                throw new ValidationException("批次不良明细为空! ".L10nFormat());
            }
        }

        /// <summary>
        /// 检查载具是否能使用
        /// </summary>
        private void CheckContainerValidation()
        {
            var containerNo = _batchDefectSetVmdl.OutPutContainerNo;
            if (!string.IsNullOrEmpty(containerNo))
            {
                var config = ConfigService.GetConfig(new ProductTrunoverBoxTypeConfig());
                var turnBox = RT.Service.Resolve<BoxController>().GetTurnoverBox(containerNo, config.BoxType);
                if (turnBox == null)
                    throw new ValidationException("载具 [{0}] 不存在! ".L10nFormat(containerNo));
                if (turnBox.State != BoxState.Unused)
                    throw new ValidationException("载具的状态为 [{0}] 不是闲置状态! ".L10nFormat(turnBox.State.ToLabel()));
                var curTurnBoxOutCount = _batchDefectSetVmdl.OutputBatchs.Count(x => x.ContainerNo == turnBox.Code);
                if (curTurnBoxOutCount > 0)
                    throw new ValidationException("载具 [{0}] 已经在出站列表中，不允许重复使用! ".L10nFormat(turnBox.Code));

                var ngSum = _batchDefectSetVmdl.BatchDefectiveViewModels.Sum(x => x.NgQty);
                var turnBoxCount = GetContainerCapacity(turnBox);
                if (ngSum - turnBoxCount > 0)
                    throw new ValidationException("不良数量超过了载具 [{0}] 的容量! ".L10nFormat(turnBox.Code));
            }
        }

        /// <summary>
        /// 获取载具容量
        /// 优先获取对应工单物料的容量
        /// 如果没有配置，获取默认容量
        /// </summary>
        /// <param name="turnBox">周转箱</param>
        /// <returns>周转箱的容量</returns>
        private decimal GetContainerCapacity(TurnoverBox turnBox)
        {
            var turnBoxCount = -1.0M;
            if (turnBox.CapacityList != null && turnBox.CapacityList.Any())
            {
                var workOrder = _batchDefectSetVmdl.InputBatchs.FirstOrDefault().WorkOrder;
                var product = workOrder.Product;
                var prdCapacitys = turnBox.CapacityList.Where(x => x.ItemId == product.Id).ToList();
                if (prdCapacitys != null && prdCapacitys.Count > 0)
                {
                    turnBoxCount = prdCapacitys.Max(x => x.Capacity);
                }
            }

            if (turnBoxCount <= 0)
            {
                turnBoxCount = turnBox.Capacity;
            }
            return turnBoxCount;
        }

        /// <summary>
        /// "生成不良批次"按钮事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void GenerateNgBatch_Click(object sender, RoutedEventArgs e)
        {
            if (_batchDefectSetVmdl.WorkOrder == null)
            {
                throw new ValidationException("未找到产线在制工单".L10N());
            }

            if (_batchDefectSetVmdl.BatchDefectiveViewModels != null && _batchDefectSetVmdl.BatchDefectiveViewModels.Count > 0)
            {
                SubWipBatch childBarcode = CreateChildBatch();
                CreateWipPrdDefects(_batchDefectSetVmdl, childBarcode);
                OutputBatch curOutputBatch = CreateOutputBatch( childBarcode);
                _batchDefectSetVmdl.OutputBatchs.Add(curOutputBatch);
                _batchDefectSetVmdl.Clear();
                CloseMe(sender, "SimpleButton");
            }
            else
            {
                throw new ValidationException("批次不良明细为空! ".L10nFormat());
            }
        }

        /// <summary>
        /// 创建子批次
        /// </summary>
        /// <returns>返回子批次</returns>
        private SubWipBatch CreateChildBatch()
        {
            SubWipBatch childBarcode = null;
            childBarcode = RT.Service.Resolve<WipController>().GenerateChildBatch(_batchDefectSetVmdl.WorkOrder.Id); ////生成不良批次条码
            return childBarcode;
        }

        /// <summary>
        /// 设置产品缺陷不良记录
        /// 在MainView的"转出"按钮中保存实体
        /// </summary>
        /// <param name="batchDefectSetVmdl">批次检验不良集合</param>
        /// <param name="childBarcode">子批次条码</param>
        private void CreateWipPrdDefects(BatchDefectiveSetViewModel batchDefectSetVmdl, SubWipBatch childBarcode)
        {
            var curWorkStation = batchDefectSetVmdl.Workstation;
            foreach (var item in batchDefectSetVmdl.BatchDefectiveViewModels)
            {
                var batchWipPrdDftItem = new BatchWipProductDefect();

                batchWipPrdDftItem.BatchNo = item.Barcode;
                batchWipPrdDftItem.SubBatchNo = childBarcode?.BatchNo; ////item.ChildBarcode
                batchWipPrdDftItem.ContainerNo = batchDefectSetVmdl.OutPutContainerNo;
                batchWipPrdDftItem.Qty = item.NgQty;
                ////Remark~~Location~~FixedDate~~FixedBy
                batchWipPrdDftItem.Remark = item.ChildBarcode; ////借用Remark字段，暂存原子批次号
                batchWipPrdDftItem.Process = curWorkStation.Process;
                batchWipPrdDftItem.Station = curWorkStation.Station;
                batchWipPrdDftItem.Resource = curWorkStation.Resource;
                ////BatchWipProductVersion Version ~~~ResponsibilityList ~~MeasureList
                batchWipPrdDftItem.DetailList.AddRange(item.BatchWipPrdDrtDetails);
                batchDefectSetVmdl.BatchWipPrdDefects.Add(batchWipPrdDftItem);
            }
        }

        /// <summary>
        /// 创建不良转出批次
        /// </summary>
        /// <param name="barcodeType">条码类型</param>
        /// <param name="childBarcode">子批次条码</param>
        /// <returns>不良转出批次</returns>
        private OutputBatch CreateOutputBatch(SubWipBatch childBarcode)
        {
            OutputBatch curOutputBatch = new OutputBatch();
            curOutputBatch.ContainerNo = _batchDefectSetVmdl.OutPutContainerNo;
            curOutputBatch.BatchNo = _batchDefectSetVmdl.Barcode;
            curOutputBatch.SubWipBatch = childBarcode;
            curOutputBatch.SubBatchNo = childBarcode?.BatchNo;
            curOutputBatch.IsGenerateBatch = true;
            curOutputBatch.MaxQty = curOutputBatch.Qty = _batchDefectSetVmdl.BatchDefectiveViewModels.Sum(x => x.NgQty);

            curOutputBatch.IsNg = true;
            curOutputBatch.OutputDate = RF.Find<BatchWipProductDefectDetail>().GetDbTime();
            curOutputBatch.WorkOrder = _batchDefectSetVmdl.WorkOrder;
            UpdateOutputBatchRelationBatchs(curOutputBatch);

            return curOutputBatch;
        }

        /// <summary>
        /// 更新转出批次的批次关联关系
        /// </summary>
        /// <param name="curOutputBatch">不良转出批次</param>
        private void UpdateOutputBatchRelationBatchs(OutputBatch curOutputBatch)
        {
            foreach (var batchDftVmlItem in _batchDefectSetVmdl.BatchDefectiveViewModels)
            {
                var inputBatchItem = _batchDefectSetVmdl.InputBatchs.FirstOrDefault(x => x.BatchNo == batchDftVmlItem.Barcode && x.SubBatchNo == batchDftVmlItem.ChildBarcode);

                var curRelationBatch = CreateRelationBatch(inputBatchItem, curOutputBatch, batchDftVmlItem);
                curOutputBatch.RelationBatchList.Add(curRelationBatch);
            }
        }

        /// <summary>
        /// 创建关联批次列表
        /// </summary>
        /// <param name="curInputBatch">当前转入批次</param>
        /// <param name="curOutputBatch">当前转出批次</param>
        /// <param name="batchDftVmlItem">批次检验不良记录</param>
        /// <returns>关联批次列表</returns>
        private RelationBatch CreateRelationBatch(InputBatch curInputBatch, OutputBatch curOutputBatch, BatchDefectiveViewModel batchDftVmlItem)
        {
            var curRelationBatch = new RelationBatch();
            curRelationBatch.InputBatch = curInputBatch;
            curRelationBatch.Qty = batchDftVmlItem.NgQty;
            curRelationBatch.OutputBatch = curOutputBatch;

            /////* ////curInputBatch.BatchNo, curInputBatch.SubBatchNo
            ////var curBatchWipPrdDft = GetBatchWipPrdDft(curOutputBatch.BatchNo, curOutputBatch.SubBatchNo, batchDftVmlItem.Defects, batchDftVmlItem.ChildBarcode); 
            ////curRelationBatch.BatchWipProductDefects.Add(curBatchWipPrdDft); */

            GetBatchWipPrdDft(curOutputBatch, batchDftVmlItem, curRelationBatch);
            return curRelationBatch;
        }

        ///// <summary>
        ///// 获取产品缺陷记录
        ///// </summary>
        ///// <param name="batchNo">批次号</param>
        ///// <param name="subBatchNo">子批次号</param>
        ///// <param name="defects">不良代码字符串</param>
        ///// <param name="childBarcode">原子批次号</param>
        ///// <returns>产品缺陷记录</returns>
        ////private BatchWipProductDefect GetBatchWipPrdDft(string batchNo, string subBatchNo, string defects, string childBarcode)
        ////{
        ////    BatchWipProductDefect result = null;
        ////    var curBatchWipPrdDfts = _batchDefectSetVmdl.BatchWipPrdDefects.Where(x => x.BatchNo == batchNo && x.SubBatchNo == subBatchNo && x.Remark == childBarcode);
        ////    foreach (var batchWipPrdDft in curBatchWipPrdDfts)
        ////    {
        ////        var curDefedts = string.Join(";", batchWipPrdDft.DetailList.Select(x => x.Defect.Code).ToArray());
        ////        if (curDefedts.Equals(defects))
        ////        {
        ////            result = batchWipPrdDft;
        ////            break;
        ////        }
        ////        else
        ////            continue;
        ////    }

        ////    return result;
        ////}

        /// <summary>
        /// 获取产品缺陷记录
        /// </summary>
        /// <param name="outputBatch">当前转出批次</param>
        /// <param name="batchDftVmlItem">批次检验不良记录</param>
        /// <param name="relationBatch">关联批次列表</param>
        private void GetBatchWipPrdDft(OutputBatch outputBatch, BatchDefectiveViewModel batchDftVmlItem, RelationBatch relationBatch)
        {
            var curBatchWipPrdDfts = _batchDefectSetVmdl.BatchWipPrdDefects.Where(x => x.BatchNo == outputBatch.BatchNo
                                                  && x.SubBatchNo == outputBatch.SubBatchNo && x.Remark == batchDftVmlItem.ChildBarcode && x.Qty == batchDftVmlItem.NgQty);
            foreach (var batchWipPrdDft in curBatchWipPrdDfts)
            {
                var curDefedts = string.Join(";", batchWipPrdDft.DetailList.Select(x => x.Defect.Code).ToArray());
                if (curDefedts.Equals(batchDftVmlItem.Defects))
                {
                    if (!relationBatch.BatchWipProductDefects.Contains(batchWipPrdDft))
                        relationBatch.BatchWipProductDefects.Add(batchWipPrdDft);
                }
                else
                    continue;
            }
        }

        /// <summary>
        /// 关闭UserControl
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="actionType">事件对象类型</param>
        private void CloseMe(object sender, string actionType)
        {
            if (actionType == "TextEdit") 
            {
                var txtEdit = sender as DevExpress.Xpf.Editors.TextEdit;
                var txtDialogCont = txtEdit.GetLogicalParent<Workbench.DialogContent>();
                txtDialogCont.Close();
            }

            if (actionType == "SimpleButton")
            {
                var smlBtn = sender as DevExpress.Xpf.Core.SimpleButton;
                var btnDialogCont = smlBtn.GetLogicalParent<Workbench.DialogContent>();
                btnDialogCont.Close();
            }
        }
    }
}
