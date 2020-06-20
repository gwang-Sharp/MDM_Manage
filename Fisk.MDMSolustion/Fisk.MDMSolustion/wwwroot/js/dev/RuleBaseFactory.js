
var RuleBaseManage = new Vue({
    el: '#RuleBaseManage',
    data: {
        tableHeight: 300, //默认给300
        RuleBaseTableData: [],//角色表格
        total: 1,//总条数
        pagenum: 1, // 当前页
        pagesize: 10, // 每页条数
        pageMaxsize: '', //当前页可以存放最大条数
        Title: '添加角色',//弹框标题
        ModelName: "",
        TabNumber: 'first',
        activeName: 'first',
        ModelSelectOptions: [],
        EntityName: "",
        EntitySelectOptions: [],
        AttributeSelectOptions: [],
        AddRuleBasedialogVisible: false,//添加角色弹框是否显示
        //添加用户角色form表单
        RuleBaseForm: {
            ID: '',
            ModelID: '',
            EntityID: '',
            AttributeID: '',
            AttributeName: "",
            RuleName: '',
            Description: '',
            Required: true,
            Type: '',
            Expression: "",
            CustomTypeName: ""
        },
        //验证Roleform表单规则
        RuleBaseRules: {
            AttributeName: [{ required: true, message: "请选择属性", trigger: 'blur' }],
            Description: [{ required: true, message: "请输入规则描述", trigger: 'blur' }],
            RuleName: [{ required: true, message: "请输入规则名称", trigger: 'blur' }],
            Type: [{ required: true, message: "请选择验证规则", trigger: 'blur' }],
        },
        RuleBaseTabLoading: false,
        pagenumBottom: '',
        EditOrAdd: 'Add',
        CustomTypeIsShow: false,
        RegularIs: false,
        DataValidationTableData: [],
        DataValidationTabLoading: false,
        DataValidationForm: {
            Id: 0,
            Name: '',
            ModelID: 0,
            EntityID: 0,
            Description: '',
            FunctionName: '',
        },
        DataValidationFormRules: {
            Name: [{ required: true, message: "请输入名称", trigger: 'blur' }],
            Description: [{ required: true, message: "请输入说明", trigger: 'blur' }],
            FunctionName: [{ required: true, message: "请输入存储过程名称", trigger: 'blur' }],
        },
        DatadialogTitle: '添加逻辑',
        DatadialogVisible: false,
        DataModelName: '',
        DataEntityName: '',
        DataModelSelectOptions: [],
        DataEntitySelectOptions: [],
        //-----------字段验证----
        FieldValidationTableData: [],
        //-----------数据维护------//
        EntityDataTableData: [],
        pagination: {
            currentpage: 1,
            pagesizes: [10, 20, 30, 40],
            pagesize: 10,
            allNum: 0,
        },
        DataFormModel: {
            RuleName: '',
            RuleType: '',
            Apiaddress: '',
            RuleRemark: '',
            AttributeID: [],
            BpmName: '',
            ApplyTitle:''
        },
        FieldEntityID: '',
        DataValidationFormRules: {
            RuleName: [{ required: true, message: "请输入名称", trigger: 'blur' }],
            RuleRemark: [{ required: true, message: "请输入说明", trigger: 'blur' }],
            RuleType: [{ required: true, message: "请选择维护类型", trigger: 'blur' }],
            Apiaddress: [{ required: true, message: "请填写API地址", trigger: 'blur' }],
            AttributeID: [{ required: true, message: "请选择属性", trigger: 'blur' }],
        },
        DataModelChangeSelectOptions: [
            { name: '添加', value: 'add' },
            { name: '变更监控', value: 'update' }
        ],
        DatadialogVisible2: false,
        DatadialogTitle2: '添加数据维护规则',
        AttrsSelectOptions: [],
        SelectedRow: '',
        HasError: false,
        //-----------数据维护---------//
        FieldValidationTabLoading: false,
        EntityDataTabLoading: false,
        FieldValidationForm: {
            Id: 0,
            Name: '',
            ModelID: 0,
            EntityID: 0,
            Description: '',
            FunctionName: '',
        },
        FieldValidationFormRules: {
            Name: [{ required: true, message: "请输入名称", trigger: 'blur' }],
            Description: [{ required: true, message: "请输入说明", trigger: 'blur' }],
            FunctionName: [{ required: true, message: "请输入存储过程名称", trigger: 'blur' }],
        },
        FieldialogTitle: '添加逻辑',
        FieldialogVisible: false,

        FieldModelName: '',
        FieldEntityName: '',
        FieldModelSelectOptions: [],
        FieldEntitySelectOptions: [],
    },
    created: function () {
    },
    mounted: function () {
        this.GetModelSelectData();
        this.$nextTick(() => {
            this.pagesize = parseInt((LayOutStaticWindowsHeight - 390) / 40);
            this.tableHeight = parseInt(LayOutStaticWindowsHeight - 210);
            this.pageMaxsize = this.pagesize;
            this.GetModelSelectData();
            this.GetRuleBaseTableList();
        });
    },
    methods: {
        GetModelSelectData: function () {
            $.post('/MasterData_Quality_Manage/GetModelSelectRule', {}, res => {
                if (res.success) {
                    this.ModelSelectOptions = res.data;

                    if (res.data != null) {
                        var ModelID = res.data[0].id;
                        this.ModelName = res.data[0].name;
                        this.RuleBaseForm.ModelID = ModelID;
                        this.GetEntitySelectData(ModelID);
                    }
                    else {
                        this.GetEntitySelectData(ModelID);
                    }
                }
            })
        },
        GetEntitySelectData: function (ModelID) {
            $.post('/MasterData_Quality_Manage/GetEntitySelectRule', { ModelID: ModelID }, res => {
                if (res.success) {

                    if (res.data.length > 0) {
                        this.EntitySelectOptions = res.data;
                        // console.log(res.data);
                        this.EntityName = res.data[0].name;
                        this.RuleBaseForm.EntityID = res.data[0].id;
                        this.GetAttributeSelectData();
                    }
                    else {
                        this.EntityName = '';
                        this.RuleBaseForm.EntityID = 0;
                    }
                    this.GetRuleBaseTableList();
                }
            })
        },
        GetAttributeSelectData: function () {
            $.post('/MasterData_Quality_Manage/GetAttributeSelectRule', { EntityID: this.RuleBaseForm.EntityID }, res => {
                if (res.success) {
                    this.AttributeSelectOptions = res.data;
                }
            })
        },
        //进入页面渲染表格
        GetRuleBaseTableList: function () {

            this.RuleBaseTabLoading = true;
            $.post('/MasterData_Quality_Manage/SearchRuleBase', {
                Where: this.RuleBaseForm.EntityID,
                Page: this.pagenum,
                limit: this.pagesize
            },
                function (res) {
                    RuleBaseManage.RuleBaseTableData = res.data;
                    RuleBaseManage.total = res.total;
                    RuleBaseManage.RuleBaseTabLoading = false;
                }, 'json');
        },
        // 只要每页条数变化了, 触发
        handleSizeChange: function (val) {
            // 更新每页条数
            RuleBaseManage.pagesize = val;
            // 只要修改了每页条数, 数据所在的页码发生了变化了, 回到第一页
            RuleBaseManage.pagenum = 1;
            // 重新渲染
            RuleBaseManage.GetRoleTableList();
        },
        // 只要当前页变化时, 触发函数
        handleCurrentChange: function (val) {
            // 更新当前页
            RuleBaseManage.pagenum = val;
            // 重新渲染
            RuleBaseManage.GetRoleTableList();
        },
        //点击添加用户对话框
        OpenRuleBaseDialog: function () {

            RuleBaseManage.AddRuleBasedialogVisible = true;
            RuleBaseManage.Title = '添加业务规则';
            RuleBaseManage.EditOrAdd = "Add";
            this.CustomTypeIsShow = false;

        },
        //点击edit 弹框 ,修改弹框名称,内容回显
        OpenRuleBaseEditDialog: function (index, row) {
            RuleBaseManage.AddRuleBasedialogVisible = true;
            RuleBaseManage.$nextTick(() => {
                RuleBaseManage.Title = '修改业务规则';
                RuleBaseManage.EditOrAdd = "Edit";
                RuleBaseManage.RuleBaseForm.ID = row.id;
                RuleBaseManage.RuleBaseForm.RuleName = row.ruleName;
                RuleBaseManage.RuleBaseForm.Description = row.description;
                RuleBaseManage.RuleBaseForm.Required = row.required == "true" ? true : false;
                if (row.type != "手机号" && row.type != "邮箱") {
                    this.CustomTypeIsShow = true;
                    RuleBaseManage.RuleBaseForm.Expression = row.expression;
                }
                else {
                    this.CustomTypeIsShow = false;
                }
                RuleBaseManage.RuleBaseForm.Type = row.type;
                RuleBaseManage.RuleBaseForm.AttributeName = row.attributeName;
                RuleBaseManage.RuleBaseForm.ModelID = row.modelID;
                RuleBaseManage.RuleBaseForm.AttributeID = row.attributeID;
                RuleBaseManage.RuleBaseForm.EntityID = row.entityID;
            });
        },
        //点击删除角色
        DelRuleBase: function (row) {
            //console.log(row)
            RuleBaseManage.$confirm("是否删除", "业务规则删除", {
                cancelButtonClass: "btn-custom-cancel",
                cancelButtonText: "取消",
                confirmButtonText: "确定",
                type: 'warning',
                callback: function callback(action, instance) {
                    if (action === 'confirm') {
                        var loading = RuleBaseManage.$loading({
                            lock: true,
                            text: '加载中……',
                            background: "rgba(255, 255, 255, 0.75)"
                        });
                        $.post('/MasterData_Quality_Manage/DeleteRuleBase', { RuleID: row.id }, function (res) {
                            loading.close();
                            if (res.success == true) {
                                RuleBaseManage.GetRuleBaseTableList();
                                RuleBaseManage.$message.success(res.message);
                            } else {
                                RuleBaseManage.$message.error(res.message);
                            }
                        }, 'json');
                    }
                }
            });
        },
        //关闭添加用户对话框或者修改用户对话框
        ClosedAddRuleBasedialog: function () {
            RuleBaseManage.$nextTick(() => {
                RuleBaseManage.AddRuleBasedialogVisible = false;
                RuleBaseManage.$refs.RuleBaseForm.resetFields();
                this.RuleBaseForm.Expression = "";
            });
        },
        //关闭添加用户对话框或者修改用户对话框
        CloseRuleBasedialogVisible: function (RoleForm) {
            RuleBaseManage.$nextTick(() => {
                RuleBaseManage.AddRuleBasedialogVisible = false;
                RuleBaseManage.$refs.RuleBaseForm.resetFields();//重置表单
                this.RuleBaseForm.Expression = "";
            });

        },
        SubmitRuleBaseForm: function () { //提交角色表单
            if (RuleBaseManage.EditOrAdd == 'Add') {
                RuleBaseManage.$refs.RuleBaseForm.validate((valid) => {
                    if (valid) {
                        var loading = RuleBaseManage.$loading({
                            lock: true,
                            text: '加载中……',
                            background: "rgba(255, 255, 255, 0.75)"
                        });
                        $.post('/MasterData_Quality_Manage/InsertRuleBase', this.RuleBaseForm, function (res) {
                            loading.close();
                            if (res.success == true) {
                                RuleBaseManage.$nextTick(() => {
                                    RuleBaseManage.$message.success(res.message);
                                    RuleBaseManage.AddRuleBasedialogVisible = false;
                                    RuleBaseManage.GetRuleBaseTableList();
                                    RuleBaseManage.$refs.RuleBaseForm.resetFields();//重置表单
                                });
                            } else {
                                RuleBaseManage.$message.error(res.message);
                                RuleBaseManage.AddRuleBasedialogVisible = false;
                            }
                        }, 'json');
                    } else {
                        return false;
                    }
                });
            } else {
                RuleBaseManage.$refs.RuleBaseForm.validate((valid) => {
                    if (valid) {
                        var loading = RuleBaseManage.$loading({
                            lock: true,
                            text: '加载中……',
                            background: "rgba(255, 255, 255, 0.75)"
                        });
                        $.post('/MasterData_Quality_Manage/EiteRuleBase', this.RuleBaseForm, function (res) {
                            loading.close();
                            if (res.success == true) {
                                RuleBaseManage.$nextTick(() => {
                                    RuleBaseManage.$message.success(res.message);
                                    RuleBaseManage.AddRuleBasedialogVisible = false;
                                    RuleBaseManage.GetRuleBaseTableList();
                                    RuleBaseManage.$refs.RuleBaseForm.resetFields(); //重置表单
                                });
                            } else {
                                RuleBaseManage.$message.error(res.message);
                                RuleBaseManage.AddRuleBasedialogVisible = false;
                            }
                        }, 'json');
                    } else {
                        return false;
                    }
                });
            }
        },
        //  表头样式
        headerClass: function () {
            return 'text-align: center;background:#eef1f6;'
        },
        AddRole: function () {
            RuleBaseManage.TitleType = 'Add';
            RuleBaseManage.Add_Edit_Role();
        },
        TabHandleClick: function (tab, event) {

            var TabNumber = tab.name;
            this.TabNumber = tab.name;
            this.pagenum = 1;
            switch (TabNumber) {
                case "second":
                    this.GetDataValidationTableList();
                    this.GetDataModelSelectData();
                    break;
                case "first":
                    this.GetRuleBaseTableList();
                    break;
                case "Three":
                    this.GetFieldModelSelectData();
                    this.InitEntityDataTable();
                    break;
                default:
                    break;
            }
        },
        //绑定回车事件
        EnterSearch: function () {
            $('#RoleKeyStr').bind('keydown', function (event) {
                if (event.keyCode == "13") {
                    RuleBaseManage.ReloadTable('Role');
                }
            });
        },
        ModelHasChanged: function (ModelID) {
            switch (this.TabNumber) {
                case "second":
                    this.GetDataEntitySelectData(ModelID);
                case "first":
                    this.GetEntitySelectData(ModelID);
                    break;
                case "Three":
                    this.GetFieldEntitySelectData(ModelID);
                    this.FieldEntityID = "";
                    break;
            }
        },
        EntityHasChanged: function (ID) {
            switch (this.TabNumber) {
                case "second":
                    this.DataValidationForm.EntityID = ID;
                    this.GetDataValidationTableList();
                case "first":
                    this.RuleBaseForm.EntityID = ID;
                    this.GetAttributeSelectData();
                    this.GetRuleBaseTableList();
                    break;
                case "Three":
                    this.FieldValidationForm.EntityID = ID;
                    break;
            }
        },
        AttributeNameDialogChange: function (ID) {
            this.RuleBaseForm.AttributeID = ID;
        },
        TypeChange: function (val) {
            switch (val) {
                case "手机号":
                    this.CustomTypeIsShow = false;
                    this.RuleBaseForm.Expression = "^[1][35678][0-9]{9}$"
                    this.RegularIs = true;
                    break;
                case "邮箱":
                    this.CustomTypeIsShow = false;
                    this.RuleBaseForm.Expression = "^[a-z0-9A-Z]+[-|a-z0-9A-Z._]+@([a-z0-9A-Z]+(-[a-z0-9A-Z]+)?\\.)+[a-z]{2,}$"
                    this.RegularIs = true;
                    break;
                case "自定义验证规则":
                    this.CustomTypeIsShow = true;
                    this.RuleBaseForm.Expression = "";
                    this.RegularIs = false;
                    break;
                default:
                    break;
            }

        },

        ReleaseProc() {
            var obj = {
                modelID: this.RuleBaseForm.ModelID,
                entityID: this.RuleBaseForm.EntityID
            }
            $.post('/MasterData_Quality_Manage/AttributeRuleRelease', obj, res => {
                if (res.success) {
                    this.$message.success(res.message);
                    this.GetRuleBaseTableList();
                } else
                    this.$message.error(res.message);
            })
        }, //发布存储过程
        //------------------------------------数据维护--------------------------------------------
        InitEntityDataTable: function () {
            let that = this;
            $.post("/MasterData_Quality_Manage/GetDatamaintenance", {
                page: that.pagination.currentpage,
                rows: that.pagination.pagesize
            }, RES => {
                // console.log(RES);
                if (RES.data) {
                    that.EntityDataTableData = RES.data;
                    that.pagination.allNum = RES.total;
                }
            })
        },
        OpenDataTableDialog: function () {
            if (!this.FieldEntityID) {
                this.$message.error("请先选择实体");
                return;
            }
            RuleBaseManage.DatadialogVisible2 = true;
            RuleBaseManage.DatadialogTitle2 = '添加数据维护规则';
            RuleBaseManage.EditOrAdd = "Add";
        },
        resetFormModel: function () {
            RuleBaseManage.DataFormModel.RuleType = "";
            RuleBaseManage.DataFormModel.AttributeID = [];
            delete RuleBaseManage.DataFormModel.Id;
            RuleBaseManage.$refs.DataFormModel.resetFields();
        },
        DataCancelAddOrUpdate2: function () {
            this.resetFormModel();
            this.DatadialogVisible2 = false;
        },
        SubmitDataFrom: function () {
            let that = this;
            that.$refs.DataFormModel.validate((valid) => {
                if (valid) {
                    var url = "";
                    var postData = {};
                    if (that.EditOrAdd == "Add") {
                        if (that.HasError) {
                            if (that.DataFormModel.AttributeID) {
                                let attrArray = that.DataFormModel.AttributeID;
                                that.DataFormModel.AttributeID = [];
                                $.each(attrArray, function (index, value) {
                                    var id = that.AttrsSelectOptions.find(a => {
                                        return a.name == value;
                                    }).id;
                                    that.DataFormModel.AttributeID.push(id);
                                })
                                that.DataFormModel.AttributeID = that.DataFormModel.AttributeID.join(",");
                            }
                        }
                        else {
                            that.DataFormModel.AttributeID = that.DataFormModel.AttributeID.join(",");
                        }
                        postData.entityid = that.FieldEntityID;
                        postData._Datamaintenance = JSON.stringify(that.DataFormModel);
                        url = "/MasterData_Quality_Manage/AddDatamaintenance";
                    }
                    if (that.EditOrAdd == "update") {
                        let attrArray = that.DataFormModel.AttributeID;
                        that.DataFormModel.AttributeID = [];
                        $.each(attrArray, function (index, value) {
                            var attr = that.AttrsSelectOptions.find(a => {
                                return a.name == value;
                            });
                            if (attr) {
                                that.DataFormModel.AttributeID.push(attr.id);
                            }
                            else {
                                that.DataFormModel.AttributeID.push(value);
                            }
                        })
                        that.DataFormModel.AttributeID = that.DataFormModel.AttributeID.join(",");
                        url = "/MasterData_Quality_Manage/UpdataDatamaintenance";
                        postData.FormModel = JSON.stringify(that.DataFormModel);
                    }
                    $.post(url, postData, RES => {
                        if (RES.success) {
                            that.$message({
                                message: RES.message,
                                type: 'success'
                            });
                            that.InitEntityDataTable();
                            that.DatadialogVisible2 = false;
                            that.resetFormModel();
                        }
                        else {
                            if (typeof (that.DataFormModel.AttributeID) == "string") {
                                if (that.DataFormModel.AttributeID) {
                                    let attrArray = that.DataFormModel.AttributeID.split(",");
                                    that.DataFormModel.AttributeID = [];
                                    $.each(attrArray, function (index, value) {
                                        var name = that.AttrsSelectOptions.find(a => {
                                            return a.id == value;
                                        }).name;
                                        that.DataFormModel.AttributeID.push(name);
                                    })
                                }
                            }
                            that.HasError = true;
                            that.$message.error(RES.message);
                        }
                    })
                }
                else {
                    return false;
                }
            })
        },
        handleSizeChange3: function (val) {
            this.pagination.pagesize = val();
            this.InitEntityDataTable();
        },
        handleCurrentChange3: function (val) {
            this.pagination.currentpage = val;
            this.InitEntityDataTable();
        },
        DatahandleClose2: function () {
            this.DataFormModel.AttributeID = [];
            this.resetFormModel();
        },
        DelDtValidation: function (row) {
            let that = this;
            // console.log(row);
            that.$confirm('此操作将删除该规则, 是否继续?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                $.post("/MasterData_Quality_Manage/DelEntityDataRule", { Id: row.id }, RES => {
                    //console.log(RES);
                    if (RES.success) {
                        that.$message({
                            message: RES.message,
                            type: 'success'
                        });
                        that.InitEntityDataTable();
                    }
                    else {
                        that.$message.error(RES.message);
                    }
                })
            }).catch(() => {
                return;
            });
        },
        EntityDataRuleEditDialog: function (row) {
            //console.log(row);
            this.SelectedRow = row;
            this.FieldEntityID = row.entityID;
            this.DatadialogVisible2 = true;
            this.DatadialogTitle2 = "编辑实体数据维护规则";
            this.EditOrAdd = "update";
            this.$nextTick(() => {
                if (row.ruleType == "update") {
                    this.AttrsSelectOptions = [];
                    this.InitAttrSelectOptions();
                    this.DataFormModel.AttributeID = [];
                }
                this.DataFormModel.RuleType = row.ruleType;
                this.DataFormModel.Id = row.id;
                this.DataFormModel.Apiaddress = row.apiaddress;
                this.DataFormModel.RuleName = row.ruleName;
                this.DataFormModel.RuleRemark = row.ruleRemark;
                this.DataFormModel.BpmName = row.bpmName;
                this.DataFormModel.ApplyTitle = row.applyTitle;
            });
        },
        InitAttrSelectOptions: function () {
            let that = this;
            $.post("/MasterData_Quality_Manage/Attributes_GetAll_ByEntityID", {
                EntityID: that.FieldEntityID
            }, RES => {
                if (RES.success) {
                    that.AttrsSelectOptions = RES.data;
                    if (that.EditOrAdd == "update") {
                        if (that.SelectedRow.attributeID) {
                            var RowsAttrs = that.SelectedRow.attributeID.split(",");
                            if (RowsAttrs.length > 0) {
                                $.each(RowsAttrs, function (index, value) {
                                    var name = RuleBaseManage.AttrsSelectOptions.find(a => {
                                        return a.id == value;
                                    }).name;
                                    RuleBaseManage.DataFormModel.AttributeID.push(name);
                                })
                            }
                        }
                    }
                }
                else {
                    that.AttrsSelectOptions = [];
                }
            })
        },
        TypeHasChanged: function () {
            if (this.DataFormModel.RuleType == "update") {
                this.AttrsSelectOptions = [];
                this.DataFormModel.AttributeID = [];
                this.InitAttrSelectOptions();
            }
            if (this.DataFormModel.RuleType == "add") {
                if (this.EditOrAdd == "update") {
                    this.SelectedRow.attributeID = "";
                }
                this.AttrsSelectOptions = [];
                this.DataFormModel.AttributeID = [];
            }
        },
        TypeTransfer: function (row, column, cellValue, index) {
            if (cellValue == "update") {
                return "变更监控";
            }
            else {
                return "添加";
            }
        },
        //------------------------------------ 数据验证------------------------------------------
        SubmitDataValidationFrom: function () {
            if (RuleBaseManage.EditOrAdd == 'Add') {
                RuleBaseManage.$refs.DataValidationForm.validate((valid) => {
                    if (valid) {
                        var loading = RuleBaseManage.$loading({
                            lock: true,
                            text: '加载中……',
                            background: "rgba(255, 255, 255, 0.75)"
                        });

                        $.post('/MasterData_Quality_Manage/InsertDataValidation', this.DataValidationForm, function (res) {
                            loading.close();
                            if (res.success == true) {
                                RuleBaseManage.$nextTick(() => {
                                    RuleBaseManage.$message.success(res.message);
                                    RuleBaseManage.DatadialogVisible = false;
                                    RuleBaseManage.GetDataValidationTableList();
                                    RuleBaseManage.$refs.DataValidationForm.resetFields();//重置表单
                                });
                            } else {
                                RuleBaseManage.$message.error(res.message);
                                RuleBaseManage.DatadialogVisible = false;
                            }
                        }, 'json');
                    } else {
                        return false;
                    }
                });
            } else {
                RuleBaseManage.$refs.DataValidationForm.validate((valid) => {
                    if (valid) {
                        var loading = RuleBaseManage.$loading({
                            lock: true,
                            text: '加载中……',
                            background: "rgba(255, 255, 255, 0.75)"
                        });
                        $.post('/MasterData_Quality_Manage/UpdateDataValidation', this.DataValidationForm, function (res) {
                            loading.close();
                            if (res.success == true) {
                                RuleBaseManage.$nextTick(() => {
                                    RuleBaseManage.$message.success(res.message);
                                    RuleBaseManage.DatadialogVisible = false;
                                    RuleBaseManage.GetDataValidationTableList();
                                    RuleBaseManage.$refs.DataValidationForm.resetFields(); //重置表单
                                });
                            } else {
                                RuleBaseManage.$message.error(res.message);
                                RuleBaseManage.DatadialogVisible = false;
                            }
                        }, 'json');
                    } else {
                        return false;
                    }
                });
            }
        }, //数据验证提交表单
        DataCancelAddOrUpdate: function () {
            RuleBaseManage.DatadialogVisible = false;
            RuleBaseManage.$refs.DataValidationForm.resetFields();
        },
        GetDataValidationTableList: function () {
            this.DataValidationTabLoading = true;
            $.post('/MasterData_Quality_Manage/SearchDataValidation', {
                Where: this.DataValidationForm.EntityID,
                Page: this.pagenum,
                limit: this.pagesize
            },
                function (res) {
                    RuleBaseManage.DataValidationTableData = res.data;
                    RuleBaseManage.total = res.total;
                    RuleBaseManage.DataValidationTabLoading = false;
                }, 'json');
        },
        OpenDataValidationTableDialog: function () {
            RuleBaseManage.DatadialogVisible = true;
            RuleBaseManage.DatadialogTitle = '添加逻辑规则';
            RuleBaseManage.EditOrAdd = "Add";
        },//  添加打开面板
        OpenDataValidationEditDialog: function (row) {
            RuleBaseManage.DatadialogVisible = true;
            RuleBaseManage.$nextTick(() => {
                RuleBaseManage.Title = '修改逻辑规则';
                RuleBaseManage.EditOrAdd = "Edit";
                RuleBaseManage.DataValidationForm.Id = row.id;
                RuleBaseManage.DataValidationForm.Name = row.name;
                RuleBaseManage.DataValidationForm.Description = row.description;
                RuleBaseManage.DataValidationForm.FunctionName = row.functionName;
            });
        },// 编辑打开页面
        DatahandleClose: function () { },
        GetDataModelSelectData: function () {
            $.post('/MasterData_Quality_Manage/GetModelSelectRule', {}, res => {
                if (res.success) {
                    this.DataModelSelectOptions = res.data;
                    if (res.data != null) {
                        this.DataModelName = res.data[0].name;
                        this.DataValidationForm.ModelID = res.data[0].id;
                        this.GetDataEntitySelectData(res.data[0].id);
                    }
                    else {
                        this.DataModelName = "";
                        this.GetDataEntitySelectData();
                    }
                }
            })
        },  //  模型下拉数据
        GetDataEntitySelectData: function (ModelID) {
            $.post('/MasterData_Quality_Manage/GetEntitySelectRule', { ModelID: ModelID }, res => {
                if (res.success) {

                    if (res.data.length > 0) {
                        this.DataEntitySelectOptions = res.data;
                        //this.DataEntityName = res.data[0].name;
                        this.DataEntityName = "";
                        this.DataValidationForm.EntityID = res.data[0].id;
                    }
                    else {
                        this.DataEntityName = "";
                        this.DataValidationForm.EntityID = null;
                    }
                    this.GetDataValidationTableList();
                }
            })
        },// 实体下拉数据
        DelDataValidation: function (row) {
            RuleBaseManage.$confirm("是否删除", "数据验证删除", {
                cancelButtonClass: "btn-custom-cancel",
                cancelButtonText: "取消",
                confirmButtonText: "确定",
                type: 'warning',
                callback: function callback(action, instance) {
                    if (action === 'confirm') {
                        var loading = RuleBaseManage.$loading({
                            lock: true,
                            text: '加载中……',
                            background: "rgba(255, 255, 255, 0.75)"
                        });
                        $.post('/MasterData_Quality_Manage/DeleteDataValidation', { DataID: row.id }, function (res) {
                            loading.close();
                            if (res.success == true) {
                                RuleBaseManage.GetDataValidationTableList();
                                RuleBaseManage.$message.success(res.message);
                            } else {
                                RuleBaseManage.$message.error(res.message);
                            }
                        }, 'json');
                    }
                }
            });
        },      // 删除数据

        //------------------------------------ 字段验证
        SubmitFieldValidationFrom: function () {
            if (RuleBaseManage.EditOrAdd == 'Add') {
                RuleBaseManage.$refs.FieldValidationForm.validate((valid) => {
                    if (valid) {
                        var loading = RuleBaseManage.$loading({
                            lock: true,
                            text: '加载中……',
                            background: "rgba(255, 255, 255, 0.75)"
                        });

                        $.post('/MasterData_Quality_Manage/InsertFieldValidation', this.FieldValidationForm, function (res) {
                            loading.close();
                            if (res.success == true) {
                                RuleBaseManage.$nextTick(() => {
                                    RuleBaseManage.$message.success(res.message);
                                    RuleBaseManage.FieldialogVisible = false;
                                    RuleBaseManage.GetFielValidationTableList();
                                    RuleBaseManage.$refs.FieldValidationForm.resetFields();//重置表单
                                });
                            } else {
                                RuleBaseManage.$message.error(res.message);
                                RuleBaseManage.FielddialogVisible = false;
                            }
                        }, 'json');
                    } else {
                        return false;
                    }
                });
            } else {
                RuleBaseManage.$refs.FieldValidationForm.validate((valid) => {
                    if (valid) {
                        var loading = RuleBaseManage.$loading({
                            lock: true,
                            text: '加载中……',
                            background: "rgba(255, 255, 255, 0.75)"
                        });
                        $.post('/MasterData_Quality_Manage/UpdateFieldValidation', this.FieldValidationForm, function (res) {
                            loading.close();
                            if (res.success == true) {
                                RuleBaseManage.$nextTick(() => {
                                    RuleBaseManage.$message.success(res.message);
                                    RuleBaseManage.FieldialogVisible = false;
                                    RuleBaseManage.GetFielValidationTableList();
                                    RuleBaseManage.$refs.FieldValidationForm.resetFields(); //重置表单
                                });
                            } else {
                                RuleBaseManage.$message.error(res.message);
                                RuleBaseManage.FielddialogVisible = false;
                            }
                        }, 'json');
                    } else {
                        return false;
                    }
                });
            }
        },//  字段验证提交表单
        FieldCancelAddOrUpdate: function () {
            RuleBaseManage.FieldialogVisible = false;
            RuleBaseManage.$refs.FieldValidationForm.resetFields();
        },
        GetFielValidationTableList: function () {
            this.FieldValidationTabLoading = true;
            $.post('/MasterData_Quality_Manage/SearchFieldValidation', {
                Where: this.FieldValidationForm.EntityID,
                Page: this.pagenum,
                limit: this.pagesize
            },
                function (res) {
                    RuleBaseManage.FieldValidationTableData = res.data;
                    RuleBaseManage.total = res.total;
                    RuleBaseManage.FieldValidationTabLoading = false;
                }, 'json');
        },   //获取字段验证表单数据
        OpenFieldValidationTableDialog: function () {
            RuleBaseManage.FieldialogVisible = true;
            RuleBaseManage.FielddialogTitle = '添加逻辑规则';
            RuleBaseManage.EditOrAdd = "Add";
        },//  添加打开面板
        OpenFieldValidationEditDialog: function (row) {
            RuleBaseManage.FieldialogVisible = true;
            RuleBaseManage.$nextTick(() => {
                RuleBaseManage.Title = '修改逻辑规则';
                RuleBaseManage.EditOrAdd = "Edit";
                RuleBaseManage.FieldValidationForm.Id = row.id;
                RuleBaseManage.FieldValidationForm.Name = row.name;
                RuleBaseManage.FieldValidationForm.Description = row.description;
            });
        },  // 编辑打开页面
        FieldhandleClose: function () { },
        GetFieldModelSelectData: function () {
            $.post('/MasterData_Quality_Manage/GetModelSelectRule', {}, res => {
                if (res.success) {
                    this.FieldModelSelectOptions = res.data;
                    if (res.data != null) {
                        this.FieldModelName = res.data[0].name;
                        this.FieldValidationForm.ModelID = res.data[0].id;
                        this.GetFieldEntitySelectData(res.data[0].id);
                    }
                    else {
                        this.FieldModelName = "";
                        this.GetFieldEntitySelectData(ModelID);
                    }
                }
            })
        },          //  模型下拉数据
        GetFieldEntitySelectData: function (ModelID) {
            $.post('/MasterData_Quality_Manage/GetEntitySelectRule', { ModelID: ModelID }, res => {
                if (res.success) {

                    if (res.data.length > 0) {
                        this.FieldEntitySelectOptions = res.data;
                        this.FieldEntityName = res.data[0].name;
                        this.FieldValidationForm.EntityID = res.data[0].id;
                    }
                    else {
                        this.FieldEntityName = "";
                        this.FieldValidationForm.EntityID = null;
                    }
                    this.GetFielValidationTableList();
                }
            })
        },  // 实体下拉数据
        DelFieldValidation: function (row) {
            RuleBaseManage.$confirm("是否删除", "数据验证删除", {
                cancelButtonClass: "btn-custom-cancel",
                cancelButtonText: "取消",
                confirmButtonText: "确定",
                type: 'warning',
                callback: function callback(action, instance) {
                    if (action === 'confirm') {
                        var loading = RuleBaseManage.$loading({
                            lock: true,
                            text: '加载中……',
                            background: "rgba(255, 255, 255, 0.75)"
                        });
                        $.post('/MasterData_Quality_Manage/DeleteFieldValidation', { DataID: row.id }, function (res) {
                            loading.close();
                            if (res.success == true) {
                                RuleBaseManage.GetFielValidationTableList();
                                RuleBaseManage.$message.success(res.message);
                            } else {
                                RuleBaseManage.$message.error(res.message);
                            }
                        }, 'json');
                    }
                }
            });
        },     // 删除数据
    },
    watch: {
        RoleSearchName(val, oldval) {
            RuleBaseManage.pagenum = 1;
            this.GetRoleTableList();
        }
    }
});