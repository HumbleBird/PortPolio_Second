using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NPCTrigerDetector : MonoBehaviour
{
    UI_Popup popup;
    NPC npc;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            popup = Managers.UI.ShowPopupUI<UI_SelectWindow>();

            npc = transform.GetComponentInParent<NPC>();
            Managers.Battle.EVENTFunction += npc.InputButtonSelect;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(popup != null)
        {
            Managers.UI.ClosePopupUI(popup);
        }

        Managers.Battle.EVENTFunction -= npc.InputButtonSelect;

    }

}
