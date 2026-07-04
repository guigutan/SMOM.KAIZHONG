SIE.defineCommand("SIE.Web.KZ.Base.Interfaces.Commands.InfNcDataLogSOGroupReUploadCommand", {
    meta: { text: "重新同步", group: "edit", iconCls: "icon-Sync icon-yellow" },//Delete.icon - Delete 黄色


    //自定义命令
    //是否可执行
    canExecute: function (view) {
        return true;
    },

    execute: function (ListView, source) {
        var me = this;
        //当前页面 的list

        var selections = ListView.getSelection();
        var ids = selections.map(function (item) { return item.getId(); });
        SIE.Msg.wait("正在处理，请稍等...".t());
        ListView.execute({
            data: ids,
            success: function (res) {
                SIE.Msg.showMessage(res.Result);
                ListView.reloadData();
            }
        });
    }
});

/*SIE.defineCommand("SIE.Web.KZ.Base.Interfaces.Commands.InfNcDataLogSOGroupReUploadCommand", {
    meta: {
        text: "重新同步",
        group: "edit",
        iconCls: "icon-Sync icon-yellow"
    },

    canExecute: function (view) {
        return true;
    },

    execute: function (ListView, source) {
        var me = this;
        var selections = ListView.getSelection();

        if (selections.length === 0) {
            SIE.Msg.showMessage("请选择要同步的数据");
            return;
        }

        var ids = selections.map(function (item) {
            return item.getId();
        });

        SIE.Msg.wait("正在处理，请稍等...");

        ListView.execute({
            data: ids,
            success: function (res) {
                // 安全处理返回结果
                var message = "";

                if (res && res.Result !== undefined && res.Result !== null) {
                    // 确保是字符串类型
                    if (typeof res.Result === 'string') {
                        message = res.Result;
                    } else if (res.Result.toString && typeof res.Result.toString === 'function') {
                        message = res.Result.toString();
                    } else {
                        message = JSON.stringify(res.Result);
                    }
                } else {
                    message = "操作成功";
                }

                // 显示消息
                SIE.Msg.showMessage(message);

                // 刷新列表
                ListView.reloadData();
            },
            failure: function (res) {
                // 错误处理
                var errorMsg = "操作失败";
                if (res && res.message) {
                    errorMsg = res.message;
                } else if (res && res.msg) {
                    errorMsg = res.msg;
                }
                SIE.Msg.showMessage(errorMsg);
            }
        });
    }
});*/