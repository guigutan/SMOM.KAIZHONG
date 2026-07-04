Ext.define('SIE.Web.AbnormalInfo.AbnormalMonitors.Behaviors.ProcessTaskBehavior',
    {
        onViewReady: function (view) {
            var me = this;
            me.adjustLayout(view);
            me.layoutExtentionView(view);
        },
        layoutExtentionView: function (view) {
            var me = this;
            var current = view.getCurrent();
            if (!current.getTypeName()) return;
            // me.EntityTypeConfigView.getView().setData(record);
            SIE.AutoUI.getMeta({
                model: current.getTypeName(),//record.data.ModuleType,// record.getTypeName().split(",")[0] + "Value",
                isDetail: true,
                ignoreCommands: true,
                ignoreQuery: true,
                async: true,
                callback: function (res) {
                    var mainBlock;
                    if (res.mainBlock) mainBlock = res.mainBlock;
                    else mainBlock = res;
                    var detailView = SIE.AutoUI.createDetailView(mainBlock);
                    //扩展视图
                    view.extensionView = detailView;
                    me.setEntityTypeConfigViewData(view, current);
                    control = {
                        padding: "10",
                        xtype: 'fieldset',
                        title: current.getExtName(),
                        defaultType: 'textfield',
                        defaults: {
                            anchor: '100%',
                        },
                        items: detailView.getControl()
                    };
                    var panel = Ext.getCmp("AbnormalMonitorTaskExtension");
                    panel.setHeight("");
                    panel.add(control);

                    var mainControl = Ext.getCmp("AbnormalMainControl");
                    mainControl.setHeight("");

                    //var secondPanel = Ext.getCmp("AbnormalSecondPanel");
                    //secondPanel.setHeight("");
                }
            });
        },
        adjustLayout: function (view) {
            //调整布局高度
            var areaFileds = view.getControl().items.items;
            if (areaFileds && areaFileds.length > 0) {
                var width = areaFileds[0].MaxWidth;
                areaFileds = areaFileds.where(x => x.xtype == "textareafield");
                areaFileds.forEach(function (filed) {
                    filed.setMaxWidth(880);
                    filed.setHeight(100);
                });
            }
        },
        /**
         * 扩展视图-setData
         * @param {any} mainView
         * @param {any} record
         */
        setEntityTypeConfigViewData: function (mainView, record) {
            var me = this;
            var view = mainView.extensionView;
            var model = view.model;
            configValue = record.getValue();
            if (Ext.isString(configValue) && configValue != "") {
                try {
                    configValue = Ext.JSON.decode(configValue);
                } catch (exp) {
                    SIE.Msg.showError('配置信息JSON格式错误,生成默认配置！');
                }
            }
            var entity = new Ext.create(model, configValue);
            entity.mon(entity, 'propertyChanged', me.onEditPropertyChanged, mainView);
            if (configValue) {
                view.setData(entity);
            }
            else {
                view.setData(entity);
                view._setDefaultValue(entity);
            }
            entity.fireEvent('onpropertySet', {
                entity: entity
            });
            entity.markSaved();
        },
        /**
         * 配置值属性变更事件
         * @param {any} e
         */
        onEditPropertyChanged: function (e) {
            var mainView = this;
            var entity = e.entity;
            var record = mainView.getCurrent();
            if (entity.isDirty())//因record设置值不会变脏数据，所以要手动设置成脏
                record.dirty = true;
            record.setValue(Ext.JSON.encode(entity.data));//每次更改数据都需设置主实体，否则保存数据不一致
            entity.markSaved();
        },
    });