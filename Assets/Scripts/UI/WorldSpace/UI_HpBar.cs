using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar : UI_Base
{
    enum Images
    {
        HPBarBG,
        HPBarBGHit,
        HPBar,
    }

    enum Texts
    {
        DamageValue
    }

    Stat m_Stat;
    TextMeshProUGUI HitDamageValue;
    int preHp;
    Canvas m_canvas;
    Stopwatch m_watch = new Stopwatch();

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        HitDamageValue = GetText((int)Texts.DamageValue);
        m_canvas = GetComponent<Canvas>();
        m_canvas.enabled = false;

        return true;
    }

    private void Start()
    {
        Character character = GetComponentInParent<Character>();
        m_Stat = character.m_Stat;
        preHp = m_Stat.m_iHp;
    }

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
        transform.rotation = Camera.main.transform.rotation;
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        m_canvas.enabled = true;



        Image HpBariamge = GetImage((int)Images.HPBar);

        HpBariamge.fillAmount = m_Stat.m_iHp / (float)m_Stat.m_iMaxHp;

        StartCoroutine(DownHP());
        StartCoroutine(DamageValueUp());

        TimeCheck();
    }

    // HP �ڿ� �ִ� HpHit
    IEnumerator DownHP()
    {
        yield return new WaitForSeconds(0.5f);

        Image HpBarBGHitiamge = GetImage((int)Images.HPBarBGHit);

        while (true)
        {
            HpBarBGHitiamge.fillAmount -= Time.deltaTime;
            if (HpBarBGHitiamge.fillAmount <= m_Stat.m_iHp / m_Stat.m_iMaxHp)
            {
                HpBarBGHitiamge.fillAmount = m_Stat.m_iHp / m_Stat.m_iMaxHp;
                yield break;
            }

            yield return null;
        }
    }

    float currentdamageValue = 0;
    float damage = 0;

    IEnumerator DamageValueUp()
    {
        int newDamage = 0;

        // ������ �� = ������ ������ �״� ü�� �� - ���� ü�� ��
        if (preHp != m_Stat.m_iHp)
        {
            newDamage = preHp - m_Stat.m_iHp;
            preHp = m_Stat.m_iHp;
        }
        else
            yield break;

        // ������ �ջ� ���
        // �̹� ������ ��ġ�� �����ٸ� ù ������ ǥ��
        // �̹� ������ ��ġ�� �����ٸ� ������ �ջ����� ��ġ�� �÷�����
        if (HitDamageValue.enabled == false)
        {
            HitDamageValue.enabled = true;

            currentdamageValue = 0;

            damage = newDamage;
        }
        else
        {
            damage += newDamage;
        }

        // ������ ��ġUI �÷� ������
        while (true)
        {
            if (currentdamageValue < damage)
            {
                currentdamageValue += 100 * Time.deltaTime;
                HitDamageValue.text = ((int)currentdamageValue).ToString();
            }
            else
                yield break;

            yield return null;
        }

    }

    void TimeCheck()
    {
        m_watch.Reset();
        m_watch.Start();
        StartCoroutine(ITimeCheck());
    }

    IEnumerator ITimeCheck()
    {
        while (true)
        {
            if (m_watch.Elapsed.TotalSeconds >= 2)
            {
                HitDamageValue.enabled = false;
            }
            if (m_watch.Elapsed.TotalSeconds >= 10)
            {
                m_canvas.enabled = false;
                yield break;
            }
            yield return null;
        }
    }
}
