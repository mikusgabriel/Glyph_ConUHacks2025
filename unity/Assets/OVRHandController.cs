using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.Input;
using TMPro;

[System.Serializable]
public class JointData
{
    public int id; // Bone ID
    public Vector3 position;
    public Quaternion rotation;
}

[System.Serializable]
public class HandData
{
    public List<JointData> joints;
}

public class OVRHandController : MonoBehaviour
{
    private OVRHand hand;
    private OVRSkeleton skeleton;
    [SerializeField] private TMP_Text debugText; // Optional UI debugging text
    [SerializeField] private float animationSpeed = 1.5f; // Speed of animation

    private Dictionary<OVRSkeleton.BoneId, Transform> jointTransforms = new Dictionary<OVRSkeleton.BoneId, Transform>();
    private List<HandData> animationFrames = new List<HandData>(); // Animation frames
    private int currentFrameIndex = 0; // Current animation frame

    void Start()
    {
        hand = GetComponent<OVRHand>();
        skeleton = GetComponent<OVRSkeleton>();

        if (hand == null || skeleton == null)
        {
            Debug.LogError("OVRHand or OVRSkeleton not found on " + gameObject.name);
            return;
        }

        StartCoroutine(InitializeJoints());
    }

    IEnumerator InitializeJoints()
    {
        while (!skeleton.IsInitialized)
            yield return null;

        foreach (var bone in skeleton.Bones)
        {
            jointTransforms[bone.Id] = bone.Transform;
        }

        Debug.Log($"Hand Joints Initialized for {hand}");

        // Load animation frames
        LoadAnimationFrames();

        // Start animation loop
        StartCoroutine(LoopAnimation());
    }

    void LoadAnimationFrames()
    {
        // Define multiple hand poses as JSON
        string jsonPose1 = @"{""joints"": [
            { ""id"": 0, ""position"": { ""x"": 0.0, ""y"": 1.0, ""z"": 0.0 }, ""rotation"": { ""x"": 0.0, ""y"": 0.0, ""z"": 0.0, ""w"": 1.0 } },
            { ""id"": 8, ""position"": { ""x"": 0.1, ""y"": 1.1, ""z"": -0.1 }, ""rotation"": { ""x"": 0.2, ""y"": 0.1, ""z"": 0.0, ""w"": 1.0 } }
        ]}";

        string jsonPose2 = @"{""joints"": [
            { ""id"": 0, ""position"": { ""x"": 0.0, ""y"": 1.0, ""z"": 0.0 }, ""rotation"": { ""x"": 0.0, ""y"": 0.0, ""z"": 0.0, ""w"": 1.0 } },
            { ""id"": 8, ""position"": { ""x"": 0.2, ""y"": 1.2, ""z"": -0.2 }, ""rotation"": { ""x"": 0.4, ""y"": 0.2, ""z"": 0.0, ""w"": 1.0 } }
        ]}";

        // Convert JSON to hand animation frames
        animationFrames.Add(JsonUtility.FromJson<HandData>(jsonPose1));
        animationFrames.Add(JsonUtility.FromJson<HandData>(jsonPose2));

        Debug.Log($"Loaded Animation Frames: {animationFrames.Count}");
    }

    IEnumerator LoopAnimation()
    {
        while (true)
        {
            HandData currentFrame = animationFrames[currentFrameIndex];
            HandData nextFrame = animationFrames[(currentFrameIndex + 1) % animationFrames.Count];

            float elapsedTime = 0f;
            while (elapsedTime < animationSpeed)
            {
                ApplyInterpolatedPose(currentFrame, nextFrame, elapsedTime / animationSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentFrameIndex = (currentFrameIndex + 1) % animationFrames.Count;
        }
    }

    void ApplyInterpolatedPose(HandData startPose, HandData endPose, float t)
    {
        foreach (var joint in startPose.joints)
        {
            OVRSkeleton.BoneId boneId = (OVRSkeleton.BoneId)joint.id;
            if (jointTransforms.ContainsKey(boneId))
            {
                Transform jointTransform = jointTransforms[boneId];

                // Find the matching joint in endPose
                JointData endJoint = endPose.joints.Find(j => j.id == joint.id);
                if (endJoint != null)
                {
                    jointTransform.localPosition = Vector3.Lerp(joint.position, endJoint.position, t);
                    jointTransform.localRotation = Quaternion.Slerp(joint.rotation, endJoint.rotation, t);
                }
            }
        }
    }
}