using System.Collections.Generic;
using UnityEngine;
public class SeamlessPortalsConfig : MonoBehaviour
{
	public Texture2D editorImage;

	public Camera Cam_Player;
	public Camera Cam_PortalB;
	public Camera Cam_PortalA;

	public GameObject GO_PortalA;
	public GameObject GO_PortalB;

	public Material Mat_PortalA;
	public Material Mat_PortalB;

	public bool CullPortalChildren;

	public enum ColliderTypes { Auto, UserDefined };
	public ColliderTypes ColliderMode;

	public BoxCollider Col_PortalA;
	public BoxCollider Col_PortalB;

	public bool inPortalA;
	public bool inPortalB;

	public enum AxisList { x, y, z };
	public AxisList PortalForwardAxis;

	public enum ObjectType { PlayerCamera, CameraParent, TargetTransform };
	public ObjectType TeleportObject;

	public GameObject tpObject;
	public bool tpoTeleported;

	public GameObject PortalObject;

	public List<GameObject> CamCorrectGameObjects = new List<GameObject>();
	public string CamCorrectFunctionName = "PortalCameraCorrect";

	public bool UpdateFOV;

	public float ClippingPlaneOffset = 0.05f;

	public GameObject[] ViewCube_PortalA = new GameObject[2];
	public GameObject[] ViewCube_PortalB = new GameObject[2];

	public float ViewCubeOffset = 0.05f;
	public float ViewCubeDepth = 0.3f;

	public bool[] CubeActive_PortalA = new bool[2];
	public bool[] CubeActive_PortalB = new bool[2];

	public BoxCollider[] ViewCubeCollider_PortalA;
	public BoxCollider[] ViewCubeCollider_PortalB;

	public MeshRenderer MR_PortalA;
	public MeshRenderer MR_PortalB;

	public MeshRenderer[] ViewCubeMR_PortalA = new MeshRenderer[2];
	public MeshRenderer[] ViewCubeMR_PortalB = new MeshRenderer[2];

	public Vector3 ViewCubeVector_PortalA;
	public Vector3 ViewCubeVector_PortalB;

	public bool AllowTeleport = true;
	public bool HardColliders = false;

	public bool ERROR = false;

	public BoxCollider Box_A;
	public BoxCollider Box_B;

	void Start()
	{
		ViewCube_PortalA = new GameObject[2];
		ViewCube_PortalB = new GameObject[2];

		CubeActive_PortalA = new bool[2];
		CubeActive_PortalB = new bool[2];

		ViewCubeCollider_PortalA = new BoxCollider[2];
		ViewCubeCollider_PortalB = new BoxCollider[2];

		ViewCubeMR_PortalA = new MeshRenderer[2];
		ViewCubeMR_PortalB = new MeshRenderer[2];

		PortalObject = new GameObject("Portal Object");

		GameObject GO_CamA = new GameObject("Cam_PortalA");
		GameObject GO_CamB = new GameObject("Cam_PortalB");

		Cam_PortalA = GO_CamA.AddComponent<Camera>();
		Cam_PortalB = GO_CamB.AddComponent<Camera>();

		if (GO_PortalA != null)
			GO_CamA.transform.parent = GO_PortalA.transform;
		else
		{
			Debug.LogWarning("[SEAMLESS PORTALS] PortalA GameObject not set", this);
			ERROR = true;
			return;
		}

		if (GO_PortalB != null)
			GO_CamB.transform.parent = GO_PortalB.transform;
		else
		{
			Debug.LogWarning("[SEAMLESS PORTALS] PortalB GameObject not set", this);
			ERROR = true;
			return;
		}

		GO_CamB.transform.parent = GO_PortalB.transform;

		Cam_PortalA.nearClipPlane = 0.01f;
		Cam_PortalB.nearClipPlane = 0.01f;

		if (LayerMask.NameToLayer("PortalA") != -1)
		{
			GO_PortalA.layer = LayerMask.NameToLayer("PortalA");
			if (CullPortalChildren)
			{
				for (int i = 0; i < GO_PortalA.transform.childCount; i++)
				{
					GO_PortalA.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("PortalA");
				}
			}
			Cam_PortalA.cullingMask += ~(1 << LayerMask.NameToLayer("PortalA"));
			Cam_PortalB.cullingMask += ~(1 << LayerMask.NameToLayer("PortalA"));
		}
		else
		{
			Debug.LogWarning("[SEAMLESS PORTALS] No 'PortalA' layer exists, please add it");
			ERROR = true;
			return;
		}


		if (LayerMask.NameToLayer("PortalB") != -1)
		{
			GO_PortalB.layer = LayerMask.NameToLayer("PortalB");
			if (CullPortalChildren)
			{
				for (int i = 0; i < GO_PortalB.transform.childCount; i++)
				{
					GO_PortalB.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("PortalB");
				}
			}
			Cam_PortalA.cullingMask += ~(1 << LayerMask.NameToLayer("PortalB"));
			Cam_PortalB.cullingMask += ~(1 << LayerMask.NameToLayer("PortalB"));
		}
		else
		{
			Debug.LogWarning("[SEAMLESS PORTALS] No 'PortalB' layer exists, please add it");
			ERROR = true;
			return;
		}


		if (GO_PortalA.GetComponent<MeshRenderer>() != null)
			Mat_PortalA = GO_PortalA.GetComponent<MeshRenderer>().material;
		else
		{
			Debug.LogWarning("[SEAMLESS PORTALS] PortalA GameObject has no mesh renderer", GO_PortalA);
			ERROR = true;
			return;
		}


		if (GO_PortalB.GetComponent<MeshRenderer>() != null)
			Mat_PortalB = GO_PortalB.GetComponent<MeshRenderer>().material;
		else
		{
			Debug.LogWarning("[SEAMLESS PORTALS] PortalB GameObject has no mesh renderer", GO_PortalB);
			ERROR = true;
			return;
		}

		Cam_PortalB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		Cam_PortalA.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

		Mat_PortalA.mainTexture = Cam_PortalA.targetTexture;
		Mat_PortalB.mainTexture = Cam_PortalB.targetTexture;

		


		if (ColliderMode == ColliderTypes.Auto)
		{
			if (GO_PortalA.GetComponent<BoxCollider>() != null)
				Box_A = GO_PortalA.GetComponent<BoxCollider>();
			else
			{
				Debug.LogWarning("[SEAMLESS PORTALS] Colliders are set to 'auto' but the PortalA GameObject has no BoxCollider (other collider types are not compatible)", GO_PortalA);
				ERROR = true;
				return;
			}

			if (GO_PortalB.GetComponent<BoxCollider>() != null)
				Box_B = GO_PortalA.GetComponent<BoxCollider>();
			else
			{
				Debug.LogWarning("[SEAMLESS PORTALS] Colliders are set to 'auto' but the PortalB GameObject has no BoxCollider (other collider types are not compatible)", GO_PortalB);
				ERROR = true;
				return;
			}
		}
		else if (ColliderMode == ColliderTypes.UserDefined)
		{
			if (Box_A == null)
			{
				Debug.LogWarning("[SEAMLESS PORTALS] Colliders are set to 'user defined' but no BoxCollider has been defined for PortalA (other collider types are not compatible)", this);
				ERROR = true;
				return;
			}

			if (Box_B == null)
			{
				Debug.LogWarning("[SEAMLESS PORTALS] Colliders are set to 'user defined' but no BoxCollider has been defined for PortalB (other collider types are not compatible)", this);
				ERROR = true;
				return;
			}
		}

		GameObject colliderObjectA = new GameObject("Col_PortalA");
		Col_PortalA = colliderObjectA.AddComponent<BoxCollider>();
		colliderObjectA.transform.SetPositionAndRotation(Box_A.transform.TransformPoint(Box_A.center), Box_A.transform.rotation);
		colliderObjectA.transform.parent = GO_PortalA.transform;
		SetGlobalScale(colliderObjectA.transform, Vector3.Scale(Box_A.size, Box_A.transform.lossyScale));
		Col_PortalA.isTrigger = true;
		Box_A.enabled = false;

		GameObject colliderObjectB = new GameObject("Col_PortalB");
		Col_PortalB = colliderObjectB.AddComponent<BoxCollider>();
		colliderObjectB.transform.SetPositionAndRotation(GO_PortalB.transform.TransformPoint(Box_B.center), GO_PortalB.transform.rotation);
		Col_PortalB.transform.parent = GO_PortalB.transform;
		SetGlobalScale(colliderObjectB.transform, Vector3.Scale(Box_B.size, Box_B.transform.lossyScale));
		Col_PortalB.isTrigger = true;
		Box_B.enabled = false;

		if (Cam_Player != null)
		{
			Cam_PortalB.fieldOfView = Cam_Player.fieldOfView;
			Cam_PortalA.fieldOfView = Cam_Player.fieldOfView;
			Cam_PortalA.depth = Cam_Player.depth - 1;
			Cam_PortalB.depth = Cam_Player.depth - 1;
		}
		else
		{
			Debug.LogWarning("[SEAMLESS PORTALS] Player camera not set", this);
			ERROR = true;
			return;
		}

		Bounds Bounds_PortalA = new Bounds();
		Bounds Bounds_PortalB = new Bounds();
		if (GO_PortalA.GetComponent<MeshFilter>() != null)
			Bounds_PortalA = GO_PortalA.GetComponent<MeshFilter>().mesh.bounds;
		else
		{
			Debug.LogWarning("[SEAMLESS PORTALS] The PortalA GameObject has no MeshFilter component", GO_PortalA);
			ERROR = true;
			return;
		}
		if (GO_PortalB.GetComponent<MeshFilter>() != null)
			Bounds_PortalB = GO_PortalB.GetComponent<MeshFilter>().mesh.bounds;
		else
		{
			Debug.LogWarning("[SEAMLESS PORTALS] The PortalB GameObject has no MeshFilter component", GO_PortalB);
			ERROR = true;
			return;
		}

		switch (PortalForwardAxis)
		{
			case (AxisList.x):
				ViewCube_PortalA[0] = Create5SideInvertedCube(GO_PortalA, GO_PortalA.transform.position, GO_PortalA.transform.right, GO_PortalA.transform.up, new Vector3(0.99f * Bounds_PortalA.size.z * GO_PortalA.transform.lossyScale.z, 0.99f * Bounds_PortalA.size.y * GO_PortalA.transform.lossyScale.y, ViewCubeDepth + Bounds_PortalA.size.x * GO_PortalA.transform.lossyScale.x), ViewCubeOffset, "ViewCube_PortalA0");
				ViewCube_PortalB[0] = Create5SideInvertedCube(GO_PortalB, GO_PortalB.transform.position, GO_PortalB.transform.right, GO_PortalB.transform.up, new Vector3(0.99f * Bounds_PortalB.size.z * GO_PortalB.transform.lossyScale.z, 0.99f * Bounds_PortalB.size.y * GO_PortalB.transform.lossyScale.y, ViewCubeDepth + Bounds_PortalB.size.x * GO_PortalB.transform.lossyScale.x), ViewCubeOffset, "ViewCube_PortalB0");
				ViewCube_PortalA[1] = Create5SideInvertedCube(GO_PortalA, GO_PortalA.transform.position, -GO_PortalA.transform.right, GO_PortalA.transform.up, new Vector3(0.99f * Bounds_PortalA.size.z * GO_PortalA.transform.lossyScale.z, 0.99f * Bounds_PortalA.size.y * GO_PortalA.transform.lossyScale.y, ViewCubeDepth + Bounds_PortalA.size.x * GO_PortalA.transform.lossyScale.x), ViewCubeOffset, "ViewCube_PortalA1");
				ViewCube_PortalB[1] = Create5SideInvertedCube(GO_PortalB, GO_PortalB.transform.position, -GO_PortalB.transform.right, GO_PortalB.transform.up, new Vector3(0.99f * Bounds_PortalB.size.z * GO_PortalB.transform.lossyScale.z, 0.99f * Bounds_PortalB.size.y * GO_PortalB.transform.lossyScale.y, ViewCubeDepth + Bounds_PortalB.size.x * GO_PortalB.transform.lossyScale.x), ViewCubeOffset, "ViewCube_PortalB1");
				break;

			case (AxisList.y):
				ViewCube_PortalA[0] = Create5SideInvertedCube(GO_PortalA, GO_PortalA.transform.position, GO_PortalA.transform.up, GO_PortalA.transform.right, new Vector3(0.99f * Bounds_PortalA.size.z * GO_PortalA.transform.lossyScale.z, 0.99f * Bounds_PortalA.size.x * GO_PortalA.transform.lossyScale.x, ViewCubeDepth + Bounds_PortalA.size.y * GO_PortalA.transform.lossyScale.y), ViewCubeOffset, "ViewCube_PortalA0");
				ViewCube_PortalB[0] = Create5SideInvertedCube(GO_PortalB, GO_PortalB.transform.position, GO_PortalB.transform.up, GO_PortalB.transform.right, new Vector3(0.99f * Bounds_PortalB.size.z * GO_PortalB.transform.lossyScale.z, 0.99f * Bounds_PortalB.size.x * GO_PortalB.transform.lossyScale.x, ViewCubeDepth + Bounds_PortalB.size.y * GO_PortalB.transform.lossyScale.y), ViewCubeOffset, "ViewCube_PortalB0");
				ViewCube_PortalA[1] = Create5SideInvertedCube(GO_PortalA, GO_PortalA.transform.position, -GO_PortalA.transform.up, GO_PortalA.transform.right, new Vector3(0.99f * Bounds_PortalA.size.z * GO_PortalA.transform.lossyScale.z, 0.99f * Bounds_PortalA.size.x * GO_PortalA.transform.lossyScale.x, ViewCubeDepth + Bounds_PortalA.size.y * GO_PortalA.transform.lossyScale.y), ViewCubeOffset, "ViewCube_PortalA1");
				ViewCube_PortalB[1] = Create5SideInvertedCube(GO_PortalB, GO_PortalB.transform.position, -GO_PortalB.transform.up, GO_PortalB.transform.right, new Vector3(0.99f * Bounds_PortalB.size.z * GO_PortalB.transform.lossyScale.z, 0.99f * Bounds_PortalB.size.x * GO_PortalB.transform.lossyScale.x, ViewCubeDepth + Bounds_PortalB.size.y * GO_PortalB.transform.lossyScale.y), ViewCubeOffset, "ViewCube_PortalB1");
				break;

			case (AxisList.z):
				ViewCube_PortalA[0] = Create5SideInvertedCube(GO_PortalA, GO_PortalA.transform.position, GO_PortalA.transform.forward, GO_PortalA.transform.up, new Vector3(0.99f * Bounds_PortalA.size.x * GO_PortalA.transform.lossyScale.x, 0.99f * Bounds_PortalA.size.y * GO_PortalA.transform.lossyScale.y, ViewCubeDepth + Bounds_PortalA.size.z * GO_PortalA.transform.lossyScale.z), ViewCubeOffset, "ViewCube_PortalA0");
				ViewCube_PortalB[0] = Create5SideInvertedCube(GO_PortalB, GO_PortalB.transform.position, GO_PortalB.transform.forward, GO_PortalB.transform.up, new Vector3(0.99f * Bounds_PortalB.size.x * GO_PortalB.transform.lossyScale.x, 0.99f * Bounds_PortalB.size.y * GO_PortalB.transform.lossyScale.y, ViewCubeDepth + Bounds_PortalB.size.z * GO_PortalB.transform.lossyScale.z), ViewCubeOffset, "ViewCube_PortalB0");
				ViewCube_PortalA[1] = Create5SideInvertedCube(GO_PortalA, GO_PortalA.transform.position, -GO_PortalA.transform.forward, GO_PortalA.transform.up, new Vector3(0.99f * Bounds_PortalA.size.x * GO_PortalA.transform.lossyScale.x, 0.99f * Bounds_PortalA.size.y * GO_PortalA.transform.lossyScale.y, ViewCubeDepth + Bounds_PortalA.size.z * GO_PortalA.transform.lossyScale.z), ViewCubeOffset, "ViewCube_PortalA1");
				ViewCube_PortalB[1] = Create5SideInvertedCube(GO_PortalB, GO_PortalB.transform.position, -GO_PortalB.transform.forward, GO_PortalB.transform.up, new Vector3(0.99f * Bounds_PortalB.size.x * GO_PortalB.transform.lossyScale.x, 0.99f * Bounds_PortalB.size.y * GO_PortalB.transform.lossyScale.y, ViewCubeDepth + Bounds_PortalB.size.z * GO_PortalB.transform.lossyScale.z), ViewCubeOffset, "ViewCube_PortalB1");
				break;
		}

		ViewCubeCollider_PortalA[0] = ViewCube_PortalA[0].GetComponent<BoxCollider>();
		ViewCubeCollider_PortalB[0] = ViewCube_PortalB[0].GetComponent<BoxCollider>();
		ViewCubeCollider_PortalA[1] = ViewCube_PortalA[1].GetComponent<BoxCollider>();
		ViewCubeCollider_PortalB[1] = ViewCube_PortalB[1].GetComponent<BoxCollider>();

		ViewCubeMR_PortalA[0] = ViewCube_PortalA[0].GetComponent<MeshRenderer>();
		ViewCubeMR_PortalB[0] = ViewCube_PortalB[0].GetComponent<MeshRenderer>();
		ViewCubeMR_PortalA[1] = ViewCube_PortalA[1].GetComponent<MeshRenderer>();
		ViewCubeMR_PortalB[1] = ViewCube_PortalB[1].GetComponent<MeshRenderer>();



		if (GO_PortalA.GetComponent<MeshRenderer>() != null)
			MR_PortalA = GO_PortalA.GetComponent<MeshRenderer>();
		else
		{
			Debug.LogWarning("[SEAMLESS PORTALS] The PortalA GameObject has no MeshRenderer component", GO_PortalA);
			ERROR = true;
			return;
		}
		if (GO_PortalB.GetComponent<MeshRenderer>() != null)
			MR_PortalB = GO_PortalB.GetComponent<MeshRenderer>();
		else
		{
			Debug.LogWarning("[SEAMLESS PORTALS] The PortalB GameObject has no MeshRenderer component", GO_PortalB);
			ERROR = true;
			return;
		}
	}

	public static void SetGlobalScale(Transform transform, Vector3 globalScale)
	{
		transform.localScale = Vector3.one;
		transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
	}


	void LateUpdate()
	{
		if (!ERROR)
		{
			Col_PortalA.isTrigger = !HardColliders;
			Col_PortalB.isTrigger = !HardColliders;

			CubeActive_PortalA[0] = ColliderContainsPoint(ViewCube_PortalA[0].transform, Cam_Player.transform.position, ViewCubeCollider_PortalA[0].enabled);
			CubeActive_PortalB[0] = ColliderContainsPoint(ViewCube_PortalB[0].transform, Cam_Player.transform.position, ViewCubeCollider_PortalB[0].enabled);
			CubeActive_PortalA[1] = ColliderContainsPoint(ViewCube_PortalA[1].transform, Cam_Player.transform.position, ViewCubeCollider_PortalA[1].enabled);
			CubeActive_PortalB[1] = ColliderContainsPoint(ViewCube_PortalB[1].transform, Cam_Player.transform.position, ViewCubeCollider_PortalB[1].enabled);

			if (inPortalA)
			{
				CubeActive_PortalA[0] = false;
				CubeActive_PortalA[1] = false;
			}
			if (inPortalB)
			{
				CubeActive_PortalB[0] = false;
				CubeActive_PortalB[1] = false;
			}

			ViewCubeMR_PortalA[0].enabled = CubeActive_PortalA[0];
			ViewCubeMR_PortalB[0].enabled = CubeActive_PortalB[0];
			ViewCubeMR_PortalA[1].enabled = CubeActive_PortalA[1];
			ViewCubeMR_PortalB[1].enabled = CubeActive_PortalB[1];

			if (CubeActive_PortalA[0] || CubeActive_PortalA[1])
				MR_PortalA.enabled = false;
			else
				MR_PortalA.enabled = true;
			if (CubeActive_PortalB[0] || CubeActive_PortalB[1])
				MR_PortalB.enabled = false;
			else
				MR_PortalB.enabled = true;

			GameObject nearestPortal;
			Vector3 camToA = GO_PortalA.transform.position - Cam_Player.transform.position;
			Vector3 camToB = GO_PortalB.transform.position - Cam_Player.transform.position;
			if (camToA.sqrMagnitude < camToB.sqrMagnitude)
				nearestPortal = GO_PortalA;
			else
				nearestPortal = GO_PortalB;

			if ((!CubeActive_PortalA[0] && !CubeActive_PortalA[1] && !CubeActive_PortalB[0] && !CubeActive_PortalB[1]) || inPortalA)
			{

				Vector3 camLocal = nearestPortal.transform.InverseTransformPoint(Cam_Player.transform.position);

				int s = GetActiveViewCubeIndex(camLocal);
				ViewCubeCollider_PortalA[s].enabled = false;
				ViewCubeCollider_PortalA[1 - s].enabled = true;

				if (!inPortalA)
				{
					Vector3 c = GO_PortalA.transform.InverseTransformPoint(Col_PortalA.transform.position);
					switch (PortalForwardAxis)
					{
						case (AxisList.x):
							c.x = Mathf.Abs(c.x) * Mathf.Sign(-camLocal.x);
							break;

						case (AxisList.y):
							c.y = Mathf.Abs(c.y) * Mathf.Sign(-camLocal.y);
							break;

						case (AxisList.z):
							c.z = Mathf.Abs(c.z) * Mathf.Sign(-camLocal.z);
							break;
					}
					Col_PortalA.transform.position = GO_PortalA.transform.TransformPoint(c);
				}

			}

			if ((!CubeActive_PortalA[0] && !CubeActive_PortalA[1] && !CubeActive_PortalB[0] && !CubeActive_PortalB[1]) || inPortalB)
			{

				Vector3 camLocal = nearestPortal.transform.InverseTransformPoint(Cam_Player.transform.position);

				int s = GetActiveViewCubeIndex(camLocal);
				ViewCubeCollider_PortalB[s].enabled = false;
				ViewCubeCollider_PortalB[1 - s].enabled = true;


				if (!inPortalB)
				{
					Vector3 c = GO_PortalB.transform.InverseTransformPoint(Col_PortalB.transform.position);
					switch (PortalForwardAxis)
					{
						case (AxisList.x):
							c.x = Mathf.Abs(c.x) * Mathf.Sign(-camLocal.x);
							break;

						case (AxisList.y):
							c.y = Mathf.Abs(c.y) * Mathf.Sign(-camLocal.y);
							break;

						case (AxisList.z):
							c.z = Mathf.Abs(c.z) * Mathf.Sign(-camLocal.z);
							break;
					}
					Col_PortalB.transform.position = GO_PortalB.transform.TransformPoint(c);
				}
			}


			if (UpdateFOV)
			{
				Cam_PortalB.fieldOfView = Cam_Player.fieldOfView;
				Cam_PortalA.fieldOfView = Cam_Player.fieldOfView;
			}

			switch (TeleportObject)
			{
				case (ObjectType.PlayerCamera):
					tpObject = Cam_Player.transform.gameObject;
					break;
				case (ObjectType.CameraParent):
					tpObject = Cam_Player.transform.parent.gameObject;
					break;
				case (ObjectType.TargetTransform):
					tpObject = PortalObject;
					break;
			}

			if (!tpoTeleported)
			{
				PortalObject.transform.SetPositionAndRotation(Cam_Player.transform.position, Cam_Player.transform.rotation);
			}


			if (ColliderContainsPoint(Col_PortalA.transform, Cam_Player.transform.position, true) && !inPortalA && AllowTeleport)
			{
				ViewCubeMR_PortalA[0].enabled = false;
				ViewCubeMR_PortalA[1].enabled = false;

				Vector4 clipPlane_PortalA = CameraSpacePlane(Cam_PortalA, GO_PortalB.transform.position, -PortalNormal(GO_PortalB, Cam_PortalA, PortalForwardAxis), ClippingPlaneOffset);
				Cam_PortalA.projectionMatrix = Cam_Player.CalculateObliqueMatrix(clipPlane_PortalA);
				Vector4 clipPlane_PortalB = CameraSpacePlane(Cam_PortalB, GO_PortalA.transform.position, -PortalNormal(GO_PortalA, Cam_PortalB, PortalForwardAxis), ClippingPlaneOffset);
				Cam_PortalB.projectionMatrix = Cam_Player.CalculateObliqueMatrix(clipPlane_PortalB);

				tpObject.transform.rotation = GO_PortalB.transform.rotation * Quaternion.Inverse(GO_PortalA.transform.rotation) * Cam_Player.transform.rotation;
				Vector3 posB = GO_PortalA.transform.InverseTransformPoint(Cam_Player.transform.position);
				posB = Vector3.Scale(posB, DivideVectors(GO_PortalA.transform.lossyScale, GO_PortalB.transform.lossyScale));
				tpObject.transform.position = GO_PortalB.transform.TransformPoint(posB);

				inPortalB = true;

				if (TeleportObject == ObjectType.TargetTransform)
				{
					tpoTeleported = true;
					foreach (GameObject go in CamCorrectGameObjects)
					{
						go.SendMessage(CamCorrectFunctionName, tpObject.transform);
					}
					tpoTeleported = false;
				}

			}

			else if (!ColliderContainsPoint(Col_PortalA.transform, Cam_Player.transform.position, true) && inPortalA && AllowTeleport)
			{
				inPortalA = false;


				Vector3 camLocal = nearestPortal.transform.InverseTransformPoint(Cam_Player.transform.position);
				Vector3 c = GO_PortalA.transform.InverseTransformPoint(Col_PortalA.transform.position);
				switch (PortalForwardAxis)
				{
					case (AxisList.x):
						c.x = Mathf.Abs(c.x) * Mathf.Sign(-camLocal.x);
						break;

					case (AxisList.y):
						c.y = Mathf.Abs(c.y) * Mathf.Sign(-camLocal.y);
						break;

					case (AxisList.z):
						c.z = Mathf.Abs(c.z) * Mathf.Sign(-camLocal.z);
						break;
				}
				Col_PortalA.transform.position = GO_PortalA.transform.TransformPoint(c);


			}

			else if (ColliderContainsPoint(Col_PortalB.transform, Cam_Player.transform.position, true) && !inPortalB && AllowTeleport)
			{
				ViewCubeMR_PortalB[0].enabled = false;
				ViewCubeMR_PortalB[1].enabled = false;

				Vector4 clipPlane_PortalA = CameraSpacePlane(Cam_PortalA, GO_PortalB.transform.position, -PortalNormal(GO_PortalB, Cam_PortalA, PortalForwardAxis), ClippingPlaneOffset);
				Cam_PortalA.projectionMatrix = Cam_Player.CalculateObliqueMatrix(clipPlane_PortalA);
				Vector4 clipPlane_PortalB = CameraSpacePlane(Cam_PortalB, GO_PortalA.transform.position, -PortalNormal(GO_PortalA, Cam_PortalB, PortalForwardAxis), ClippingPlaneOffset);
				Cam_PortalB.projectionMatrix = Cam_Player.CalculateObliqueMatrix(clipPlane_PortalB);


				tpObject.transform.rotation = GO_PortalA.transform.rotation * Quaternion.Inverse(GO_PortalB.transform.rotation) * Cam_Player.transform.rotation;
				Vector3 posA = GO_PortalB.transform.InverseTransformPoint(Cam_Player.transform.position);
				posA = Vector3.Scale(posA, DivideVectors(GO_PortalB.transform.lossyScale, GO_PortalA.transform.lossyScale));
				tpObject.transform.position = GO_PortalA.transform.TransformPoint(posA);

				inPortalA = true;

				if (TeleportObject == ObjectType.TargetTransform)
				{
					tpoTeleported = true;
					foreach (GameObject go in CamCorrectGameObjects)
					{
						go.SendMessage(CamCorrectFunctionName, tpObject.transform);
					}
					tpoTeleported = false;
				}
			}

			else if (!ColliderContainsPoint(Col_PortalB.transform, Cam_Player.transform.position, true) && inPortalB && AllowTeleport)
			{
				inPortalB = false;

				if (!inPortalB)
				{
					Vector3 camLocal = nearestPortal.transform.InverseTransformPoint(Cam_Player.transform.position);
					Vector3 c = GO_PortalB.transform.InverseTransformPoint(Col_PortalB.transform.position);
					switch (PortalForwardAxis)
					{
						case (AxisList.x):
							c.x = Mathf.Abs(c.x) * Mathf.Sign(-camLocal.x);
							break;

						case (AxisList.y):
							c.y = Mathf.Abs(c.y) * Mathf.Sign(-camLocal.y);
							break;

						case (AxisList.z):
							c.z = Mathf.Abs(c.z) * Mathf.Sign(-camLocal.z);
							break;
					}
					Col_PortalB.transform.position = GO_PortalB.transform.TransformPoint(c);
				}


			}

			Vector3 localPos = GO_PortalB.transform.InverseTransformPoint(Cam_Player.transform.position);
			Quaternion localRot = Quaternion.Inverse(GO_PortalB.transform.rotation) * Cam_Player.transform.rotation;
			localPos = Vector3.Scale(localPos, DivideVectors(GO_PortalB.transform.lossyScale, GO_PortalA.transform.lossyScale));
			Cam_PortalB.transform.SetPositionAndRotation(GO_PortalA.transform.TransformPoint(localPos), GO_PortalA.transform.rotation * localRot);

			localPos = GO_PortalA.transform.InverseTransformPoint(Cam_Player.transform.position);
			localRot = Quaternion.Inverse(GO_PortalA.transform.rotation) * Cam_Player.transform.rotation;
			localPos = Vector3.Scale(localPos, DivideVectors(GO_PortalA.transform.lossyScale, GO_PortalB.transform.lossyScale));
			Cam_PortalA.transform.SetPositionAndRotation(GO_PortalB.transform.TransformPoint(localPos), GO_PortalB.transform.rotation * localRot);

			if ((!CubeActive_PortalA[0] && !CubeActive_PortalA[1]) && !inPortalA && !inPortalB)
			{
				Vector4 clipPlane_PortalA = CameraSpacePlane(Cam_PortalA, GO_PortalB.transform.position, PortalNormal(GO_PortalB, Cam_PortalA, PortalForwardAxis), ClippingPlaneOffset);
				Cam_PortalA.projectionMatrix = Cam_Player.CalculateObliqueMatrix(clipPlane_PortalA);
			}
			else
				Cam_PortalA.ResetProjectionMatrix();

			if (!CubeActive_PortalB[0] && !CubeActive_PortalB[1] && !inPortalA && !inPortalB)
			{
				Vector4 clipPlane_PortalB = CameraSpacePlane(Cam_PortalB, GO_PortalA.transform.position, PortalNormal(GO_PortalA, Cam_PortalB, PortalForwardAxis), ClippingPlaneOffset);
				Cam_PortalB.projectionMatrix = Cam_Player.CalculateObliqueMatrix(clipPlane_PortalB);
			}
			else
				Cam_PortalB.ResetProjectionMatrix();

		}
	}
	
	Vector3 DivideVectors(Vector3 Numerator, Vector3 Denominator)
	{
		Vector3 result;

		if (Denominator.x != 0)
			result.x = Numerator.x / Denominator.x;
		else
			result.x = 0;
		if (Denominator.y != 0)


			result.y = Numerator.y / Denominator.y;
		else
			result.y = 0;


		if (Denominator.z != 0)
			result.z = Numerator.z / Denominator.z;
		else
			result.z = 0;

		return result;
	}

	bool ColliderContainsPoint(Transform ColliderTransform, Vector3 Point, bool Enabled)
	{
		Vector3 localPos = ColliderTransform.InverseTransformPoint(Point);
		if (Enabled && Mathf.Abs(localPos.x) < 0.5f && Mathf.Abs(localPos.y) < 0.5f && Mathf.Abs(localPos.z) < 0.5f)
			return true;
		else
			return false;
	}

	Vector3 PortalNormal(GameObject portal, Camera camera, AxisList forwardAxis)
	{
		switch (forwardAxis)
		{
			case (AxisList.x):
				if (Vector3.Angle(portal.transform.right, (camera.transform.position - portal.transform.position)) <= 90)
					return -portal.transform.right;
				else
					return portal.transform.right;

			case (AxisList.y):
				if (Vector3.Angle(portal.transform.up, (camera.transform.position - portal.transform.position)) <= 90)
					return -portal.transform.up;
				else
					return portal.transform.up;

			case (AxisList.z):
				if (Vector3.Angle(portal.transform.forward, (camera.transform.position - portal.transform.position)) <= 90)
					return -portal.transform.forward;
				else
					return portal.transform.forward;
		}

		return Vector3.zero;
	}


	Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float offset)
	{
		Vector3 offsetPos = pos - normal * offset;
		Matrix4x4 w2c = cam.worldToCameraMatrix;
		Vector3 posCamSpace = w2c.MultiplyPoint(offsetPos);
		Vector3 normalCamSpace = w2c.MultiplyVector(normal).normalized;
		return new Vector4(normalCamSpace.x, normalCamSpace.y, normalCamSpace.z, -Vector3.Dot(posCamSpace, normalCamSpace));
	}

	GameObject Create5SideInvertedCube(GameObject portal, Vector3 pos, Vector3 direction, Vector3 upVector, Vector3 scale, float offset, string name)
	{
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.name = name;
		cube.GetComponent<BoxCollider>().isTrigger = true;
		Mesh cubeMesh = cube.GetComponent<MeshFilter>().mesh;

		Vector3[] verts = new Vector3[24] {new Vector3 (-0.5f, 0.5f, -0.5f), new Vector3 (-0.5f, -0.5f, -0.5f), new Vector3 (0.5f, -0.5f, -0.5f), new Vector3 (0.5f, 0.5f, -0.5f), new Vector3 (-0.5f, 0.5f, 0.5f), new Vector3 (0.5f, 0.5f, 0.5f),
						  new Vector3 (0.5f, -0.5f, 0.5f), new Vector3 (-0.5f, -0.5f, 0.5f), new Vector3 (-0.5f, 0.5f, -0.5f), new Vector3 (-0.5f, -0.5f, -0.5f), new Vector3 (-0.5f, -0.5f, 0.5f), new Vector3 (-0.5f, 0.5f, 0.5f),
						  new Vector3 (-0.5f, -0.5f, -0.5f), new Vector3 (-0.5f, -0.5f, 0.5f), new Vector3 (0.5f, -0.5f, 0.5f), new Vector3 (0.5f, -0.5f, -0.5f), new Vector3 (0.5f, -0.5f, -0.5f), new Vector3 (0.5f, -0.5f, 0.5f),
						  new Vector3 (0.5f, 0.5f, 0.5f), new Vector3 (0.5f, 0.5f, -0.5f), new Vector3 (-0.5f, 0.5f, 0.5f), new Vector3 (-0.5f, 0.5f, -0.5f), new Vector3 (0.5f, 0.5f, -0.5f), new Vector3 (0.5f, 0.5f, 0.5f)};


		int[] tris = new int[36] { 0, 1, 2, 0, 2, 3, 4, 5, 6, 4, 6, 7, 8, 9, 10, 8, 10, 11, 12, 13, 14, 12, 14, 15, 16, 17, 18, 16, 18, 19, 20, 21, 22, 20, 22, 23 };

		cubeMesh.vertices = verts;
		cubeMesh.triangles = tris;

		Vector3 shift = Vector3.zero;
		switch (PortalForwardAxis)
		{
			case (AxisList.x):
				shift = portal.GetComponent<MeshFilter>().mesh.bounds.extents.x * portal.transform.lossyScale.x * direction;
				break;

			case (AxisList.y):
				shift = portal.GetComponent<MeshFilter>().mesh.bounds.extents.y * portal.transform.lossyScale.y * direction;
				break;

			case (AxisList.z):
				shift = portal.GetComponent<MeshFilter>().mesh.bounds.extents.z * portal.transform.lossyScale.z * direction;
				break;
		}

		cube.transform.SetPositionAndRotation(pos - direction * scale.z / 2 + shift + offset * direction, Quaternion.LookRotation(direction, upVector));
		cube.transform.RotateAround(cube.transform.position, cube.transform.up, 90);
		cube.transform.localScale = new Vector3(scale.z, scale.y, scale.x);
		cube.transform.parent = portal.transform;

		Material mat = new Material(portal.GetComponent<MeshRenderer>().material);
		mat.SetTexture("_MaskTex", null);
		mat.SetTexture("_BGTex", null);
		cube.GetComponent<MeshRenderer>().material = mat;

		return cube;
	}

	Vector3 NewColliderPosition(Vector3 c, Vector3 centre, Vector3 cL)
	{
		switch (PortalForwardAxis)
		{
			case (AxisList.x):
				c.x = Mathf.Abs(centre.x) * Mathf.Sign(-cL.x);
				break;

			case (AxisList.y):
				c.y = Mathf.Abs(centre.y) * Mathf.Sign(-cL.y);
				break;

			case (AxisList.z):
				c.z = Mathf.Abs(centre.z) * Mathf.Sign(-cL.z);
				break;
		}

		return c;
	}

	int GetActiveViewCubeIndex(Vector3 cL)
	{
		switch (PortalForwardAxis)
		{
			case (AxisList.x):
				return (int)(1 + Mathf.Sign(cL.x)) / 2;

			case (AxisList.y):
				return (int)(1 + Mathf.Sign(cL.y)) / 2;

			case (AxisList.z):
				return (int)(1 + Mathf.Sign(cL.z)) / 2;
		}
		return 0;
	}


	public bool DrawAxisTest;
	public void OnDrawGizmos()
	{
		if (DrawAxisTest)
		{
			if (PortalForwardAxis == AxisList.x)
			{
				Color gizColour = Gizmos.color;
				Gizmos.color = Color.red;
				Gizmos.DrawRay(GO_PortalA.transform.position, GO_PortalA.transform.right * 3);
				Gizmos.DrawRay(GO_PortalA.transform.position, -GO_PortalA.transform.right * 3);
				Gizmos.DrawRay(GO_PortalB.transform.position, GO_PortalB.transform.right * 3);
				Gizmos.DrawRay(GO_PortalB.transform.position, -GO_PortalB.transform.right * 3);
				Gizmos.color = gizColour;
			}
			if (PortalForwardAxis == AxisList.y)
			{
				Color gizColour = Gizmos.color;
				Gizmos.color = Color.green;
				Gizmos.DrawRay(GO_PortalA.transform.position, GO_PortalA.transform.up * 3);
				Gizmos.DrawRay(GO_PortalA.transform.position, -GO_PortalA.transform.up * 3);
				Gizmos.DrawRay(GO_PortalB.transform.position, GO_PortalB.transform.up * 3);
				Gizmos.DrawRay(GO_PortalB.transform.position, -GO_PortalB.transform.up * 3);
				Gizmos.color = gizColour;
			}
			if (PortalForwardAxis == AxisList.z)
			{
				Color gizColour = Gizmos.color;
				Gizmos.color = Color.blue;
				Gizmos.DrawRay(GO_PortalA.transform.position, GO_PortalA.transform.forward * 3);
				Gizmos.DrawRay(GO_PortalA.transform.position, -GO_PortalA.transform.forward * 3);
				Gizmos.DrawRay(GO_PortalB.transform.position, GO_PortalB.transform.forward * 3);
				Gizmos.DrawRay(GO_PortalB.transform.position, -GO_PortalB.transform.forward * 3);
				Gizmos.color = gizColour;
			}
		}
	}

}
