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

    Character _player;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        RefreshUI();

        return true;
    }

    public void SetInfo(Character player)
    {
        _player = player;

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (_init == false)
            return;

        Image HpBariamge = GetImage((int)Images.HPBar);
        HpBariamge.fillAmount = _player.m_strStat.m_fHp / _player.m_strStat.m_fMaxHp;
        Image StaminaBariamge = GetImage((int)Images.STAMINABar);
        StaminaBariamge.fillAmount = _player.m_strStat.m_fStemina / _player.m_strStat.m_fMaxStemina;

        TextMeshProUGUI nameText = GetText((int)Texts.Name);
        nameText.text = _player.name;
    }

    public IEnumerator DownHP()
    {
        yield return new WaitForSeconds(0.5f);

        Image HpBarBGHitiamge = GetImage((int)Images.HPBarBGHit);

        while (true)
        {
            HpBarBGHitiamge.fillAmount -= Time.deltaTime;
            if (HpBarBGHitiamge.fillAmount <= _player.m_strStat.m_fHp / _player.m_strStat.m_fMaxHp)
            {
                HpBarBGHitiamge.fillAmount = _player.m_strStat.m_fHp / _player.m_strStat.m_fMaxHp;
                break;
            }

            yield return null;
        }

        RefreshUI();
    }
}
