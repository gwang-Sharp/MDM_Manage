using Fisk.MDM.DataAccess.Models;
using Fisk.MDM.Interface;
using Fisk.MDM.Utility.Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fisk.MDMSolustion.Models
{
    public class VersionHub : Hub
    {
        private readonly IMasterData_Version_Manage _masterdatamanage;
        private readonly MDMDBContext _dbContext;
        public VersionHub(IMasterData_Version_Manage MasterDataManage, MDMDBContext dbContext)
        {
            this._masterdatamanage = MasterDataManage;
            this._dbContext = dbContext;
        }
        /// <summary>
        /// 数据版本备份
        /// </summary>
        /// <param name="linkTables"></param>
        /// <param name="entityID"></param>
        /// <param name="versionForm"></param>
        /// <returns></returns>
        public async Task CreateVersion(string linkTables, int entityID, string versionForm)
        {
            JObject jObject = JObject.Parse(versionForm);
            var LinkTables = new string[] { };
            if (this._dbContext.system_version_snapshot.AsNoTracking().Where(it => it.Name == jObject["Name"].ToString() && it.EntityID == entityID).Any())
            {
                await Clients.Client(Context.ConnectionId).SendAsync("onfailed", new { success = false, msg = "存在相同版本名" });
                return;
            }
            if (string.IsNullOrEmpty(linkTables))
            {
                await Clients.Client(Context.ConnectionId).SendAsync("onshowdialog");
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("onclosedialog");
                await _masterdatamanage.CreateVersion("", entityID, jObject["Name"].ToString());
                LinkTables = linkTables.Split(",");
                for (int i = 0; i < LinkTables.Length; i++)
                {
                    var result = await _masterdatamanage.CreateVersion(LinkTables[i], entityID, jObject["Name"].ToString());
                    if (result)
                    {
                        await Clients.Client(Context.ConnectionId).SendAsync("onsuccess", new { next = i + 1 });
                    }
                }
            }
            system_version_snapshot _Snapshot = new system_version_snapshot();
            _Snapshot.EntityID = entityID;
            _Snapshot.Name = jObject["Name"].ToString();
            _Snapshot.Remark = jObject["Remark"].ToString();
            _Snapshot.CreateTime = DateTime.Now;
            _Snapshot.CreateUser = CurrentUser.UserAccount;
            _Snapshot.UpdateTime = DateTime.Now;
            _Snapshot.UpdateUser = CurrentUser.UserAccount;
            var insertVersion = this._dbContext.Add(_Snapshot);
            this._dbContext.SaveChanges();
            List<system_version_snapshot_detail> _Snapshot_Details = new List<system_version_snapshot_detail>();
            for (int i = 0; i < LinkTables.Length; i++)
            {
                system_version_snapshot_detail _Snapshot_Detail = new system_version_snapshot_detail();
                _Snapshot_Detail.VersionID = insertVersion.Entity.Id;
                _Snapshot_Detail.LinkEntityTable = LinkTables[i];
                _Snapshot_Detail.LinkEntityID = entityID;
                _Snapshot_Details.Add(_Snapshot_Detail);
            }
            if (_Snapshot_Details.Count > 0)
            {
                this._dbContext.system_version_snapshot_detail.BulkInsert(_Snapshot_Details);
                this._dbContext.BulkSaveChangesAsync();
            }
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns></returns>
        public async Task FinishConnect()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("onfinished");
        }
        /// <summary>
        /// 成功连接
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var client = Clients.Client(Context.ConnectionId);
            await client.SendAsync("onconnect", new { msg = "您已连接......" });
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
