#pragma strict

var guiSkin: GUISkin;


private var windowRect : Rect = Rect (0, 0, 400, 380);


function Start () 
{

}


function OnGUI () 
{
   windowRect.x = (Screen.width - windowRect.width)/2;
   windowRect.y = (Screen.height - windowRect.height)/2;
   GUI.skin = guiSkin;
   windowRect = GUI.Window (0, windowRect, DoMyWindow, "Wargaming Command Reference");
  
}
	
	

function DoMyWindow (windowID : int) 
{

    GUI.Box(Rect(10,30,380,80),"Test Text");



   
    
			
	GUI.DragWindow (Rect (0,0,10000,10000));
}