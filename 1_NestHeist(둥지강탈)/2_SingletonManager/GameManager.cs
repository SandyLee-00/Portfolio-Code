using UnityEngine;
using System.IO;

/// <summary>
/// 게임 플레이 중인 데이터 저장하기
/// </summary>
public class GameManager : Singleton<GameManager>
{
    public ServerUserData UserData;

    /// <summary>
    /// TODO : Awake 순서 확인하기 DataManager -> GameManager
    /// </summary>
    protected override void Awake()
    {
        _isDontDestroyOnLoad = true;
        base.Awake();

        DataManager.Instance.OnDataLoaded += Load;

        Load();
    }

    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// DataManager에서 데이터 로드 완료되면 호출
    /// 맨 처음은 Awake에서 호출
    /// </summary>
    private void Load()
    {
        UserData = DataManager.Instance.User;
        Debug.Log($"GameManager::Load() User : {UserData.Id}");
    }

}