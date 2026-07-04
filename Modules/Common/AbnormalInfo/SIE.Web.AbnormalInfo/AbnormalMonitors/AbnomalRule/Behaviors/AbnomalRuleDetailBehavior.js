Ext.define('SIE.Web.AbnormalInfo.AnomalyMonitors.AbnomalRule.Behaviors.AbnomalRuleDetailBehavior',
    {
        onDataLoaded: function (view) {
            var me = this;
            var store = view.getData();
            var mainView = CRT.Context.PageContext.getLogicalView();
            var curent = mainView.getCurrent();
            var ctl = mainView.getController();
            mainView.mon(curent, 'propertyChanged', me.onPropertyChanged, mainView);
            view.mon(store, 'propertyChanged', ctl.onPropertyChanged, view);
            me.indicatorViewAssign(mainView);
            //SQL语句赋值
            var sqlFiled = Ext.getCmp("whereConditionSqlFiled");
            if (sqlFiled) sqlFiled.setValue(curent.getDisPlaySelect());

            me.hiddenWhereConditionView(mainView, curent.getIsSQL());
            me.settActiveTab(mainView);
        },
        indicatorViewAssign: function (mainView) {
            var sourceView = mainView.getChildren()[1].indicatorOpraView;
            var ctl = mainView.getController();
            ctl.viewDataCopy(sourceView, mainView);
        },
        /**
         * 属性变更处理
         * @param {any} 
         */
        onPropertyChanged: function (e, opt) {
            var me = this;
            var entity = e.entity;
            var mainView = CRT.Context.PageContext.getLogicalView();
            if (e.property === "IsSQL") {
                var whereConditionView = mainView.getChildren()[0];
                if (whereConditionView) whereConditionView.getControl().setHidden(e.value);
                var sqlFiled = Ext.getCmp("whereConditionSqlFiled");
                if (!sqlFiled.getValue()) mainView.getController().generalSqlByDataSource(mainView, sqlFiled);
            }
        },
        /**
         * 
         * @param {any} mainView
         * @param {any} isHide 
         */
        hiddenWhereConditionView: function (mainView,isHide) {
            var whereConditionView = mainView.getChildren()[0];
            if (whereConditionView) whereConditionView.getControl().setHidden(isHide);
        },
        /**
         * 激活tab页
         * @param {any} mainView
         */
        settActiveTab: function (mainView) {
            var tabPanel = mainView.getChildren()[0].getControl().up("tabpanel");
            if (tabPanel) {
                var atTab = tabPanel.getActiveTab();
                // 获取所有的Tab页
                var tabs = tabPanel.items.items;

                // 循环遍历所有的Tab页，并激活它们
                Ext.Array.each(tabs, function (tab) {
                    tabPanel.setActiveTab(tab);
                });
                tabPanel.setActiveTab(atTab);
            }
        },
    });