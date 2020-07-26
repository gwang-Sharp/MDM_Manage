using Dapper;
using Fisk.MDM.DataAccess;
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
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using Z.EntityFramework.Plus;

namespace Fisk.MDM.Business
{
    /// <summary>
    /// 主数据质量管理
    /// </summary>
    public class MasterData_Quality_Manage : IMasterData_Quality_Manage
    {

        private readonly MDMDBContext _dbContext;
        public MasterData_Quality_Manage(MDMDBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        #region 数据维护 WG
        /// <summary>
        /// 获取数据维护表信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public object GetDatamaintenance(int page, int rows)
        {
            tableResult result = new tableResult();
            try
            {
                var query = from d in this._dbContext.system_datamaintenance.AsNoTracking()
                            join e in this._dbContext.system_entity.AsNoTracking()
                            on d.EntityID equals e.Id
                            select new
                            {
                                d.Id,
                                d.RuleName,
                                d.RuleRemark,
                                d.RuleType,
                                d.Apiaddress,
                                d.MergeAPI,
                                d.CreateUser,
                                d.CreateTime,
                                d.UpdateUser,
                                d.UpdateTime,
                                d.AttributeID,
                                d.BpmName,
                                d.ApplyTitle,
                                d.EntityID,
                                e.Name
                            };
                result.success = true;
                result.data = query.Skip((page - 1) * rows).Take(rows).ToList();
                result.total = query.Count();
                result.message = "查询成功";
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 添加实体数据维护规则
        /// </summary>
        /// <param name="EntityID"></param>
        /// <param name="_Datamaintenance"></param>
        /// <returns></returns>
        public object AddDatamaintenance(int EntityID, string Datamaintenance)
        {
            Result result = new Result();
            try
            {
                var _Datamaintenance = JsonConvert.DeserializeObject<system_datamaintenance>(Datamaintenance);
                if (this._dbContext.system_datamaintenance.AsNoTracking().Any(it => it.EntityID == EntityID && it.RuleName == _Datamaintenance.RuleName))
                {
                    result.success = false;
                    result.message = "存在相同规则名";
                    return result;
                }
                _Datamaintenance.MergeAPI = _Datamaintenance.Apiaddress + "?bpmName=" + _Datamaintenance.BpmName + "&applyTitle=" + _Datamaintenance.ApplyTitle;
                _Datamaintenance.CreateTime = DateTime.Now;
                _Datamaintenance.CreateUser = CurrentUser.UserAccount;
                _Datamaintenance.UpdateTime = DateTime.Now;
                _Datamaintenance.UpdateUser = CurrentUser.UserAccount;
                _Datamaintenance.EntityID = EntityID;
                this._dbContext.Add(_Datamaintenance);
                int rows = this._dbContext.SaveChanges();
                if (rows > 0)
                {
                    result.success = true;
                    result.message = "添加成功";
                }
                else
                {
                    result.success = false;
                    result.message = "添加失败";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除实体维护规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object DelEntityDataRule(int id)
        {
            Result result = new Result();
            try
            {
                using (IDbConnection con = DapperContext.Connection())
                {
                    var parms = new DynamicParameters();
                    parms.Add("@id", id, DbType.Int32);
                    var rows = con.Execute("DELETE from system_datamaintenance where id=@id", parms);
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
        /// 更新实体数据维护规则
        /// </summary>
        /// <param name="FormModel"></param>
        /// <returns></returns>
        public object UpdataDatamaintenance(string FormModel)
        {
            Result result = new Result();
            try
            {
                var _Datamaintenance = JsonConvert.DeserializeObject<system_datamaintenance>(FormModel);
                var has = this._dbContext.system_datamaintenance.Where(it => it.Id == _Datamaintenance.Id).FirstOrDefault();
                has.RuleName = _Datamaintenance.RuleName;
                has.RuleRemark = _Datamaintenance.RuleRemark;
                has.RuleType = _Datamaintenance.RuleType;
                has.UpdateTime = DateTime.Now;
                has.UpdateUser = CurrentUser.UserAccount;
                has.AttributeID = _Datamaintenance.AttributeID;
                has.Apiaddress = _Datamaintenance.Apiaddress;
                has.MergeAPI = _Datamaintenance.Apiaddress + "?bpmName=" + _Datamaintenance.BpmName + "&applyTitle=" + _Datamaintenance.ApplyTitle;
                has.BpmName = _Datamaintenance.BpmName;
                has.ApplyTitle = _Datamaintenance.ApplyTitle;
                this._dbContext.Entry(has).State = EntityState.Modified;
                this._dbContext.Entry(has).Property(it => it.CreateTime).IsModified = false;
                this._dbContext.Entry(has).Property(it => it.CreateUser).IsModified = false;
                int rows = this._dbContext.SaveChanges();
                if (rows > 0)
                {
                    result.success = true;
                    result.message = "更新成功";
                }
                else
                {
                    result.success = false;
                    result.message = "更新失败";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// 获取实体下开启变更的属性
        /// </summary>
        /// <returns></returns>
        public object Attributes_GetAll_ByEntityID(int EntityID)
        {
            Result result = new Result();
            try
            {
                var obj = this._dbContext.system_attribute.AsNoTracking().Where(it => it.EntityID == EntityID).Select(it => new { it.Name, it.Id }).ToList();
                result.success = true;
                result.message = "查询成功";
                result.data = obj;
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region 合并规则
        public object ChangeThreshold(system_mergingrules item)
        {
            Result result = new Result();
            try
            {
                var ChangeMergingrules = _dbContext.system_mergingrules.Where(x => x.Id == item.Id).FirstOrDefault();
                if (ChangeMergingrules != null)
                {
                    ChangeMergingrules.NoProcessing = item.NoProcessing;
                    ChangeMergingrules.SelfMotion = item.SelfMotion;
                    ChangeMergingrules.Manual = item.Manual;
                    ChangeMergingrules.Updater = CurrentUser.UserAccount;
                    ChangeMergingrules.UpdateTime = DateTime.Now;
                    int count = _dbContext.SaveChanges();
                    if (count != 0)
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
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }


        /// <summary>
        /// 编辑合并规则
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public object EiteMergingrules(MergingrulesItem item)
        {
            Result result = new Result();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var EiteMerging = _dbContext.system_mergingrules.FirstOrDefault(x => x.Validity == "1" && x.Id == item.ID);
                    EiteMerging.ModelID = item.ModelID;
                    EiteMerging.EntityID = item.EntityID;
                    EiteMerging.Updater = CurrentUser.UserAccount;
                    EiteMerging.UpdateTime = DateTime.Now;
                    var DeleteRules = _dbContext.system_rulesdetails.Where(x => x.Validity == "1" && x.MerginfCode == EiteMerging.MergingCode).ToList();
                    foreach (var Del in DeleteRules)
                    {
                        Del.Validity = "0";
                        Del.Updater = CurrentUser.UserAccount;
                        Del.UpdateTime = DateTime.Now;
                    }

                    foreach (var Det in item.RulesOfTheData)
                    {
                        system_rulesdetails detail = new system_rulesdetails();
                        detail.AttributeID = Det.AttributeID;
                        detail.MerginfCode = EiteMerging.MergingCode;
                        detail.CreateTime = DateTime.Now;
                        detail.Updater = CurrentUser.UserAccount;
                        detail.UpdateTime = DateTime.Now;
                        detail.Validity = "1";
                        if (Det.Weight != 0 && Det.AttributeValue != "分组")   //数据为权重
                        {
                            detail.IsGroop = 0;
                            detail.Weight = Det.Weight;
                        }
                        else                               //分组
                        {
                            detail.IsGroop = 1;
                            detail.Weight = 0;
                        }
                        _dbContext.system_rulesdetails.Add(detail);
                    }
                    int count = _dbContext.SaveChanges();
                    if (count != 0)
                    {
                        tran.Commit();
                        result.success = true;
                        result.message = "更新成功";
                    }
                    else
                    {
                        tran.Rollback();
                        result.success = false;
                        result.message = "更新失败";
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
        /// 添加合并规则
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public object InsertMergingrules(MergingrulesItem item)
        {
            Result result = new Result();
            try
            {
                system_mergingrules rules = new system_mergingrules();
                rules.ModelID = item.ModelID;
                rules.EntityID = item.EntityID;
                rules.MergingCode = Guid.NewGuid().ToString();
                rules.Creater = CurrentUser.UserAccount;
                rules.CreateTime = DateTime.Now;
                rules.NoProcessing = "33";
                rules.SelfMotion = "33";
                rules.Manual = "34";
                rules.Updater = CurrentUser.UserAccount;
                rules.UpdateTime = DateTime.Now;
                rules.State = 0;
                rules.Validity = "1";
                _dbContext.system_mergingrules.Add(rules);
                foreach (var Det in item.RulesOfTheData)
                {
                    system_rulesdetails detail = new system_rulesdetails();
                    detail.AttributeID = Det.AttributeID;
                    detail.MerginfCode = rules.MergingCode;
                    detail.CreateTime = DateTime.Now;
                    detail.Updater = CurrentUser.UserAccount;
                    detail.UpdateTime = DateTime.Now;
                    detail.Validity = "1";
                    if (Det.Weight != 0 && Det.AttributeValue != "分组")
                    {
                        detail.IsGroop = 0;
                        detail.Weight = Det.Weight;
                    }
                    else
                    {
                        detail.IsGroop = 1;
                        detail.Weight = 0;
                    }
                    _dbContext.system_rulesdetails.Add(detail);
                }
                if (_dbContext.SaveChanges() != 0)
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
        /// 删除合并规则  2020年4月27日   hhyang
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public object DeleteMergingrules(int ruleID)
        {
            Result result = new Result();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var DeleteMerging = _dbContext.system_mergingrules.Where(x => x.Validity == "1" && x.Id == ruleID).FirstOrDefault();
                    DeleteMerging.Validity = "0";
                    DeleteMerging.Updater = CurrentUser.UserAccount;
                    DeleteMerging.UpdateTime = DateTime.Now;
                    var DeleteDeist = _dbContext.system_mergingrules.Where(x => x.Validity == "1" && x.MergingCode.Contains(DeleteMerging.MergingCode)).ToList();
                    foreach (var item in DeleteDeist)
                    {
                        item.Validity = "0";
                        item.Updater = CurrentUser.UserAccount;
                        item.UpdateTime = DateTime.Now;

                    }
                    int count = _dbContext.SaveChanges();
                    if (count != 0)
                    {
                        tran.Commit();
                        result.success = true;
                        result.message = "删除成功";
                    }
                    else
                    {
                        tran.Rollback();
                        result.success = false;
                        result.message = "删除失败";
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
        /// 被计算相似度字段
        /// </summary>
        public class Colum
        {
            public string Name { get; set; }
            public int? Weight { get; set; }
        }
        /// <summary>
        /// 合并数据  2020年5月6日15:26:20  Dennyhui
        /// </summary>
        public void MergeData(string entityID, string mergingCode)
        {
            var entity = _dbContext.system_entity.Where(e => e.Id.ToString() == entityID).FirstOrDefault();
            //获取阀值
            var Threshold_str = _dbContext.system_mergingrules.Where(m => m.MergingCode == mergingCode && m.Validity == "1").Select(m => m.SelfMotion).FirstOrDefault();
            var Threshold = Int32.Parse(Threshold_str);
            //获取MDM正式表
            string mdmtable = entity.EntityTable;
            //获取需要进行相似度比例计算的字段
            IEnumerable<Colum> columns = (from r in _dbContext.system_rulesdetails
                                          join a in _dbContext.system_attribute on r.AttributeID equals a.Id
                                          where r.MerginfCode == mergingCode && r.Validity == "1"
                                          select new Colum
                                          {
                                              Name = a.Name,
                                              Weight = r.Weight
                                          }
                         ).ToList();
            //动态拼接SQL查询语句
            StringBuilder sqlColumns = new StringBuilder();
            foreach (var column in columns)
            {
                sqlColumns.Append(column.Name + ",");
            }
            sqlColumns.Append("Code,");
            //去掉逗号
            string columnstr = DelLastComma(sqlColumns.ToString());
            string sqlQuery = "select " + columnstr + " from " + mdmtable;
            DataTable sourcetable = new DataTable();
            DataTable targettable = new DataTable();
            using (IDbConnection connection = DapperContext.Connection())
            {
                //先清空该实体的历史数据
                string delsql = "DELETE FROM system_mergingrules_similarresult WHERE EntityID=" + entityID;
                connection.Execute(delsql);
                var reader = connection.ExecuteReader(sqlQuery);
                sourcetable.Load(reader);
                //复制结构和数据
                targettable = sourcetable.Copy();
            }
            int sourcecount = sourcetable.Rows.Count;
            if (sourcecount == 0)
            {
                return;
            }
            //DataTable转成Linq To Object
            var similarDS = sourcetable.AsEnumerable();
            //每个线程分配多少条数据
            int limit = Int32.Parse(AppsettingsHelper.GetSection("DataMergeThreadLinit"));
            //定义线程池
            List<Thread> threadPool = new List<Thread>();
            //根据limit判断出需要生成多少个线程
            int fornum = Int32.Parse(Math.Ceiling((decimal)sourcecount / (decimal)limit).ToString());
            //得到当前登录用户
            string creator = CurrentUser.UserAccount;
            //循环生成线程
            for (int i = 0; i < fornum; i++)
            {
                //获取批次数据
                IEnumerable<DataRow> query = similarDS.Skip(limit * i - 1).Take(limit).ToList();
                //实例化线程
                Thread mergedata_thread = new Thread(() => mergesync(query.CopyToDataTable<DataRow>(), targettable, columns, Threshold, entityID, mergingCode, creator));
                //添加到线程池
                threadPool.Add(mergedata_thread);
            }
            bool IfRunOver = false;
            int OverThread = 0;
            //依次开启所有线程
            foreach (var item in threadPool)
            {
                item.Start();
            }
            List<string> threadid = new List<string>();
            //判断所有线程的执行情况，等待所有线程执行完毕；
            while (!IfRunOver)
            {
                foreach (var item in threadPool)
                {
                    if (!item.IsAlive)
                    {
                        if (!threadid.Any(t => t == item.ManagedThreadId.ToString()))
                        {
                            OverThread += 1;
                            threadid.Add(item.ManagedThreadId.ToString());
                        }
                    }
                    if (fornum - OverThread == 0)
                    {
                        IfRunOver = true;
                    }
                }
            }
            //对相似度计算后的结果集进行去重操作  2020年6月5日17:29:20  Dennyhui
            using (IDbConnection connection = DapperContext.Connection())
            {
                var h_para = new DynamicParameters();
                h_para.Add("@entityid", entityID);
                var _Handling_Similarity_Results = connection.Query("Handling_Similarity_Results", h_para, null, true, null, CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        /// <summary>
        /// 合并数据子线程 2020年5月27日18:24:21  Dennyhui
        /// </summary>
        /// <param name="sourcetable">需要对比的数据</param>
        /// <param name="targettable">被对比数据</param>
        /// <param name="columns">需要被计算相似度的字段列</param>
        /// <param name="Threshold">阈值</param>
        /// <param name="entityID">实体ID</param>
        /// <param name="mergingCode">合并规则Code</param>
        /// <param name="creator">执行人</param>
        public void mergesync(DataTable sourcetable, DataTable targettable, IEnumerable<Colum> columns, int Threshold, string entityID, string mergingCode, string creator)
        {
            try
            {
                MDMDBContext dbc = new MDMDBContext();
                int sourcecount = sourcetable.Rows.Count;
                int targetcount = targettable.Rows.Count;
                var similarDS = targettable.AsEnumerable();
                var sourcetableFor = sourcetable.AsEnumerable();
                foreach (var sf in sourcetableFor)
                {
                    string guid = Guid.NewGuid().ToString();
                    string code1 = sf.Field<string>("Code");
                    var similarResult = similarDS.Where(a => GetSimilarityWith(sf, a, columns) > Threshold).Select(b => new { similar = GetSimilarityWith(sf, b, columns), Code = b.Field<string>("Code") }).ToList();
                    if (similarResult.Count > 0)
                    {
                        List<system_mergingrules_similarresult> _similar_add_list = new List<system_mergingrules_similarresult>();
                        foreach (var item in similarResult)
                        {
                            system_mergingrules_similarresult m = new system_mergingrules_similarresult();
                            m.EntityID = entityID;
                            m.Code = item.Code;
                            m.MergingCode = mergingCode;
                            m.SimilarFlag = guid;
                            m.SimilarNum = item.similar.ToString();
                            m.IdentityFlag = "child";
                            m.Creator = creator;
                            m.CreateTime = DateTime.Now;
                            _similar_add_list.Add(m);
                        }
                        system_mergingrules_similarresult m2 = new system_mergingrules_similarresult();
                        m2.EntityID = entityID;
                        m2.Code = code1;
                        m2.MergingCode = mergingCode;
                        m2.SimilarFlag = guid;
                        m2.SimilarNum = "N";
                        m2.IdentityFlag = "father";
                        m2.Creator = creator;
                        m2.CreateTime = DateTime.Now;
                        _similar_add_list.Add(m2);
                        //批量插入  EFCore Extended
                        dbc.system_mergingrules_similarresult.AddRange(_similar_add_list);
                        dbc.SaveChangesAsync();
                        //批量异步保存   EFCore Extended
                        _similar_add_list.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取两个字符串的相似度
        /// </summary>
        /// <param name=”sourceString”>第一个字符串</param>
        /// <param name=”str”>第二个字符串</param>
        /// <returns></returns>
        public static decimal GetSimilarityWith(DataRow sf, DataRow tf, IEnumerable<Colum> columns)
        {
            try
            {
                string sourceString = string.Empty;
                string str = string.Empty;
                decimal Kq = 2;
                decimal Kr = 1;
                decimal Ks = 1;
                List<decimal> _Similarity_list = new List<decimal>();
                decimal _Similar_Total = 0;
                foreach (var _c_item in columns)
                {
                    sourceString = sf.Field<string>(_c_item.Name);
                    str = tf.Field<string>(_c_item.Name);
                    //两个字符串完全一样或者其中有一个为空的时候跳出本次相似度计算
                    if (String.IsNullOrWhiteSpace(sourceString) || String.IsNullOrWhiteSpace(str))
                    {
                        return 0;
                    }
                    if (sourceString.Equals(str))
                    {
                        return 0;
                    }
                    char[] ss = sourceString.ToCharArray();
                    char[] st = str.ToCharArray();
                    //获取交集数量
                    int q = ss.Intersect(st).Count();
                    int s = ss.Length - q;
                    int r = st.Length - q;
                    decimal similar = Kq * q / (Kq * q + Kr * r + Ks * s);
                    var _Similar_Result = Math.Truncate(similar * 100) * (((decimal)_c_item.Weight) / 100);
                    _Similarity_list.Add((decimal)_Similar_Result);
                }
                //根据相似度计算规则求和
                foreach (var _s_total in _Similarity_list)
                {
                    _Similar_Total += _s_total;
                }
                return _Similar_Total;
            }
            catch (Exception ex)
            {
                return 0;
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
        /// 查询合并规则
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object SearchMergingrules(int page, int limit, string where)
        {
            tableResult result = new tableResult();
            try
            {
                List<TabelMergingrules> tabels = new List<TabelMergingrules>();
                List<system_mergingrules> MergingrulesList = _dbContext.system_mergingrules.Where(x => x.Validity == "1").ToList();
                var MergingrulesLists = (from a in _dbContext.system_mergingrules
                                         join b in _dbContext.system_model
                                         on a.ModelID equals b.Id
                                         join c in _dbContext.system_entity
                                         on a.EntityID equals c.Id
                                         where a.Validity == "1"
                                         select new
                                         {
                                             a.Id,
                                             a.ModelID,
                                             a.EntityID,
                                             a.MergingCode,
                                             a.Creater,
                                             a.NoProcessing,
                                             a.SelfMotion,
                                             a.Manual,
                                             State = a.State == 0 ? "空闲" : "执行",
                                             a.CreateTime,
                                             a.Updater,
                                             a.UpdateTime,
                                             ModelName = b.Name,
                                             EntityName = c.Name

                                         }
                                       ).ToList();

                if (!string.IsNullOrWhiteSpace(where))
                {
                    MergingrulesLists = MergingrulesLists.Where(x => x.EntityID.ToString() == where).ToList();
                }
                foreach (var item in MergingrulesLists)
                {
                    TabelMergingrules tabel = new TabelMergingrules();
                    tabel.Id = item.Id;
                    tabel.ModelID = item.ModelID;
                    tabel.ModelName = item.ModelName;
                    tabel.EntityName = item.EntityName;
                    tabel.EntityID = item.EntityID;
                    tabel.Creater = item.Creater;
                    tabel.State = item.State;
                    tabel.MergingCode = item.MergingCode;
                    tabel.SelfMotion = item.SelfMotion;
                    tabel.NoProcessing = item.NoProcessing;
                    tabel.Manual = item.Manual;
                    tabel.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    tabel.Updater = item.Updater;
                    tabel.UpdateTime = item.UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    List<DetailsItem> details = new List<DetailsItem>();
                    var rulesDList = (from a in _dbContext.system_rulesdetails
                                      join b in _dbContext.system_attribute
                                      on a.AttributeID equals b.Id
                                      where a.Validity == "1" && a.MerginfCode.Contains(item.MergingCode)
                                      select new
                                      {
                                          a.IsGroop,
                                          a.Weight,
                                          b.Name,
                                          b.Id,
                                      }
                                    ).Distinct();
                    string CleaningRules = "";    //  权限文字描述
                    string IsGroop = "";  //分组文字描述
                    int IsNullIsGroop = 0; //该状态0代表这条数据没有权重规则,1为有权限规则
                    int IsGroops = 0;  //  该状态0代表这条数据没有分组规则,1为有分组规则
                    foreach (var val in rulesDList)
                    {
                        DetailsItem Details = new DetailsItem();
                        Details.IsGroop = val.IsGroop;
                        Details.ID = val.Id;
                        Details.Name = val.Name;
                        Details.weight = val.Weight;
                        details.Add(Details);
                        if (val.IsGroop == 0)
                        {
                            IsNullIsGroop = 1;
                            CleaningRules = CleaningRules + val.Name + val.Weight + "%和";
                        }
                        if (val.IsGroop == 1)
                        {
                            IsGroops = 1;
                            IsGroop = IsGroop + val.Name + "和";
                        }
                    }

                    tabel.DetailsItem = details;
                    if (IsNullIsGroop == 1)
                    {
                        CleaningRules = CleaningRules.Substring(0, CleaningRules.Length - 1);  //去掉循环后面的一个和字
                        tabel.CleaningRules = "按照" + CleaningRules + "进行权重计算;";
                    }
                    if (IsGroops == 1)
                    {
                        IsGroop = IsGroop.Substring(0, IsGroop.Length - 1);
                        tabel.CleaningRules = tabel.CleaningRules + IsGroop + "进行分组";
                    }

                    tabels.Add(tabel);
                }

                result.total = tabels.Count();
                result.data = tabels.OrderByDescending(x => x.CreateTime).Skip(limit * (page - 1)).Take(limit).ToList();
                result.success = true;


            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        #endregion

        #region 业务规则
        /// <summary>
        /// 删除字段验证
        /// </summary>
        /// <param name="dataID"></param>
        /// <returns></returns>
        public object DeleteFieldValidation(string dataID)
        {
            Result result = new Result();
            try
            {
                var DeleteValidation = _dbContext.system_fieldvalidation.Where(x => x.Validity == "1" && x.Id.ToString() == dataID).FirstOrDefault();
                DeleteValidation.Validity = "0";
                DeleteValidation.Updater = CurrentUser.UserAccount;
                DeleteValidation.UpdateTime = DateTime.Now;
                int count = _dbContext.SaveChanges();
                if (count != 0)
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
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }


        /// <summary>
        /// 查看字段验证
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object SearchFieldValidation(int page, int limit, string where)
        {
            tableResult result = new tableResult();
            try
            {
                var DataList = _dbContext.system_fieldvalidation.Where(X => X.Validity == "1");
                if (!string.IsNullOrWhiteSpace(where))
                {
                    DataList = DataList.Where(x => x.EntityID.ToString() == where);
                }
                result.total = DataList.Count();
                var Datas = DataList.OrderByDescending(x => x.CreateTime).Skip(limit * (page - 1)).Take(limit).ToList();
                result.data = Datas;
                result.success = true;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 更新字段验证
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public object UpdateFieldValidation(system_fieldvalidation data)
        {
            Result result = new Result();
            try
            {
                var ValidationIsNull = _dbContext.system_fieldvalidation.FirstOrDefault(x => x.Id == data.Id && x.Validity == "1");
                if (ValidationIsNull != null)
                {
                    ValidationIsNull.Name = data.Name;
                    ValidationIsNull.Description = data.Description;
                    ValidationIsNull.Updater = CurrentUser.UserAccount;
                    ValidationIsNull.UpdateTime = DateTime.Now;
                    _dbContext.SaveChanges();
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
        /// 添加字段验证
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public object InsertFieldValidation(system_fieldvalidation data)
        {
            Result result = new Result();
            try
            {
                var DataIsNull = _dbContext.system_fieldvalidation.Where(x => x.Name.Contains(data.Name) && x.Validity == "1");
                if (DataIsNull.Count() > 0)
                {
                    result.success = false;
                    result.message = "添加失败,名称已存在";
                    return DataIsNull;
                }
                else
                {
                    system_fieldvalidation system = new system_fieldvalidation();
                    system.ModelID = data.ModelID;
                    system.EntityID = data.EntityID;
                    system.Name = data.Name;
                    system.Description = data.Description;
                    system.Creater = CurrentUser.UserAccount;
                    system.CreateTime = DateTime.Now;
                    system.Updater = CurrentUser.UserAccount;
                    system.UpdateTime = DateTime.Now;
                    system.Validity = "1";
                    _dbContext.system_fieldvalidation.Add(system);
                    if (_dbContext.SaveChanges() != 0)
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

        /// <summary>
        /// 删除数据验证
        /// </summary>
        /// <param name="dataID"></param>
        /// <returns></returns>
        public object DeleteDataValidation(string dataID)
        {
            Result result = new Result();
            try
            {
                var DeleteValidation = _dbContext.system_datavalidation.Where(x => x.Validity == "1" && x.Id.ToString() == dataID).FirstOrDefault();
                DeleteValidation.Validity = "0";
                DeleteValidation.Updater = CurrentUser.UserAccount;
                DeleteValidation.UpdateTime = DateTime.Now;
                int count = _dbContext.SaveChanges();
                if (count != 0)
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
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 查看数据验证
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object SearchDataValidation(int page, int limit, string where)
        {
            tableResult result = new tableResult();
            try
            {
                var DataList = _dbContext.system_datavalidation.Where(X => X.Validity == "1");
                if (!string.IsNullOrWhiteSpace(where))
                {
                    DataList = DataList.Where(x => x.EntityID.ToString() == where);
                }
                result.total = DataList.Count();
                var Datas = DataList.OrderByDescending(x => x.CreateTime).Skip(limit * (page - 1)).Take(limit).ToList();

                result.data = Datas;
                result.success = true;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 更新数据验证
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public object UpdateDataValidation(system_datavalidation data)
        {
            Result result = new Result();
            try
            {
                var ValidationIsNull = _dbContext.system_datavalidation.FirstOrDefault(x => x.Id == data.Id && x.Validity == "1");
                if (ValidationIsNull != null)
                {
                    ValidationIsNull.Name = data.Name;
                    ValidationIsNull.Description = data.Description;
                    ValidationIsNull.FunctionName = data.FunctionName;
                    ValidationIsNull.Updater = CurrentUser.UserAccount;
                    ValidationIsNull.UpdateTime = DateTime.Now;
                    _dbContext.SaveChanges();
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
        /// 添加数据验证
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public object InsertDataValidation(system_datavalidation data)
        {
            Result result = new Result();
            try
            {
                var DataIsNull = _dbContext.system_datavalidation.Where(x => x.Name.Contains(data.Name) && x.Validity == "1");
                if (DataIsNull.Count() > 0)
                {
                    result.success = false;
                    result.message = "添加失败,名称已存在";
                    return DataIsNull;
                }
                else
                {
                    system_datavalidation system = new system_datavalidation();
                    system.ModelID = data.ModelID;
                    system.EntityID = data.EntityID;
                    system.Name = data.Name;
                    system.Description = data.Description;
                    system.Creater = CurrentUser.UserAccount;
                    system.CreateTime = DateTime.Now;
                    system.Updater = CurrentUser.UserAccount;
                    system.UpdateTime = DateTime.Now;
                    system.Validity = "1";
                    system.FunctionName = data.FunctionName;
                    _dbContext.system_datavalidation.Add(system);
                    if (_dbContext.SaveChanges() != 0)
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
                        createProcSql.Append("  DECLARE batchid varchar(50);");
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


        /// <summary>
        /// 编辑业务法规则
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public object EiteRuleBase(system_businessrule_attribute file)
        {
            Result result = new Result();
            try
            {
                var RuleBase = _dbContext.system_businessrule_attribute.FirstOrDefault(x => x.ID == file.ID && x.Validity == "1");
                if (RuleBase != null)
                {
                    RuleBase.AttributeID = file.AttributeID;
                    RuleBase.RuleName = file.RuleName;
                    RuleBase.Type = file.Type;
                    RuleBase.Required = file.Required;
                    RuleBase.Description = file.Description;
                    RuleBase.Expression = file.Expression;
                    RuleBase.Updater = CurrentUser.UserAccount;
                    RuleBase.UpdateTime = DateTime.Now;
                    if (_dbContext.SaveChanges() != 0)
                    {
                        AttributeRuleRelease(RuleBase.ModelID.Value, RuleBase.EntityID.Value);
                        result.success = true;
                        result.message = "更新成功";
                    }
                    else
                    {
                        result.success = false;
                        result.message = "更新失败";
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
        /// 查询业务规则
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public object SearchRuleBase(system_businessrule_attribute file)
        {
            Result result = new Result();
            try
            {
                var RuleNameIsNull = _dbContext.system_businessrule_attribute.Where(x => x.RuleName.Contains(file.RuleName) && x.Validity == "1" && x.EntityID == file.EntityID && x.ModelID == file.ModelID).ToList();
                if (RuleNameIsNull.Count() > 0)
                {
                    result.success = false;
                    result.message = "验证名称已存在";
                    return result;
                }
                else
                {
                    system_businessrule_attribute Field = new system_businessrule_attribute();
                    Field.ModelID = file.ModelID;
                    Field.EntityID = file.EntityID;
                    Field.AttributeID = file.AttributeID;
                    Field.RuleName = file.RuleName;
                    Field.Description = file.Description;
                    Field.Type = file.Type;
                    Field.Required = file.Required;
                    Field.Expression = file.Expression;
                    Field.Creater = CurrentUser.UserAccount;
                    Field.CreateTime = DateTime.Now;
                    Field.Updater = CurrentUser.UserAccount;
                    Field.UpdateTime = DateTime.Now;
                    Field.Validity = "1";
                    Field.State = "0";
                    _dbContext.system_businessrule_attribute.Add(Field);
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
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }


        /// <summary>
        /// 删除业务规则
        /// </summary>
        /// <param name="ruleID"></param>
        /// <returns></returns>
        public object DeleteRuleBase(string ruleID)
        {
            Result result = new Result();
            try
            {
                int RuleID = Convert.ToInt32(ruleID);
                var DelRuleBase = _dbContext.system_businessrule_attribute.FirstOrDefault(x => x.ID == RuleID);
                if (DelRuleBase != null)
                {
                    DelRuleBase.Validity = "0";
                    DelRuleBase.Updater = CurrentUser.UserAccount;
                    DelRuleBase.UpdateTime = DateTime.Now;
                    if (_dbContext.SaveChanges() != 0)
                    {
                        AttributeRuleRelease(DelRuleBase.ModelID.Value, DelRuleBase.EntityID.Value);
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
        /// 查询业务规则
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object SearchRuleBase(int page, int limit, string where)
        {
            tableResult result = new tableResult();
            try
            {
                var RuleBaseList = _dbContext.system_businessrule_attribute.Where(x => x.Validity == "1").ToList();
                if (!string.IsNullOrWhiteSpace(where))
                {
                    int EntityID = Convert.ToInt32(where);
                    RuleBaseList = RuleBaseList.Where(x => x.EntityID == EntityID).ToList();
                }
                result.total = RuleBaseList.Count();
                var RuleBaseData = RuleBaseList.OrderByDescending(x => x.CreateTime).Skip(limit * (page - 1)).Take(limit).Select(x => new
                {
                    x.ID,
                    x.ModelID,
                    x.EntityID,
                    x.AttributeID,
                    AttributeName = _dbContext.system_attribute.FirstOrDefault(b => b.Id == x.AttributeID) != null ? _dbContext.system_attribute.FirstOrDefault(b => b.Id == x.AttributeID).Name.ToString() : "",
                    State = x.State == "0" ? "未发布" : "已发布",
                    x.RuleName,
                    x.Description,
                    x.Type,
                    x.Required,
                    x.Creater,
                    x.Updater,
                    x.Expression,
                    CreateTime = x.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    UpdateTime = x.UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),

                }).Distinct().ToList();
                result.data = RuleBaseData;
                result.success = true;

            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }


        /// <summary>
        /// 得到属性下拉数据
        /// </summary>
        /// <returns></returns>
        public object GetAttributeSelectRule(string entityID)
        {
            Result result = new Result();
            try
            {
                int EntityID = entityID != null ? Convert.ToInt32(entityID) : 0;
                var AttributeList = _dbContext.system_attribute.Where(x => x.EntityID == EntityID).Select(x => new { x.Id, x.Name }).ToList();
                result.success = true;
                result.data = AttributeList;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 得到模型下拉数据
        /// </summary>
        /// <returns></returns>
        public object GetModelSelectRule()
        {
            Result result = new Result();
            try
            {
                var ModelList = _dbContext.system_model.Select(x => new { x.Id, x.Name }).ToList();
                result.success = true;
                result.data = ModelList;


            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 得到实体下拉数据
        /// </summary>
        /// <returns></returns>
        public object GetEntitySelectRule(string modelID)
        {
            Result result = new Result();
            try
            {
                int ModelID = modelID != null ? Convert.ToInt32(modelID) : 0;
                var EntityList = _dbContext.system_entity.Where(x => x.ModelId == ModelID).Select(x => new { x.Id, x.Name }).ToList();
                result.success = true;
                result.data = EntityList;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        #endregion
    }
}
