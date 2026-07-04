using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Tab;
using SIE.Equipments.Enums;
using SIE.EventMessages.EMS.EquipAccount;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Commands
{
    /// <summary>
    /// 计量设备台账保存命令
    /// </summary>
    public class MeteringEquipSaveCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存设备台账
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            //新旧值
            var newEntity = entity as MeteringEquipmentAccount;

            ValidateEquipmentAccount(newEntity);

            //实体添加履历列表
            newEntity.ResumeList.AddRange(GenerateEquipAccountResumes(newEntity));

            //执行保存
            base.DoSave(newEntity);
        }

        /// <summary>
        /// 保存后添加到设备与人员权限中
        /// </summary>
        /// <param name="entity"></param>
        protected override void OnSaved(Entity entity)
        {
            var equip = entity as MeteringEquipmentAccount;
            RT.Service.Resolve<IEquipAccount>().SynDevicePur(new List<double> { equip.Id });
            base.OnSaved(entity);
        }

        /// <summary>
        /// 验证设备信息
        /// </summary>
        /// <param name="MeteringEquipmentAccount">设备台账</param>
        /// <exception cref="ValidationException"></exception>
        protected void ValidateEquipmentAccount(MeteringEquipmentAccount MeteringEquipmentAccount)
        {
            if (MeteringEquipmentAccount == null)
            {
                throw new ValidationException("要保存设备台账实体为空".L10N());
            }
            if (string.IsNullOrEmpty(MeteringEquipmentAccount.Name))
            {
                throw new ValidationException("请输入设备的名称".L10N());
            }
            if (!MeteringEquipmentAccount.UseDepartmentId.HasValue)
            {
                throw new ValidationException("使用部门不能为空".L10N());
            }
            if (!MeteringEquipmentAccount.ManageDepartmentId.HasValue)
            {
                throw new ValidationException("管理部门不能为空".L10N());
            }
        }

        /// <summary>
        /// 创建履历实体
        /// </summary>
        /// <param name="changed">变更类型</param>
        /// <param name="state">设备状态</param>
        /// <param name="oldValue">旧值</param>
        /// <param name="newValue">新值</param>
        /// <returns></returns>
        protected MeterEquipAccountResume GenerateEquipAccountResume(string changed, AccountState state, string oldValue, string newValue)
        {
            return new MeterEquipAccountResume
            {
                Changed = changed,
                State = state,
                ResumeType = ResumeType.Changed,
                Remark = "由[{0}]{2}变成[{1}]{2}".L10nFormat(oldValue, newValue, changed)
            };
        }

        /// <summary>
        ///创建履历列表
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        protected EntityList<MeterEquipAccountResume> GenerateEquipAccountResumes(MeteringEquipmentAccount newEntity)
        {
            var oldEntity = RF.GetById<MeteringEquipmentAccount>(newEntity.Id);
            var equipAccountResumes = new EntityList<MeterEquipAccountResume>();
            //对比字段
            //工序
            if (oldEntity != null)
            {
                if (newEntity.ProcessId != oldEntity.ProcessId)
                    equipAccountResumes.Add(GenerateEquipAccountResume("工序".L10N(), newEntity.State, oldEntity.Process?.Name, newEntity.Process?.Name));
                //产线
                if (newEntity.ResourceId != oldEntity.ResourceId)
                    equipAccountResumes.Add(GenerateEquipAccountResume("产线".L10N(), newEntity.State, oldEntity.Resource?.Name, newEntity.Resource?.Name));
                //车间
                if (newEntity.WorkShopId != oldEntity.WorkShopId)
                    equipAccountResumes.Add(GenerateEquipAccountResume("车间".L10N(), newEntity.State, oldEntity.WorkShop?.Name, newEntity.WorkShop?.Name));
                //部门
                if (newEntity.UseDepartmentId != oldEntity.UseDepartmentId)
                    equipAccountResumes.Add(GenerateEquipAccountResume("部门".L10N(), newEntity.State, oldEntity.UseDepartment?.Name, newEntity.UseDepartment?.Name));
                //责任人
                if (newEntity.ResPersonId != oldEntity.ResPersonId)
                    equipAccountResumes.Add(GenerateEquipAccountResume("责任人".L10N(), newEntity.State, oldEntity.ResPerson?.Name, newEntity.ResPerson?.Name));
                //使用状态
                if (newEntity.UseState != oldEntity.UseState)
                    equipAccountResumes.Add(GenerateEquipAccountResume("使用状态".L10N(), newEntity.State, oldEntity.UseState.ToLabel(), newEntity.UseState.ToLabel()));
            }
            return equipAccountResumes;
        }
    }
}
