using NativeWebSocket;
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering; // Import TextMeshPro for UI

public class ServerConnection : MonoBehaviour
{
    [SerializeField]
    private Puppeter puppeter;

    private WebSocket websocket;
    private readonly string serverUrl = "wss://helpful-blatantly-koi.ngrok-free.app/meta";
    private bool isConnected = false; // ✅ Connection state
    [SerializeField] private TMP_Text connectionStatusText; // ✅ UI Text for WebSocket status
    [SerializeField] private TMP_Text currentLetter; // ✅ UI Text for WebSocket status
    private string old_letter; // ✅ UI Text for WebSocket status
    [SerializeField] private AudioSource audioSource; // ✅ Audio player
    [SerializeField] private AudioClip highlightSound; // ✅ Assignable sound clip



    private async void Start()
    {
        websocket = new WebSocket(serverUrl);
        websocket.OnOpen += () =>
        {
            Debug.Log("✅ Server Connected");
            isConnected = true; // ✅ Set connection status
            UpdateConnectionUI();
        };

        websocket.OnClose += (code) =>
        {
            Debug.Log("❌ Server Disconnected: " + code);
            isConnected = false; // ✅ Update connection state
            UpdateConnectionUI();
        };
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
            Debug.Log("🔄 Reconnecting...");
            await websocket.Connect();
            isConnected = websocket.State == WebSocketState.Open;
            UpdateConnectionUI();
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
            case "letter":
                var nextLetter = JsonUtility.FromJson<LetterJsonData>(message);
                // UpdateLetter(nextLetter);
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

    private void UpdateLetter(LetterJsonData message)
    {
        if (connectionStatusText != null)
        {
            int position;
            if (!int.TryParse(message.position, out position))
            {
                Debug.LogError($"Invalid position value: {message.position}");
            }
            currentLetter.text = $"<color=green>{message.word.Substring(0, position)}</color>" +
                     $"<color=red>{message.word.Substring(position)}</color>";
            if (position == message.word.Length && audioSource != null && highlightSound != null)
            {
                audioSource.PlayOneShot(highlightSound);
            }
        }
    }

    private void UpdateConnectionUI()
    {
        if (connectionStatusText != null)
        {
            connectionStatusText.text = $"WebSocket: {(isConnected ? "<color=green>Connected</color>" : "<color=red>Disconnected</color>")}";
        }
    }


    [Serializable]
    private class JsonData
    {
        public string type;
    }

    [Serializable]
    private class LetterJsonData : JsonData
    {
        public string word;
        public string position;
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
