using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerInfo : UI_Scene
{
    enum Images
    {
        HPBarBG,
        HPBarBGHit,
        HPBar,
        STAMINABarBG,
        STAMINABar,
    }

    enum Texts
    {
        Name
    }

    Charater _player;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        RefreshUI();

        return true;
    }

    public void SetInfo(Charater player)
    {
        _player = player;

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (_init == false)
            return;

        Image HpBariamge = GetImage((int)Images.HPBar);
        HpBariamge.fillAmount = (float)_player.Hp / _player.MaxHp;
        Image StaminaBariamge = GetImage((int)Images.STAMINABar);
        StaminaBariamge.fillAmount = (float)_player.Stamina / _player.MaxStamina;

        TextMeshProUGUI nameText = GetText((int)Texts.Name);
        nameText.text = _player.name;
    }

    public void HitEvent()
    {
        StartCoroutine(DownHP());
    }

    public IEnumerator DownHP()
    {
        RefreshUI();

        yield return new WaitForSeconds(0.5f);

        Image HpBarBGHitiamge = GetImage((int)Images.HPBarBGHit);

        while (true)
        {
            HpBarBGHitiamge.fillAmount -= Time.deltaTime;
            if (HpBarBGHitiamge.fillAmount <= (float)_player.Hp / _player.MaxHp)
            {
                HpBarBGHitiamge.fillAmount = (float)_player.Hp / _player.MaxHp;
                break;
            }

            yield return null;
        }

        
    }
}
