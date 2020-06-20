"use strict";

var _data, _methods;

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var AuthorityCenter = new Vue({
  el: '#AuthorityCenter',
  computed: {},
  data: (_data = {
    activeName: 'first',
    search_MenuAuthority: '',
    //搜索菜单权限
    MenuAuthorityData: [],
    //菜单权限树数据
    UserLoading: false,
    search_AssignUsers: '',
    //搜索分配用户
    AssignUsersData: [],
    IsAllUser: false,
    defaultProps: {
      children: 'children',
      label: 'label'
    },
    UsersPagationChecked: 0,
    AssignUserHasConfigure: [],
    //已经配置的用户数据
    CurrentUserRole: '',
    //当前点击的角色
    AssignUsersPagation: {
      total: 0,
      page: 1,
      limit: 10,
      where: ''
    },
    UserRolePagation: {
      total: 0,
      page: 1,
      limit: 16,
      haschecked: 0,
      where: ''
    },
    //角色表格分页数据
    ajaxSend: false,
    tableHeight: 300,
    search_userrole: ''
  }, _defineProperty(_data, "UserLoading", false), _defineProperty(_data, "Rolepane", 'Role'), _defineProperty(_data, "UserRoleData", []), _defineProperty(_data, "UserRolePagation", {
    total: 0,
    page: 1,
    limit: 16,
    haschecked: 0,
    where: ''
  }), _defineProperty(_data, "screenHeight", ""), _defineProperty(_data, "MenuHasChecked", []), _data),
  created: function created() {
    this.tableHeight = parseInt(LayOutStaticWindowsHeight - 250);
    this.TableRowNum = parseInt((LayOutStaticWindowsHeight - 350) / 40);
    this.AssignUsersPagation.limit = parseInt((LayOutStaticWindowsHeight - 420) / 40);
    this.UserRolePagation.limit = this.TableRowNum;
    this.screenHeight = this.TableRowNum * 40 + 41 + "px";
    this.GetAssignUsersData();
    this.GetUserRoleData();
    this.GetMenuAuthorityTree();
  },
  methods: (_methods = {
    GetAssignUsersData: function GetAssignUsersData() {
      var _this = this;

      var QueryParameters = {
        page: this.AssignUsersPagation.page,
        rows: this.AssignUsersPagation.limit,
        where: this.search_AssignUsers
      };
      this.UserLoading = true;
      $.ajax({
        data: QueryParameters,
        //async: false,
        dataType: 'json',
        type: 'post',
        url: '/system/UserGet',
        success: function success(res) {
          if (res.success) {
            _this.$nextTick(function () {
              _this.AssignUsersData = res.data;
              _this.AssignUsersPagation.total = res.total;

              _this.$refs.AssignUsers.clearSelection();

              var tempHasChecked = [];

              _this.AssignUserHasConfigure.forEach(function (e) {
                if (e.RoleID == _this.CurrentUserRole) {
                  tempHasChecked = e.UserHasCheckData;
                }
              });

              _this.AssignUsersData.forEach(function (e) {
                if (tempHasChecked.indexOf(e.id) >= 0) {
                  _this.$refs.AssignUsers.toggleRowSelection(e, true);
                }
              });

              _this.UsersPagationChecked = tempHasChecked.length;
              _this.UserLoading = false;
            });
          } else {
            _this.UserLoading = false;
          }
        }
      }); //根据CheckUser中的数据，选中某行
    },
    //获取用户表格数据
    SearchAssignUsersData: function SearchAssignUsersData() {
      this.GetAssignUsersData();
    },
    //回车键刷新表格
    SaveUsersData: function SaveUsersData() {
      var _this2 = this;

      //if (this.CurrentUserRole === '') {
      //    return this.$message.warning("请选择角色");
      //}
      //if (this.AssignUserHasConfigure.filter(e => e.IsCheck != 0).length < 1) {
      //    return this.$message.success("提交成功，未做任何修改");
      //}
      var obj = {
        UserConfig: this.AssignUserHasConfigure.filter(function (e) {
          return e.IsCheck != 0;
        })
      };
      this.SubmitLoading = true;
      $.post('/System/NewUserAssignmentRole', {
        data: JSON.stringify(obj)
      }, function (res) {
        if (res.success) {
          //if (res.otherMsg.success == true) {
          _this2.$message({
            type: 'success',
            message: res.message
          }); //}
          //else {
          //this.$message({
          //type: 'warning',
          // message: $.i18n.prop("PermissionsWere")
          //});
          //}


          _this2.IsAllUser = false;

          _this2.ResetUsersPagination();

          _this2.AssignUserHasConfigure = [];
          _this2.search_AssignUsers = '';

          _this2.AssignUsersTableSizeChange(_this2.CurrentUserRole);
        } else {
          _this2.$message.error(res.message);
        }

        _this2.SubmitLoading = false;
        IsHasPermissionsData = false;
      });
    },
    //提交分配的用户
    UserclickRow: function UserclickRow(row, column, event) {
      var _this3 = this;

      if (this.CurrentUserRole == "") {
        return;
      }

      this.$refs.AssignUsers.toggleRowSelection(row);
      var tempHasChecked = [];
      this.AssignUserHasConfigure.forEach(function (e) {
        if (e.RoleID == _this3.CurrentUserRole) {
          tempHasChecked = JSON.parse(JSON.stringify(e.UserHasCheckData));
        }
      });

      if (tempHasChecked.indexOf(row.id) < 0) {
        tempHasChecked.push(row.id);
      } else if (tempHasChecked.indexOf(row.id) > -1) {
        tempHasChecked.splice(tempHasChecked.indexOf(row.id), 1);
      }

      this.UsersPagationChecked = tempHasChecked.length; //设置当前用户选择了多少用户

      this.AssignUserHasConfigure.forEach(function (e) {
        if (e.RoleID == _this3.CurrentUserRole) {
          e.UserHasCheckData = tempHasChecked;
          e.IsCheck = 1;
        }
      });
      IsHasPermissionsData = true;
    },
    //用户表格行点击事件
    getRowKey: function getRowKey(row) {
      return row.id;
    },
    selectRow: function selectRow(selection, row) {
      var _this4 = this;

      if (this.CurrentUserRole == "") {
        return;
      }

      var selected = selection.length && selection.indexOf(row) !== -1; ////selected为 true就是选中，0或者false是取消选中

      var tempHasChecked = [];
      this.AssignUserHasConfigure.forEach(function (e) {
        if (e.RoleID == _this4.CurrentUserRole) {
          tempHasChecked = JSON.parse(JSON.stringify(e.UserHasCheckData));
        }
      });

      if (selected === true) {
        //勾选复选框
        if (tempHasChecked.indexOf(row.id) < 0) {
          tempHasChecked.push(row.id);
        }
      } else {
        //取消勾选复选框
        if (tempHasChecked.indexOf(row.id) > -1) {
          tempHasChecked.splice(tempHasChecked.indexOf(row.id), 1);
        }
      }

      this.UsersPagationChecked = tempHasChecked.length; //设置当前用户选择了多少用户

      this.AssignUserHasConfigure.forEach(function (e) {
        if (e.RoleID == _this4.CurrentUserRole) {
          e.UserHasCheckData = tempHasChecked;
          e.IsCheck = 1;
        }
      });
      IsHasPermissionsData = true;
    },
    //用户行复选框点击事件
    selectAllRow: function selectAllRow(selection) {
      var _this5 = this;

      var selectedall = false; ////selected为 true就是选中，false是取消选中

      selection.forEach(function (e) {
        selectedall = _this5.AssignUsersData.indexOf(e) !== -1;
      });
      var tempHasChecked = [];
      this.AssignUserHasConfigure.forEach(function (e) {
        if (e.RoleID == _this5.CurrentUserRole) {
          tempHasChecked = JSON.parse(JSON.stringify(e.UserHasCheckData));
        }
      });

      if (selectedall) {
        selection.forEach(function (e) {
          if (tempHasChecked.indexOf(e.id) < 0) {
            tempHasChecked.push(e.id);
          }
        });
      } else {
        this.AssignUsersData.forEach(function (e) {
          if (tempHasChecked.indexOf(e.id) > -1) {
            tempHasChecked.splice(tempHasChecked.indexOf(e.id), 1);
          }
        });
      }

      this.UsersPagationChecked = tempHasChecked.length; //设置当前用户选择了多少用户

      this.AssignUserHasConfigure.forEach(function (e) {
        if (e.RoleID == _this5.CurrentUserRole) {
          e.UserHasCheckData = tempHasChecked;
          e.IsCheck = 1;
        }
      });
      IsHasPermissionsData = true;
    },
    //表头全选点击事件
    openLoading: function openLoading() {
      this.Loading = true;
    },
    closeLoading: function closeLoading() {
      //this.loading.close();
      this.Loading = false;
    },
    GetMenuAuthorityTree: function GetMenuAuthorityTree() {
      var _this6 = this;

      this.openLoading();
      $.post('/System/SearchMenu', {
        RoleNavCode: 'null'
      }, function (res) {
        _this6.MenuAuthorityData = res.data;

        _this6.closeLoading();
      }, 'json');
    },
    //获取菜单层级树结构
    MenutreeNodeClick: function MenutreeNodeClick(node, check, child) {
      var _this7 = this;

      if (this.CurrentUserRole != "") {
        ////console.log("进入了点击事件")
        var checkMenu = [];
        checkMenu = this.$refs.MenuAuthority.getCheckedKeys().map(function (e) {
          return {
            id: e
          };
        });
        this.MenuHasChecked.forEach(function (e) {
          if (e.RoleID == _this7.CurrentUserRole) {
            e.idList = checkMenu;
            e.hasCheck = 1;
          }
        });
        IsHasPermissionsData = true;
      }
    },
    //选中状态改变时
    ResetUsersPagination: function ResetUsersPagination() {
      this.AssignUsersPagation.page = 1; //this.AssignUsersPagation.limit = 10;

      this.AssignUsersPagation.where = '';
    },
    //重置用户表格回到第一页
    AllUserChange: function AllUserChange() {
      var _this8 = this;

      var tempHasChecked = [];
      this.AssignUserHasConfigure.forEach(function (e) {
        if (e.RoleID == _this8.CurrentUserRole) {
          tempHasChecked = JSON.parse(JSON.stringify(e.UserHasCheckData));
        }
      });

      if (this.IsAllUser) {
        this.openLoading();
        $.post('/System/GetAllUser', {
          where: this.AssignUsersPagation.where
        }, function (res) {
          if (res.success) {
            tempHasChecked = res.data;

            _this8.$nextTick(function () {
              _this8.AssignUsersData.forEach(function (e) {
                if (res.data.indexOf(e.id + '') >= 0) {
                  _this8.$refs.AssignUsers.toggleRowSelection(e, true);
                }
              });
            });
          } else {
            _this8.$message.error(res.message);
          }

          _this8.$refs.AssignUsers.clearSelection();

          _this8.UsersPagationChecked = tempHasChecked.length; //设置当前用户选择了多少用户

          _this8.AssignUserHasConfigure.forEach(function (e) {
            if (e.RoleID == _this8.CurrentUserRole) {
              e.UserHasCheckData = res.data;
              e.IsCheck = 1;
            }
          });

          IsHasPermissionsData = true;

          _this8.closeLoading();
        });
      } else {
        tempHasChecked = [];
        this.$refs.AssignUsers.clearSelection();
        this.UsersPagationChecked = tempHasChecked.length; //设置当前用户选择了多少用户

        this.AssignUserHasConfigure.forEach(function (e) {
          if (e.RoleID == _this8.CurrentUserRole) {
            e.UserHasCheckData = res;
            e.IsCheck = 1;
          }
        });
        IsHasPermissionsData = true;
      } //setTimeout(() => {
      //    this.UsersPagationChecked = tempHasChecked.length;  //设置当前用户选择了多少用户
      //    this.AssignUserHasConfigure.forEach(e => {
      //        if (e.RoleID == this.CurrentUserRole) {
      //            e.UserHasCheckData = tempHasChecked;
      //            e.IsCheck = 1;
      //        }
      //    });
      //}, 0)

    },
    //全选所有用户点击事件
    filterNode: function filterNode(value, data) {
      if (!value) return true;
      return data.label.toLowerCase().indexOf(value.toLowerCase()) !== -1;
    },
    AssignUsersTableSizeChange: function AssignUsersTableSizeChange(RoleID) {
      var _this9 = this;

      this.UserLoading = true;
      var QueryParameters = {
        page: this.AssignUsersPagation.page,
        limit: this.AssignUsersPagation.limit,
        where: this.AssignUsersPagation.where,
        RoleID: RoleID
      };
      $.ajax({
        data: QueryParameters,
        //async: false,
        dataType: 'json',
        type: 'post',
        url: '/System/getRoleInUserPaginationList',
        success: function success(res) {
          if (res.status === 0) {
            _this9.AssignUsersData = res.data;
            _this9.AssignUsersPagation.total = res.total;
            $.ajax({
              data: {
                RoleID: RoleID
              },
              //async: false,
              dataType: 'json',
              type: 'post',
              url: '/Syste/RoleHaveUser',
              success: function success(res) {
                if (res.success) {
                  //把用户对应的所有角色放入CheckUser（已存在就不再放入）
                  _this9.$nextTick(function () {
                    _this9.$refs.AssignUsers.clearSelection();

                    var isIn = false;
                    var tempHasChecked = [];

                    _this9.AssignUserHasConfigure.forEach(function (e) {
                      if (e.RoleID == RoleID) {
                        isIn = true;
                        tempHasChecked = e.UserHasCheckData;
                      }
                    });

                    if (isIn) {
                      _this9.AssignUsersData.forEach(function (e) {
                        if (tempHasChecked.indexOf(e.id) >= 0) {
                          _this9.$refs.AssignUsers.toggleRowSelection(e, true);
                        }
                      });

                      _this9.UsersPagationChecked = tempHasChecked.length;
                    } else {
                      var temp = {
                        RoleID: RoleID,
                        UserHasCheckData: res.data,
                        IsCheck: 0
                      };

                      _this9.AssignUserHasConfigure.push(temp);

                      _this9.UsersPagationChecked = res.data.length;

                      _this9.AssignUsersData.forEach(function (e) {
                        if (res.data.indexOf(e.id + '') >= 0) {
                          _this9.$refs.AssignUsers.toggleRowSelection(e, true);
                        }
                      });
                    }

                    _this9.UserLoading = false;
                  });
                } else {
                  _this9.$message.error(res.msg);

                  _this9.UserLoading = false;
                }
              }
            }); //根据CheckUser中的数据，选中某行
          }
        }
      }); //根据CheckUser中的数据，选中某行
    },
    //角色切换时重新请求用户数据和已选用数据
    AssignUsersTablePageChange: function AssignUsersTablePageChange(val) {
      this.AssignUsersPagation.page = val; //this.AssignUsersTableSizeChange(this.CurrentUserRole);

      this.GetAssignUsersData();
    },
    //用户表格页码切换事件
    UserecheckboxInit: function UserecheckboxInit(row, index) {
      return this.CurrentUserRole == "" ? 0 : 1;
    },
    //设置第一次进入页面时 ，角色未选择时 用户禁用
    RoleCurrentChange_row: function RoleCurrentChange_row(val) {
      this.UserRolePagation.page = val;
      this.GetUserRoleData();
    },
    GetUserRoleData: function GetUserRoleData() {
      var _this10 = this;

      var QueryParameters = {
        Page: this.UserRolePagation.page,
        limit: this.UserRolePagation.limit,
        where: this.UserRolePagation.where
      };
      this.RoleLoading = true;
      $.post('/System/SearchRole', QueryParameters, function (res) {
        if (res.success) {
          _this10.UserRoleData = res.data;
          _this10.UserRolePagation.total = res.total;
        }

        _this10.RoleLoading = false;
      }, 'json');
    },
    UserRoleTableClick: function UserRoleTableClick(currentRow, oldCurrentRow) {
      this.CurrentUserRole = currentRow.id;

      if (oldCurrentRow != null) {
        this.OldUserRole = oldCurrentRow.id;
      } //this.OldUserRole = oldCurrentRow.ID;


      switch (this.activeName) {
        case "first":
          this.AssignUsersPagation.page = 1;
          this.AssignUsersTableSizeChange(currentRow.id); //设置角色已分配的用户

          break;

        case "five":
          this.GetMenuAuthorityTreeChecked(currentRow.id); //设置角色已配置的菜单权限

          break;
      } //this.GetGetSerchCubeData(); //获取已设置的Cube权限
      //this.GetTabularOptions();

    },
    //点击角色表格
    GetMenuAuthorityTreeChecked: function GetMenuAuthorityTreeChecked(RoleID) {
      var _this11 = this;

      if (this.CurrentUserRole == "") {
        return;
      }

      this.openLoading();
      var isIn = false;
      var hasCheckData = [];
      this.MenuHasChecked.forEach(function (e) {
        if (e.RoleID == _this11.CurrentUserRole) {
          isIn = true;
          hasCheckData = e.idList.map(function (i) {
            return i.id;
          });
        }
      });

      if (isIn) {
        var noChecked = false;
        this.MenuHasChecked.forEach(function (e) {
          if (e.hasCheck == 1) {
            noChecked = true;
          }
        });

        if (!noChecked) {
          this.$refs.MenuAuthority.setCheckedKeys(hasCheckData);
          setTimeout(function () {
            _this11.MenuHasChecked.forEach(function (e) {
              if (e.RoleID == _this11.CurrentUserRole) {
                e.hasCheck = 0;
              }
            });
          }, 0);
        } else {
          this.$refs.MenuAuthority.setCheckedKeys(hasCheckData);
        }

        this.closeLoading();
      } else {
        $.post("/System/GetRoleCheckedMenu", {
          RoleID: RoleID
        }, function (res) {
          if (res.success) {
            _this11.$nextTick(function () {
              var _this12 = this;

              this.$refs.MenuAuthority.setCheckedKeys(res.data);
              var oldCheck = res.data.map(function (e) {
                return {
                  id: e
                };
              });
              this.MenuHasChecked.push({
                RoleID: RoleID,
                idList: oldCheck,
                hasCheck: 0
              });
              setTimeout(function () {
                _this12.MenuHasChecked.forEach(function (e) {
                  if (e.RoleID == _this12.CurrentUserRole) {
                    e.hasCheck = 0;
                  }
                });
              }, 0);
            });

            _this11.closeLoading();
          } else {
            _this11.$message.error(res.message);
          }
        });
      }
    }
  }, _defineProperty(_methods, "AssignUsersTableSizeChange", function AssignUsersTableSizeChange(RoleID) {
    var _this13 = this;

    this.UserLoading = true;
    var QueryParameters = {
      page: this.AssignUsersPagation.page,
      rows: this.AssignUsersPagation.limit,
      where: this.AssignUsersPagation.where,
      RoleID: RoleID
    };
    $.ajax({
      data: QueryParameters,
      //async: false,
      dataType: 'json',
      type: 'post',
      url: '/System/getRoleInUserPaginationList',
      success: function success(res) {
        if (res.success) {
          _this13.AssignUsersData = res.data;
          _this13.AssignUsersPagation.total = res.total;
          $.ajax({
            data: {
              RoleID: RoleID
            },
            //async: false,
            dataType: 'json',
            type: 'post',
            url: '/System/RoleHaveUser',
            success: function success(res) {
              if (res.success) {
                //把用户对应的所有角色放入CheckUser（已存在就不再放入）
                _this13.$nextTick(function () {
                  _this13.$refs.AssignUsers.clearSelection();

                  var isIn = false;
                  var tempHasChecked = [];
                  console.log(_this13.AssignUserHasConfigure);

                  _this13.AssignUserHasConfigure.forEach(function (e) {
                    if (e.RoleID == RoleID) {
                      isIn = true;
                      tempHasChecked = e.UserHasCheckData;
                    }
                  });

                  if (isIn) {
                    _this13.AssignUsersData.forEach(function (e) {
                      if (tempHasChecked.indexOf(e.id + '') >= 0) {
                        _this13.$refs.AssignUsers.toggleRowSelection(e, true);
                      }
                    });

                    _this13.UsersPagationChecked = tempHasChecked.length;
                  } else {
                    var temp = {
                      RoleID: RoleID,
                      UserHasCheckData: res.data,
                      IsCheck: 0
                    };

                    _this13.AssignUserHasConfigure.push(temp);

                    _this13.UsersPagationChecked = res.data.length;

                    _this13.AssignUsersData.forEach(function (e) {
                      debugger;

                      if (res.data.indexOf(e.id + '') >= 0) {
                        _this13.$refs.AssignUsers.toggleRowSelection(e, true);
                      }
                    });
                  }

                  _this13.UserLoading = false;
                });
              } else {
                _this13.$message.error(res.message);

                _this13.UserLoading = false;
              }
            }
          }); //根据CheckUser中的数据，选中某行
        }
      }
    }); //根据CheckUser中的数据，选中某行
  }), _defineProperty(_methods, "SaveMenuData", function SaveMenuData() {
    var _this14 = this;

    if (this.CurrentUserRole === '') {
      return this.$message.warning($.i18n.prop('Pleaseselectarole'));
    }

    var ISChecked = false; //this.MenuHasChecked.forEach(e => {
    //    if (e.hasCheck == 1) {
    //        ISChecked = true;
    //    }
    //})
    //if (!ISChecked) {
    //    return this.$message.success($.i18n.prop('NoPermissionToModifyPleaseModifyBeforeSubmitting'));
    //}

    this.SubmitLoading = true;
    $.post('/System/NewEditRoleMenuByNavCode', {
      NavCodeObj: JSON.stringify(this.MenuHasChecked.filter(function (e) {
        return e.hasCheck != 0;
      }))
    }, function (res) {
      if (res.success) {
        _this14.$message({
          message: res.message,
          type: 'success'
        });

        _this14.MenuHasChecked = [];

        _this14.GetMenuAuthorityTreeChecked(_this14.CurrentUserRole);
      } else {
        _this14.$message.error(res.message);
      }

      _this14.search_MenuAuthority = '';
      _this14.SubmitLoading = false;
      IsHasPermissionsData = false;
    });
  }), _methods),
  watch: {
    search_AssignUsers: function search_AssignUsers(val, oldval) {
      this.search_AssignUsers = val;
      this.GetAssignUsersData();
    },
    search_userrole: function search_userrole(val, oldval) {
      this.UserRolePagation.where = val;
      this.GetUserRoleData();
    },
    search_MenuAuthority: function search_MenuAuthority(val, oldval) {
      this.$refs.MenuAuthority.filter(val);
    }
  }
});