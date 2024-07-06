using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 모든 매니저들을 관리하는 싱글톤 클래스
/// Sound / Game / Data
/// </summary>
public class Managers : MonoBehaviour
{
    public static Managers s_instance = null;
    public static Managers Instance { get { return s_instance; } }

    private static SoundManager s_soundManager = new SoundManager();
    private static GameManager s_gameManager = new GameManager();
    private static DataManager s_dataManager = new DataManager();
    private static ResourceManager s_resourceManager = new ResourceManager();
    private static SceneManagerEx s_sceneManager = new SceneManagerEx();
    private static UIManager s_uiManager = new UIManager();


    public static SoundManager Sound { get { Init(); return s_soundManager; } }
    public static GameManager Game { get { Init(); return s_gameManager; } }
    public static DataManager Data { get { Init(); return s_dataManager; } }
    public static ResourceManager Resource { get { Init(); return s_resourceManager; } }
    public static SceneManagerEx Scene { get { Init(); return s_sceneManager; } }
    public static UIManager UI { get { Init(); return s_uiManager; } }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("Managers");

            if (go == null)
            {
                go = new GameObject { name = "Managers" };
            }

            s_instance = go.GetOrAddComponent<Managers>();
            DontDestroyOnLoad(go);

            // 매니저 객체 만들어주기
            s_soundManager.Init();

            // 데이터 로드
            s_dataManager.Init();


            Application.targetFrameRate = 60;
        }
    }
}
