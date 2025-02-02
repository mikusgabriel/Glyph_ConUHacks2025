using UnityEngine;
using System;

public class Puppeter : MonoBehaviour
{
    [Header("Hands")]
    [SerializeField]
    private GameObject leftHand;
    [SerializeField]
    private GameObject rightHand;
    [SerializeField]
    private FakeHandDataSource leftHandData;
    [SerializeField]
    private FakeHandDataSource rightHandData;


    [Header("Settings")]
    [SerializeField]
    private float duration = 2f;
    [SerializeField]
    private float animationCooldown = 2f;

    private LTDescr leftAnimation;
    private LTDescr rightAnimation;


    public void StartAnimateLeftHand(Pose[] inital, Pose[] final)
    {
        leftHand.SetActive(true);
        leftAnimation = LeanTween.value(0, 1, duration)
        .setLoopPingPong()
        .setDelay(animationCooldown)
        .setEase(LeanTweenType.easeInOutQuad)
        .setOnUpdate((float finalRatio) =>
        {
            float initialRatio = 1 - finalRatio;
            Pose[] data = new Pose[inital.Length];

            for (int i = 0; i < inital.Length; i++)
            {
                Vector3 pos = initialRatio * inital[i].position + finalRatio * final[i].position;
                Vector3 rot = initialRatio * inital[i].rotation.eulerAngles + finalRatio * final[i].rotation.eulerAngles;
                data[i] = new Pose(pos, Quaternion.Euler(rot));
            }

            leftHandData.SetCurrentPoses(data, data[1]);
        });
    }

    public void StartAnimateRightHand(Pose[] inital, Pose[] final)
    {
        rightHand.SetActive(true);
        rightAnimation = LeanTween.value(0, 1, duration)
        .setLoopPingPong()
        .setDelay(animationCooldown)
        .setEase(LeanTweenType.easeInOutQuad)
        .setOnUpdate((float finalRatio) =>
        {
            float initialRatio = 1 - finalRatio;
            Pose[] data = new Pose[inital.Length];

            for (int i = 0; i < inital.Length; i++)
            {
                Vector3 pos = initialRatio * inital[i].position + finalRatio * final[i].position;
                Vector3 rot = initialRatio * inital[i].rotation.eulerAngles + finalRatio * final[i].rotation.eulerAngles;
                data[i] = new Pose(pos, Quaternion.Euler(rot));
            }

            rightHandData.SetCurrentPoses(data, data[1]);
        });
    }

    public void StopAnimateLeftHand()
    {
        leftHand.SetActive(false);
        if (leftAnimation != null)
        {
            leftAnimation.pause();
            LeanTween.cancel(leftAnimation.uniqueId, false);
            leftAnimation = null;
        }
    }

    public void StopAnimateRightHand()
    {
        rightHand.SetActive(false);
        if (rightAnimation != null)
        {
            rightAnimation.pause();
            LeanTween.cancel(rightAnimation.uniqueId, false);
            rightAnimation = null;
        }
    }

    [Serializable]
    class MessageData
    {
        public string type;
        public string occupation;
    }
}
