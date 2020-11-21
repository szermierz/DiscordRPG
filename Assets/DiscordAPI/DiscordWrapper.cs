using System;
using System.Collections;
using UnityEngine;

public class DiscordWrapper : Singleton<DiscordWrapper>
{
    #region Fake wrapper

    public string SelectedServer = null;
    public string SelectedChannel = null;

    public void Logout(Action<bool> resultCallback)
    {
        if (m_connected)
            StartCoroutine(LogoutRoutine(resultCallback));
        else
            resultCallback(true);
    }

    private IEnumerator LogoutRoutine(Action<bool> resultCallback)
    {
        yield return new WaitForSecondsRealtime(1.0f);

        SelectedChannel = null;
        SelectedServer= null;

        m_connected = false;
        resultCallback(true);
    }

    public void Connect(Action<bool> resultCallback)
    {
        if (m_connected)
            resultCallback(true);
        else
            StartCoroutine(ConnectRoutine(resultCallback));
    }

    private bool m_connected = false;

    private IEnumerator ConnectRoutine(Action<bool> resultCallback)
    {
        yield return new WaitForSecondsRealtime(1.0f);
        m_connected = true;
        resultCallback(true);
    }

    public void GetChannelMessages(DateTime from, DateTime to, Action<Message[]> resultCallback)
    {

    }

    public void GetServers(Action<string[]> resultCallback)
    {
        StartCoroutine(GetServersRoutine(resultCallback));
    }

    private IEnumerator GetServersRoutine(Action<string[]> resultCallback)
    {
        yield return new WaitForSecondsRealtime(1.0f);
        resultCallback(new[] 
        {
            "SamoGenste",
            "InnyNieWybieraj1",
            "InnyNieWybieraj2"
        });
    }

    public void GetTextChannels(Action<string[]> resultCallback)
    {
        StartCoroutine(GetTextChannelsRoutine(resultCallback));
    }

    private IEnumerator GetTextChannelsRoutine(Action<string[]> resultCallback)
    {
        if(string.IsNullOrEmpty(SelectedServer))
        {
            resultCallback.Invoke(Array.Empty<string>());
            yield break;
        }

        yield return new WaitForSecondsRealtime(1.0f);

        resultCallback.Invoke(new[] 
        {
            "general",
            "linki",
            "inne"
        });
    }

    public struct Message
    {

    }

    #endregion
}
