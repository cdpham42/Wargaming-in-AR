// Shared Space Project 2
// Booz Allen Hamilton, Seattle Summer Games Team
// Casey Pham
// July 31st, 2017
// Script attached to units to track ownership and abstract movement on fofline 
// DEPRECATED

using UnityEngine;

namespace GameUnit {

    [System.Serializable]
    public class Unit : MonoBehaviour
    {
        [SerializeField]
        public int Owner;

        public static Unit unitPrefab;

        // Attempting to be able to pass position to unit when instantiating so that unit
        // instantiates at mouse point
        
        public Vector3 Position {
            get
            {
                return position;
            }
            set
            {
                position = value;
                transform.localPosition = value;
            }
        }

        private Vector3 position;

        // Unit Type - Will determine model and unit rules
        //public enum UnitType { Destroyer, Carrier, Airplane };
        //private UnitType Type;

        public void Die()
        {
            Destroy(gameObject);
        }

        public void DoMove(Vector3 NewPosition)
        {
            // Account for object height; Move so object is above board and not through it
            NewPosition.y = 0.05f;

            this.Position = NewPosition;
        }

        


    }


}
