using Dapper;
using Fisk.MDM.DataAccess;
using Fisk.MDM.DataAccess.Models;
using Fisk.MDM.Interface;
using Fisk.MDM.Utility.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Fisk.MDM.Business
{
    /// <summary>
    /// 主数据维护管理 WG
    /// </summary>
    public class MasterData_Maintain_Manage : IMasterData_Maintain_Manage
    {
        private readonly MDMDBContext _dbContext;
        private readonly SessionHelper helper;
        public MasterData_Maintain_Manage(MDMDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = dbContext;
            helper = new SessionHelper(httpContextAccessor);
        }
        #region 维护管理 WG
        /// <summary>
        /// 获取实体表格列
        /// </summary>
        /// <param name="EntityID"></param>
        /// <returns></returns>
        public object InitEntityTable(int EntityID, string where, int page, int rows)
        {
            List<dynamic> cols = null;
            try
            {
                using (IDbConnection con = DapperContext.Connection())
                {
                    //获取表头列
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@EntityID", EntityID, DbType.Int32);
                    DataTable ForeignAttrsDT = new DataTable();
                    DataTable ForeignTables = new DataTable();
                    Dictionary<string, List<dynamic>> ForeignMdmDatas = new Dictionary<string, List<dynamic>>();
                    var Entity = this._dbContext.system_entity.AsNoTracking().Where(it => it.Id == EntityID).Select(it => new { it.EntityTable, it.Name }).FirstOrDefault();
                    cols = con.Query("SELECT t.`Name`,t.DisplayName FROM `system_attribute` t where EntityID=@EntityID", parameters).ToList();
                    if (con.Query("select name from system_attribute where EntityID=@EntityID and type='基于域'", parameters).Any())
                    {
                        IDataReader ForeignAttrsReader = con.ExecuteReader("select name from system_attribute where EntityID=@EntityID and type='基于域'", parameters);
                        //获取基于域的属性
                        ForeignAttrsDT.Load(ForeignAttrsReader);
                        //获取基于域实体的mdm表
                        IDataReader ForeignTablesReader =  con.ExecuteReader("select EntityTable  from system_entity where id in (select LinkEntityID from system_attribute where EntityID=@EntityID and Type='基于域')", parameters);
                        ForeignTables.Load(ForeignTablesReader);
                        List<dynamic> ForeignData = null;
                        DataRow rowValue = null;
                        int i = 0;
                        foreach (DataRow item in ForeignTables.Rows)
                        {
                            rowValue = ForeignAttrsDT.Rows[i];
                            if (item["EntityTable"].ToString() != Entity.EntityTable)
                            {
                                ForeignData = con.Query(@$"select DISTINCT  b.`Code`, b.`Name` from {Entity.EntityTable}
                                                        left join {item["EntityTable"].ToString()} b
                                                        on {rowValue["name"].ToString()} = b.code").AsQueryable().AsTracking().ToList();
                            }
                            else
                            {
                                ForeignData = con.Query(@$"select DISTINCT `Code`, `Name` from {Entity.EntityTable}").AsQueryable().AsNoTracking().ToList();
                            }
                            ForeignMdmDatas.Add(rowValue["name"].ToString(), ForeignData);
                            i++;
                        }
                    }
                    //获取mdm表数据
                    StringBuilder builder = new StringBuilder($"SELECT * FROM {Entity.EntityTable}  ");
                    if (!string.IsNullOrEmpty(where))
                    {
                        builder.Append(" where ");
                        JArray jArray = JArray.Parse(where);
                        foreach (var item in jArray)
                        {
                            if (item["Operator"].ToString() == "匹配")
                            {
                                builder.Append(item["AttrName"].ToString() + " like '%" + item["Content"].ToString() + "%' and ");
                            }
                            else if (item["Operator"].ToString() == "不匹配")
                            {
                                builder.Append(item["AttrName"].ToString() + " not like '%" + item["Content"].ToString() + "%' and ");
                            }
                            else if (item["Operator"].ToString() == "开头为")
                            {
                                builder.Append(item["AttrName"].ToString() + " like '" + item["Content"].ToString() + "%' and ");
                            }
                            else if (item["Operator"].ToString() == "is not null")
                            {
                                builder.Append(item["AttrName"].ToString() + " is not null and ");
                            }
                            else if (item["Operator"].ToString() == "is null")
                            {
                                builder.Append(item["AttrName"].ToString() + " is null  and ");
                            }
                            else if (item["Operator"].ToString() == "不包含模式")
                            {
                                builder.Append(item["AttrName"].ToString() + " not in (" + item["Content"].ToString() + " " + ") and ");
                            }
                            else if (item["Operator"].ToString() == "包含模式")
                            {
                                builder.Append(item["AttrName"].ToString() + " in (" + item["Content"].ToString() + " " + ") and ");
                            }
                            else
                            {
                                builder.Append(item["AttrName"].ToString() + " " + item["Operator"].ToString() + "  '" + item["Content"].ToString() + "'  and ");
                            }

                        }
                        builder.Append(" 1=1; ");
                    }
                    var MdmData = con.Query(builder.ToString()).AsQueryable().AsNoTracking();
                    return new { success = true, msg = "查询成功", tableCols = cols, data = MdmData.Skip((page - 1) * rows).Take(rows).ToList(), total = MdmData.Count(), EntityName = Entity.Name, LinkTableData = ForeignMdmDatas };
                }
            }
            catch (Exception ex)
            {
                throw;
                //return new { success = false, msg = "初始化失败,Err:系统内部错误！", tableCols = cols, data = "", total = 0, EntityName = "", LinkTableData = "" };
            }
        }


        /// <summary>
        /// 对外页面表格渲染
        /// </summary>
        /// <param name="EntityID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public object InitViewTable(string EntityName, string where, int page, int rows)
        {
            try
            {
                using (IDbConnection con = DapperContext.Connection())
                {
                    //获取表头列
                    DynamicParameters parameters = new DynamicParameters();
                    DataTable ForeignAttrsDT = new DataTable();
                    DataTable ForeignTables = new DataTable();
                    Dictionary<string, List<dynamic>> ForeignMdmDatas = new Dictionary<string, List<dynamic>>();
                    var Entity = this._dbContext.system_entity.AsNoTracking().Where(it => it.Name == EntityName).Select(it => new { it.EntityTable, it.Name, it.Id }).FirstOrDefault();
                    parameters.Add("@EntityID", Entity.Id, DbType.Int32);
                    var cols = con.Query("SELECT t.`Name`,t.DisplayName FROM `system_attribute` t where EntityID=@EntityID", parameters).ToList();
                    if (con.Query("select name from system_attribute where EntityID=@EntityID and type='基于域'", parameters).Any())
                    {
                        IDataReader ForeignAttrsReader = con.ExecuteReader("select name from system_attribute where EntityID=@EntityID and type='基于域'", parameters);
                        //获取基于域的属性
                        ForeignAttrsDT.Load(ForeignAttrsReader);
                        //获取基于域实体的mdm表
                        IDataReader ForeignTablesReader = con.ExecuteReader("select EntityTable  from system_entity where id in (select LinkEntityID from system_attribute where EntityID=@EntityID and Type='基于域')", parameters);
                        ForeignTables.Load(ForeignTablesReader);
                        int i = 0;
                        DataRow rowValue = null;
                        List<dynamic> ForeignData = null;
                        foreach (DataRow item in ForeignTables.Rows)
                        {
                            rowValue = ForeignAttrsDT.Rows[i];
                            if (item["EntityTable"].ToString() != Entity.EntityTable)
                            {
                                ForeignData = con.Query(@$"select DISTINCT  b.`Code`, b.`Name` from {Entity.EntityTable}
                                                        left join {item["EntityTable"].ToString()} b
                                                        on {rowValue["name"].ToString()} = b.code").ToList();
                            }
                            else
                            {
                                ForeignData = con.Query(@$"select DISTINCT `Code`, `Name` from {Entity.EntityTable}").ToList();
                            }
                            ForeignMdmDatas.Add(rowValue["name"].ToString(), ForeignData);
                            i++;
                        }
                    }
                    //获取mdm表数据
                    StringBuilder builder = new StringBuilder($"SELECT * FROM {Entity.EntityTable} where ");
                    if (!string.IsNullOrEmpty(where))
                    {
                        builder.Append($" {where} and ");
                    }
                    builder.Append(" Validity = 1 ");
                    var MdmData = con.Query(builder.ToString()).AsQueryable();
                    return new { success = true, msg = "查询成功", tableCols = cols, data = MdmData.Skip((page - 1) * rows).Take(rows).ToList(), total = MdmData.Count(), EntityID = Entity.Id, LinkTableData = ForeignMdmDatas };
                }
            }
            catch (Exception ex)
            {
                throw;
                //return new { success = false, msg ="初始化失败,Err:系统内部错误！", tableCols = "", data = "", total = 0, EntityID = "", LinkTableData = "" };
            }
        }
        #endregion
    }
}
