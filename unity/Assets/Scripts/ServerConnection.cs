using NativeWebSocket;
using System;
using System.Text;
using UnityEngine;
using TMPro;
using Meta.WitAi.TTS.Utilities;
using System.Threading.Tasks;

public class ServerConnection : MonoBehaviour
{
    [SerializeField]
    private Puppeter puppeter;
    [SerializeField]
    private TTSSpeaker tts;

    private WebSocket websocket;
    private readonly string serverUrl = "wss://helpful-blatantly-koi.ngrok-free.app/meta";
    [SerializeField] private TMP_Text currentLetter; // ✅ UI Text for WebSocket status
    [SerializeField] public TMP_Text aiTextReponse; // ✅ UI Text for WebSocket status
    [SerializeField] private AudioSource audioSource; // ✅ Audio player
    [SerializeField] private AudioClip highlightSound; // ✅ Assignable sound clip



    private async void Start()
    {
        websocket = new WebSocket(serverUrl);
        websocket.OnOpen += () =>
        {
            Debug.Log("✅ Server Connected");
        };

        websocket.OnClose += (code) =>
        {
            Debug.Log("❌ Server Disconnected: " + code);
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
            case "talk":
                var talk = JsonUtility.FromJson<TalkJsonData>(message);
                StartCoroutine(tts.SpeakAsync(talk.text));
                aiTextReponse.text = talk.text;
                break;
            case "letter":
                var nextLetter = JsonUtility.FromJson<LetterJsonData>(message);
                UpdateLetter(nextLetter);
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
        if (!int.TryParse(message.position, out int position))
        {
            Debug.LogError($"Invalid position value: {message.position}");
            return;
        }

        currentLetter.text = $"<color=green>{message.word[..position]}</color>" + message.word[position..];
        if (position == message.word.Length)
        {
            audioSource.PlayOneShot(highlightSound);
            RequestNextWord();
        }
    }

    async void RequestNextWord()
    {
        await Task.Delay(1500);
        SendJson(new JsonData
        {
            type = "start_alphabet",
        });
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
    private class TalkJsonData : JsonData
    {
        public string text;
    }
}
