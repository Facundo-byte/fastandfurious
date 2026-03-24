using UnityEngine;

public class centerofmass : MonoBehaviour
{
    public Transform comDebug;

    void Update()
    {
        if (comDebug != null)
            comDebug.position = GetComponent<Rigidbody>().worldCenterOfMass;
    }
}
