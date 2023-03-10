using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShop
{
    void BuyItem(int id);
    void SellItem(int id);
}

public class NPC : MonoBehaviour
{
    // Diarogue
    protected Dialogue m_dialogue = new Dialogue();

    public UI_Popup ShowSelectWindow()
    {
        StartCoroutine(Managers.Battle.NPCInteractionEventFunction());
        return Managers.UI.ShowPopupUI<UI_SelectWindow>();
    }

    public virtual void Talk()
    {
        Managers.UI.ClosePopupUI();
    }

    public virtual void Interaction()
    {
        Managers.UI.ClosePopupUI();


    }

    // 모든 상호작용이 끝나면
    public virtual void EndInteraction()
    {
        Managers.UI.ClosePopupUI();

        Managers.Object.myPlayer.m_bWaiting = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Managers.Battle.m_npc = null;
    }
}
