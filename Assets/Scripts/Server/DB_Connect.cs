using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using System.Collections;

public class DB_Connect : MonoBehaviour
{
    string m_strUrl = "process/dbconnect"; //"http://58.78.211.147:3000/process/dbconnect":
    string m_strDisUrl = "process/dbdisconnect"; //"http://58.78.211.147:3000/process/dbdisconnect":

    IEnumerator RequestPost(string _url, string _strNum)
    {
        WWWForm form = new WWWForm();
        form.AddField("num", _strNum);

        UnityWebRequest www = UnityWebRequest.Post(_url, form);

        yield return www.SendWebRequest();

        Debug.Log(www.downloadHandler.text);
    }

    public void OnBtnConnect()
    {
        StartCoroutine(RequestPost(Managers.m_strHttp + m_strUrl, "1"));
    }

    public void OnBtnDisConnect()
    {
        StartCoroutine(RequestPost(Managers.m_strHttp + m_strDisUrl, "1"));
    }
}
