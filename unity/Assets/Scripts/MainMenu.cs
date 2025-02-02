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
    [SerializeField]
    private GameObject conversePanel;


    public void OnClickStopGame()
    {
        Debug.Log("Menu: Stopping game");
        server.SendJson(new MessageData
        {
            type = "stop_game",
        });
        welcomePanel.SetActive(true);
        gamePanel.SetActive(false);
        conversePanel.SetActive(false);
    }

    public void OnClickStartAlphabet()
    {
        Debug.Log("Menu: Starting alphabet");
        server.SendJson(new MessageData
        {
            type = "start_alphabet",
        });
        welcomePanel.SetActive(false);
        gamePanel.SetActive(true);
        conversePanel.SetActive(false);
    }

    public void OnClickStartConversation()
    {
        Debug.Log("Menu: Starting conversation");
        server.SendJson(new MessageData
        {
            type = "start_conversation",
        });
        welcomePanel.SetActive(false);
        gamePanel.SetActive(false);
        conversePanel.SetActive(true);
        server.aiTextReponse.text = "";
    }

    [Serializable]
    class MessageData
    {
        public string type;
    }
}
