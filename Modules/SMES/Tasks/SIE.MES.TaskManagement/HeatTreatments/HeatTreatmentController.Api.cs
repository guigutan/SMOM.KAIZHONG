using Newtonsoft.Json;
using SIE.Api;
using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TaskManagement.HeatTreatments.Datas;
using SIE.Security;
using SIE.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.TaskManagement.HeatTreatments
{
    /// <summary>
    /// 控制器(API)
    /// </summary>
    public partial class HeatTreatmentController
    {
        /// <summary>
        /// 保存老化炉标签进出炉信息
        /// </summary>
        /// <param name="datas"></param>
        /// <exception cref="ValidationException"></exception>
        [ApiService("保存老化炉标签进出炉信息")]
        [AllowAnonymous]
        public virtual void SaveHeatTreatments([ApiParameter("进出炉信息")] List<HeatTreatmentInfo> datas)
        {
            if (datas == null || datas.Count == 0)
                throw new ValidationException("提交数据为空");

            //if (datas.Any(p => p.Barcode.IsNullOrEmpty()))
            //    throw new ValidationException("标签号不能为空");

            if (datas.Any(p => p.Factory.IsNullOrEmpty()))
                throw new ValidationException("工厂不能为空");

            if (datas.Any(p => p.OperationType == null))
                throw new ValidationException("作业类型不能为空");

            RT.Logger.Info($"保存老化炉标签进出炉信息 SaveHeatTreatments: {JsonConvert.SerializeObject(datas)}");

            var invOrgs = RF.GetAll<Rbac.InvOrgs.InvOrg>();
            EntityList<HeatTreatment> htSaveList = new EntityList<HeatTreatment>();

            using (var trans = DB.TransactionScope(TaskManagementEntityDataProvider.ConnectionStringName))
            {
                datas.GroupBy(p => p.Factory).ForEach(data =>
                {
                    var invOrg = invOrgs.FirstOrDefault(p => p.ExternalId == data.Key);
                    RT.InvOrg = invOrg?.Code;
                    var barcodes = data.Select(p => p.Barcode).Distinct().ToList();
                    var wipBatchs = barcodes.SplitContains(temp =>
                    {
                        return RT.Service.Resolve<WipBatchController>().GetWipBatches(temp.ToList());
                    });
                    //var htList = barcodes.SplitContains(temp =>
                    //{
                    //    return GetHeatTreatments(temp.ToList());
                    //});
                    foreach (var item in data)
                    {
                        var wipBatch = wipBatchs.FirstOrDefault(p => p.BatchNo == item.Barcode);
                        //if (wipBatch == null)
                        //    throw new ValidationException("工厂[{0}]标签[{1}]不存在".L10nFormat(item.Factory, item.Barcode));
                        var ht = new HeatTreatment();

                        //if (!item.Barcode.Trim().IsNullOrEmpty())
                        //    ht = htList.FirstOrDefault(p => p.Barcode == item.Barcode && (int?)p.OperationType == item.OperationType);

                        ht = new HeatTreatment()
                        {
                            WipBatch = wipBatch,
                            Barcode = item.Barcode,
                            OperationType = (OperationType?)item.OperationType,
                        };
                        ht.Card00 = item.Card00;
                        ht.Ch1 = item.Ch1;
                        ht.Ch2 = item.Ch2;
                        ht.Ch3 = item.Ch3;
                        ht.Ch4 = item.Ch4;
                        ht.Count00 = item.Count00;
                        ht.DevId = item.DevId;
                        ht.DevName = item.DevName;
                        ht.EnableTime = item.EnableTime;
                        ht.EnableTime1 = item.EnableTime1;
                        ht.ErrNum = item.ErrNum;
                        ht.Flag = item.Flag;
                        ht.Layer00 = item.Layer00;
                        ht.MaterialCode = item.MaterialCode;
                        ht.Model = item.Model;
                        ht.PlanNum = item.PlanNum;
                        ht.ProductNum = item.ProductNum;
                        ht.Rec = item.Rec;
                        ht.RunId = item.RunId;
                        ht.RunPro = item.RunPro;
                        ht.RunTime = item.RunTime;
                        ht.State = item.State;
                        ht.SvId = item.SvId;
                        ht.SvTime = item.SvTime;
                        ht.SvTimeMs = item.SvTimeMs;
                        ht.Tmp = item.Tmp;
                        ht.Tmp1 = item.Tmp1;
                        ht.Tmp2 = item.Tmp2;
                        ht.Tmp3 = item.Tmp3;
                        ht.Tmp4 = item.Tmp4;
                        ht.TmpH = item.TmpH;
                        ht.TmpH1 = item.TmpH1;
                        ht.TmpH2 = item.TmpH2;
                        ht.TmpL = item.TmpL;
                        ht.Type00 = item.Type00;
                        ht.WorkId = item.WorkId;
                        ht.Factory = item.Factory;

                        RF.Save(ht);
                        htSaveList.Add(ht);
                    }

                });
                trans.Complete();
            }


        }
    }
}
