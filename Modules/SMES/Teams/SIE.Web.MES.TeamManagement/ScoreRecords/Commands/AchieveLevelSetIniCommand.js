SIE.defineCommand('SIE.Web.MES.TeamManagement.ScoreRecords.AchieveLevelSetIniCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "初始化".t(), group: "edit", iconCls: "icon-EditEntity icon-blue" },

    canExecute: function (view) {
        return true;
    },

    execute: function (view, source) {
        var me = this;
        var indata = {};
        var viewData = this.view.getData();
        ////var current = this.view.getCurrent();
        var inforData = '绩效等级配置初始化';
        indata.Data = Ext.encode({ InforData: inforData });
        var dialogFlag = Ext.Msg.show({
            title: '提示'.t(),
            message: '您确定要初始化吗?'.t(),
            buttons: Ext.Msg.YESNO,
            icon: Ext.Msg.QUESTION,
            fn: function (btn) {
                if (btn === 'yes') {
                    view.execute({
                        data: indata,
                        success: function (res) {
                            /*viewData.removeAll();
                            viewData.add(res.Result);*/
                            SIE.Msg.showInstantMessage('初始化完成!'.t(), "初始化完成".t(), 3);
                            var filter = {
                                Method: 'GetAchieveLevelSettings',
                                Parameters: []
                            };
                            filter = Ext.encode(filter);
                            view.loadData({
                                filter: filter,
                                action: 'queryer',
                                token: me.token,
                                type: 'SIE.Web.MES.TeamManagement.ScoreRecords.DataQuery.AchieveLevelSettingDataQuery',
                            });
                        },
                        error: function (res) {
                            Ext.Msg.alert('提示', res.Message);
                        }
                    });
                } else if (btn === 'no') {
                    console.log('No pressed');
                    return;
                } else {
                    console.log('Cancel pressed');
                    return;
                }
            }
        });


    },

});