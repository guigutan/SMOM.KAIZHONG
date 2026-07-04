SIE.definePlugin('SIE.RBAC.InvOrgPlugin', {
    init: function (app) {
        var me = this;
        me.initOrgButton();
    },

    /**
     * 添加 切换基地 按钮
     */
    initOrgButton: function () {
        var me = this;
        var div = Ext.getElementById('btnOrg');
        //div.id = "btnOrgExt";
        var config = {
            renderTo: div,
            iconCls: 'iconfont icon-GlobeWire',
            arrowVisible: false,
            border: false,
            handler: function () {
                me.showOrgDialog(false);
            }
        };
        var btn = Ext.create('Ext.button.Button', config);
        SIE.invokeDataQuery({
            method: 'GetCurOrg',
            type: 'SIE.Web.Rbac.InvOrgs.DataQuery.UserInvOrgDataQuery',
            token: 'GetCurOrg',
            success: function (jsonRes) {
                if (jsonRes.Success) {
                    if (!jsonRes.Result) {
                        me.showOrgDialog(true);
                        return;
                    }
                    var invOrg = jsonRes.Result;
                    btn.setText(invOrg.Name);
                    btn.setText("切换工厂");
                    CRT.Context.GlobalContext.setContext('orgInfo', invOrg);
                    //sync
                    var curUser = CurUserStateHelper.getSessionUser();
                    curUser.CurInvOrg = invOrg.Code;
                    CRT.Context.GlobalContext.setContext(portal.userKey, curUser);
                    CurUserStateHelper.setSessionUser(curUser);
                }
            }
        });
    },

    /**
     * 弹出选择组织机构对话框
     * @param {bool} isFirstTime -是否第一次选择
     */
    showOrgDialog: function (isFirstTime) {
        SIE.AutoUI.getMeta({
            model: "SIE.KZ.Base.InvOrgs.InvOrgGroup",
            ignoreCommands: true,
            isReadonly: true,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;

                mainBlock.gridConfig.selModel = {
                    singleSelect: true
                };
                mainBlock.gridConfig.pagingBarConfig = {
                    hidden: true
                };
                var listView = SIE.AutoUI.createListView(mainBlock);
                var ui = listView.getControl();
                var win = SIE.Window.show({
                    title: '切换工厂 '.t() + listView.label.t(),
                    items: ui,
                    width: 600,
                    height: 400,
                    listeners: {
                        close: function () {
                            //if (isFirstTime) {
                            //    location.replace('/Account/LogOff');
                            //}
                        }
                    },
                    callback: function (btn) {
                        if (btn.t() === '确定'.t()) {
                            var selection = listView.getSelection();
                            if (selection.length > 1) {
                                Ext.Msg.show({
                                    title: '错误'.t(),
                                    message: '只能选择一行'.t()
                                });
                                return;
                            }
                            var cmd = Ext.create(
                                'SIE.Web.KZ.RBAC.InvOrgs.Commands.SwitchInvOrgCommand', {});
                            cmd._setOwnerView(listView);
                            cmd.command = Ext.getClassName(cmd);
                            cmd.tryExecute(cmd);
                        } else {
                            //win.close();
                            if (isFirstTime) {
                                location.replace('/Account/LogOff');
                            }
                        }
                    }
                });
                SIE.invokeDataQuery({
                    method: 'GetInvOrgGroupsByUser',
                    type: 'SIE.Web.KZ.RBAC.InvOrgs.InvOrgDataQuery',
                    token: 'GetInvOrgGroupsByUser',
                    success: function (res) {
                        if (res.Success) {
                            var store = SIE.data.Utils.createStore({
                                model: res.Result.model,
                                remoteSort: false
                            })
                            store.setData(res.Result.data)
                            listView.setData(store)
                        }
                    }
                })
            }
        });
    },

})