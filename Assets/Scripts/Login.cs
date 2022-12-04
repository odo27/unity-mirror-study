using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{
    [SerializeField] TMP_InputField idInput;
    [SerializeField] TMP_InputField passwordInput;

    [SerializeField] string url;

    public void LoginClick() => StartCoroutine(LoginCo());

    IEnumerator LoginCo()
    {
        WWWForm form = new WWWForm();
        form.AddField("command", "login");
        form.AddField("id", idInput.text);
        form.AddField("password", passwordInput.text);
        form.AddField("nickname", "empty");

        UnityWebRequest www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();

        if (!www.downloadHandler.text.Equals("Fail to login"))
        {
            www.Dispose();
            SceneManager.LoadScene("Game");
        }
    }
}
