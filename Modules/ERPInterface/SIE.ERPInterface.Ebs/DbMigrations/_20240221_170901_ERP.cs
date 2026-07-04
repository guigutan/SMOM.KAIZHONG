using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.ERPInterface.Ebs;
using SIE.Rbac.InvOrgs;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using System;

namespace SIE.WCS.DbMigrations
{
    /// <summary>
    /// WCS初始化脚本
    /// </summary>
    public class _20240221_170901_ERP : ManualDbMigration
    {
        /// <summary>
        /// 数据库设置
        /// </summary>
        public override string DbSetting
        {
            get { return EbsInterfaceEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "ERP初始化".L10N(); }
        }

        /// <summary>
        /// 手动升级的类型：数据
        /// </summary>
        public override ManualMigrationType Type
        {
            get { return ManualMigrationType.Data; }
        }

        /// <summary>
        /// 不支持 Down
        /// </summary>
        protected override void Down() { }

        /// <summary>
        /// 注入
        /// </summary>
        protected override void Up()
        {
            this.RunCode(db =>
            {
                ////由于本类没有支持 Down 操作，所以这里面的 Up 需要防止重入。
                if (!RT.InvOrg.HasValue)
                {
                    RT.InvOrg = 1;
                }

                using (var tran = DB.TransactionScope(EbsInterfaceEntityDataProvider.ConnectionStringName))
                {
                    //User
                    var userCode = "InfUser";//"ERP_" + RT.InvOrg;
                    var user = RT.Service.Resolve<UserController>().GetUserByCode(userCode);
                    if(user != null)
                    {
                        return;
                    }
                    user = new User();
                    user.GenerateId();
                    var emp = new Employee();
                    emp.GenerateId();

                    user.Code = "InfUser";//"ERP_" + RT.InvOrg;
                    user.State = State.Enable;
                    user.AuthenticateType = "Native";
                    user.EmployeeId = emp.Id;

                    emp.Code = user.Code;
                    emp.Name = "InfUser";//"ERP登录账号".L10N();
                    emp.UserId = user.Id;
                    emp.EmployeeStatus = EmployeeStatus.Job;
                    RF.Save(user);
                    RF.Save(emp);

                    var security = new UserSecurity
                    {
                        UserId = user.Id,
                        Password = "666666",//SIE.Security.CryptographyHelper.MD5("123456"),
                        ChangePwdNextTime = false,
                        EnforcePwdExpiration = false,
                        EnforcePwdPolicy = false
                    };
                    RF.Save(security);

                    ////保存用户提交事件已创建，这里更新首次登录密码
                    DB.Update<UserSecurity>()
                    .Set(t => t.ChangePwdNextTime, false)
                    .Set(p => p.EnforcePwdExpiration, false)
                    .Set(p => p.EnforcePwdPolicy, false)
                    .Where(t => t.UserId == user.Id)
                    .Execute();

                    var userData = new UserData
                    {
                        UserId = user.Id,
                        CurInvOrg = RT.InvOrg.Value
                    };
                    RF.Save(userData);

                    var org = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);
                    var orgUser = new UserInInvOrg { InvOrg = org, User = user, IsInternal = true };
                    RF.Save(orgUser);

                    tran.Complete();
                }
            });
        }
    }
}
