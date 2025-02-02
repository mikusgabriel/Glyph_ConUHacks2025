using UnityEngine;
using Oculus.Interaction.Input;
using TMPro;

public class HandTracker : MonoBehaviour
{

    [SerializeField]
    private ServerConnection server;
    [SerializeField] private Hand LeftHand;
    [SerializeField] private Hand RightHand;
    [SerializeField] private OVRCameraRig cameraRig;
    [SerializeField] private TMP_Text uiText; // Reference to UI Text (TextMeshPro)

    void Start()
    {
        Debug.Log(JsonUtility.ToJson(LeftHand.GetData()));

        if (uiText == null)
        {
            Debug.LogError("UI Text is not assigned! Assign a TextMeshPro UI object in the Inspector.");
        }
    }

    void Update()
    {
        if (cameraRig == null)
        {
            Debug.LogError("CameraRig is not assigned! Please assign an OVRCameraRig in the Inspector.");
            return;
        }
        Vector3 handPosition = LeftHand.GetData().Root.position;

        Vector3 headsetPosition = cameraRig.centerEyeAnchor.position;
        Quaternion headsetRotation = cameraRig.centerEyeAnchor.rotation;
        Vector3 euler = headsetRotation.eulerAngles;

        Vector3 delta = handPosition - headsetPosition;

        Quaternion headsetRotat = Quaternion.Euler(euler);

        Vector3 handRelativeToHead = Quaternion.Inverse(headsetRotat) * delta;

        string logText = $"Delta: {delta}\n" +
                         $"Head Rotation {headsetRotat}\n" +
                         $"Hand relative to Head{handRelativeToHead}";

        Vector3 leftH = getNormalizedHand(LeftHand);
        Vector3 rightH = getNormalizedHand(RightHand);

        server.SendJson(new HandsData
        {
            type = "hands_data",
            left = leftH,
            right = rightH,
        });

        Debug.Log(logText);

        // Update UI Text
        if (uiText != null)
        {
            uiText.text = logText;
        }
    }

    Vector3 getNormalizedHand(Hand hand)
    {
        Vector3 handPosition = hand.GetData().Root.position;

        Vector3 headsetPosition = cameraRig.centerEyeAnchor.position;
        Quaternion headsetRotation = cameraRig.centerEyeAnchor.rotation;
        Vector3 euler = headsetRotation.eulerAngles;

        Vector3 delta = handPosition - headsetPosition;

        Quaternion headsetRotat = Quaternion.Euler(euler);

        Vector3 handRelativeToHead = Quaternion.Inverse(headsetRotat) * delta;

        return handRelativeToHead;
    }

    [System.Serializable]
    class HandsData
    {
        public string type;
        public Vector3 left;
        public Vector3 right;
    }
}