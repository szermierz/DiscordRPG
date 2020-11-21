using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    

}
