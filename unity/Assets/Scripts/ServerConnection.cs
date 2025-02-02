using NativeWebSocket;
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ServerConnection : MonoBehaviour
{
    [SerializeField]
    private Puppeter puppeter;

    private WebSocket websocket;
    private readonly string serverUrl = "wss://helpful-blatantly-koi.ngrok-free.app/meta";


    private async void Start()
    {
        websocket = new WebSocket(serverUrl);

        websocket.OnOpen += () => Debug.Log("Server Connected");
        websocket.OnClose += (code) => Debug.Log("Server Disconnected: " + code);
        websocket.OnError += (error) => Debug.LogError("WebSocket Error: " + error);
        websocket.OnMessage += (bytes) =>
        {
            string message = Encoding.UTF8.GetString(bytes);
            Debug.Log("Received: " + message);
            var json = JsonUtility.FromJson<JsonData>(message);
            OnMessageReceived(json.type, message);
        };

        await websocket.Connect();
    }


    private async void OnDestroy()
    {
        if (websocket != null)
        {
            await websocket.Close();
        }
    }

    private async void Update()
    {
        if (websocket.State == WebSocketState.Closed || websocket.State == WebSocketState.Closing)
        {
            Debug.Log("Reconnecting...");
            await websocket.Connect();
        }

#if !UNITY_WEBGL || UNITY_EDITOR
        if (websocket.State == WebSocketState.Open)
        {
            websocket.DispatchMessageQueue();
        }
#endif
    }

    private void OnMessageReceived(string type, string message)
    {
        switch (type)
        {
            case "spawn":
                var data = JsonUtility.FromJson<SpawnJsonData>(message);
                break;
        }
    }

    public async void SendJson(object obj)
    {
        if (websocket.State == WebSocketState.Open)
        {
            string message = JsonUtility.ToJson(obj);
            await websocket.SendText(message);
            Debug.Log("Sent to WebSocket: " + message);
        }
        else
        {
            Debug.Log("Unable to send message to WebSocket.");
        }
    }

    [Serializable]
    private class JsonData
    {
        public string type;
    }

    [Serializable]
    private class SpawnJsonData : JsonData
    {
        public string name;
        public int x;
        public int y;
        public string meshyId;
    }
}
