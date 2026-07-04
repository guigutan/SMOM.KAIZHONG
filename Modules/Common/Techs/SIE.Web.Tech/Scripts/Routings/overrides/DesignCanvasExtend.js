//重写DesignCanvas的部分方法
if (typeof DesignCanvas !== 'undefined') {
    DesignCanvas.override({
        /**
         * 显示脚本编辑窗口
         * @method showScriptWindow
         * @for DesignCanvas
         * @param {object} conninfo 规则信息
         */
        showScriptWindow: function (conninfo) {
            var me = this;
            var versionRecord = me.mainView.CurRoutingVersion;
            var isPublish = versionRecord.get('state') === 1;
            var linedata = {};

            if (!conninfo.linedata) {
                return;
            }

            linedata = JSON.parse(conninfo.linedata);

            if (linedata.ParamResultType !== 'Custom') {
                return;
            }

            var win = Ext.create('SIE.Web.Tech.Processs.Controls.ProcessConditionDialog', {
                title: '编辑脚本'.t(),
                bindcontent: '{l.Expression}',
                callback: function (btn) {
                    if (btn === Ext.window.MessageBox.OK) {
                        conninfo.linedata = JSON.stringify(linedata);
                        conninfo.canvas.setAttribute('data-linedata', conninfo.linedata);
                        win.close();
                    } else if (btn === Ext.window.MessageBox.CANCEL) {
                        win.close();
                    }
                }
            });
            win.getViewModel().setData({
                l: linedata
            });
            //弹窗
            win.show();
        },
    });
}
