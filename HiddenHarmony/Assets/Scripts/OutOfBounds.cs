using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private GameObject player;
    private Transform respawn;
    // Start is called before the first frame update
    void Start()
    {
        respawn = GameObject.Find("GameplayObjects/RespawnPoint").transform;
    }

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.y < -300f)
        {
            player.transform.position = respawn.position;
        }
    }
}
