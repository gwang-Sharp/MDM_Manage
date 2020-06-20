var NavManage = new Vue({
    el: '#NavManage',
    data: {
        search: '', //模糊查询
        MenuTableData: [], //  表格数据 
        Pagination: {  //  分页
            limit: 10,
            page: 1,
            pageSizes: [10, 20, 30],
            total: 0
        },
        RoleTabLoading: false, // 加载条
        tableHeight: 300,
        id: '',
        AddPanelStarts: 1,    //添加面板的状态 1  添加  2  编辑
        dialogTitle: '添加菜单',
        AddMenudialogVisible: false,
        MenuForm: {
            Name: '',//中文名
            Address: '',//地址
            Sorting: '',//排序号
            Describ: '',//描述
            ParentNavCode: 1,
            ConfirmDelete: '',
            id: '',
            Grade: '',
            NavCodeID: '',
            addOrUpdate: 0,
            OneADD: 3,
        },
        rules: {    //   表单验证
            Sorting: [{ required: true, message: "请输入排序号", trigger: 'blur' }],
            Name: [{ required: true, message: "请输入菜单名称", trigger: 'blur' }],
            Address: [{ required: true, message: "请输入菜单地址", trigger: 'blur' }],
            Describ: [{ required: true, message: "请输入菜单描述", trigger: 'blur' }],
            ConfirmDelete: [{ required: true, message: "请选择是否允许删除", trigger: 'change' }]
        },
    },
    mounted: function () {
        this.GetMenuList();
    },
    methods: {
        //获取菜单数据
        GetMenuList() {
            this.RoleTabLoading = true;
            $.post("/System/SearchMenu", function (res) {
                NavManage.MenuTableData = res.data
                //console.log(NavManage.MenuTableData);
                NavManage.RoleTabLoading = false;
            }, 'json')
        },
        // 打开添加或者编辑面板
        openMenuDialog: function (data, addOrUpdate) {
            this.AddMenudialogVisible = true;
            if (addOrUpdate == 1) {
                NavManage.dialogTitle = "添加菜单";
                this.MenuForm.ParentNavCode = data.navCode;
                this.AddPanelStarts = 1;
            }
            else if (addOrUpdate == 0) {
                NavManage.dialogTitle = "添加菜单";
                this.MenuForm.ParentNavCode = "1";
                this.AddPanelStarts = 1;
            }
            else {
                NavManage.$nextTick(() => {
                    NavManage.dialogTitle = "编辑菜单";
                    NavManage.MenuForm.id = data.id;
                    NavManage.MenuForm.Name = data.navName;
                    NavManage.MenuForm.Address = data.navURL;
                    NavManage.MenuForm.Sorting = data.sequenceNo;
                    NavManage.MenuForm.Describ = data.navDesc;
                    NavManage.MenuForm.Grade = data.grade;
                    NavManage.MenuForm.ParentNavCode = 1;
                    this.AddPanelStarts = 2;
                    NavManage.MenuForm.ConfirmDelete = '0';
                });
            }
        },
        // 删除菜单
        DelMenu: function (row) {
            this.$confirm('此操作将永久删除改菜单，是否继续？', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning',
                callback: (action, instance) => {
                    if (action === 'confirm') {
                        var NavRuleParams =
                        {
                            Id: row.id,
                        };
                        $.post('/System/DeleteMenu', NavRuleParams, res => {
                            if (res.success) {
                                this.$message.success(res.message);
                                this.GetMenuList();
                                this.AddMenudialogVisible = false;
                            } else
                                this.$message.error(res.message);
                        });
                    }
                }
            })
        },
        // 关闭面板
        ClosedialogVisible() {

            this.AddMenudialogVisible = false;
            this.$refs.MenuForm.resetFields();

        },
        // 添加菜单打开面板
        AddNav: function () {
            this.AddMenudialogVisible = true;
        },
        // 添加提交
        AddMenu: function (MenuForm) {
            NavManage.$refs.MenuForm.validate((valid) => {
                if (valid) {
                    var url;
                    switch (this.AddPanelStarts) {
                        case 1:
                            url = '/System/InserMenu';
                            break;
                        case 2:
                            url = '/System/UpdateMenu';
                            break;
                        default:
                    }
                    $.post(url, this.MenuForm, function (res) {

                        if (res.success == true) {
                            NavManage.$nextTick(() => {
                                NavManage.AddMenudialogVisible = false;
                                NavManage.$refs.MenuForm.resetFields();
                                NavManage.GetMenuList();
                                NavManage.$message.success(res.message);
                            });
                        } else {
                            NavManage.$message.error(res.message);
                        }
                    }, 'json')
                }
                else {
                    return false;
                }
            })
        },
        //表头样式
        headerClass: function () {
            return 'text-align: center;background:#eef1f6;'
        },
    },


})