using System;
using UnityEngine;
using Oculus.Interaction.Input;
using System.Linq;

public class FakeHandDataSource : MonoBehaviour, IHand
{
    private Pose[] _poses;
    private Quaternion _rootRotation;

    public void SetCurrentPoses(ReadOnlyHandJointPoses poses, Quaternion rootRotation)
    {
        _poses = poses.ToArray();
        _rootRotation = rootRotation;
        _currentDataVersion += 1;
        WhenHandUpdated.Invoke();
    }


    [SerializeField]
    private Handedness _handeness = Handedness.Left;
    public Handedness Handedness => _handeness;

    public bool IsConnected => _poses.Length > 0;

    public bool IsHighConfidence => true;

    public bool IsDominantHand => Handedness == Handedness.Right;

    public float Scale => 1f;

    public bool IsPointerPoseValid => false;

    public bool IsTrackedDataValid => true;

    private int _currentDataVersion = 0;
    public int CurrentDataVersion => _currentDataVersion;

    public event Action WhenHandUpdated;

    public bool GetFingerIsHighConfidence(HandFinger finger)
    {
        return IsHighConfidence;
    }

    public bool GetFingerIsPinching(HandFinger finger)
    {
        return false;
    }

    public float GetFingerPinchStrength(HandFinger finger)
    {
        return 0f;
    }

    public bool GetIndexFingerIsPinching()
    {
        return false;
    }

    public bool GetJointPose(HandJointId handJointId, out Pose pose)
    {
        if ((int)handJointId >= _poses.Length)
        {
            pose = Pose.identity;
            return false;
        }

        pose = _poses[(int)handJointId];
        return true;
    }

    public bool GetJointPoseFromWrist(HandJointId handJointId, out Pose pose)
    {
        // Bad
        if ((int)handJointId >= _poses.Length)
        {
            pose = Pose.identity;
            return false;
        }

        pose = _poses[(int)handJointId];
        return true;
    }

    public bool GetJointPoseLocal(HandJointId handJointId, out Pose pose)
    {
        // Bad
        if ((int)handJointId >= _poses.Length)
        {
            pose = Pose.identity;
            return false;
        }

        pose = _poses[(int)handJointId];
        return true;
    }

    public bool GetJointPosesFromWrist(out ReadOnlyHandJointPoses jointPosesFromWrist)
    {
        // Bad
        jointPosesFromWrist = new ReadOnlyHandJointPoses(_poses);
        return true;
    }

    public bool GetJointPosesLocal(out ReadOnlyHandJointPoses localJointPoses)
    {
        localJointPoses = new ReadOnlyHandJointPoses(_poses);
        return true;
    }

    public bool GetPalmPoseLocal(out Pose pose)
    {
        if (_poses.Length == (int)HandJointId.HandPalm)
        {
            pose = Pose.identity;
            return false;
        }

        pose = _poses[(int)HandJointId.HandPalm];
        return true;
    }

    public bool GetPointerPose(out Pose pose)
    {
        if (_poses.Length == (int)HandJointId.HandIndexTip)
        {
            pose = Pose.identity;
            return false;
        }

        pose = _poses[(int)HandJointId.HandIndexTip];
        return true;
    }

    public bool GetRootPose(out Pose pose)
    {
        if (_poses.Length == (int)HandJointId.HandPalm)
        {
            pose = Pose.identity;
            return false;
        }

        pose = _poses[(int)HandJointId.HandPalm];
        pose.rotation = _rootRotation;
        pose.position = gameObject.transform.position;
        return true;
    }
}
