using HoloToolkit.Sharing;
using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Broadcasts the hologram transform of the local user to other users in the session,
/// and adds and updates the hologram transforms of remote users.
/// Hologram transforms are sent and received in the local coordinate space of the GameObject
/// this component is on.
/// </summary>
public class RemoteHologramManager : Singleton<RemoteHologramManager>
{
    public class RemoteHologramInfo
    {
        public int ObjectID;
        public GameObject HologramObject;

    }

    private ImportExportAnchorManager anchorManager;
    
    /// <summary>
    /// Keep a list of holograms, indexed by long
    /// </summary>
    Dictionary<long, RemoteHologramInfo> remoteHolograms = new Dictionary<long, RemoteHologramInfo>();

    void Start()
    {
        anchorManager = ImportExportAnchorManager.Instance;
        // unique index for each object (not sync'd? TODO: Check)
        foreach (Transform child in transform)
        {
            // create new holoInfo
            RemoteHologramInfo HoloInfo = new RemoteHologramInfo();
            HoloInfo.ObjectID = child.gameObject.GetInstanceID();
            HoloInfo.HologramObject = child.gameObject;
            Debug.Log("Initialized hologram in remoteHolograms, objectID: " + HoloInfo.ObjectID);
            
            // add hologram to the dictionary
            remoteHolograms.Add(HoloInfo.ObjectID, HoloInfo);
        }
        //remoteHolograms.Add(this.child);
        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.HologramTransform] = this.UpdateHologramTransform;

        SharingSessionTracker.Instance.SessionJoined += Instance_SessionJoined;
        SharingSessionTracker.Instance.SessionLeft += Instance_SessionLeft;
    }

    void Update()
    {
        // don't do anything if anchor not established
        if (!anchorManager.AnchorEstablished) { return; }
#if !UNITY_WSA
        // broadcast if you are the host
        if (LocalUserHasLowestUserId()) {
            Debug.Log("Have lowest userID, broadcasting all object transforms");
            foreach (int key in remoteHolograms.Keys)
            {
                Debug.Log("Handling key: " + key);
                // Grab the current hologram transform and broadcast it to all the other users in the session
                Transform hologramTransform = remoteHolograms[key].HologramObject.transform;
                Debug.Log("Broadcasting hologram at: " + hologramTransform.localPosition.ToString());
                // Transform the hologram position and rotation from world space into local space
                Vector3 hologramPosition = this.transform.InverseTransformPoint(hologramTransform.position);
                Quaternion hologramRotation = Quaternion.Inverse(this.transform.rotation) * hologramTransform.rotation;


                CustomMessages.Instance.SendHologramTransform(hologramPosition, hologramRotation, key);
            }

        }
#endif
    }

    /// <summary>
    /// Called when an existing user leaves the session.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Instance_SessionLeft(object sender, SharingSessionTracker.SessionLeftEventArgs e)
    {
        /*
        if (e.exitingUserId != SharingStage.Instance.Manager.GetLocalUser().GetID())
        {
            RemoveRemoteHologram(this.remoteHolograms[e.exitingUserId].HologramObject);
            this.remoteHolograms.Remove(e.exitingUserId);
        }*/
        Debug.Log("User left session");
    }

    /// <summary>
    /// Called when a remote user is joins the session.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Instance_SessionJoined(object sender, SharingSessionTracker.SessionJoinedEventArgs e)
    {
        /*
        if (e.joiningUser.GetID() != SharingStage.Instance.Manager.GetLocalUser().GetID())
        {
            GetRemoteHologramInfo(e.joiningUser.GetID());
        }*/
        Debug.Log("User joined session");
    }

    /// <summary>
    /// Gets the data structure for the remote users' hologram position.
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public RemoteHologramInfo GetRemoteHologramInfo(int objectID)                  // TODO: Only creates primitive cubes
    {
        RemoteHologramInfo hologramInfo;
        Debug.Log("entered GetRemoteHologramInfo");
        // Get the hologram info if its already in the list, otherwise add it
        if (!this.remoteHolograms.TryGetValue(objectID, out hologramInfo))
        {
            Debug.Log("Hologram not found, adding new hologram");
            //Debug.Log("objectID: " + objectID.ToString());
            hologramInfo = new RemoteHologramInfo();
            hologramInfo.ObjectID = objectID;
            hologramInfo.HologramObject = CreateRemoteHologram();

            this.remoteHolograms.Add(objectID, hologramInfo);
        }

        return hologramInfo;
    }

    /// <summary>
    /// Called when a remote user sends a hologram transform.
    /// </summary>
    /// <param name="msg"></param>
    void UpdateHologramTransform(NetworkInMessage msg)
    {
        Debug.Log("Entered UpdateHologramTransform");
        // Parse the message
        long userID = msg.ReadInt64();  // first part is user ID
        //Debug.Log("Finished reading long userID: " + userID.ToString());

        //int objID = msg.ReadInt32(); // second part is object ID
        //Debug.Log("Finished reading int ojID: " + objID.ToString());

        Vector3 hologramPos = CustomMessages.Instance.ReadVector3(msg);
        //Debug.Log("Finished reading hologramPos: " + hologramPos.ToString());

        Quaternion hologramRot = CustomMessages.Instance.ReadQuaternion(msg);
        //Debug.Log("Finished reading hologramRot: " + hologramRot.ToString());

        RemoteHologramInfo hologramInfo = GetRemoteHologramInfo(0);
        hologramInfo.HologramObject.transform.localPosition = hologramPos;
        hologramInfo.HologramObject.transform.localRotation = hologramRot;
    }

    /// <summary>
    /// Creates a new game object to represent the user's hologram.
    /// </summary>
    /// <returns></returns>
    GameObject CreateRemoteHologram()
    {
        Debug.Log("Creating a RemoteHologram");
        GameObject newHologramObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newHologramObj.transform.parent = this.gameObject.transform;
        newHologramObj.transform.localScale = Vector3.one * 1f; // scaling factor of holograms made
        return newHologramObj;
    }

    /// <summary>
    /// Call to destroy a RemoteHologram object
    /// </summary>
    /// <param name="remoteHologramObject"></param>
	void RemoveRemoteHologram(GameObject remoteHologramObject)
    {
        DestroyImmediate(remoteHologramObject);
    }

    /// <summary>
    /// Returns true if you are the first user joined in the session (lowest user ID)
    /// </summary>
    /// <returns></returns>
    private bool LocalUserHasLowestUserId()
    {
        for (int i = 0; i < SharingSessionTracker.Instance.UserIds.Count; i++)
        {
            if (SharingSessionTracker.Instance.UserIds[i] < CustomMessages.Instance.localUserID)
            {
                return false;
            }
        }

        return true;
    }
}