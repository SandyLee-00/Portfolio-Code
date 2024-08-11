using System;
using System.IO;
using UnityEngine;

public class LocalDataSystem
{
    string _persistentDataPath;

    public LocalSoundSettingData Sound { get; private set; }

    public void Init()
    {
        _persistentDataPath = Application.persistentDataPath;

        InitData();
    }

    private void InitData()
    {
        Sound = LoadJsonData<LocalSoundSettingData>();

        EventManager.Instance.Invoke(EventType.OnSetVolume, this, Sound);
    }
    
    /// <summary>
    /// 로컬에 json파일로 저장하기 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dataClass"></param>
    public void SaveJsonData<T>(T dataClass)
    {
        string path = _persistentDataPath + $"/{typeof(T).ToString()}.json";
        File.WriteAllText(path, JsonUtility.ToJson(dataClass));
        Logging.Log($"DataManager::SaveJsonData : {path}");
    }

    /// <summary>
    /// 로컬에 저장된 json파일 불러오기
    /// 없으면 new()로 생성한다. Data클래스는 MonoBehaviour를 상속받지 않는 클래스만 사용 가능
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T LoadJsonData<T>() where T : new()
    {
        try
        {
            string path = _persistentDataPath + $"/{typeof(T).ToString()}.json";
            if (!File.Exists(path))
            {
                Logging.Log($"DataManager::LoadJsonData : {typeof(T).ToString()}.json not found.");

                T localTData = new T();

                return localTData;
            }

            string jsonData = File.ReadAllText(path);
            Logging.Log($"DataManager::LoadJsonData : {path} loaded.");
            return JsonUtility.FromJson<T>(jsonData);
        }
        catch (Exception ex)
        {
            Logging.LogError($"LocalDattaSystem's LoadJsonData Exception {ex.Message}");
            return default(T);
        }
    }
}