using UnityEngine;
using Oculus.Interaction.Input;
using TMPro;
using Unity.VisualScripting;

public class HandTracker : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField]
    private ServerConnection server;
    [SerializeField]
    private Hand LeftHand;
    [SerializeField]
    private Hand RightHand;
    [SerializeField]
    private OVRCameraRig cameraRig;

    [Header("Settings")]
    [SerializeField]
    private float sendCooldown = 0.2f;
    private float lastTimeDataSent = 0;


    void Update()
    {
        lastTimeDataSent += Time.deltaTime;
        if (lastTimeDataSent < sendCooldown)
            return;
        else
            lastTimeDataSent -= sendCooldown;

        server.SendJson(new HandsData
        {
            type = "hands_data",
            left = GetNormalizedHand(LeftHand),
            right = GetNormalizedHand(RightHand),
        });
    }

    Vector3 GetNormalizedHand(Hand hand)
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
