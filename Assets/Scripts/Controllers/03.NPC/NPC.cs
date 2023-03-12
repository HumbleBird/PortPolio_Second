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
    protected Coroutine m_coInteraction = null;

    public UI_Popup ShowSelectWindow()
    {
        NPCInteraction();

        return Managers.UI.ShowPopupUI<UI_SelectWindow>();
    }

    public virtual void Talk()
    {
    }

    public virtual void StartInteraction()
    {
    }

    // 모든 상호작용이 끝나면
    public virtual void EndInteraction()
    {

        Managers.Battle.PlayerCanMove();
    }

    protected void NPCInteraction()
    {
        m_coInteraction = StartCoroutine(Managers.Battle.IStandAction(
            () => {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    Talk();
                    Managers.UI.ClosePopupUI();
                    StopStandInputkey();
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    StartInteraction();
                    Managers.UI.ClosePopupUI();
                    StopStandInputkey();

                }
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    EndInteraction();
                    Managers.UI.ClosePopupUI();
                    StopStandInputkey();
                }
            }));
    }

    public void StopStandInputkey()
    {
        StopCoroutine(m_coInteraction);
    }
}
