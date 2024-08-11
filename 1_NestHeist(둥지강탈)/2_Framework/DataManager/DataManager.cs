using System;

/// <summary>
/// CSV : Loader에 CSVLoader 컴포넌트, 모든 데이터 로드해서 딕셔너리로 만들어주기
/// </summary>
public class DataManager : Singleton<DataManager>
{
    public DefaultDataManager BaseData { get; private set; }
    public ServerDataManager ServerData { get; private set; }
    public LocalDataSystem LocalData { get; private set; }

    /*public event Action OnDataLoaded;
    public event Action OnDataSaved;*/

    protected override void Awake()
    {
        _isDontDestroyOnLoad = true;
        base.Awake();

        BaseData = new DefaultDataManager();
        ServerData = new ServerDataManager();
        LocalData = new LocalDataSystem();

        BaseData.Init();
        ServerData.Init();
        LocalData.Init();

        /*OnDataLoaded?.Invoke();*/
    }
}
