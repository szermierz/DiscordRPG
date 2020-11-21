using UnityEngine;

public class DiscordWrapper : MonoBehaviour
{
    public void Dupa()
    {
        var wrapper = new DiscordAPI.Wrapper();
        GetComponentInChildren<UnityEngine.UI.Text>().text = wrapper.Test().ToString();
    }
}
