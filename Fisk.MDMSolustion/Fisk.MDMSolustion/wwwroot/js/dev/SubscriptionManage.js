
var subVm = new Vue({
    el: "#SubscriptionManage",
    data: {
        ModelID: '',
        EntityID: '',
        TableHeight: 'calc(100vh - 210px)',
        ModelSelectOptions: [],
        EntitySelectOptions: [],
        AttrsSelectOptions: [],
        AllEntityOptions: [],
        ColsTabLoading: false,
        AddOrEdit: '',
        pagination: {
            currentpage: 1,
            pagesizes: [10, 20, 30, 40],
            pagesize: 10,
            allNum: 0,
        },
        DataFormModel: {
            EntityID: '',
            Name: '',
            SubscriptionRemark: '',
            Apiaddress: '',
            AttributeID: []
        },
        DataValidationFormRules: {
            Name: [{ required: true, message: "请输入名称", trigger: 'blur' }],
            SubscriptionRemark: [{ required: true, message: "请输入说明", trigger: 'blur' }],
            Apiaddress: [{ required: true, message: "请填写API地址", trigger: 'blur' }],
            AttributeID: [{ required: true, message: "请选择属性", trigger: 'blur' }],
        },
        DialogVisible: false,
        DialogTitle: "添加订阅",
        SubscriptionTableCols: [
            { field: "name", title: "名称" },
            { field: "subscriptionRemark", title: "说明" },
            { field: "apiaddress", title: "API地址" },
            { field: "createUser", title: "创建时间" },
            { field: "createTime", title: "创建人" },
            { field: "updateUser", title: "最近更新人" },
            { field: "updateTime", title: "最近更新时间" }
        ],
        TableData: [],
        HasError: false,
        SelectedRow: ''
    },
    created: function () {
        this.InitSubscriptionTable();
        this.InitModelSelectOptions();
        this.InitEntitySelectOptions();
    },
    mounted: function () { },
    methods: {
        headerClass: function () {
            return 'text-align: center;background:#eef1f6;'
        },
        InitSubscriptionTable: function () {
            let that = this;
            $.post("/MasterData_Subscription_Manage/InitSubscriptionTable",
                {
                    EntityID: that.EntityID,
                    page: that.pagination.currentpage,
                    rows: that.pagination.pagesize
                },
                RES => {
                    //console.log(RES);
                    if (RES.data) {
                        that.TableData = RES.data;
                    }
                    else {
                        that.TableData = [];
                    }
                    that.pagination.allNum = RES.total;
                })
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
        InitAttrSelectOptions: function () {
            let that = this;
            $.post("/MasterData_Subscription_Manage/AttributesGet_ByEntityID", {
                EntityID: that.EntityID || that.SelectedRow.entityID
            }, RES => {
                if (RES.success) {
                    that.AttrsSelectOptions = RES.data;
                    if (that.AddOrEdit == "update") {
                        if (that.SelectedRow.attributeID) {
                            var RowsAttrs = that.SelectedRow.attributeID.split(",");
                            if (RowsAttrs.length > 0) {
                                $.each(RowsAttrs, function (index, value) {
                                    var name = that.AttrsSelectOptions.find(a => {
                                        return a.id == value;
                                    }).name;
                                    that.DataFormModel.AttributeID.push(name);
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
                    that.AllEntityOptions = [];
                }
            })
        },
        ModelHasChanged: function () {
            //console.log(this.ModelID);
            this.EntityID = "";
            this.EntitySelectOptions = this.AllEntityOptions.filter(o => {
                return o.modelId == this.ModelID;
            })
        },
        EntityHasChanged: function () {
            if (!this.ModelID) {
                this.$message.error("请先选择模型");
                this.EntityID = "";
                return;
            }
            this.InitSubscriptionTable();
        },
        handleSizeChange: function (val) {
            this.pagination.pagesize = val;
            this.InitSubscriptionTable()
        },
        handleCurrentChange: function (val) {
            this.pagination.currentpage = val;
            this.InitSubscriptionTable()
        },
        AddSubscription: function () {
            if (!this.ModelID) {
                this.$message.error("请先选择模型");
                return;
            }
            if (!this.EntityID) {
                this.$message.error("请先选择实体");
                return;
            }
            this.DialogTitle = "添加订阅";
            this.AddOrEdit = "add";
            this.InitAttrSelectOptions();
            this.DialogVisible = true;
        },
        handleDelete: function (row) {
            let that = this;
            // console.log(row);
            that.$confirm('此操作将删除该订阅数据, 是否继续?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                $.post("/MasterData_Subscription_Manage/DelSubscription", { Id: row.id }, RES => {
                    //console.log(RES);
                    if (RES.success) {
                        that.$message({
                            message: RES.message,
                            type: 'success'
                        });
                        that.InitSubscriptionTable();
                    }
                    else {
                        that.$message.error(RES.message);
                    }
                })
            }).catch(() => {
                return;
            });
        },
        handleEdit: function (row) {
            this.AddOrEdit = "update";
            this.SelectedRow = row;
            this.DialogTitle = "编辑订阅";
            this.DialogVisible = true;
            this.$nextTick(() => {
                this.AttrsSelectOptions = [];
                this.InitAttrSelectOptions();
                this.DataFormModel.Name = row.name;
                this.DataFormModel.Id = row.id;
                this.DataFormModel.Apiaddress = row.apiaddress;
                this.DataFormModel.EntityID = row.entityID;
                this.DataFormModel.SubscriptionRemark = row.subscriptionRemark;

            });
        },
        DialoghandleClose: function () {
            this.resetFormModel();
        },
        resetFormModel: function () {
            delete this.DataFormModel.Id;
            this.$refs.DataFormModel.resetFields();
        },
        SubmitDataFrom: function () {
            let that = this;
            that.$refs.DataFormModel.validate((valid) => {
                if (valid) {
                    var url = "";
                    var postData = {};
                    if (that.AddOrEdit == "add") {
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
                                that.DataFormModel.EntityID = that.EntityID;
                                postData.FormModel = JSON.stringify(that.DataFormModel);
                            }
                        }
                        else {
                            that.DataFormModel.AttributeID = that.DataFormModel.AttributeID.join(",");
                            that.DataFormModel.EntityID = that.EntityID;
                            postData.FormModel = JSON.stringify(that.DataFormModel);
                        }
                        url = "/MasterData_Subscription_Manage/AddSubscription";
                    }
                    if (that.AddOrEdit == "update") {
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
                        url = "/MasterData_Subscription_Manage/UpdataSubscription";
                        postData.FormModel = JSON.stringify(that.DataFormModel);
                    }
                    $.post(url, postData, RES => {
                        if (RES.success) {
                            that.$message({
                                message: RES.message,
                                type: 'success'
                            });
                            that.InitSubscriptionTable();
                            that.DialogVisible = false;
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
        DialogCancelAddOrUpdate: function () {
            this.DataFormModel.AttributeID = [];
            this.resetFormModel();
            this.DialogVisible = false;
        }
    }
})
