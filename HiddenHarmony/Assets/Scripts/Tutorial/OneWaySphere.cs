using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OneWaySphere : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        player = GameObject.Find("Player");
        // Invert Mesh
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();

        gameObject.AddComponent<MeshCollider>();
    }
}
