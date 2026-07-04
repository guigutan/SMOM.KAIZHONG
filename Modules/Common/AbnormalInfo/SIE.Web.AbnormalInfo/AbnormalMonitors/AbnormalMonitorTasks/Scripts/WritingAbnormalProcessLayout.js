Ext.define('SIE.Web.AbnormalInfo.AnomalyMonitors.AbnormalMonitorTasks.WritingAbnormalProcessLayout',
    {
        extend: 'SIE.autoUI.layouts.Common',
        title: "",
        module: null,
        _layoutFormChildrenCore: function (mainControl, secondPanel) {
            var me = this;
            me.getExtentionViewData(mainControl.SIEView);
            //var control = me.layoutExtentionView(mainControl.SIEView);
            return Ext.widget('container', {
                layout: {
                    type: 'vbox',
                },
                autoScroll: true,
                defaults: {
                    collapsible: false,
                    split: false,
                    layout: 'fit',
                    border: 0
                },
                items: [{
                    id: "AbnormalMainControl",
                    //minHeight: 300,
                    height: "50%",
                    width: "100%",
                    items: mainControl
                },
                {
                    width: "100%",
                    xtype: 'container',
                    id: "AbnormalMonitorTaskExtension",
                    style: {
                        background: '#FBFBFB'
                    },
                    items: [],
                },
                {
                    id: "AbnormalSecondPanel",
                    //minHeight: 400,
                    height: "50%",
                    width: "100%",
                    items: secondPanel
                }]
            });
        },
        getExtentionViewData: function (view) {
            var current = view.getCurrent();
            if (current.getTypeName()) return;
            //获取扩展内容配置
            SIE.invokeDataQuery({
                type: "SIE.Web.AbnormalInfo.AbnormalMonitors.DataQuerys.AbnormalMonitorTaskDataQuery",
                method: "GetExtentionViewData",
                params: [current.getId()],
                token: view.getToken(),
                async: false,
                callback: function callback(res) {
                    if (res.Result == null) {
                        return;
                    }
                    if (res.Success && res.Result.getData()) {
                        var data = res.Result.getData().items;
                        if (data.length > 0) {
                            var entity = data[0];
                            current.setValue(entity.getValue());
                            current.setTypeName(entity.getTypeName());
                            current.setExtName(entity.getExtName());
                            current.markSaved();
                        }
                    }
                }
            });
        }
    });