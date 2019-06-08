using System.Collections;
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
    [SerializeField][Range(0.0f,1.0f)] private float volume = 0.75f;
    [SerializeField] private GameObject living;
    [SerializeField] private Material statueMaterial;
    [SerializeField][ColorUsageAttribute(true,true)] private Color emissionColor;
    private int itemCount = 0;

    private Material stoneMat;
    private Count counter;
    private AudioSource source;
    private GameObject s;
    private int lastItemCount;
    private ParticleSystem breakout;
    private ParticleSystem cracked1;
    private ParticleSystem cracked2;
    private ParticleSystem animalAwaits;
    private GameObject pedestal;
    // Start is called before the first frame update
    void Start()
    {
        s = new GameObject("StatueAudio");
        source = s.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.volume = volume;
        stoneMat = new Material(statueMaterial);
        stoneMat.SetColor("_EmissionColor", emissionColor);
        foreach(Renderer rend in GetComponentsInChildren<Renderer>()){
            rend.material = stoneMat;
        }
        counter = GameObject.Find("GameplayObjects/Count").GetComponent<Count>();
       if (this.gameObject.name != "coralDead"){
            if (statue == Statues.Subwoofer){
                pedestal = GameObject.FindGameObjectsWithTag("SubwooferPedestal")[0];
            }
            else if (statue == Statues.Belldeer){
                pedestal = GameObject.FindGameObjectsWithTag("BelldeerPedestal")[0];
            }
            else if (statue == Statues.Orcastra){
                pedestal = GameObject.FindGameObjectsWithTag("OrcastraPedestal")[0];
            }
            else
            {
                Debug.LogError("Enum Error");
            }
            cracked1 = pedestal.transform.Find("Cracked1").gameObject.GetComponent<ParticleSystem>();
            cracked2 = pedestal.transform.Find("Cracked2").gameObject.GetComponent<ParticleSystem>();
            breakout = pedestal.transform.Find("Breakout").gameObject.GetComponent<ParticleSystem>();
            animalAwaits = pedestal.transform.Find("AnimalAwaits").gameObject.GetComponent<ParticleSystem>();
        }
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
            if (this.gameObject.name != "coralDead"){
                breakout.Play();
            }
        }
        if (itemCount == 1 && this.gameObject.name != "coralDead" && !cracked1.isPlaying){
            cracked1.Stop();
            cracked1.Play();
        }
        else if (itemCount == 2 && this.gameObject.name != "coralDead" && !cracked2.isPlaying){
            if (cracked1.isPlaying){
                cracked1.Stop();
            }
            if (!cracked2.isPlaying)
            {
                cracked2.Play();
            }
        }
        if(itemCount < 3){
            stoneMat.SetInt("_CrackStage", itemCount);
        } else {
            living.SetActive(true);
            if (this.gameObject.name != "coralDead" && cracked1.isPlaying){
                cracked1.Stop();
            }
            if (this.gameObject.name != "coralDead" && cracked2.isPlaying){
                cracked2.Stop();
            }
            /*if (this.gameObject.name != "coralDead" && !animalAwaits.isPlaying){
                animalAwaits.Play();
            }*/
            Destroy(this.gameObject);
        }

    }
}
