using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Save
{
    /// <summary>
    /// 游戏存档
    /// </summary>
    [System.Serializable]
    public partial class GameSave
    {
        /// <summary>
        /// 获取默认的初始游戏存档
        /// </summary>
        public static GameSave OriSave()
        {
            GameSave save = new GameSave();
            save.Inventory.Items = new Dictionary<string, int>();
            save.Story.MessagePath = new Dictionary<string, EventCenter.EventArgs>();
            save.Player = new Dictionary<string, NPC_Info>();
            save.Player.Add("Shury", Resources.Load<NPC_Info>(GameManager.
                Instance.GameConfig.PathConfig.
                Paths[ObjectType.NPC_Info].PathDic["OriShury"]));
            save.Position.Scene = "初始场景";
            save.Position.Point = "最初的地点";
            return save;
        }

        /// <summary>
        /// 玩家角色存档
        /// </summary>
        public Dictionary<string, NPC_Info> Player;

        /// <summary>
        /// 故事进度存档
        /// </summary>
        public ProgressSave Story;

        /// <summary>
        /// 仓库存档
        /// </summary>
        public InventorySave Inventory;

        public Map.MapPosition Position;
    }
}
