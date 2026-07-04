SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.HandoverConfirmCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "维修交机确认", group: "edit", iconCls: "icon-EnableUsers icon-blue" },
    selectedItems: [],
    canExecute: function (listview) {
        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length === 0)
            return false;

        for (i = 0; i < this.selectedItems.length; i++) {
            var item = this.selectedItems[i];
            if (item.data.RepairState != 3)
                return false;
        }
        return true;
    },
    onEntityPropertyChanged: function (e) {
        if (e.property == 'HandoverConfirmResult') {
            var tabPanel = this.detailView._view._children[0].getControl().ownerCt.ownerCt;
            var tpmTab = this.detailView._view._children[0]._control.ownerLayout.config.owner.tab;
            var abnormalTab = this.detailView._view._children[1]._control.ownerLayout.config.owner.tab;
            if (e.value == 0) {
                tpmTab.show();
                abnormalTab.hide();
                tabPanel.setActiveTab(0);
            }
            else if (e.value == 1) {
                tpmTab.hide();
                abnormalTab.show();
                tabPanel.setActiveTab(1);
            }
            else {
                
                tpmTab.show();
                abnormalTab.show();
                tabPanel.setActiveTab(0);
            }
        }
    },
    onSaving: function (view) {
        view.getCurrent().dirty = true;
        return this.callParent(arguments);
    },
    onSaved: function (view, res) {
        var me = this;
        var editEntity = this.view.getCurrent();

        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: true,
            model: this.view.model,
            viewGroup: "HandoverConfirmViewGroup",
            callback: function (meta) {
                meta.token = me.view.token;
                me.viewMeta = meta;
                var detailView = SIE.AutoUI.generateAggtControl(meta);
                me.detailView = detailView;
                //设置默认值(交机确认人)
                var curId = CRT.Context.GlobalContext.getContext('userInfo').EmployeeId;
                var curName = CRT.Context.GlobalContext.getContext('userInfo').Name;

                //var model = SIE.getModel(me.view.model);
                //var entity = new model();

                editEntity.setHandoverConfirmResult(null);
                editEntity.setHandoverConfirmEmployeeId(curId);
                editEntity.setHandoverConfirmEmployeeId_Display(curName);

                var entity = editEntity;

                detailView._view._setDefaultValue(entity);
                detailView._view.setData(entity);

                detailView._view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, me);

                var win = SIE.Window.show({
                    title: '交机确认'.t(),
                    width: '50%',
                    height: '80%',
                    items: detailView.getControl(),
                    callback: function (btn) {
                        var retFlag = false;
                        if (btn === "确定".t()) {
                            entity.data.Id = editEntity.data.Id;
                            entity.data.RepairType = editEntity.data.RepairType;
                            entity.data.EquipAccountId = editEntity.data.EquipAccountId;

                            if (entity.data.HandoverConfirmResult != 0 && entity.data.HandoverConfirmResult != 1) {
                                SIE.Msg.showError('确认结果不能为空，请确认!'.t());
                                return false;
                            }
                            var detailArr = [];
                            if (entity.data.HandoverConfirmResult == 0) {
                                var tpmList = detailView._view._children[0].getData().getData().items;
                                Ext.each(tpmList, function (item) {
                                    item.data.EquipRepairBillId = editEntity.data.Id;
                                    detailArr.push(item.data);
                                });
                            }

                            if (entity.data.HandoverConfirmResult == 1) {
                                var abnormalInfo = detailView._view._children[1].getData();
                                if (abnormalInfo.data.HandoverConfirmAbnormal != 0 && abnormalInfo.data.HandoverConfirmAbnormal != 1) {
                                    SIE.Msg.showError('异常情况为必填项，请选择!'.t());
                                    return false;
                                }

                                entity.data.HandoverConfirmAbnormal = abnormalInfo.data.HandoverConfirmAbnormal;
                                entity.data.HandoverDeviceAbnormalId = abnormalInfo.data.HandoverDeviceAbnormalId;
                                entity.data.HandoverDeviceAbnormalRem = abnormalInfo.data.HandoverDeviceAbnormalRem;
                                entity.data.HandoverAttachment = abnormalInfo.data.HandoverAttachment;
                            }

                            var indata = {};
                            var data = { repairBill: entity.data, detailList: detailArr };

                            //indata = Ext.encode(data);
                            indata = data;
                            view.execute({
                                data: indata,
                                success: function (res) {
                                    retFlag = true;
                                    SIE.Msg.showMessage('确认完成'.t());
                                    view.reloadData();
                                    win.close();
                                },
                                error: function (res) {
                                    SIE.Msg.showError(res.Message);
                                }
                            });

                            if (retFlag)
                                return true;
                            else
                                return false;
                        }
                    }
                });
            }
        });
    }
});