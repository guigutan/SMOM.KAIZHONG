using SIE.Api;
using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.ApiModels;
using SIE.EMS.Tpms.ApiModels;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Tpms
{
    /// <summary>
    /// Tmp控制器API
    /// </summary>
    public partial class TpmController : DomainController
    {
        #region TPM评分 
        /// <summary>
        /// 获取设备TPM信息
        /// </summary>
        /// <param name="equipCode">设备编码</param>
        /// <returns>设备TMP信息</returns>
        [ApiService("获取设备TPM信息")]
        [return: ApiReturn("设备TMP信息 EquipTpmInfo")]
        public virtual EquipTpmInfo GetTpmEquipInfo([ApiParameter("设备编码")] string equipCode)
        {
            var code = equipCode.Trim();
            if (code.IsNullOrEmpty())
                throw new ValidationException("设备编号不能为空".L10N());
            var equip = RT.Service.Resolve<EquipController>().GetEquipAccount(p => p.Code == code);
            if (equip == null)
                throw new ValidationException("找不到该设备！请联系管理员处理！".L10N());
            var tpmEquipData = new EquipTpmInfo()
            {
                EquipInfo = new EquipInfo()
                {
                    Id = equip.Id,
                    WorkShop = equip.WorkShop?.Name,
                    Code = equip.Code,
                    EquipType = equip.EquipModel?.EquipType?.TypeCode,
                    Name = equip.Name,
                    Process = equip.Process?.Name,
                }
            };
            var isDoneTpm = RT.Service.Resolve<TpmController>().HasLatestWeekScore(equip.Id);
            if (isDoneTpm)
            {
                var tpm_query = Query<TpmRecord>().OrderByDescending(o => o.CreateDate).Where(o => o.EquipmentId == equip.Id).FirstOrDefault();
                tpmEquipData.IsFinish = true;//是否完成评分
                tpmEquipData.Score = tpm_query.TotalScore;//得分
                return tpmEquipData;
            }
            else
            {
                tpmEquipData.IsFinish = false;//是否完成评分
                var itemScore = GetAllTmpScoreItems();
                tpmEquipData.DetailDatas.AddRange(itemScore);
            }
            return tpmEquipData;
        }

        /// <summary>
        /// 获取设备所在部门下面的班组集合
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <returns>班组信息列表</returns>
        [ApiService("获取设备所在部门下面的班组集合")]
        [return: ApiReturn("班组信息列表 List<WorkGroupInfo>")]
        public virtual List<BaseDataInfo> GetWorkGroupsByEquipId([ApiParameter("设备ID")] double equipId)
        {
            var equip = GetById<EquipAccount>(equipId);
            if (equip == null)
                throw new ValidationException("找不到对应的设备".L10N());
            if (!equip.WorkShopId.HasValue)
                throw new ValidationException("设备未维护部门信息".L10N());
            var workGroups = RT.Service.Resolve<EmployeeController>().GetWorkGroups(p => p.DepartmentId == equip.WorkShopId);
            if (workGroups.Count == 0)
                throw new ValidationException("部门[{0}]未维护班组".L10nFormat(equip.WorkShop.Name));
            var infos = new List<BaseDataInfo>();
            workGroups.ForEach(e =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                });
            });
            return infos;
        }

        /// <summary>
        /// 提交TPM评分
        /// </summary>
        /// <param name="tpmInfo">设备TMP信息</param> 
        [ApiService("提交TPM评分")]
        public virtual void SubmitTpmScore([ApiParameter("设备TMP信息")] EquipTpmInfo tpmInfo)
        {
            if (tpmInfo.DetailDatas.Count <= 0)
                throw new ValidationException("评分项为空，请维护评分项".L10N());
            if (tpmInfo.DetailDatas.Any(p => p.DeductScore == null))
                throw new ValidationException("扣分栏不可为空，请填写完整".L10N());
            var no = RT.Service.Resolve<TpmController>().GetTpmScoreNo();
            var sumScore = tpmInfo.DetailDatas.Sum(p => p.DeductScore).Value;
            var equipInfo = tpmInfo.EquipInfo;
            TpmRecord mainInfo = new TpmRecord()
            {
                TpmNo = no,
                EquipmentId = equipInfo.Id,
                WorkGroupId = tpmInfo.WorkGroupId,
                ExecutionTime = DateTime.Now,
                ScorerName = RT.Identity.Name,
                TotalScore = (100 - sumScore) <= 0 ? 0 : 100 - sumScore,
                MachineNo = equipInfo.Name,
            };
            var details = new EntityList<TpmRecordDetail>();
            tpmInfo.DetailDatas.ForEach(e =>
            {
                byte[] photo = null;//初始化图片
                if (e.Photo.IsNotEmpty())
                {
                    try
                    {
                        byte[] imageBytes = System.Text.Encoding.Default.GetBytes(e.Photo);
                        photo = imageBytes;
                    }
                    catch
                    {
                        throw new ValidationException("图片字节转换失败".L10N());
                    }
                }

                var dt = new TpmRecordDetail()
                {
                    DeductScore = e.DeductScore.Value,//扣分项
                    Remark = e.Remark,//备注
                    WeekInspectScoreId = e.WeekJobScoreItemId,//评分项id
                    Photo = photo,//图片
                };

                details.Add(dt);

            });

            RT.Service.Resolve<TpmController>().SaveTempScoreRecord(mainInfo, details);
        }

        /// <summary>
        /// 获取所有的TMP检查项目
        /// </summary>
        /// <returns></returns>
        List<TpmDetailInfo> GetAllTmpScoreItems()
        {
            var inspectScores = GetTpmWeekInspectScores();
            if (inspectScores.Count <= 0)
                throw new ValidationException("评分项为空，请维护评分项".L10N());
            var detailList = new List<TpmDetailInfo>();
            foreach (var score in inspectScores)
            {
                detailList.Add(new TpmDetailInfo()
                {
                    WeekJobScoreItemId = score.Id,
                    ProjectName = score.ProjectName,
                    Type = (int)score.ScoreType,
                    StringType = score.ScoreType.ToLabel(),
                    //ScoreRate = score.ScoreRate,
                    //CheckStandard = score.CheckStandard,
                });
            }
            return detailList;
        }

        ///// <summary>
        ///// base64图片的大小是否超过1M
        ///// </summary>
        ///// <param name="base64str"></param>
        ///// <returns></returns>
        //bool GetBase64Length(string base64str)
        //{
        //    var str = base64str.Split(',')[1];
        //    var equalIndex = str.IndexOf('=');
        //    if (str.IndexOf('=') != -1)
        //    {
        //        str = str.Substring(0, equalIndex);
        //    }
        //    var strLength = str.Length;
        //    var fileLength = (strLength - (strLength / 8) * 2) / 1024;
        //    if (fileLength > 1024)
        //    {
        //        throw new ValidationException("图片大小不能超过1M");
        //    }
        //    return true;
        //}
        #endregion
    }
}