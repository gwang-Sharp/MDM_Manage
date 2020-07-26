using Dapper;
using ExcelDataReader;
using Fisk.MDM.DataAccess;
using Fisk.MDM.DataAccess.Models;
using Fisk.MDM.Interface;
using Fisk.MDM.Utility.Common;
using Fisk.MDM.ViewModel;
using Fisk.MDM.ViewModel.System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
namespace Fisk.MDM.Business
{
    public class MasterDataManage : IMasterDataManage
    {
        //private readonly ILogger<MasterDataManage> _logger;
        private readonly MDMDBContext _dbContext;
        private IHttpClientFactory _httpClientFactory; //注入HttpClient工厂类

        public MasterDataManage(MDMDBContext dbContext, IHttpClientFactory httpClientFactory)
        {
            this._dbContext = dbContext;
            //this._logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 获取实体下属性
        /// </summary>
        /// <param name="EntityID"></param>
        /// <returns></returns>
        public object GetAll_Attributes(int EntityID)
        {
            Result result = new Result();
            try
            {
                var attrs = this._dbContext.system_attribute.AsNoTracking().Where(it => it.EntityID == EntityID).Select(it => it.Name).ToList();
                result.success = true;
                result.message = "查询成功";
                result.data = attrs;
                return result;
            }
            catch (Exception ex)
            {
                result.success = true;
                result.message = "查询失败，系统错误";
                result.data = "";
                return result;
            }
        }


        /// <summary>
        /// 公共方法—— 发送http get 请求  2020年6月2日11:22:11  Dennyhui
        /// <para>最终以url参数的方式提交</para>
        /// </summary>
        /// <param name="parameters">参数字典,可为空</param>
        /// <param name="requestUri">例如/api/Files/UploadFile</param>
        /// <returns></returns>
        public async Task<string> Client_Get(Dictionary<string, string> parameters, string requestUri, string token)
        {
            //从工厂获取请求对象
            var client = _httpClientFactory.CreateClient();
            //添加请求头
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            //拼接地址
            if (parameters != null)
            {
                var strParam = string.Join("&", parameters.Select(o => o.Key + "=" + o.Value));
                requestUri = string.Concat(requestUri, '?', strParam);
            }
            client.BaseAddress = new Uri(requestUri);
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json; charset=utf-8");
            return client.GetStringAsync(requestUri).Result;
        }
        /// <summary>
        /// 公共方法—— 发送http post请求  2020年6月2日11:20:42  Dennyhui
        /// </summary>
        /// <param name="formData">参数</param>
        /// <param name="requestUri">请求地址</param>
        /// <param name="token">身份验证秘钥，可为空</param>
        /// <returns></returns>
        public async Task<string> Client_Post(MultipartFormDataContent formData, string requestUri, string token)
        {
            //从工厂获取请求对象
            var client = _httpClientFactory.CreateClient();
            //添加请求头
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", token);
            }
            HttpResponseMessage response = client.PostAsync(requestUri, formData).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        #region 模型管理 wyt

        public object SearchModel(int limit, int page, string where)
        {
            tableResult result = new tableResult();
            try
            {
                var ModelQuery = _dbContext.system_model.AsNoTracking();
                if (!string.IsNullOrEmpty(where))
                {
                    ModelQuery = ModelQuery.Where(e => e.Name.ToLower().Contains(where.ToLower()) || e.Remark.ToLower().Contains(where.ToLower()));
                }
                result.data = ModelQuery.Skip((page - 1) * limit).Take(limit).OrderByDescending(e => e.CreateTime).ToList();
                result.total = ModelQuery.Count();
                result.success = true;
                result.message = "查询成功";
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public object InserModel(system_model model)
        {
            Result result = new Result();
            try
            {
                var Name = _dbContext.system_model.Where(e => e.Name == model.Name).FirstOrDefault();
                if (Name != null)
                {
                    result.success = false;
                    result.message = "模型名称重复";
                    return result;
                }
                model.Creater = CurrentUser.UserAccount;
                _dbContext.system_model.Add(model);

                var flag = _dbContext.SaveChanges();
                if (flag != 0)
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

        public object UpdateModel(system_model model)
        {
            Result result = new Result();
            system_model data = _dbContext.system_model.Find(model.Id);
            data.Name = model.Name;
            data.Remark = model.Remark;
            data.LogRetentionDays = model.LogRetentionDays;
            data.UpdateTime = Microsoft.VisualBasic.DateAndTime.Now;
            data.Updater = CurrentUser.UserAccount;

            try
            {
                if (data != null)
                {

                    _dbContext.Update(data);
                    _dbContext.SaveChanges();
                    result.success = true;
                    result.message = "更新成功！";

                }
                else
                {
                    result.success = false;
                    result.message = "更新失败,Err:缺少必要的参数！";

                }
            }
            catch (Exception ex)
            {
                throw;

            }
            return result;

        }

        public object DeleteModel(system_model model)
        {
            Result result = new Result();
            try
            {
                system_model data = _dbContext.system_model.Find(model.Id);
                var delModel = _dbContext.Remove(data);
                var flag = this._dbContext.SaveChanges();
                if (flag != 0)
                {
                    var Enames = this._dbContext.system_entity.AsNoTracking().Where(it => it.ModelId == delModel.Entity.Id).ToList();
                    Enames.ForEach(e =>
                    {
                        system_drop_table_entity(e);
                    });
                    MySqlParameter sqlParameter = new MySqlParameter("@ModelID",MySqlDbType.VarChar);
                    sqlParameter.Value = delModel.Entity.Id;
                    this._dbContext.Database.ExecuteSqlRaw($"delete from system_entity where ModelID =@ModelID",sqlParameter);
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
        #endregion


        #region 属性管理 WG
        ///调用system_drop_attribute存储过程为表删除列
        public void system_drop_attribute(system_attribute attribute)
        {
            try
            {
                MySqlParameter[] parameters = {
                        new MySqlParameter("@EntityID", MySqlDbType.Int32),
                        new MySqlParameter("@Attribute", MySqlDbType.VarChar,50),
                    };
                parameters[0].Value = attribute.EntityID;
                parameters[1].Value = attribute.Name;
                string sql = "call system_drop_attribute(@EntityID,@Attribute)";
                this._dbContext.Database.ExecuteSqlRaw(sql, parameters);
                CreateEntityValiditeProc(attribute.EntityID.Value);
                //调用导入数据存储过程
                var entity = this._dbContext.system_entity.AsNoTracking().Where(it => it.Id == attribute.EntityID.Value).FirstOrDefault();
                CreateEntityDataImportProc(entity);
                AttributeRuleRelease(entity.ModelId.Value, entity.Id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        ///调用system_create_attribute存储过程为表加列
        public void system_create_attribute(system_attribute attribute)
        {
            try
            {
                MySqlParameter[] parameters = {
                        new MySqlParameter("@EntityID", MySqlDbType.Int32),
                        new MySqlParameter("@Attribute", MySqlDbType.VarChar,50),
                        new MySqlParameter("@AttributeType", MySqlDbType.VarChar,255),
                    };
                parameters[0].Value = attribute.EntityID;
                parameters[1].Value = attribute.Name;
                switch (attribute.DataType)
                {
                    case "文本": parameters[2].Value = $" varchar({attribute.TypeLength}) "; break;
                    case "数字": parameters[2].Value = " int "; break;
                    case "时间": parameters[2].Value = " datetime "; break;
                    case "小数": parameters[2].Value = " decimal(18,4) "; break;
                    case "文件": parameters[2].Value = " blob"; break;
                    default: parameters[2].Value = " varchar(400) "; break;
                }
                string sql = "call system_create_attribute(@EntityID,@Attribute,@AttributeType)";
                this._dbContext.Database.ExecuteSqlRaw(sql, parameters);
                CreateEntityValiditeProc(attribute.EntityID.Value);
                //调用创建数据导入存储过程
                var entity = this._dbContext.system_entity.AsNoTracking().Where(it => it.Id == attribute.EntityID.Value).FirstOrDefault();
                CreateEntityDataImportProc(entity);
                AttributeRuleRelease(entity.ModelId.Value, entity.Id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public object AddAttribute(system_attribute attribute)
        {
            Result result = new Result();
            try
            {
                if (this._dbContext.system_attribute.AsNoTracking().Any(it => it.Name == attribute.Name && it.EntityID == attribute.EntityID))
                {
                    result.success = false;
                    result.message = "已存在相同属性名";
                    return result;
                }
                attribute.Creater = CurrentUser.UserAccount;
                attribute.CreateTime = DateTime.Now;
                attribute.TypeLength = string.IsNullOrWhiteSpace(attribute.DataType) ? 0 : attribute.TypeLength;
                attribute.Updater = CurrentUser.UserAccount;
                attribute.UpdateTime = DateTime.Now;
                attribute.IsDisplay = attribute.IsDisplay ?? 1;
                attribute.IsDefault = 0;
                attribute.IsSystem = 0;
                this._dbContext.Add(attribute);
                var flag = this._dbContext.SaveChanges();
                if (flag != 0)
                {
                    system_create_attribute(attribute);
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
        /// 获取所有模型
        /// </summary>
        /// <returns></returns>
        public object Attributes_ModelsGet()
        {
            Result result = new Result();
            try
            {
                var data = this._dbContext.system_model.AsNoTracking().Select(it => new { it.Id, it.Name }).ToList();
                result.success = true;
                result.message = "查询成功";
                result.data = data;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns></returns>
        public object Attributes_EntitysGet()
        {
            Result result = new Result();
            try
            {
                var data = this._dbContext.system_entity.AsNoTracking().Select(it => new { it.Id, it.Name, it.ModelId }).ToList();
                result.success = true;
                result.message = "查询成功";
                result.data = data;

            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 更新属性
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public object AttributeUpdate(system_attribute attribute)
        {
            Result result = new Result();
            try
            {
                var ExistData = this._dbContext.system_attribute.Where(it => it.Id == attribute.Id).FirstOrDefault();
                var entity = this._dbContext.system_entity.AsNoTracking().Where(it => it.Id == ExistData.EntityID).FirstOrDefault();
                if (ExistData.TypeLength != attribute.TypeLength)
                {
                    this._dbContext.Database.ExecuteSqlRaw($"ALTER TABLE {entity.EntityTable} MODIFY {attribute.Name} varchar({attribute.TypeLength}) ");//修改mdm表字段长度
                }
                this._dbContext.Entry(ExistData).State = EntityState.Modified;
                ExistData.DisplayName = attribute.DisplayName;
                ExistData.Remark = attribute.Remark;
                ExistData.UpdateTime = DateTime.Now;
                ExistData.Updater = CurrentUser.UserAccount;
                ExistData.StartTrace = attribute.StartTrace;
                ExistData.TypeLength = attribute.TypeLength;
                ExistData.IsDisplay = attribute.IsDisplay;
                this._dbContext.Entry(ExistData).Property(it => it.Creater).IsModified = false;
                this._dbContext.Entry(ExistData).Property(it => it.Name).IsModified = false;
                this._dbContext.Entry(ExistData).Property(it => it.Type).IsModified = false;
                this._dbContext.Entry(ExistData).Property(it => it.DataType).IsModified = false;
                this._dbContext.Entry(ExistData).Property(it => it.LinkModelID).IsModified = false;
                this._dbContext.Entry(ExistData).Property(it => it.LinkEntityID).IsModified = false;
                this._dbContext.Entry(ExistData).Property(it => it.IsDefault).IsModified = false;
                this._dbContext.Entry(ExistData).Property(it => it.IsSystem).IsModified = false;
                this._dbContext.Entry(ExistData).Property(it => it.CreateTime).IsModified = false;
                this._dbContext.Entry(ExistData).Property(it => it.EntityID).IsModified = false;
                int rows = this._dbContext.SaveChanges();
                if (rows >= 0)
                {
                    CreateEntityValiditeProc(ExistData.EntityID.Value);
                    CreateEntityDataImportProc(entity);
                    AttributeRuleRelease(entity.ModelId.Value, entity.Id);
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
        /// 删除属性
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public object AttributeDel(int ID)
        {
            Result result = new Result();
            try
            {
                var DelAttribute = this._dbContext.system_attribute.Where(it => it.Id == ID).FirstOrDefault();
                if (DelAttribute.IsDefault == 1)
                {
                    result.message = "该属性为默认属性,不能删除";
                    result.success = false;
                    return result;
                }
                var delAttr = this._dbContext.Remove(DelAttribute);
                int rows = this._dbContext.SaveChanges();
                if (rows >= 0)
                {
                    system_drop_attribute(delAttr.Entity);
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
        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <returns></returns>
        public object AttributesGet(int id, int page, int rows)
        {
            tableResult result = new tableResult();
            try
            {
                var obj = this._dbContext.system_attribute.AsNoTracking();
                if (id != 0)
                {
                    obj = obj.Where(it => it.EntityID == id);
                }
                result.success = true;
                result.message = "查询成功";
                result.data = obj.Skip((page - 1) * rows).Take(rows).ToList();
                result.total = obj.Count();

            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        /// <summary>
        /// 获取所有属性
        /// </summary>
        /// <returns></returns>
        public object Attributes_GetAll()
        {
            Result result = new Result();
            try
            {
                var obj = this._dbContext.system_attribute.AsNoTracking().Where(it => it.StartTrace == "1").Select(it => new { it.Name, it.EntityID, it.Id }).ToList();
                result.success = true;
                result.message = "查询成功";
                result.data = obj;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        #endregion

        #region 实体管管理 hhyang WG

        #region 实体操作对数据库操作 wg
        /// <summary>
        /// 删除stage表格数据
        /// </summary>
        /// <param name="EntityID"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public object DelStageData(int EntityID, int id)
        {
            Result result = new Result();
            try
            {
                var stageTable = this._dbContext.system_entity.AsNoTracking().Where(it => it.Id == EntityID).FirstOrDefault().StageTable.ToLower();
                using (IDbConnection con = DapperContext.Connection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id, DbType.Int32);
                    var rows = con.Execute($"delete from {stageTable} where Id=@ID", parameters);
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
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 通过实体去刷新相关存储过程
        /// </summary>
        /// <returns></returns>
        public object RefreshEntity()
        {
            Result result = new Result();
            try
            {
                result.success = false;
                result.message = "待完成......";
                var AllEntitys = this._dbContext.system_entity.AsNoTracking().ToList();
                var ParallelResult = Parallel.ForEach(AllEntitys, e =>
                {
                    CreateEntityDataImportProc(e);
                    CreateEntityValiditeProc(e.Id);
                    //AttributeRuleRelease(e.ModelId.Value, e.Id);
                });
                if (ParallelResult.IsCompleted)
                {
                    result.success = true;
                    result.message = "更新成功";
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 删除实体  2020年 4月 20日 hhyang
        /// </summary>
        /// <param name="model">删除所需要的参数</param>
        /// <returns></returns>
        public object DeleteEntity(EntityManageItem model)
        {
            Result result = new Result();
            try
            {
                var list = _dbContext.system_entity.Where(x => x.Id == model.Id).FirstOrDefault();
                var delEntity = _dbContext.system_entity.Remove(list);
                var flag = _dbContext.SaveChanges();
                if (flag != 0)
                {
                    system_drop_table_entity(delEntity.Entity);
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
        /// 添加实体数据导入存储过程逻辑
        /// </summary>
        /// <param name="builder"></param>
        public void CreateEntityDataImportProc(system_entity _Entity)
        {
            using (var tran = this._dbContext.Database.BeginTransaction())
            {
                try
                {
                    var dropSql = $"DROP PROCEDURE IF EXISTS { _Entity.DataImportProc}";
                    _dbContext.Database.ExecuteSqlRaw(dropSql);  //判断该存储过程是否存在,若存在则删除
                    StringBuilder builder = new StringBuilder();
                    builder.Append("CREATE PROCEDURE " + _Entity.DataImportProc + "(IN batchid VARCHAR(100)) ");
                    builder.Append(" BEGIN ");

                    var AttributeData = _dbContext.system_attribute.Where(e => e.EntityID == _Entity.Id).ToList();
                    string attributes = "";
                    string stgAttributes = "";
                    string updateSql = "";
                    foreach (var item in AttributeData)
                    {
                        attributes += $" {item.Name} ,";
                        stgAttributes += $" S.{item.Name} ,";
                        if (item.Name != "Code" && item.Name != "CreateTime" && item.Name != "CreateUser")
                        {
                            updateSql += $@" {_Entity.EntityTable}.{item.Name} =(CASE WHEN S.{item.Name} IS NULL OR S.{item.Name} = '' THEN {_Entity.EntityTable}.{item.Name}  WHEN S.{item.Name} = '~' THEN NULL  ELSE S.{item.Name} END)  ,";
                        }
                    }
                    attributes = attributes.TrimEnd(',');
                    stgAttributes = stgAttributes.TrimEnd(',');
                    updateSql = updateSql.TrimEnd(',');


                    //开始拼接存在 更新语句
                    builder.Append($" UPDATE {_Entity.EntityTable} ,  {_Entity.StageTable} AS S");
                    builder.Append($" SET {updateSql} WHERE ");
                    builder.Append($@" {_Entity.EntityTable}.Code = S.Code AND S.batch_id = batchid  AND   S.BissnessRuleStatus='1' AND S.ValidateStatus='1'  AND S.WorkFlowStatus = '0'  ;");

                    //拼接添加语句
                    builder.Append("INSERT INTO " + _Entity.EntityTable + "( ");
                    builder.Append(attributes + " ) ");
                    builder.Append("SELECT  " + stgAttributes + " FROM " + _Entity.StageTable + " AS S");
                    builder.Append(" LEFT JOIN  " + _Entity.EntityTable + $" ON S.Code = {_Entity.EntityTable}.Code ");
                    builder.Append($" WHERE S.batch_id = batchid  AND   S.BissnessRuleStatus='1' AND S.ValidateStatus='1' AND S.WorkFlowStatus = '0'  AND " + _Entity.EntityTable + ".Code IS NULL ;   ");  // 目标表不存在则添加
                    builder.Append(" END");
                    this._dbContext.Database.ExecuteSqlRaw(builder.ToString());
                    builder.Clear();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }
        }

        /// <summary>
        /// 添加实体验证存储过程逻辑
        /// </summary>
        /// <param name="builder"></param>
        public void CreateEntityValiditeProc(int EntityID)
        {
            using (var tran = this._dbContext.Database.BeginTransaction())
            {
                try
                {
                    StringBuilder builder = new StringBuilder();
                    var entity = this._dbContext.system_entity.AsNoTracking().Where(it => it.Id == EntityID).FirstOrDefault();//获取临时表名
                                                                                                                              //获取所有属性
                    var attrDetails = this._dbContext.system_attribute.AsNoTracking().Where(it => it.EntityID == EntityID).Select(it => new { it.Name, it.DataType, it.TypeLength, it.LinkEntityID, it.DisplayName }).ToList();
                    var dropSql = $"DROP PROCEDURE IF EXISTS {entity.ValiditeProc}";
                    this._dbContext.Database.ExecuteSqlRaw(dropSql);//判断该存储过程是否存在,若存在则删除
                    builder.Append("CREATE PROCEDURE " + entity.ValiditeProc + "(in batchid varchar(50)) ");
                    builder.Append("begin ");
                    builder.Append($" DROP TABLE if EXISTS Temp_{entity.StageTable};");
                    //创建批次数据临时表
                    builder.Append(@$"  CREATE TEMPORARY TABLE Temp_{entity.StageTable}(
                                       select * from {entity.StageTable} where batch_id=batchid
                                       );");
                    //创建中间临时表
                    builder.Append(@$"  CREATE TEMPORARY TABLE Middle_Temp_{entity.StageTable}(
                                       select * from Temp_{entity.StageTable}
                                       );");
                    builder.Append($" delete from {entity.StageTable} where batch_id=batchid;");
                    builder.Append($" update Temp_{entity.StageTable} set ValidateErrorDesc=null;");
                    //同一批batchid中code重复数据
                    builder.Append($" update Temp_{entity.StageTable} S, (select code  ,batch_id  from Middle_Temp_{entity.StageTable} GROUP BY code HAVING count(1) > 1 and code is not null ) AS M  SET S.ValidateErrorDesc=CONCAT('Code重复,', IFNULL(ValidateErrorDesc,'') ),ValidateStatus='2' WHERE M.code = S.code  AND S.batch_id = batchid;");
                    builder.Append($" update Temp_{entity.StageTable} S, (select code  ,batch_id  from Middle_Temp_{entity.StageTable} GROUP BY code HAVING count(1) = 1 and code is not null ) AS M  SET S.ValidateStatus='1' WHERE M.code = S.code AND S.batch_id = batchid  and isnull(ValidateErrorDesc);");
                    foreach (var item in attrDetails)
                    {
                        switch (item.DataType)
                        {
                            case "文本":
                                //文本长度验证
                                builder.Append($" update Temp_{entity.StageTable} set {item.Name}=null where  {item.Name}='' ;");
                                //builder.Append($" update Temp_{entity.StageTable} set ValidateErrorDesc= case  ");
                                //builder.Append($" when  LENGTH({item.Name})>{item.TypeLength} then CONCAT('{item.DisplayName ?? item.Name}字段数据长度超出,',IFNULL(ValidateErrorDesc,'')) ");
                                //builder.Append($" when  LENGTH({item.Name})<={item.TypeLength} then null");
                                //builder.Append($" when  {item.Name} is null then null");
                                //builder.Append(" END ,");
                                //builder.Append($" ValidateStatus= case  ");
                                //builder.Append($" when  LENGTH({item.Name})>{item.TypeLength} then '2' ");
                                //builder.Append($" when  LENGTH({item.Name})<={item.TypeLength} then '1' ");
                                //builder.Append($" when  {item.Name} is null then '0'");
                                //builder.Append(" END ");
                                //builder.Append(" where batch_id=batchid;");
                                builder.Append($" update Temp_{entity.StageTable} set ValidateErrorDesc=CONCAT('{item.DisplayName ?? item.Name}字段数据长度超出,',IFNULL(ValidateErrorDesc,'')) ,ValidateStatus='2' where  LENGTH({item.Name})>{item.TypeLength} ;");
                                builder.Append($" update Temp_{entity.StageTable} set ValidateStatus='1' where  LENGTH({item.Name})<={item.TypeLength} and isnull(ValidateErrorDesc);");
                                break;
                            case "数字":
                                //数据类型验证
                                builder.Append($" update Temp_{entity.StageTable} set {item.Name}=null where  {item.Name}='' ;");
                                //builder.Append($" update Temp_{entity.StageTable} set ValidateErrorDesc= case  ");
                                //builder.Append($" when {item.Name} REGEXP '[^0-9.]' then CONCAT('{item.DisplayName ?? item.Name}字段数据类型应为数字,', IFNULL(ValidateErrorDesc,'')) ");
                                //builder.Append($" when {item.Name} NOT REGEXP '[^0-9.]' then null");
                                //builder.Append($" when {item.Name}  is null  then null");
                                //builder.Append(" END ,");
                                //builder.Append($" ValidateStatus= case  ");
                                //builder.Append($" when {item.Name} REGEXP '[^0-9.]' then '2' ");
                                //builder.Append($" when {item.Name} NOT REGEXP '[^0-9.]' then '1'");
                                //builder.Append($" when {item.Name} is null  then '0'");
                                //builder.Append(" END ");
                                //builder.Append(" where batch_id=batchid;");
                                builder.Append($" update Temp_{entity.StageTable} set ValidateErrorDesc= CONCAT('{item.DisplayName ?? item.Name}字段数据类型应为数字,', IFNULL(ValidateErrorDesc,'')),ValidateStatus='2' where  {item.Name} REGEXP '[^0-9.]' ;");
                                builder.Append($" update Temp_{entity.StageTable} set ValidateStatus='1' where  {item.Name} NOT REGEXP '[^0-9.]'  and isnull(ValidateErrorDesc); ");
                                break;
                            case "小数":
                                //数据类型验证
                                builder.Append($" update Temp_{entity.StageTable} SET {item.Name}=null where {item.Name}='' ;");
                                //builder.Append($" update Temp_{entity.StageTable} set ValidateErrorDesc= case  ");
                                //builder.Append($" when {item.Name} REGEXP ");
                                //builder.Append(@"'[^[1-9](\d{0,9})((\.\d{1,2})?)$]'");
                                //builder.Append($" or {item.Name} like '%.' ");
                                //builder.Append($" then CONCAT('{item.DisplayName ?? item.Name}字段数据类型应为小数,', IFNULL(ValidateErrorDesc,''))");
                                //builder.Append($" when {item.Name} NOT REGEXP ");
                                //builder.Append(@"'[^[1-9](\d{0,9})((\.\d{1,2})?)$]'");
                                //builder.Append($" or {item.Name} like '%.' ");
                                //builder.Append(" then null ");
                                //builder.Append($" when {item.Name}  is null  then null ");
                                //builder.Append(" END ,");
                                //builder.Append($" ValidateStatus= case  ");
                                //builder.Append($" when {item.Name} REGEXP ");
                                //builder.Append(@"'[^[1-9](\d{0,9})((\.\d{1,2})?)$]'");
                                //builder.Append($" or {item.Name} like '%.' ");
                                //builder.Append($" then '2' ");
                                //builder.Append($" when {item.Name} NOT REGEXP ");
                                //builder.Append(@"'[^[1-9](\d{0,9})((\.\d{1,2})?)$]'");
                                //builder.Append($" or {item.Name} like '%.' ");
                                //builder.Append(" then '1'");
                                //builder.Append($" when {item.Name} is null  then '0'");
                                //builder.Append(" END");
                                //builder.Append(" where batch_id=batchid;");
                                builder.Append($@" update Temp_{entity.StageTable}  set ValidateErrorDesc= CONCAT('{item.DisplayName ?? item.Name}字段数据类型应为小数,', IFNULL(ValidateErrorDesc,'')),ValidateStatus='2' where  {item.Name} REGEXP ");
                                builder.Append(@"'[^[1-9](\d{0,9})((\.\d{1,2})?)$]'");
                                builder.Append($" or {item.Name} like '%.'  ;");
                                builder.Append($@" update Temp_{entity.StageTable} set ValidateStatus='1' where  {item.Name} NOT REGEXP ");
                                builder.Append(@"'[^[1-9](\d{0,9})((\.\d{1,2})?)$]'");
                                builder.Append($" and {item.Name}  not like '%.' and isnull(ValidateErrorDesc); ");
                                break;
                            case "时间":
                                builder.Append($" update Temp_{entity.StageTable} SET {item.Name}=null where {item.Name}='' ;");
                                //builder.Append($" update {entity.StageTable} set ValidateErrorDesc= case  ");
                                //builder.Append($" when {item.Name} not REGEXP ");
                                //builder.Append(@"'[^(\d{4}|\d{2})(\-|\/|\.)\d{1,2}\3\d{1,2}$)|(^\d{4}\d{1,2}\d{1,2}$)]'");
                                //builder.Append($"   and {item.Name} is not null ");
                                //builder.Append($" then CONCAT('{item.DisplayName ?? item.Name}字段格式应为年-月-日 时:分:秒,', IFNULL(ValidateErrorDesc,''))");
                                //builder.Append($" when {item.Name}  REGEXP ");
                                //builder.Append(@"'[^(\d{4}|\d{2})(\-|\/|\.)\d{1,2}\3\d{1,2}$)|(^\d{4}\d{1,2}\d{1,2}$)]' and isnull(ValidateErrorDesc) ");
                                //builder.Append(" then null ");
                                //builder.Append($" when {item.Name}  is null  then null");
                                //builder.Append(" END ,");
                                //builder.Append($" ValidateStatus= case ");
                                //builder.Append($" when {item.Name} not REGEXP ");
                                //builder.Append(@"'[^(\d{4}|\d{2})(\-|\/|\.)\d{1,2}\3\d{1,2}$)|(^\d{4}\d{1,2}\d{1,2}$)]'");
                                //builder.Append($"   and {item.Name} is not null ");
                                //builder.Append($" then '2' ");
                                //builder.Append($" when {item.Name}  REGEXP ");
                                //builder.Append(@"'[^(\d{4}|\d{2})(\-|\/|\.)\d{1,2}\3\d{1,2}$)|(^\d{4}\d{1,2}\d{1,2}$)]' and isnull(ValidateErrorDesc) ");
                                //builder.Append(" then '1' ");
                                //builder.Append($" when {item.Name} is null  then '0'");
                                //builder.Append(" END ");
                                //builder.Append(" where batch_id=batchid;");

                                builder.Append($" update Temp_{entity.StageTable}  set ValidateErrorDesc= CONCAT('{item.DisplayName ?? item.Name}字段格式应为年-月-日 时:分:秒,', IFNULL(ValidateErrorDesc,'')),ValidateStatus='2' where  {item.Name} not REGEXP  ");
                                builder.Append(@"'[^(\d{4}|\d{2})(\-|\/|\.)\d{1,2}\3\d{1,2}$)|(^\d{4}\d{1,2}\d{1,2}$)]'");
                                builder.Append($"   and {item.Name} is not null;");
                                builder.Append($" update Temp_{entity.StageTable}  set ValidateStatus='1' where  {item.Name} REGEXP  ");
                                builder.Append(@"'[^(\d{4}|\d{2})(\-|\/|\.)\d{1,2}\3\d{1,2}$)|(^\d{4}\d{1,2}\d{1,2}$)]' and isnull(ValidateErrorDesc);");
                                break;
                            case "":
                                //基于域的验证
                                builder.Append($" update Temp_{entity.StageTable} set {item.Name}=null where  {item.Name}='' ;");
                                var linkEntityTable = this._dbContext.system_entity.AsNoTracking().Where(it => it.Id == item.LinkEntityID).FirstOrDefault().EntityTable;
                                //builder.Append($" update {entity.StageTable} set ValidateErrorDesc= case  ");
                                //builder.Append($" when {item.Name} not in (select Code from {linkEntityTable}) and {item.Name} is not null ");
                                //builder.Append($" then CONCAT('{item.DisplayName ?? item.Name}字段基于域错误,', IFNULL(ValidateErrorDesc,''))");
                                //builder.Append($" when {item.Name} in (select Code from {linkEntityTable})  and isnull(ValidateErrorDesc) ");
                                //builder.Append($" then null ");
                                //builder.Append($" when {item.Name}  is null  then null");
                                //builder.Append(" END, ");
                                //builder.Append($" ValidateStatus= case  ");
                                //builder.Append($" when {item.Name} not in (select Code from {linkEntityTable}) and {item.Name} is not null ");
                                //builder.Append($" then '2' ");
                                //builder.Append($" when {item.Name} in (select Code from {linkEntityTable})  and isnull(ValidateErrorDesc) ");
                                //builder.Append($" then '1'");
                                //builder.Append($" when {item.Name} is null  then '0'");
                                //builder.Append(" END ");
                                //builder.Append(" where batch_id=batchid;");


                                builder.Append($" update Temp_{entity.StageTable} set ValidateErrorDesc= CONCAT('{item.DisplayName ?? item.Name}字段基于域错误,', IFNULL(ValidateErrorDesc,'')),ValidateStatus='2' where {item.Name} not in (select Code from {linkEntityTable}) and {item.Name} is not null;");
                                builder.Append($" update Temp_{entity.StageTable} set ValidateStatus='1' where {item.Name} in (select Code from {linkEntityTable})  and isnull(ValidateErrorDesc) or ISNULL({item.Name});");
                                break;
                            case "文件":
                                break;
                        }
                    }
                    builder.Append($" insert into {entity.StageTable} select * from Temp_{entity.StageTable};");

                    builder.Append($" drop table Temp_{entity.StageTable};");
                    builder.Append($" drop table Middle_Temp_{entity.StageTable};");
                    builder.Append("end");
                    builder.Replace("{", "{{").Replace("}", "}}");
                    this._dbContext.Database.ExecuteSqlRaw(builder.ToString());
                    builder.Clear();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }
        }
        /// <summary>
        /// 删除实体时删除实体的属性以及对应的实体表、临时表、数据导入、验证、业务规则存储过程
        /// </summary>
        /// <param name="system_Entity"></param>
        public void system_drop_table_entity(system_entity system_Entity)
        {
            try
            {
                var query = this._dbContext.system_attribute.Where(it => it.EntityID == system_Entity.Id);
                if (query.Any())
                {
                    this._dbContext.RemoveRange(query.ToList());
                    this._dbContext.SaveChanges();
                }

                StringBuilder builder = new StringBuilder();
                builder.Append("drop table " + system_Entity.EntityTable);
                this._dbContext.Database.ExecuteSqlRaw(builder.ToString());
                builder.Clear();
                builder.Append($" DROP TRIGGER IF EXISTS trig_{system_Entity.Name}");
                this._dbContext.Database.ExecuteSqlRaw(builder.ToString());
                builder.Clear();
                builder.Append("drop table " + system_Entity.StageTable);
                this._dbContext.Database.ExecuteSqlRaw(builder.ToString());
                builder.Clear();
                builder.Append("drop table " + system_Entity.HistoryTable);
                this._dbContext.Database.ExecuteSqlRaw(builder.ToString());
                builder.Clear();
                builder.Append("drop PROCEDURE " + system_Entity.DataImportProc);
                this._dbContext.Database.ExecuteSqlRaw(builder.ToString());
                builder.Clear();
                builder.Append("drop PROCEDURE " + system_Entity.ValiditeProc);
                this._dbContext.Database.ExecuteSqlRaw(builder.ToString());
                builder.Clear();
                builder.Append("drop PROCEDURE " + system_Entity.BusinessProc);
                this._dbContext.Database.ExecuteSqlRaw(builder.ToString());
                builder.Clear();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// 创建实体表以及添加默认属性创建验证、数据导入及业务规则存储过程
        /// </summary>
        /// <param name="system_Entity"></param>
        public void system_create_table_entity(system_entity system_Entity)
        {
            try
            {
                string sql = "";
                //调用创建表和临时表以及相应存储过程创建的存储过程
                MySqlParameter[] parameters = null;

                if (system_Entity.AutoCreateCode == 0)
                {
                    parameters = new MySqlParameter[]{
                        new MySqlParameter("@RowId", MySqlDbType.Int32),
                        new MySqlParameter("@Seed", MySqlDbType.Int32),
                    };
                    parameters[0].Value = system_Entity.Id;
                    parameters[1].Value = system_Entity.BeganIn;
                    sql = "call system_create_table_entity_BYSeed(@RowId,@Seed)";
                    this._dbContext.Database.ExecuteSqlRaw(sql, parameters);
                    system_create_table_trigger(system_Entity);
                }
                else
                {
                    parameters = new MySqlParameter[]{
                        new MySqlParameter("@RowId", MySqlDbType.Int32),
                    };
                    parameters[0].Value = system_Entity.Id;
                    sql = "call system_create_table_entity(@RowId)";
                    this._dbContext.Database.ExecuteSqlRaw(sql, parameters);
                }
                List<system_attribute> attrs = new List<system_attribute>();
                //获取表的列
                string QueryTableColumn = $"select COLUMN_NAME from information_schema.COLUMNS where table_name = '{system_Entity.EntityTable}'  and table_schema = 'mdmdb'";
                using (MySqlConnection dbcon = this._dbContext.Database.GetDbConnection() as MySqlConnection)
                {
                    if (dbcon.State == ConnectionState.Closed)
                    {
                        dbcon.Open();
                    }
                    //查找mdm表里的默认字段
                    MySqlDataAdapter adapter = new MySqlDataAdapter(QueryTableColumn, dbcon);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][0].ToString().ToLower() == "id")
                        {
                            continue;
                        }
                        system_attribute _Attribute = new system_attribute();
                        _Attribute.Name = dt.Rows[i][0].ToString();
                        _Attribute.Remark = "";
                        _Attribute.Type = "自由格式";
                        _Attribute.StartTrace = "0";
                        #region 字段
                        if (_Attribute.Name.ToLower() == "createtime")
                        {
                            _Attribute.DisplayName = "创建时间";
                            _Attribute.DataType = "时间";
                            _Attribute.TypeLength = 0;
                            _Attribute.IsSystem = 1;
                        }
                        else if (_Attribute.Name.ToLower() == "updatetime")
                        {
                            _Attribute.DisplayName = "最近更新时间";
                            _Attribute.DataType = "时间";
                            _Attribute.TypeLength = 0;
                            _Attribute.IsSystem = 1;
                        }
                        else if (_Attribute.Name.ToLower() == "updateuser")
                        {
                            _Attribute.DisplayName = "最近更新人";
                            _Attribute.DataType = "文本";
                            _Attribute.TypeLength = 50;
                            _Attribute.IsSystem = 1;
                        }
                        else if (_Attribute.Name.ToLower() == "createuser")
                        {
                            _Attribute.DisplayName = "创建人";
                            _Attribute.DataType = "文本";
                            _Attribute.TypeLength = 50;
                            _Attribute.IsSystem = 1;
                        }
                        else if (_Attribute.Name.ToLower() == "name")
                        {
                            _Attribute.DisplayName = "名称";
                            _Attribute.DataType = "文本";
                            _Attribute.TypeLength = 200;
                            _Attribute.IsSystem = 0;
                        }
                        else if (_Attribute.Name.ToLower() == "code")
                        {
                            _Attribute.DisplayName = "编号";
                            _Attribute.DataType = "文本";
                            _Attribute.TypeLength = 50;
                            _Attribute.IsSystem = 0;
                        }
                        else if (_Attribute.Name.ToLower() == "validity")
                        {
                            _Attribute.DisplayName = "有效性";
                            _Attribute.DataType = "数字";
                            _Attribute.TypeLength = 0;
                            _Attribute.IsSystem = 0;
                        }
                        else
                        {
                            _Attribute.DisplayName = "";
                            _Attribute.DataType = "文本";
                            _Attribute.TypeLength = 50;
                            _Attribute.IsSystem = 0;
                        }
                        #endregion
                        _Attribute.Creater = CurrentUser.UserAccount;
                        _Attribute.CreateTime = DateTime.Now;
                        _Attribute.Updater = CurrentUser.UserAccount;
                        _Attribute.UpdateTime = DateTime.Now;
                        _Attribute.EntityID = system_Entity.Id;
                        _Attribute.LinkEntityID = 0;
                        _Attribute.LinkModelID = 0;
                        _Attribute.IsDefault = 1;
                        _Attribute.IsDisplay = 1;
                        attrs.Add(_Attribute);
                    }
                    dbcon.Close();
                    this._dbContext.AddRange(attrs);
                    int res = this._dbContext.SaveChanges();
                    if (res > 0)
                    {
                        //添加数据导入存储过程逻辑
                        CreateEntityDataImportProc(system_Entity);

                        //添加实体验证存储过程逻辑
                        CreateEntityValiditeProc(system_Entity.Id);
                        //业务规则生成
                        AttributeRuleRelease(system_Entity.ModelId.Value, system_Entity.Id);
                        //创建触发器
                        //system_create_table_trigger(system_Entity);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 创建实体选择自动创建代码时为表创建触发器
        /// </summary>
        /// <param name="system_Entity"></param>
        public void system_create_table_trigger(system_entity system_Entity)
        {
            using (var tran = this._dbContext.Database.BeginTransaction())
            {
                try
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append($" DROP TRIGGER IF EXISTS trig_{system_Entity.Name}");
                    this._dbContext.Database.ExecuteSqlRaw(builder.ToString());
                    builder.Clear();
                    builder.Append($" CREATE TRIGGER trig_{system_Entity.Name} BEFORE INSERT");
                    builder.Append($" ON {system_Entity.StageTable} FOR EACH ROW");
                    builder.Append($" BEGIN ");
                    builder.Append("  DECLARE PrefixCode varchar(50);");//定义含前缀Code
                    builder.Append("  DECLARE MdmAllCount int;");//定义Mdm表总数
                    builder.Append("  DECLARE StgAllCount int;");//定义stage表总数
                    builder.Append("  DECLARE MdmOrStg int;");//决定取mdm还是stage的最大code
                    builder.Append("  DECLARE MiddleCode varchar(50);");//中间变量
                    builder.Append("  DECLARE Prefix varchar(50);");//定义code前缀
                    builder.Append("  DECLARE Postfix varchar(50);");//定义Code前缀后的编码
                    builder.Append($" SELECT count(*) into MdmAllCount  from {system_Entity.EntityTable}; ");//先查mdm表是否有数据
                    builder.Append($" SELECT count(*) into StgAllCount  from {system_Entity.StageTable}; ");//先查stage表是否有数据
                    builder.Append("  if MdmAllCount <> 0 THEN ");//如果mdm表中有数据
                    builder.Append($"    SELECT max(Code+0) into PrefixCode from {system_Entity.EntityTable};");//取mdm最大code值
                    builder.Append($"    SELECT Count(*) into MdmOrStg from {system_Entity.StageTable} where Code=PrefixCode and ValidateStatus != 2 and BissnessRuleStatus !=2;");//决定取mdm还是取stage表
                    builder.Append($"    if MdmOrStg > 0 THEN");//如果mdm表中的最大值在stage表中存在,则取stage表中的最大code作为自增初始值
                    builder.Append($"        SELECT max(Code+0) into PrefixCode from {system_Entity.StageTable} where ValidateStatus != 2 and BissnessRuleStatus !=2;");
                    builder.Append("         if ISNULL(NEW.Code) THEN ");//如果新增的时候code没有值
                    builder.Append("            SET NEW.code = (PrefixCode+0+1);");//给当前Code加一
                    builder.Append("         ELSE ");//插入时有值
                    builder.Append("            SET NEW.code = NEW.Code;");//用当前值赋值给Code
                    builder.Append("         end if;");
                    builder.Append("     ELSE ");//如果mdm表中的最大值在stage表中不存在,则取mdm表中的最大code作为自增初始值
                    builder.Append($"        SELECT IFNULL(max(Code+0),0)  into MiddleCode from {system_Entity.StageTable} where ValidateStatus != 2 and BissnessRuleStatus !=2;");
                    builder.Append($"        if PrefixCode > (MiddleCode+0) THEN ");
                    builder.Append("            if ISNULL(NEW.Code) THEN ");//如果新增的时候code没有值
                    builder.Append("               SET NEW.code = (PrefixCode+0+1);");//给当前Code加一
                    builder.Append("            ELSE ");//插入时有值
                    builder.Append("               SET NEW.code = NEW.Code;");//用当前值赋值给Code
                    builder.Append("            end if;");
                    builder.Append("         ELSE ");
                    builder.Append("             if ISNULL(NEW.Code) THEN ");//如果新增的时候code没有值
                    builder.Append("                SET NEW.code = (MiddleCode+0+1);");//给当前Code加一
                    builder.Append("             ELSE ");//插入时有值
                    builder.Append("                SET NEW.code = NEW.Code;");//用当前值赋值给Code
                    builder.Append("             end if;");
                    builder.Append("        end if;");
                    builder.Append("     end if;");
                    builder.Append("  ELSE ");//如果mdm表中没有数据
                    builder.Append("    if StgAllCount <> 0 THEN ");//如果stage表中有值
                                                                    //取stage表中通过验证的最大code(包括ValidateStatus和BissnessRuleStatus为0的)
                    builder.Append($"      SELECT max(Code+0) into PrefixCode from {system_Entity.StageTable} where ValidateStatus != 2 and BissnessRuleStatus !=2; ");
                    builder.Append("       if ISNULL(NEW.Code) THEN ");//如果新增的时候code没有值
                    builder.Append("          SET NEW.Code = (PrefixCode+0+1);");//给当前Code加一
                    builder.Append("       ELSE ");//插入时有值
                    builder.Append("          SET NEW.Code = NEW.Code;");//用当前值赋值给Code
                    builder.Append("       end if;");
                    builder.Append("   ELSE ");//如果stage表中没有数据
                    builder.Append("       SET PrefixCode=0;");
                    builder.Append("       if ISNULL(NEW.Code) THEN ");//如果新增时code为空
                    builder.Append("          SET NEW.Code=(PrefixCode+0+1);");//code从1开始
                    builder.Append("       ELSE ");//新增时Code不为空
                    builder.Append("          SET NEW.Code = NEW.Code;");
                    builder.Append("       end if;");
                    builder.Append("   end if;");
                    builder.Append("  end if;");
                    builder.Append("  END	");
                    this._dbContext.Database.ExecuteSqlRaw(builder.ToString());
                    builder.Clear();
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                }
            }

        }
        /// <summary>
        /// 发布验证业务规则存储过程  2020年4月26   wyt
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="entityID"></param>
        /// <returns></returns>
        public object AttributeRuleRelease(int modelID, int entityID)
        {
            using (var tran = this._dbContext.Database.BeginTransaction())
            {
                Result result = new Result();
                int res = 0;
                try
                {
                    var entityData = _dbContext.system_entity.AsNoTracking().Where(e => e.Id == entityID).FirstOrDefault(); //获取实体信息
                    if (entityData != null)
                    {
                        var ruleData = _dbContext.system_businessrule_attribute.Where(e => e.ModelID == modelID && e.EntityID == entityID && e.Validity == "1").ToList();
                        if (ruleData == null || ruleData.Count == 0)
                        {
                            result.success = false;
                            result.message = "未存在业务规则";
                        }
                        var dropSql = $"DROP PROCEDURE IF EXISTS { entityData.BusinessProc}";
                        _dbContext.Database.ExecuteSqlRaw(dropSql);  //判断该存储过程是否存在,若存在则删除
                        StringBuilder createProcSql = new StringBuilder();
                        createProcSql.Append(" CREATE PROCEDURE " + entityData.BusinessProc + "(IN batchid VARCHAR(100))");//创建实体(有参版本号)
                        createProcSql.Append(" BEGIN ");
                        //createProcSql.Append(" SELECT * FROM " + entityData.StageTable);
                        string columnRule = ""; //循环添加Update 验证条件
                        foreach (var item in ruleData)
                        {
                            string tempSql = "";
                            var attributeConfig = _dbContext.system_attribute.Where(e => e.Id == item.AttributeID).Select(it => new { it.Name, it.DisplayName }).FirstOrDefault();
                            if (attributeConfig != null)
                            {
                                if (attributeConfig.Name.ToLower() == "code")
                                {
                                    if (item.Type == "自定义验证规则" && string.IsNullOrEmpty(item.Expression))
                                    {
                                        tempSql += $" select batch_id into batchid  from {entityData.StageTable} order by CreateTime desc  LIMIT 1;";
                                        tempSql += $" call SP_MDM_Update_StoreCode(batchid); ";
                                    }
                                }
                                else
                                {
                                    tempSql += $@" UPDATE {entityData.StageTable} SET BissnessRuleErrorDesc = CONCAT( IFNULL(BissnessRuleErrorDesc,''), '{attributeConfig.DisplayName ?? attributeConfig.Name}错误  ' ) ,
	                           BissnessRuleStatus = '2' 
                               WHERE batch_id = batchid AND {attributeConfig.Name} NOT REGEXP '{item.Expression}' AND 
                               IFNULL(BissnessRuleErrorDesc,'') NOT LIKE '%{attributeConfig.DisplayName ?? attributeConfig.Name}错误%';  "; //正则验证 update语句
                                    if (item.Required == "true")
                                    {
                                        tempSql += $@" UPDATE {entityData.StageTable} SET BissnessRuleErrorDesc = CONCAT(IFNULL(BissnessRuleErrorDesc,'') , '{attributeConfig.DisplayName ?? attributeConfig.Name}必填项为空  ' ) ,
	                           BissnessRuleStatus = '2' 
                               WHERE batch_id = batchid AND ( {attributeConfig.Name} IS NULL OR {attributeConfig.Name} = '' ) AND 
                               BissnessRuleErrorDesc NOT LIKE '%{attributeConfig.DisplayName ?? attributeConfig.Name}必填项为空%';  ";    //判断是否为必填
                                    }
                                }
                            }
                            columnRule += tempSql;
                        }
                        columnRule += $@" UPDATE {entityData.StageTable} SET 
                                     BissnessRuleStatus = '1'
                                     WHERE batch_id = batchid  AND BissnessRuleStatus ='0' ; ";
                        createProcSql.Append(columnRule + " ");
                        createProcSql.Append(" END");
                        string createProcSqlStr = createProcSql.ToString();
                        createProcSqlStr = createProcSqlStr.Replace("{", "{{").Replace("}", "}}");
                        _dbContext.Database.ExecuteSqlRaw(createProcSqlStr.ToString());
                    }
                    else
                    {
                        result.success = false;
                        result.message = "未存在业务规则";
                    }

                    res = _dbContext.system_businessrule_attribute.Where(e => e.ModelID == modelID && e.EntityID == entityID && e.Validity == "1").Update(e => new system_businessrule_attribute { State = "1" });
                    if (res > 0)
                    {
                        result.success = true;
                        result.message = "发布成功";
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw;
                }
                return result;
            }


        }
        #endregion
        /// <summary>
        /// 添加实体  2020年 4月 20日 hhyang
        /// </summary>
        /// <param name="model">添加所需要的参数</param>
        /// <returns></returns>
        public object InserEntity(EntityManageItem model)
        {
            Result result = new Result();
            try
            {
                if (this._dbContext.system_entity.AsNoTracking().Any(it => it.Name == model.Name))
                {
                    result.success = false;
                    result.message = "已存在相同实体名";
                    return result;
                }
                system_entity date = new system_entity();
                date.Name = model.Name;
                date.Remark = model.Remark;
                date.EntityTable = "mdm_" + model.Name.ToLower();
                date.StageTable = model.StageTable.ToLower();
                date.HistoryTable = "history_" + model.Name.ToLower();
                date.ModelId = model.ModelId;
                date.BeganIn = model.BeganIn;
                date.AutoCreateCode = model.AutoCreateCode;
                date.ValiditeProc = "mdm_validate_" + model.Name;
                date.BusinessProc = "business_" + model.Name;
                date.DataImportProc = "dataimport_" + model.Name;
                date.Creater = CurrentUser.UserAccount;
                date.CreateTime = DateTime.Now;
                date.Updater = CurrentUser.UserAccount;
                date.UpdateTime = DateTime.Now;
                var insertEntity = _dbContext.system_entity.Add(date);
                var rows = this._dbContext.SaveChanges();
                if (rows != 0)
                {
                    system_create_table_entity(insertEntity.Entity);
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
                var _del_entity = _dbContext.system_entity.Where(e => e.Name == model.Name).FirstOrDefault();
                _dbContext.system_entity.Remove(_del_entity);
                _dbContext.SaveChanges();

                throw;
            }
            return result;
        }

        /// <summary>
        /// 查询实体  2020年 4月 20日 hhyang
        /// </summary>
        /// <param name="model">所需要的参数</param>
        /// <returns></returns>
        public object SearchEntity(EntityManageItem model)
        {
            tableResult result = new tableResult();
            try
            {

                List<system_entity> EntityList = _dbContext.system_entity.ToList();
                if (!string.IsNullOrWhiteSpace(model.Where))
                {
                    EntityList = EntityList.Where(x => x.Remark.Contains(model.Where) || x.Name.Contains(model.Where)).ToList();
                }
                if (model.ModelId != 0)
                {
                    EntityList = EntityList.Where(x => x.ModelId == model.ModelId).ToList();
                }
                int total = EntityList.Count(); //算总数
                if (total > 0)
                {
                    var resultList = EntityList.OrderByDescending(x => x.CreateTime).Skip((model.Page - 1) * model.limit).Take(model.limit).ToList().Select(
                                              a => new
                                              {
                                                  a.Id,
                                                  a.ModelId,
                                                  a.Name,
                                                  a.Remark,
                                                  a.StageTable,
                                                  a.BeganIn,
                                                  AutoCreateCode = a.AutoCreateCode == 0 ? "是" : "否",
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
                else
                {
                    result.total = 0;
                    result.data = "";
                    result.message = "暂无数据！";
                    result.success = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 编辑实体  2020年 4月 20日 hhyang
        /// </summary>
        /// <param name="model">编辑所需要的参数</param>
        /// <returns></returns>
        public object UpdateEntity(EntityManageItem model)
        {
            Result result = new Result();
            try
            {
                if (model.Id != null && model.Id != 0)
                {
                    system_entity FindEntity = _dbContext.system_entity.FirstOrDefault(x => x.Id == model.Id);
                    if (model.AutoCreateCode == 1 && FindEntity.AutoCreateCode == 0)
                    {
                        this._dbContext.Database.ExecuteSqlRaw($" DROP TRIGGER IF EXISTS trig_{FindEntity.Name}");//删除对应表的触发器
                    }
                    if (model.AutoCreateCode == 0 && FindEntity.AutoCreateCode == 1)
                    {
                        system_create_table_trigger(FindEntity);
                    }
                    FindEntity.Name = model.Name;
                    FindEntity.Remark = model.Remark;
                    FindEntity.BeganIn = model.BeganIn;
                    FindEntity.AutoCreateCode = model.AutoCreateCode;
                    FindEntity.StageTable = model.StageTable.ToLower();
                    FindEntity.UpdateTime = DateTime.Now;
                    FindEntity.Updater = CurrentUser.UserAccount;
                    //FindEntity.ModelId = model.ModelId;
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
        #endregion

        #region 用户登录验证账号密码  wyt
        /// <summary>
        /// 用户登录验证账号密码   2020年4月22日  wyt
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public Result Login(string Account, string pwd)
        {
            Result result = new Result();
            try
            {
                system_user user = _dbContext.system_user.Where(e => e.UserAccount == Account && e.Validity == "1").FirstOrDefault();

                if (user != null)
                {
                    byte[] pwdAndSalt = Encoding.UTF8.GetBytes(pwd + user.Salt);
                    byte[] hasBytes = new SHA256Managed().ComputeHash(pwdAndSalt);
                    string pwdBytes = Convert.ToBase64String(hasBytes);
                    result.success = pwdBytes == user.UserPwd ? true : false;
                }
                else
                {
                    result.success = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        #endregion
        /// <summary>
        ///  动态拼接sql插入语句保存数据到数据库(支持多条插入)  2020年4月27日21:32:13   Dennyhui
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public object DataSaveEntityMember(string body)
        {
            Result result = new Result();
            bool NotValidite = false;
            bool isWorkFlow = false;
            try
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(body);
                string entity = jo["EntityName"].ToString();
                string batch_id = Guid.NewGuid().ToString();
                var entitydata = _dbContext.system_entity.Where(e => e.Name == entity).FirstOrDefault();
                foreach (var ditem in jo["Data"])
                {
                    string datastr = ditem.ToString();
                    JObject o = JObject.Parse(datastr);
                    IEnumerable<JProperty> properties = o.Properties();
                    StringBuilder sqlstr = new StringBuilder();
                    StringBuilder sqlvalue = new StringBuilder(" values (");
                    sqlstr.Append("insert into");
                    sqlstr.Append(" " + entitydata.StageTable);
                    sqlstr.Append("(");
                    foreach (JProperty item in properties)
                    {
                        if (item.Name.ToLower().Equals("id"))
                        {
                            if (!string.IsNullOrEmpty(item.Value?.ToString()))
                            {
                                NotValidite = true;
                            }
                            continue;
                        }
                        if (item.Name.Equals("batch_id"))
                        {
                            item.Value = batch_id;
                            //batch_id = item.Value.ToString();
                        }
                        sqlstr.Append(item.Name + ",");
                        sqlvalue.Append("\"" + item.Value + "\"" + ",");
                    }
                    string execSqlStr = DelLastComma(sqlstr.ToString()) + ")" + DelLastComma(sqlvalue.ToString()) + ")";

                    this._dbContext.Database.ExecuteSqlRaw(execSqlStr);
                }
                var para = new DynamicParameters();
                para.Add("@batchid", batch_id);
                using (IDbConnection connection = DapperContext.Connection())
                {
                    if (!NotValidite)
                    {
                        connection.Execute(entitydata.ValiditeProc, para, null, 18000, CommandType.StoredProcedure);
                        //var validityresult = connection.ex(entitydata.ValiditeProc, para, null, true, null, CommandType.StoredProcedure).FirstOrDefault();
                        string attributeCheckSql = $"select * from  {entitydata.StageTable}  where ValidateStatus=2 and batch_id='{batch_id}'";
                        var attributeCheckData = connection.Query(attributeCheckSql);
                        if (attributeCheckData.Count() > 0)
                        {
                            result.success = false;
                            result.message = "操作失败";
                            system_entitydatchlogs enlogfailed = new system_entitydatchlogs();
                            enlogfailed.EntityID = entitydata.Id;
                            enlogfailed.BatchID = batch_id;
                            enlogfailed.CreateTime = DateTime.Now;
                            enlogfailed.CreateUser = CurrentUser.UserAccount;
                            enlogfailed.UpdateTime = DateTime.Now;
                            enlogfailed.UpdateUser = CurrentUser.UserAccount;
                            enlogfailed.State = "失败";
                            enlogfailed.ErrorMessages = "操作失败,字段类型有误";
                            enlogfailed.Code = Guid.NewGuid().ToString();
                            _dbContext.system_entitydatchlogs.Add(enlogfailed);
                            _dbContext.SaveChanges();
                            return result;
                        }
                        connection.Execute(entitydata.BusinessProc, para, null, 18000, CommandType.StoredProcedure);
                        //var businessresult = connection.Query(entitydata.BusinessProc, para, null, true, null, CommandType.StoredProcedure).FirstOrDefault();
                        string businessCheckSql = $"select * from  {entitydata.StageTable}  where BissnessRuleStatus=2 and batch_id='{batch_id}'";
                        var businessCheckData = connection.Query(businessCheckSql);
                        if (businessCheckData.Count() > 0)
                        {
                            result.success = false;
                            result.message = "操作失败";
                            system_entitydatchlogs enlogfailed2 = new system_entitydatchlogs();
                            enlogfailed2.EntityID = entitydata.Id;
                            enlogfailed2.BatchID = batch_id;
                            enlogfailed2.CreateTime = DateTime.Now;
                            enlogfailed2.CreateUser = CurrentUser.UserAccount;
                            enlogfailed2.UpdateTime = DateTime.Now;
                            enlogfailed2.UpdateUser = CurrentUser.UserAccount;
                            enlogfailed2.State = "失败";
                            enlogfailed2.ErrorMessages = "操作失败,数据有误";
                            enlogfailed2.Code = Guid.NewGuid().ToString();
                            _dbContext.system_entitydatchlogs.Add(enlogfailed2);
                            _dbContext.SaveChanges();
                            return result;
                        }
                    }
                    //版本管理-拉链表
                    var v_para = new DynamicParameters();
                    v_para.Add("@entityid", entitydata.Id);
                    v_para.Add("@batchid", batch_id);
                    connection.Execute("Field_Track", v_para, null, 18000, CommandType.StoredProcedure);
                    //var visionZipper = connection.Query("Field_Track", v_para, null, true, null, CommandType.StoredProcedure).FirstOrDefault();
                    //数据审批流
                    //工作流处理 
                    connection.Execute("WorkFlow_Check", v_para, null, 18000, CommandType.StoredProcedure);
                    //var workflow_check_PROC = connection.Query("WorkFlow_Check", v_para, null, true, null, CommandType.StoredProcedure).FirstOrDefault();
                    string workflowCheckSql = $"SELECT * FROM {entitydata.StageTable}  WHERE WorkFlowstatus=1 AND batch_id='{batch_id}' GROUP BY WorkFlow_ProcessingType";
                    var workflowCheckData = connection.Query(workflowCheckSql);
                    if (workflowCheckData.Count() > 0)
                    {
                        //构造请求身份验证token的参数
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        param.Add("name", AppsettingsHelper.GetSection("WorkFlowInfos:WFUser"));
                        param.Add("pwd", AppsettingsHelper.GetSection("WorkFlowInfos:WFPwd"));
                        //获取身份验证的token
                        string apiurl = AppsettingsHelper.GetSection("WorkFlowInfos:WorkFlowApiUrl");
                        var authstr = Client_Get(param, apiurl + "/OAuth/token", "").Result;
                        JObject authjobject = (JObject)JsonConvert.DeserializeObject(authstr);
                        string tokenstr = ((Newtonsoft.Json.Linq.JValue)authjobject["authorization"]).Value.ToString();
                        foreach (var workflowitem in workflowCheckData)
                        {
                            //构造调用发起审批流程的api参数
                            var formData = new MultipartFormDataContent();
                            //formData.Add(new StringContent("测试审批流"), "bpmName");
                            //formData.Add(new StringContent("Add_WorkFlow"), "applyTitle");
                            formData.Add(new StringContent(workflowitem.CreateUser ?? workflowitem.UpdateUser), "userName");
                            formData.Add(new StringContent(entitydata.Id.ToString()), "tableFlag");
                            //发起审批流程
                            JObject resultpost = (JObject)JsonConvert.DeserializeObject(Client_Post(formData, workflowitem.WorkFlowApi, tokenstr).Result);
                            string instanceID = string.Empty;
                            bool workflow = bool.Parse(((Newtonsoft.Json.Linq.JValue)resultpost["isok"]).Value.ToString());
                            instanceID = ((Newtonsoft.Json.Linq.JValue)resultpost["entityID"]).Value.ToString();
                            if (workflow)
                            {
                                isWorkFlow = true;
                                connection.Execute($"update {entitydata.StageTable} set WorkFlow_InstanceID='{instanceID}' where batch_id='{batch_id}' and WorkFlow_ProcessingType='{workflowitem.WorkFlow_ProcessingType}'");
                            };
                            if (!workflow && instanceID == "-1")
                            {
                                isWorkFlow = false;
                                connection.Execute($"update {entitydata.StageTable} set WorkFlowstatus=0 where batch_id='{batch_id}' and WorkFlow_ProcessingType='{workflowitem.WorkFlow_ProcessingType}'");
                            }
                        }
                    }
                    //进行数据的最终导入
                    //var importresult = connection.Query(entitydata.DataImportProc, para, null, true, null, CommandType.StoredProcedure).FirstOrDefault();
                    connection.Execute(entitydata.DataImportProc, para, null, 18000, CommandType.StoredProcedure);
                }
                system_entitydatchlogs enlog = new system_entitydatchlogs();
                enlog.EntityID = entitydata.Id;
                enlog.BatchID = batch_id;
                enlog.CreateTime = DateTime.Now;
                enlog.CreateUser = CurrentUser.UserAccount;
                if (!isWorkFlow)
                {
                    result.success = true;
                    result.message = "操作成功";
                    enlog.State = "成功";
                    enlog.ErrorMessages = "";
                }
                else
                {
                    result.success = true;
                    result.message = "添加成功, 部分数据需要进行流程审批";
                    enlog.State = "添加成功, 部分数据需要进行流程审批";
                    enlog.ErrorMessages = "";
                }
                enlog.UpdateUser = CurrentUser.UserAccount;
                enlog.UpdateTime = DateTime.Now;
                enlog.Code = Guid.NewGuid().ToString();
                _dbContext.system_entitydatchlogs.Add(enlog);
                _dbContext.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// 去掉字符串最后一个逗号  2020年4月27日15:37:47  Dennyhui
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string DelLastComma(string str)
        {
            try
            {
                return str.Substring(0, str.LastIndexOf(","));
            }
            catch (Exception ex)
            {
                return str;
            }
        }


        /// <summary>
        /// 导出数据合并结果的Excel文件 2020年5月9日15:05:01 Dennyhui
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="mergingCode"></param>
        /// <returns></returns>
        public byte[] ExportMergeDataResult(string entityID, string mergingCode)
        {
            try
            {
                var entity = _dbContext.system_entity.Where(e => e.Id.ToString() == entityID).Select(e => e.EntityTable).FirstOrDefault();
                var columns = (from r in _dbContext.system_rulesdetails
                               join a in _dbContext.system_attribute on r.AttributeID equals a.Id
                               where r.MerginfCode == mergingCode && r.Validity == "1"
                               select new
                               {
                                   a.Name,
                                   r.Weight
                               }
                             ).ToList();
                StringBuilder sqlColumns = new StringBuilder();
                foreach (var column in columns)
                {
                    sqlColumns.Append(column.Name + ",");
                }
                string columnstr = DelLastComma(sqlColumns.ToString());
                StringBuilder sqlstr = new StringBuilder();
                sqlstr.Append("select  ms.SimilarFlag,ms.Code, " + columnstr + ",ms.SimilarNum from system_mergingrules_similarresult as ms left join mdm_distributor as m on ms.Code=m.Code");
                DataTable table = new DataTable();
                using (IDbConnection connection = DapperContext.Connection())
                {
                    var reader = connection.ExecuteReader(sqlstr.ToString());
                    table.Load(reader);
                }
                IWorkbook workbook = new NPOI.XSSF.UserModel.XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("sheet");
                IRow Title = null;
                IRow rows = null;
                for (int i = 1; i <= table.Rows.Count; i++)
                {
                    //创建表头
                    if (i - 1 == 0)
                    {
                        Title = sheet.CreateRow(0);
                        foreach (DataColumn column in table.Columns)
                        {
                            Title.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                        }
                        continue;
                    }
                    else
                    {
                        rows = sheet.CreateRow(i - 1);
                        for (int j = 1; j <= table.Columns.Count; j++)
                        {
                            rows.CreateCell(0).SetCellValue(i - 1);
                            rows.CreateCell(j).SetCellValue(table.Rows[i - 1][j - 1].ToString());
                        }
                    }
                }
                byte[] buffer;
                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.Write(ms);
                    buffer = ms.ToArray();
                    ms.Close();
                }
                return buffer;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        /// <summary>
        /// 导出excel模板  2020年4月28日18:18:18  dennyhui
        /// </summary>
        /// <param name="entityid">实体ID</param>
        /// <returns></returns>
        public byte[] OutputExcel(string entityid, string type, string Where)
        {
            try
            {
                IWorkbook workbook = new NPOI.XSSF.UserModel.XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("sheet");
                IRow Title = sheet.CreateRow(0);
                var attributeColumn = _dbContext.system_attribute.AsNoTracking().Where(e => e.EntityID.ToString() == entityid && e.IsSystem == 0).Select(e => new { e.DisplayName, e.Name }).ToList();
                for (int i = 0; i < attributeColumn.Count; i++)
                {
                    Title.CreateCell(i).SetCellValue(attributeColumn[i].DisplayName.ToString() == "" ? attributeColumn[i].Name : attributeColumn[i].DisplayName);
                }
                if (type == "Get")
                {
                    Dictionary<string, List<dynamic>> ForeignMdmDatas = new Dictionary<string, List<dynamic>>();

                    if (this._dbContext.system_attribute.AsNoTracking().Any(e => e.EntityID.ToString() == entityid && e.IsSystem == 0 && e.Type == "基于域"))
                    {
                        var Entity = this._dbContext.system_entity.AsNoTracking().Where(it => it.Id.ToString() == entityid).Select(it => new { it.EntityTable, it.Name }).FirstOrDefault();
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@EntityID", entityid, DbType.Int32);
                        DataTable mdmTable = new DataTable();
                        DataTable ForeignAttrsDT = new DataTable();
                        using (IDbConnection con = DapperContext.Connection())
                        {
                            IDataReader ForeignAttrsReader = con.ExecuteReader("select name from system_attribute where EntityID=@EntityID and type='基于域'", parameters);
                            //获取基于域的属性
                            ForeignAttrsDT.Load(ForeignAttrsReader);
                            IDataReader ForeignTablesReader = con.ExecuteReader("select EntityTable  from system_entity where id in (select LinkEntityID from system_attribute where EntityID=@EntityID and Type='基于域')", parameters);
                            mdmTable.Load(ForeignTablesReader);
                            List<dynamic> ForeignData = null;
                            List<dynamic> ForeignData2 = con.Query(@$"select DISTINCT `Code`, `Name` from {Entity.EntityTable}").AsQueryable().AsNoTracking().ToList();
                            DataRow rowValue = null;
                            int i = 0;
                            foreach (DataRow item in mdmTable.Rows)
                            {
                                rowValue = ForeignAttrsDT.Rows[i];
                                if (item["EntityTable"].ToString() != Entity.EntityTable)
                                {
                                    ForeignData = con.Query(@$"select DISTINCT  b.`Code`, b.`Name` from {Entity.EntityTable}
                                                        left join {item["EntityTable"].ToString()} b
                                                        on {rowValue["name"].ToString()} = b.code").AsQueryable().AsNoTracking().ToList();
                                }
                                else
                                {
                                    ForeignData = ForeignData2;
                                }
                                ForeignMdmDatas.Add(rowValue["name"].ToString(), ForeignData);
                                i++;
                            }
                        }
                    }
                    var EntityTable = this._dbContext.system_entity.AsNoTracking().Where(it => it.Id.ToString() == entityid).Select(it => it.EntityTable).FirstOrDefault();
                    StringBuilder builder = new StringBuilder();
                    foreach (var item in attributeColumn)
                    {
                        builder.Append(item.Name + ",");
                    }
                    //builder.Append(" Validity ");
                    string SelectColumn = DelLastComma(builder.ToString());
                    builder.Clear();
                    DataTable mdmDT = new DataTable();
                    IDataReader reader = null;
                    using (IDbConnection con = DapperContext.Connection())
                    {
                        string querySql = $"select {SelectColumn} from {EntityTable} where  ";
                        if (!string.IsNullOrEmpty(Where))
                        {
                            querySql = querySql + Where;
                            reader = con.ExecuteReader(querySql);
                        }
                        else
                        {
                            querySql = querySql + " 1=1 ;";
                            reader = con.ExecuteReader(querySql);
                        }
                        mdmDT.Load(reader);
                    }
                    if (mdmDT.Rows.Count > 0)
                    {
                        bool addTitleCell = true;
                        DataRow rowValue = null;
                        for (int j = 0; j < mdmDT.Rows.Count; j++)
                        {
                            rowValue = mdmDT.Rows[j];
                            IRow row = sheet.CreateRow(j + 1);
                            for (int a = 0; a < attributeColumn.Count; a++)
                            {
                                if (rowValue.Table.Columns.Contains(attributeColumn[a].Name))
                                {
                                    var v = rowValue[attributeColumn[a].Name]?.ToString() ?? "";
                                    if (addTitleCell)
                                    {
                                        if (ForeignMdmDatas.ContainsKey(attributeColumn[a].Name))
                                        {
                                            var index = Title.LastCellNum;
                                            Title.CreateCell(index).SetCellValue(attributeColumn[a].DisplayName + "名称");
                                            if (!string.IsNullOrEmpty(v))
                                            {
                                                var Name = ForeignMdmDatas[attributeColumn[a].Name].Find(it => it.Code == v)?.Name ?? "";
                                                row.CreateCell(a).SetCellValue(v);
                                                row.CreateCell(index).SetCellValue(Name);
                                            }
                                            else
                                            {
                                                row.CreateCell(a).SetCellValue("");
                                                row.CreateCell(index).SetCellValue("");
                                            }
                                            attributeColumn.Add(new { DisplayName = attributeColumn[a].Name + "名称", Name = "" });
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(v))
                                            {
                                                row.CreateCell(a).SetCellValue(rowValue[attributeColumn[a].Name]?.ToString() ?? "");
                                            }
                                            else
                                            {
                                                row.CreateCell(a).SetCellValue("");
                                            }

                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(v))
                                        {
                                            row.CreateCell(a).SetCellValue(rowValue[attributeColumn[a].Name]?.ToString() ?? "");
                                        }
                                        else
                                        {
                                            row.CreateCell(a).SetCellValue("");
                                        }

                                    }
                                }
                                else
                                {
                                    if (j != 0)
                                    {
                                        var colume = attributeColumn[a].DisplayName.Substring(0, attributeColumn[a].DisplayName.Length - 2);
                                        var v = rowValue[colume].ToString();
                                        var Name = ForeignMdmDatas[colume].Find(it => it.Code == v)?.Name ?? "";
                                        row.CreateCell(a).SetCellValue(Name);
                                    }

                                }
                            }
                            addTitleCell = false;
                        }
                    }
                }
                byte[] buffer = new byte[1024 * 5];
                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.Write(ms);
                    //ms.Flush();
                    //ms.Position=0;
                    buffer = ms.ToArray();
                    ms.Close();
                }
                return buffer;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// 上传Excel 2020年4月30日11:55:40 Dennyhui
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public object UploadExcel(IFormFile file, string entityName)
        {
            UploadResult result = new UploadResult();
            string batch_id = Guid.NewGuid().ToString();
            system_entitydatchlogs enlogfailed = new system_entitydatchlogs();
            enlogfailed.Code = Guid.NewGuid().ToString();
            enlogfailed.BatchID = batch_id;
            enlogfailed.CreateTime = DateTime.Now;
            enlogfailed.CreateUser = CurrentUser.UserAccount;
            enlogfailed.UpdateUser = CurrentUser.UserAccount;
            enlogfailed.UpdateTime = DateTime.Now;
            bool isWorkFlow = false;
            try
            {
                var entitydata = _dbContext.system_entity.Where(e => e.Name.Equals(entityName)).FirstOrDefault();
                MemoryStream target = new MemoryStream();
                file.OpenReadStream().CopyTo(target);
                byte[] data = target.ToArray();
                Stream stream = new MemoryStream(data);//excel文件转成数据流
                var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);//流数据转成Excelreader
                //Excelreader转成datatable
                var dataSet = excelReader.AsDataSet(new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true // Use first row is ColumnName here :D
                    }
                });
                if (dataSet.Tables.Count > 0)
                {
                    var dtData = dataSet.Tables[0];
                    dtData.Columns.Add("batch_id");
                    dtData.Columns.Add("BissnessRuleStatus");
                    dtData.Columns.Add("WorkFlowStatus");
                    dtData.Columns.Add("ValidateStatus");
                    dtData.Columns.Add("CreateUser");
                    dtData.Columns.Add("CreateTime");
                    dtData.Columns.Add("UpdateUser");
                    dtData.Columns.Add("UpdateTime");
                    //dtData.Columns.Add("Validity");
                    foreach (DataRow dr in dtData.Rows)
                    {
                        //为新添加的列进行赋值
                        dr["batch_id"] = batch_id;
                        dr["BissnessRuleStatus"] = 0;
                        dr["WorkFlowStatus"] = 0;
                        dr["ValidateStatus"] = 0;
                        dr["CreateUser"] = CurrentUser.UserAccount;
                        dr["CreateTime"] = DateTime.Now.ToStringYMD24HMS();
                        dr["UpdateUser"] = CurrentUser.UserAccount;
                        dr["UpdateTime"] = DateTime.Now.ToStringYMD24HMS();
                        //dr["Validity"] = dr["有效性"]?.ToString() ?? "1";
                    }
                    //获取表头
                    var entityId = this._dbContext.system_entity.AsNoTracking().Where(it => it.Name.ToLower() == entityName.ToLower()).Select(it => it.Id).FirstOrDefault();
                    var attrs = this._dbContext.system_attribute.AsNoTracking().Where(it => it.EntityID == entityId && it.IsSystem == 0).Select(it => new { it.Name, it.DisplayName }).ToList();
                    var Dtcolumns = dtData.Columns;
                    foreach (DataColumn item in Dtcolumns)
                    {
                        item.ColumnName = attrs.Find(it => it.DisplayName == item.ColumnName)?.Name ?? item.ColumnName;
                    }
                    var columns = dtData.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList();
                    //datatable转成CSV数据流
                    //Stream csvStream = DTToCsvStream(dtData);
                    // Do Something
                    //CSV数据流导入数据库
                    int count = MysqlBulkLoad(DapperContext.Connection(), columns, entitydata.StageTable, dtData);
                    if (count > 0)
                    {
                        var para = new DynamicParameters();
                        para.Add("@batchid", batch_id);
                        using (IDbConnection connection = DapperContext.Connection())
                        {
                            connection.Execute(entitydata.ValiditeProc, para, null, 18000, CommandType.StoredProcedure);
                            string attributeCheckSql = $"select * from {entitydata.StageTable}  where ValidateStatus=2 and batch_id='{batch_id}'";
                            var attributeCheckData = connection.Query(attributeCheckSql);
                            if (attributeCheckData.Count() > 0)
                            {
                                result.success = false;
                                result.batchid = batch_id;
                                result.message = "添加失败,数据有误,attributeCheck";
                                enlogfailed.State = "失败";
                                enlogfailed.ErrorMessages = "导入失败,字段类型有误";
                                _dbContext.system_entitydatchlogs.Add(enlogfailed);
                                _dbContext.SaveChanges();
                                return result;
                            }
                            connection.Execute(entitydata.BusinessProc, para, null, 18000, CommandType.StoredProcedure);
                            string businessCheckSql = $"select * from {entitydata.StageTable}  where BissnessRuleStatus=2 and batch_id='{batch_id}'";
                            var businessCheckData = connection.Query(attributeCheckSql);
                            if (businessCheckData.Count() > 0)
                            {
                                result.success = false;
                                result.batchid = batch_id;
                                result.message = "添加失败,数据有误,businessCheck";
                                enlogfailed.State = "失败";
                                enlogfailed.ErrorMessages = "导入失败,数据有误";
                                _dbContext.system_entitydatchlogs.Add(enlogfailed);
                                _dbContext.SaveChanges();
                                return result;
                            }
                            //版本管理-拉链表
                            var v_para = new DynamicParameters();
                            v_para.Add("@entityid", entityId);
                            v_para.Add("@batchid", batch_id);
                            connection.Execute("Field_Track", v_para, null, 18000, CommandType.StoredProcedure);
                            #region 数据审批流
                            //工作流处理 
                            connection.Execute("WorkFlow_Check", v_para, null, 18000, CommandType.StoredProcedure);
                            string workflowCheckSql = $"SELECT * FROM {entitydata.StageTable}  WHERE WorkFlowstatus=1 AND batch_id='{batch_id}' GROUP BY WorkFlow_ProcessingType";
                            var workflowCheckData = connection.Query(workflowCheckSql);
                            if (workflowCheckData.Count() > 0)
                            {
                                isWorkFlow = true;
                                //构造请求身份验证token的参数
                                Dictionary<string, string> param = new Dictionary<string, string>();
                                param.Add("name", AppsettingsHelper.GetSection("WorkFlowInfos:WFUser"));
                                param.Add("pwd", AppsettingsHelper.GetSection("WorkFlowInfos:WFPwd"));
                                string WorkFlowAPI = AppsettingsHelper.GetSection("WorkFlowInfos:WorkFlowApiUrl");
                                //获取身份验证的token
                                var authstr = Client_Get(param, "" + WorkFlowAPI + "/OAuth/token", "").Result;
                                JObject authjobject = (JObject)JsonConvert.DeserializeObject(authstr);
                                string tokenstr = ((JValue)authjobject["authorization"]).Value.ToString();
                                foreach (var workflowitem in workflowCheckData)
                                {
                                    //构造调用发起审批流程的api参数
                                    var formData = new MultipartFormDataContent();
                                    //formData.Add(new StringContent("测试审批流"), "bpmName");
                                    //formData.Add(new StringContent("Add_WorkFlow"), "applyTitle");
                                    formData.Add(new StringContent("admin"), "userName");
                                    formData.Add(new StringContent(entityId.ToString()), "tableFlag");
                                    //发起审批流程
                                    JObject resultpost = (JObject)JsonConvert.DeserializeObject(Client_Post(formData, workflowitem.WorkFlowApi, tokenstr).Result);
                                    string instanceID = string.Empty;
                                    bool workflow = bool.Parse(((JValue)resultpost["isok"]).Value.ToString());
                                    instanceID = ((JValue)resultpost["entityID"]).Value.ToString();
                                    if (workflow)
                                    {
                                        connection.Execute($"update {entitydata.StageTable} set WorkFlow_InstanceID='{instanceID}' where batch_id='{batch_id}' and WorkFlow_ProcessingType='{workflowitem.WorkFlow_ProcessingType}'");
                                    };
                                    if (!workflow && instanceID == "-1")
                                    {
                                        isWorkFlow = false;
                                        connection.Execute($"update {entitydata.StageTable} set WorkFlowstatus=0 where batch_id='{batch_id}' and WorkFlow_ProcessingType='{workflowitem.WorkFlow_ProcessingType}'");
                                    };
                                }
                            }
                            //进行数据导入 
                            connection.Execute(entitydata.DataImportProc, para, null, 18000, CommandType.StoredProcedure);
                            //var columns_track = _dbContext.system_attribute.Where(a => a.EntityID == entityId && a.StartTrace == "1").Select(a => a.Name).ToList();
                            //if (columns_track.Count > 0) //判断是否存在追踪字段
                            //{
                            //    var entity = _dbContext.system_entity.Where(e => e.Id == entityId).Select(e => new { e.StageTable, e.EntityTable }).FirstOrDefault();
                            //    StringBuilder sqlColumns = new StringBuilder();
                            //    foreach (var column in columns_track)
                            //    {
                            //        sqlColumns.Append(column + ",");
                            //    }
                            //    string columnstr = sqlColumns.ToString();
                            //    string sqlstr = $"select {columnstr} Code from {entity.StageTable}  where batch_id='{batch_id}'";
                            //    DataTable stgtable = new DataTable();
                            //    var reader = connection.ExecuteReader(sqlstr.ToString());
                            //    stgtable.Load(reader);
                            //    var dtcolumns = stgtable.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList();
                            //    for (int i = 0; i < stgtable.Rows.Count; i++)
                            //    {
                            //        string stg_mdm_sqlstr=$"select Code from {entity.EntityTable} where Code='{stgtable.Rows[i]["Code"]}'";
                            //        var ishave = connection.Query(stg_mdm_sqlstr);
                            //        if (ishave.Count() > 0)
                            //        {
                            //            foreach (var item in dtcolumns)
                            //            {
                            //                string insertsqlstr = $"insert into system_version_management_zipper(EntityID,Attribute,Value,CreateTime,Creator) vaues({entityId},'{item}','{stgtable.Rows[i][item]}','{DateTime.Now}','{CurrentUser.UserAccount}')";
                            //            }
                            //        }
                            //    }
                            //}
                            #endregion
                        }
                        if (!isWorkFlow)
                        {
                            result.success = true;
                            result.message = "上传成功";
                            enlogfailed.State = "成功";
                            enlogfailed.ErrorMessages = "无";
                        }
                        else
                        {
                            result.success = true;
                            result.batchid = batch_id;
                            result.message = "添加成功, 部分数据需要进行流程审批";
                            enlogfailed.State = "成功";
                            enlogfailed.ErrorMessages = "添加成功, 部分数据需要进行流程审批";
                        }
                        _dbContext.system_entitydatchlogs.Add(enlogfailed);
                        _dbContext.SaveChanges();

                    }
                    else
                    {
                        enlogfailed.State = "失败";
                        enlogfailed.ErrorMessages = "上传失败，导入数据库操作失败，没有数据";
                        _dbContext.system_entitydatchlogs.Add(enlogfailed);
                        _dbContext.SaveChanges();
                        result.batchid = batch_id;
                        result.success = false;
                        result.message = "上传失败，导入数据库操作失败";
                    }

                }
                else
                {
                    enlogfailed.State = "失败";
                    enlogfailed.ErrorMessages = "空数据";
                    _dbContext.system_entitydatchlogs.Add(enlogfailed);
                    _dbContext.SaveChanges();
                    result.batchid = batch_id;
                    result.success = false;
                    result.message = "空数据";
                }
            }
            catch (Exception ex)
            {
                enlogfailed.State = "失败";
                enlogfailed.ErrorMessages = "上传失败," + ex.Message;
                _dbContext.system_entitydatchlogs.Add(enlogfailed);
                _dbContext.SaveChanges();
                result.batchid = batch_id;
                result.success = false;
                result.message = "上传失败," + ex.Message;
                throw;
            }
            return result;
        }


        /// <summary>
        /// 获取上传操作日志列表
        /// </summary>
        /// <param name="BatchID"></param>
        /// <returns></returns>
        public object GetErrorLogs(string BatchID, int page, int rows)
        {
            tableResult result = new tableResult();
            try
            {
                var batchIds = BatchID.Split(',');
                var errorLogs = this._dbContext.system_entitydatchlogs.AsNoTracking().Where(it => batchIds.Contains(it.BatchID)).Select(it => new { it.Id, it.ErrorMessages, it.State });
                result.data = errorLogs.Skip((page - 1) * rows).Take(rows).ToList();
                result.total = errorLogs.Count();
                result.success = true;
                result.message = "查询成功";
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// 下载上传错误文件
        /// </summary>
        /// <param name="BatchID"></param>
        /// <param name="entityid"></param>
        /// <returns></returns>
        public byte[] DownError(string BatchID, string entityid)
        {
            try
            {
                IWorkbook workbook = new NPOI.XSSF.UserModel.XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("sheet");
                IRow Title = sheet.CreateRow(0);
                Title.CreateCell(0).SetCellValue("错误描述");
                var stageTable = this._dbContext.system_entity.AsNoTracking().Where(it => it.Id.ToString() == entityid).Select(it => it.StageTable).FirstOrDefault();
                var attributeColumn = _dbContext.system_attribute.AsNoTracking().Where(e => e.EntityID.ToString() == entityid && e.IsSystem == 0).Select(e => new { e.DisplayName, e.Name }).ToList();
                for (int i = 0; i < attributeColumn.Count; i++)
                {
                    Title.CreateCell(i + 1).SetCellValue(attributeColumn[i].DisplayName.ToString() == "" ? attributeColumn[i].Name : attributeColumn[i].DisplayName);
                }
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < attributeColumn.Count; i++)
                {
                    if (i == attributeColumn.Count - 1)
                    {
                        builder.Append(attributeColumn[i].Name);
                    }
                    else
                    {
                        builder.Append(attributeColumn[i].Name + ",");
                    }
                }
                DataTable list = new DataTable();
                DataTable ErrorDt = new DataTable();
                IDataReader reader = null;
                IDataReader reader1 = null;
                using (IDbConnection con = DapperContext.Connection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@batchID", BatchID);
                    string errQuery = $"SELECT Code, Name, BissnessRuleErrorDesc, ValidateErrorDesc from { stageTable.ToLower()} where batch_id = @batchID and ValidateStatus = 2";
                    var errormsgsQuery = con.Query(errQuery, parameters).AsQueryable().AsNoTracking();
                    if (errormsgsQuery.Any())
                    {
                        reader = con.ExecuteReader(errQuery, parameters);
                    }
                    else
                    {
                        reader = con.ExecuteReader($"SELECT Code,Name,BissnessRuleErrorDesc,ValidateErrorDesc from {stageTable.ToLower()} where batch_id=@batchID and BissnessRuleStatus=2", parameters);
                    }
                    ErrorDt.Load(reader);
                    reader1 = con.ExecuteReader($"select {builder.ToString()} from {stageTable} where batch_id=@batchID", parameters);
                    list.Load(reader1);
                }
                var errTable = ErrorDt.AsEnumerable();
                DataRow rowValue = null;
                for (int i = 0; i < list.Rows.Count; i++)
                {
                    rowValue = list.Rows[i];
                    IRow row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < attributeColumn.Count; j++)
                    {
                        ICell cell = row.CreateCell(j);
                        if (j == 0)
                        {
                            var has = errTable.FirstOrDefault(e => e.Field<string>("Code") == rowValue.Field<string>("Code"));
                            if (has != null)
                            {
                                cell.SetCellValue(has.Field<string>("ValidateErrorDesc") + "  " + has.Field<string>("BissnessRuleErrorDesc") ?? " ");
                            }
                            else
                            {
                                cell.SetCellValue("");
                            }
                        }
                        else
                        {
                            cell.SetCellValue(rowValue[attributeColumn[j - 1].Name]?.ToString() ?? "");
                        }
                    }
                }
                byte[] buffer = new byte[1024 * 5];
                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.Write(ms);
                    //ms.Flush();
                    //ms.Position=0;
                    buffer = ms.ToArray();
                    ms.Close();
                }
                return buffer;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public DataTable InsertDataToDataTable(Stream filestream)
        //{
        //    int maxRow = 100000;
        //    DataTable dt = new DataTable();

        //    int rowCount = 0;
        //    do
        //    {
        //        while (excelReader.Read())
        //        {
        //            DataRow row = dt.NewRow();
        //            if (rowCount != 0)
        //            {
        //                //获取Excel数据
        //                for (int i = 0; i < excelReader.FieldCount; i++)
        //                {
        //                    row[i] = excelReader[i] == null ? "" : excelReader[i].ToString();
        //                }
        //                dt.Rows.Add(row);
        //            }
        //            else
        //            { //第一行，获取Excel表头
        //                for (int i = 0; i < excelReader.FieldCount; i++)
        //                {
        //                    dt.Columns.Add(excelReader[i] == null ? "" : excelReader[i].ToString());
        //                }
        //            }
        //            if (dt.Rows.Count == maxRow)
        //            {
        //                yield return dt;
        //                dt.Rows.Clear();
        //                dt.Clear();
        //            }
        //            rowCount++;
        //        }
        //    } while (excelReader.NextResult());
        //    excelReader.Close();
        //}

        /// <summary>
        /// Mysql 数据批量导入数据库  2020年4月30日12:55:16  Dennyhui
        /// </summary>
        /// <param name="_mySqlConnection">mysql连接字符串</param>
        /// <param name="Columns">表头</param>
        /// <param name="stream">数据流</param>
        /// <returns></returns>
        public int MysqlBulkLoad(MySqlConnection _mySqlConnection, List<string> Columns, string TableName, DataTable dtdata)
        {
            StringBuilder sCommand = new StringBuilder("INSERT INTO " + TableName + " ( ");
            StringBuilder filedstr = new StringBuilder();
            foreach (var colitem in Columns)
            {
                filedstr.Append(colitem + ",");
            }
            string filed = DelLastComma(filedstr.ToString());
            sCommand.Append(filed);
            sCommand.Append(") VALUE");
            using (MySqlConnection mConnection = _mySqlConnection)
            {
                List<string> Rows = new List<string>();
                for (int i = 0; i < dtdata.Rows.Count; i++)
                {
                    StringBuilder colv = new StringBuilder("(");
                    for (int r = 0; r < Columns.Count; r++)
                    {
                        colv.Append("'" + MySqlHelper.EscapeString(dtdata.Rows[i][Columns[r]].ToString()) + "',");
                    }
                    string colstr = DelLastComma(colv.ToString()) + ")";
                    Rows.Add(colstr);
                }
                sCommand.Append(string.Join(",", Rows));
                sCommand.Append(";");
                int result = 0;
                using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                {
                    myCmd.CommandType = CommandType.Text;
                    result = myCmd.ExecuteNonQuery();
                }
                return result;
            }
            //MySqlBulkLoader bulk = new MySqlBulkLoader(_mySqlConnection)
            //{
            //    CharacterSet = "UTF8",//防止中文乱码
            //    FieldTerminator = ",",
            //    FieldQuotationCharacter = '"',
            //    EscapeCharacter = '"',
            //    LineTerminator = "\r\n",
            //    //FileName = table.TableName + ".csv",
            //    SourceStream =  stream,
            //    NumberOfLinesToSkip = 0,
            //    TableName = TableName,
            //    Local = true
            //};
            //bulk.Columns.AddRange(Columns);
            //return await bulk.LoadAsync();
        }

        /// <summary>
        ///将DataTable转换为标准的CSV，最后转成流数据 2020年4月30日13:38:21 Dennyhui
        /// </summary>
        /// <param name="table">数据表</param>
        /// <returns>返回标准的CSV流数据</returns>
        public Stream DTToCsvStream(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            try
            {
                StringBuilder sb = new StringBuilder();
                DataColumn colum;
                foreach (DataRow row in table.Rows)
                {
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        colum = table.Columns[i];
                        if (i != 0) sb.Append(",");
                        if (/*colum.DataType == typeof(string) && */row[colum].ToString().Contains(","))
                        {
                            sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                        }
                        else sb.Append(row[colum].ToString());
                    }
                    sb.AppendLine();
                }
                byte[] array = Encoding.UTF8.GetBytes(string.Format(sb.ToString()));//使用utf8编码防止中文乱码
                MemoryStream stream = new MemoryStream(array);             //convert stream 2 string      
                //Stream csvstream = new MemoryStream(array);
                return stream;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// 查询需要审批的数据  2020年5月20日16:46:37  Dennyhui
        /// </summary>
        /// <param name="workflow_instanceid"></param>
        /// <param name="entityid"></param>
        /// <returns></returns>
        public object GetWorkFlow_ApproveData(string workflow_instanceid, string entityid, int page, int rows)
        {
            try
            {
                tableResult result = new tableResult();
                var StageTable = _dbContext.system_entity.AsNoTracking().Where(e => e.Id.ToString() == entityid).Select(it => it.StageTable).FirstOrDefault(); ;
                using (IDbConnection connection = DapperContext.Connection())
                {
                    var query = connection.Query($"SELECT * from {StageTable} where WorkFlow_InstanceID={workflow_instanceid} and ValidateStatus=1").AsQueryable();
                    result.data = query.Skip((page - 1) * rows).Take(rows).ToList();
                    result.total = query.Count();
                    result.success = true;
                    result.message = "查询成功";
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// 审批数据成功，导入数据  2020年5月20日18:26:35  Dennyhui
        /// </summary>
        /// <param name="workflow_instanceid"></param>
        /// <param name="entityid"></param>
        /// <returns></returns>
        public object ApproveData_OK(string workflow_instanceid, string entityid)
        {
            Result result = new Result();
            try
            {
                var entitydata = _dbContext.system_entity.Where(e => e.Id.ToString() == entityid).FirstOrDefault();
                using (IDbConnection connection = DapperContext.Connection())
                {
                    DataTable dt = new DataTable();
                    IDataReader reader = connection.ExecuteReader($"SELECT  batch_id FROM {entitydata.StageTable}  WHERE WorkFlow_InstanceID='{workflow_instanceid}' limit 1");
                    dt.Load(reader);
                    connection.Execute($"update {entitydata.StageTable} set WorkFlowstatus=0 where batch_id= '{dt.Rows[0]["batch_id"]}'");
                    //进行数据导入 
                    var i_para = new DynamicParameters();
                    i_para.Add("@batchid", dt.Rows[0].Field<string>("batch_id"));
                    //var importresult = connection.Query(entitydata.DataImportProc, i_para, null, true, null, CommandType.StoredProcedure).FirstOrDefault();
                    connection.Execute(entitydata.DataImportProc, i_para, null, 18000, CommandType.StoredProcedure);
                    result.success = true;
                    result.message = " 操作成功";
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 编辑距离（Levenshtein Distance）
        /// </summary>
        /// <param name="source">源串</param>
        /// <param name="target">目标串</param>
        /// <param name="similarity">输出：相似度，值在0～１</param>
        /// <param name="isCaseSensitive">是否大小写敏感</param>
        /// <returns>源串和目标串之间的编辑距离</returns>
        public static Int32 LevenshteinDistance(String source, String target, out Double similarity, Boolean isCaseSensitive = false)
        {
            if (String.IsNullOrEmpty(source))
            {
                if (String.IsNullOrEmpty(target))
                {
                    similarity = 1;
                    return 0;
                }
                else
                {
                    similarity = 0;
                    return target.Length;
                }
            }
            else if (String.IsNullOrEmpty(target))
            {
                similarity = 0;
                return source.Length;
            }

            String From, To;
            if (isCaseSensitive)
            {   // 大小写敏感
                From = source;
                To = target;
            }
            else
            {   // 大小写无关
                From = source.ToLower();
                To = target.ToLower();
            }

            // 初始化
            Int32 m = From.Length;
            Int32 n = To.Length;
            Int32[,] H = new Int32[m + 1, n + 1];
            for (Int32 i = 0; i <= m; i++) H[i, 0] = i;  // 注意：初始化[0,0]
            for (Int32 j = 1; j <= n; j++) H[0, j] = j;

            // 迭代
            for (Int32 i = 1; i <= m; i++)
            {
                Char SI = From[i - 1];
                for (Int32 j = 1; j <= n; j++)
                {   // 删除（deletion） 插入（insertion） 替换（substitution）
                    if (SI == To[j - 1])
                        H[i, j] = H[i - 1, j - 1];
                    else
                        H[i, j] = Math.Min(H[i - 1, j - 1], Math.Min(H[i - 1, j], H[i, j - 1])) + 1;
                }
            }

            // 计算相似度
            Int32 MaxLength = Math.Max(m, n);   // 两字符串的最大长度
            similarity = ((Double)(MaxLength - H[m, n])) / MaxLength;

            return H[m, n];    // 编辑距离
        }

        public object testsimilar(string entityID, string mergingCode)
        {
            var entity = _dbContext.system_entity.Where(e => e.Id.ToString() == entityID).FirstOrDefault();
            var Threshold_str = _dbContext.system_mergingrules.Where(m => m.MergingCode == mergingCode && m.Validity == "1").Select(m => m.SelfMotion).FirstOrDefault();//获取阀值
            var Threshold = Int32.Parse(Threshold_str);
            string mdmtable = entity.EntityTable;
            var columns = (from r in _dbContext.system_rulesdetails
                           join a in _dbContext.system_attribute on r.AttributeID equals a.Id
                           where r.MerginfCode == mergingCode && r.Validity == "1"
                           select new
                           {
                               a.Name,
                               r.Weight
                           }
                         ).ToList();
            StringBuilder sqlColumns = new StringBuilder();
            foreach (var column in columns)
            {
                sqlColumns.Append(column.Name + ",");
            }
            sqlColumns.Append("Code,");
            string columnstr = DelLastComma(sqlColumns.ToString());
            string sqlQuery = "select " + columnstr + " from " + mdmtable;
            DataTable table = new DataTable();
            using (IDbConnection connection = DapperContext.Connection())
            {
                //先清空该实体的历史数据
                string delsql = "DELETE FROM system_mergingrules_similarresult WHERE EntityID=" + entityID;
                connection.Execute(delsql);
                var reader = connection.ExecuteReader(sqlQuery);
                table.Load(reader);
            }
            Double Similarity;
            var rstable = table.AsEnumerable();
            var similarResult = rstable.Where(rt => LevenshteinDistance(rt.Field<string>("Address"), "广州市", out Similarity, true) > 0.7);
            return "";
        }

        public object testGlobalException()
        {
            try
            {
                int[] numbers = new int[3] { 1, 2, 3 };//定长
                var a = numbers[5];
                return "1";
            }
            catch (Exception)
            {
                throw;
                return "";
            }


        }
    }
}
