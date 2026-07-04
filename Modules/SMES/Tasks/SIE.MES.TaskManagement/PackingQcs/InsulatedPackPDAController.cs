//using DocumentFormat.OpenXml.Spreadsheet;
using SIE.Api;
using SIE.Barcodes.WipBatchs;
using SIE.Core.ApiLogs;
using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.MES.BlueLable;
using SIE.MES.PackingQC;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.PackingQcs.Data;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.PackingQcs
{
    /// <summary>
    /// 绝缘线包装采集
    /// </summary>
    public partial class InsulatedPackPDAController : DomainController
    {
        /// <summary>
        /// 扫描蓝标
        /// </summary>
        /// <param name="barcode">蓝标信息</param>
        /// <param name="boxExChange">是否开箱 1=换箱,0不换箱</param>
        /// <param name="firstBarcode">原蓝标</param>
        /// <param name="isFirst">是否原蓝标</param>
        /// <returns></returns>
        [ApiService("扫码蓝标")]
        public virtual InsulatePackWoinfo ScanBlueLabel([ApiParameter("扫码蓝标")] string barcode, int boxExChange, string firstBarcode, bool isFirst)
        {
            InsulatePackWoinfo info = new InsulatePackWoinfo();
            var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(barcode);
            if (boxExChange == 0)
            {
                info.PackingDetail = null;
            }

            info.Scan = false;
            info.IsUse = true;
            SIE.MES.BlueLable.BlueLable blueBable = new BlueLable.BlueLable();
            //if (boxExChange == 0)
            //{
            //    blueBable = RT.Service.Resolve<PackingQcController>().GetBlueLable(barcode);
            //}
            //else
            //{
            //    blueBable = RT.Service.Resolve<PackingQcController>().AllBlueLable(barcode);
            //}

            blueBable = RT.Service.Resolve<PackingQcController>().AllBlueLable(barcode);

            if (blueBable == null)
            {
                info.Error = "系统中没有此蓝标!";
                return info;
            }
            var itemData = RT.Service.Resolve<PackingQcController>().GetItem(blueBable.ItemId);
            if (itemData == null)
            {
                info.Error = "蓝标中的物料,在系统中不存在!";
            }
            info.XtBlue = barcode;
            info.BlueZInt = blueBable.PackageNum;
            //info.WoId = WorkOrder.Id;
            info.WoNo = blueBable.ProductionNo;
            info.ProductName = itemData.Name;
            info.ProductCode = itemData.Code;
            info.ShortDescription = itemData.ShortDescription;



            if (packingQc == null)
            {
                if (boxExChange == 1 && isFirst == true)
                {
                    info.Error = "新蓝标无需换箱";
                    return info;
                }

                //蓝标下明细为空，且有删除标识
                var Blue = RT.Service.Resolve<PackingQcController>().AllBlueLable(barcode);
                if (Blue.CreateDeleteident == "删除")
                {
                    info.Error = "该蓝标存在装箱明细，不允许换箱";
                    return info;
                }

                if (boxExChange == 1)
                {
                    info.XtBlue = barcode;

                    info.Tips = "请点提交按钮,确认换箱!!!";
                    return info;
                }
            }
            else
            {
                //换箱，且原标签不为空，就说明当前扫的是第二个蓝标
                if (boxExChange == 1 && isFirst == false)
                {
                    //第二个蓝标需要校验
                    if (packingQc.PackingDetailList.Count > 0)
                    {
                        throw new ValidationException("该蓝标存在装箱明细，不允许换箱".L10N());
                        //info.Error = "该蓝标存在装箱明细，不允许换箱";
                        //return info;
                    }
                }

                //开箱
                var packDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetail(packingQc.Id);
                //记录已经装了多少箱数量
                List<InsulatePackModel> list = new List<InsulatePackModel>();
                foreach (var item in packDetailList)
                {
                    InsulatePackModel pqc = new InsulatePackModel();
                    pqc.BlueLabel = barcode;

                    pqc.ItemLabel = item.ProductLabel;
                    pqc.PackingNum = item.PackingNum;
                    pqc.ItemId = packingQc.ItemId;
                    pqc.ItemName = packingQc.Item.Name;
                    info.BlueInt += 1;
                    list.Add(pqc);
                }
                info.PackingDetail = list;
                if (boxExChange == 1)
                {
                    info.YXtBlue = barcode;
                    info.Tips = "请扫码换箱的蓝标!!!";
                    return info;
                }

                if (packingQc.BoxState != BoxStateEnum.NO)
                {

                }
                else
                {
                    info.Error = "该蓝标已经封箱,请先点击开箱按钮!";
                    return info;
                }
            }
            info.XtBlue = barcode;
            info.Scan = true;
            info.Tips = "请输入物料标签";
            return info;
        }

        /// <summary>
        /// 扫描物料标签
        /// </summary>
        /// <param name="barcode">物料标签</param>
        /// <param name="woInfo"></param>
        /// <returns></returns>
        [ApiService("扫码物料标签")]
        [ApiLog]
        public virtual InsulatePackWoinfo ScanItemLabel([ApiParameter("扫码物料标签")] string barcode, InsulatePackWoinfo woInfo, int deleteIdent = 0)
        {
            InsulatePackWoinfo info = new InsulatePackWoinfo();
            info = woInfo;
            info.Error = "";
            info.Tips = "";
            info.IsUse = true;
            var list = info.PackingDetail;
            int blueLable = 0;
            if (list == null)
            {
                list = new List<InsulatePackModel>();
            }

            //包装采集主表
            PackingQc packingQc = new PackingQc();
            //物料标签
            ItemLabel itemLabel = new ItemLabel();

            double resourceId = RT.Service.Resolve<WipResourceController>().GetOneWipResource();

            packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(info.XtBlue);

            var blueBable = RT.Service.Resolve<PackingQcController>().AllBlueLable(info.XtBlue);

            //获取产品
            var itemData = RT.Service.Resolve<PackingQcController>().GetItem(blueBable.ItemId);
            if (itemData == null)
            {
                info.Error = "蓝标中的物料,在系统中不存在!";
            }

            if (deleteIdent == 1)
            {
                return DeleteLabel(woInfo.XtBlue, barcode, itemData, info);

            }
            else
            {
                if (blueBable.CreateDeleteident == "删除")
                {
                    throw new ValidationException("删除的蓝标不允许其他操作，只能移除!");
                }
            }


            //批次标签等于null， 在去物料标签查询。
            itemLabel = RT.Service.Resolve<ItemLabelController>().GetPackingItemLabel(barcode);
            if (itemLabel == null)
            {
                info.Error = "当前物料标签,物料类型为批次生产采集的,物料条码【" + barcode + "】不存在!";
                return info;
            }

            if (packingQc != null)
            {
                var packdetails = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
                blueLable = packdetails.Sum(p => p.PackingNum);
                if (blueLable >= info.BlueZInt)
                {
                    info.Error = "已装箱数,大于蓝标数,不允许继续装箱!";
                    return info;
                }
            }

            if (itemLabel.Isuse)
            {
                info.Error = "物料标签【" + barcode + "】已经使用!";
                return info;
            }

            #region 存入数据库

            //界面显示明细

            InsulatePackModel pqc = new InsulatePackModel();
            pqc.BlueLabel = info.XtBlue;
            pqc.ItemLabel = barcode;

            pqc.ItemId = itemData.Id;
            pqc.ItemName = itemData.Name;

            info.BlueInt += 1;
            pqc.PackingNum = 1;

            list.Add(pqc);

            if (packingQc == null)
            {
                packingQc = new PackingQc();
                packingQc.BlueLabel = info.XtBlue;
                packingQc.Confirm = ConfirmEnum.YES;
                packingQc.PackIdent = PackIdentEnum.NotFullTank;
                packingQc.ProductLabel = barcode;
                packingQc.ItemId = itemData.Id;
                packingQc.ItemName = itemData.Name;
                packingQc.BlueLableNum = info.BlueZInt;
                packingQc.PackingNum = info.BlueInt;
                packingQc.ResourceId = resourceId;
                packingQc.BoxState = BoxStateEnum.YES;
                packingQc.ReportsType = ReportsTypeEnum.NO;
                blueBable.IsPack = true;
            }
            else
            {
                if (info.BlueInt == blueBable.PackageNum)
                {
                    packingQc.PackIdent = PackIdentEnum.FullTank;
                }
                else
                {
                    packingQc.PackIdent = PackIdentEnum.NotFullTank;
                }

                packingQc.Confirm = ConfirmEnum.YES;
                packingQc.PackingNum = info.BlueInt;
                blueBable.IsPack = true;
            }
            //存入包装QC确认表
            RF.Save(packingQc);
            RF.Save(blueBable);
            PackingDetail packingDetail = new PackingDetail();
            packingDetail.PackingNum = 1;
            packingDetail.ProductLabel = barcode;
            packingDetail.PackingQcId = packingQc.Id;
            packingDetail.Confirm = ConfirmEnum.YES;
            packingDetail.WorkOrderNo = blueBable.ProductionNo;
            packingDetail.LabelType = LabelTypeEnum.SnLabel;
            packingDetail.ReportsType = ReportsTypeEnum.NO;
            RF.Save(packingDetail);
            itemLabel.Isuse = true;
            RF.Save(itemLabel);
            #endregion
            if (info.BlueInt == info.BlueZInt)
            {
                info.Tips = "已经装箱完成,请输入蓝标标签!";
                packingQc.BoxState = BoxStateEnum.NO;
                packingQc.PackIdent = PackIdentEnum.FullTank;
                RF.Save(packingQc);
                return info;
            }
            info.PackingDetail = list;
            info.Tips = "情输入工序标签";
            return info;
        }

        /// <summary>
        /// 开箱
        /// </summary>
        /// <param name="barcode">蓝标</param>
        /// <param name="woInfo"></param>
        /// <returns></returns>
        /// <exception cref="PlatformException"></exception>
        [ApiService("开箱")]
        public virtual InsulatePackWoinfo InsulateBoxing([ApiParameter("开箱")] string barcode, InsulatePackWoinfo woInfo)
        {
            if (barcode == "")
            {
                throw new PlatformException("蓝标不能为空!");
            }
            InsulatePackWoinfo info = new InsulatePackWoinfo();
            info = woInfo;
            List<InsulatePackModel> list = new List<InsulatePackModel>();
            var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(barcode);
            if (packingQc.BoxState == BoxStateEnum.YES)
            {
                info.Error = "该蓝标是开箱状态,无需再次开箱!";
                return info;
            }

            var packDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetail(packingQc.Id);
            foreach (var item in packDetailList)
            {
                InsulatePackModel pqc = new InsulatePackModel();
                pqc.BlueLabel = info.XtBlue;
                pqc.ItemLabel = item.ProductLabel;
                pqc.ItemId = packingQc.ItemId;
                pqc.ItemName = packingQc.Item.Name;
                pqc.PackingNum = item.PackingNum;
                list.Add(pqc);
            }
            info.PackingDetail = list;
            packingQc.BoxState = BoxStateEnum.YES;
            packingQc.ReportsType = ReportsTypeEnum.NO;
            RF.Save(packingQc);
            info.Tips = "已开箱请输入物料标签!";
            info.Error = "";
            return info;
        }

        /// <summary>
        /// InsulatePackWoinfo
        /// </summary>
        /// <param name="xtBlue">蓝标</param>
        /// <param name="yxtBlue">原蓝标</param>
        /// <param name="boxExChange">1换箱 其他提交</param>
        /// <returns></returns>
        [ApiService("提交")]
        public virtual string NewNormal([ApiParameter("提交")] string xtBlue, string yxtBlue, int boxExChange)
        {
            if (boxExChange == 1)
            {
                if (xtBlue == null)
                {
                    throw new ValidationException("蓝标不能为空!");
                }
                if (yxtBlue == null)
                {
                    throw new ValidationException("原蓝标不能为空!");
                }

                var XtblueBable = RT.Service.Resolve<PackingQcController>().AllBlueLable(xtBlue);
                if (XtblueBable == null)
                {
                    throw new ValidationException("蓝标不存在!");
                }
                var YXtblueBable = RT.Service.Resolve<PackingQcController>().AllBlueLable(yxtBlue);
                if (YXtblueBable == null)
                {
                    throw new ValidationException("原蓝标不存在!");
                }

                //原蓝标查询装箱数量
                var yXpackingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(yxtBlue);

                //现有蓝标
                var packdetails = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(yXpackingQc.Id);
                var detailSum = packdetails.Sum(p => p.PackingNum);

                if (XtblueBable.ProductionNo == YXtblueBable.ProductionNo)
                {
                    //查找包装QC主表BlueLableController
                    var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(yxtBlue);
                    if (packingQc != null)
                    {
                        if (detailSum != XtblueBable.PackageNum)
                        {
                            packingQc.PackIdent = PackIdentEnum.NotFullTank;
                        }
                        //if (XtblueBable.PackageNum >= detailSum)
                        if (XtblueBable.PackageNum >= packdetails.Sum(p => p.PackingNum))
                        {
                            packingQc.BlueLableNum = XtblueBable.PackageNum;

                            packingQc.BlueLabel = xtBlue;
                            packingQc.OldBlueLabel = yxtBlue;
                            packingQc.IsUploadSap = false;
                            packingQc.UploadResult = "";
                            if (packingQc.PackingDetailList.Sum(p => p.PackingNum) == packingQc.BlueLableNum)
                            {
                                packingQc.PackIdent = PackIdentEnum.FullTank;
                            }
                            RF.Save(packingQc);
                            YXtblueBable.IsPack = false;
                            XtblueBable.IsPack = true;
                            RF.Save(YXtblueBable);
                            RF.Save(XtblueBable);
                        }
                        else
                        {
                            throw new ValidationException("装箱数大于蓝标数!");
                        }
                    }
                    var XzpackingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(xtBlue);



                    return "换箱成功!!!";
                }
                else
                {
                    throw new ValidationException("现在的蓝标和原蓝标的工单号不一致!");
                }

            }
            else
            {

                string message = "";
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    //查找包装QC主表
                    var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(xtBlue);

                    if (packingQc != null)
                    {
                        var boolWork = RT.Service.Resolve<PackingQcController>().BoolUnreportedWork(packingQc.Id);
                        if (boolWork == true)
                        {
                            packingQc.Confirm = ConfirmEnum.YES;
                            packingQc.BoxState = BoxStateEnum.NO;
                            RF.Save(packingQc);
                        }
                        else
                        {
                            return "没有需要报工的数据,无需提交！！！";
                        }
                    }
                    tran.Complete();
                    return message;

                }
            }
        }


        /// <summary>
        /// 移除批次标签
        /// </summary>
        /// <param name="xtBlue">蓝标</param>
        /// <param name="barcode">批次标签</param>
        /// <param name="itemData">物料</param>
        public virtual InsulatePackWoinfo DeleteLabel(string xtBlue, string barcode, Item itemData, InsulatePackWoinfo info)
        {
            List<InsulatePackModel> list = new List<InsulatePackModel>();
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //主表
                var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(xtBlue);
                if (packingQc == null)
                {
                    throw new ValidationException("此蓝标没有包装!");
                }
                //从表
                var packingDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
                var itemLabel = RT.Service.Resolve<ItemLabelController>().GetPackingItemLabel(barcode);
                if (packingDetailList.Count > 0)
                {
                    var packDetail = packingDetailList.Where(p => p.ProductLabel == barcode).FirstOrDefault();
                    if (packDetail != null)
                    {
                        if (packDetail.ReportsType == ReportsTypeEnum.NO)
                        {
                            info.BlueInt -= packDetail.PackingNum;
                            packingQc.PackingNum -= packDetail.PackingNum;
                            packDetail.PersistenceStatus = PersistenceStatus.Deleted;
                            RF.Save(packDetail);
                            itemLabel.Isuse = false;
                            RF.Save(itemLabel);
                            if (packingDetailList.Count == 1)
                            {
                                packingQc.PersistenceStatus = PersistenceStatus.Deleted;
                                info.PackingDetail = null;
                            }
                            else
                            {
                                var newPackingDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
                                foreach (var item in newPackingDetailList)
                                {
                                    InsulatePackModel pqc = new InsulatePackModel();
                                    pqc.BlueLabel = xtBlue;
                                    //pqc.Confirm = "是";
                                    //pqc.PackIdent = "不满箱";
                                    //pqc.ProductLabel = item.ProductLabel;
                                    pqc.ItemId = itemData.Id;
                                    pqc.ItemName = itemData.Name;
                                    pqc.PackingNum = item.PackingNum;
                                    list.Add(pqc);
                                }
                                info.PackingDetail = list;
                            }

                            RF.Save(packingQc);
                        }
                        else
                        {
                            throw new ValidationException("该标签【" + barcode + "】已经报工,没法移除!");
                        }
                    }
                    else
                    {
                        throw new ValidationException("该标签不存在,标签【" + barcode + "】!");
                    }
                }
                else
                {
                    throw new ValidationException("蓝标【" + xtBlue + "】没有装箱!");
                }
                tran.Complete();
            }
            info.IsUse = true;
            info.Tips = "移除成功!";
            return info;
        }
    }
}
