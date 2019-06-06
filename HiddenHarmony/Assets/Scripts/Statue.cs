﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{
    enum Statues
    {
        Subwoofer = 0,
        Belldeer = 1,
        Orcastra = 2
    }
    [SerializeField] private Statues statue = Statues.Subwoofer;
    [SerializeField] private AudioClip[] ac = new AudioClip[3];
    [SerializeField] private GameObject living;
    [SerializeField] private Material statueMaterial;
    [SerializeField][ColorUsageAttribute(true,true)] private Color emissionColor;
    private int itemCount = 0;

    private Material stoneMat;
    private Count counter;
    private AudioSource source;
    private GameObject s;
    private int lastItemCount;
    // Start is called before the first frame update
    void Start()
    {
        s = new GameObject("StatueAudio");
        source = s.AddComponent<AudioSource>();
        source.playOnAwake = false;
        stoneMat = new Material(statueMaterial);
        stoneMat.SetColor("_EmissionColor", emissionColor);
        foreach(Renderer rend in GetComponentsInChildren<Renderer>()){
            rend.material = stoneMat;
        }
        counter = GameObject.Find("GameplayObjects/Count").GetComponent<Count>();
    }

    // Update is called once per frame
    void Update()
    {
        lastItemCount = itemCount;
        if(statue == Statues.Subwoofer){
            itemCount = counter.DenCount();
        } else if (statue == Statues.Belldeer){
            itemCount = counter.ForestCount();
        } else if (statue == Statues.Orcastra){
            itemCount = counter.CavernCount();
        } else {
            Debug.LogError("Enum Error");
        }
        if(lastItemCount != itemCount){
            source.clip = ac[lastItemCount];
            source.Play();
        }
        if(itemCount < 3){
            stoneMat.SetInt("_CrackStage", itemCount);
        } else {
            living.SetActive(true);
            Destroy(this.gameObject);
        }

    }
}
