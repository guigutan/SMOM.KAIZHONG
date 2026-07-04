SIE.defineCommand('SIE.Web.MES.PackRule.Commands.QRCodeRuleSaveCommands', function () {
    return {
        extend: 'SIE.cmd.Save',
        meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },

        /**
         * 执行保存操作 - 主入口
         */
        execute: function (view, source) {
            var me = this;

            // 先调用父类的execute方法，确保框架基础逻辑正常执行
            me.callParent(arguments);
        },

        /**
         * 保存前的验证 - 重写此方法
         */
        onSaving: function (view) {
            var me = this;

            // 获取所有变更数据记录（包括新增、修改、删除）
            var allChangedRecords = me.getAllChangedRecords(view);
            if (allChangedRecords.totalCount === 0) {
                Ext.Msg.alert("提示", "没有检测到变更数据");
                return false;
            }

            // 验证所有脏数据（只验证新增和修改的，删除的不需要验证数据内容）
            if (allChangedRecords.dirtyRecords.length > 0) {
                var isValid = me.validateAllRecords(allChangedRecords.dirtyRecords, view);
                if (!isValid) {
                    return false;
                }
            }

            return true;
        },

        /**
         * 获取所有变更记录（包括新增、修改、删除）
         */
        getAllChangedRecords: function (view) {
            var me = this;
            var dirtyRecords = [];
            var removedRecords = [];
            var newRecords = [];
            var updatedRecords = [];

            // 获取store
            var store = me.getStoreFromView(view);

            if (store) {
                // 获取脏记录（新增和修改的）
                if (store.getModifiedRecords) {
                    dirtyRecords = store.getModifiedRecords();
                } else if (store.getUpdatedRecords) {
                    dirtyRecords = store.getUpdatedRecords();
                }

                // 获取删除的记录
                if (store.getRemovedRecords) {
                    removedRecords = store.getRemovedRecords();
                } else if (store.removed) {
                    removedRecords = store.removed;
                }

                // 如果没有上述方法，尝试其他方式
                if (dirtyRecords.length === 0 && removedRecords.length === 0) {
                    // 方法1：检查store的modified属性
                    if (store.modified && Ext.isObject(store.modified)) {
                        dirtyRecords = Ext.Object.getValues(store.modified);
                    }
                    // 方法2：遍历所有记录
                    else if (store.getRange) {
                        var allRecords = store.getRange();
                        dirtyRecords = allRecords.filter(function (record) {
                            return record.dirty === true ||
                                (record.isDirty && record.isDirty()) ||
                                record.phantom === true; // 新增记录
                        });
                    }
                }

                // 分离新增记录和更新记录（可选，用于更详细的处理）
                if (dirtyRecords.length > 0) {
                    newRecords = dirtyRecords.filter(function (record) {
                        return record.phantom === true;
                    });
                    updatedRecords = dirtyRecords.filter(function (record) {
                        return record.phantom !== true;
                    });
                }
            } else {
                // 如果还是获取不到store，回退到只验证当前记录
                console.warn("无法获取store，将只验证当前记录");
                var current = view.getCurrent();
                if (current && (current.dirty || current.isDirty())) {
                    dirtyRecords = [current];
                }
            }

            return {
                dirtyRecords: dirtyRecords,
                removedRecords: removedRecords,
                newRecords: newRecords,
                updatedRecords: updatedRecords,
                totalCount: dirtyRecords.length + removedRecords.length
            };
        },

        /**
         * 从view获取store
         */
        getStoreFromView: function (view) {
            var store = null;

            // 方式1：通过view的data属性获取store
            store = view.data;
            if (store) return store;

            // 方式2：通过getData方法获取store
            if (view.getData) {
                store = view.getData();
                if (store) return store;
            }

            // 方式3：通过grid获取store（如果是网格视图）
            if (view.grid) {
                store = view.grid.getStore();
                if (store) return store;
            }

            // 方式4：通过关联的cmd获取store
            if (view.associateCmd && view.associateCmd.view) {
                var associateView = view.associateCmd.view;
                store = associateView.data || (associateView.getData && associateView.getData());
                if (store) return store;
            }

            // 方式5：尝试从view的其他属性获取
            if (view.store) {
                store = view.store;
            } else if (view.getStore) {
                store = view.getStore();
            }

            return store;
        },

        /**
         * 获取记录在store中的行号
         */
        getRecordRowNumber: function (record, view) {
            var store = this.getStoreFromView(view);

            if (store && store.indexOf) {
                var index = store.indexOf(record);
                // 行号通常从1开始显示，所以index+1
                return index >= 0 ? (index + 1) : null;
            }

            return null;
        },

        /**
         * 验证所有记录
         */
        validateAllRecords: function (records, view) {
            var me = this;
            var allValid = true;
            var errorMessages = [];

            Ext.Array.forEach(records, function (record, index) {
                var rowNumber = me.getRecordRowNumber(record, view) || (index + 1);
                var result = me.validateSingleRecord(record, rowNumber);
                if (!result.isValid) {
                    allValid = false;
                    var recordInfo = '第' + rowNumber + '行';

                    // 添加记录标识信息
                    var ruleName = record.get('RuleName') || record.get('Name');
                    if (ruleName) {
                        recordInfo += ' (' + ruleName + ')';
                    }

                    errorMessages.push(recordInfo + ':\n  - ' + result.errors.join('\n  - '));
                }
            });

            if (!allValid) {
                Ext.Msg.alert('数据验证失败',
                    '以下记录存在验证错误：\n\n' + errorMessages.join('\n\n') +
                    '\n\n请修正后重新保存。');
            }

            return allValid;
        },

        /**
         * 验证单条记录
         */
        validateSingleRecord: function (record, rowNumber) {
            var me = this;
            var errors = [];

            // 验证二维码总位数 - 正整数
            var totalDigit = record.get('TotalDigit');
            if (!me.isPositiveInteger(totalDigit)) {
                errors.push("二维码总位数必须为正整数");
            }

            // 验证序列号开始位数 - 正整数
            var serialStart = record.get('SerialNumberStartDigit');
            if (!me.isPositiveInteger(serialStart)) {
                errors.push("序列号开始位数必须为正整数");
            }

            // 验证序列号结束位数 - 正整数
            var serialEnd = record.get('SerialNumberEndDigit');
            if (!me.isPositiveInteger(serialEnd)) {
                errors.push("序列号结束位数必须为正整数");
            }

            // 验证序列号开始位 ≤ 结束位
            if (me.isPositiveInteger(serialStart) && me.isPositiveInteger(serialEnd)) {
                var startNum = parseInt(serialStart);
                var endNum = parseInt(serialEnd);
                if (startNum > endNum) {
                    errors.push("序列号开始位数不能大于结束位数");
                }

                // 验证序列号范围不超过总位数
                if (me.isPositiveInteger(totalDigit)) {
                    var totalNum = parseInt(totalDigit);
                    if (endNum > totalNum) {
                        errors.push("序列号结束位数不能超过二维码总位数");
                    }
                }
            }

            // 验证客户零件号开始位数
            var customPnStart = record.get('CustomPnStartDigit');
            if (customPnStart && customPnStart.trim() !== '') {
                if (!me.isPositiveInteger(customPnStart)) {
                    errors.push("客户零件号开始位数必须为正整数");
                }
            }

            // 验证客户零件号结束位数
            var customPnEnd = record.get('CustomPnEndDigit');
            if (customPnEnd && customPnEnd.trim() !== '') {
                if (!me.isPositiveInteger(customPnEnd)) {
                    errors.push("客户零件号结束位数必须为正整数");
                }
            }

            // 验证客户零件号开始位 ≤ 结束位
            if (customPnStart && customPnStart.trim() !== '' &&
                customPnEnd && customPnEnd.trim() !== '' &&
                me.isPositiveInteger(customPnStart) && me.isPositiveInteger(customPnEnd)) {
                var customStartNum = parseInt(customPnStart);
                var customEndNum = parseInt(customPnEnd);
                if (customStartNum > customEndNum) {
                    errors.push("客户零件号开始位数不能大于结束位数");
                }

                // 验证客户零件号范围不超过总位数
                if (me.isPositiveInteger(totalDigit)) {
                    var totalNum = parseInt(totalDigit);
                    if (customEndNum > totalNum) {
                        errors.push("客户零件号结束位数不能超过二维码总位数");
                    }
                }
            }

            // 验证客户版本号开始位数
            var versionStart = record.get('VersionNumberStartDigit');
            if (versionStart && versionStart.trim() !== '') {
                if (!me.isPositiveInteger(versionStart)) {
                    errors.push("客户版本号开始位数必须为正整数");
                }
            }

            // 验证客户版本号结束位数
            var versionEnd = record.get('VersionNumberEndDigit');
            if (versionEnd && versionEnd.trim() !== '') {
                if (!me.isPositiveInteger(versionEnd)) {
                    errors.push("客户版本号结束位数必须为正整数");
                }
            }

            // 验证客户版本号开始位 ≤ 结束位
            if (versionStart && versionStart.trim() !== '' &&
                versionEnd && versionEnd.trim() !== '' &&
                me.isPositiveInteger(versionStart) && me.isPositiveInteger(versionEnd)) {
                var versionStartNum = parseInt(versionStart);
                var versionEndNum = parseInt(versionEnd);
                if (versionStartNum > versionEndNum) {
                    errors.push("客户版本号开始位数不能大于结束位数");
                }

                // 验证客户版本号范围不超过总位数
                if (me.isPositiveInteger(totalDigit)) {
                    var totalNum = parseInt(totalDigit);
                    if (versionEndNum > totalNum) {
                        errors.push("客户版本号结束位数不能超过二维码总位数");
                    }
                }
            }

            // 新增：验证序列号范围长度不超过总位数
            if (me.isPositiveInteger(serialStart) && me.isPositiveInteger(serialEnd) && me.isPositiveInteger(totalDigit)) {
                var startNum = parseInt(serialStart);
                var endNum = parseInt(serialEnd);
                var totalNum = parseInt(totalDigit);
                if ((endNum - startNum) > totalNum) {
                    errors.push("序列号开始位数到序列号结束位数总共【" + (endNum - startNum) + "】位，不允许超过二维码总位数【" + totalNum + "】");
                }
            }

            // 新增：验证所有字段总位数不超过二维码总位数
            if (me.isPositiveInteger(totalDigit)) {
                var totalNum = parseInt(totalDigit);
                var totalUsedDigits = 0;

                // 计算序列号使用的位数
                if (me.isPositiveInteger(serialStart) && me.isPositiveInteger(serialEnd)) {
                    totalUsedDigits += (parseInt(serialEnd) - parseInt(serialStart));
                }

                // 计算客户零件号使用的位数
                if (customPnStart && customPnStart.trim() !== '' && customPnEnd && customPnEnd.trim() !== '' &&
                    me.isPositiveInteger(customPnStart) && me.isPositiveInteger(customPnEnd)) {
                    totalUsedDigits += (parseInt(customPnEnd) - parseInt(customPnStart));
                }

                // 计算客户版本号使用的位数
                if (versionStart && versionStart.trim() !== '' && versionEnd && versionEnd.trim() !== '' &&
                    me.isPositiveInteger(versionStart) && me.isPositiveInteger(versionEnd)) {
                    totalUsedDigits += (parseInt(versionEnd) - parseInt(versionStart));
                }

                if (totalUsedDigits > totalNum) {
                    errors.push("客户零件号+客户版本号+序列号总共【" + totalUsedDigits + "】位，不允许超过二维码总位数【" + totalNum + "】");
                }
            }

            return {
                isValid: errors.length === 0,
                errors: errors
            };
        },

        /**
         * 验证是否为正整数
         */
        isPositiveInteger: function (value) {
            if (!value || value.trim() === '') {
                return false;
            }
            var num = parseInt(value);
            return !isNaN(num) && num > 0 && num.toString() === value.trim();
        },

        /**
         * 保存成功后的处理
         */
        onSaved: function (view, res) {
            // 调用父类处理
            this.callParent(arguments);
        },

        /**
         * 判断是否可以执行保存
         */
        canExecute: function (view) {
            if (view.isDetailView) {
                // 获取所有变更记录
                var changedRecords = this.getAllChangedRecords(view);
                if (changedRecords.totalCount > 0) {
                    return true;
                }

                // 回退到原有逻辑
                var current = view.getCurrent();
                return current ? current.isDirty() : false;
            }
            return this.callParent(arguments);
        }
    };
});