using UnityEngine;
using Oculus.Interaction.Input;
using System;

public class HandTracker : MonoBehaviour
{
    [SerializeField]
    private ServerConnection server;


    [Header("Hands")]
    [SerializeField]
    private Hand leftHand;
    [SerializeField]
    private Hand rightHand;

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

        Debug.Log(leftHand.GetData());
        server.SendJson(new HandsData
        {
            type = "hands",
            left = leftHand.GetData(),
            right = rightHand.GetData(),
        });
    }

    [Serializable]
    class HandsData
    {
        public string type;
        public HandDataAsset left;
        public HandDataAsset right;
    }
}
