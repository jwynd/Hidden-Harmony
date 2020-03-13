using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SeamlessPortalsConfig))]
public class SPC_UI : Editor
{
	SeamlessPortalsConfig SPC;

	Color DefaultGUIColour;

	GUIStyle Blank = new GUIStyle();
	GUIStyle ERROR = new GUIStyle();

	public Texture2D titleImage;

	enum settings {None, Player, PortalObject, Teleport, Camera};
	settings CurrentPage;

	void OnEnable()
	{
		SPC = (SeamlessPortalsConfig)target;
		DefaultGUIColour = GUI.color;
		if (SPC.editorImage != null) titleImage = SPC.editorImage;
		ERROR.wordWrap = true;
		ERROR.normal.textColor = Color.red;
		ERROR.alignment = TextAnchor.MiddleCenter;

		if(SPC.CamCorrectGameObjects.Count == 0)
		{
			GameObject obj = new GameObject();
			SPC.CamCorrectGameObjects.Add(obj);
			SPC.CamCorrectGameObjects[SPC.CamCorrectGameObjects.IndexOf(obj)] = null;
			DestroyImmediate(obj);
		}

	}


	public override void OnInspectorGUI()
	{
		GUILayout.Space(5);

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Box(titleImage, Blank);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();


		if (CurrentPage == settings.None)
		{
			GUI.FocusControl("");

			GUILayout.BeginVertical();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUI.color = new Color32(171, 210, 121, 255);
			if (GUILayout.Button("Player Settings", GUILayout.Width(165), GUILayout.Height(40)))
				CurrentPage = settings.Player;
			if (GUILayout.Button("Portal Object Settings", GUILayout.Width(165), GUILayout.Height(40)))
				CurrentPage = settings.PortalObject;
			GUI.color = DefaultGUIColour;
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUI.color = new Color32(171, 210, 121, 255);
			if (GUILayout.Button("Teleport Settings", GUILayout.Width(165), GUILayout.Height(40)))
				CurrentPage = settings.Teleport;
			if (GUILayout.Button("Camera Settings", GUILayout.Width(165), GUILayout.Height(40)))
				CurrentPage = settings.Camera;
			GUI.color = DefaultGUIColour;
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.EndVertical();

			if (SPC.ERROR)
				GUILayout.Label("AN ERROR IS PREVENTING THIS SCRIPT FROM RUNNING\nCHECK CONSOLE", ERROR);
		}
		else if(CurrentPage == settings.Player)
		{
			GUILayout.BeginVertical("box");

			GUILayout.Label("Player Settings", EditorStyles.boldLabel);

			GUILayout.BeginHorizontal();
			SPC.Cam_Player = (Camera)EditorGUILayout.ObjectField(new GUIContent("Player Camera","Set this to the main camera that renders what the player sees, if using VR you will have two cameras (one per eye) so will need to set up identical portals and configs for each camera and set it so that each camera only renders one set of portals."), SPC.Cam_Player, typeof(Camera), true);
			GUILayout.EndHorizontal();

			GUILayout.EndVertical();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUI.color = new Color32(171, 210, 121, 255);
			if (GUILayout.Button("Back", GUILayout.Width(60), GUILayout.Height(20)))
				CurrentPage = settings.None;
			GUI.color = DefaultGUIColour;
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			
		}
		else if(CurrentPage == settings.PortalObject)
		{
			GUILayout.BeginVertical("box");

			GUILayout.Label("Portal Object Settings", EditorStyles.boldLabel);

			GUILayout.BeginHorizontal();
			SPC.GO_PortalA = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Portal A", "The GameObject that you want to render as portal A. This should ideally be a plane (if it only needs to be visible from one side) or a very thin cube (if you want it to be visible from both sides)."), SPC.GO_PortalA, typeof(GameObject), true);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			SPC.GO_PortalB = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Portal B", "The GameObject that you want to render as portal B. This should ideally be a plane (if it only needs to be visible from one side) or a very thin cube (if you want it to be visible from both sides)."), SPC.GO_PortalB, typeof(GameObject), true);
			GUILayout.EndHorizontal();


			GUILayout.BeginHorizontal();
			SPC.PortalForwardAxis = (SeamlessPortalsConfig.AxisList)EditorGUILayout.EnumPopup(new GUIContent("Portal Forward Axis", "This is the direction that your portals should work in. You can click the 'Test' button to check the axis you've set, the line should pass through your portal in the direction that the player would walk through it."), SPC.PortalForwardAxis);
			SPC.DrawAxisTest = GUILayout.Toggle(SPC.DrawAxisTest, "Test", GUI.skin.button, GUILayout.Width(50), GUILayout.Height(14));
			GUILayout.EndHorizontal();


			GUILayout.BeginHorizontal();
			SPC.ColliderMode = (SeamlessPortalsConfig.ColliderTypes)EditorGUILayout.EnumPopup(new GUIContent("Portal Colliders", "From this drop-down you can select which colliders to use to detect when a player has walked through your portal. 'Auto' will use the colliders attached to the 'PortalA' and 'PortalB' GameObjects, if you select 'User Defined' then you can set your own colliders."), SPC.ColliderMode);
			GUILayout.EndHorizontal();


			if(SPC.ColliderMode == SeamlessPortalsConfig.ColliderTypes.UserDefined)
			{
				GUILayout.BeginHorizontal();
				SPC.Box_A = (BoxCollider)EditorGUILayout.ObjectField(new GUIContent("Portal A Collider","The collider you want to use to detect when the player has walked throught Portal A."), SPC.Box_A, typeof(BoxCollider), true);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				SPC.Box_B = (BoxCollider)EditorGUILayout.ObjectField(new GUIContent("Portal B Collider", "The collider you want to use to detect when the player has walked throught Portal B."), SPC.Box_B, typeof(BoxCollider), true);
				GUILayout.EndHorizontal();
			}

			

			GUILayout.EndVertical();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUI.color = new Color32(171, 210, 121, 255);
			if (GUILayout.Button("Back", GUILayout.Width(60), GUILayout.Height(20)))
				CurrentPage = settings.None;
			GUI.color = DefaultGUIColour;
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			SceneView.RepaintAll();
		}
		else if(CurrentPage == settings.Teleport)
		{
			GUILayout.BeginVertical("box");
			GUILayout.Label("Teleport Settings", EditorStyles.boldLabel);
			

			GUILayout.BeginHorizontal();
			SPC.AllowTeleport= GUILayout.Toggle(SPC.AllowTeleport, new GUIContent("Allow Teleport","This toggles whether or not the player can teleport by walking into the portal."));
			GUILayout.EndHorizontal();

			if (SPC.AllowTeleport)
			{
				SPC.HardColliders = false;
				GUILayout.BeginHorizontal();
				SPC.TeleportObject = (SeamlessPortalsConfig.ObjectType)EditorGUILayout.EnumPopup(new GUIContent("Object to Teleport", "The GameObject which will be teleported as the player camera moves through the portal. 'Target Transform' is an empty GameObject that will be teleported to the correct position and rotation which can be used as reference for other obects."), SPC.TeleportObject);
				GUILayout.EndHorizontal();


				if (SPC.TeleportObject == SeamlessPortalsConfig.ObjectType.TargetTransform)
				{

					GUILayout.BeginHorizontal();
					SPC.CamCorrectFunctionName = EditorGUILayout.TextField(new GUIContent("Update Function Name","This function will be called on all objects listed below after 'Target Transform' has been teleported."), SPC.CamCorrectFunctionName);
					GUILayout.EndHorizontal();

					GUILayout.BeginVertical("box");
					GUILayout.BeginHorizontal();
					GUILayout.Label(new GUIContent("Objects to Update","The update function named above will be called on all objects in this list when 'Target Transform' is teleported."), GUILayout.Width(105));
					if (GUILayout.Button("Add", GUI.skin.button, GUILayout.Width(40), GUILayout.Height(14)))
					{
						GameObject obj = new GameObject();
						SPC.CamCorrectGameObjects.Add(obj);
						SPC.CamCorrectGameObjects[SPC.CamCorrectGameObjects.IndexOf(obj)] = null;
						DestroyImmediate(obj);
					}
					GUILayout.EndHorizontal();

					for (int i = 0; i < SPC.CamCorrectGameObjects.Count; i++)
					{
						GUILayout.BeginHorizontal();
						SPC.CamCorrectGameObjects[i] = (GameObject)EditorGUILayout.ObjectField("", SPC.CamCorrectGameObjects[i], typeof(GameObject), true);
						if (GUILayout.Button("Remove", GUI.skin.button, GUILayout.Width(70), GUILayout.Height(14)))
						{
							if (SPC.CamCorrectGameObjects.Count > 1)
								SPC.CamCorrectGameObjects.Remove(SPC.CamCorrectGameObjects[i]);
							else
								SPC.CamCorrectGameObjects[i] = null;
						}
						GUILayout.EndHorizontal();
					}

					GUILayout.EndVertical();

				}
			}
			else
			{
				GUILayout.BeginHorizontal();
				SPC.HardColliders = GUILayout.Toggle(SPC.HardColliders, new GUIContent("Hard Colliders","When teleporting is disabled, this sets whether or not the portals' colliders prevent the player from walking through the portal object."));
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUI.color = new Color32(171, 210, 121, 255);
			if (GUILayout.Button("Back", GUILayout.Width(60), GUILayout.Height(20)))
				CurrentPage = settings.None;
			GUI.color = DefaultGUIColour;
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			
		}
		else if (CurrentPage == settings.Camera)
		{
			GUILayout.BeginVertical("box");

			GUILayout.Label("Camera Settings", EditorStyles.boldLabel);

			GUILayout.BeginHorizontal();
			SPC.ClippingPlaneOffset = EditorGUILayout.FloatField(new GUIContent("Clipping Plane Offset","Offset of the clipping plane for portal cameras."), SPC.ClippingPlaneOffset);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			SPC.ViewCubeDepth = EditorGUILayout.FloatField(new GUIContent("View Cube Depth","The depth of the 'View Cubes' which become visible, check documentation for more info."), SPC.ViewCubeDepth);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			SPC.ViewCubeOffset = EditorGUILayout.FloatField(new GUIContent("View Cube Offset","The offset of the 'View Cubes', check documentation for more info."), SPC.ViewCubeOffset);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			SPC.CullPortalChildren = GUILayout.Toggle(SPC.CullPortalChildren, new GUIContent("Cull Portal Children","If enabled all children of the portal objects will be invisible when looking through the portals."));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			SPC.UpdateFOV = GUILayout.Toggle(SPC.UpdateFOV, new GUIContent("Maintain FOV of Portal Cameras","Ensures the portal object cameras' FOV is always the same as the player camera's. Enable if the player camera FOV changes in your game, otherwise it can be left disabled."));
			GUILayout.EndHorizontal();

			GUILayout.EndVertical();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUI.color = new Color32(171, 210, 121, 255);
			if (GUILayout.Button("Back", GUILayout.Width(60), GUILayout.Height(20)))
				CurrentPage = settings.None;
			GUI.color = DefaultGUIColour;
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();


		}

		//Uncomment out this section to show the default inspector - this will allow you to see all of the variables if you need to debug
		/*GUILayout.Space(20);
		GUILayout.Label("Default Inspector");
		GUILayout.Space(5);

		DrawDefaultInspector();*/

		GUILayout.Space(2);

		
	}


	Texture2D ColourToTex2D(Color colour)
	{
		Texture2D tex = new Texture2D(1, 1);
		tex.SetPixel(1, 1, colour);
		tex.Apply();
		return tex;
	}


}