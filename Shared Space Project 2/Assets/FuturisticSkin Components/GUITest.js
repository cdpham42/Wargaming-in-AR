var MenuSkin : GUISkin;

var toggleTxt : boolean;

var toolbarInt : int = 0;

var selGridInt : int = 0;

function OnGUI() {

	GUI.skin = MenuSkin;

    GUI.BeginGroup(new Rect(Screen.width/2-200,Screen.height/2-300,410,410));

        		GUI.Box(Rect(0,0,410,410),"Wargame Command Index");

                GUI.Label (Rect (120, 35, 140, 40), "Spawning Units:");

         		GUI.Label (Rect (30, 60, 140, 40), "Blue Battleship:");
         		GUI.Label (Rect (180, 60, 180, 40), "Press 1 on Keyboard");
         		GUI.Label (Rect (30, 80, 140, 40), "Blue Submarine:");
         		GUI.Label (Rect (180, 80, 180, 40), "Press 2 on Keyboard");
         		GUI.Label (Rect (30, 100, 140, 40), "Blue Fighter Jet:");
         		GUI.Label (Rect (180, 100, 180, 40), "Press 3 on Keyboard");
         		GUI.Label (Rect (30, 120, 140, 40), "Blue Carrier:");
         		GUI.Label (Rect (180, 120, 180, 40), "Press 4 on Keyboard");

         		GUI.Label (Rect (30, 160, 140, 40), "Red Battleship:");
         		GUI.Label (Rect (180, 160, 180, 40), "Press 5 on Keyboard");
         		GUI.Label (Rect (30, 180, 140, 40), "Red Submarine:");
         		GUI.Label (Rect (180, 180, 180, 40), "Press 6 on Keyboard");
         		GUI.Label (Rect (30, 200, 140, 40), "Red Fighter Jet:");
         		GUI.Label (Rect (180, 200, 180, 40), "Press 7 on Keyboard");
         		GUI.Label (Rect (30, 220, 140, 40), "Red Carrier:");
         		GUI.Label (Rect (180, 220, 180, 40), "Press 8 on Keyboard");

         		GUI.Label (Rect (30, 250, 350, 40), "*Note: Units spawn at gaze cursor location.");

         		GUI.Label(Rect(120, 280, 250, 40), "Moving Units:");
         		GUI.Label(Rect(20, 305, 370, 40), "Hover gaze cursor over a unit and tap to select.");
         		GUI.Label(Rect(20, 325, 370, 40), "Gaze to desired new location, and tap to place.");
         		GUI.Label(Rect(20, 355, 360, 40), "Say: \"Dismiss Help\" to hide this window.");
		GUI.EndGroup ();
}
