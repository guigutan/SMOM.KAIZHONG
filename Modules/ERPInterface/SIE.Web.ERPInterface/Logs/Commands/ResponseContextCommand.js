SIE.defineCommand('SIE.Web.ERPInterface.Logs.Commands.ResponseContextCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "查看接收报文", group: "edit", iconCls: "icon-PageSearch icon-blue" },
    canExecute: function (listView) {
        return listView != null && listView.getSelection().length == 1;
    },
    execute: function (view, source) {
        var me = this;
        var downloadJobTimeDetail = me.view.getCurrent().data;
        
        SIE.invokeDataQuery({
            async: false,
            type: "SIE.Web.ERPInterface.Logs.DataQueryer.DownloadExcDataQueryer",
            method: 'ChangeResponseData',
            token: me.view.token,
            params: [downloadJobTimeDetail.Id],
            callback: function (r) {
                if (r.Success && r.Result.length > 0) {
                    var dataStore = Ext.create('Ext.data.Store', {
                        data: r.Result,
                    });
                    me.createWin(dataStore);
                }
                else {
                    me.createSimpleWin(downloadJobTimeDetail);
                }
            }
        });
    },
    createSimpleWin: function (downloadJobTimeDetail) {
        var win = SIE.Window.show({
            title: "查看接收报文".t(),
            width: 472,
            height: 500,
            items: [{
                id: 'ErpResponseStrtextareaId',
                xtype: 'textareafield',
                name: 'ErpResponseStrtextarea1',
                value: downloadJobTimeDetail.ResponseStr,
                LogId: downloadJobTimeDetail.Id,
                readOnly: true,//ebsUploadLog.IsSuccess 
                hideLabel: true,
                anchor: '100%',
                grow: true,
                height: 500,
                width: 450,
                autoScroll: true
            }],
            buttons: ['取消'.t()],
            callback: function (btn) {
                if (btn == "取消".t()) {
                    win.close();
                }
            }
        });
    },
    createWin: function (store) {
        var me = this;
        Ext.QuickTips.init();
        var ctl = me.createTreeCtl(store);
        var ui = me.createPanel(ctl);
        var win = SIE.Window.show({
            title: "查看接收报文".t(),
            width: 700,
            height: 500,
            items: ui,
            buttons: ['取消'.t()],
            callback: function (btn) {
                if (btn == "取消".t()) {
                    win.close();
                }
            }
        });
    },

    createPanel: function (treeCtl) {
        return {
            xtype: 'panel',
            bodyStyle: {
                border: 0
            },
            layout: {
                border: false,
                type: 'fit',
                align: 'stretch',
            },
            items: treeCtl,
        };
    },
    createTreeCtl: function (store) {
        var tree = {
            region: 'center',
            xtype: 'gridpanel',
            reserveScrollbar: true,

            store: store,
            columnLines: true,
            columns: [{
                text: '来源单号'.t(),
                dataIndex: 'SCUX_SOURCE_NUM',
                sortable: true,
                align: 'center',
                width: 120,
            }, {
                text: '来源行号'.t(),
                dataIndex: 'SCUX_SOURCE_LINE_NUM',
                width: 80,
                sortable: true,
                align: 'center',
            }, {
                text: '事务交易Id'.t(),
                dataIndex: 'SCUX_SOURCE_LOT_NUM',
                width: 90,
                sortable: true,
                align: 'center',
            }, {
                text: '返回状态'.t(),
                dataIndex: 'PROCESS_STATUS',
                sortable: true,
                align: 'center',
                width: 110,
                renderer: function (value) {
                    if (value === "COMPLETED") {
                        return "<span style='color:green;'>" + '成功'.t() + "</span>";
                    }
                    if (value === "ERROR") {
                        return "<span style='color:red;'>"+ '失败'.t()+"</span>";
                    }                     
                    return value;
                }
            },
            {
                text: '返回信息'.t(),
                dataIndex: 'PROCESS_MESSAGE',
                width: 250,
                sortable: true,
                align: 'center',
                renderer: function (value, metaData, record) {
                    if (record.data.PROCESS_STATUS == "PENDING" && value == "") {
                        return "ERP处理中".t();
                    }
                    metaData.tdAttr = 'data-qtip="' + value + '"';
                    return value;
                }
            }],
        };

        return tree;
    }
});