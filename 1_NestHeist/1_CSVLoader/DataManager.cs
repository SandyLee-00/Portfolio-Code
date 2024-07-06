using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 데이터 로드해서 딕셔너리로 만들어주는 인터페이스
/// </summary>
/// <typeparam name="Key"></typeparam>
/// <typeparam name="Item"></typeparam>
public interface ILoader<Key, Item>
{
    Dictionary<Key, Item> MakeDic();
}

/// <summary>
/// 필요한 데이터 Json파일 -> 서버 / CSV / SO로 되어있는 것 전부 로드해서 갖고있기
/// 변하지 않는 값 위주로 Id : SO 로 딕셔너리로 들고있기
/// SO : Loader에 해당하는 DataLoader 붙혀놓고 리스트에 추가 -> SO 데이터를 인스펙터에서 넣어서 딕셔너리로 만들어주기 
/// CSV : Loader에 CSVLoader 컴포넌트, 모든 데이터 로드해서 딕셔너리로 만들어주기
/// </summary>
public class DataManager : Singleton<DataManager>
{
    string _persistentDataPath;

    private GameObject Loader;
    private CSVLoader _csvLoader;

    public ServerUserData User { get; private set; }
    public Dictionary<string, TextDataSO> Text { get; private set; }

    public Dictionary<string, List<DungeonEggProbabilityData>> DungeonEggProbability { get; private set; }
    public Dictionary<string, DungeonInfoData> DungeonInfo { get; private set; }
    public Dictionary<string, List<DungeonMonsterProbabilityData>> DungeonMonsterProbability { get; private set; }

    public Dictionary<string, EggInfoData> EggInfo { get; private set; }

    public Dictionary<string, ItemInfoData> ItemInfo { get; private set; }

    public Dictionary<string, MonsterStatData> MonsterBaseStat { get; private set; }
    public Dictionary<string, List<MonsterDropItemData>> MonsterDropItem { get; private set; }
    public Dictionary<string, MonsterInfoData> MonsterInfo { get; private set; }
    public Dictionary<string, MonsterStatData> MonsterIVStat { get; private set; }

    public Dictionary<string, MonsterStatData> MonsterLevelStat { get; private set; }
    public Dictionary<int, MonsterLevelUpData> MonsterLevelUp { get; private set; }
    public Dictionary<string, MonsterStatData> MonsterRankStat { get; private set; }
    public Dictionary<string, MonsterSkillInfoData> MonsterSkillInfo { get; private set; }

    public Dictionary<string, PlayerBaseStatData> PlayerBaseStat { get; private set; }
    public Dictionary<string, PlayerInfoData> PlayerInfo { get; private set; }
    public Dictionary<string, PlayerLevelStatData> PlayerLevelStat { get; private set; }
    public Dictionary<int, PlayerLevelUpData> PlayerLevelUp { get; private set; }

    // 사용하는 쪽에저 접근하기 편하게 생성하기
    public Dictionary<string, MonsterAllStatData> MonsterAllStat { get; private set; }


    // Button X / 이벤트로 실행 가능하게 하기O
    public event Action OnDataLoaded;
    public event Action OnDataSaved;

    protected override void Awake()
    {
        _isDontDestroyOnLoad = true;
        base.Awake();

        _persistentDataPath = Application.persistentDataPath;

        /*        // 각 로더에 있는 MakeDic() 호출해서 딕셔너리로 만들어주기

                Text = FindObjectOfType<TextDataLoader>().MakeDic();*/

        User = LoadJsonData<ServerUserData>();


        #region CSV 데이터 로드

        _csvLoader = FindObjectOfType<CSVLoader>();

        // 로더 없으면 프리팹 꺼내와서 씬에 만들어주기
        if (_csvLoader == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Loader");
            Loader = Instantiate(prefab);
            Loader.name = "Loader";
            _csvLoader = Loader.GetComponent<CSVLoader>();
        }

        // ----- CSV 데이터 로드 시작 -----
        DungeonEggProbability = _csvLoader.MakeDungeonEggProbabilityData();
        DungeonInfo = _csvLoader.MakeDungeonInfoData();
        DungeonMonsterProbability = _csvLoader.MakeDungeonMonsterProbabilityData();

        EggInfo = _csvLoader.MakeEggInfoData();

        ItemInfo = _csvLoader.MakeItemInfoData();

        MonsterBaseStat = _csvLoader.MakeMonsterBaseStatData();
        MonsterDropItem = _csvLoader.MakeMonsterDropItemData();
        MonsterInfo = _csvLoader.MakeMonsterInfoData();
        MonsterIVStat = _csvLoader.MakeMonsterIVStatData();

        MonsterLevelStat = _csvLoader.MakeMonsterLevelStatData();
        MonsterLevelUp = _csvLoader.MakeMonsterLevelUpData();
        MonsterRankStat = _csvLoader.MakeMonsterRankStatData();
        MonsterSkillInfo = _csvLoader.MakeMonsterSkillInfoData();

        PlayerBaseStat = _csvLoader.MakePlayerBaseStatData();
        PlayerInfo = _csvLoader.MakePlayerInfoData();
        PlayerLevelStat = _csvLoader.MakePlayerLevelStatData();
        PlayerLevelUp = _csvLoader.MakePlayerLevelUpData();

        // ----- CSV 데이터 로드 끝 -----
        #endregion


        // ----- CSV 데이터 접근 편하게 정제 -----
        MonsterAllStat = MergeMonsterAllStat();


        // 데이터 로드 완료
        OnDataLoaded?.Invoke();
    }

    /// <summary>
    /// CSV 데이터 다 불러오기 끝나고 불러온 데이터 한 번 더 정제하기
    /// MonsterAllStatData에 MonsterId 기준으로 모든 데이터 넣어주기
    /// </summary>
    /// <returns></returns>
    private Dictionary<string, MonsterAllStatData> MergeMonsterAllStat()
    {
        Dictionary<string, MonsterAllStatData> dic = new Dictionary<string, MonsterAllStatData>();

        foreach (MonsterStatData monsterdata in MonsterBaseStat.Values)
        {
            // 이 아이디로 다시 돌면서 찾아주기
            string monsterId = monsterdata.MonsterId;

            MonsterStatData monsterBaseStat = MonsterBaseStat[monsterId];
            MonsterStatData monsterIVStatData = MonsterIVStat.ContainsKey(monsterId) ? MonsterIVStat[monsterId] : null;
            MonsterStatData monsterLevelStatData = MonsterLevelStat.ContainsKey(monsterId) ? MonsterLevelStat[monsterId] : null;
            MonsterStatData monsterRankStatData = MonsterRankStat.ContainsKey(monsterId) ? MonsterRankStat[monsterId] : null;

            MonsterAllStatData monsterAllStatData = new MonsterAllStatData();
            monsterAllStatData.BaseStat = monsterBaseStat;
            monsterAllStatData.IVStat = monsterIVStatData;
            monsterAllStatData.LevelStat = monsterLevelStatData;
            monsterAllStatData.RankStat = monsterRankStatData;

            dic.Add(monsterId, monsterAllStatData);
        }
        return dic;
    }

    /// <summary>
    /// 로컬에 json파일로 저장하기 
    /// TODO : 서버 데이터 저장 요청, Button X 이벤트로 실행 가능하게 하기O
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dataClass"></param>
    public void SaveJsonData<T>(T dataClass)
    {
        string path = _persistentDataPath + $"/{typeof(T).ToString()}.json";
        File.WriteAllText(path, JsonUtility.ToJson(dataClass));
        Debug.Log($"DataManager::SaveJsonData : {path}");
    }

    /// <summary>
    /// 로컬에 저장된 json파일 불러오기
    /// 없으면 new()로 생성한다. Data클래스는 MonoBehaviour를 상속받지 않는 클래스만 사용 가능
    /// TODO : 서버 데이터 로드 요청
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T LoadJsonData<T>() where T : new()
    {
        string path = _persistentDataPath + $"/{typeof(T).ToString()}.json";
        if (!File.Exists(path))
        {
            Debug.Log($"DataManager::LoadJsonData : {typeof(T).ToString()}.json not found.");

            T constructerClassData = new T();

            return constructerClassData;
        }

        string jsonData = File.ReadAllText(path);
        Debug.Log($"DataManager::LoadJsonData : {path} loaded.");
        return JsonUtility.FromJson<T>(jsonData);
    }


}
