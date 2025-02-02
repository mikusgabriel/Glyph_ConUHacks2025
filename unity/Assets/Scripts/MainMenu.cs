using UnityEngine;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private ServerConnection server;


    [Header("Panels")]
    [SerializeField]
    private GameObject welcomePanel;
    [SerializeField]
    private GameObject gamePanel;


    public void OnClickStartAlphabet()
    {
        Debug.Log("Menu: Starting alphabet");
        server.SendJson(new MessageData
        {
            type = "start_alphabet",
        });
        welcomePanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void OnClickStartConversation()
    {
        Debug.Log("Menu: Starting conversation");
        server.SendJson(new MessageData
        {
            type = "start_conversation",
        });
        welcomePanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    [Serializable]
    class MessageData
    {
        public string type;
    }
}
