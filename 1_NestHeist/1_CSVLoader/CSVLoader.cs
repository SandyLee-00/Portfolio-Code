using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static Define;

/// <summary>
/// 시작되면 데이터 로드해서 딕셔너리로 만들어주는 클래스
/// TODO : CSV 바로 로드 방식을 SO로 바꾸는 툴 제작하기 
/// 인스펙터에서 보기 편하게 MonoBehaviour 붙혔다. 나중에 제거하기
/// </summary>
public class CSVLoader : MonoBehaviour
{
    private string _csvPath = "CSV";

    public string DungeonEggProbabilityData = "DungeonEggProbabilityData";
    public string DungeonInfoData = "DungeonInfoData";
    public string DungeonMonsterProbabilityData = "DungeonMonsterProbabilityData";

    public string EggInfoData = "EggInfoData";

    public string ItemInfoData = "ItemInfoData";

    public string MonsterBaseStatData = "MonsterBaseStatData";
    public string MonsterDropItemData = "MonsterDropItemData";
    public string MonsterInfoData = "MonsterInfoData";
    public string MonsterIVStatData = "MonsterIVStatData";

    public string MonsterLevelStatData = "MonsterLevelStatData";
    public string MonsterLevelUpData = "MonsterLevelUpData";
    public string MonsterRankStatData = "MonsterRankStatData";
    public string MonsterSkillInfoData = "MonsterSkillInfoData";

    public string PlayerBaseStatData = "PlayerBaseStatData";
    public string PlayerInfoData = "PlayerInfoData";
    public string PlayerLevelStatData = "PlayerLevelStatData";
    public string PlayerLevelUpData = "PlayerLevelUpData";

    public Dictionary<string, List<DungeonEggProbabilityData>> MakeDungeonEggProbabilityData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{DungeonEggProbabilityData}");

        Dictionary<string, List<DungeonEggProbabilityData>> dic = new Dictionary<string, List<DungeonEggProbabilityData>>();

        for(int i = 0; i < _tempData.Count; i++)
        {
            DungeonEggProbabilityData data = new DungeonEggProbabilityData();

            data.DungeonId = _tempData[i]["DungeonId"].ToString();
            data.EggId = _tempData[i]["EggId"].ToString();
            data.Probability = float.Parse(_tempData[i]["Probability"].ToString());

            if(dic.ContainsKey(data.DungeonId))
            {
                dic[data.DungeonId].Add(data);
            }
            else
            {
                dic.Add(data.DungeonId, new List<DungeonEggProbabilityData> { data });
            }
        }

        return dic;
    }

    public Dictionary<string, DungeonInfoData> MakeDungeonInfoData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{DungeonInfoData}");

        Dictionary<string, DungeonInfoData> dic = new Dictionary<string, DungeonInfoData>();

        for (int i = 0; i < _tempData.Count; i++)
        {
            DungeonInfoData data = new DungeonInfoData();

            data.Id = _tempData[i]["Id"].ToString();
            data.EnvironmentType = (EnvironmentType)Enum.Parse(typeof(EnvironmentType), _tempData[i]["EnvironmentType"].ToString());
            data.Stage = int.Parse(_tempData[i]["Stage"].ToString());
            data.Fatigue = int.Parse(_tempData[i]["Fatigue"].ToString());
            data.MinLevel = int.Parse(_tempData[i]["MinLevel"].ToString());
            data.MaxLevel = int.Parse(_tempData[i]["MaxLevel"].ToString());
            data.DungeonWidth = int.Parse(_tempData[i]["DungeonWidth"].ToString());
            data.DungeonHeight = int.Parse(_tempData[i]["DungeonHeight"].ToString());
            data.ClearExp = int.Parse(_tempData[i]["ClearExp"].ToString());
            data.MonsterSpawnNum = int.Parse(_tempData[i]["MonsterSpawnNum"].ToString());

            dic.Add(data.Id, data);
        }

        return dic;
    }

    public Dictionary<string, List<DungeonMonsterProbabilityData>> MakeDungeonMonsterProbabilityData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{DungeonMonsterProbabilityData}");

        Dictionary<string, List<DungeonMonsterProbabilityData>> dic = new Dictionary<string, List<DungeonMonsterProbabilityData>>();

        for (int i = 0; i < _tempData.Count; i++)
        {
            DungeonMonsterProbabilityData data = new DungeonMonsterProbabilityData();

            data.DungeonId = _tempData[i]["DungeonId"].ToString();
            data.MonsterId = _tempData[i]["MonsterId"].ToString();
            data.Probability = float.Parse(_tempData[i]["Probability"].ToString());

            if (dic.ContainsKey(data.DungeonId))
            {
                dic[data.DungeonId].Add(data);
            }
            else
            {
                dic.Add(data.DungeonId, new List<DungeonMonsterProbabilityData> { data });
            }
        }

        return dic;
    }

    public Dictionary<string, EggInfoData> MakeEggInfoData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{EggInfoData}");

        Dictionary<string, EggInfoData> dic = new Dictionary<string, EggInfoData>();

        for (int i = 0; i < _tempData.Count; i++)
        {
            EggInfoData data = new EggInfoData();

            data.Id = _tempData[i]["Id"].ToString();
            data.Name = _tempData[i]["Name"].ToString();
            data.MonsterId = _tempData[i]["MonsterId"].ToString();
            data.EnvironmentType = (EnvironmentType)Enum.Parse(typeof(EnvironmentType), _tempData[i]["EnvironmentType"].ToString());
            data.HatchTime = int.Parse(_tempData[i]["HatchTime"].ToString());
            data.NeedGauge = int.Parse(_tempData[i]["NeedGauge"].ToString());

            dic.Add(data.Id, data);
        }

        return dic;
    }

    public Dictionary<string, ItemInfoData> MakeItemInfoData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{ItemInfoData}");
            
        Dictionary<string, ItemInfoData> dic = new Dictionary<string, ItemInfoData>();

        for (int i = 0; i < _tempData.Count; i++)
        {
            ItemInfoData data = new ItemInfoData();

            data.Id = _tempData[i]["Id"].ToString();
            data.Name = _tempData[i]["Name"].ToString();
            data.Desc = _tempData[i]["Desc"].ToString();
            data.Type = (ItemType)Enum.Parse(typeof(ItemType), _tempData[i]["Type"].ToString());
            data.Value = int.Parse(_tempData[i]["Value"].ToString());

            dic.Add(data.Id, data);
        }

        return dic;
    }

    public Dictionary<string, MonsterStatData> MakeMonsterBaseStatData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{MonsterBaseStatData}");

        Dictionary<string, MonsterStatData> dic = new Dictionary<string, MonsterStatData>();

        for (int i = 0; i < _tempData.Count; i++)
        {
            MonsterStatData data = new MonsterStatData();

            data.MonsterId = _tempData[i]["MonsterId"].ToString();
            data.Attack = int.Parse(_tempData[i]["Attack"].ToString());
            data.MaxHP = int.Parse(_tempData[i]["MaxHP"].ToString());
            data.Defence = int.Parse(_tempData[i]["Defence"].ToString());
            data.Avoid = float.Parse(_tempData[i]["Avoid"].ToString());
            data.Critical = float.Parse(_tempData[i]["Critical"].ToString());
            data.CriticalDamage = int.Parse(_tempData[i]["CriticalDamage"].ToString());
            data.WalkSpeed = float.Parse(_tempData[i]["WalkSpeed"].ToString());

            dic.Add(data.MonsterId, data);
        }
        return dic;
    }

    public Dictionary<string, List<MonsterDropItemData>> MakeMonsterDropItemData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{MonsterDropItemData}");

        Dictionary<string, List<MonsterDropItemData>> dic = new Dictionary<string, List<MonsterDropItemData>>();

        for(int i = 0; i < _tempData.Count; i++)
        {
            MonsterDropItemData data = new MonsterDropItemData();

            data.MonsterId = _tempData[i]["MonsterId"].ToString();
            data.ItemId = _tempData[i]["ItemId"].ToString();
            data.Probability = float.Parse(_tempData[i]["Probability"].ToString());

            // 그냥 작업하다가 MinMax int를 안 넣었다. 그리고 디버깅하면서 안나온다고 했다...ㅎ
            data.MinNum = int.Parse(_tempData[i]["MinNum"].ToString());
            data.MaxNum = int.Parse(_tempData[i]["MaxNum"].ToString());


            if(dic.ContainsKey(data.MonsterId))
            {
                dic[data.MonsterId].Add(data);
            }
            else
            {
                dic.Add(data.MonsterId, new List<MonsterDropItemData> { data });
            }
        }

        return dic;
    }

    public Dictionary<string, MonsterInfoData> MakeMonsterInfoData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{MonsterInfoData}");

        Dictionary<string, MonsterInfoData> dic = new Dictionary<string, MonsterInfoData>();

        for (int i = 0; i < _tempData.Count; i++)
        {
            MonsterInfoData data = new MonsterInfoData();

            data.Id = _tempData[i]["Id"].ToString();
            data.Name = _tempData[i]["Name"].ToString();
            data.Desc = _tempData[i]["Desc"].ToString();
            data.EnvironmentType = (EnvironmentType)Enum.Parse(typeof(EnvironmentType), _tempData[i]["EnvironmentType"].ToString());
            data.AttackType = (AttackType)Enum.Parse(typeof(AttackType), _tempData[i]["AttackType"].ToString());

            dic.Add(data.Id, data);
        }   
        return dic;
    }

    public Dictionary<string, MonsterStatData> MakeMonsterIVStatData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{MonsterIVStatData}");

        Dictionary<string, MonsterStatData> dic = new Dictionary<string, MonsterStatData>();

        for (int i = 0; i < _tempData.Count; i++)
        {
            MonsterStatData data = new MonsterStatData();

            data.MonsterId = _tempData[i]["MonsterId"].ToString();
            data.Attack = int.Parse(_tempData[i]["Attack"].ToString());
            data.MaxHP = int.Parse(_tempData[i]["MaxHP"].ToString());
            data.Defence = int.Parse(_tempData[i]["Defence"].ToString());
            data.Avoid = float.Parse(_tempData[i]["Avoid"].ToString());
            data.Critical = float.Parse(_tempData[i]["Critical"].ToString());
            data.CriticalDamage = int.Parse(_tempData[i]["CriticalDamage"].ToString());
            data.WalkSpeed = float.Parse(_tempData[i]["WalkSpeed"].ToString());

            dic.Add(data.MonsterId, data);
        }
        return dic;
    }

    public Dictionary<string, MonsterStatData> MakeMonsterLevelStatData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{MonsterLevelStatData}");

        Dictionary<string, MonsterStatData> dic = new Dictionary<string, MonsterStatData>();

        for (int i = 0; i < _tempData.Count; i++)
        {
            MonsterStatData data = new MonsterStatData();

            data.MonsterId = _tempData[i]["MonsterId"].ToString();
            data.Attack = int.Parse(_tempData[i]["Attack"].ToString());
            data.MaxHP = int.Parse(_tempData[i]["MaxHP"].ToString());
            data.Defence = int.Parse(_tempData[i]["Defence"].ToString());
            data.Avoid = float.Parse(_tempData[i]["Avoid"].ToString());
            data.Critical = float.Parse(_tempData[i]["Critical"].ToString());
            data.CriticalDamage = int.Parse(_tempData[i]["CriticalDamage"].ToString());
            data.WalkSpeed = float.Parse(_tempData[i]["WalkSpeed"].ToString());

            dic.Add(data.MonsterId, data);
        }
        return dic;
    }

    public Dictionary<int, MonsterLevelUpData> MakeMonsterLevelUpData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{MonsterLevelUpData}");

        Dictionary<int, MonsterLevelUpData> dic = new Dictionary<int, MonsterLevelUpData>();

        for (int i = 0; i < _tempData.Count; i++)
        {
            MonsterLevelUpData data = new MonsterLevelUpData();

            data.Level = int.Parse(_tempData[i]["Level"].ToString());
            data.SumOfExp = int.Parse(_tempData[i]["SumOfExp"].ToString());

            dic.Add(data.Level, data);
        }
        return dic;
    }

    public Dictionary<string, MonsterStatData> MakeMonsterRankStatData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{MonsterRankStatData}");

        Dictionary<string, MonsterStatData> dic = new Dictionary<string, MonsterStatData>();

        for (int i = 0; i < _tempData.Count; i++)
        {
            MonsterStatData data = new MonsterStatData();

            data.MonsterId = _tempData[i]["MonsterId"].ToString();
            data.Attack = int.Parse(_tempData[i]["Attack"].ToString());
            data.MaxHP = int.Parse(_tempData[i]["MaxHP"].ToString());
            data.Defence = int.Parse(_tempData[i]["Defence"].ToString());
            data.Avoid = float.Parse(_tempData[i]["Avoid"].ToString());
            data.Critical = float.Parse(_tempData[i]["Critical"].ToString());
            data.CriticalDamage = int.Parse(_tempData[i]["CriticalDamage"].ToString());
            data.WalkSpeed = float.Parse(_tempData[i]["WalkSpeed"].ToString());

            dic.Add(data.MonsterId, data);
        }
        return dic;
    }

    public Dictionary<string, MonsterSkillInfoData> MakeMonsterSkillInfoData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{MonsterSkillInfoData}");

        Dictionary<string, MonsterSkillInfoData> dic = new Dictionary<string, MonsterSkillInfoData>();

        for (int i = 0; i < _tempData.Count; i++)
        {
            MonsterSkillInfoData data = new MonsterSkillInfoData();

            data.Id = _tempData[i]["Id"].ToString();
            data.Name = _tempData[i]["Name"].ToString();
            data.Desc = _tempData[i]["Desc"].ToString();
            data.Range = float.Parse(_tempData[i]["Range"].ToString());
            data.CoolDown = float.Parse(_tempData[i]["CoolDown"].ToString());
            data.CastTime = float.Parse(_tempData[i]["CastTime"].ToString());

            dic.Add(data.Id, data);
        }
        return dic;
    }

    public Dictionary<string, PlayerBaseStatData> MakePlayerBaseStatData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{PlayerBaseStatData}");

        Dictionary<string, PlayerBaseStatData> dic = new Dictionary<string, PlayerBaseStatData>();

        for(int i = 0; i < _tempData.Count; i++)
        {
            PlayerBaseStatData data = new PlayerBaseStatData();

            data.PlayerId = _tempData[i]["PlayerId"].ToString();
            data.MaxHP = int.Parse(_tempData[i]["MaxHP"].ToString());
            data.Avoid = float.Parse(_tempData[i]["Avoid"].ToString());
            data.WalkSpeed = float.Parse(_tempData[i]["WalkSpeed"].ToString());

            dic.Add(data.PlayerId, data);
        }
        return dic;
    }

    /// <summary>
    /// TODO : 플레이어 캐릭터 추가되면 몬스터 Info처럼 캐릭터 이름, 설명 추가하기 
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, PlayerInfoData> MakePlayerInfoData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{PlayerInfoData}");

        Dictionary<string, PlayerInfoData> dic = new Dictionary<string, PlayerInfoData>();

        for(int i = 0; i < _tempData.Count; i++)
        {
            PlayerInfoData data = new PlayerInfoData();

            data.Id = _tempData[i]["Id"].ToString();

            dic.Add(data.Id, data);
        }

        return dic;
    }

    public Dictionary<string, PlayerLevelStatData> MakePlayerLevelStatData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{PlayerLevelStatData}");

        Dictionary<string, PlayerLevelStatData> dic = new Dictionary<string, PlayerLevelStatData>();

        for(int i = 0; i < _tempData.Count; i++)
        {
            PlayerLevelStatData data = new PlayerLevelStatData();

            data.PlayerId = _tempData[i]["PlayerId"].ToString();
            data.MaxHP = int.Parse(_tempData[i]["MaxHP"].ToString());
            data.Avoid = float.Parse(_tempData[i]["Avoid"].ToString());
            data.WalkSpeed = float.Parse(_tempData[i]["WalkSpeed"].ToString());

            dic.Add(data.PlayerId, data);
        }
        return dic;
    }

    public Dictionary<int, PlayerLevelUpData> MakePlayerLevelUpData()
    {
        List<Dictionary<string, object>> _tempData = CSVReader.Read(_csvPath + $"/{PlayerLevelUpData}");

        Dictionary<int, PlayerLevelUpData> dic = new Dictionary<int, PlayerLevelUpData>();

        for(int i = 0; i < _tempData.Count; i++)
        {
            PlayerLevelUpData data = new PlayerLevelUpData();

            data.Level = int.Parse(_tempData[i]["Level"].ToString());
            data.SumOfExp = int.Parse(_tempData[i]["SumOfExp"].ToString());

            dic.Add(data.Level, data);
        }
        return dic;
    }
}
