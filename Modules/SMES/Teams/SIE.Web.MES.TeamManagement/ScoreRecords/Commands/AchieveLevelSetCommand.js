SIE.defineCommand('SIE.Web.MES.TeamManagement.ScoreRecords.AchieveLevelSetCommand', {
    meta: { text: "绩效等级", group: "edit", iconCls: "icon-Repair icon-blue" },

    canExecute: function (view) {
        return true;
    },

    execute: function (view, source) {
        var me = this;
        ////var params = view.getCurrent().data;
        if (!this.viewMeta) {
            SIE.AutoUI.getMeta({
                async: false,
                ignoreCommands: false,
                isDetail: false,
                ignoreQuery: true,
                viewGroup: "ListView",
                token: this.view.token,
                module: view.module,
                model: "SIE.MES.TeamManagement.ScoreRecords.AchieveLevelSetting",
                callback: function (res) {
                    var mainBlock;
                    if (res.mainBlock)
                        mainBlock = res.mainBlock;
                    else
                        mainBlock = res;
                    var listView = SIE.AutoUI.createListView(mainBlock);
                    var ui = listView.getControl();

                    var win = SIE.Window.show({
                        title: "绩效等级配置".t(),
                        width: 900,
                        height: 520,
                        items: ui,
                        //buttons: [''],
                        id: "AchieveLevelSetting001",
                        callback: function (btn) { ////Undefine callback
                        }
                    });

                    var filter = {
                        Method: 'GetAchieveLevelSettings',
                        Parameters: [] //[params]
                    };
                    filter = Ext.encode(filter);
                    listView.loadData({
                        filter: filter,
                        action: 'queryer',
                        token: me.token,
                        type: 'SIE.Web.MES.TeamManagement.ScoreRecords.DataQuery.AchieveLevelSettingDataQuery',
                    });

                },
            });
        }
    },

});