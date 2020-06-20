"use strict";

var UserVm = new Vue({
  el: "#UserVue",
  data: {
    UserTabLoading: false,
    UserstableData: [],
    FormModel: {
      Id: '',
      UserAccount: '',
      Type: '',
      ADID: '',
      UserName: '',
      UserPwd: '',
      Salt: '',
      Email: '',
      Icon: '',
      Validity: 1
    },
    searchName: '',
    pagination: {
      currentpage: 1,
      pagesizes: [10, 20, 30, 40],
      pagesize: 10,
      allNum: 0
    },
    RoleClassificationData: [],
    //角色设置表格
    dialogTitle: '添加用户',
    UserFromrules: {
      UserName: [{
        required: true,
        message: '请输入用户名',
        trigger: 'blur'
      }],
      UserAccount: [{
        required: true,
        message: '请输入用户账户',
        trigger: 'change'
      }],
      UserPwd: [{
        required: true,
        message: '请输入密码',
        trigger: 'change'
      }],
      Email: [{
        required: true,
        message: '请输入邮箱地址',
        trigger: 'change'
      }]
    },
    search_RoleName: '',
    hasSelectList: [],
    //已经选择的id组成的数组
    NowCheckUserID: '',
    //当前操作的用户ID
    Rolepagenum: 1,
    Rolepagesize: 8,
    Roletotall: 1,
    dialogVisible: false,
    RoleSettingDialogVisible: false,
    //角色设置面板
    AddHtmlLoading: false,
    multipleSelection: []
  },
  created: function created() {
    this.InitUserTable();
  },
  mounted: function mounted() {},
  methods: {
    //获取角色表格
    getRoleTableData: function getRoleTableData() {
      $.post('/System/GetRolByUserDataOrderByDesc', {
        ParmsId: UserVm.NowCheckUserID,
        Page: UserVm.Rolepagenum,
        limit: UserVm.Rolepagesize,
        Where: UserVm.search_RoleName
      }, function (res) {
        //console.log(res)
        if (res.success == true) {
          UserVm.RoleClassificationData = res.data;
          UserVm.Roletotall = res.total;
          UserVm.AddHtmlLoading = false;
          UserVm.$nextTick(function () {
            UserVm.$refs.RoleClassificationData.clearSelection();
            UserVm.RoleClassificationData.forEach(function (row) {
              if (UserVm.hasSelectList.indexOf(row.id) >= 0) {
                UserVm.$refs.RoleClassificationData.toggleRowSelection(row, true);
              }
            });
          });
        }
      }, 'json');
    },
    getRowKey: function getRowKey(row) {
      return row.id;
    },
    InitUserTable: function InitUserTable() {
      var that = this;
      this.UserTabLoading = true;
      $.post("/system/UserGet", {
        page: that.pagination.currentpage,
        rows: that.pagination.pagesize
      }, function (RES) {
        //console.log(RES);
        if (RES.success) {
          if (that.pagination.currentpage > 1 && RES.data.length == 0) {
            that.pagination.currentpage = 1;
            that.InitUserTable();
          } else {
            that.UserstableData = RES.data;
            that.pagination.allNum = RES.total;
          }

          that.UserTabLoading = false;
        } else {
          that.AttributestableData = [];
          that.pagination.allNum = 0;
        }
      });
    },
    //单选表格
    OnRoleTableSelect: function OnRoleTableSelect(selection, row) {
      // console.log(row);
      var index = UserVm.hasSelectList.indexOf(row.id);

      if (index < 0) {
        //如果不存在则添加
        UserVm.hasSelectList.push(row.id);
      } else {
        //如果存在则删除
        UserVm.hasSelectList.splice(index, 1);
      }
    },
    //AD表格全选选触发事件
    OnRoleTableSelectALL: function OnRoleTableSelectALL(val) {
      var ADIDData = [];
      this.RoleClassificationData.forEach(function (itme) {
        ADIDData.push(itme.id);
      }); //判断是否是全选 因为存在 reserve-selection属性 会保留之前操作的数据

      var index = -1;
      val.forEach(function (itme) {
        index = ADIDData.indexOf(itme.id);
        return index;
      }); //index -1 则是反选 index大于零 则是全选

      var status = "";

      if (index >= 0) {
        status = "add";
      } else {
        status = "delete";
      }

      this.JudgeIsCheck(status, ADIDData);
    },
    //改变设置角色分页表格每页大小
    RoleTablehandleSizeChange: function RoleTablehandleSizeChange(val) {
      UserVm.Rolepagenum = 1; // 角色设置对话框中的分页当前页

      UserVm.Rolepagesize = val; // 角色设置对话框中的分页每页条数

      UserVm.getRoleTableData();
    },
    //改变设置角色分页表格页数
    RoleTablehandleCurrentChange: function RoleTablehandleCurrentChange(val) {
      UserVm.Rolepagenum = val; // 角色设置对话框中的分页当前页

      UserVm.getRoleTableData();
    },
    //删除 添加选中项的角色
    JudgeIsCheck: function JudgeIsCheck(status, data, loding) {
      data.forEach(function (item) {
        var index = UserVm.hasSelectList.indexOf(item);

        if (status == "add") {
          if (index < 0) {
            //如果不存在则添加
            UserVm.hasSelectList.push(item);
          }
        } else {
          if (index >= 0) {
            //如果存在则删除
            UserVm.hasSelectList.splice(index, 1);
          }
        }
      });
    },
    headerClass: function headerClass() {
      return 'text-align: center;background:#eef1f6;';
    },
    RoleSettingHandleSelectionChange: function RoleSettingHandleSelectionChange(val) {
      this.multipleSelection = val; //console.log(this.multipleSelection)
    },
    //点击表格某一行触发，选中或不选中复选框
    OnRolehandleRowClick: function OnRolehandleRowClick(row, column, event) {
      this.$refs.RoleClassificationData.toggleRowSelection(row);
      var index = this.hasSelectList.indexOf(row.id);

      if (index < 0) {
        //如果不存在则添加
        this.hasSelectList.push(row.id);
      } else {
        //如果存在则删除
        this.hasSelectList.splice(index, 1);
      }
    },
    SaveRoleSetting: function SaveRoleSetting() {
      var UserID = UserVm.NowCheckUserID;
      var RoleID = UserVm.hasSelectList;
      var obj = {
        UserID: UserID,
        RoleID: RoleID
      };
      var loading = UserVm.$loading({
        lock: true,
        text: '加载中……',
        target: document.getElementById('ShareReportDiv'),
        background: "rgba(255, 255, 255, 0.75)"
      });
      $.post('/System/UserConfigRole', {
        RoleList: RoleID,
        UserID: UserID
      }, function (res) {
        loading.close();

        if (res.success) {
          UserVm.$message.success(res.message);
          UserVm.RoleSettingDialogVisible = false;
        } else {
          UserVm.$message.error(res.message);
        }
      });
    },
    onSearch: function onSearch() {
      if (!this.FormModel.UserName) {
        this.InitUserTable();
        return;
      } else {
        var that = this;

        if (that.pagination.currentpage > 1) {
          that.pagination.currentpage = 1;
        }

        $.post("/system/UserSearch", {
          name: that.FormModel.UserName,
          page: that.pagination.currentpage,
          rows: that.pagination.pagesize
        }, function (RES) {
          if (RES.success) {
            //that.$message({
            //    message: RES.message,
            //    type: 'success'
            //});
            that.UserstableData = RES.data;
            that.pagination.allNum = RES.total;
          } else {
            that.AttributestableData = [];
            that.pagination.allNum = 0;
          }
        });
      }
    },
    onCreate: function onCreate() {
      this.dialogTitle = "添加用户";
      this.dialogVisible = true;
    },
    handleSizeChange: function handleSizeChange(val) {
      this.pagination.pagesize = val;
      this.InitUserTable();
    },
    handleCurrentChange: function handleCurrentChange(val) {
      this.pagination.currentpage = val;
      this.InitUserTable();
    },
    resetFormModel: function resetFormModel() {
      this.$refs.FormModel.resetFields();
    },
    handleClose: function handleClose() {
      this.resetFormModel();
    },
    handleDelete: function handleDelete(index, row) {
      var id = row.id;
      var that = this;
      that.$confirm('此操作将将删除该用户, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(function () {
        $.post("/system/UserDel", {
          Id: id
        }, function (RES) {
          //console.log(RES);
          if (RES.success) {
            that.$message({
              message: RES.message,
              type: 'success'
            });
            that.InitUserTable();
          } else {
            that.$message.error(RES.message);
          }
        });
      })["catch"](function () {
        return;
      });
    },
    handleEdit: function handleEdit(index, row) {
      var _this = this;

      //console.log(row);
      this.dialogVisible = true;
      this.$nextTick(function () {
        _this.dialogTitle = "编辑用户";
        _this.FormModel.Id = row.id;
        _this.FormModel.UserAccount = row.userAccount;
        _this.FormModel.Type = row.type;
        _this.FormModel.ADID = row.aDID;
        _this.FormModel.UserName = row.userName;
        _this.FormModel.UserPwd = row.userPwd;
        _this.FormModel.Salt = row.salt;
        _this.FormModel.Email = row.email;
        _this.FormModel.Icon = row.icon;
        _this.FormModel.Validity = row.validity;
      });
    },
    AddOrUpdate: function AddOrUpdate() {
      var _this2 = this;

      UserVm.$refs.FormModel.validate(function (valid) {
        if (valid) {
          var that = _this2;
          var url = "";

          if (that.FormModel.Id) {
            url = "/system/UserUpdate";
          } else {
            url = "/system/UserAdd";
            that.FormModel.Id = 0;
          } //console.log(that.FormModel);


          $.post(url, {
            user: JSON.stringify(that.FormModel)
          }, function (RES) {
            //console.log(RES);
            if (RES.success) {
              that.$message({
                message: RES.message,
                type: 'success'
              });
              that.resetFormModel();
              that.dialogVisible = false;
              that.InitUserTable();
            } else {
              that.$message.error(RES.message);
            }
          });
        } else {
          return false;
        }
      });
    },
    CancelAddOrUpdate: function CancelAddOrUpdate() {
      this.resetFormModel();
      this.dialogVisible = false;
    },
    ShowRoleSettingDialogVisible: function ShowRoleSettingDialogVisible(row) {
      this.RoleSettingDialogVisible = true;
      this.AddHtmlLoading = true; //清空以选中的ID

      this.clearTableCheckOrTableData();
      this.NowCheckUserID = row.id;
      this.getCheckRoleList();
      this.getRoleTableData();
    },
    //获取表格选中的角色
    getCheckRoleList: function getCheckRoleList() {
      //$.post('/SystemManage/UserHaveRole', { UserID: UserVm.NowCheckUserID }, function (res) {
      //    //console.log(res)
      //    UserVm.hasSelectList = res.data
      //}, 'json');
      $.ajax({
        type: 'post',
        async: false,
        url: '/System/UserHaveRole',
        data: {
          UserId: UserVm.NowCheckUserID
        },
        dataType: 'json',
        success: function success(res) {
          if (res.success == true) {
            UserVm.hasSelectList = res.data;
          }
        }
      });
    },
    closeRoleSettingDialog: function closeRoleSettingDialog() {
      UserVm.clearTableCheckOrTableData();
    },
    //清空角色表格数据以及 查询框的值 和选中的值
    clearTableCheckOrTableData: function clearTableCheckOrTableData() {
      UserVm.RoleClassificationData = [];
      UserVm.search_RoleName = "";
      UserVm.hasSelectList = [];
      UserVm.Rolepagenum = 1;
    }
  },
  watch: {
    searchName: function searchName(val) {
      this.pagination.currentpage = 1;
      this.FormModel.UserName = val;
      this.onSearch();
    }
  }
});