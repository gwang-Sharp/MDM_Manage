"use strict";

var RoleManage = new Vue({
  el: '#RoleManage',
  data: {
    tableHeight: 300,
    //默认给300
    RoleTableData: [],
    //角色表格
    RoleSearchName: '',
    total: 1,
    //总条数
    pagenum: 1,
    // 当前页
    pagesize: 10,
    // 每页条数
    pageMaxsize: '',
    //当前页可以存放最大条数
    Title: '添加角色',
    //弹框标题
    AddRoledialogVisible: false,
    //添加角色弹框是否显示
    //添加用户角色form表单
    RoleForm: {
      RoleName: '',
      RoleDesc: ''
    },
    //验证Roleform表单规则
    rules: {
      RoleName: [{
        required: true,
        message: "请输入角色名称",
        trigger: 'blur'
      }],
      //动态给表格高度
      RoleDesc: [{
        required: true,
        message: "请输入角色描述",
        trigger: 'blur'
      }]
    },
    RoleTabLoading: false,
    pagenumBottom: '',
    EditOrAdd: 'Add'
  },
  created: function created() {},
  mounted: function mounted() {
    var _this = this;

    this.$nextTick(function () {
      _this.pagesize = parseInt((LayOutStaticWindowsHeight - 390) / 40);
      _this.tableHeight = parseInt(LayOutStaticWindowsHeight - 150);
      _this.pageMaxsize = _this.pagesize;

      _this.GetRoleTableList(); //this.UploadUserRolePermissions();

    });
  },
  methods: {
    //进入页面渲染表格
    GetRoleTableList: function GetRoleTableList() {
      this.RoleTabLoading = true;
      $.post('/System/SearchRole', {
        Where: this.RoleSearchName,
        Page: this.pagenum,
        limit: this.pagesize
      }, function (res) {
        RoleManage.RoleTableData = res.data;
        RoleManage.total = res.total;
        RoleManage.RoleTabLoading = false;
      }, 'json');
    },
    // 只要每页条数变化了, 触发
    handleSizeChange: function handleSizeChange(val) {
      // 更新每页条数
      RoleManage.pagesize = val; // 只要修改了每页条数, 数据所在的页码发生了变化了, 回到第一页

      RoleManage.pagenum = 1; // 重新渲染

      RoleManage.GetRoleTableList();
    },
    // 只要当前页变化时, 触发函数
    handleCurrentChange: function handleCurrentChange(val) {
      // 更新当前页
      RoleManage.pagenum = val; // 重新渲染

      RoleManage.GetRoleTableList();
    },
    //点击添加用户对话框
    OpenRoleDialog: function OpenRoleDialog() {
      RoleManage.AddRoledialogVisible = true;
      RoleManage.Title = '添加角色';
      RoleManage.EditOrAdd = "Add";
    },
    //点击edit 弹框 ,修改弹框名称,内容回显
    OpenRoleEditDialog: function OpenRoleEditDialog(index, row) {
      RoleManage.AddRoledialogVisible = true;
      RoleManage.$nextTick(function () {
        RoleManage.Title = '修改角色';
        RoleManage.EditOrAdd = "Edit";
        RoleManage.RoleForm.ID = row.id;
        RoleManage.RoleForm.RoleName = row.roleName;
        RoleManage.RoleForm.RoleDesc = row.roleDesc;
      });
    },
    //点击删除角色
    DelRole: function DelRole(row) {
      //console.log(row)
      RoleManage.$confirm("是否删除", "角色删除", {
        cancelButtonClass: "btn-custom-cancel",
        cancelButtonText: "取消",
        confirmButtonText: "确定",
        type: 'warning',
        callback: function callback(action, instance) {
          if (action === 'confirm') {
            var loading = RoleManage.$loading({
              lock: true,
              text: '加载中……',
              background: "rgba(255, 255, 255, 0.75)"
            });
            $.post('/System/DeleteRole', {
              id: row.id
            }, function (res) {
              loading.close();

              if (res.success == true) {
                RoleManage.GetRoleTableList();
                RoleManage.$message.success(res.message);
              } else {
                RoleManage.$message.error(res.message);
              }
            }, 'json');
          }
        }
      });
    },
    //关闭添加用户对话框或者修改用户对话框
    ClosedAddRoledialog: function ClosedAddRoledialog() {
      RoleManage.$nextTick(function () {
        RoleManage.AddRoledialogVisible = false;
        RoleManage.$refs.RoleForm.resetFields();
      });
    },
    //关闭添加用户对话框或者修改用户对话框
    CloseRoledialogVisible: function CloseRoledialogVisible(RoleForm) {
      RoleManage.$nextTick(function () {
        RoleManage.AddRoledialogVisible = false;
        RoleManage.$refs.RoleForm.resetFields(); //重置表单
      });
    },
    SubmitRoleForm: function SubmitRoleForm() {
      var _this2 = this;

      //提交角色表单
      if (RoleManage.EditOrAdd == 'Add') {
        RoleManage.$refs.RoleForm.validate(function (valid) {
          if (valid) {
            var loading = RoleManage.$loading({
              lock: true,
              text: '加载中……',
              background: "rgba(255, 255, 255, 0.75)"
            });
            $.post('/System/InserRole', _this2.RoleForm, function (res) {
              loading.close();

              if (res.success == true) {
                debugger;
                RoleManage.$nextTick(function () {
                  RoleManage.$message.success(res.message);
                  RoleManage.AddRoledialogVisible = false;
                  RoleManage.GetRoleTableList();
                  RoleManage.$refs.RoleForm.resetFields(); //重置表单
                });
              } else {
                RoleManage.$message.error(res.message);
                RoleManage.AddRoledialogVisible = false;
              }
            }, 'json');
          } else {
            return false;
          }
        });
      } else {
        RoleManage.$refs.RoleForm.validate(function (valid) {
          if (valid) {
            var loading = RoleManage.$loading({
              lock: true,
              text: '加载中……',
              background: "rgba(255, 255, 255, 0.75)"
            });
            $.post('/System/UpdateRole', _this2.RoleForm, function (res) {
              loading.close();

              if (res.success == true) {
                RoleManage.$nextTick(function () {
                  RoleManage.$message.success(res.message);
                  RoleManage.AddRoledialogVisible = false;
                  RoleManage.GetRoleTableList();
                  RoleManage.$refs.RoleForm.resetFields(); //重置表单
                });
              } else {
                RoleManage.$message.error(res.message);
                RoleManage.AddRoledialogVisible = false;
              }
            }, 'json');
          } else {
            return false;
          }
        });
      }
    },
    //  表头样式
    headerClass: function headerClass() {
      return 'text-align: center;background:#eef1f6;';
    },
    AddRole: function AddRole() {
      RoleManage.TitleType = 'Add';
      RoleManage.Add_Edit_Role();
    },
    //绑定回车事件
    EnterSearch: function EnterSearch() {
      $('#RoleKeyStr').bind('keydown', function (event) {
        if (event.keyCode == "13") {
          RoleManage.ReloadTable('Role');
        }
      });
    }
  },
  watch: {
    RoleSearchName: function RoleSearchName(val, oldval) {
      RoleManage.pagenum = 1;
      this.GetRoleTableList();
    }
  }
});