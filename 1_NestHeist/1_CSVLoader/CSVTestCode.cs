using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CSVTestCode : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _monsterListText;
    [SerializeField] private TextMeshProUGUI _fatigueText;

    private void Start()
    {
        string monsterID = "MON00001";
        MonsterStatData monsterBaseStat = DataManager.Instance.MonsterBaseStat[monsterID];
        List<MonsterDropItemData> monsterDropItem = DataManager.Instance.MonsterDropItem[monsterID];
        MonsterInfoData monsterInfo = DataManager.Instance.MonsterInfo[monsterID];
        MonsterStatData monsterIVStat = DataManager.Instance.MonsterIVStat[monsterID];
        MonsterStatData monsterLevelStat = DataManager.Instance.MonsterLevelStat[monsterID];

        int level = 2;
        MonsterLevelUpData monsterLevelUp = DataManager.Instance.MonsterLevelUp[level];
        MonsterStatData monsterRankStat = DataManager.Instance.MonsterRankStat[monsterID];

        string skillID = "MSK00001";
        MonsterSkillInfoData monsterSkillInfo = DataManager.Instance.MonsterSkillInfo[skillID];

        string playerID = "PID00001";
        PlayerBaseStatData playerBaseStat = DataManager.Instance.PlayerBaseStat[playerID];
        PlayerInfoData playerInfo = DataManager.Instance.PlayerInfo[playerID];
        PlayerLevelStatData playerLevelStat = DataManager.Instance.PlayerLevelStat[playerID];
        PlayerLevelUpData playerLevelUp = DataManager.Instance.PlayerLevelUp[level];

        string DungeonID = "DUN00001";
        List<DungeonEggProbabilityData> dungeonEggProbability = DataManager.Instance.DungeonEggProbability[DungeonID];
        DungeonInfoData dungeonInfo = DataManager.Instance.DungeonInfo[DungeonID];
        List<DungeonMonsterProbabilityData> dungeonMonsterProbability = DataManager.Instance.DungeonMonsterProbability[DungeonID];

        string EggID = "EGG00001";
        EggInfoData eggInfo = DataManager.Instance.EggInfo[EggID];

        string ItemID = "ITE00002";
        ItemInfoData itemInfo = DataManager.Instance.ItemInfo[ItemID];

        string text = $"MonsterBaseStatData : {monsterBaseStat.MonsterId} \n" +
                     $"MonsterDropItemData : {monsterDropItem[0].MonsterId} \n" +
                     $"MonsterInfoData : {monsterInfo.Id} \n" +
                     $"MonsterIVStatData : {monsterIVStat.MonsterId} \n" +
                     $"MonsterLevelStatData : {monsterLevelStat.MonsterId} \n" +
                     $"MonsterLevelUpData : {monsterLevelUp.Level} \n" +
                     $"MonsterRankStatData : {monsterRankStat.MonsterId} \n" +
                     $"MonsterSkillInfoData : {monsterSkillInfo.Id} \n" +
                     $"PlayerBaseStatData : {playerBaseStat.PlayerId} \n" +
                     $"PlayerInfoData : {playerInfo.Id} \n" +
                     $"PlayerLevelStatData : {playerLevelStat.PlayerId} \n" +
                     $"PlayerLevelUpData : {playerLevelUp.Level} \n" +
                     $"DungeonEggProbabilityData : {dungeonEggProbability[0].DungeonId} \n" +
                     $"DungeonInfoData : {dungeonInfo.Id} \n" +
                     $"DungeonMonsterProbabilityData : {dungeonMonsterProbability[0].DungeonId} \n" +
                     $"EggInfoData : {eggInfo.Id} \n" +
                     $"ItemInfoData : {itemInfo.Id} \n";

        MonsterAllStatData monsterAllStat = DataManager.Instance.MonsterAllStat[monsterID];
        monsterAllStat.BaseStat = monsterBaseStat;

        _monsterListText.text = text;
    }
}
