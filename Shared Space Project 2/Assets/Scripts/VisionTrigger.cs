// Shared Space Project 2
// Booz Allen Hamilton, Seattle Summer Games Team
// Arvindh Natajaran, Casey Pham
// July 31st, 2017
// Triggers visibility logic based on unit proximity

using System.Collections.Generic;
using UnityEngine;

public class VisionTrigger : MonoBehaviour {

    /// <summary>
    /// Determines when the unit enters another unit's vision and changes layers appropriately
    /// </summary>

    string trigger;

    GameObject sprite;

    // Set layers for use in vision
    int layer;
    int all = 12;

    public List<GameObject> vision = new List<GameObject>();

    void Start()
    {
        // Determine if piece is red or blue; Set trigger and layer appropriately
        if (this.gameObject.CompareTag("Red"))
        {
            trigger = "VisionBlue";
            layer = 11;
            
        }
        else
        {
            trigger = "VisionRed";
            layer = 10;
        }

        sprite = this.gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject;

    }

    void Update()
    {

    }

    // Set layer to all vision layer
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(trigger))
        {
            print("Enter");
            this.gameObject.layer = all;
            sprite.layer = all;

            GameObject seen = other.gameObject;
            vision.Add(seen);

            //if (!vision.Contains(seen))
            //{
            //    vision.Add(seen);
            //}
        }
    }

    // Return layer to original layer
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(trigger))
        {
            print("Exit");
            GameObject seen = other.gameObject;
            vision.Remove(seen);
        }

        if (vision.Count == 0)
        {
            this.gameObject.layer = layer;
            sprite.layer = layer;
        }
    }

}
