using SIE.CrossPlatform.Collect.Common.Extensions;
using SIE.CrossPlatform.Collect.Common.Settings;

namespace SIE.CrossPlatform.Collect.Common.ApiCall
{
    /// <summary>
    /// 平台登录信息
    /// </summary>
    public class LoginInfo
    {
        private  readonly string loginInfoPath = Path.Combine(Global.ExecutingPath, "LoginInfo.txt");
        private readonly string pppPath = Path.Combine(Global.ExecutingPath, "ppp.txt");

        private static LoginInfo _Instance;
        public static LoginInfo Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new LoginInfo();
                    _Instance.LoadFromFile();
                }
                return _Instance;
            }
        }

        public double UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public double EmployeeId { get; set; }
        public string Password { get; set; }
        public double InvOrgId { get; set; }

        public string Ticket { get; set; }

        public string Context { get; set; } = "{'Ticket':'','InvOrgId':''}";

        public List<InvOrg> AllInvOrgs = new List<InvOrg>();

        public List<MenuPermission> AllPermissions = new List<MenuPermission>();

        public List<string> HistoryAccountList = new List<string>();

        public bool IsRemberPwd { get; set; }
        public string RemberUser { get; set; }
        public string RemberPwd { get; set; }
        public string RemberLanguage { get; set; }


        /// <summary>
        /// 读取上一次登录的用户名称
        /// </summary>
        private void LoadFromFile()
        {
            
            HistoryAccountList.Clear();

            if (File.Exists(loginInfoPath))
            {
                string[] lines = File.ReadAllLines(loginInfoPath);
                foreach (string userCode in lines)
                {
                    if (!string.IsNullOrEmpty(userCode))
                        HistoryAccountList.Add(userCode);
                }
            }

            if (File.Exists(pppPath))
            {
                string[] lines = File.ReadAllLines(pppPath);
                IsRemberPwd = lines.Length > 0 && lines[0].Equals("1");
                RemberUser = lines.Length > 1 ? lines[1].AESDecrypt() : "";
                RemberPwd = lines.Length > 2 ? lines[2].AESDecrypt() : "";
                RemberLanguage = lines.Length > 3 ? lines[3] : "";
            }
        }

        /// <summary>
        /// 记住上一次登录的用户名称
        /// </summary>
        public void SaveHistoryAccount()
        {
            if (this.HistoryAccountList.Contains(this.UserCode))
                this.HistoryAccountList.Remove(this.UserCode);

            this.HistoryAccountList.Insert(0, this.UserCode);
            File.WriteAllLines(loginInfoPath, this.HistoryAccountList.ToArray());
        }

        /// <summary>
        /// 清空历史账号
        /// </summary>
        public void CleanHistoryAccount()
        {
            this.HistoryAccountList.Clear();
            File.WriteAllText(loginInfoPath, "");
        }

        /// <summary>
        /// 保存密码
        /// </summary>
        /// <param name="isRemberPwd"></param>
        public void SavePwd(bool isRemberPwd)
        {
            List<string> list = new List<string>();
            list.Add(isRemberPwd ? "1" : "0");
            list.Add(this.UserCode.AESEncrypt());
            list.Add(this.Password.AESEncrypt());
            list.Add(Global.Language == null ? "" : Global.Language.Code);
            File.WriteAllLines(pppPath, list.ToArray());
        }
    }
}
