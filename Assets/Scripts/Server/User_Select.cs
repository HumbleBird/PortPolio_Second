using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using System.Collections;

public class User_Select : MonoBehaviour
{
    string m_strUrl = "process/userselect";

    IEnumerator RequestPost(string _url, string _strUserName)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", _strUserName);

        UnityWebRequest www = UnityWebRequest.Post(_url, form);

        yield return www.SendWebRequest();

        Debug.Log(www.downloadHandler.text);
    }

    public void OnBtnSelect()
    {
        StartCoroutine(RequestPost(Managers.m_strHttp + m_strUrl, "1001"));
    }
}
