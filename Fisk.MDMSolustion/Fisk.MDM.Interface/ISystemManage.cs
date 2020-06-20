using Fisk.MDM.DataAccess.Models;
using Fisk.MDM.ViewModel.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fisk.MDM.Interface
{
    public interface ISystemManage
    {
        #region 角色管理
        object RoleRelation(RoleItem model);
        object RoleHaveUser(string roleID);
        object getRoleInUserPaginationList(int page, int rows, string roleID);
        object SearchRole(RoleItem model);
        object GetRolByUserDataOrderByDesc(RoleItem parmas);
        object UserConfigRole(string[] roleList, string userId);
        object DeleteRole(RoleItem model);

        object InserRole(RoleItem model);
        object UpdateRole(RoleItem model);
        #endregion

        #region 用户管理
        object GetUserAllCheckedRole(string userID);
        object GetAllUser(string where);
        object UserHaveRole(string userId);
        object UserGet(int page, int rows, string where);
        object UserAdd(system_user user);
        object UserUpdate(system_user user);
        object UserDel(int Id);
        object UserSearch(string UserName, int page, int rows);
        #endregion

        #region 权限管理
        object GetRolAllDataList();
        object ModifUserRole(List<ModifRoleRelation> filNoList);
        object NewUserAssignmentRole(string data);
        #endregion

        #region 菜单管理
        object getMenuList();
        object GetMenuNodeCheckRoleList(string folderId);
        object ModifMenuRole(List<ModifRoleRelation> filNoList);
        object GetMenuNodeOrderByDesc(RoleItem parmas);
        object NewEditRoleMenuByNavCode(string navCodeObj);
        object GetRoleCheckedMenu(string roleID);
        object SearchMenu(MenuItem model);
        object DeleteMenu(MenuItem model);
        object InserMenu(MenuItem model);
        object UpdateMenu(MenuItem model);
        #endregion
    }
}
