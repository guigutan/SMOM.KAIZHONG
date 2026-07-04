SIE.defineCommand('SIE.Web.MES.PrepareProducts.Commands.ComfrimCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "确认", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        if (view.getCurrent() === null) {
            return false;
        }
        if (view.getCurrent() && view.getCurrent().getData().PrepareState != 0) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        SIE.Msg.askQuestion("是否确认？确认后信息不能修改。".t(),
            function () {
                //提交时，数据设置为脏，重新保存并校验所有内容。
                view.getCurrent().dirty = true;

                me.doSave(view);
            });
    }, doSave: function (view) {
        var me = this;
        var children = view.getChildren();
        var childrenData = children[0].getData().getData().items;
        var prepareRecordDetail = [];
        if (childrenData) {
            childrenData.forEach(function (item) {
                prepareRecordDetail.push(item.getData());
            });
        }
        var indata = Ext.encode(prepareRecordDetail);
        console.log(indata);
        view.execute({
            data: prepareRecordDetail,
            success: function (res) {
                if (res.Result == false) {
                    SIE.Msg.showError("保存失败".t());
                    return;
                }
                else
                    me.onSavedHandler(me, res);
            }
        });
    },
    /**
    * 关闭新增页面
    * @param {any} returnObj
    */
    closeAddView: function (returnObj) {
        var data = this.getCurrent();
        returnObj.data = data;
        returnObj.hasData = false;
    },
    /**
     * 重写保存后方法，保存后打开填写检验报告
     * @param {} view 
     * @returns {} 
     */
    onSavedHandler: function (view, res) {
        var me = this;
        SIE.Msg.showInstantMessage('保存成功！'.t(), '提示'.t(), 3);
        var current = view.view.getCurrent()
        SIE.Web.Core.CommonFuns.markSaved(current);
        if (res && res.Result) {
            setTimeout(function () {
                CRT.Event.fire(view.view.model + "_refresh", current.data.Id);
                var currentTab = CRT.Workbench.getTabPanel().getActiveTab();
                CRT.Workbench.closeTab(currentTab);
            }
                , 3000)

        }
    },
});