using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UISubItem_EggInventoryItem : UISubItem
{
    [Header("Image")]
    public Image EggImage;

    [Header("Button")]
    public Button EggButton;

    [Header("Text")]
    public TextMeshProUGUI EggNameText;

    public UISubItem_EggInventoryItem_Presenter Presenter;

    protected override void Awake()
    {
        base.Awake();
        Presenter = new UISubItem_EggInventoryItem_Presenter(this);
    }

    public void SetButton(Action OnClickEggButton)
    {
        EggButton.onClick.AddListener(() => { OnClickEggButton(); OnUIClickSound(); });
    }

    public void SetText(string eggName)
    {
        EggNameText.text = eggName;
    }

    public void SetSprite(Sprite sprite)
    {
        EggImage.sprite = sprite;
    }
}

public class UISubItem_EggInventoryItem_Presenter
{
    private UISubItem_EggInventoryItem _view;

    private ServerUserEggData _egg;

    private UIPopup_EggInventory _parent;

    public UISubItem_EggInventoryItem_Presenter(UISubItem_EggInventoryItem view)
    {
        _view = view;

        SetButton();
    }

    public void SetButton()
    {
        _view.SetButton(OnClickEggButton);
    }

    private void OnClickEggButton()
    {
        _parent.Presenter.SelectEgg(_egg);
    }

    public void SetSubItem(ServerUserEggData egg, UIPopup_EggInventory parent)
    {
        _egg = egg;
        _parent = parent;

        string eggName = DataManager.Instance.BaseData.EggInfo[egg.EggId].Name;
        Sprite sprite = ResourceManager.Instance.Load<Sprite>($"Icon/EggIcon/{egg.EggId}");

        _view.SetText(eggName);
        _view.SetSprite(sprite);
    }
}
