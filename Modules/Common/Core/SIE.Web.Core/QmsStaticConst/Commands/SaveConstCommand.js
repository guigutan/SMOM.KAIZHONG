SIE.defineCommand('SIE.Web.Core.QmsStaticConst.Commands.SaveConstCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-ClipboardVariantEdit icon-blue" },

    /**
    * @override 命令可执行判断
    * @param {} view 逻辑视图
    * @returns {} 
    */
    canExecute: function (view) {
        return true;
        //this.callParent(arguments);
    },

    /**
     * @override 执行
     * @param {} view 视图
     * @param {} source 
     * @returns {} 
     */
    execute: function (view, source) {

        var me = this;
        me.onSaving(view);

        me.viewExecute({
            withChildren: true,
            success: function (res) {
                me.onSaved(view, res);
            }
        }, view, me);
    },
    viewExecute: function (opt, scope, command) {
        SIE.Msg.wait('正在保存，请稍候...'.t());
        var me = scope || this;
        opt = opt || {};
        if (Ext.isFunction(opt)) {
            opt = {
                callback: opt
            };
        }
        opt = Ext.apply({
            withChildren: false,
            model: me._meta.model
        }, opt);

        var indata = {};
        indata.Type = opt.model;

        if (opt.data) { //支持命令自己传值
            indata.Data = opt.data;
        } else {
            var data = me.getData();
            opt._changeSetData = SIE.data.StaticConstSerializer.serialize(data, opt.withChildren, this);
            if (!opt._changeSetData.isEmpty()) {
                var submitData = opt._changeSetData.getSubmitData();
                indata.Data = submitData;
            }
        }

        if (opt.withIds) {
            indata.SelectedIds = opt.selectIds;
        }

        if (indata.Data || indata.SelectedIds) {
            var outerCallback = opt.callback;
            opt.callback = function (res) {
                //内部处理
                if (outerCallback) {
                    outerCallback(res);
                }
            };
            if (me.getSourceCmd() && me._sourceCmd.ownerCt && me._sourceCmd.ownerCt.ownerCt) {
                var view = me._sourceCmd.ownerCt.ownerCt.SIEView;
                if (view && view._parent) {
                    indata.ParentType = view._parent.model;
                }
            }


            indata.Data = Ext.encode(indata.Data);
            if (typeof (opt.async) == "undefined") opt.async = true;


            SIE.invokeCommand({
                token: opt.token || me.getToken(),
                cmd: opt.command || (me.getSourceCmd() ? me.getSourceCmd().command : null),
                async: opt.async,
                data: indata,
                timeout: opt.timeout,
                sourceCmd: (me.getSourceCmd() ? me.getSourceCmd().command : null),
                isSubmmit: opt.isSubmmit,
                logInfo: opt.logInfo,
                callback: function (res) {
                    if (res.Cancel && opt.cancel) {
                        opt.cancel(res);
                    }
                    else if (res.Success && opt.success)
                        opt.success(res);
                    else if (!res.Success) {
                        if (opt.error)
                            opt.error(res);
                        if (res.Message)
                            SIE.Msg.showError(res.Message);
                    }
                    opt.callback(res);
                }
            });
        } else {
            SIE.Msg.hide();
            SIE.Msg.showWarning('没有可提交的数据，请检查!'.t());
        }
    },
    /**
     * @protected virtual void
    * 保存后的提示信息
     * @param {type} view
     * @param {type} res
     */
    onSavedMsg: function (view, res) {
        SIE.Msg.hide();
        SIE.Msg.showInstantMessage('保存成功'.t());
    },

});