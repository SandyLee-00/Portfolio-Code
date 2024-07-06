using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;

public class UIManager
{
    int order = -20;
    Stack<UI_Popup> popupStack = new Stack<UI_Popup>();
    public UI_Scene SceneUI { get; private set; }

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("UI_Root");
            if (root == null)
            {
                root = new GameObject { name = "UI_Root" };
            }

            return root;
        }
    }

    /// <summary>
    /// SubItem UI는 여기서 Instantiate 한다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject prefab = Resources.Load<GameObject>($"Prefabs/UI/SubItem/{name}");

        GameObject _gameObject = Managers.Resource.Instantiate(prefab);
        if (parent != null)
        {
            _gameObject.transform.SetParent(parent);
        }

        _gameObject.transform.localScale = Vector3.one;
        _gameObject.transform.localPosition = prefab.transform.position;

        return _gameObject.GetOrAddComponent<T>();
    }

    /// <summary>
    /// Popup UI는 여기서 Instantiate 한다, sortingOrder 스택으로 관리해주기 때문이다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public T ShowPopupUI<T>(string name = null, Transform parent = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject prefab = Resources.Load<GameObject>($"Prefabs/UI/Popup/{name}");
        GameObject _gameObject = Managers.Resource.Instantiate(prefab);
        T popup = _gameObject.GetOrAddComponent<T>();
        popupStack.Push(popup);

        if (parent != null)
        {
            _gameObject.transform.SetParent(parent);
        }
        else
        {
            _gameObject.transform.SetParent(Root.transform);
        }

        _gameObject.transform.localScale = Vector3.one;
        _gameObject.transform.localPosition = prefab.transform.position;

        return popup;
    }

    /// <summary>
    /// Popup UI 닫기 
    /// </summary>
    /// <param name="popup"></param>
    public void ClosePopupUI(UI_Popup popup = null)
    {
        if (popupStack.Count == 0)
        {
            Debug.LogError("UIManager::ClosePopupUI() popupStack is empty.");
            return;
        }

        // 인자 없이 호출했을 때 가장 위에 있는 팝업을 삭제한다.
        if (popup == null)
        {
            popup = popupStack.Peek();
        }

        // 삭제하려고 하는게 가장 위에 있는 팝업이 아니면 에러
        if (popupStack.Peek() != popup)
        {
            Debug.LogError("UIManager::ClosePopupUI() popupStack is not same.");
            return;
        }

        GameObject.Destroy(popupStack.Pop().gameObject);
        popup = null;
        order--;
    }


    /// <summary>
    /// 씬 시작할 때 각 캔버스의 sortingOrder를 설정한다. -20부터 시작해서 1씩 증가한다.
    /// </summary>
    /// <param name="_gameObject"></param>
    /// <param name="sort"></param>
    public void SetCanvas(GameObject _gameObject, bool sort = true)
    {
        Canvas canvas = _gameObject.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = order;
            order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    /// <summary>
    /// 씬 시작할 때 열려있는 팝업 UI 모두 닫기
    /// </summary>
    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }

    /// <summary>
    /// Scene UI는 여기서 Instantiate 한다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject prefab = Resources.Load<GameObject>($"Prefabs/UI/Scene/{name}");
        if (prefab == null)
        {
            Debug.LogError($"UIManager::ShowSceneUI() prefab is null. name = {name}");
            return null;
        }
        GameObject _gameObject = Managers.Resource.Instantiate(prefab);
        T sceneUI = _gameObject.GetOrAddComponent<T>();
        SceneUI = sceneUI;

        _gameObject.transform.SetParent(Root.transform);

        return sceneUI;
    }
}
