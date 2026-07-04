SIE.defineCommand('SIE.Web.EMS.AssetIssues.Commands.ApprovalAssetIssueCommand', {
    meta: { text: "审核", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        if (view.hasSelectedEntities()) {
            var flag = true;
            Ext.each(view.getSelection(), function (item) {
                if (item.getApprovalStatus() != 20) {
                    flag = false;
                    return false;
                }
            });
            return flag;
        }
        else {
            return false;
        }
    },
    canVisible: function (view, source) {
        var configValue = CRT.Context.PageContext.getContext('AssetIssueConfig');
        if (!configValue.EnableAudit) {
            return false;
        }
        else {
            if (configValue.EnableApproval) {
                return false;
            }
        }
        return true;
    },
    execute: function (view, source) {
        var win = SIE.Window.show({
            title: "审核".t(),
            width: 500,
            height: 250,
            items: {
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'combobox',
                    id: 'AssetIssuesApproval_Result',
                    name: 'AssetIssuesApprovalResult',
                    margin: '5',
                    fieldLabel: '审核结果'.t(),
                    editable: false,
                    queryMode: 'local',
                    displayField: 'Name',
                    valueField: 'Value',
                    value:30,
                    store: {
                        fields: ["Name", "Value"],
                        data: [
                            { Name: "通过".t(), Value: 30 },
                            { Name: "驳回".t(), Value: 40 }
                        ]
                    },
                }, {
                    xtype: 'textareafield',
                    grow: true,
                    id: 'AssetIssuesApproval_Opinions',
                    name: 'Opinions',
                    margin: '5',
                    fieldLabel: '审核意见'.t(),
                    anchor: '100%'
                }]
            },
            id: "AssetIssuesApprovalCommand001",
            callback: function (btn) {
                if (btn == "确定".t()) {
                    var selectModels = view.getSelection();
                    var selectIds = view.getSelectionIds(selectModels);
                    var result = Ext.getCmp('AssetIssuesApproval_Result');
                    var opinions = Ext.getCmp('AssetIssuesApproval_Opinions');
                    var postdata = {
                        ApprovalResult: result.value,
                        Remark: opinions.value,
                    };
                    view.execute({
                        data: postdata,
                        withIds: true,
                        selectIds: selectIds,
                        success: function (res) {
                            SIE.Msg.showMessage("审核成功!".t());
                            win.close();
                            view.reloadData();
                        }
                    });
                    return false;
                }
            }
        });
    }
});