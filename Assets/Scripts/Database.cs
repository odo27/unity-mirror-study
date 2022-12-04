using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Database : MonoBehaviour
{
    private string url = "https://82mi0xvyvd.execute-api.ap-northeast-2.amazonaws.com/default/game-log-controller";

    public void RequestSave(int gameid, int mainturn, int subturn, int subjectIdentity, int objectIdentity, string action, int value, int positionx, int positiony)
    {
        StartCoroutine(SaveLog(gameid, mainturn, subturn, subjectIdentity, objectIdentity, action, value, positionx, positiony));
    }

    IEnumerator SaveLog(int gameid, int mainturn, int subturn, int subjectIdentity, int objectIdentity, string action, int value, int positionx, int positiony)
    {
        WWWForm form = new WWWForm();
        form.AddField("command", "save");
        form.AddField("gameid", gameid);
        form.AddField("mainturn", mainturn);
        form.AddField("subturn", subturn);
        form.AddField("subject", subjectIdentity);
        form.AddField("object", objectIdentity);
        form.AddField("action", action);
        form.AddField("value", value);
        form.AddField("positionx", positionx);
        form.AddField("positiony", positiony);

        UnityWebRequest www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();
        www.Dispose();
    }
}
