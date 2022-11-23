using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SettingKey : UI_Popup
{
    enum Buttons
    {
        Preset1_btn,
        Preset2_btn,
        Preset3_btn,
        Save_btn,
    }

    enum GameObjes
    {
        VerticalLayout,
        InputWatingImage,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjes));

        Managers.InputKey._presetButtons[0] = GetButton((int)Buttons.Preset1_btn);
        Managers.InputKey._presetButtons[1] = GetButton((int)Buttons.Preset2_btn);
        Managers.InputKey._presetButtons[2] = GetButton((int)Buttons.Preset3_btn);

        Managers.InputKey._saveButton = GetButton((int)Buttons.Save_btn);

        Managers.InputKey._waitingInputGo = GetObject((int)GameObjes.InputWatingImage);
        Managers.InputKey._waitingInputGo.gameObject.SetActive(false);

        Managers.InputKey._verticalLayoutTr = GetObject((int)GameObjes.VerticalLayout).transform;

        // ���߿� ��� �ɼ� ���� â���� �� ���� ���ִ� ����� ����� ����
        gameObject.SetActive(false);

        return true;
    }

    public void RefreshUI()
    {
        if (_init == false)
            return;
    }
}
