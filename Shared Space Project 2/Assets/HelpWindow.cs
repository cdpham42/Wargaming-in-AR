using UnityEngine;
using System.Collections;

public class HelpWindow : MonoBehaviour {
	[SerializeField]
	GUISkin MenuSkin;

	public void OnGUI(){
		GUI.skin = MenuSkin;
		GUI.BeginGroup(new Rect(Screen.width/2-200,Screen.height/2-250,410,510));
		GUI.Box(new Rect(0,10,410,500), "");

		GUI.Label (new Rect (100, 0, 250, 40), "Wargame Command Index");

		GUI.Label (new Rect (120, 35, 140, 40), "Spawning Units:");

		GUI.Label (new Rect (30, 60, 140, 40), "Blue Battleship:");
		GUI.Label (new Rect (180, 60, 180, 40), "Press 1 on Keyboard");
		GUI.Label (new Rect (30, 80, 140, 40), "Blue Submarine:");
		GUI.Label (new Rect (180, 80, 180, 40), "Press 2 on Keyboard");
		GUI.Label (new Rect (30, 100, 140, 40), "Blue Fighter Jet:");
		GUI.Label (new Rect (180, 100, 180, 40), "Press 3 on Keyboard");
		GUI.Label (new Rect (30, 120, 140, 40), "Blue Carrier:");
		GUI.Label (new Rect (180, 120, 180, 40), "Press 4 on Keyboard");

		GUI.Label (new Rect (30, 160, 140, 40), "Red Battleship:");
		GUI.Label (new Rect (180, 160, 180, 40), "Press 5 on Keyboard");
		GUI.Label (new Rect (30, 180, 140, 40), "Red Submarine:");
		GUI.Label (new Rect (180, 180, 180, 40), "Press 6 on Keyboard");
		GUI.Label (new Rect (30, 200, 140, 40), "Red Fighter Jet:");
		GUI.Label (new Rect (180, 200, 180, 40), "Press 7 on Keyboard");
		GUI.Label (new Rect (30, 220, 140, 40), "Red Carrier:");
		GUI.Label (new Rect (180, 220, 180, 40), "Press 8 on Keyboard");

		GUI.Label (new Rect (30, 250, 350, 40), "*Note: Units spawn at gaze cursor location.");

		GUI.Label(new Rect(120, 280, 250, 40), "Moving Units:");
		GUI.Label(new Rect(20, 305, 370, 40), "Hover gaze cursor over a unit and tap to select.");
		GUI.Label(new Rect(20, 325, 370, 40), "Gaze to desired new location, and tap to place.");
		GUI.Label(new Rect(20, 355, 360, 40), "Say: \"Dismiss Help\" to hide this window.");
		GUI.EndGroup ();
	}
}
