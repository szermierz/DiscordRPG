using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectChannelButton : MonoBehaviour
{
    [SerializeField]
    private Text m_channelNameText = default;
    
    private string m_channelName = default;
    public string ChannelName
    {
        get => m_channelName;
        set
        {
            if (value == m_channelName)
                return;

            m_channelName = value;
            m_channelNameText.text = m_channelName;
        }
    }

    public static SelectChannelButton Create(Transform root, SelectChannelButton channelPrefab, string channel, Action<string> selectedCallback)
    {
        var button = Instantiate(channelPrefab, root);
        button.ChannelName = channel;
        button.GetComponentInChildren<Button>().onClick.AddListener(() => selectedCallback.Invoke(channel));

        return button;
    }
}
