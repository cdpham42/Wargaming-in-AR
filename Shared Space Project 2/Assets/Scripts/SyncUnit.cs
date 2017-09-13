// Shared Space Project 2
// Booz Allen Hamilton, Seattle Summer Games Team
// Tremaine Ng, Casey Pham
// July 31st, 2017
// Network script used to synchronize all unit information across different users
// i.e. position, rotation, and ownership.

// Updates every frame. To improve performance, reduce update rate.

using UnityEngine;
using UnityEngine.Networking;

namespace GameUnit
{
    public class SyncUnit : NetworkBehaviour
    {
        // represents the unit's localposition on parent object with spatial sharing anchor
        [SyncVar]
        private Vector3 localPosition;
        [SyncVar]
        private Quaternion localRotation;

        [SyncVar]
        public int Owner;

        public static GameObject unitPrefab;    // Should contain SyncUnit script
        public bool selected;
        // Unit Type - Will determine model and unit rules
        //public enum UnitType { Destroyer, Carrier, Airplane };
        //private UnitType Type;

        private void Awake()
        {
            // Using this is a way to initialize position without needing to call DoMove
            // from where you instantiate the object in code.
            localPosition = transform.position;
        }

        private void Update()
        {
            this.transform.localPosition = localPosition;
            this.transform.localRotation = localRotation;
        }

        /// <returns>position of SyncUnit</returns>
        // get method needs to be used because of SyncVar
        public Vector3 GetLocalPosition()
        {
            return this.localPosition;
        }

        public Quaternion GetLocalRotation()
        {
            return this.localRotation;
        }

        /// <summary>
        /// Moves the unit in local coordinates
        /// </summary>
        /// <param name="newPosition">local position to move to</param>
        /// <param name="newRotation">local rotation to change to</param>
        public void DoMove(Vector3 newPosition, Quaternion newRotation)
        {
            if (isServer)
            {
                Debug.Log("before domove" + this.localPosition.ToString());
                // add new displacements to original
                localPosition = newPosition;
                localRotation = newRotation;
                // Keep object upright on board
                localRotation.x = 0;
                localRotation.z = 0;
                Debug.Log("after DoMove" + this.localPosition.ToString());
            }
            else
            {
                CmdDoMove(newPosition, newRotation);
            }
        }

        [Command]
        private void CmdDoMove(Vector3 newPosition, Quaternion newRotation)
        {
            DoMove(newPosition, newRotation);
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Gets position and rotation of object relative to immediate parent's transform
        /// on the client calling this method from a world position and rotation
        /// </summary>
        /// <param name="pos">world position</param>
        /// <param name="rot">world rotation</param>
        /// <param name="localPos">local position</param>
        /// <param name="localRot">local rotation</param>
        private void ConvertLocalPosRot(Vector3 pos, Quaternion rot, out Vector3 localPos, out Quaternion localRot)
        {
            Transform parentTransform = this.transform.parent.transform;
            // convert to local position and rotation from displacement
            localPos = parentTransform.InverseTransformPoint(pos);
            localRot = Quaternion.Inverse(parentTransform.rotation) * rot;
        }

        /// <summary>
        /// Offsets object's transformation to ignore parent 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private Vector3 IgnoreParentScale(Vector3 v)
        {
            Vector3 newVector = new Vector3();
            Transform parent = this.transform.parent;
            newVector.x = v.x / parent.localScale.x;
            newVector.y = v.y / parent.localScale.y;
            newVector.z = v.z / parent.localScale.z;
            return newVector;
        }
    }
}
