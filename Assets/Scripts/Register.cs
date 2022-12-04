using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;

public class Register : MonoBehaviour
{
    [SerializeField] TMP_InputField idInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TMP_InputField nicknameInput;

    [SerializeField] string url;

    public void RegisterClick() => StartCoroutine(RegisterCo());

    IEnumerator RegisterCo()
    {
        WWWForm form = new WWWForm();
        form.AddField("command", "register");
        form.AddField("id", idInput.text);
        form.AddField("password", passwordInput.text);
        form.AddField("nickname", nicknameInput.text);

        UnityWebRequest www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();

        if (www.downloadHandler.text.Equals("Register complete"))
        {
            www.Dispose();
            SceneManager.LoadScene("Login");
        }
    }
}
