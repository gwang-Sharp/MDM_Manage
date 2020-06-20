"use strict";

var AssociateRole = new Vue({
  el: "#AssociateRole",
  data: {
    activeName: 'first',
    activeNamess: 'ninet',
    tableData: [],
    where: '',
    tableDataCount: 0,
    defaultProps: {
      children: 'children',
      label: 'label'
    },
    CubedialogMannageTitle: '',
    dialogFormVisible: false,
    IsTableShow: false,
    IsFenYeShow: false,
    checked: false,
    //cube 是否全选
    IsDisPlaycheckbox: false,
    //cube复选框是否显示
    radio: '1',
    dialogRoloVisible: false,
    AddRepoerIfarme: false,
    AddCubeIfarme: false,
    AddTabularIfarme: false,
    AddMenuIfarme: false,
    AddUplodeIfarme: false,
    AddFolderIfarme: false,
    ALLchecked: false,
    TableUserLenghts: 0,
    diaJobJobVisible: false,
    IsAllUser: 0,
    AssignUsersPagation: {
      length: 0,
      total: 0
    },
    RoleQueryParameters: {
      page: 1,
      limit: 10,
      where: ''
    },
    UserUserName: '',
    CubeSaveType: false,
    ModifyRoleIdList: {},
    //角色集合
    ReportPermissionTreeId: '',
    //选中报表权树节点的Id
    TabNumber: '',
    ReportPermissionTreeDisAble: '',
    ReportPermissionTree: [],
    //报表权限树集合
    ReportPermissionTreeProps: {
      // 报表权限树绑定
      children: 'children',
      label: 'label'
    },
    MenuPermissionTreeID: '',
    //选中菜单树节点的Id
    MenuPermissionTree: [],
    // 菜单权限树集合
    MenutPermissionTreeProps: {
      // 菜单权限树绑定
      children: 'children',
      label: 'label'
    },
    Buttondisabled: true,
    //提交按钮
    UploadPermissionTreeID: '',
    UploadPermissionTree: [],
    //  上传树集合
    UploadPermissionTreeProps: {
      // 菜单权限树绑定
      children: 'children',
      label: 'label'
    },
    DistributionUserTableDataId: '',
    DistributionUserTableData: [],
    //分配用户集合
    UserQueryParameters: {
      page: 1,
      limit: 10,
      where: ''
    },
    UsertableDataCount: 0,
    FolderPermissionTreeID: '',
    FolderPermissionTree: [],
    //文件夹权限数据集合
    FolderPermissionTreeProps: {
      children: 'children',
      label: 'label'
    },
    TabularOptions: [],
    //级联选择器数据
    TabularValue: [],
    CubeValue: [],
    CubeOptions: [],
    //cube表格数据集合
    getFolderpLoadeleUTreeParms: {
      DataSource: '',
      Catalog: '',
      Cube_Name: '',
      Dimension_Name: '',
      Attribute_Name: '',
      page: 1,
      limit: 10,
      RoleId: '',
      Where: '',
      Status: ''
    },
    //获取cube数据得到的参数
    CubeTableData: [],
    //cube表格数据集合
    CubeCount: 0,
    //cube表格数据长度
    EidCubeDataRolParms: {
      Description: '',
      MEMBERKEY: '',
      MEMBERNAME: '',
      MEMBERUNIQUENAME: '',
      status: ''
    },
    RoltableData: [10, 20, 30, 40],
    TemporaryStorageData: [],
    CheckedBoxRolTable: [],
    ParmseTableTypeID: '',
    TabularID: '',
    TabularLabel: '',
    TabularRolLoading: false,
    GetTabularDataParms: {
      Instance: '',
      DataBase: '',
      Table: '',
      Filed: '',
      page: 1,
      limit: 10,
      where: ''
    },
    TabularTableData: [],
    IsTabularTableShow: false,
    IsTabularTableCount: 0,
    TabularClickRowName: '',
    SearchCubeName: '',
    SearchTabularName: '',
    ReportRoleManagesTheJSON: [],
    OverallSituationID: '',
    Rolloading: false,
    tableHeight: 0,
    rolePageSize: 0,
    roleTableHeight: 0,
    pagesize: 0,
    pageMaxsize: 0,
    SqlJobsData: [],
    //  sqljson数据集合
    SqlJobsDataCount: 0,
    SqlJobsDataPames: {
      page: 1,
      limit: 0,
      where: ''
    },
    JobISloading: false,
    //搜索
    MeunPermissionsNameWhere: '',
    SearchReportName: '',
    SearchUploadName: '',
    SearchFolderName: '',
    SearchJobWhere: '',
    ReportPermissionLoading: false,
    CubePermissionLoading: false,
    TabularPermissionLoading: false,
    BoxLoading: false,
    AddHtmlLoading: false,
    //添加用户laoding
    AddRepoerHtmlLoading: false,
    //添加报表loading
    AddCubeHtmlLoading: false,
    //添加cubeloading
    AddMenuHtmlLoading: false,
    //添加菜单
    AddUplodHtmlLoading: false,
    //上传
    AddJobHtmlLoading: false,
    //job
    AddRolHtmlLoading: false,
    //角色
    rolePageMaxSize: 0,
    screenHeight: "",
    MenuPermissionTreeLoading: false,
    UploadLoading: false,
    FolderLoading: false,
    UserTableTable: false //user 数据loadg

  },
  created: function created() {
    this.Rolloading = true;
    this.tableHeight = parseInt(LayOutStaticWindowsHeight - 250);
    this.roleTableHeight = parseInt(LayOutStaticWindowsHeight - 252);
    this.rolePageSize = parseInt((LayOutStaticWindowsHeight - 320) / 40);
    this.pagesize = parseInt((LayOutStaticWindowsHeight - 270) / 40);
    this.CubePageSize = parseInt((LayOutStaticWindowsHeight - 300) / 40);
    this.SqlJobsDataPames.limit = parseInt((LayOutStaticWindowsHeight - 310) / 40);
    this.pageMaxsize = this.pagesize;
    this.rolePageMaxSize = this.rolePageSize;
    this.screenHeight = parseInt(LayOutStaticWindowsHeight - 240) + "px"; //console.log(this.tableHeight);
    //console.log()

    this.GetListRoles(1, '', 1);
    this.GetUsetDataTableList(1);
  },
  methods: {
    //获取用户分配数据请求
    GetUsetDataTableList: function GetUsetDataTableList(type) {
      var QueryParameters;

      if (type == 1) {
        QueryParameters = {
          page: 1,
          limit: this.pagesize,
          where: '',
          RoleID: '1'
        };
      } else {
        QueryParameters = {
          page: AssociateRole.UserQueryParameters.page,
          limit: AssociateRole.pagesize,
          where: AssociateRole.UserQueryParameters.where,
          RoleID: '1'
        };
      }

      this.UserTableTable = true; //console.log("loading::"+ AssociateRole.Rolloading);

      $.post('/system/UserGet', {
        page: QueryParameters.page,
        rows: QueryParameters.limit,
        where: QueryParameters.where
      }, function (res) {
        console.log(res.data);

        if (res.success) {
          AssociateRole.DistributionUserTableData = res.data;
          AssociateRole.UsertableDataCount = res.total;
        }

        AssociateRole.UserTableTable = false;
        AssociateRole.Rolloading = false;
      }, 'json');
    },
    //获取菜单层级结构数据请求
    GetMenuPermissionTree: function GetMenuPermissionTree() {
      this.MenuPermissionTreeLoading = true;
      $.post('/System/SearchMenu', {}, function (res) {
        AssociateRole.MenuPermissionTree = res.data;
        AssociateRole.MenuPermissionTreeLoading = false;
      }, 'json');
    },
    getRowKey: function getRowKey(row) {
      return row.id;
    },
    //角色table获取表格数据
    GetListRoles: function GetListRoles(type, ParmsId, types) {
      this.Rolloading = true;
      var url;

      if (types == 1) {
        url = '/System/RoleRelation';
      } else if (types == 2) {
        url = '/SystemManage/GetRolByCubeOrderByDesc';
      } else if (types == 0) {
        url = '/System/GetRolByUserDataOrderByDesc';
      } else if (types == 4) {
        url = '/System/GetMenuNodeOrderByDesc';
      }

      var QueryParameters;

      if (type == 1 && types != 3) {
        QueryParameters = {
          Page: 1,
          limit: this.rolePageSize,
          Where: this.where,
          ParmsId: ParmsId
        };
      } else {
        QueryParameters = {
          Page: AssociateRole.RoleQueryParameters.page,
          limit: this.rolePageSize,
          where: this.where,
          ParmsId: ParmsId
        };
      }

      $.post(url, QueryParameters, function (res) {
        if (res.success) {
          AssociateRole.tableData = [];
          AssociateRole.$nextTick(function () {
            AssociateRole.$refs.table.clearSelection();
            AssociateRole.tableData = res.data;
            AssociateRole.tableDataCount = res.total;

            if (AssociateRole.tableDataCount <= AssociateRole.CheckedBoxRolTable.length) {
              AssociateRole.ALLchecked = true;
            } else {
              AssociateRole.ALLchecked = false;
            }

            AssociateRole.AssignUsersPagation.total = res.total;
            debugger;

            if (AssociateRole.CheckedBoxRolTable != null) {
              AssociateRole.tableData.forEach(function (e) {
                if (AssociateRole.CheckedBoxRolTable.indexOf(e.id) >= 0) {
                  AssociateRole.$refs.table.toggleRowSelection(e, true);
                }
              });
            }
          });
        }

        AssociateRole.Rolloading = false;
      }, 'json');
    },
    //角色table分页改变每页条数
    TableDataSize_change: function TableDataSize_change(valSaze) {
      AssociateRole.RoleQueryParameters.limit = valSaze;
      this.pagesize = valSaze;
      AssociateRole.GetListRoles(0);
    },
    //角色table分页改变页数
    handleCurrentChange: function handleCurrentChange(val) {
      AssociateRole.RoleQueryParameters.page = val;

      if (AssociateRole.TabNumber == 'first' || AssociateRole.TabNumber == '') {
        AssociateRole.GetListRoles(0, AssociateRole.ParmseTableTypeID, 0);
      } else if (AssociateRole.TabNumber == 'Eignt') {
        AssociateRole.GetListRoles(0, AssociateRole.ParmseTableTypeID, 4);
      }
    },
    //点击角色table Row复选框
    selectCheckBox: function selectCheckBox(selection, row) {
      var selected = selection.length && selection.indexOf(row) !== -1; ////selected为 true就是选中，0或者false是取消选中

      AssociateRole.ReportRoleManagesTheJSON.forEach(function (res) {
        if (AssociateRole.ReportPermissionTreeId === res.ID) {
          if (selected === true) {
            //勾选复选框
            if (res.RolList.indexOf(row.id + '') < 0) {
              res.RolList.push(row.id + '');
              res.isEit = true;
            }
          } else {
            //取消勾选复选框
            if (res.RolList.indexOf(row.id + '') > -1) {
              res.RolList.splice(res.RolList.indexOf(row.id + ''), 1);
              res.isEit = true;
            }
          }
        }
      });

      if (selected === true) {
        //勾选复选框
        if (this.CheckedBoxRolTable.indexOf(row.id + '') < 0) {
          this.CheckedBoxRolTable.push(row.id + '');
        }
      } else {
        //取消勾选复选框
        if (this.CheckedBoxRolTable.indexOf(row.id + '') > -1) {
          this.CheckedBoxRolTable.splice(this.CheckedBoxRolTable.indexOf(row.id + ''), 1);
        }
      }

      if (AssociateRole.tableDataCount === AssociateRole.CheckedBoxRolTable.length) {
        AssociateRole.ALLchecked = true;
      } else {
        AssociateRole.ALLchecked = false;
      }

      IsHasPermissionsData = true;
    },
    //点击角色table表头复选框
    selectCheckBox_ALL: function selectCheckBox_ALL(selection) {
      var _this = this;

      IsHasPermissionsData = true;
      var selectedall = false; ////selected为 true就是选中，false是取消选中

      selection.forEach(function (e) {
        selectedall = _this.tableData.indexOf(e) !== -1;
      });
      AssociateRole.ReportRoleManagesTheJSON.forEach(function (res) {
        if (AssociateRole.ReportPermissionTreeId === res.ID) {
          if (selectedall === true) {
            //勾选复选框
            selection.forEach(function (e) {
              if (res.RolList.indexOf(e.id + '') < 0) {
                res.RolList.push(e.id + '');
                res.isEit = true;
              }
            });
          } else {
            //取消勾选复选框
            _this.tableData.forEach(function (e) {
              if (res.RolList.indexOf(e.id + '') > -1) {
                res.RolList.splice(res.RolList.indexOf(e.id + ''), 1);
                res.isEit = true;
              }
            });
          }
        }
      });

      if (selectedall) {
        selection.forEach(function (e) {
          if (_this.CheckedBoxRolTable.indexOf(e.id + '') < 0) {
            _this.CheckedBoxRolTable.push(e.id + '');
          }
        });
      } else {
        this.tableData.forEach(function (e) {
          if (_this.CheckedBoxRolTable.indexOf(e.id + '') > -1) {
            _this.CheckedBoxRolTable.splice(_this.CheckedBoxRolTable.indexOf(e.id + ''), 1);
          }
        });
      }

      if (AssociateRole.tableDataCount === AssociateRole.CheckedBoxRolTable.length) {
        AssociateRole.ALLchecked = true;
      } else {
        AssociateRole.ALLchecked = false;
      }
    },
    //提交修改权限角色
    SaveRoleData: function SaveRoleData() {
      if (AssociateRole.TabNumber == 'Eignt') {
        if (AssociateRole.MenuPermissionTreeID != null && AssociateRole.MenuPermissionTreeID != "") {
          var RolIdList = [];
          AssociateRole.ReportRoleManagesTheJSON.forEach(function (value) {
            if (value.isEit == true) {
              RolIdList.push(value);
            }
          });

          if (RolIdList != null && RolIdList[0] != null) {
            AssociateRole.BoxLoading = true;
            $.post('/System/ModifMenuRole', {
              FilNoList: RolIdList
            }, function (res) {
              if (res.success == true) {
                AssociateRole.Rolloading = false;
                AssociateRole.$message({
                  type: 'success',
                  message: res.message
                });
                AssociateRole.TemporaryStorageData = AssociateRole.ReportRoleManagesTheJSON;
                AssociateRole.ReportRoleManagesTheJSON = [];
                AssociateRole.GetListRoles(1, AssociateRole.ReportPermissionTreeId, 4);
                AssociateRole.RoleQueryParameters.page = 1;
                IsHasPermissionsData = false;
              } else {
                AssociateRole.$message({
                  type: 'error',
                  message: res.message
                });
              }

              AssociateRole.BoxLoading = false;
            }, 'json');
          } else {
            AssociateRole.$message({
              message: "提交成功,未做任何修改",
              type: 'success'
            });
          }
        } else {
          AssociateRole.$message("请选择菜单后再修改权限");
        }
      } else if (AssociateRole.TabNumber == 'first' || AssociateRole.TabNumber == '') {
        if (AssociateRole.DistributionUserTableDataId != null && AssociateRole.DistributionUserTableDataId != "") {
          var RolIdList = [];
          AssociateRole.ReportRoleManagesTheJSON.forEach(function (value) {
            if (value.isEit == true) {
              RolIdList.push(value);
            }
          });

          if (RolIdList != null && RolIdList.length > 0) {
            AssociateRole.BoxLoading = true;
            $.post('/System/ModifUserRole', {
              FilNoList: RolIdList
            }, function (res) {
              if (res.success == true) {
                AssociateRole.Rolloading = false; // if (res.otherMsg!=null && res.otherMsg.success == true) {

                AssociateRole.$message({
                  type: 'success',
                  message: res.message
                });
                IsHasPermissionsData = false; //}
                //else {
                //AssociateRole.$message({
                // type: 'warning',
                //message: $.i18n.prop("PermissionsWere")
                //});
                //}

                AssociateRole.TemporaryStorageData = AssociateRole.ReportRoleManagesTheJSON;
                AssociateRole.ReportRoleManagesTheJSON = [];
                AssociateRole.RoleQueryParameters.page = 1;
                AssociateRole.GetListRoles(0, AssociateRole.ReportPermissionTreeId, 0); //AssociateRole.GetUsetDataTableList()
              } else {
                AssociateRole.Rolloading = false;
                AssociateRole.$message({
                  type: 'error',
                  message: res.message
                });
              }

              AssociateRole.BoxLoading = false;
            }, 'json');
          } else {
            AssociateRole.$message({
              message: "提交成功，未做任何修改",
              type: 'success'
            });
          }
        } else {
          AssociateRole.$message("请先选择用户再进行权限分配");
        }
      }
    },
    ///获取tab便签name 请求该tab数据
    tab_click: function tab_click(tabnode) {
      var _this2 = this;

      //console.log(tabnode.paneName);
      this.IsDisPlaycheckbox = false;

      if (AssociateRole.ReportRoleManagesTheJSON != null && AssociateRole.ReportRoleManagesTheJSON[0] != null) {
        var item = [];
        AssociateRole.ReportRoleManagesTheJSON.forEach(function (res) {
          if (res.isEit == true) {
            item.push(res);
          }
        }); //console.log(item);

        if (item != null && item[0] != null) {
          this.$confirm("修改权限还未保存,是否提交保存", "提示", {
            cancelButtonClass: "btn-custom-cancel",
            cancelButtonText: "放弃",
            confirmButtonText: "提交",
            type: 'warning',
            callback: function callback(action) {
              if (action === 'confirm') {
                AssociateRole.SaveRoleData();
                AssociateRole.TabNumber = tabnode.paneName;
                AssociateRole.Buttondisabled = true;
                AssociateRole.tableData.forEach(function (e) {
                  AssociateRole.$refs.table.toggleRowSelection(e, false);
                });
                AssociateRole.ReportRoleManagesTheJSON = [];

                if (tabnode.paneName == 'Eignt') {
                  AssociateRole.GetMenuPermissionTree();
                } else if (tabnode.paneName == 'first') {
                  AssociateRole.GetUsetDataTableList(1);
                }
              } else {
                AssociateRole.ReportRoleManagesTheJSON = [];

                _this2.$message({
                  type: 'info',
                  message: $.i18n.prop("hasTogiveup")
                });

                AssociateRole.TabNumber = tabnode.paneName;
                AssociateRole.Buttondisabled = true;
                AssociateRole.tableData.forEach(function (e) {
                  AssociateRole.$refs.table.toggleRowSelection(e, false);
                });
              }
            }
          });
        } else {
          AssociateRole.TabNumber = tabnode.paneName;
          AssociateRole.Buttondisabled = true;
          AssociateRole.tableData.forEach(function (e) {
            AssociateRole.$refs.table.toggleRowSelection(e, false);
          });
          AssociateRole.ReportRoleManagesTheJSON = [];

          if (tabnode.paneName == 'Eignt') {
            AssociateRole.GetMenuPermissionTree();
          } else if (tabnode.paneName == 'first') {
            AssociateRole.GetUsetDataTableList(1);
            AssociateRole.Rolloading = false;
          }
        }
      } else {
        AssociateRole.TabNumber = tabnode.paneName;
        AssociateRole.Buttondisabled = false;
        AssociateRole.tableData.forEach(function (e) {
          AssociateRole.$refs.table.toggleRowSelection(e, false);
        });
        AssociateRole.ReportRoleManagesTheJSON = [];

        if (tabnode.paneName == 'Eignt') {
          AssociateRole.GetMenuPermissionTree();
        } else if (tabnode.paneName == 'first') {
          AssociateRole.GetUsetDataTableList(1);
          AssociateRole.Rolloading = false;
        }
      }
    },
    //点击菜单获取角色被选中
    MenuPermissionTreeNodeClick: function MenuPermissionTreeNodeClick(even, node, de) {
      if (node.data != null) {
        AssociateRole.MenuPermissionTreeID = node.data.id; //if (AssociateRole.MenuPermissionTreeID == 1) {
        //    AssociateRole.Buttondisabled = true;
        //    AssociateRole.tableData.forEach(row => {
        //            AssociateRole.$refs.table.toggleRowSelection(row, false);
        //    })
        //    return;
        //}

        AssociateRole.Buttondisabled = false;
        AssociateRole.Rolloading = true;
        AssociateRole.RoleQueryParameters.page = 1;
        AssociateRole.ParmseTableTypeID = node.data.id;
        AssociateRole.ReportPermissionTreeId = node.data.id;
        var type = 0;

        if (AssociateRole.ReportRoleManagesTheJSON != null) {
          AssociateRole.ReportRoleManagesTheJSON.forEach(function (res) {
            if (res.ID === node.data.id && res.isEit == true) {
              AssociateRole.CheckedBoxRolTable = res.RolList;
              type = 1;
              AssociateRole.$nextTick(function () {
                AssociateRole.GetListRoles(1, node.data.id, 1);
              });
              return;
            }
          });
        }

        if (type == 0) {
          $.post('/System/GetMenuNodeCheckRoleList', {
            MenuId: node.data.id
          }, function (res) {
            if (res != null) {
              if (AssociateRole.ReportRoleManagesTheJSON[0] == null) {
                var json = {
                  ID: node.data.id,
                  RolList: res,
                  isEit: false
                };
                AssociateRole.ReportRoleManagesTheJSON.push(json);
              } else {
                var item = [];

                for (var i = 0; i < AssociateRole.ReportRoleManagesTheJSON.length; i++) {
                  item.push(AssociateRole.ReportRoleManagesTheJSON[i].ID);
                }

                if (item.indexOf(node.data.id) < 0) {
                  var json = {
                    ID: node.data.id,
                    RolList: res,
                    isEit: false
                  };
                  AssociateRole.ReportRoleManagesTheJSON.push(json);
                }
              }

              AssociateRole.CheckedBoxRolTable = res;
              AssociateRole.ModifyRoleIdList = res;
              AssociateRole.GetListRoles(1, node.data.id, 4);
            }
          }, 'json');
        }
      }
    },
    //用户分页改变页码
    UserhandleCurrentChange_row: function UserhandleCurrentChange_row(val) {
      AssociateRole.UserQueryParameters.page = val;
      AssociateRole.GetUsetDataTableList(0);
    },
    //用户分页改变条数
    UserTableDataSize_changesize: function UserTableDataSize_changesize(valSaze) {
      AssociateRole.UserQueryParameters.limit = valSaze;
      this.pagesize = valSaze;
      AssociateRole.GetUsetDataTableList(0);
    },
    //获取用户对应的角色权限
    UserTablehandleCurrentChange: function UserTablehandleCurrentChange(row, column, event) {
      //console.log(row);
      if (row != null) {
        AssociateRole.Buttondisabled = false;
        AssociateRole.Rolloading = true;
        AssociateRole.DistributionUserTableDataId = row.id;
        AssociateRole.ReportPermissionTreeId = row.id;
        AssociateRole.ParmseTableTypeID = row.id;
        var type = 0;

        if (AssociateRole.ReportRoleManagesTheJSON != null && AssociateRole.ReportRoleManagesTheJSON[0] != null) {
          //console.log(AssociateRole.ReportRoleManagesTheJSON[0]);
          AssociateRole.ReportRoleManagesTheJSON.forEach(function (res) {
            if (res.ID === row.id && res.isEit == true) {
              AssociateRole.CheckedBoxRolTable = res.RolList;
              type = 1;
              AssociateRole.$nextTick(function () {
                AssociateRole.GetListRoles(0, row.id, 0);
                AssociateRole.$nextTick(function () {
                  AssociateRole.Rolloading = false;
                });
              });
              return;
            }
          });
        }

        if (type == 0) {
          $.post('/System/GetUserAllCheckedRole', {
            UserID: row.id
          }, function (res) {
            if (res != null) {
              if (AssociateRole.ReportRoleManagesTheJSON[0] == null) {
                var json = {
                  ID: row.id,
                  RolList: res,
                  isEit: false
                };
                AssociateRole.ReportRoleManagesTheJSON.push(json);
              } else {
                var item = [];

                for (var i = 0; i < AssociateRole.ReportRoleManagesTheJSON.length; i++) {
                  item.push(AssociateRole.ReportRoleManagesTheJSON[i].ID);
                }

                if (item.indexOf(row.id) < 0) {
                  var json = {
                    ID: row.id,
                    RolList: res,
                    isEit: false
                  };
                  AssociateRole.ReportRoleManagesTheJSON.push(json);
                }
              }

              AssociateRole.CheckedBoxRolTable = res;
              AssociateRole.ModifyRoleIdList = res;
              AssociateRole.GetListRoles(0, row.id, 0);
            }

            AssociateRole.Rolloading = false;
          }, 'json');
        }
      }
    },
    UserTableRowClass: function UserTableRowClass(row, rowIndex) {
      if (AssociateRole.selectedIndex === rowIndex) {
        return {
          "background-color": " #c2d6ea !important;"
        };
      }
    },
    //文件夹树点击节点
    //角色选中row 复选框选中
    RolCheckedSelect: function RolCheckedSelect(row, column, event) {
      IsHasPermissionsData = true;

      if (AssociateRole.Buttondisabled == false) {
        if (AssociateRole.ReportRoleManagesTheJSON.length === 0) {
          AssociateRole.ReportRoleManagesTheJSON = AssociateRole.TemporaryStorageData;
        }

        AssociateRole.$nextTick(function () {
          //console.log(AssociateRole.ReportRoleManagesTheJSON);
          AssociateRole.ReportRoleManagesTheJSON.forEach(function (res) {
            if (AssociateRole.ReportPermissionTreeId === res.ID) {
              if (res.RolList.indexOf(row.id + '') < 0) {
                AssociateRole.$refs.table.toggleRowSelection(row, true);
                AssociateRole.$nextTick(function () {
                  res.RolList.push(row.id + '');
                  res.isEit = true;
                }); //console.log(res.RolList.length);
                //console.log("tableData" + AssociateRole.tableDataCount - 1);

                if (AssociateRole.tableDataCount - 1 === res.RolList.length) {
                  AssociateRole.ALLchecked = true;
                }
              } else {
                AssociateRole.$refs.table.toggleRowSelection(row, false);
                AssociateRole.$nextTick(function () {
                  res.RolList.splice(res.RolList.indexOf(row.id + ''), 1);
                  res.isEit = true;
                  AssociateRole.ALLchecked = false;
                });
              }
            }
          });
        });
      } //console.log(AssociateRole.ReportRoleManagesTheJSON);

    },
    //获取返回选中Key值
    getRowKeys: function getRowKeys(row) {
      return row.ID;
    },
    //得到返回所有的选中数据
    RolhandleSelectionChange: function RolhandleSelectionChange(rows) {
      AssociateRole.ModifyRoleIdList = rows;
    },
    GetRolTableList: function GetRolTableList() {
      AssociateRole.GetListRoles(1, '', 1);
    },
    GetUserTableList: function GetUserTableList() {
      AssociateRole.GetUsetDataTableList();
    },
    //判断复选框是否禁用
    checkboxT: function checkboxT() {
      if (AssociateRole.Buttondisabled == false) {
        return 1;
      } else {
        return 0;
      }
    },
    //点击全选所有添加面板回调
    AllUserChange: function AllUserChange(val) {
      if (val == true && AssociateRole.Buttondisabled == false) {
        IsHasPermissionsData = true;
        AssociateRole.ALLchecked = true;
        $.post('/System/GetRolAllDataList', {}, function (res) {
          if (res != null) {
            var item = [];

            for (var i = 0; i < res.length; i++) {
              item.push(res[i].id);
            } //console.log(item);


            AssociateRole.CheckedBoxRolTable = item;
            AssociateRole.$nextTick(function () {
              if (AssociateRole.CheckedBoxRolTable != null) {
                AssociateRole.tableData.forEach(function (e) {
                  AssociateRole.$refs.table.toggleRowSelection(e, true);
                });
              }
            });
            AssociateRole.ReportRoleManagesTheJSON.forEach(function (res) {
              if (AssociateRole.ReportPermissionTreeId === res.ID) {
                res.RolList = item;
                res.isEit = true;
              }
            });
          }

          console.log(AssociateRole.ReportRoleManagesTheJSON);
        }, 'json');
      } else {
        AssociateRole.CheckedBoxRolTable = [];
        AssociateRole.tableData.forEach(function (e) {
          AssociateRole.$refs.table.toggleRowSelection(e, false);
        });
        AssociateRole.ReportRoleManagesTheJSON.forEach(function (res) {
          if (AssociateRole.ReportPermissionTreeId === res.ID) {
            res.RolList = [];
            res.isEit = true;
          }
        });
      }
    },
    filterNode: function filterNode(value, data) {
      if (!value) return true;
      return data.label.toLowerCase().indexOf(value.toLowerCase()) !== -1;
    },
    ReportfilterNode: function ReportfilterNode(value, data) {
      if (!value) return true;
      return data.label.toLowerCase().indexOf(value.toLowerCase()) !== -1;
    },
    UploadfilterNode: function UploadfilterNode(value, data) {
      if (!value) return true;
      return data.label.toLowerCase().indexOf(value.toLowerCase()) !== -1;
    }
  },
  watch: {
    MeunPermissionsNameWhere: function MeunPermissionsNameWhere(val, oldval) {
      AssociateRole.$refs.Menutree.filter(val);
    },
    SearchReportName: function SearchReportName(val, oldval) {
      AssociateRole.$refs.ReportTree.filter(val);
    },
    SearchUploadName: function SearchUploadName(val, oldval) {
      AssociateRole.$refs.UploadTree.filter(val);
    },
    where: function where(val, oldval) {
      debugger;
      this.RoleQueryParameters.where = this.where;
      this.Rolloading = true;
      this.GetRolTableList(1, '', 1);
    },
    UserUserName: function UserUserName(val, oldval) {
      this.UserQueryParameters.page = 1;
      this.UserQueryParameters.where = this.UserUserName;
      this.GetUserTableList();
      this.Rolloading = true;
      this.Buttondisabled = true;
      this.GetListRoles(1, '', 1);
    },
    SearchCubeName: function SearchCubeName(val, oldval) {
      AssociateRole.getFolderpLoadeleUTreeParms.page = 1;
      this.getFolderpLoadeleUTreeParms.Where = this.SearchCubeName;
      this.GetGetSerchCubeData();
    },
    SearchTabularName: function SearchTabularName(val, oldval) {
      AssociateRole.getFolderpLoadeleUTreeParms.page = 1;
      this.GetTabularDataParms.where = this.SearchTabularName;
      this.GetSerchTabularData();
    },
    SearchFolderName: function SearchFolderName(val, oldval) {
      AssociateRole.$refs.FolderTree.filter(val);
    },
    SearchJobWhere: function SearchJobWhere(val, oldval) {
      AssociateRole.SqlJobsDataPames.page = 1;
      AssociateRole.SqlJobsDataPames.limit = this.pagesize;
      AssociateRole.SqlJobsDataPames.where = val;
      AssociateRole.GetSqlJobsData();
    }
  }
});