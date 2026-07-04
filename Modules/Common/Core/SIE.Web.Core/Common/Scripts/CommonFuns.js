Ext.define('SIE.Web.Core.CommonFuns',
    {
        singleton: true,
        /**
        * 列表指定列显示ToolTips
        * @param {} view 视图
        * @param {} delegate 过滤
        * @returns {} 
        */
        showToolTips: function (view, delegate) {
            var tip = Ext.create('Ext.tip.ToolTip',
                {
                    // The overall target element.
                    target: view.getControl(),
                    // Each grid row causes its own separate show and hide.
                    delegate: delegate,
                    // Moving within the row should not hide the tip.
                    trackMouse: true,
                    // Render immediately so that tip.body can be referenced prior to the first show.
                    renderTo: Ext.getBody(),
                    listeners: {
                        // Change content dynamically depending on which element triggered the show.
                        beforeshow: function updateTipBody(tip) {
                            tip.update(Ext.String.htmlEncode(tip.triggerElement.innerText.replace("\r\n", "")));
                        }
                    }
                });
        },

        /**
         * 数据已保存到服务器,前端标记已保存
         * @param {} entity 
         * @returns {} 
         */
        markSaved: function (entity) {
            if (entity.data.hasOwnProperty("PersistenceStatus") && entity.data.PersistenceStatus == 2)
                entity.data.PersistenceStatus = 0;
            entity.markSaved();
        },

        /**
         * 视图当前数据已保存到服务器,前端标记已保存
         * @param {} view 
         * @returns {} 
         */
        markViewSaved: function (view) {
            var data = view.getCurrent();
            if (data) {
                data.markSaved();
            }
        },

        /**
        * 视图所有数据已保存到服务器,前端标记已保存
        * @param {} view 
        * @returns {} 
        */
        markViewAllDataSaved: function (view) {
            var viewDatas = view.getData();
            if (viewDatas) {
                var datas = viewDatas.getData().items;
                datas.forEach(function (data) {
                    if (data) data.markSaved();
                });
            }
        },

        /**
         * Listview界面重新加载，用于带查询块的ListView
         * @param {} view 
         * @returns {} 
         */
        mainReloadData: function (view) {
            if (view.isListView) {
                //当ReloadData之前未查询过，则执行查询块的查询方法。以免不使用查询实体来查询
                if (!view._lastDataArgs) {
                    var conditionView = view.getConditionView();
                    if (conditionView) {
                        conditionView._commands.items.first(function (p) { return p.meta.command === "SIE.cmd.ExecuteQuery" }).execute(conditionView);
                        return;
                    }
                }
                view.reloadData();
            }
        },

        /**
        * 获取view的控制器
        * @param {SIE.view.View} view实例
        */
        getViewController: function (view, controller) {
            var ctl = null;
            if (view) {
                ctl = view.getController();
                if (!ctl) {
                    ctl = new controller();
                    view.setController(ctl);
                }
                var childrens = view.getChildren();
                if (childrens && childrens.length > 0) {
                    for (var i = 0, length = childrens.length; i < length; i++) {
                        var children = childrens[i];
                        if (children.isView) {
                            children.setController(ctl);
                        }
                    }
                }
            }
            return ctl;
        },


        /**
         * 根据实体和viewGroup，生成Tab的id
         * @param {} entity 实体
         * @param {} viewGroup 视图类型
         * @returns {} 
         */
        getTabid: function (entity, viewGroup) {
            var entityId = entity.entityName + '-' + viewGroup + '-' + entity.getId();
            return ('tab_' + entityId).replace(/[.|,]/g, '');
        },    

        /**
         * 四舍五入
         * @param {any} num 数值
         * @param {any} fractiondigits  精度
         */
        round: function (num, fractiondigits) {
            if (isNaN(num))
                return "";
            var powDigit = Math.pow(10, fractiondigits);
            return Math.round(num * powDigit) / powDigit;
        },

        /**
         * 弹出打印预览
         * @param {any} view 选择模板的视图
         * @param {any} rst 打印命令返回的数据
         */
        ShowPrintPreview: function (res, type = 1) {
            var rstPrint = res.Result;
            var printCmpt = new SIE.Web.Common.Prints.Report.WebReportComponents({ ReportType: rstPrint.Type, ReportData: { path: rstPrint.Url, content: rstPrint.Url } });
            var cfg = printCmpt.getExtTarget();
            if (cfg && cfg.printCallback) {
                cfg.printCallback(printCmpt);
            }
            else {
                var param = printCmpt.getPrintParams();
                if (!printCmpt.hasError()) {
                    var printUrl = printCmpt.getPrintUrl();
                    if (!printCmpt.hasError())
                        CRT.Workbench.showPageDialog({ id: 'Label_rpt', text: "打印".t(), method: 'POST', url: printUrl, params: param });
                }
            }
        },
        /**
         * 设置打印的模板缓存
         * @param {any} entityData 实体的data
         * @param {any} model 当前view的模型
         */
        setPrintCache: function (entityData, model) {
            var tempData = {
                TemplateId: entityData.LabelTemplateId,
                TemplateName: entityData.LabelTemplateId_Display,
            };//缓存选择的模板
            localStorage.setItem(model, JSON.stringify(tempData));
        },
        /**
         * 读取打印模板缓存
         * @param {any} entity 实体
         * @param {any} model 当前view的模型
         */
        getPrintCache: function (entity, model) {
            var templateData = localStorage.getItem(model);
            if (templateData && templateData != "{}") {
                tempData = JSON.parse(templateData);
                if (tempData.TemplateId) {
                    temId = tempData.TemplateId;
                    entity.setLabelTemplateId(tempData.TemplateId);
                    entity.setLabelTemplateId_Display(tempData.TemplateName);
                    if (entity.data.LabelTemplateName) {
                        entity.setLabelTemplateName(tempData.TemplateName);
                    }
                }
            }
        },

        /**
         * 设置单据打印的模板缓存
         * @param {any} entityData 实体的data
         * @param {any} model 当前view的模型
         */
        setBillPrintCache: function (entityData, model) {
            var tempData = {
                TemplateId: entityData.BillTemplateId,
                TemplateName: entityData.BillTemplateId_Display,
            };//缓存选择的模板
            localStorage.setItem(model, JSON.stringify(tempData));
        },

        /**
         * 读取单据打印模板缓存
         * @param {any} entity 实体
         * @param {any} model 当前view的模型
         */
        getBillPrintCache: function (entity, model) {
            var templateData = localStorage.getItem(model);
            if (templateData && templateData != "{}") {
                tempData = JSON.parse(templateData);
                if (tempData.TemplateId) {
                    temId = tempData.TemplateId;
                    entity.setBillTemplateId(tempData.TemplateId);
                    entity.setBillTemplateId_Display(tempData.TemplateName);
                }
            }
        }
    }
);