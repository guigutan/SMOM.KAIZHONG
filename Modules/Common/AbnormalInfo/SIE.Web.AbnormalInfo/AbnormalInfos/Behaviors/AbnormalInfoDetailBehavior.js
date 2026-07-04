Ext.define('SIE.Web.AbnormalInfo.AbnormalInfos.Behaviors.AbnormalInfoDetailBehavior',
    {
        memoList: ['ReasonAnalysis', 'Measure', 'Experience'],  //原因分析、改善对策、 经验总结
        wordsKeyName: '_Words',
        beforeCreate: function (meta) {
            if (!meta || !meta.formConfig) return;
            var items = meta.formConfig.items;
            if (Ext.isEmpty(items)) return;
            var indexList = [];
            this.memoList.forEach(function (memo) {
                var memoIndex = items.findIndex(function (p) { return p.name == memo; });
                if (memoIndex > -1) {
                    indexList.push(memoIndex);
                }
            });

            //添加字数统计控制
            if (Ext.isEmpty(indexList)) return;
            for (var i = indexList.length - 1; i >= 0; i--) {

                index = indexList[i];
                var config = items[index];
                if (config.maxLength) {
                    var labelConfig = {
                        bind: "{p." + config.name + this.wordsKeyName + "}",
                        colspan: config.colspan,
                        fieldLabel: "",
                        //labelAlign: "right",
                        //labelWidth: 108,
                        maxLength: config.maxLength,
                        maxWidth: config.maxWidth,
                        minWidth: config.minWidth,
                        name: config.name + this.wordsKeyName,
                        preventScrollbars: false,
                        readOnlyCls: config.readOnlyCls,
                        width: config.width,
                        xtype: "displayfield",
                        //renderTpl: '{%this.renderContent(out,values)%}/' + config.maxLength,
                        style: {
                            'text-align': 'right'
                        }
                    };
                    items.splice(index + 1, 0, labelConfig);
                }
            }
        },
        /**
        * view生命周期函数--view聚合后
        * @param {*} view 生成的view
        */
        onViewReady: function (view) {
            var me = this;
            this.view = view;
            var entity = view.getCurrent();
            var params = CRT.Context.PageContext.getParams();
            if (params && entity) {
                entity.data.JoinDefectCodes = params.JoinDefectCodes;
                entity.data.JoinDefectCodeDescriptions = params.JoinDefectCodeDescriptions;
            }
            //大文本字符统计初始化
            if (entity) {
                var configs = view.formConfig.items;
                this.memoList.forEach(function (memoName) {
                    var config = configs.first(function (p) { return p.name == memoName });
                    if (config && config.maxLength) {
                        entity.set(memoName + me.wordsKeyName, entity.data[memoName].length + "/" + config.maxLength);
                    }
                });
                view.mon(entity, "propertyChanged", me.onPropertyChanged, me);
                entity.markSaved();
            }
        },

        /**
         * 属性变更处理
         * @param {any} 
         */
        onPropertyChanged: function (e) {
            var me = this;
            var entity = e.entity;
            if (e.property.length > 0) {
                me.memoList.forEach(function (memoName) {
                    if (e.property === memoName) {
                        var lastText = entity.data[memoName + me.wordsKeyName];
                        if (lastText) {
                            var wordTexts = lastText.split('/');
                            if (wordTexts && wordTexts.length == 2) {
                                entity.set(memoName + me.wordsKeyName, e.value.length + "/" + wordTexts[1]);
                            }
                        }
                    }
                });
            }
        }
    });