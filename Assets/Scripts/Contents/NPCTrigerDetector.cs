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
            npc = transform.GetComponentInParent<NPC>();

            popup = npc.ShowSelectWindow();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(popup != null)
        {
            Managers.UI.ClosePopupUI(popup);
        }
        npc.StopStandInputkey();
    }

}
