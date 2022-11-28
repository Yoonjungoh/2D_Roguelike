using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
    enum Texts
    {
        ButtonText,
        ScoreText,
    }
    enum Buttons
    {
        ScoreButton,
    }
    enum Images
    {
        ItemImage,
    }
    enum GameObjects
    {
        TestObject,
    }

    private void Start()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));

        //Get<Text>((int)Texts.ScoreText).text = "Bind Test1";
        //GetText((int)Texts.ScoreText).text = "Bind Test2";

        GetButton((int)Buttons.ScoreButton).gameObject.AddUIEvent(OnButtonClicked, Define.UIEvent.Click);

        GameObject go = GetImage((int)Images.ItemImage).gameObject;
        AddUIEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);
    }

    private int _score = 0;

    public void OnButtonClicked(PointerEventData data)
    {
        _score++;
        GetText((int)Texts.ScoreText).text = $"Á¡¼ö : {_score}";
    }
}
