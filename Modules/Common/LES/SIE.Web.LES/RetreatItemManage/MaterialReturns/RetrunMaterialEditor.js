Ext.define('SIE.Web.LES.RetreatItemManage.MaterialReturns.RetrunMaterialEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.RetrunMaterialEditor',
    items: [{
        xtype: 'textfield',
        id: 'RetrunMaterialSn',
        name: 'RetrunMaterialSn',
        hideLabel: false,
        style: 'width:100%;border-color:#3892D4;',
        fieldStyle: 'background-color:#90EE90;height:30px;',
        allowBlank: true,
        forceSelection: true,
        listeners: {
            specialkey: function (comp, e) {

                var me = this;
                if (e.getKey() == e.ENTER) {
                    var barcode = comp.getValue();
                    if (barcode == "") return;
                    var form = this.up('form').SIEView;
                    var ctl = form.getControl();
                    if (ctl && ctl.up() && ctl.up().up())
                        ctl.up().up().setLoading(true); //开始提交

                    if (barcode === "") {
                        SIE.Msg.showMessage("请扫描标签/批次号/物料号".t());
                        comp.setValue("");
                        if (ctl && ctl.up() && ctl.up().up())
                            ctl.up().up().setLoading(false);
                        return;
                    }
                    var me = this;
                    SIE.invokeDataQuery({
                        type: "SIE.Web.LES.RetreatItemManage.MaterialReturns.MaterialReturnsDataQueryer",
                        method: "RetrunSearch",
                        params: [barcode],
                        async: true,
                        token: form.token,
                        success: function (res) {
                            var allResult = res.Result.data.items;
                            if (res.Result.data.items.length > 1) {
                                SIE.AutoUI.getMeta({
                                    model: "SIE.LES.RetreatItemManage.MaterialReturns.MaterialReturnForSelect",
                                    module: "SIE.LES.RetreatItemManage.MaterialReturns.MaterialReturn,SIE.LES",
                                    ignoreCommands: true,
                                    isReadonly: false,
                                    ignoreQuery: false,
                                    isAggt: true,
                                    callback: function (res) {
                                        var mainBlock = res;
                                        if (mainBlock.surrounders) {
                                            debugger;
                                           
                                            var surround = mainBlock.surrounders["0"];
                                            if (surround) {
                                                var items = surround.mainBlock.formConfig.items;
                                                for (var i = 0, len = items.length; i < len; i++) {
                                                    var item = items[i];
                                                    if (item.name == "Sn") {
                                                        item.readOnly = true;
                                                    }
                                                }
                                            }
                                        }

                                        if (ctl && ctl.up() && ctl.up().up())
                                            ctl.up().up().setLoading(false); //提交结束
                                        me.token = mainBlock.token;
                                        var aggtUi = SIE.AutoUI.generateAggtControl(mainBlock);
                                        debugger;
                                        var ui = aggtUi.getControl();
                                        var listView = aggtUi.getView();
                                        if (listView._relations[0]) {
                                            var criteria = listView._relations[0]._target.getData();
                                            criteria.setSn(barcode);
                                        }
                                        me.view = listView;
                                        var win = SIE.Window.show({
                                            title: "标签不唯一，请选择一行记录".t(),
                                            listView: me.view ,
                                            listeners: {
                                                show: function () {
                                                    var clearCommand = listView._relations[0]._target._commands.items.first(function (p) { return p.meta.command == "SIE.cmd.ClearCondition"; });
                                                    if (clearCommand) {
                                                        var Id = clearCommand.meta.id;
                                                        document.getElementById(Id).style.display = "none";
                                                    }
                                                }
                                            },
                                            callback: function (btn) {
                                                if (btn == "确定".t()) {
                                                    var selection = listView.getSelection();
                                                    if (selection.length > 1 || selection.length <= 0) {
                                                        Ext.Msg.show({
                                                            title: '错误'.t(),
                                                            message: '请选择选择一行'.t()
                                                        });

                                                        return false;
                                                    }
                                                    win.close();
                                                    form.setData(selection[0]);
                                                    form.getCurrent().dirty = true;
                                                }
                                            },
                                            width: 800,
                                            height: 400,
                                            items: ui,

                                        });
                                        var filter = {
                                            Method: 'RetrunSearch',
                                            Parameters: [barcode]
                                        };
                                        filter = Ext.encode(filter);
                                        if (allResult.length > 0) {
                                            for (var i = 0; i < allResult.length; i++) {
                                                var result = allResult[i];
                                                var store = listView.getData();
                                                var item = listView.addNew();
                                                item.data = result.data;
                                                item.markSaved();
                                                store.add(item);

                                            }

                                        }
                                    }
                                });
                            } else
                            //直接赋值
                            {
                                if (res.Result.data.items.length > 0) {
                                    var data = res.Result.data.items[0];
                                    form.setData(data);
                                } else {
                                    SIE.Msg.showMessage("未找到标签！".t());
                                }
                            }
                            if (ctl && ctl.up() && ctl.up().up())
                                ctl.up().up().setLoading(false);
                        }
                    });
                }
            }
        }
    }],
});