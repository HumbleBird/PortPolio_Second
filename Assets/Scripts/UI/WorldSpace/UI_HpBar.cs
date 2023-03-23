using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar : UI_Base
{
    enum GameObjects
    {
        HpBar
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        Character character = GetComponentInParent<Character>();
        m_Stat = character.m_Stat;

        return true;
    }

    Stat m_Stat;

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
        transform.rotation = Camera.main.transform.rotation;
    }

    public override void RefreshUI()
    {
        float ratio = m_Stat.m_iHp / (float)m_Stat.m_iMaxHp;
        GetObject((int)GameObjects.HpBar).GetComponent<Slider>().value = ratio;
    }

    public IEnumerator DownHP()
    {
        yield return new WaitForSeconds(0.5f);

        Image HpBarBGHitiamge = GetImage((int)Images.HPBarBGHit);

        while (true)
        {
            HpBarBGHitiamge.fillAmount -= Time.deltaTime;
            if (HpBarBGHitiamge.fillAmount <= _player.m_Stat.m_iHp / _player.m_Stat.m_iMaxHp)
            {
                HpBarBGHitiamge.fillAmount = _player.m_Stat.m_iHp / _player.m_Stat.m_iMaxHp;
                break;
            }

            yield return null;
        }
    }
}
