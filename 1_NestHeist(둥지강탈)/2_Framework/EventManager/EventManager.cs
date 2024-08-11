using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EventType
{
    // 퀘스트 컨텐츠
    OnMonsterDead,
    OnEnterPortal,

    OnUseSmallNutrient,
    OnUseMediumNutrient,
    OnUseLargeNutrient,
    OnEndHatching,

    OnGetGold,
    OnLogin,

    // 사운드
    OnSetVolume,

    // 퀘스트 완료처리
    OnWaitingForCompletion,
}

/// <summary>
/// 리스너 인터페이스
/// </summary>
public interface IListener
{
    void OnEvent(EventType eventType, object sender, params object[] paramObjects);
}

/// <summary>
/// 중앙에서 이벤트 관리하는 매니저
/// </summary>
public class EventManager : Singleton<EventManager>
{
    private Dictionary<EventType, List<IListener>> ListenerDic = new Dictionary<EventType, List<IListener>>();

    protected override void Awake()
    {
        _isDontDestroyOnLoad = true;
        base.Awake();
    }

    /// <summary>
    /// 이벤트 타입에 해당하는 리스너리스트에 리스너 추가 
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="Listener"></param>
    public void AddListener(EventType eventType, IListener Listener)
    {
        List<IListener> ListenerList = null;

        if (ListenerDic.TryGetValue(eventType, out ListenerList))
        {
            ListenerList.Add(Listener);
            return;
        }

        ListenerList = new List<IListener>
        {
            Listener
        };
        ListenerDic.Add(eventType, ListenerList);
    }

    /// <summary>
    /// 이벤트 타입에 해당하는 리스너리스트에서 리스너 제거
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="listener"></param>
    public void RemoveListener(EventType eventType, IListener listener)
    {
        List<IListener> ListenerList = null;

        if (ListenerDic.TryGetValue(eventType, out ListenerList))
        {
            ListenerList.Remove(listener);

            // 리스너 리스트가 비었다면 이벤트 타입 제거
            if (ListenerList.Count == 0)
            {
                ListenerDic.Remove(eventType);
            }
        }
    }

    /// <summary>
    /// 리스너에게 이벤트를 알린다. 
    /// </summary>
    /// <param name="eventType">불려질 이벤트</param>
    /// <param name="sender">이벤트를 부르는 오브젝트</param>
    /// <param name="param">선택 가능한 파라미터  </param>
    public void Invoke(EventType eventType, object sender, params object[] paramObjects)
    {
        List<IListener> listenerList = null;

        // 이벤트 타입에 해당하는 리스너 리스트가 없다면 리턴
        if (ListenerDic.TryGetValue(eventType, out listenerList) == false)
        {
            return;
        }

        // OnEvent 돌면서 불러주기
        foreach (IListener listener in listenerList)
        {
            if (listener.Equals(null) == false)
            {
                listener.OnEvent(eventType, sender, paramObjects);
            }
        }
    }

    /// <summary>
    /// 이벤트 제거
    /// </summary>
    /// <param name="eventType"></param>
    public void RemoveEvent(EventType eventType)
    {
        ListenerDic.Remove(eventType);
    }

    /// <summary>
    /// null인 리스너 제거
    /// </summary>
    public void RemoveNullListener()
    {
        Dictionary<EventType, List<IListener>> TempListenerDic = new Dictionary<EventType, List<IListener>>();

        foreach (KeyValuePair<EventType, List<IListener>> Item in ListenerDic)
        {
            // 널인 리스너 제거, 리스트여서 뒤에서부터 제거
            for (int i = Item.Value.Count - 1; i >= 0; i--)
            {
                if (Item.Value[i].Equals(null))
                {
                    Item.Value.RemoveAt(i);
                }
            }

            // 널 아닌 것만 넣어주기
            if (Item.Value.Count > 0)
            {
                TempListenerDic.Add(Item.Key, Item.Value);
            }
        }

        ListenerDic = TempListenerDic;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RemoveNullListener();
    }
}
