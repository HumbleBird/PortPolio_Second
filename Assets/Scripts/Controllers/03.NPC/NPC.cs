using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // Diarogue
    protected Dialogue m_dialogue = new Dialogue();

    public UI_Popup MeetPlayer()
    {
        Managers.Battle.m_npc = this;

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

        StartCoroutine(Managers.UIBattle.DelegateShowAndClose(() 
            => { 
                Managers.UI.ClosePopupUI();
                Managers.UI.ShowPopupUI<UI_SelectWindow>();
                StartCoroutine(Managers.Battle.NPCInteractionEventFunction());
            }));
    }

    // 모든 상호작용이 끝나면
    public virtual void EndInteraction()
    {
        Managers.UI.ClosePopupUI();

        Managers.Object.myPlayer.m_bWaiting = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Managers.UI.ClosePopupUI();
        Managers.Battle.m_npc = null;
    }
}
