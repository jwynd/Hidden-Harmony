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
    [SerializeField] private GameObject living;
    [SerializeField] private Material statueMaterial;
    private int itemCount = 0;

    private Material stoneMat;
    private Count counter;
    // Start is called before the first frame update
    void Start()
    {
        stoneMat = new Material(statueMaterial);
        foreach(Renderer rend in GetComponentsInChildren<Renderer>()){
            rend.material = stoneMat;
        }
        counter = GameObject.Find("GameplayObjects/Count").GetComponent<Count>();
    }

    // Update is called once per frame
    void Update()
    {
        if(statue == Statues.Subwoofer){
            itemCount = counter.DenCount();
        } else if (statue == Statues.Belldeer){
            itemCount = counter.ForestCount();
        } else if (statue == Statues.Orcastra){
            itemCount = counter.CavernCount();
        } else {
            print("Enum Error");
        }
        if(itemCount < 3){
            stoneMat.SetInt("_CrackStage", itemCount);
        } else {
            living.SetActive(true);
            Destroy(this.gameObject);
        }

    }
}
