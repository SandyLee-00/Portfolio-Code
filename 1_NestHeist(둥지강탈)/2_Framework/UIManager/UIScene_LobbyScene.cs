using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScene_LobbyScene : UIScene
{
    [Header("Button")]
    public Button GotoBossButton;
    public Button GotoDungeonButton;
    public Button GotoHatchingButton;
    public Button GotoPlayerStatButton;
    public Button GotoInventoryButton;
    public Button GotoMonsterInventoryButton;
    public Button GotoPartySetScreenButton;
    public Button GotoQuestButton;
    public Button SettingButton;
    public Button GuidButton;

    private UISubItem_UserCurrency _middleUpCurrency;
    [SerializeField] private List<UISubItem_LobbyLiveMonster> _liveMonsterList;

    [Header("Text")]
    public TextMeshProUGUI UserLevelText;
    public TextMeshProUGUI UserNameText;

    [Header("Image")]
    public Image PlayerImage;
    public Image PlayerEXPImage;

    private UIScene_LobbyScene_Presenter _presenter;

    [Header("Tween")]
    [SerializeField] private List<DOTweenAnimation> _tweens;

    public TutorialManager TutorialManager;

    /// <summary>
    /// 버튼 연결 말고는 다 Refresh 해주기
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        _presenter = new UIScene_LobbyScene_Presenter(this);
        _presenter.Init();

        _middleUpCurrency = GetComponentInChildren<UISubItem_UserCurrency>();
        _middleUpCurrency.Init();

        DataManager.Instance.ServerData.OnUpdateParty += _presenter.LiveMonsters;
        TutorialManager = TutorialManager.Instance;
    }

    protected override void Start()
    {
        base.Start();

        EventManager.Instance.Invoke(EventType.OnLogin, this);
        AudioManager.Instance.PlayBGM("IntroLobbyBGM");
    }

    private void OnDestroy()
    {
        DataManager.Instance.ServerData.OnUpdateParty -= _presenter.LiveMonsters;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        OnEnableDotween();

        _presenter.Init();

        // 튜토 버튼 활성화 조절
        SetGuidButtonActive();

        if (_middleUpCurrency == null)
        {
            _middleUpCurrency = GetComponentInChildren<UISubItem_UserCurrency>();
        }
        _middleUpCurrency.Init();

        var generalTutorial = TutorialManager.GetTutorialById("general_tutorial");
        if (generalTutorial != null)
        {
            TutorialManager.ShowGeneralTutorialIfNotShown(generalTutorial);
        }
    }

    private void OnEnableDotween()
    {
        foreach (DOTweenAnimation tween in _tweens)
        {
            tween.DORestart();
        }
    }

    public void SetButton(Action OnGotoPlayerStat, Action OnGotoInventory, Action OnGotoMonsterInventory, 
        Action OnGotoHatching, Action OnGotoDungeon, Action OnGotoPartySetScreen, Action OnGotoQuest,
        Action OnSetting, Action OnGuide)
    {
        GotoPlayerStatButton.onClick.AddListener(() => { OnGotoPlayerStat(); OnUIClickSound(); });
        GotoInventoryButton.onClick.AddListener(() => { OnGotoInventory(); OnUIClickSound(); });
        GotoMonsterInventoryButton.onClick.AddListener(() => { OnGotoMonsterInventory(); OnUIClickSound(); });
        GotoHatchingButton.onClick.AddListener(() => { OnGotoHatching(); OnUIClickSound(); });
        GotoDungeonButton.onClick.AddListener(() => { OnGotoDungeon(); OnUIClickSound(); });
        GotoPartySetScreenButton.onClick.AddListener(() => { OnGotoPartySetScreen(); OnUIClickSound(); });
        GotoQuestButton.onClick.AddListener(() => { OnGotoQuest(); OnUIClickSound(); });
        SettingButton.onClick.AddListener(() => { OnSetting(); OnUIClickSound(); });
        GuidButton.onClick.AddListener(() => { OnGuide(); OnUIClickSound(); });
    }

    public void SetText(string level, string username)
    {
        UserLevelText.text = level;
    }

    public void SetImage(Sprite playerImage)
    {
        PlayerImage.sprite = playerImage;
    }

    public void SetMiddlePartnerMonsters(List<string> monsterIds)
    {
        for (int i = 0; i < _liveMonsterList.Count; i++)
        {
            if (i < monsterIds.Count)
            {
                _liveMonsterList[i].Initialize(monsterIds[i]);
            }
            else
            {
                _liveMonsterList[i].Clear();
            }

        }
    }

    public void SetBar(float fill)
    {
        PlayerEXPImage.fillAmount = fill;
    }

    private void SetGuidButtonActive()
    {
        // TutorialManager의 상태를 기준으로 버튼 활성화 조절
        if (TutorialManager != null)
        {
            var isGeneralTutorialEnabled = TutorialManager.IsTutorialEnabled("general_tutorial");
            GuidButton.gameObject.SetActive(isGeneralTutorialEnabled);
        }
    }
}

public class UIScene_LobbyScene_Presenter : Presenter
{
    private UIScene_LobbyScene _view;

    public UIScene_LobbyScene_Presenter(UIScene_LobbyScene view)
    {
        _view = view;

        SetButton();
    }

    public void Init()
    {
        SetText();
        SetSprite();
        LiveMonsters();
        SetBar();
    }

    private void SetButton()
    {
        _view.SetButton(UserStat, Inventory, MonsterInventory, Hatching, Dungeon, PartySetScreen, Quest, Setting, GuidOpen);
        _view.GotoBossButton.onClick.AddListener(() => { SceneManager.LoadScene("Boss001Scene"); });
    }

    private void SetText()
    {
        string userLevel = DataManager.Instance.ServerData.User.Level.ToString();
        string username = DataManager.Instance.ServerData.User.Username;

        _view.SetText(userLevel, username);
    }

    /// <summary>
    /// 가운데 몬스터 3마리 이미지
    /// </summary>
    private void SetSprite()
    {
        string playerImageId = "PID00001";
        Sprite playerSprite = ResourceManager.Instance.Load<Sprite>($"Icon/PlayerIcon/{playerImageId}");

        _view.SetImage(playerSprite);
    }

    public void LiveMonsters(ServerUserGameInfoData body = null)
    {
        List<string> monsterIds = new List<string>();
        foreach (var id in DataManager.Instance.ServerData.User.PartnerMonsterIds)
        {
            monsterIds.Add(GameManager.Instance.UserMonsterManager.UserMonsterList[id].UserData.MonsterId);
        }
        _view.SetMiddlePartnerMonsters(monsterIds);
    }

    private void SetBar()
    {
        // TODO : 플레이어 레벨 업 계산
        /*float fill = 

        _view.SetBar(fill);*/
    }

    private void UserStat()
    {
        UIManager.Instance.InstanciateUIPopup<UIPopup_UserStat>();
    }

    private void Inventory()
    {
        UIManager.Instance.InstanciateUIPopup<UIPopup_Inventory>();
        _view.gameObject.SetActive(false);
    }

    private void MonsterInventory()
    {
        UIManager.Instance.InstanciateUIPopup<UIPopup_MonsterInventory>();
        _view.gameObject.SetActive(false);
    }

    private void Hatching()
    {
        UIManager.Instance.InstanciateUIPopup<UIPopup_Incubator>();
        _view.gameObject.SetActive(false);
    }

    private void Dungeon()
    {
        UIManager.Instance.InstanciateUIPopup<UIPopup_DungeonTypeSelect>();
        _view.gameObject.SetActive(false);
    }

    private void PartySetScreen()
    {
        UIManager.Instance.InstanciateUIPopup<UIPopup_ShowPartyStateScreen>();
        _view.gameObject.SetActive(false);
    }

    private void Quest()
    {
        UIManager.Instance.InstanciateUIPopup<UIPopup_QuestList>();
        _view.gameObject.SetActive(false);
    }

    private void Setting()
    {
        UIManager.Instance.InstanciateUIPopup<UIPopup_Setting>();
    }

    private void GuidOpen()
    {
        var generalTutorial = _view.TutorialManager.GetTutorialById("general_tutorial");
        if (generalTutorial != null && generalTutorial.enabled)
        {
            UIPopup_Tutorial popup = UIManager.Instance.InstanciateUIPopup<UIPopup_Tutorial>();

            foreach (var page in generalTutorial.pages)
            {
                if (page.screen == Define.Screen1)
                    popup.Scrren1.SetActive(page.index == popup.CurrentPage);
                if (page.screen == Define.Screen2)
                    popup.Scrren2.SetActive(page.index == popup.CurrentPage);
                if (page.screen == Define.Screen3)
                    popup.Scrren3.SetActive(page.index == popup.CurrentPage);
            }
        }
    }
}


