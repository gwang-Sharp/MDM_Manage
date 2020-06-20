
var MergeRuleManage = new Vue({
    el: '#MergeRuleManage',
    data: {
        tableHeight: 300, //默认给300
        RuleBaseTableData: [],//角色表格
        total: 1,//总条数
        pagenum: 1, // 当前页
        pagesize: 10, // 每页条数
        WeightIndex: 0,
        GlobalWeight: 0,
        TabNumber:'first',
        pageMaxsize: '', //当前页可以存放最大条数
        Title: '添加角色',//弹框标题
        num: 0,
        RulesOfTheData: [],
        ModelName: "",
        slider: [15, 20],
        SliderFirstWidth:0,
        SliderSoncendWidth: 0,
        SliderThreeWidth: 0,
        RuleID:0,
        value: [1,3,4, 8,9,10],
        ModelSelectOptions: [],
        EntityName: "",
        IntNumber: 0,
        activeName:'first',
        AddAndSubtractState:0,
        EntitySelectOptions: [],
        AttributeSelectOptions: [],
        AddRuleBasedialogVisible: false,//添加角色弹框是否显示
        //添加用户角色form表单
        RuleBaseForm: {
            ID: '',
            ModelID: '',
            EntityID: '',
            AttributeID: '',
            RulesOfTheData:[],
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
        SetTheThresholddialogVisible: false,
        ResultHandlingdialogVisible: false,

      

       
    },
    created: function () {
    },
    mounted: function () {
        this.GetModelSelectData();
        this.$nextTick(() => {
            this.pagesize = parseInt((LayOutStaticWindowsHeight - 450) / 40);
            this.tableHeight = parseInt(LayOutStaticWindowsHeight - 250);
            this.pageMaxsize = this.pagesize;
            console.log(this.pageMaxsize );
            this.GetModelSelectData();
            this.GetRuleBaseTableList();
        });
    },
    methods: {
        GetModelSelectData: function () {
            $.post('/System/GetModelSelectRule', {}, res => {
                if (res.success) {
                    this.ModelSelectOptions = res.data;
                    this.DataModelSelectOptions = res.data;
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
        Getonmousedown: function (ev)
        {
            debugger
           
        },
        SetTheThresholddialog: function (row)
        {
            this.SetTheThresholddialogVisible = true;
            this.RuleID = row.id;
            this.SliderFirstWidth = parseInt(row.noProcessing);
            this.SliderSoncendWidth = parseInt(row.manual); 
            this.SliderThreeWidth = parseInt(row.selfMotion);
            this.slider = [this.SliderFirstWidth, this.SliderFirstWidth + this.SliderSoncendWidth];
            debugger
           
        },
        GetEntitySelectData: function (ModelID) {
            debugger
            $.post('/System/GetEntitySelectRule', { ModelID: ModelID }, res => {
                if (res.success) {

                    if (res.data.length > 0) {
                        this.EntitySelectOptions = res.data;
                       
                        this.EntityName = res.data[0].name;
                        this.RuleBaseForm.EntityID = res.data[0].id;
                        this.GetAttributeSelectData();
                    }
                    else {
                        this.EntityName = res.data[0].name;
                        this.RuleBaseForm.EntityID = 0;
                    }
                    this.GetRuleBaseTableList();
                }
            })
        },
        GetAttributeSelectData: function () {
            debugger
            $.post('/System/GetAttributeSelectRule', { EntityID: this.RuleBaseForm.EntityID }, res => {
                this.AttributeSelectOptions = [];
                if (res.success) {
                    this.AttributeSelectOptions = res.data;
                }
            })
        },
       
        //进入页面渲染表格
        GetRuleBaseTableList: function () {
            debugger
            this.RuleBaseTabLoading = true;
            $.post('/System/SearchMergingrules', {
                Where: this.RuleBaseForm.EntityID,
                Page: this.pagenum,
                limit: this.pagesize <= 1 ? 10 : this.pagesize
            },
                function (res) {
                    MergeRuleManage.RuleBaseTableData = res.data;
                    MergeRuleManage.total = res.total;
                    MergeRuleManage.RuleBaseTabLoading = false;
                }, 'json');
        },
        // 只要每页条数变化了, 触发
        handleSizeChange: function (val) {
            // 更新每页条数
            MergeRuleManage.pagesize = val;
            // 只要修改了每页条数, 数据所在的页码发生了变化了, 回到第一页
            MergeRuleManage.pagenum = 1;
            // 重新渲染
            MergeRuleManage.GetRoleTableList();
        },
        // 只要当前页变化时, 触发函数
        handleCurrentChange: function (val) {
            // 更新当前页
            MergeRuleManage.pagenum = val;
            // 重新渲染
            MergeRuleManage.GetRoleTableList();
        },
        //点击添加用户对话框
        OpenRuleBaseDialog: function () {

            MergeRuleManage.AddRuleBasedialogVisible = true;
            MergeRuleManage.Title = '添加合并规则';
            MergeRuleManage.EditOrAdd = "Add";
            this.CustomTypeIsShow = false;

        },
        //点击edit 弹框 ,修改弹框名称,内容回显
        OpenRuleBaseEditDialog: function (index, row) {
            debugger

            MergeRuleManage.AddRuleBasedialogVisible = true;
            MergeRuleManage.$nextTick(() => {
                MergeRuleManage.Title = '修改合并规则';
                MergeRuleManage.EditOrAdd = "Edit";
                MergeRuleManage.RuleBaseForm.ID = row.id;
                MergeRuleManage.RuleBaseForm.ModelID = row.modelID;
                MergeRuleManage.RuleBaseForm.EntityID = row.entityID;
                var IntNymber = 0;
                for (var i = 0; i < row.detailsItem.length.length; i++) {
                    IntNymber += row.detailsItem[i].weight
                }
                this.IntNumber = IntNymber;
                this.RulesOfTheData = [];
                for (var i = 0; i < row.detailsItem.length; i++) {

                    var Rul = MergeRuleManage.RulesOfTheData.length + 1;
                    var RulesItem =
                    {
                        Index: Rul,
                        AttributeName: row.detailsItem[i].name,
                        AttributeID: row.detailsItem[i].id,
                        Weight: row.detailsItem[i].weight,
                    };
                    this.RulesOfTheData.push(RulesItem);
                }
            });
        },
        //点击删除角色
        DelRuleBase: function (row) {
           
            MergeRuleManage.$confirm("是否删除", "业务规则删除", {
                cancelButtonClass: "btn-custom-cancel",
                cancelButtonText: "取消",
                confirmButtonText: "确定",
                type: 'warning',
                callback: function callback(action, instance) {
                    if (action === 'confirm') {
                        var loading = MergeRuleManage.$loading({
                            lock: true,
                            text: '加载中……',
                            background: "rgba(255, 255, 255, 0.75)"
                        });
                        $.post('/System/DeleteMergingrules', { RuleID: row.id }, function (res) {
                            loading.close();
                            if (res.success == true) {
                                MergeRuleManage.GetRuleBaseTableList();
                                MergeRuleManage.$message.success(res.message);
                            } else {
                                MergeRuleManage.$message.error(res.message);
                            }
                        }, 'json');
                    }
                }
            });
        },
        //关闭添加用户对话框或者修改用户对话框
        ClosedAddRuleBasedialog: function () {
            MergeRuleManage.$nextTick(() => {
                MergeRuleManage.AddRuleBasedialogVisible = false;
                MergeRuleManage.$refs.RuleBaseForm.resetFields();
                MergeRuleManage.RulesOfTheData = [];
            });
        },
        //关闭添加用户对话框或者修改用户对话框
        CloseRuleBasedialogVisible: function (RoleForm) {
            MergeRuleManage.$nextTick(() => {
                MergeRuleManage.AddRuleBasedialogVisible = false;
                MergeRuleManage.$refs.RuleBaseForm.resetFields();//重置表单
                MergeRuleManage.RulesOfTheData = [];
            });

        },
        SubmitRuleBaseForm: function () { //提交角色表单
            debugger
            if (this.IntNumber != 100 && this.RulesOfTheData.length!=0) {
                    MergeRuleManage.$message.error("权重总值必须等于100%");
            }
            else
            {
                var item = []
                for (var i = 0; i < this.RulesOfTheData.length; i++) {
                    if (this.RulesOfTheData[i].Weight > 0) {
                        item.push(this.RulesOfTheData[i]);
                    }
                }
                this.RuleBaseForm.RulesOfTheData = item;
                if (MergeRuleManage.EditOrAdd == 'Add') {
                    MergeRuleManage.$refs.RuleBaseForm.validate((valid) => {
                        if (valid) {
                            var loading = MergeRuleManage.$loading({
                                lock: true,
                                text: '加载中……',
                                background: "rgba(255, 255, 255, 0.75)"
                            });

                            $.post('/System/InsertMergingrules', this.RuleBaseForm, function (res) {
                                loading.close();
                                if (res.success == true) {
                                    debugger
                                    MergeRuleManage.$nextTick(() => {
                                        MergeRuleManage.$message.success(res.message);
                                        MergeRuleManage.AddRuleBasedialogVisible = false;
                                        MergeRuleManage.GetRuleBaseTableList();
                                        MergeRuleManage.$refs.RuleBaseForm.resetFields();//重置表单
                                        MergeRuleManage.RulesOfTheData = [];
                                    });
                                } else {
                                    MergeRuleManage.$message.error(res.message);
                                    MergeRuleManage.AddRuleBasedialogVisible = false;
                                }
                            }, 'json');
                        } else {
                            return false;
                        }
                    });
                } else {
                    MergeRuleManage.$refs.RuleBaseForm.validate((valid) => {
                        if (valid) {
                            var loading = MergeRuleManage.$loading({
                                lock: true,
                                text: '加载中……',
                                background: "rgba(255, 255, 255, 0.75)"
                            });
                            $.post('/System/EiteMergingrules', this.RuleBaseForm, function (res) {
                                loading.close();
                                if (res.success == true) {
                                    MergeRuleManage.$nextTick(() => {
                                        MergeRuleManage.$message.success(res.message);
                                        MergeRuleManage.AddRuleBasedialogVisible = false;
                                        MergeRuleManage.GetRuleBaseTableList();
                                        MergeRuleManage.$refs.RuleBaseForm.resetFields(); //重置表单
                                        MergeRuleManage.RulesOfTheData = [];
                                    });
                                } else {
                                    MergeRuleManage.$message.error(res.message);
                                    MergeRuleManage.AddRuleBasedialogVisible = false;
                                }
                            }, 'json');
                        } else {
                            return false;
                        }
                    });
                }
            }
          
        },
        //  表头样式
        headerClass: function () {
            return 'text-align: center;background:#eef1f6;'
        },
        AddRole: function () {
            MergeRuleManage.TitleType = 'Add';
            MergeRuleManage.Add_Edit_Role();
        },
        //绑定回车事件
        EnterSearch: function () {
            $('#RoleKeyStr').bind('keydown', function (event) {
                if (event.keyCode == "13") {
                    MergeRuleManage.ReloadTable('Role');
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

                    break;
            }
           
        },
        EntityHasChanged: function (ID) {
            debugger
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
                    this.GetFielValidationTableList();
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
                    this.RegularIs = true;
                    break;
                case "邮箱":
                    this.CustomTypeIsShow = false;
                    this.RegularIs = true;
                    break;
                case "自定义验证规则":
                    this.CustomTypeIsShow = true;
                    this.RegularIs = false;
                    break;
                default:
                    break;
            }

        },
        AddRuleBase: function ()  //添加数组 数据
        {
         
            if (this.IntNumber==0) {
                this.IntNumber = 100;
            }
            var Rul = MergeRuleManage.RulesOfTheData.length + 1;
            var IntNymber = 0;
            for (var i = 0; i < MergeRuleManage.RulesOfTheData.length; i++) {
                IntNymber += MergeRuleManage.RulesOfTheData[i].Weight
            }
            var RulesItem =
            {
                Index: Rul,
                AttributeName: '',
                TemporaryWeight: 100 - IntNymber, 
                AttributeID: 0,
                Weight: 100 - IntNymber,
            };
            this.RulesOfTheData.push(RulesItem);
           
        },
        DelRule: function (item)  //删除数组数据
        {
            for (var i = 0; i < MergeRuleManage.RulesOfTheData.length; i++) {
                if (MergeRuleManage.RulesOfTheData[i].Index === item.Index) {
                    MergeRuleManage.RulesOfTheData.splice(i, 1);
                }
            }
        },
        AttributeNameDialogChange: function (val, index) {
            for (var i = 0; i < MergeRuleManage.RulesOfTheData.length; i++) {
                if (MergeRuleManage.RulesOfTheData[i].Index === index) {
                    MergeRuleManage.RulesOfTheData[i].AttributeID = val;
                }
            }
          
        },
        NumberhandleChange: function (currentValue, oldValue) {
            var IntNymber = 0;
            for (var i = 0; i < MergeRuleManage.RulesOfTheData.length; i++) {
                IntNymber += MergeRuleManage.RulesOfTheData[i].Weight
            }
            this.IntNumber = IntNymber;

        },
        ChangeThreshold: function ()
        {
            if (this.SliderFirstWidth + this.SliderThreeWidth + this.SliderSoncendWidth != 100) {
                MergeRuleManage.$message.error("阀值总和必须等于100%");
            }
            else
            {
                var Parmas = {
                    Id: this.RuleID,
                    NoProcessing: this.SliderFirstWidth,
                    SelfMotion: this.SliderThreeWidth,
                    Manual: this.SliderSoncendWidth
                }
                var loading = MergeRuleManage.$loading({
                    lock: true,
                    text: '加载中……',
                    background: "rgba(255, 255, 255, 0.75)"
                });

                $.post('/System/ChangeThreshold', Parmas, function (res) {
                    loading.close();
                    if (res.success == true) {
                        debugger
                        MergeRuleManage.$nextTick(() => {
                            MergeRuleManage.$message.success(res.message);
                            MergeRuleManage.SetTheThresholddialogVisible = false;
                            MergeRuleManage.GetRuleBaseTableList();
                            $("canvas").remove();

                        });
                    } else {
                        MergeRuleManage.$message.error(res.message);
                        MergeRuleManage.SetTheThresholddialogVisible = false;
                    }
                }, 'json');
            }
           
        },
        SliderChange: function (val)
        {
            debugger
           
            MergeRuleManage.SliderFirstWidth = val[0];
            MergeRuleManage.SliderSoncendWidth = val[1] - val[0];
            MergeRuleManage.SliderThreeWidth=100-val[1]

           
        },
       
    },
    watch: {
        RoleSearchName(val, oldval) {
            MergeRuleManage.pagenum = 1;
            this.GetRoleTableList();
        }
    }
});