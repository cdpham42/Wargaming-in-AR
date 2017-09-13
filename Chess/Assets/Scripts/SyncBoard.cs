// Shared Space Project 2
// Booz Allen Hamilton, Seattle Summer Games Team
// Tremaine Ng
// July 31st, 2017
// Synchronizes board location in local coordinates,
// allowing boards to stay up to date as a new anchor
// is downloaded from a sharingService

// Updates every frame. To improve performance, reduce update rate.

using UnityEngine;
using UnityEngine.Networking;

public class SyncBoard : NetworkBehaviour
{
    [SyncVar]
    private Vector3 anchorLocalPos;
    [SyncVar]
    private Quaternion anchorLocalRot;

    private void Awake()
    {
        anchorLocalPos = transform.localPosition;
        anchorLocalRot = transform.localRotation;
    }
    private void Update()
    {
        transform.localPosition = anchorLocalPos;
        transform.localRotation = anchorLocalRot;
    }
}
