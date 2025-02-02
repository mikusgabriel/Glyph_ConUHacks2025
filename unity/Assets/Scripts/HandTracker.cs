using UnityEngine;
using Oculus.Interaction.Input;
using TMPro;
using Unity.VisualScripting;
using Oculus.Interaction;
using System.Linq;
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
    private float sendCooldown = 0.05f;
    private float lastTimeDataSent = 0;


    void Update()
    {
        lastTimeDataSent += Time.deltaTime;
        if (lastTimeDataSent < sendCooldown)
            return;
        else
            lastTimeDataSent -= sendCooldown;

        cameraRig.centerEyeAnchor.GetPositionAndRotation(out Vector3 headsetPosition, out Quaternion headsetRotation);

        Vector3 LeftRoothandPosition = LeftHand.GetData().Root.position;
        Vector3 RightRoothandPosition = RightHand.GetData().Root.position;

        Quaternion RightRoothandRotation = RightHand.GetData().Root.rotation;
        Quaternion LeftRoothandRotation = LeftHand.GetData().Root.rotation;

        Pose[] lefthandJointsPositions = LeftHand.GetData().JointPoses;
        Pose[] righthandJointsPositions = RightHand.GetData().JointPoses;

        server.SendJson(new HandsData
        {
            type = "hands_data",
            left = lefthandJointsPositions,
            right = righthandJointsPositions,
            rootL = LeftRoothandPosition,
            rootR = RightRoothandPosition,
            RighthandRotation = RightRoothandRotation,
            LefthandRotation = LeftRoothandRotation,
            headPos = headsetPosition,
            headRot = headsetRotation
        });
    }


    [System.Serializable]
    class HandsData
    {
        public string type;
        public Pose[] left;
        public Pose[] right;
        public Vector3 rootL;
        public Vector3 rootR;

        public Quaternion RighthandRotation;
        public Quaternion LefthandRotation;
        public Vector3 headPos;
        public Quaternion headRot;

    }
}
