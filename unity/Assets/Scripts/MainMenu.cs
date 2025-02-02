using UnityEngine;
using Oculus.Interaction.Input;
using System;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private ServerConnection server;


    [Header("Panels")]
    [SerializeField]
    private Button main;
    [SerializeField]
    private Button sttOccupation;


    [Header("Buttons")]
    [SerializeField]
    private Button leftHand;
    [SerializeField]
    private Button rightHand;

    [Header("Settings")]
    [SerializeField]
    private float sendCooldown = 0.2f;
    private float lastTimeDataSent = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        lastTimeDataSent += Time.deltaTime;
        if (lastTimeDataSent < sendCooldown)
            return;
        else
            lastTimeDataSent -= sendCooldown;

        server.SendJson(new MessageData
        {
            type = "start_all",
        });
    }

    void StartDiscover()
    {
        server.SendJson(new MessageData
        {
            type = "start_discover",
        });
    }

    void StartConversation()
    {
        server.SendJson(new MessageData
        {
            type = "start_conversation",
        });
    }

    [Serializable]
    class MessageData
    {
        public string type;
        public string occupation;
    }
}
