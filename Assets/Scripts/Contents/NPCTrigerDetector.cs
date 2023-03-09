using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NPCTrigerDetector : MonoBehaviour
{
    UI_Popup popup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            NPC npc = transform.GetComponentInParent<NPC>();
            popup = npc.MeetPlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(popup != null)
        {
            Managers.UI.ClosePopupUI(popup);
            StopCoroutine(Managers.Battle.NPCInteractionEventFunction());
        }
    }

}
