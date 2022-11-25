using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public partial class PlayerAction : Strategy
{
    bool _ispopupsetting = false;
    void ShowInputKeySetting()
    {
        UI_SettingKey popup = Managers.UIBattle.UISetting;

        if (_ispopupsetting == false)
        {
            popup.gameObject.SetActive(true);
            _ispopupsetting = true;
        }
        else
        {
            popup.gameObject.SetActive(false);
            _ispopupsetting = false;
        }
    }
}
