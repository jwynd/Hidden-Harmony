using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishOffset : MonoBehaviour
{
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        MeshRenderer renderer;
        speed = Random.Range(3.0f, 5.0f);

        props.SetFloat("_Speed", speed);

        renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.SetPropertyBlock(props);
        
    }
}
