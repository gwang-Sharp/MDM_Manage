using Fisk.MDM.DataAccess.Models;
using Fisk.MDM.Interface;
using Fisk.MDM.Utility.Common;
using Fisk.MDM.ViewModel;
using Fisk.MDM.ViewModel.System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Fisk.MDM.Business
{
    /// <summary>
    /// 系统管理
    /// </summary>
    public class SystemManage: ISystemManage
    {
        private readonly MDMDBContext _dbContext;
        private readonly SessionHelper helper;
        public SystemManage(MDMDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = dbContext;
            helper = new SessionHelper(httpContextAccessor);
        }

        #region 角色管理 hhyang
        /// <summary>
        /// 查询所有的角色 hhyang
        /// </summary>
        /// <param name="Parmas"></param>
        /// <returns></returns>
        public object RoleRelation(RoleItem model)
        {
            tableResult result = new tableResult();

            try
            {
                //获取已选中的角色
                var List = (from role in _dbContext.system_role
                            join urs in _dbContext.system_rolenavassignment.Where(x => x.Id == model.UserId && x.Validity == "1")
                            on role.Id.ToString() equals urs.RoleID into CheckSort
                            from checkRole in CheckSort.DefaultIfEmpty()
                            where role.Validity == "1"
                            select new
                            {
                                ID = role.Id,
                                RoleDesc = role.RoleDesc,
                                RoleName = role.RoleName,
                                CreateTime = role.CreateTime,
                                Validity = checkRole == null ? 1 : 2
                            }).ToList();

                if (!string.IsNullOrEmpty(model.Where))
                {
                    List = List.Where(e => e.RoleName.ToLower().Contains(model.Where.ToLower())).ToList();
                }
                int total = List.Count(); //算总数
                var resultList = List.OrderByDescending(x => x.Validity).Skip((model.Page - 1) * model.limit).Take(model.limit).ToList().Select(
                                             a => new
                                             {
                                                 ID = a.ID.ToString(),
                                                 RoleDesc = a.RoleDesc,
                                                 RoleName = a.RoleName,
                                                 CreateTime = a.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),

                                             }
                                        ).Distinct().ToList();

                result.total = total;
                result.data = resultList;
                result.message = "查询成功！";
                result.success = true;
            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }


        /// <summary>
        /// 得到角色里面的用户 hhyang
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public object RoleHaveUser(string roleID)
        {
            Result result = new Result();

            try
            {
                var RoleIDList = (from role_user in _dbContext.system_userroleassignment
                                  join user in _dbContext.system_user
                                  on role_user.UserID equals user.Id
                                  where role_user.Validity == "1" && role_user.RoleID.ToString() == roleID && user.Validity != "0"
                                  select new { role_user.UserID }).ToList();
                List<string> List = new List<string>();
                foreach (var item in RoleIDList)
                {
                    List.Add(item.UserID.ToString());
                }
                result.success = true;
                result.data = List;

            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }

        public object getRoleInUserPaginationList(int page, int rows, string roleID)
        {
            tableResult tr = new tableResult();
            try
            {
                var data = (from paginationU in _dbContext.system_user
                            join role in _dbContext.system_userroleassignment.Where(e => e.RoleID.ToString() == roleID && e.Validity == "1") on paginationU.Id equals role.UserID into checklist
                            from checkUser in checklist.DefaultIfEmpty()
                            where paginationU.Validity == "1"
                            select new
                            {
                                ID = paginationU.Id,
                                UserAccount = paginationU.UserAccount,
                                UserName = paginationU.UserName,
                                CreateTime = paginationU.CreateTime,
                                Creater = paginationU.Creater,
                                Email = paginationU.Email,
                                SortNumber = checkUser == null ? 1 : 2
                            }).ToList();
                tr.total = data.Count(); //算总数
                var resualtList = data.OrderByDescending(u => u.SortNumber).ThenByDescending(u => u.CreateTime).Skip(rows * (page - 1)).Take(rows).ToList();
                tr.data = resualtList;

                //(from c in resualtList.AsEnumerable()
                //           select new
                //           {
                //               ID = c.ID,
                //               UserAccount = c.UserAccount,
                //               UserName = c.UserName,
                //               Creater = c.Creater!=""? c.Creater:"",
                //               Email = c.Email,
                //               CreateTime = c.CreateTime.Value.ToString()!=""? c.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"):"2020-02-0"
                //           });
                tr.success = true;
                return tr;
            }
            catch (Exception ex)
            {
                throw;

            }
            return tr;
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object UpdateRole(RoleItem model)
        {
            Result result = new Result();
            try
            {
                var role = _dbContext.system_role.Where(x => x.Id == model.Id && x.Validity == "1").FirstOrDefault();
                if (role != null)
                {
                    role.RoleName = model.RoleName;
                    role.RoleDesc = model.RoleDesc;
                    role.Updater = CurrentUser.UserAccount;
                    role.UpdateTime = DateTime.Now;
                    int count = _dbContext.SaveChanges();
                    if (count > 0)
                    {
                        result.success = true;
                        result.message = "更新成功";
                    }
                    else
                    {
                        result.success = false;
                        result.message = "更新失败";
                    }
                }
                else
                {
                    result.success = false;
                    result.message = "更新失败";
                }
            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object InserRole(RoleItem model)
        {
            Result result = new Result();
            try
            {
                List<system_role> RoleList = _dbContext.system_role.Where(x => x.RoleName.ToUpper() == model.RoleName.ToUpper() && x.Validity == "1").ToList();
                if (RoleList.Count() > 0)
                {
                    result.success = false;
                    result.message = "角色名称已存在";
                    return result;
                }
                else
                {
                    system_role role = new system_role();
                    role.RoleName = model.RoleName;
                    role.RoleDesc = model.RoleDesc;
                    role.Creater = CurrentUser.UserAccount;
                    role.CreateTime = DateTime.Now;
                    role.Updater = CurrentUser.UserAccount;
                    role.UpdateTime = DateTime.Now;
                    role.Validity = "1";
                    _dbContext.system_role.Add(role);
                    int count = _dbContext.SaveChanges();
                    if (count > 0)
                    {
                        result.success = true;
                        result.message = "添加成功";
                    }
                    else
                    {
                        result.success = false;
                        result.message = "添加失败";
                    }


                }
            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }

        //删除角色
        public object DeleteRole(RoleItem model)
        {
            Result result = new Result();
            try
            {
                var role = _dbContext.system_role.Where(x => x.Id == model.Id && x.Validity == "1").FirstOrDefault();
                if (role != null)
                {
                    role.Validity = "0";
                    role.Updater = CurrentUser.UserAccount;
                    role.UpdateTime = DateTime.Now;
                    int count = _dbContext.SaveChanges();
                    if (count > 0)
                    {
                        result.success = true;
                        result.message = "删除成功";
                    }
                    else
                    {
                        result.success = false;
                        result.message = "删除失败";
                    }
                }
                else
                {
                    result.success = false;
                    result.message = "删除失败";
                }
            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }
        /// <summary>
        /// 获取角色表格
        /// </summary>
        /// <param name="parmas"></param>
        /// <returns></returns>
        public object GetRolByUserDataOrderByDesc(RoleItem parmas)
        {
            tableResult tr = new tableResult();
            tr.success = true;
            try
            {

                var data = (from paginationR in _dbContext.system_role
                            join urs in _dbContext.system_userroleassignment.Where(e => e.UserID.ToString() == parmas.ParmsId && e.Validity == "1") on paginationR.Id equals urs.RoleID into CheckSort
                            from checkRole in CheckSort.DefaultIfEmpty()
                            where paginationR.Validity == "1"
                            select new
                            {
                                ID = paginationR.Id.ToString(),
                                RoleDesc = paginationR.RoleDesc,
                                RoleName = paginationR.RoleName,
                                CreateTime = paginationR.CreateTime,
                                Creater = paginationR.Creater,
                                Validity = checkRole == null ? 1 : 2
                            }
                            ).Distinct().ToList();
                //拼接where条件
                if (!string.IsNullOrEmpty(parmas.Where))
                {
                    data = data.Where(u => u.RoleName.ToLower().Contains(parmas.Where.ToLower())).ToList();
                }
                tr.total = data.Count(); //算总数
                var resualtList = data.OrderByDescending(u => u.Validity).ThenByDescending(x => x.CreateTime).Skip(parmas.limit * (parmas.Page - 1)).Take(parmas.limit);
                tr.data = (from c in resualtList.AsEnumerable()
                           select new
                           {
                               ID = c.ID,
                               RoleName = c.RoleName,
                               MenuCount = (from menu in _dbContext.system_userroleassignment
                                            where menu.RoleID.ToString() == c.ID && menu.Validity == "1"
                                            select menu).Count(),
                               RoleDesc = c.RoleDesc,
                               Creater = c.Creater,
                               CreateTime = c.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                           });
                return tr;
            }
            catch (Exception ex)
            {
                throw;

            }
            return tr;
        }

        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="roleList"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public object UserConfigRole(string[] roleList, string userId)
        {
            Result result = new Result();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    List<string> NavList = new List<string>();
                    foreach (var i in roleList)
                    {
                        NavList.Add(i.ToString());
                    }
                    List<system_userroleassignment> Role = _dbContext.system_userroleassignment.Where(x => x.UserID.ToString() == userId).ToList();
                    foreach (var item in Role)
                    {
                        item.Validity = "0";
                        item.Updater = CurrentUser.UserAccount;
                        item.UpdateTime = DateTime.Now;
                    }
                    _dbContext.SaveChanges();
                    foreach (var item in NavList)
                    {
                        system_userroleassignment userRole = new system_userroleassignment();
                        userRole.RoleID = int.Parse(item);
                        userRole.UserID = int.Parse(userId);
                        userRole.Creater = CurrentUser.UserAccount;
                        userRole.CreateTime = DateTime.Now;
                        userRole.Updater = CurrentUser.UserAccount;
                        userRole.UpdateTime = DateTime.Now;
                        userRole.Validity = "1";
                        _dbContext.system_userroleassignment.Add(userRole);
                    }
                    int count = _dbContext.SaveChanges();
                    if (count > 0)
                    {
                        result.success = true;
                        result.message = "设置成功";
                        tran.Commit();
                    }
                    else
                    {
                        result.success = false;
                        result.message = "设置失败";
                        tran.Rollback();
                    }

                }
                catch (Exception ex)
                {

                    tran.Rollback();
                    throw;

                }

                return result;
            }
        }



        /// <summary>
        /// 查看角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object SearchRole(RoleItem model)
        {
            tableResult result = new tableResult();
            try
            {
                List<system_role> RoleList = _dbContext.system_role.Where(x => x.Validity == "1").ToList();
                if (!string.IsNullOrWhiteSpace(model.Where))
                {
                    RoleList = RoleList.Where(x => x.RoleName.Contains(model.Where) || x.RoleDesc.Contains(model.Where) || x.Creater.Contains(model.Where)).ToList();
                }
                int total = RoleList.Count(); //算总数
                var resultList = RoleList.OrderByDescending(x => x.CreateTime).Skip((model.Page - 1) * model.limit).Take(model.limit).ToList().Select(
                                             a => new
                                             {
                                                 a.Id,
                                                 a.RoleName,
                                                 a.RoleDesc,
                                                 a.Creater,
                                                 CreateTime = a.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                                                 a.Updater,
                                                 UpdateTime = a.UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                                             }
                                        ).Distinct().ToList();

                result.total = total;
                result.data = resultList;
                result.message = "查询成功！";
                result.success = true;
            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }
        #endregion

        #region 用户管理 WG
        /// <summary>
        /// 获取用户角色被选中的id hhyang
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public object GetUserAllCheckedRole(string userID)
        {
            try
            {
                int UserId = int.Parse(userID);
                var RoldList = _dbContext.system_userroleassignment.Where(x => x.UserID == UserId && x.Validity == "1").Select(x => new { x.RoleID }).Distinct().ToList();
                List<string> str = new List<string>();
                foreach (var item in RoldList)
                {
                    str.Add(item.RoleID.ToString());
                }
                return str;
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public object GetAllUser(string where)
        {
            Result result = new Result();
            try
            {
                var data = _dbContext.system_user.Where(u => u.Validity != "0").ToList();
                if (!string.IsNullOrEmpty(where))
                {
                    data = data.Where(e => e.Validity != "0" && (e.UserAccount.ToLower().Contains(where.ToLower()) || e.UserName.ToLower().Contains(where.ToLower()))).ToList();
                }

                List<string> List = new List<string>();
                foreach (var item in data)
                {
                    List.Add(item.Id.ToString());
                }
                result.data = List;
                result.success = true;
            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }

        /// <summary>
        /// 获取用户对应的角色ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public object UserHaveRole(string userId)
        {
            Result result = new Result();
            try
            {
                var RoleIDList = (from role_user in _dbContext.system_userroleassignment
                                  join role in _dbContext.system_role
                                  on role_user.RoleID equals role.Id
                                  where role.Validity == "1" && role_user.Validity == "1"
                                  && role_user.UserID.ToString() == userId
                                  select role_user.RoleID).ToList();
                List<string> List = new List<string>();
                foreach (var item in RoleIDList)
                {
                    List.Add(item.ToString());
                }
                result.success = true;
                result.data = List;
            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public object UserGet(int page, int rows, string where)
        {
            tableResult result = new tableResult();
            try
            {
                var data = _dbContext.system_user.Where(x => x.Validity == "1").AsNoTracking();
                if (!string.IsNullOrWhiteSpace(where))
                {
                    data = data.Where(x => x.UserAccount.Contains(where) || x.UserName.Contains(where));
                }
                result.success = true;
                result.message = "查询成功";
                result.data = data.Skip((page - 1) * rows).Take(rows).Select(x => new
                {
                    Id = x.Id.ToString(),
                    x.UserAccount,
                    x.UserName,
                    x.UserPwd,
                    x.Updater,
                    x.Creater,
                    x.Email,
                    x.CreateTime,
                    x.UpdateTime,
                }).ToList();
                result.total = data.Count();
            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }
        /// <summary>
        /// 检索用户
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public object UserSearch(string UserName, int page, int rows)
        {
            tableResult result = new tableResult();
            try
            {
                var data = _dbContext.system_user.AsNoTracking().Where(it => it.UserName.Contains(UserName));
                result.success = true;
                result.message = "查询成功";
                result.data = data.Skip((page - 1) * rows).Take(rows).ToList();
                result.total = data.Count();
            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public object UserUpdate(system_user user)
        {
            Result result = new Result();
            try
            {
                var ExistData = this._dbContext.system_user.Where(it => it.Id == user.Id).FirstOrDefault();
                this._dbContext.Entry(ExistData).State = EntityState.Modified;
                byte[] pwdAndSalt = Encoding.UTF8.GetBytes(user.UserPwd + ExistData.Salt);
                byte[] hasBytes = new SHA256Managed().ComputeHash(pwdAndSalt);
                string pwd = Convert.ToBase64String(hasBytes);
                ExistData.UserAccount = user.UserAccount;
                ExistData.UserName = user.UserName;
                ExistData.Email = user.Email;
                ExistData.UserPwd = pwd;
                ExistData.UpdateTime = DateTime.Now;
                this._dbContext.Entry(ExistData).Property(it => it.Creater).IsModified = false;
                this._dbContext.Entry(ExistData).Property(it => it.ADID).IsModified = false;
                this._dbContext.Entry(ExistData).Property(it => it.Type).IsModified = false;
                this._dbContext.Entry(ExistData).Property(it => it.Icon).IsModified = false;
                this._dbContext.Entry(ExistData).Property(it => it.Salt).IsModified = false;
                this._dbContext.Entry(ExistData).Property(it => it.Validity).IsModified = false;
                this._dbContext.Entry(ExistData).Property(it => it.CreateTime).IsModified = false;
                int rows = this._dbContext.SaveChanges();
                if (rows >= 0)
                {
                    result.message = "更新成功";
                    result.success = true;
                    result.data = "";
                }
                else
                {
                    result.message = "更新失败";
                    result.success = true;
                    result.data = "";
                }
            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public object UserAdd(system_user user)
        {
            Result result = new Result();
            try
            {
                if (this._dbContext.system_user.AsNoTracking().Any(it => it.UserAccount == user.UserAccount))
                {
                    result.success = false;
                    result.message = "存在相同用户名";
                    return result;
                }
                string saltStr = Guid.NewGuid().ToString();
                byte[] pwdAndSalt = Encoding.UTF8.GetBytes(user.UserPwd + saltStr);
                byte[] hasBytes = new SHA256Managed().ComputeHash(pwdAndSalt);
                string pwd = Convert.ToBase64String(hasBytes);
                user.UserPwd = pwd;
                user.Creater = CurrentUser.UserAccount;
                user.CreateTime = DateTime.Now;
                user.Updater = CurrentUser.UserAccount;
                user.UpdateTime = DateTime.Now;
                user.ADID = "";
                user.Type = "";
                user.Validity = "1";
                user.Icon = "";
                user.Salt = saltStr;
                this._dbContext.system_user.Add(user);
                int rows = this._dbContext.SaveChanges();
                if (rows != 0)
                {
                    result.success = true;
                    result.message = "添加成功";

                }
                else
                {
                    result.success = false;
                    result.message = "添加失败";
                }
            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public object UserDel(int Id)
        {
            Result result = new Result();
            try
            {
                var DelUser = this._dbContext.system_user.Where(it => it.Id == Id).FirstOrDefault();
                this._dbContext.Remove(DelUser);
                int rows = this._dbContext.SaveChanges();
                if (rows >= 0)
                {
                    result.message = "删除成功";
                    result.success = true;
                }
                else
                {
                    result.message = "删除失败";
                    result.success = true;
                }
            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }
        #endregion

        #region 权限管理 hhyang
        /// <summary>
        /// 点击全选查询所有的id  hhyang
        /// </summary>
        /// <returns></returns>
        public object GetRolAllDataList()
        {

            try
            {
                var RolList = _dbContext.system_role.Where(x => x.Validity == "1").Select(x => new { x.Id }).Distinct().ToArray();
                return RolList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改用户对应的权限 hhyang
        /// </summary>
        /// <param name="FilNoList"></param>
        /// <returns></returns>
        public object ModifUserRole(List<ModifRoleRelation> filNoList)
        {
            Result result = new Result();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (filNoList != null && filNoList.Count() > 0)
                    {
                        foreach (var item in filNoList)
                        {

                            var distNull = _dbContext.system_userroleassignment.Where(x => x.UserID.ToString() == item.ID && x.Validity == "1").ToList();
                            foreach (var items in distNull)
                            {
                                items.Validity = "0";
                            }
                            if (item.RolList != null && item.RolList.Count() != 0)
                            {
                                foreach (var val in item.RolList)
                                {
                                    system_userroleassignment model = new system_userroleassignment();
                                    model.UserID = Convert.ToInt32(item.ID);
                                    model.RoleID = Convert.ToInt32(val);
                                    model.Creater = CurrentUser.UserAccount;
                                    model.CreateTime = DateTime.Now;
                                    model.Validity = "1";
                                    _dbContext.system_userroleassignment.Add(model);
                                }
                            }
                        }
                    }
                    // var UserRoleList = (from r in BE.System_UserRoleAssignment where r.UserID == UserID && r.Validity == "1" select new { RoleID = r.RoleID }).ToList().Select(r => r.RoleID).ToList<string>();
                    if (_dbContext.SaveChanges() > 0)
                    {
                        tran.Commit();
                        result.success = true;
                        result.message = "设置成功";
                    }
                    else
                    {
                        tran.Rollback();
                        result.success = false;
                        result.message = "失败";
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw;

                }
                return result;
            }

        }


        public object NewUserAssignmentRole(string data)
        {
            Result result = new Result();
            string RoleID = string.Empty;
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    dynamic Configs = JsonConvert.DeserializeObject<dynamic>(data);
                    foreach (var item in Configs["UserConfig"])
                    {
                        RoleID = item["RoleID"];
                        var UserIDList = item["UserHasCheckData"];
                        List<string> userList = new List<string>();
                        foreach (var items in UserIDList)
                        {
                            userList.Add(items.ToString());
                        }
                        int RoleI = Convert.ToInt32(RoleID);
                        var del = _dbContext.system_userroleassignment.Where(e => e.Validity == "1" && e.RoleID == RoleI).ToList();
                        foreach (var d in del)
                        {
                            d.Validity = "0";
                            d.UpdateTime = DateTime.Now;
                        }

                        foreach (var val in userList)
                        {
                            if (val != "")
                            {
                                system_userroleassignment system_UserRoleAssignment = new system_userroleassignment();
                                system_UserRoleAssignment.RoleID = RoleI;
                                system_UserRoleAssignment.UserID = int.Parse(val);
                                system_UserRoleAssignment.Creater = CurrentUser.UserAccount;
                                system_UserRoleAssignment.CreateTime = DateTime.Now;
                                system_UserRoleAssignment.Validity = "1";
                                _dbContext.system_userroleassignment.Add(system_UserRoleAssignment);
                            }

                        }
                    }

                    int count = _dbContext.SaveChanges();
                    if (count > 0)
                    {
                        result.success = true;
                        result.message = "设置成功";
                        tran.Commit();
                    }
                    else
                    {
                        result.success = false;
                        result.message = "设置失败";
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw;

                }
            }
            return result;

        }
        #endregion

        #region 菜单管理 hhyang


        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public object getMenuList()
        {
            try
            {
                string UserName = CurrentUser.UserAccount;

                //string UserName = "test3";
                var list = (from user in _dbContext.system_user
                            join userRole in _dbContext.system_userroleassignment on user.Id equals userRole.UserID
                            join roleNav in _dbContext.system_rolenavassignment on userRole.RoleID.ToString() equals roleNav.RoleID
                            join navList in _dbContext.system_navigation.AsNoTracking() on roleNav.NavCode equals navList.Id.ToString()
                            where user.Validity == "1" && userRole.Validity == "1" && roleNav.Validity == "1"
                            && user.UserAccount == UserName
                            select new
                            {
                                NavName = navList.NavName
                                ,
                                navList.NavCode
                                ,
                                navList.ParentNavCode
                                ,
                                navList.NavURL
                                ,
                                navList.SequenceNo
                            }
                          ).Distinct().ToList().OrderBy(a => a.SequenceNo).ToList();
                //.FromCache(CachePolicy.WithDurationExpiration(TimeSpan.FromSeconds(0))).ToList();//菜单查询结果缓存600秒;
                return list;
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        //编辑菜单  hhyang
        public object UpdateMenu(MenuItem model)
        {
            Result result = new Result();
            try
            {
                if (model.Id != null && model.Id != 0)
                {
                    system_navigation FindMenu = _dbContext.system_navigation.FirstOrDefault(x => x.Id == model.Id);
                    FindMenu.NavName = model.Name;
                    FindMenu.NavDesc = model.Describ;
                    FindMenu.NavURL = model.Address;
                    FindMenu.ConfirmDelete = Convert.ToInt32(model.ConfirmDelete);
                    FindMenu.Updater = CurrentUser.UserAccount;
                    FindMenu.UpdateTime = DateTime.Now;
                    int ChangeLimit = _dbContext.SaveChanges();
                    if (ChangeLimit > 0)
                    {
                        result.message = "更新成功";
                        result.success = true;
                    }
                    else
                    {

                        result.message = "更新失败";
                        result.success = false;
                    }
                }
                else
                {

                    result.message = "更新失败";
                    result.success = false;
                }

            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }


        //添加菜单 hhyang
        public object InserMenu(MenuItem model)
        {
            Result result = new Result();
            try
            {
                int Grade = 0;
                if (model.ParentNavCode == "1")
                {
                    //说明是一级菜单
                    Grade = 1;
                }
                else
                {
                    var menu = _dbContext.system_navigation.FirstOrDefault(x => x.NavCode == model.ParentNavCode);
                    Grade = Convert.ToInt32(model.Grade) + 1;

                    _dbContext.SaveChanges();
                }
                if (_dbContext.system_navigation.Where(e => e.NavName == model.Name && e.Grade == Grade.ToString()).Any())
                {
                    //如果当前等级文件夹中文名称已存在，跳出

                    result.success = false;
                    result.message = "该菜单名称已经存在!";
                    return result;
                }

                system_navigation NewMenu = new system_navigation();
                NewMenu.NavCode = Guid.NewGuid().ToString();
                NewMenu.NavName = model.Name;
                NewMenu.NavDesc = model.Describ;
                NewMenu.NavURL = model.Address;
                NewMenu.ParentNavCode = model.ParentNavCode;
                NewMenu.SequenceNo = int.Parse(model.Sorting);
                NewMenu.Updater = CurrentUser.UserAccount;
                NewMenu.UpdateTime = DateTime.Now;
                NewMenu.Creater = CurrentUser.UserAccount;
                NewMenu.CreateTime = DateTime.Now;
                NewMenu.Grade = Grade.ToString();
                NewMenu.ConfirmDelete = int.Parse(model.ConfirmDelete);
                _dbContext.system_navigation.Add(NewMenu);
                int count = _dbContext.SaveChanges();
                if (count != 0)
                {
                    result.success = true;
                    result.message = "添加成功";

                }
                else
                {
                    result.success = false;
                    result.message = "添加失败";

                }
            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }

        //删除菜单  hhyang
        public object DeleteMenu(MenuItem model)
        {
            Result result = new Result();
            try
            {
                system_navigation data = _dbContext.system_navigation.Find(model.Id);
                _dbContext.system_navigation.Remove(data);
                var flag = _dbContext.SaveChanges();
                if (flag != 0)
                {
                    result.success = true;
                    result.message = "删除成功！";

                }
                else
                {
                    result.success = false;
                    result.message = "删除失败！";

                }
            }
            catch (Exception ex)
            {
                throw;


            }
            return result;
        }


        /// <summary>
        /// 得到角色对应的菜单权限 hhyang
        /// </summary>
        /// <param name="MenuId"></param>
        /// <returns></returns>
        public object GetMenuNodeCheckRoleList(string folderId)
        {
            try
            {
                var FolderList = (from a in _dbContext.system_rolenavassignment
                                  where a.Validity == "1" && a.NavCode.Equals(folderId)
                                  select a.RoleID
                                ).Distinct().ToList();
                return FolderList;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// 反关联权限修改菜单
        /// </summary>
        /// <param name="filNoList"></param>
        /// <returns></returns>
        public object ModifMenuRole(List<ModifRoleRelation> filNoList)
        {
            Result result = new Result();
            using (var tan = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (filNoList != null && filNoList.Count() > 0)
                    {
                        foreach (var item in filNoList)
                        {
                            var disNull = _dbContext.system_rolenavassignment.Where(x => x.NavCode == item.ID && x.Validity == "1");
                            if (disNull != null && disNull.Count() > 0)
                            {
                                foreach (var items in disNull)
                                {
                                    items.Validity = "0";

                                }
                            }
                            if (item.RolList != null && item.RolList.Count() != 0)
                            {
                                foreach (var val in item.RolList)
                                {
                                    system_rolenavassignment model = new system_rolenavassignment();
                                    model.RoleID = val;
                                    model.NavCode = item.ID;
                                    model.Creater = CurrentUser.UserAccount;
                                    model.CreateTime = DateTime.Now;
                                    model.Validity = "1";
                                    _dbContext.system_rolenavassignment.Add(model);
                                }
                            }
                        }
                    }
                    if (_dbContext.SaveChanges() > 0)
                    {
                        tan.Commit();
                        result.success = true;
                        result.message = "设置成功";
                    }
                    else
                    {
                        tan.Rollback();
                        result.success = false;
                        result.message = "设置失败";
                    }
                }
                catch (Exception ex)
                {
                    tan.Rollback();
                    throw;

                }
                return result;
            }
        }


        /// <summary>
        /// 查询 hhyang
        /// </summary>
        /// <param name="parmas"></param>
        /// <returns></returns>
        public object GetMenuNodeOrderByDesc(RoleItem parmas)
        {
            tableResult tr = new tableResult();
            tr.success = true;
            try
            {
                var data = (from paginationR in _dbContext.system_role
                            join urs in _dbContext.system_rolenavassignment.Where(e => e.NavCode == parmas.ParmsId && e.Validity == "1") on paginationR.Id.ToString() equals urs.RoleID into CheckSort
                            from checkRole in CheckSort.DefaultIfEmpty()
                            where paginationR.Validity == "1"
                            select new
                            {
                                ID = paginationR.Id.ToString(),
                                RoleDesc = paginationR.RoleDesc,
                                RoleName = paginationR.RoleName,
                                CreateTime = paginationR.CreateTime,
                                Creater = paginationR.Creater,
                                Validity = checkRole == null ? 1 : 2
                            }
                            );
                //拼接where条件
                if (!string.IsNullOrEmpty(parmas.Where))
                {
                    data = data.Where(u => u.RoleName.ToLower().Contains(parmas.Where.ToLower()));
                }
                tr.total = data.Count(); //算总数
                var resualtList = data.OrderByDescending(u => u.Validity).ThenByDescending(x => x.CreateTime).Skip(parmas.limit * (parmas.Page - 1)).Take(parmas.limit);
                tr.data = (from c in resualtList.AsEnumerable()
                           select new
                           {
                               ID = c.ID,
                               RoleName = c.RoleName,
                               RoleDesc = c.RoleDesc,
                               Creater = c.Creater,
                               CreateTime = c.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                           }).Distinct().ToList();
                return tr;
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        /// <summary>
        /// 修改菜单权限 hhyang
        /// </summary>
        /// <param name="NavCodeObj"></param>
        /// <returns></returns>
        public object NewEditRoleMenuByNavCode(string navCodeObj)
        {
            Result result = new Result();

            dynamic NavObj = JsonConvert.DeserializeObject<dynamic>(navCodeObj);
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var AllData in NavObj)
                    {
                        string RoleID = AllData["RoleID"].ToString();

                        List<string> NavList = new List<string>();
                        foreach (var i in AllData["idList"])
                        {
                            NavList.Add(i["id"].ToString());
                        }

                        //先删除角色对应的菜单，重新添加
                        var disNull = _dbContext.system_rolenavassignment.AsNoTracking().Where(e => e.Validity == "1" && e.RoleID == RoleID).ToList();
                        foreach (var item in disNull)
                        {
                            item.Validity = "0";
                            item.UpdateTime = DateTime.Now;
                            item.Updater = CurrentUser.UserAccount;
                        }
                        List<system_rolenavassignment> modelList = new List<system_rolenavassignment>();
                        for (int i = 0; i < NavList.Count; i++)
                        {
                            system_rolenavassignment model = new system_rolenavassignment();
                            model.RoleID = RoleID;
                            model.NavCode = NavList[i].ToString();
                            model.Creater = CurrentUser.UserAccount;
                            model.CreateTime = DateTime.Now;
                            model.Validity = "1";
                            //model.UpdateTime = DateTime.Now;
                            //model.Updater = SessionHelper.Get("UserName").ToString();
                            modelList.Add(model);
                        }
                        _dbContext.system_rolenavassignment.AddRange(modelList);
                    }

                    int res = _dbContext.SaveChanges();
                    if (res > 0)
                    {
                        tran.Commit();
                        result.success = true;
                        result.message = "设置成功";
                    }
                    else
                    {
                        tran.Rollback();
                        result.success = false;
                        result.message = "设置失败";
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw;

                }
            }
            return result;
        }

        /// <summary>
        /// 点击角色获取菜单选中 hhyang
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public object GetRoleCheckedMenu(string roleID)
        {
            Result result = new Result();
            try
            {
                var data = _dbContext.system_rolenavassignment.Where(e => e.RoleID == roleID && e.Validity == "1").Select(e => e.NavCode).Distinct().ToList();
                result.success = true;
                result.data = data;
                return result;
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object SearchMenu(MenuItem model)
        {
            tableResult result = new tableResult();

            try
            {
                List<system_navigation> menus = _dbContext.system_navigation.ToList();
                List<NavTreeGroupVM> NavTreeGroups = new List<NavTreeGroupVM>();
                NavTreeGroups.AddRange(GetMenuChildren("1", menus, "1"));
                result.data = NavTreeGroups;
                result.success = true;
                result.message = "查询成功";
            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }
        /// <summary>
        /// 递归得到数据结构  hhyang
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="menus"></param>
        /// <param name="Squen"></param>
        /// <returns></returns>
        private List<NavTreeGroupVM> GetMenuChildren(string code, List<system_navigation> menus, string Squen)
        {
            List<system_navigation> children_Menue = (from p in menus
                                                      where p.ParentNavCode == code
                                                      select p).OrderBy(e => e.SequenceNo).ToList<system_navigation>();
            List<NavTreeGroupVM> list = new List<NavTreeGroupVM>();
            if (children_Menue.Count > 0)
            {
                foreach (var item in children_Menue)
                {
                    NavTreeGroupVM NavTree = new NavTreeGroupVM();
                    NavTree.id = item.Id.ToString();
                    NavTree.NavCode = item.NavCode;
                    NavTree.label = item.NavName;
                    NavTree.Grade = item.Grade != "" ? int.Parse(item.Grade) : 0;
                    NavTree.NavName = item.NavName;
                    NavTree.NavDesc = item.NavDesc;
                    NavTree.ParentNavCode = item.ParentNavCode;
                    NavTree.ConfirmDelete = item.ConfirmDelete;
                    NavTree.NavURL = item.NavURL;
                    if (code == "1")
                    {
                        NavTree.SequenceNo = item.SequenceNo.ToString();
                    }
                    else
                    {
                        NavTree.SequenceNo = Squen + "-" + item.SequenceNo;
                    }
                    NavTree.children = GetMenuChildren(item.NavCode, menus, NavTree.SequenceNo);
                    NavTree.ConfirmDelete = item.ConfirmDelete;
                    list.Add(NavTree);
                }
            }

            return list;
        }
        #endregion

    }
}
