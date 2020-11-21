using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectServerButton : MonoBehaviour
{
    [SerializeField]
    private Text m_serverNameText = default;
    
    private string m_serverName = default;
    public string ServerName
    {
        get => m_serverName;
        set
        {
            if (value == m_serverName)
                return;

            m_serverName = value;
            m_serverNameText.text = m_serverName;
        }
    }

    public static SelectServerButton Create(Transform root, SelectServerButton m_selectServerPrefab, string server, Action<string> selectedCallback)
    {
        var button = Instantiate(m_selectServerPrefab, root);
        button.ServerName = server;
        button.GetComponentInChildren<Button>().onClick.AddListener(() => selectedCallback.Invoke(server));

        return button;
    }
}
