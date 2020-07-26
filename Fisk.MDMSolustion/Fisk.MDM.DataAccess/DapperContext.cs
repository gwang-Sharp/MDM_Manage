/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：使用dapper操作mysql                                                    
*│　作    者：Dennyhui                                             
*│　版    本：1.0                                                 
*│　创建时间：2020年4月30日09:58:49                      
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Fisk.MDM.DataAccess                               
*│　类    名： DapperContext                                      
*└──────────────────────────────────────────────────────────────┘
*/
using Fisk.MDM.Utility.Common;
using MySql.Data.MySqlClient;

namespace Fisk.MDM.DataAccess
{
    public class DapperContext
    {
        public static string conStr;
        public static string Env;
        static DapperContext()
        {
            Env = AppsettingsHelper.GetSection("Env");
            if (Env == "Uat" || Env == "Prd")
            {
                conStr = AppsettingsHelper.GetSection("ConnectionString" + Env + ":MysqlConnection");
            }
            else
            {
                conStr = AppsettingsHelper.GetSection("ConnectionStringLocalhost:MysqlConnection");
            }
        }
        //连接字符串

        public static MySqlConnection Connection()
        {
            var mysql = new MySqlConnection(conStr);
            mysql.Open();
            return mysql;
        }
    }
}
