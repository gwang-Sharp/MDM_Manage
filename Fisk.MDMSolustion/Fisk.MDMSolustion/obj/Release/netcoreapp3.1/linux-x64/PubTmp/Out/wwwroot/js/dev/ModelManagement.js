var ModelVue = new Vue({
    el: '#Model',
    data: {
        SQLCommonFiled: [{
            Value: 'name', Label: '模板名称'
        }, {
            Value: 'remark', Label: '备注'
        }, {
            Value: 'logRetentionDays', Label: '日志天数'
        }, {
            Value: 'creater', Label: '创建人'
        }, {
            Value: 'createTime', Label: '创建时间'
        }
        ],
        searchName: "",
        TableLoading: false,
        ModeltableData: [],
        Pagination: {
            limit: 10,
            page: 1,
            pageSizes: [10, 20, 30],
            total: 0
        },
        dialogVisible: false,
        dialogTitle: "添加实体",
        DialogType: 'Add',
        ModelFormInfo: {
            name: '',
            remark: "",
            logRetentionDays: ""
        },
        ModelFromrules: {
            name: [{ required: true, message: '请输入模型名称', trigger: 'change' }],
            //remark: [{ required: true, message: '请输入备注', trigger: 'change' }],
            logRetentionDays:
                [
                    { required: true, message: '请输入日志保留', trigger: 'change' },
                    { type: 'number', message: '必须为数字值' },
                    {
                        validator: function (rule, value, callback) {
                            if (value > 100) {
                                callback(new Error('不能超过100'));
                            }
                            else if (value < 0) {
                                callback(new Error('不能小于0'));
                            }
                            callback();
                        }, trigger: 'blur'
                    }
                ]
        }

    },
    created: function () {
        this.SelectModelInfo();
    },
    mounted: function () {

    },
    methods: {
        SelectModelInfo() {
            let obj = {
                page: this.Pagination.page,
                limit: this.Pagination.limit,
                where: this.searchName

            };
            this.TableLoading = true;
            $.post('/MasterDataManage/SearchModel', obj, res => {
                if (res.success) {
                    this.ModeltableData = res.data;
                    this.Pagination.total = res.total
                }
                this.TableLoading = false;
            })
        },  //获取数据
        CurrentChange(val) {
            this.Pagination.page = val;
            this.SelectModelInfo();
        },  //页码切换事件
        AddModel() {
            this.DialogType = 'Add';
            this.dialogVisible = true;
            this.dialogTitle = "添加模型";
        },      //添加模型
        EditModel(row) {
            this.DialogType = 'Edit';
            this.dialogVisible = true;
            this.dialogTitle = "编辑模型";
            this.$nextTick(() => {
                this.ModelFormInfo = Object.assign({}, row);
            });
        },   // 编辑模型
        headerClass: function () {
            return 'background:#eef1f6;'
        }, //表格表头颜色
        DeleteModel(row) {
            this.$confirm('此操作将永久删除该模型以及相关实体信息，是否继续？', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning',
                callback: (action, instance) => {
                    if (action === 'confirm') {
                        $.post('/MasterDataManage/DeleteModel', { 'id': row.id }, res => {
                            if (res.success) {
                                this.$message.success(res.message);
                                this.SelectModelInfo();
                                this.dialogVisible = false;
                            } else
                                this.$message.error(res.message);
                        });
                    }
                }
            });
        }, // 删除模型
        DialogClose() {
            this.$refs.ModelForm.resetFields();
        },   //添加或编辑弹框编辑事件
        resetForm() {
            this.dialogVisible = false;
            this.DialogClose();
        },    //重置表单
        SubmitInserModel() {
            this.$refs.ModelForm.validate((valid) => {
                if (valid) {
                    let url = this.DialogType == 'Add' ? '/MasterDataManage/InserModel' : '/MasterDataManage/UpdateModel';
                    $.post(url, this.ModelFormInfo, res => {
                        if (res.success) {
                            this.$message.success(res.message);
                            this.SelectModelInfo();
                            this.dialogVisible = false;
                        } else
                            this.$message.error(res.message);
                    }, 'json');
                } else {
                    //console.log('error submit!!');
                    return false;
                }
            });
        }  //提交数据
    },
    watch: {
        searchName(val) {
            this.Pagination.page = 1;
            this.SelectModelInfo();
        }
    }

});