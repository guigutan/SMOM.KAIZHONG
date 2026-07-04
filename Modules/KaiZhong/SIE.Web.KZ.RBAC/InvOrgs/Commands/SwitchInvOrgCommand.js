SIE.defineCommand("SIE.Web.KZ.RBAC.InvOrgs.Commands.SwitchInvOrgCommand", {
    meta: { text: "SIE", iconCls: "iconfont icon-Globe" },
    canExecute: function (view) {
        return view.getSelection().length > 0;
    },
    execute: function (view, source) {
        var selection = view.getSelection();
        view.execute({
            data: selection[0].data,
            success: function (res) {
                if (res.Result.length == 0) {
                    var orgData = {
                        Id: selection[0].data.Id,
                        Code: selection[0].data.Code,
                        Name: selection[0].data.Name
                    };
                    var curUser = CurUserStateHelper.getSessionUser();
                    curUser.CurInvOrg = orgData.Code;
                    CurUserStateHelper.setSessionUser(curUser);
                    CRT.Context.GlobalContext.setContext(portal.userKey, curUser);
                    location.replace('/');
                } else
                    location.replace(res.Result);
            }
        });
    }
});