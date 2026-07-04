SIE.defineCommand('SIE.Web.Items.Items.Commands.ItemPropertyValueAddCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        if (view._parent && view._parent.getCurrent() && view._parent.getCurrent().data.CreateBy > 0)
            return true;

        return false;
    },

    getEditEntity: function () {
        var model = SIE.getModel('SIE.Web.Items.Items.ViewModels.ItemPropertyValueViewModel');
        var entity = new model();

        //entity.setTextValue("12345");
        entity.token = this.view.token;
        return entity;
    },
    /**
     * virtual 方法 弹出页面
     * @param editEntity 绑定实体
     */
    showView: function (editEntity) {
        var me = this;
        var mainView = me.view;
        if (!this.viewMeta) {
            SIE.AutoUI.getMeta({
                async: false,
                ignoreCommands: false,
                isDetail: true,
                ignoreQuery: true,
                viewGroup: "DetailsView",
                token: this.view.token,
                module: mainView.module,
                model: "SIE.Web.Items.Items.ViewModels.ItemPropertyValueViewModel",
                callback: function (res) {
                    var mainBlock;
                    if (res.mainBlock)
                        mainBlock = res.mainBlock;
                    else
                        mainBlock = res;

                    var detailView = SIE.AutoUI.createDetailView(mainBlock);
                    detailView._setDefaultValue(editEntity);
                    detailView.setData(editEntity);
                    detailView.mainView = mainView;
                    //var ui = detailView.getControl();
                    var ui = detailView.getControl();
                    var win = SIE.Window.show({
                        title: me.getEditViewTitle(editEntity),
                        width: 400,
                        height: 200,
                        items: ui,
                        id: "ItemPropertyValueViewModel001",
                        callback: function (btn) {
                            if (btn === '确定'.t()) {
                                me.save(editEntity, win);
                                return false;
                            }
                        }
                    });
                },
            });
        }

    },
    save: function (editEntity, win) {
        /// <summary>
        /// 保存数据。
        var me = this;
        if (editEntity) {
            type = editEntity.getPropertyType();
            value = type == 0 ? editEntity.data.CatalogValue : type == 1 ? editEntity.getTextValue() : type == 2 ? editEntity.getNumberValue() : null;
            if (value == null || value === "") {
                SIE.Msg.showInstantMessage('属性值不能为空'.t());
                return;
            }
            me._ownerView.execute({
                data: {
                    DefinitionId: editEntity.getDefinitionId(),
                    Value: value,
                    ItemId: me._ownerView._parent._current.data.Id,
                    PropertyGroup: editEntity.getPropertyGroup()
                },
                success: function (res) {
                    //win.close();  //关闭模态窗口
                    me._ownerView.loadChildData(true); //重载视图数据
                    me._ownerView._parent.reloadData();
                    win.close();
                }
            }, me._ownerView);
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    }
});