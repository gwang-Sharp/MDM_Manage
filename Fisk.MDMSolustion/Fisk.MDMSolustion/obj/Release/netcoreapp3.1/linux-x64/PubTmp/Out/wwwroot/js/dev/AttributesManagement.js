
var ModelVM = new Vue({
    el: "#AttributesVue",
    data: {
        AttributesTabLoading: false,
        AttributestableData: [],
        ModelSelectOptions: [],
        EntitySelectOptions: [],
        EntitySelectOptions2: [],
        AllEntityOptions: [],
        pagination: {
            currentpage: 1,
            pagesizes: [10, 20, 30, 40],
            pagesize: 10,
            allNum: 0,
        },
        ModelID: '',
        dialogTitle: "添加属性",
        AttributesRuleForm: {
            Id: '',
            Name: '',
            Remark: '',
            Type: '',
            TypeLength: '',
            DataType: '',
            EntityID: '',
            LinkEntityID: '',
            LinkModelID: '',
            StartTrace: false,
            IsDisplay: true,
            DisplayName: '',
            Creater: '',
            CreateTime: '',
            Updater: '',
            UpdateTime: ''
        },
        SelectedRow: '',
        AttributesFromrules: {
            Name: [{
                required: true,
                message: '请输入名称',
                trigger: 'blur'
            },
            {
                validator: function (rule, value, callback) {
                    var containSpecial = RegExp(/[(\——)(\（）)(\……)(\`)(\·)(\；)(\‘’)(\、)(\。)(\，)(\￥)(\ )(\~)(\!)(\@)(\#)(\$)(\%)(\^)(\&)(\*)(\()(\))(\-)(\_)(\+)(\=)(\[)(\])(\{)(\})(\|)(\\)(\;)(\:)(\')(\")(\,)(\.)(\/)(\<)(\>)(\?)(\)]+/g);
                    if (value) {
                        if (/[\u4E00-\u9FA5]/g.test(value)) {
                            callback(new Error('属性不能输入中文'));
                        } else if (/^[0-9]*$/g.test(value)) {
                            callback(new Error('属性不能为数字'));
                        }
                        else if (containSpecial.test(value)) {
                            callback(new Error('属性不能含有特殊字符'));
                        }
                        else {
                            callback();
                        }
                    }
                    callback();
                }, trigger: 'blur'
            }
            ],
            Type: [{
                required: true,
                message: '请选择属性类型',
                trigger: 'blur'
            }],
            DisplayName: [{
                required: true,
                message: '请输入显示名称',
                trigger: 'change'
            }],
            Remark: [{

                required: true,
                message: '请输入说明',
                trigger: 'change'
            }],
            TypeLength: [{
                validator: function (rule, value, callback) {
                    if (value < 50) {
                        callback(new Error('长度至少50'));
                    }
                    else if (/[\u4E00-\u9FA5]/g.test(value)) {
                        callback(new Error('长度非数字'));
                    }
                    else if (isNaN(value)) {
                        callback(new Error('长度非数字'));
                    }
                    else {
                        callback();
                    }
                    callback();
                },
                trigger: 'blur'
            }]
        },
        AttributeTypes: [{
            label: '自由格式',
            value: '自由格式'
        },
        {
            label: '基于域',
            value: '基于域'
        }, {
            label: '文件',
            value: '文件'
        }],
        AttributeDataType: [{
            label: '文本',
            value: '文本'
        },
        {
            label: '数字',
            value: '数字'
        }, {
            label: '小数',
            value: '小数'
        }, {
            label: '时间',
            value: '时间'
        }],
        dialogVisible: false,
        loading: false,
        disabled: false,
        disabled2: false
    },
    created: function () {
        this.InitAttributesTable(0);
        this.InitModelSelectOptions();
        this.InitEntitySelectOptions();
    },
    mounted: function () {
    },
    methods: {
        InitAttributesTable: function (id) {
            let that = this;
            that.AttributesTabLoading = true;
            $.post("/MasterDataManage/AttributesGet", {
                id: id,
                page: that.pagination.currentpage,
                rows: that.pagination.pagesize
            }, RES => {
                that.loading = false;
                //console.log(RES);
                if (RES.success) {
                    if (that.pagination.currentpage > 1 && RES.data.length == 0) {
                        that.pagination.currentpage = 1;
                        that.InitAttributesTable();
                    }
                    else {
                        that.AttributestableData = RES.data;
                        that.pagination.allNum = RES.total;
                    }
                    that.AttributesTabLoading = false;
                }
                else {
                    that.AttributestableData = [];
                    that.pagination.allNum = 0;
                }
            })
        },
        ChangeValue: function (row, column, cellValue, index) {
            if (cellValue == "1") {
                return "是";
            }
            else {
                return "否";
            }
        },
        InitModelSelectOptions: function () {
            let that = this;
            $.post("/system/Attributes_ModelsGet", RES => {
                //console.log(RES);
                if (RES.success) {
                    that.ModelSelectOptions = RES.data;
                }
                else {
                    that.ModelSelectOptions = [];
                }
            })
        },
        InitEntitySelectOptions: function () {
            let that = this;
            $.post("/system/Attributes_EntitysGet", RES => {
                //console.log(RES);
                if (RES.success) {
                    that.EntitySelectOptions = RES.data;
                    that.AllEntityOptions = RES.data;
                }
                else {
                    that.EntitySelectOptions = [];
                }
            })
        },
        headerClass: function () {
            return 'text-align: center;background:#eef1f6;'
        },
        handleSizeChange: function (val) {
            this.pagination.pagesize = val;
            if (this.AttributesRuleForm.EntityID) {
                this.InitAttributesTable(this.AttributesRuleForm.EntityID);
            }
            else {
                this.InitAttributesTable(0);
            }
        },
        handleCurrentChange: function (val) {
            this.pagination.currentpage = val;
            if (this.AttributesRuleForm.EntityID) {
                this.InitAttributesTable(this.AttributesRuleForm.EntityID);
            }
            else {
                this.InitAttributesTable(0);
            }
        },
        handleClose: function () {
            this.AttributesRuleForm.DataType = "";
            this.resetFormModel();
        },
        handleOpen: function () {
            this.AttributesRuleForm.Type = "";
        },
        handleDelete: function (index, row) {
            let that = this;
            var id = row.id;
            that.SelectedRow = row;
            that.$confirm('此操作将删除该属性, 是否继续?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                $.post("/MasterDataManage/AttributeDel", { Id: id }, RES => {
                    //console.log(RES);
                    if (RES.success) {
                        that.$message({
                            message: RES.message,
                            type: 'success'
                        });
                        if (!that.AttributesRuleForm.EntityID) {
                            that.InitAttributesTable(0);
                        }
                        else {
                            that.InitAttributesTable(that.AttributesRuleForm.EntityID);
                        }
                    }
                    else {
                        if (that.pagination.currentpage > 1 && !RES.data) {
                            that.pagination.currentpage = 1;
                            that.InitAttributesTable(that.AttributesRuleForm.EntityID || 0);
                        }
                        that.$message.error(RES.message);
                    }
                })
            }).catch(() => {
                return;
            });
        },
        handleEdit: function (index, row) {
            // console.log(row);
            this.disabled2 = true;
            this.SelectedRow = row;
            this.dialogVisible = true;
            this.disabled = true;
            this.$nextTick(() => {
                this.dialogTitle = "编辑属性";
                this.AttributesRuleForm.Id = row.id;
                this.AttributesRuleForm.LinkEntityID = row.linkEntityID == 0 ? "" : row.linkEntityID;
                if (this.AttributesRuleForm.LinkEntityID) {
                    this.EntitySelectOptions2 = this.AllEntityOptions;
                }
                this.AttributesRuleForm.LinkModelID = row.linkModelID == 0 ? "" : row.linkModelID;
                this.AttributesRuleForm.Name = row.name;
                this.AttributesRuleForm.Remark = row.remark;
                this.AttributesRuleForm.Type = row.type;
                this.AttributesRuleForm.StartTrace = row.startTrace == "1" ? true : false;
                this.AttributesRuleForm.IsDisplay = row.isDisplay == 1 ? true : false;
                this.AttributesRuleForm.TypeLength = row.typeLength;
                this.AttributesRuleForm.DisplayName = row.displayName;
                this.AttributesRuleForm.Creater = row.Creater;
                this.AttributesRuleForm.CreateTime = row.createTime;
                this.AttributesRuleForm.Updater = row.name;
                this.AttributesRuleForm.UpdateTime = row.updateTime;
                this.AttributesRuleForm.DataType = row.dataType;
            });
        },
        resetFormModel: function () {
            this.AttributesRuleForm.StartTrace = false;
            this.AttributesRuleForm.LinkModelID = "";
            this.AttributesRuleForm.LinkEntityID = "";
            this.$refs.AttributesRuleForm.resetFields();
        },
        AddAttributes: function () {
            if (!this.ModelID) {
                this.$message.warning("请选择模型");
                return;
            }
            if (!this.AttributesRuleForm.EntityID) {
                this.$message.warning("请选择实体");
                return;
            }
            this.disabled2 = false;
            this.dialogTitle = '添加属性';
            this.disabled = false;
            this.AttributesRuleForm.IsDisplay = true;
            this.AttributesRuleForm.Id = "";
            this.AttributesRuleForm.Type = "";
            this.dialogVisible = true;
        },
        AddOrUpdate: function () {
            ModelVM.$refs.AttributesRuleForm.validate((valid) => {
                if (valid) {
                    let that = this;
                    var url = "";
                    if (that.AttributesRuleForm.Id) {
                        url = "/MasterDataManage/AttributeUpdate";
                        that.AttributesRuleForm.EntityID = that.SelectedRow.entityID;
                    }
                    else {
                        url = "/MasterDataManage/AttributeAdd";
                        that.AttributesRuleForm.Id = 0;
                    }
                    if (!that.AttributesRuleForm.LinkModelID) {
                        that.AttributesRuleForm.LinkModelID = 0;
                    }
                    if (!that.AttributesRuleForm.LinkEntityID) {
                        that.AttributesRuleForm.LinkEntityID = 0;
                    }
                    if (that.AttributesRuleForm.StartTrace) {
                        that.AttributesRuleForm.StartTrace = "1";
                    }
                    else {
                        that.AttributesRuleForm.StartTrace = "0";
                    }
                    if (that.AttributesRuleForm.IsDisplay) {
                        that.AttributesRuleForm.IsDisplay = 1;
                    }
                    else {
                        that.AttributesRuleForm.IsDisplay = 0;
                    }
                    if (that.AttributesRuleForm.Type == "文件") {
                        that.AttributesRuleForm.DataType = "文件";
                    }
                    //console.log(that.AttributesRuleForm);
                    $.post(url, { attribute: JSON.stringify(that.AttributesRuleForm) }, RES => {
                        //console.log(RES);
                        if (RES.success) {
                            that.$message({
                                message: RES.message,
                                type: 'success'
                            });
                            that.resetFormModel();
                            if (!that.ModelID) {
                                that.AttributesRuleForm.EntityID = "";
                            }
                            else {
                                if (that.SelectedRow) {
                                    that.AttributesRuleForm.EntityID = that.SelectedRow.entityID;
                                }
                            }
                            that.AttributesRuleForm.DataType = "";
                            that.dialogVisible = false;
                            that.InitAttributesTable(that.AttributesRuleForm.EntityID || 0);
                        }
                        else {
                            that.$message.error(RES.message);
                        }
                    })
                }
                else {
                    return false;
                }
            })

        },
        CancelAddOrUpdate: function () {
            this.resetFormModel();
            this.dialogVisible = false;
        },
        ModelHasChanged: function () {
            //console.log(this.ModelID);
            this.AttributesRuleForm.EntityID = "";
            this.EntitySelectOptions = this.AllEntityOptions.filter(o => {
                return o.modelId == this.ModelID;
            })
        },
        EntityHasChanged: function () {
            if (!this.ModelID) {
                this.$message.warning("请选择模型");
                this.AttributesRuleForm.EntityID = "";
                return;
            }
            this.pagination.currentpage = 1;
            this.InitAttributesTable(this.AttributesRuleForm.EntityID);
        },
        ModelHasChanged2: function () {
            if (this.AttributesRuleForm.LinkEntityID) {
                this.AttributesRuleForm.LinkEntityID = "";
            }
            this.EntitySelectOptions2 = this.AllEntityOptions.filter(o => {
                return o.modelId == this.AttributesRuleForm.LinkModelID;
            })
        },
        TypeHasChanged: function () {
            if (this.AttributesRuleForm.Type != "基于域") {
                this.AttributesRuleForm.LinkEntityID = "";
                this.AttributesRuleForm.LinkModelID = "";
                this.AttributesRuleForm.TypeLength = "";
            }
            else {
                this.AttributesRuleForm.DataType = "";
            }
        }
    }
})