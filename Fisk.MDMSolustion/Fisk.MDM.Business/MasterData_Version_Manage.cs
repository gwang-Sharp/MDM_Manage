using Dapper;
using Fisk.MDM.DataAccess;
using Fisk.MDM.DataAccess.Models;
using Fisk.MDM.Interface;
using Fisk.MDM.Utility.Common;
using Fisk.MDM.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fisk.MDM.Business
{
    /// <summary>
    /// 主数据版本管理 WG
    /// </summary>
    public class MasterData_Version_Manage : IMasterData_Version_Manage
    {
        private readonly MDMDBContext _dbContext;
        private readonly SessionHelper helper;
        public MasterData_Version_Manage(MDMDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = dbContext;
            helper = new SessionHelper(httpContextAccessor);
        }
        #region 版本管理 wg
        /// <summary>
        /// 实体版本快照表格
        /// </summary>
        /// <param name="EntityID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public object InitVersionTable(int EntityID, int page, int rows)
        {
            tableResult result = new tableResult();
            try
            {
                using (IDbConnection con = DapperContext.Connection())
                {
                    //查询版本管理表数据以及实体相关联的表名
                    var versionQuery = this._dbContext.system_version_snapshot.AsNoTracking().Where(it => it.EntityID == EntityID);
                    string sql = @"SELECT e.`EntityTable` FROM `system_attribute` t 
                           LEFT JOIN system_entity e
                           on t.LinkEntityID = e.id
                           where t.Type = '基于域' and t.EntityID =@EntityID";
                    DynamicParameters dynamic = new DynamicParameters();
                    dynamic.Add("@EntityID", EntityID, DbType.Int32);
                    var linkTableNames = con.Query(sql, dynamic).ToList();
                    result.success = true;
                    result.data = versionQuery.Skip((page - 1) * rows).Take(rows).ToList();
                    result.message = "查询成功";
                    result.ExtraData = linkTableNames;
                    result.total = versionQuery.Count();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取实体开启跟踪的属性列表
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public object InitAttrStraceTable(int entityID, string attrID, int page, int rows)
        {
            tableResult result = new tableResult();
            try
            {
                using (IDbConnection con = DapperContext.Connection())
                {
                    //查询版本管理表数据以及实体相关联的表名
                    DynamicParameters dynamic = new DynamicParameters();
                    dynamic.Add("@EntityID", entityID, DbType.Int32);
                    string sql = @"SELECT s.`Name` as name,a.StartTrace, a.DisplayName as AttributeName,s.Id,s.CreateTime,z.`Value` FROM `system_entity` s
                                   LEFT JOIN system_attribute a
                                   on s.id=a.EntityID
                                   left join system_version_zipper z
                                   on a.EntityID=z.AttributeID
                                   where s.Id=@EntityID";

                    if (!string.IsNullOrEmpty(attrID))
                    {
                        dynamic.Add("@AttrID", attrID);
                        sql = @"SELECT s.`Name` as name,a.StartTrace, a.DisplayName as AttributeName,s.Id,s.CreateTime,z.`Value` FROM `system_entity` s
                                LEFT JOIN system_attribute a
                                on s.id=a.EntityID
                                left join system_version_zipper z
                                on a.EntityID=z.EntityID
                                where s.Id=@EntityID and a.StartTrace='1' and z.EntityID=@EntityID  and z.AttributeID=@AttrID";
                    }
                    var tableQuery = con.Query(sql, dynamic).AsQueryable();
                    result.success = true;
                    result.data = tableQuery.Skip((page - 1) * rows).Take(rows).ToList();
                    result.message = "查询成功";
                    result.total = tableQuery.Count();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 数据版本备份
        /// </summary>
        /// <param name="linkTable"></param>
        /// <param name="entityId"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public async Task<bool> CreateVersion(string linkTable, int entityId, string versionName)
        {
            try
            {
                var historyTable = "";
                if (string.IsNullOrEmpty(linkTable))
                {
                    var thisEntity = this._dbContext.system_entity.AsNoTracking().Where(it => it.Id == entityId).Select(it => new { it.HistoryTable, it.EntityTable }).FirstOrDefault();
                    linkTable = thisEntity.EntityTable;
                    historyTable = thisEntity.HistoryTable;
                }
                else
                {
                    historyTable = this._dbContext.system_entity.AsNoTracking().Where(it => it.EntityTable == linkTable).Select(it => it.HistoryTable).FirstOrDefault();
                }
                using (MySqlConnection con = DapperContext.Connection())
                {
                    if (con.Query($"select VersionId from {historyTable} where VersionId={versionName}").Any())
                    {
                        return true;
                    }
                    string sql = $"select * from {linkTable}";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.Add("VersionId");
                        foreach (DataRow dr in dt.Rows)
                        {
                            //为新添加的列进行赋值
                            dr["VersionId"] = versionName;
                        }
                        var columns = dt.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList();
                        Stream csvStream = DTToCsvStream(dt);//datatable转成CSV数据流
                        int count = await MysqlBulkLoad(DapperContext.Connection(), columns, csvStream, historyTable);//CSV数据流导入数据库
                        if (count >= 0)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除对应版本数据
        /// </summary>
        /// <param name="EntityID"></param>
        /// <param name="versionName"></param>
        /// <returns></returns>
        public object VersionDel(int EntityID, string versionName)
        {
            Result result = new Result();
            try
            {
                using (IDbConnection con = DapperContext.Connection())
                {

                    string sql = $@"SELECT e.`HistoryTable` FROM `system_attribute` t 
                           LEFT JOIN system_entity e
                           on t.LinkEntityID = e.id
                           where t.Type = '基于域' and t.EntityID ={EntityID}";
                    DataTable linkTableNames = new DataTable();
                    IDataReader reader = con.ExecuteReader(sql);
                    linkTableNames.Load(reader);
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@VersionName", versionName, DbType.String);
                    var linkTables = linkTableNames.AsEnumerable();
                    foreach (DataRow item in linkTables)
                    {
                        if (item[0] != DBNull.Value)
                        {
                            con.Execute($"DELETE from {item["HistoryTable"]} where VersionId=@VersionName", parameters);
                        }
                    }
                    var ThisEntityHis = this._dbContext.system_entity.AsNoTracking().Where(it => it.Id == EntityID).Select(it => it.HistoryTable).FirstOrDefault();
                    var id = this._dbContext.system_version_snapshot.AsNoTracking().Where(it => it.Name == versionName).Select(it => it.Id).FirstOrDefault();
                    if (!string.IsNullOrEmpty(ThisEntityHis))
                    {
                        con.Execute($"delete from {ThisEntityHis} where VersionId=@VersionName", parameters);
                    }
                    con.Execute($"delete FROM system_version_snapshot_detail where LinkEntityID={EntityID} and VersionID={id}");
                    con.Execute($"delete from system_version_snapshot where `Name`=@VersionName and EntityID={EntityID}", parameters);
                    result.success = true;
                    result.message = "删除成功";
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
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
                byte[] array = Encoding.UTF8.GetBytes(sb.ToString());//使用utf8编码防止中文乱码
                //MemoryStream stream = new MemoryStream(array);             //convert stream 2 string      
                Stream csvstream = new MemoryStream(array);
                return csvstream;
            }
            catch (Exception ex)
            {
                throw;

            }
        }


        /// <summary>
        /// Mysql 数据批量导入数据库  2020年4月30日12:55:16  Dennyhui
        /// </summary>
        /// <param name="_mySqlConnection">mysql连接字符串</param>
        /// <param name="Columns">表头</param>
        /// <param name="stream">数据流</param>
        /// <returns></returns>
        public async Task<int> MysqlBulkLoad(MySqlConnection _mySqlConnection, List<string> Columns, Stream stream, string TableName)
        {
            MySqlBulkLoader bulk = new MySqlBulkLoader(_mySqlConnection)
            {
                CharacterSet = "UTF8",//防止中文乱码
                FieldTerminator = ",",
                FieldQuotationCharacter = '"',
                EscapeCharacter = '"',
                LineTerminator = "\r\n",
                //FileName = table.TableName + ".csv",
                SourceStream = stream,
                NumberOfLinesToSkip = 0,
                TableName = TableName,
                Local = true
            };
            bulk.Columns.AddRange(Columns);
            return await bulk.LoadAsync();
        }
        #endregion
    }
}
