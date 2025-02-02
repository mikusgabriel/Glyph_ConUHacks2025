using UnityEngine;
using Oculus.Interaction.Input;
using TMPro;
using Unity.VisualScripting;

public class HandTracker : MonoBehaviour
{

    [SerializeField]
    private ServerConnection server;
    [SerializeField] private Hand LeftHand;
    [SerializeField] private Hand RightHand;
    [SerializeField] private OVRCameraRig cameraRig;


    void Update()
    {
        Vector3 leftH = getNormalizedHand(LeftHand);
        Vector3 rightH = getNormalizedHand(RightHand);
        // Debug.Log($"{leftH}");
        server.SendJson(new HandsData
        {
            type = "hands_data",
            left = leftH,
            right = rightH,
        });
    }

    Vector3 getNormalizedHand(Hand hand)
    {
        Vector3 handPosition = hand.GetData().Root.position;

        cameraRig.centerEyeAnchor.GetPositionAndRotation(out Vector3 headsetPosition, out Quaternion headsetRotation);
        Vector3 euler = headsetRotation.eulerAngles;

        Vector3 delta = handPosition - headsetPosition;

        Quaternion headsetRotat = Quaternion.Euler(euler);

        Vector3 handRelativeToHead = Quaternion.Inverse(headsetRotat) * delta;

        return handRelativeToHead;
    }

    void Record(Hand left, Hand right)
    {

    }

    [System.Serializable]
    class HandsData
    {
        public string type;
        public Vector3 left;
        public Vector3 right;
    }
}