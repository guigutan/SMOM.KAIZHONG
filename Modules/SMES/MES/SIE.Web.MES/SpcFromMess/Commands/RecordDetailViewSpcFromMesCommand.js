SIE.defineCommand('SIE.Web.MES.SpcFromMess.Commands.RecordDetailViewSpcFromMesCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "记录测量数据", group: "edit", iconCls: "icon-PageSearch icon-blue" },

    canExecute: function (view) {
        if (!view) { console.log('视图不存在'); return false; }
        if (view.getSelection && typeof view.getSelection === 'function') {
            console.log('有 getSelection');
            return view.getSelection().length === 1;
        }
        return true;
    },

    // 重写 execute 方法，直接调用 showView
    execute: function (view, source) {
        console.log('execute 被调用了！');
        var selection = view.getSelection();
        if (selection && selection.length === 1) {
            this.showView(selection[0]);
        } else {
            SIE.Msg.showWarning('请选择一条记录');
        }
    },

    showView: function (editEntity) {
        console.log('已经进入showView:', editEntity);
        if (!editEntity) {
            console.log('请先选择一条记录');
            return;
        }
        var me = this;
        CRT.Workbench.addPage({
            entityType: me.view.model,
            recordId: editEntity.getId(),
            viewGroup: "RecordDetailView",
            title: Ext.String.format('记录测量数据-{0}'.L10N(), editEntity.getNo()),
            isDetail: true,
            params: {
                token: me.view.token,
                action: 3
            }
        });
    }
});