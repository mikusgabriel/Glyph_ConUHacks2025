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


    public void StartAnimateLeftHand(Pose[] inital, Pose[] final, Pose initialRoot, Pose finalRoot)
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

            Vector3 posR = initialRatio * initialRoot.position + finalRatio * finalRoot.position;
            Vector3 rotR = initialRatio * initialRoot.rotation.eulerAngles + finalRatio * finalRoot.rotation.eulerAngles;
            Pose root = new Pose(posR, Quaternion.Euler(rotR));

            leftHandData.SetCurrentPoses(data, root);
        });
    }

    public void StartAnimateRightHand(Pose[] inital, Pose[] final, Pose initialRoot, Pose finalRoot)
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

            Vector3 posR = initialRatio * initialRoot.position + finalRatio * finalRoot.position;
            Vector3 rotR = initialRatio * initialRoot.rotation.eulerAngles + finalRatio * finalRoot.rotation.eulerAngles;
            Pose root = new Pose(posR, Quaternion.Euler(rotR));

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
