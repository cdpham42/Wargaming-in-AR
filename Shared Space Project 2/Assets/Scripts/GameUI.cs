// Shared Space Project 2
// Booz Allen Hamilton, Seattle Summer Games Team
// Tremaine Ng, Casey Pham, Connor Hughes
// July 31st, 2017
// Implements all player input behavior when attached to a player prefab object
// to be instantiated on player join by UNET's NetworkManager

using System;
using UnityEngine;
using GameUnit;
using UnityEngine.Networking;

public class GameUI : NetworkBehaviour
{
    /// <summary>
    /// Fetches KeyCommand Strings (e.g. "RedPawn") from Unity Editor assigned values.
    /// Ensures valid value is passed
    /// </summary>
    /// <return>
    /// Returns the command string if it is assigned, "Error" if empty or null
    /// </return>
    [Serializable]
    public struct KeyCommand
    {
        /// <summary>
        /// DO NOT USE FOR SCRIPTING
        /// Use GetCommand instead
        /// This field exists to be assigned by the unity editor and is unsafe for access
        /// </summary>
        public string Command;
        private string GetCommand
        {
            get
            {
                if (Command != null && Command != "")
                {
                    return Command;
                }
                else
                {
                    Debug.LogError("Key \"" + Command + "\" not assigned to proper value. Check GameUI in Unity Editor.");
                    return Command;
                }
            }
            set
            {
                Command = value;
            }
        }
    }
    // KeyCommand keys to retrieve from SpawnableList Dictionary
    public KeyCommand Key1Command;
    public KeyCommand Key2Command;
    public KeyCommand Key3Command;
    public KeyCommand Key4Command;
    public KeyCommand Key5Command;
    public KeyCommand Key6Command;
    public KeyCommand Key7Command;
    public KeyCommand Key8Command;
    public KeyCommand Key9Command;
    public KeyCommand Key0Command;
    public KeyCommand KeyMinusCommand;
    public KeyCommand KeyEqualsCommand;
    public KeyCommand KeyQCommand;
    public KeyCommand KeyWCommand;
    public KeyCommand KeyECommand;
    public KeyCommand KeyRCommand;
    public KeyCommand KeyTCommand;
    public KeyCommand KeyYCommand;

    GameObject Board;

    SpawnableList SpawnList;

    SyncUnit selectedUnit;
    GameObject highlightedObject;
    GameObject selectedObject;
    GameObject selectedCube;
    GameObject movementHighlight;

    bool selected = false;

    Renderer highlightedObjectRenderer = null;
    Renderer selectedObjectRenderer = null;

    public Material highlightMaterial;
    public Material selectMaterial;
    
    BaseMaterial highlightBaseMaterial;
    BaseMaterial selectedBaseMaterial;

    // save layer mask values for easy access
    private int boardLayer;
    private int unitLayer;
    private int blueLayer;
    private int redLayer;
    private int allLayer;

    private void Start()
    {
        //Board = FindObjectOfType<HoloToolkit.Unity.SpatialMapping.PlaceableBoard>().gameObject;
        Board = FindObjectOfType<SyncBoard>().gameObject;
        SpawnList = FindObjectOfType<SpawnableList>();
        if (SpawnList != null) Debug.Log("SpawnList successfully loaded");

        if (!UnityEngine.VR.VRDevice.isPresent)
        {
            Camera.main.transform.position = new Vector3(0f, 1.5f, 0f);
            Camera.main.transform.rotation = Quaternion.LookRotation(-Board.transform.up);
        }

        boardLayer = LayerMask.GetMask("Board");
        unitLayer = LayerMask.GetMask("Units");
        blueLayer = LayerMask.GetMask("Blue");
        redLayer = LayerMask.GetMask("Red");
        allLayer = LayerMask.GetMask("All");
    }

    void Update()
    {
        // MOUSE COMMANDS ====================================================================== //
        // 
        // Left Click

        RaycastHit highlightHit = new RaycastHit();
        Ray highlightRay;
        highlightRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        // If Camera ray hits a unit, which are constrained to specified layers
        if (GetDeviceRaycast(out highlightHit, float.PositiveInfinity, redLayer | blueLayer | allLayer | unitLayer))
        {
            //Debug.Log("Hit Object");
            Highlight(highlightHit);
        }

        else if (Physics.Raycast(highlightRay, out highlightHit, float.PositiveInfinity, boardLayer))
        {
            DeHighlight();
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            SelectUnit();
        }
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.M))
        {
            // Right Click while Unit selected
            if (selected)
            {
                Vector3 toPos = GetRaycastPosition(LayerMask.GetMask("Board"));
                toPos = Board.transform.InverseTransformPoint(toPos);
                selectedUnit.DoMove(toPos, Camera.main.transform.rotation);

                selectedObjectRenderer.material = selectedBaseMaterial.material;
                selected = false;
                selectedUnit.selected = false;
                if (movementHighlight != null)
                {
                    movementHighlight.SetActive(false);
                    movementHighlight = null;
                }
            }


            // Right Click, no unit selected
            else
            {
                GetRaycastPosition(boardLayer);
            }
        }
        
        // KEYBOARD COMMANDS =================================================================== //
        //
        // Press U to spawn unit; Shift+U to destroy unit
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CreateUnit(Key1Command.Command);      
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CreateUnit(Key2Command.Command);   
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
			CreateUnit(Key3Command.Command);  
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
			CreateUnit(Key4Command.Command);  
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
			CreateUnit(Key5Command.Command);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
			CreateUnit(Key6Command.Command);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
			CreateUnit(Key7Command.Command);
		}
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            CreateUnit(Key8Command.Command);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CreateUnit(Key9Command.Command);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CreateUnit(Key0Command.Command);
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            CreateUnit(KeyMinusCommand.Command);
        }
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            CreateUnit(KeyEqualsCommand.Command);
        }
		if (Input.GetKeyDown(KeyCode.Delete))
		{
			DestroyUnit();
		}

        if (Input.GetKeyDown(KeyCode.B))
        {
            Camera.main.cullingMask = ~redLayer;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Camera.main.cullingMask = ~blueLayer;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Camera.main.cullingMask = -1;
        }

        //        // Change Owner to 1
        //        if (Input.GetKeyDown(KeyCode.Alpha1))
        //        {
        //            if (selected)
        //            {
        //                selectedUnit.Owner = 1;
        //            }
        //        }

        //        // Change Owner to 2
        //        if (Input.GetKeyDown(KeyCode.Alpha2))
        //        {
        //            if (selected)
        //            {
        //                selectedUnit.Owner = 2;
        //            }
        //        }
    }

    private void SelectUnit()
    {
        RaycastHit hit = new RaycastHit();
        // TODO: Single bar operator is weird and shouldn't work. Fix later
        if (GetDeviceRaycast(out hit, float.PositiveInfinity, unitLayer | blueLayer | redLayer | allLayer))
        {
            //Debug.Log(hit.collider.transform.parent.gameObject.name);
            //Debug.Log(hit.collider.transform.parent.gameObject);

            if (selected)
            {
                if (selectedObjectRenderer != null)
                {
                    selectedObjectRenderer.material = selectedBaseMaterial.material;
                }

                if(movementHighlight != null)
                {
                movementHighlight.SetActive(false);
                movementHighlight = null;
                }

                selected = false;
            }

            // Set selected unit to gameobject clicked
            selectedObject = hit.collider.transform.parent.gameObject;
            selectedUnit = selectedObject.GetComponent<SyncUnit>();
            selectedCube = selectedObject.transform.GetChild(0).gameObject;

            // Unit highlight on select
            selectedObjectRenderer = selectedObject.transform.GetChild(0).GetComponent<Renderer>();
            selectedBaseMaterial = selectedObject.transform.GetChild(0).GetComponent<BaseMaterial>();
            selectedObjectRenderer.material = selectMaterial;

            // A unit is selected
            selected = true;
            selectedUnit.selected = true;
            //Debug.Log(selected);

            // Movement Circle
            try
            {
                movementHighlight = selectedObject.transform.Find("Cylinder").gameObject;
            }
            catch (NullReferenceException)
            {
                movementHighlight = null;
            }
            if (movementHighlight != null)
            {
                movementHighlight.SetActive(true);
            }
        }
        else if (selected)  // If an invalid object or no object is clicked, deselect.
        {
            selected = false;
            if (selectedUnit != null)
            {
                selectedObjectRenderer.material = selectedBaseMaterial.material;
                selectedUnit.selected = false;
                selectedObjectRenderer = null;
                selectedObject = null;
                selectedUnit = null;

                if (movementHighlight != null)
                {
                    movementHighlight.SetActive(false);
                    movementHighlight = null;
                }
            }
        }
    }

    /// <summary>
    /// Gets world position fo a raycast across all layers
    /// </summary>
    /// <returns>returns world position</returns>
    public Vector3 GetRaycastPosition()
    {
        return GetRaycastPosition(-1);
    }

    /// <summary>
    /// Gets world position of a raycast across the given layer
    /// </summary>
    /// <returns>World Position</returns>
    public Vector3 GetRaycastPosition(int layerMask)
    {
        RaycastHit hit;

        if (GetDeviceRaycast(out hit, layerMask))
        {
            // Return point clicked
            //Debug.Log(hit.point);
            return hit.point;
        }

        // If invalid position clicked, return position 0,0,0
        return Vector3.zero;
    }

    void CreateUnit(string unitToSpawn)
    {
        Vector3 position = GetRaycastPosition(LayerMask.GetMask("Board"));
        // get local position
        //Debug.Log("world: " + position);
        position = Board.transform.InverseTransformPoint(position);
        //Debug.Log("local: " + position);
        CreateUnit(position, unitToSpawn);
    }

    /// <summary>
    /// Creates a Unit at the position given by GetRaycastPosition
    /// </summary>
	void CreateUnit(Vector3 position, string unitToSpawn)
    {
        if (unitToSpawn.Equals("Error"))    // internal error value
        {
            return;
        }

        if (isServer)
        {
            Debug.Log("Server is creating a unit");

            if (!SpawnList.UnitDictionary.ContainsKey(unitToSpawn))
            {
                Debug.LogError("Unit key \"" + unitToSpawn + "\" does not exist! Check SpawnableList in Scene.");
                return;
            }
            GameObject unit = Instantiate(SpawnList.UnitDictionary[unitToSpawn], 
                                            position, Camera.main.transform.rotation, Board.transform);

            SyncUnit unitScript = unit.GetComponent<SyncUnit>();
            if (unitScript == null)
            {
                Debug.LogError("unitPrefab does not contain SyncUnit script!");
                return;
            }

            NetworkServer.SpawnWithClientAuthority(unit, this.gameObject);

            // Redundant but instantiate at position and parent not tested to work correctly yet
            unit.transform.SetParent(Board.transform);
            RpcSetParent(unit, Board);
            unitScript.DoMove(position, Camera.main.transform.rotation);
        }
        else
        {
            Debug.Log("Sending create unit command");
            CmdCreateUnit(position, unitToSpawn);
        }
    }

    [Command]
    private void CmdCreateUnit(Vector3 position, string unitToSpawn)
    {
        // get local position
        if (isClient)
        {
            //Debug.Log("world: " + position);
            position = Board.transform.InverseTransformPoint(position);
            //Debug.Log("local: " + position);
        }
        CreateUnit(position, unitToSpawn);
    }

    /// <summary>
    ///  Tells client to assign a GameObject to the the child of another GameObject
    /// </summary>
    [ClientRpc]
    private void RpcSetParent(GameObject child, GameObject parent)
    {
        if (child == null)
            Debug.LogError("Tried to assign parent to a null child");
        if (parent == null)
            Debug.LogError("Tried to assign null parent to a child");
        child.transform.parent = parent.transform;
    }

    void DestroyUnit()
    {
        SelectUnit();
        DestroyUnit(selectedObject);
    }

    void DestroyUnit(GameObject inputObject)
    {
        if (isServer)
        {
            Destroy(inputObject);
        }
        else
        {
            SelectUnit();
            Cmd_DestroyUnit(selectedObject);
        }
    }

    [Command]
    void Cmd_DestroyUnit(GameObject inputObject)
    {
        DestroyUnit(inputObject);
    }

    public bool GetSelected()
    {
        return selected;
    }

    private void OnDestroy()
    {
        // handles random case of camera being destroyed before gameUI on game close
        if (Camera.main != null)
            Camera.main.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    #region Helpers
    /// <summary>
    /// Gets a RaycastHit at any distance across all layer masks in an out variable
    /// </summary>
    bool GetDeviceRaycast(out RaycastHit hitInfo)
    {
        return GetDeviceRaycast(out hitInfo, float.PositiveInfinity, -1);
    }

    /// <summary>
    /// Gets a RaycastHit at a given distance across all layer masks in an out variable
    /// </summary>
    /// <param name="distance">Max distance for raycast hits</param>
    bool GetDeviceRaycast(out RaycastHit hitInfo, float maxDistance)
    {
        return GetDeviceRaycast(out hitInfo, maxDistance, -1);
    }

    /// <summary>
    /// Gets a RaycastHit across given distance and layermasks in an out variable
    /// </summary>
    /// <param name="masks">Mask values the raycast can hit (-1 for all)</param>
    /// <returns></returns>
    bool GetDeviceRaycast(out RaycastHit hitInfo, int mask)
    {
        return GetDeviceRaycast(out hitInfo, float.PositiveInfinity, mask);
    }

    /// <summary>
    /// Gets a RaycastHit across given distance and layermasks in an out variable
    /// </summary>
    /// <param name="distance">Max distance for raycast hits</param>
    /// <param name="masks">Mask values the raycast can hit (-1 for all)</param>
    bool GetDeviceRaycast(out RaycastHit hitInfo, float maxDistance, int mask)
    {
        Ray ray;
        if (UnityEngine.VR.VRDevice.isPresent)
        {
            ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }
        else
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        }
        // check for empty mask array, default to all masks
        if (mask == -1)
            return Physics.Raycast(ray, out hitInfo, maxDistance);
        else
            return Physics.Raycast(ray, out hitInfo, maxDistance, mask);
    }

    public void Highlight(RaycastHit highlightHit)
    {
        // Prevent constant highlight updates while gazing at single objec
        if (highlightedObject != highlightHit.collider.gameObject)
        {
            // if highlighted object not null to prevent null exception errors on game start
            if (highlightedObject != null)
            {
                // Make old object return to its base material before updating the new object
                highlightedObjectRenderer.material = highlightBaseMaterial.material;
            }

            // Select object camera collides with
            highlightedObject = highlightHit.collider.gameObject;

            // Get object renderer, get its base material, current material, then make its material the new material
            highlightedObjectRenderer = highlightedObject.GetComponent<Renderer>();
            highlightBaseMaterial = highlightedObject.GetComponent<BaseMaterial>();
            highlightedObjectRenderer.material = highlightMaterial;

            //Debug.Log(highlightedObject);
        }
    }

    public void DeHighlight()
    {
        // So on start there is no null reference exception
        if (highlightedObjectRenderer != null)
        {
            // If a unit is selected, determine if the highlighted object is the selected unit and dehighlight accordingly
            if (selected)
            {
                if (selectedCube == highlightedObject)
                {
                    highlightedObjectRenderer.material = selectMaterial;
                    highlightedObject = null;
                    highlightedObjectRenderer = null;
                }
                else
                {
                    highlightedObjectRenderer.material = highlightBaseMaterial.material;
                    highlightedObject = null;
                    highlightedObjectRenderer = null;
                }
            }

            // Else if object isn't selected
            else
            {
                highlightedObjectRenderer.material = highlightBaseMaterial.material;
                highlightedObject = null;
                highlightedObjectRenderer = null;
                //Debug.Log(highlightedObject);
            }
        }
    }
    #endregion
}