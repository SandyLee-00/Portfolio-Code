using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Diagnostics;
using Unity.VisualScripting;
using System.Xml.Linq;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// 모든 UI는 UI_Base 클래스를 상속받아야 한다.
/// 프리팹에 있는 자식 오브젝트 이름, Enum으로 바인딩을 해서 타입별로 dictionary에 저장한다.
/// </summary>
public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _unityObjectDictionary = new Dictionary<Type, UnityEngine.Object[]>();

    protected bool isInitalized = false;

    private void Start()
    {
        Init();
    }

    public virtual bool Init()
    {
        if (isInitalized)
        {
            return false;
        }

        isInitalized = true;
        return true;
    }

    /// <summary>
    /// enum 타입으로 선언한 이름과 동일한 이름을 가진 자식 오브젝트를 찾아서 타입별로 Dictionary에 저장한다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _unityObjectDictionary.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            // 게임 오브젝트
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, names[i], true);
            }
            // UI 컴포넌트
            else
            {
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            }

            if (objects[i] == null)
            {
                Debug.LogError($"{this.gameObject.name}::Bind() {names[i]}");
            }
        }
    }

    protected void BindObject(Type type) { Bind<GameObject>(type); }
    protected void BindImage(Type type) { Bind<Image>(type); }
    protected void BindText(Type type) { Bind<TextMeshProUGUI>(type); }
    protected void BindButton(Type type) { Bind<Button>(type); }

    /// <summary>
    /// Dictionary에서 해당 오브젝트를 가져온다. enum으로 (int)해서 인덱스를 전달한다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="idx"></param>
    /// <returns></returns>
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if(_unityObjectDictionary.TryGetValue(typeof(T), out objects) == false)
        {
            Debug.LogError($"UI_Base::Get() Fail. key is not found. {typeof(T)}");
            return null;
        }

        if (objects == null || objects.Length == 0 || idx < 0 || idx >= objects.Length)
        {
            Debug.LogError($"UI_Base::Get() Fail. {typeof(T)} idx is not valid");
            return null;
        }

        return objects[idx] as T;
    }
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
}
