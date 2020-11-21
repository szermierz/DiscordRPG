using System;
using System.Collections;
using UnityEngine;

public class MainMenuController : MonoBehaviourEx
{
    #region State

    [Flags]
    public enum ControllerState
    {
        NotLogged     = 0b00001,
        Logging       = 0b00010,
        Logged        = 0b00100,
        SelectServer  = 0b01000,
        SelectChannel = 0b10000,
    }

    private int m_changingStateDepth = 0;

    private ControllerState m_state = ControllerState.NotLogged;
    private ControllerState State
    {
        get => m_state;
        set
        {
            if (value == m_state)
                return;

            ++m_changingStateDepth;

            var previos = m_state;
            m_state = value;

            OnStateChanged(previos, m_state);

            --m_changingStateDepth;

            if(0 == m_changingStateDepth)
                Debug.Log($"MainMenu state: {m_state}");

            RefreshMainMenu();
        }
    }

    private void AddStateFlag(ControllerState flag)
    {
        State |= flag;
    }

    private void RemoveStateFlag(ControllerState flag)
    {
        State &= ~flag;
    }

    private void OnStateChanged(ControllerState previos, ControllerState current)
    {
        // NotLogged autoswitch

        if (!previos.HasFlag(ControllerState.NotLogged) && current.HasFlag(ControllerState.NotLogged))
        {
            State = ControllerState.NotLogged;
        }

        // NotLogged exclusions

        if (!previos.HasFlag(ControllerState.Logged) && current.HasFlag(ControllerState.Logged))
        {
            RemoveStateFlag(ControllerState.NotLogged);
        }

        if (!previos.HasFlag(ControllerState.Logging) && current.HasFlag(ControllerState.Logging))
        {
            RemoveStateFlag(ControllerState.NotLogged);
        }

        // Logging autoswitch

        if (!previos.HasFlag(ControllerState.Logging) && current.HasFlag(ControllerState.Logging))
        {
            RemoveStateFlag(ControllerState.SelectChannel);
            RemoveStateFlag(ControllerState.SelectServer);
        }

        // Logging exclusions

        if (!previos.HasFlag(ControllerState.Logged) && current.HasFlag(ControllerState.Logged))
        {
            RemoveStateFlag(ControllerState.Logging);
        }

        // Logged exclusions

        if (!previos.HasFlag(ControllerState.Logging) && current.HasFlag(ControllerState.Logging))
        {
            RemoveStateFlag(ControllerState.Logged);
        }

        // SelectServer autoswitch

        if (!previos.HasFlag(ControllerState.Logged) && current.HasFlag(ControllerState.Logged))
        {
            AddStateFlag(ControllerState.SelectServer);
        }

        if(!current.HasFlag(ControllerState.Logged))
        {
            RemoveStateFlag(ControllerState.SelectServer);
            RemoveStateFlag(ControllerState.SelectChannel);
        }

        // SelectServer exclusions

        if (!previos.HasFlag(ControllerState.SelectChannel) && current.HasFlag(ControllerState.SelectChannel))
        {
            RemoveStateFlag(ControllerState.SelectServer);
        }
    }

    #endregion

    #region Initialization

    private void Start()
    {
        RefreshMainMenu();

        DiscordWrapper.Instance.SelectedServer = null;
        DiscordWrapper.Instance.SelectedChannel = null;
    }

    #endregion
    
    #region UI

    private void RefreshMainMenu()
    {
        RefreshLoggingGroups();
        RefreshSelectServerGroups();
        RefreshSelectChannelGroups();
    }

    #endregion

    #region Logging

    [Header("Logging")]
    [SerializeField]
    private GameObject m_notLoggedGroup = default;

    [SerializeField]
    private GameObject m_loggingGroup = default;

    [SerializeField]
    private GameObject m_loggedGroup = default;

    private void RefreshLoggingGroups()
    {
        m_loggedGroup.SetActive(State.HasFlag(ControllerState.Logged));
        m_loggingGroup.SetActive(State.HasFlag(ControllerState.Logging));
        m_notLoggedGroup.SetActive(State.HasFlag(ControllerState.NotLogged));
    }

    public void Login()
    {
        DiscordWrapper.Instance.Connect(OnLoginFinished);
        AddStateFlag(ControllerState.Logging);
        RefreshMainMenu();
    }

    private void OnLoginFinished(bool success)
    {
        RemoveStateFlag(ControllerState.Logging);

        if (success)
            AddStateFlag(ControllerState.Logged);
        else
            RemoveStateFlag(ControllerState.Logged);

        RefreshSelectServerButtons();
    }

    #endregion

    #region Select server

    [Header("Select server")]
    [SerializeField]
    private GameObject m_selectServerGroup = default;

    private void RefreshSelectServerGroups()
    {
        m_selectServerGroup.SetActive(State.HasFlag(ControllerState.SelectServer));
    }

    [SerializeField]
    private SelectServerButton m_selectServerPrefab = default;

    [SerializeField]
    private Transform m_selectServerButtonsRoot = default;

    [SerializeField]
    private Vector2 m_selectServerButtonPos = default;

    [SerializeField]
    private Vector2 m_selectServerButtonOffset = default;

    [SerializeField]
    private GameObject m_fetchingServersGroup = default;

    private void RefreshSelectServerButtons()
    {
        StartCoroutine(RefreshSelectServerButtonsRoutine());
    }

    private IEnumerator RefreshSelectServerButtonsRoutine()
    {
        for (int i = 0; i < m_selectServerButtonsRoot.childCount; ++i)
            Destroy(m_selectServerButtonsRoot.GetChild(i).gameObject);

        m_fetchingServersGroup.SetActive(true);

        string[] servers = null;

        DiscordWrapper.Instance.GetServers(result => { servers = result; });

        while (null == servers)
            yield return null;

        m_fetchingServersGroup.SetActive(false);

        for(int i = 0; i < servers.Length; ++i)
        {
            var server = servers[i];
            var button = SelectServerButton.Create(m_selectServerButtonsRoot, m_selectServerPrefab, server, SelectServer);

            button.GetComponent<RectTransform>().anchoredPosition = m_selectServerButtonPos + i * m_selectServerButtonOffset;
        }
    }

    public void SelectServer(string server)
    {
        DiscordWrapper.Instance.SelectedServer = server;
        AddStateFlag(ControllerState.SelectChannel);

        RefreshSelectChannelButtons();
    }

    #endregion

    #region Select channel

    [Header("Select channel")]
    [SerializeField]
    private GameObject m_selectChannelGroup = default;

    private void RefreshSelectChannelGroups()
    {
        m_selectChannelGroup.SetActive(State.HasFlag(ControllerState.SelectChannel));
    }

    [SerializeField]
    private SelectChannelButton m_selectChannelPrefab = default;

    [SerializeField]
    private Transform m_selectChannelButtonsRoot = default;

    [SerializeField]
    private Vector2 m_selectChannelButtonPos = default;

    [SerializeField]
    private Vector2 m_selectChannelButtonOffset = default;

    [SerializeField]
    private GameObject m_fetchingChannelsGroup = default;

    private void RefreshSelectChannelButtons()
    {
        StartCoroutine(RefreshSelectChannelButtonsRoutine());
    }

    private IEnumerator RefreshSelectChannelButtonsRoutine()
    {
        for (int i = 0; i < m_selectChannelButtonsRoot.childCount; ++i)
            Destroy(m_selectChannelButtonsRoot.GetChild(i).gameObject);

        m_fetchingChannelsGroup.SetActive(true);

        string[] channels = null;

        DiscordWrapper.Instance.GetTextChannels(result => { channels = result; });

        while (null == channels)
            yield return null;

        m_fetchingChannelsGroup.SetActive(false);

        for (int i = 0; i < channels.Length; ++i)
        {
            var channel = channels[i];
            var button = SelectChannelButton.Create(m_selectChannelButtonsRoot, m_selectChannelPrefab, channel, SelectChannel);

            button.GetComponent<RectTransform>().anchoredPosition = m_selectChannelButtonPos + i * m_selectChannelButtonOffset;
        }
    }

    public void SelectChannel(string channel)
    {
        DiscordWrapper.Instance.SelectedChannel = channel;
        SwitchToLobby();
    }

    #endregion

    #region Lobby

    [Header("Lobby")]
    [SerializeField]
    private string m_lobbySceneName = default;

    public void SwitchToLobby()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(m_lobbySceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    #endregion

    #region Logout

    public void Logout()
    {
        DiscordWrapper.Instance.Logout((result) => { if(result) AddStateFlag(ControllerState.NotLogged);});
    }

    #endregion
}
