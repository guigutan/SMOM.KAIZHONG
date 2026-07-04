
using SIE.Rbac.Security.Authenticates;
using SIE.Rbac.Users;
using SIE.Security;
using SIE.Security.Authentications;

namespace SIE.KZ.Base.Rbac
{
    /// <summary>
    /// 
    /// </summary>
    [AuthenticationMode("KZSmomNative", "KZ-SMOM", 0, Description = "凯中-SMOM验证方式")]
    public class KZSmomNativerProvider : NativeProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public override string Authenticate(string ticket)
        {
            var userCode = ticket;
            var user = RT.Service.Resolve<UserController>().GetUserByCode(userCode);
            if (user == null)
            {
                throw new AuthenticationException(AuthenticationErrorCode.Ticket, "用户[{0}]不存在".FormatArgs(userCode));
            }
            return userCode;// AppRuntime.Service.Resolve<UserController>().GetCode(emp.UserId.Value);
        }
    }
}
