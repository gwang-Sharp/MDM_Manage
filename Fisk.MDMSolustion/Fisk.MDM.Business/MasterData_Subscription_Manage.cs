using Dapper;
using Fisk.MDM.DataAccess;
using Fisk.MDM.DataAccess.Models;
using Fisk.MDM.Interface;
using Fisk.MDM.Utility.Common;
using Fisk.MDM.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Fisk.MDM.Business
{
    /// <summary>
    /// 主数据订阅管理
    /// </summary>
    public class MasterData_Subscription_Manage: IMasterData_Subscription_Manage
    {
        private readonly MDMDBContext _dbContext;
        public MasterData_Subscription_Manage(MDMDBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        #region 数据订阅 WG
        /// <summary>
        /// 删除实体订阅数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public object DelSubscription(int Id)
        {
            Result result = new Result();
            try
            {
                using (IDbConnection con = DapperContext.Connection())
                {
                    var parms = new DynamicParameters();
                    parms.Add("@Id", Id, DbType.Int32);
                    int rows = con.Execute("delete from system_subscription where Id=@Id", parms);
                    if (rows > 0)
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
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 添加实体订阅数据
        /// </summary>
        /// <param name="FormModel"></param>
        /// <returns></returns>
        public object AddSubscription(string FormModel)
        {
            Result result = new Result();
            try
            {

                var Subscription = JsonConvert.DeserializeObject<system_subscription>(FormModel);
                if (this._dbContext.system_subscription.AsNoTracking().Any(it => it.Name == Subscription.Name && it.EntityID == Subscription.EntityID))
                {
                    result.message = "存在相同订阅名";
                    result.success = false;
                    return result;
                }
                Subscription.CreateTime = DateTime.Now;
                Subscription.CreateUser = CurrentUser.UserAccount;
                Subscription.UpdateTime = DateTime.Now;
                Subscription.UpdateUser = CurrentUser.UserAccount;
                this._dbContext.Add(Subscription);
                int rows = this._dbContext.SaveChanges();
                if (rows > 0)
                {
                    result.message = "添加成功";
                    result.success = true;
                }
                else
                {
                    result.message = "添加失败";
                    result.success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 更新实体订阅数据
        /// </summary>
        /// <param name="FormModel"></param>
        /// <returns></returns>
        public object UpdataSubscription(string FormModel)
        {
            Result result = new Result();
            try
            {
                var Subscription = JsonConvert.DeserializeObject<system_subscription>(FormModel);
                var has = this._dbContext.system_subscription.Where(it => it.Id == Subscription.Id).FirstOrDefault();
                has.Name = Subscription.Name;
                has.SubscriptionRemark = Subscription.SubscriptionRemark;
                has.AttributeID = Subscription.AttributeID;
                has.UpdateTime = DateTime.Now;
                has.UpdateUser = CurrentUser.UserAccount;
                has.Apiaddress = Subscription.Apiaddress;
                this._dbContext.Entry(has).State = EntityState.Modified;
                this._dbContext.Entry(has).Property(it => it.EntityID).IsModified = false;
                this._dbContext.Entry(has).Property(it => it.CreateTime).IsModified = false;
                this._dbContext.Entry(has).Property(it => it.CreateUser).IsModified = false;
                int rows = this._dbContext.SaveChanges();
                if (rows > 0)
                {
                    result.message = "更新成功";
                    result.success = true;
                }
                else
                {
                    result.message = "更新失败";
                    result.success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取实体相关属性
        /// </summary>
        /// <param name="entityid"></param>
        /// <returns></returns>
        public object AttributesGet_ByEntityID(int entityid)
        {
            Result result = new Result();
            try
            {
                var data = this._dbContext.system_attribute.AsNoTracking().Where(it => it.EntityID == entityid).Select(it => new { it.Id, it.Name }).ToList();
                result.data = data;
                result.message = "查询成功";
                result.success = true;
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// 初始化数据订阅列表
        /// </summary>
        /// <param name="EntityID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public object InitSubscriptionTable(string EntityID, int page, int rows)
        {
            tableResult result = new tableResult();
            try
            {
                IQueryable<dynamic> queryable = null;
                if (!string.IsNullOrEmpty(EntityID))
                {
                    queryable = this._dbContext.system_subscription.AsNoTracking().Where(it => it.EntityID.ToString() == EntityID);
                }
                else
                {
                    queryable = this._dbContext.system_subscription.AsNoTracking();
                }
                result.data = queryable.Skip((page - 1) * rows).Take(rows).ToList();
                result.message = "查询成功";
                result.success = true;
                result.total = queryable.Count();
                return result;
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        #endregion
    }
}
