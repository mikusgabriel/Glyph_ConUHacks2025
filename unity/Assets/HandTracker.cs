using UnityEngine;
using Oculus.Interaction.Input;
using TMPro; // Import TextMeshPro for UI text

public class HandTracker : MonoBehaviour
{
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

        // Debug.Log(logText);

        // Update UI Text
        if (uiText != null)
        {
            uiText.text = logText;
        }
    }

    Vector3 ConvertPos(Vector3 CamPos)
    {
        if (LeftHand == null)
        {
            Debug.LogError("LeftHand is not assigned!");
            return Vector3.zero;
        }

        var handData = LeftHand.GetData();
        if (handData == null)
        {
            Debug.LogError("Hand data is null!");
            return Vector3.zero;
        }

        Vector3 handRootPosition = handData.Root.position;

        return handRootPosition - CamPos; // Optimized Vector3 subtraction
    }

    Vector2 ComputeSignedDistances(float theta, float x, float y)
    {
        float a = Mathf.Tan((Mathf.PI / 2) - 0.000001f - theta);
        float dPar = (x + a * y) / Mathf.Sqrt(1 + a * a);
        float dPerp = (x - (1 / a) * y) / Mathf.Sqrt(1 + (1 / (a * a)));

        return new Vector2(dPar, dPerp);
    }
}