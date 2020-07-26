using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class MDMDBContext : DbContext
    {
        public MDMDBContext()
        {
        }

        public MDMDBContext(DbContextOptions<MDMDBContext> options)
            : base(options)
        {
            this.Database.SetCommandTimeout(1800);
        }

        public virtual DbSet<similarityaddress> similarityaddress { get; set; }
        public virtual DbSet<system_attribute> system_attribute { get; set; }
        public virtual DbSet<system_businessrule_attribute> system_businessrule_attribute { get; set; }
        public virtual DbSet<system_datamaintenance> system_datamaintenance { get; set; }
        public virtual DbSet<system_datavalidation> system_datavalidation { get; set; }
        public virtual DbSet<system_entity> system_entity { get; set; }
        public virtual DbSet<system_entitydatchlogs> system_entitydatchlogs { get; set; }
        public virtual DbSet<system_fieldvalidation> system_fieldvalidation { get; set; }
        public virtual DbSet<system_globalexception_log> system_globalexception_log { get; set; }
        public virtual DbSet<system_log> system_log { get; set; }
        public virtual DbSet<system_mergingrules> system_mergingrules { get; set; }
        public virtual DbSet<system_mergingrules_similarresult> system_mergingrules_similarresult { get; set; }
        public virtual DbSet<system_model> system_model { get; set; }
        public virtual DbSet<system_navigation> system_navigation { get; set; }
        public virtual DbSet<system_role> system_role { get; set; }
        public virtual DbSet<system_rolenavassignment> system_rolenavassignment { get; set; }
        public virtual DbSet<system_rulesdetails> system_rulesdetails { get; set; }
        public virtual DbSet<system_subscription> system_subscription { get; set; }
        public virtual DbSet<system_user> system_user { get; set; }
        public virtual DbSet<system_userroleassignment> system_userroleassignment { get; set; }
        public virtual DbSet<system_version_snapshot> system_version_snapshot { get; set; }
        public virtual DbSet<system_version_snapshot_detail> system_version_snapshot_detail { get; set; }
        public virtual DbSet<system_version_zipper> system_version_zipper { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=192.168.1.20;user id=mdmroot;password=password01;database=mdmdb", x => x.ServerVersion("8.0.16-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<similarityaddress>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.address)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.code)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<system_attribute>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("自增主键");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.Creater)
                    .HasColumnType("varchar(50)")
                    .HasComment("创建者")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.DataType)
                    .HasColumnType("varchar(50)")
                    .HasComment("数据类型")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.DisplayName)
                    .HasColumnType("varchar(50)")
                    .HasComment("前台文字段名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.EntityID)
                    .HasColumnType("int(11)")
                    .HasComment("实体ID");

                entity.Property(e => e.IsDefault)
                    .HasColumnType("int(11)")
                    .HasComment("是否为默认属性");

                entity.Property(e => e.IsDisplay)
                    .HasColumnType("int(11)")
                    .HasComment("是否显示");

                entity.Property(e => e.IsSystem)
                    .HasColumnType("int(11)")
                    .HasComment("是否系统字段");

                entity.Property(e => e.LinkEntityID)
                    .HasColumnType("int(11)")
                    .HasComment("基于域的实体ID");

                entity.Property(e => e.LinkModelID)
                    .HasColumnType("int(11)")
                    .HasComment("基于域的模型ID");

                entity.Property(e => e.Name)
                    .HasColumnType("varchar(50)")
                    .HasComment("后台字段名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Remark)
                    .HasColumnType("varchar(50)")
                    .HasComment("说明")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.StartTrace)
                    .HasColumnType("varchar(50)")
                    .HasComment("是否队成员进行启动跟踪")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Type)
                    .HasColumnType("varchar(50)")
                    .HasComment("属性类型")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.TypeLength)
                    .HasColumnType("int(11)")
                    .HasComment("属性长度");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("修改时间");

                entity.Property(e => e.Updater)
                    .HasColumnType("varchar(50)")
                    .HasComment("修改者")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<system_businessrule_attribute>(entity =>
            {
                entity.Property(e => e.ID)
                    .HasColumnType("int(11)")
                    .HasComment("自增主键");

                entity.Property(e => e.AttributeID)
                    .HasColumnType("int(11)")
                    .HasComment("属性Id");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.Creater)
                    .HasColumnType("varchar(255)")
                    .HasComment("创建者")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Description)
                    .HasColumnType("varchar(255)")
                    .HasComment("规则名称描述")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.EntityID)
                    .HasColumnType("int(11)")
                    .HasComment("实体ID");

                entity.Property(e => e.Expression)
                    .HasColumnType("varchar(500)")
                    .HasComment("正则表达式")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ModelID)
                    .HasColumnType("int(11)")
                    .HasComment("模型Id");

                entity.Property(e => e.Required)
                    .HasColumnType("varchar(255)")
                    .HasComment("必填验证")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RuleName)
                    .HasColumnType("varchar(255)")
                    .HasComment("规则名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.State)
                    .HasColumnType("varchar(255)")
                    .HasComment("状态")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Type)
                    .HasColumnType("varchar(255)")
                    .HasComment("类型")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("更新时间");

                entity.Property(e => e.Updater)
                    .HasColumnType("varchar(255)")
                    .HasComment("更新者")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Validity)
                    .HasColumnType("varchar(255)")
                    .HasComment("逻辑删除")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<system_datamaintenance>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("自增主键");

                entity.Property(e => e.Apiaddress)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasComment("API地址")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.ApplyTitle)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.AttributeID)
                    .HasColumnType("varchar(255)")
                    .HasComment("属性ID")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.BpmName)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.CreateUser)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasComment("创建人")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.EntityID)
                    .HasColumnType("int(11)")
                    .HasComment("实体ID");

                entity.Property(e => e.MergeAPI)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.RuleName)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasComment("逻辑名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.RuleRemark)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasComment("逻辑说明")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.RuleType)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasComment("逻辑类型")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("更新时间");

                entity.Property(e => e.UpdateUser)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasComment("更新人")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<system_datavalidation>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("Id 自增主键");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.Creater)
                    .HasColumnType("varchar(255)")
                    .HasComment("创建者")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Description)
                    .HasColumnType("varchar(255)")
                    .HasComment("描述")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.EntityID)
                    .HasColumnType("int(11)")
                    .HasComment("实体id");

                entity.Property(e => e.FunctionName)
                    .HasColumnType("varchar(255)")
                    .HasComment("存储过程名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ModelID)
                    .HasColumnType("int(11)")
                    .HasComment("模型ID");

                entity.Property(e => e.Name)
                    .HasColumnType("varchar(255)")
                    .HasComment("名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("更新时间");

                entity.Property(e => e.Updater)
                    .HasColumnType("varchar(255)")
                    .HasComment("更新者 ")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Validity)
                    .HasColumnType("varchar(255)")
                    .HasComment("逻辑删除")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<system_entity>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("自增主键");

                entity.Property(e => e.AutoCreateCode)
                    .HasColumnType("tinyint(4)")
                    .HasComment("是否自动创建代码");

                entity.Property(e => e.BeganIn)
                    .HasColumnType("int(11)")
                    .HasComment("开始于");

                entity.Property(e => e.BusinessProc)
                    .HasColumnType("varchar(50)")
                    .HasComment("业务规则存储过程")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.Creater)
                    .HasColumnType("varchar(50)")
                    .HasComment("创建者")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.DataImportProc)
                    .HasColumnType("varchar(50)")
                    .HasComment("数据导入存储过程")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.EntityTable)
                    .HasColumnType("varchar(50)")
                    .HasComment("生成的mdm实体表名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.HistoryTable)
                    .HasColumnType("varchar(50)")
                    .HasComment("生成的history实体表名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ModelId)
                    .HasColumnType("int(11)")
                    .HasComment("模型ID");

                entity.Property(e => e.Name)
                    .HasColumnType("varchar(50)")
                    .HasComment("实体名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Remark)
                    .HasColumnType("varchar(50)")
                    .HasComment("说明")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.StageTable)
                    .HasColumnType("varchar(50)")
                    .HasComment("临时表   stg.staff")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("修改时间");

                entity.Property(e => e.Updater)
                    .HasColumnType("varchar(50)")
                    .HasComment("修改者")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ValiditeProc)
                    .HasColumnType("varchar(50)")
                    .HasComment("普通验证存储过程")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<system_entitydatchlogs>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("自增主键");

                entity.Property(e => e.BatchID)
                    .IsRequired()
                    .HasColumnType("varchar(55)")
                    .HasComment("批次ID")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnType("varchar(55)")
                    .HasComment("编号")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("操作时间");

                entity.Property(e => e.CreateUser)
                    .IsRequired()
                    .HasColumnType("varchar(55)")
                    .HasComment("操作人")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.EntityID)
                    .HasColumnType("int(11)")
                    .HasComment("实体ID");

                entity.Property(e => e.ErrorMessages)
                    .IsRequired()
                    .HasColumnType("varchar(400)")
                    .HasComment("错误描述")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnType("varchar(55)")
                    .HasComment("状态")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("修改时间");

                entity.Property(e => e.UpdateUser)
                    .IsRequired()
                    .HasColumnType("varchar(55)")
                    .HasComment("修改人")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<system_fieldvalidation>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("ID自增主键");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.Creater)
                    .HasColumnType("varchar(255)")
                    .HasComment("创建者")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Description)
                    .HasColumnType("varchar(255)")
                    .HasComment("描述")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.EntityID)
                    .HasColumnType("int(11)")
                    .HasComment("实体id");

                entity.Property(e => e.ModelID)
                    .HasColumnType("int(11)")
                    .HasComment("模型id");

                entity.Property(e => e.Name)
                    .HasColumnType("varchar(255)")
                    .HasComment("名字")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("更新时间");

                entity.Property(e => e.Updater)
                    .HasColumnType("varchar(255)")
                    .HasComment("更新者")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Validity)
                    .HasColumnType("varchar(255)")
                    .HasComment("逻辑删除")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<system_globalexception_log>(entity =>
            {
                entity.Property(e => e.ID)
                    .HasColumnType("varchar(225)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Action)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Controller)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Creater)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.ErrorMsg)
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.IP)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<system_log>(entity =>
            {
                entity.Property(e => e.ID)
                    .HasColumnType("varchar(50)")
                    .HasComment("主键")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ActionName)
                    .HasColumnType("varchar(50)")
                    .HasComment("方法名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ControllerName)
                    .HasColumnType("varchar(50)")
                    .HasComment("控制器名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("操作时间");

                entity.Property(e => e.OperateResult)
                    .HasColumnType("text")
                    .HasComment("返回结果")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Parameters)
                    .HasColumnType("text")
                    .HasComment("参数")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserAccount)
                    .HasColumnType("varchar(50)")
                    .HasComment("用户名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserHostAddress)
                    .HasColumnType("varchar(50)")
                    .HasComment("访问者IP")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<system_mergingrules>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Creater)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.EntityID).HasColumnType("int(11)");

                entity.Property(e => e.IsGroop).HasColumnType("int(11)");

                entity.Property(e => e.Manual)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.MergingCode)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ModelID).HasColumnType("int(11)");

                entity.Property(e => e.NoProcessing)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SelfMotion)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.State).HasColumnType("int(11)");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.Property(e => e.Updater)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Validity)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<system_mergingrules_similarresult>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("主键");

                entity.Property(e => e.Code)
                    .HasColumnType("varchar(255)")
                    .HasComment("MDM表的Code")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Creator)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.EntityID)
                    .HasColumnType("varchar(255)")
                    .HasComment("实体ID")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.IdentityFlag)
                    .HasColumnType("varchar(255)")
                    .HasComment("身份标识")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.MergingCode)
                    .HasColumnType("varchar(255)")
                    .HasComment("合并规则code")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.SimilarFlag)
                    .HasColumnType("varchar(255)")
                    .HasComment("相似标识度")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.SimilarNum)
                    .HasColumnType("varchar(255)")
                    .HasComment("相似度值")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<system_model>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("自增主键");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.Creater)
                    .HasColumnType("varchar(50)")
                    .HasComment("创建者")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.LogRetentionDays)
                    .HasColumnType("int(11)")
                    .HasComment("日志保留天数");

                entity.Property(e => e.Name)
                    .HasColumnType("varchar(50)")
                    .HasComment("模型名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Remark)
                    .HasColumnType("varchar(50)")
                    .HasComment("说明")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("修改时间");

                entity.Property(e => e.Updater)
                    .HasColumnType("varchar(50)")
                    .HasComment("修改者")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<system_navigation>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("自增主键");

                entity.Property(e => e.ConfirmDelete)
                    .HasColumnType("int(11)")
                    .HasComment("能否删除");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.Creater)
                    .HasColumnType("varchar(255)")
                    .HasComment("创建者")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Grade)
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasComment("菜单层级")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.NavCode)
                    .HasColumnType("varchar(255)")
                    .HasComment("菜单code")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.NavDesc)
                    .HasColumnType("varchar(255)")
                    .HasComment("菜单描述")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.NavName)
                    .HasColumnType("varchar(255)")
                    .HasComment("菜单名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.NavURL)
                    .HasColumnType("text")
                    .HasComment("菜单路径")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ParentNavCode)
                    .HasColumnType("varchar(255)")
                    .HasComment("父级菜单code")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SequenceNo)
                    .HasColumnType("int(11)")
                    .HasComment("菜单排序");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("更新时间");

                entity.Property(e => e.Updater)
                    .HasColumnType("varchar(255)")
                    .HasComment("更新者")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<system_role>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("角色Id");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.Creater)
                    .HasColumnType("varchar(255)")
                    .HasComment("创建人")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RoleDesc)
                    .HasColumnType("varchar(255)")
                    .HasComment("角色描述")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RoleName)
                    .HasColumnType("varchar(255)")
                    .HasComment("角色名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("更新时间");

                entity.Property(e => e.Updater)
                    .HasColumnType("varchar(255)")
                    .HasComment("更新者")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Validity)
                    .HasColumnType("varchar(255)")
                    .HasComment("逻辑删除")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<system_rolenavassignment>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("自增id");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("开始时间");

                entity.Property(e => e.Creater)
                    .HasColumnType("varchar(255)")
                    .HasComment("创建者")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.NavCode)
                    .HasColumnType("varchar(255)")
                    .HasComment("菜单code")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RoleID)
                    .HasColumnType("varchar(255)")
                    .HasComment("角色id")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.Updater)
                    .HasColumnType("varchar(255)")
                    .HasComment("创建者")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Validity)
                    .HasColumnType("varchar(255)")
                    .HasComment("逻辑删除")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<system_rulesdetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AttributeID).HasColumnType("int(11)");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Creater)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IsGroop).HasColumnType("int(11)");

                entity.Property(e => e.MerginfCode)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.Property(e => e.Updater)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Validity)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Weight).HasColumnType("int(11)");
            });

            modelBuilder.Entity<system_subscription>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("自增主键");

                entity.Property(e => e.Apiaddress)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasComment("API地址")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.AttributeID)
                    .IsRequired()
                    .HasColumnType("varchar(55)")
                    .HasComment("属性ID")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.CreateUser)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasComment("创建人")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.EntityID)
                    .HasColumnType("int(11)")
                    .HasComment("实体ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasComment("订阅名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.SubscriptionRemark)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasComment("订阅说明")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("修改时间");

                entity.Property(e => e.UpdateUser)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasComment("修改人")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<system_user>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("自增id");

                entity.Property(e => e.ADID)
                    .HasColumnType("varchar(50)")
                    .HasComment("域ID")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.Creater)
                    .HasColumnType("varchar(50)")
                    .HasComment("创建人")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Email)
                    .HasColumnType("varchar(50)")
                    .HasComment("用户邮箱")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Icon)
                    .HasColumnType("varchar(50)")
                    .HasComment("用户头像")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Salt)
                    .HasColumnType("varchar(50)")
                    .HasComment("盐值")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Type)
                    .HasColumnType("varchar(50)")
                    .HasComment("用户类型")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("更新时间");

                entity.Property(e => e.Updater)
                    .HasColumnType("varchar(50)")
                    .HasComment("更新人")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserAccount)
                    .HasColumnType("varchar(50)")
                    .HasComment("用户账号")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserName)
                    .HasColumnType("varchar(50)")
                    .HasComment("用户名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserPwd)
                    .HasColumnType("varchar(50)")
                    .HasComment("用户密码")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Validity)
                    .HasColumnType("varchar(50)")
                    .HasComment("有效性")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<system_userroleassignment>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("自增id");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.Creater)
                    .HasColumnType("varchar(255)")
                    .HasComment("创建人")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RoleID)
                    .HasColumnType("int(255)")
                    .HasComment("角色id");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("更新时间");

                entity.Property(e => e.Updater)
                    .HasColumnType("varchar(255)")
                    .HasComment("更新人")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserID)
                    .HasColumnType("int(255)")
                    .HasComment("用户id");

                entity.Property(e => e.Validity)
                    .HasColumnType("varchar(255)")
                    .HasComment("逻辑删除")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<system_version_snapshot>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("自增主键");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.CreateUser)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasComment("创建人")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.EntityID)
                    .HasColumnType("int(11)")
                    .HasComment("实体ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasComment("版本名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Remark)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasComment("版本说明")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("修改时间");

                entity.Property(e => e.UpdateUser)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasComment("修改人")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<system_version_snapshot_detail>(entity =>
            {
                entity.Property(e => e.id)
                    .HasColumnType("int(11)")
                    .HasComment("自增主键");

                entity.Property(e => e.LinkEntityID)
                    .HasColumnType("int(11)")
                    .HasComment("操作实体ID");

                entity.Property(e => e.LinkEntityTable)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasComment("涉及表名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.VersionID)
                    .HasColumnType("int(11)")
                    .HasComment("版本快照自增主键ID");
            });

            modelBuilder.Entity<system_version_zipper>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AttributeID)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.AttributeName)
                    .IsRequired()
                    .HasColumnType("varchar(55)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Creator)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.EntityID).HasColumnType("int(11)");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
