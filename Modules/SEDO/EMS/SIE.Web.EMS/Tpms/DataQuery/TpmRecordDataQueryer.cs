using SIE.Domain;
using SIE.EMS.Tpms;
using SIE.EMS.Tpms.ViewModels;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Tpms.DataQuery
{
    /// <summary>
    /// TPM操作记录查询器
    /// </summary>
    public class TpmRecordDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取所有的TMP检查项目
        /// </summary>
        /// <param name="paras">参数</param>
        /// <returns>所有的TMP检查项目</returns>
        public EntityList<TpmRecordDetail> GetAllTmpScoreItems(string[] paras)
        {
            List<TpmWeekInspectScore> list = RF.GetAll<TpmWeekInspectScore>().ToList();
            EntityList<TpmRecordDetail> vlist = new EntityList<TpmRecordDetail>();
            foreach (var k in list)
            {
                TpmRecordDetail v = new TpmRecordDetail();
                v.WeekInspectScoreId = k.Id;
                v.ProjectName = k.ProjectName;
                v.ProjectType = k.ScoreType;//2019-05-30,ScoreType改为枚举。钟南盛
                //v.ScoreRate = k.ScoreRate;
                //v.CheckStandard = k.CheckStandard;

                vlist.Add(v);
            }
            return vlist;
        }

        /// <summary>
        /// 获取TPM操作记录
        /// </summary>
        /// <returns>TPM操作记录</returns>
        public TpmRecord GetTpmRecord()
        {
            return new TpmRecord()
            {
                TpmNo = RT.Service.Resolve<TpmController>().GetTpmScoreNo()
            };
        }

        /// <summary>
        /// 获取TPM操作记录信息
        /// </summary>
        /// <returns>TPM操作记录信息</returns>
        public TpmRecordInfo GetTpmRecordInfo()
        {
            TpmRecordInfo recordInfo = new TpmRecordInfo();
            recordInfo.ErrMsg = string.Empty;
            try
            {
                recordInfo = RT.Service.Resolve<TpmController>().CreateTpmRecordInfo();
            }
            catch (Exception ex)
            {
                recordInfo.ErrMsg = ex.Message;
            }

            return recordInfo;
        }
    }
}
