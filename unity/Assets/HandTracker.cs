using UnityEngine;
using Oculus.Interaction.Input;

public class HandTracker : MonoBehaviour
{
    [SerializeField]
    private Hand LeftHand;
    [SerializeField]
    private Hand RightHand;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(JsonUtility.ToJson(LeftHand.GetData()));
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(JsonUtility.ToJson(LeftHand.GetData()));

    }
}
