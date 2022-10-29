using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Table_Reload : MonoBehaviour
{
    static public TableManager Table_Mgr;

    [MenuItem("CS_Utill/Table/CSV &F1", false, 1)]
    static public void Parser_Table_CSV()
    {
        init();
    }

    static public void init()
    {
        Table_Mgr = new TableManager();
        Table_Mgr.Init();
        Table_Mgr.Save();
    }

}
