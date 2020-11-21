using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    #region Main menu

    [SerializeField]
    private string m_mainMenuSceneName = default;

    public void Logout()
    {
        DiscordWrapper.Instance.Logout((result) => 
        {
            if (result)
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(m_mainMenuSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        });
    }

    #endregion

    #region Lobby

    [SerializeField]
    private Text m_result = null;

    private FinishOAuth2Delegate m_oauthReceived;

    public void Dupa()
    {
        Application.OpenURL("https://discord.com/api/oauth2/authorize?client_id=779750345458188290&redirect_uri=discordrpg%3A%2F%2Fauth&response_type=code&scope=email");
    }


    public FinishOAuth2Delegate Connect(StartOAuth2Delegate start)
    {
        var clientID = "0";
        var url = $"hello im:{clientID}";

        start(url);
        return Connect2;
    }

    private void Connect2(string url) //FinishOAuth2Delegate
    {

    }


    public delegate void StartOAuth2Delegate(string url);
    public delegate void FinishOAuth2Delegate(string url);

    private void Start()
    {
        Application.deepLinkActivated += OnDeepLinkURLOpened;
    }

    public void OnDeepLinkURLOpened(string deepLinkUrl)
    {
        m_result.text = "0" + deepLinkUrl;
    }

    #endregion
}
