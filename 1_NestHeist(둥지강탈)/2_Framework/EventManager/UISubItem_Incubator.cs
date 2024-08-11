using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 부화기 
/// </summary>
public class UISubItem_Incubator : UISubItem
{
    [Header("GameObject")]
    public GameObject EggImage;

    [Header("Text")]
    public TextMeshProUGUI LeftSeconds;

    [Header("Button")]
    public Button IncubatorButton;

    private UISubItem_Incubator_Presenter _presenter;

    protected override void Awake()
    {
        base.Awake();

        _presenter = new UISubItem_Incubator_Presenter(this);
        _presenter.Init();

        GameManager.Instance.HatchingManager.OnRemainSecondUpdated += _presenter.SetText;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // TODO : _presenter가 null일 때 에러 처리
        if(_presenter == null)
        {
            Logging.LogError("UISubItem_Incubator::OnEnable() _presenter is null");
        }

        _presenter.Init();
    }

    public void SetGameObject(Sprite egg, bool isActive)
    {
        EggImage.GetComponent<Image>().sprite = egg;
        EggImage.SetActive(isActive);
    }

    public void SetText(string time)
    {
        LeftSeconds.text = time;
    }

    public void SetButton(Action OnIncubator)
    {
        IncubatorButton.onClick.AddListener(() => { OnIncubator(); OnUIClickSound(); }) ;
    }
}

public class UISubItem_Incubator_Presenter
{
    private UISubItem_Incubator _view;

    private string _incubatorId = GameManager.Instance.HatchingManager.IncubatorId;

    public UISubItem_Incubator_Presenter(UISubItem_Incubator view)
    {
        _view = view;

        SetButton();
    }

    public void Init()
    {
        SetText();
        SetGameObject();
    }

    public void SetText()
    {
        string indicator = "";

        if (GameManager.Instance.HatchingManager.IncubatorDic[_incubatorId].ServerData.UserEggId == Define.NoEgg)
        {
            indicator = "빈 부화기";
        }
        else
        {
            // 부화기 비어있지 않음
            int leftSeconds = GameManager.Instance.HatchingManager.IncubatorDic[_incubatorId].ServerData.RemainingTime;

            if (leftSeconds > 0)
            {
                TimeSpan time = TimeSpan.FromSeconds(leftSeconds);
                indicator = time.ToString();
            }
            else
            {
                indicator = "부화 완료!";
            }
        }

        _view.SetText(indicator);
    }

    public void SetButton()
    {
        _view.SetButton(OnClickIncubatorButton);
    }

    private void OnClickIncubatorButton()
    {
        // 알 비어있을 때
        if (GameManager.Instance.HatchingManager.IncubatorDic[_incubatorId].ServerData.UserEggId == Define.NoEgg)
        {
            UIManager.Instance.InstanciateUIPopup<UIPopup_EggInventory>();
        }
        else
        {
            int leftSeconds = GameManager.Instance.HatchingManager.IncubatorDic[_incubatorId].ServerData.RemainingTime;

            // 알 있을 때 : 부화 시간 남음 -> 팝업
            if (leftSeconds > 0)
            {
                UIPopup_Warning popup = UIManager.Instance.InstanciateUIPopup<UIPopup_Warning>();
                popup.SetText("부화 중입니다.");
            }

            // 알 있을 때 : 부화 시간 끝 -> 팝업
            if (leftSeconds <= 0)
            {
                OnEndHatch();
            }
        }
    }

    private async void OnEndHatch()
    {
        Incubator incubator = GameManager.Instance.HatchingManager.CurrentIncubator;
        string EggId = incubator.ServerData.EggId;

        // 몬스터 랜덤 생성
        string monsterId = DataManager.Instance.BaseData.EggInfo[EggId].MonsterIdList.GetRandomT();

        // 몬스터의 속성 중 하나 랜덤 결정 
        Define.AttributeType attribute = DataManager.Instance.BaseData.MonsterInfo[monsterId].AttributeTypeList.GetRandomT();

        // 스킬 중 맞는 속성으로 리스트, 스킬 랜덤 결정
        Dictionary<string, MonsterSkillInfoData> dic = DataManager.Instance.BaseData.MonsterSkillInfo;
        List<string> tempSkillList = new List<string>();
        foreach (string skill in dic.Keys)
        {
            if (dic[skill].AttributeType == attribute)
            {
                tempSkillList.Add(skill);
            }
        }

        string skillKId = tempSkillList.GetRandomT();


        await LoadingManager.Instance.RunWithLoading(async () =>
        {
            await DataManager.Instance.ServerData.EndHatchEgg(new HatchEggBody
            {
                Id = incubator.ServerData.Id,
                UserId = DataManager.Instance.ServerData.User.UserId,
                MonsterId = monsterId,
                SkillId = skillKId
            });
        },
        onLoaded: () =>
        {
            UIPopup_TitleSpriteText uipopup = UIManager.Instance.InstanciateUIPopup<UIPopup_TitleSpriteText>();

            string monsterName = DataManager.Instance.BaseData.MonsterInfo[monsterId].Name;
            string skillName = DataManager.Instance.BaseData.MonsterSkillInfo[skillKId].Name;
            string mosterDesc = DataManager.Instance.BaseData.MonsterInfo[monsterId].Desc;
            Sprite sprite = ResourceManager.Instance.LoadSprite($"Icon/MonsterIcon/monster-icon", monsterId);

            uipopup.SetText("몬스터 부화 성공!", $"[{monsterName}]\n{mosterDesc}\n\n[{skillName}] 스킬을 부여받았습니다!");
            uipopup.SetSprite(sprite);
            

            GameManager.Instance.HatchingManager.EndHatching(incubator.ServerData.Id);

            EventManager.Instance.Invoke(EventType.OnEndHatching, this, EggId);

            this.Init();
        },
        onError: (message) =>
        {
            // 서버 요청 실패 시 롤백
            Logging.LogError($"UISubItem_Incubator::OnEndHatch()  실패: {message}");
        });
    }

    private void SetGameObject()
    {
        if (GameManager.Instance.HatchingManager.IncubatorDic[_incubatorId].ServerData.UserEggId == Define.NoEgg)
        {
            _view.SetGameObject(null, false);
        }
        else
        {
            string eggtypeId = GameManager.Instance.HatchingManager.CurrentIncubator.ServerData.EggId;

            Sprite eggImage = ResourceManager.Instance.Load<Sprite>($"Icon/EggIcon/{eggtypeId}");
            _view.SetGameObject(eggImage, true);
        }
    }
}