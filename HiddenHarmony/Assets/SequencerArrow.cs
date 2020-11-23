using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SequencerArrow : MonoBehaviour
{
    public List<Vector2> ArrowLocations = new List<Vector2>();

    public int numOfBars;
    public int i;

    // Start is called before the first frame update
    void Awake()
    {
        i = 1;
        var foundSequencerBars = FindObjectsOfType<SequencerBar>();
        List<GameObject> Bars = new List<GameObject>();
        foreach (SequencerBar barObject in foundSequencerBars)
        {
            numOfBars++;
            Bars.Add(barObject.gameObject);
            Bars = Bars.OrderBy(GameObject => GameObject.name).ToList(); // done
        }
        foreach (GameObject bar in Bars)
        {
            float YBarExtent = bar.GetComponent<RectTransform>().rect.height/2;
            Vector2 CalculatedArrowLocation = (Vector2)bar.GetComponent<RectTransform>().localPosition - new Vector2(0, YBarExtent);
            ArrowLocations.Add(CalculatedArrowLocation);
        }


    }

    // Update is called once per frame
    void UpdateArrowLocation()
    {
        if (numOfBars % i == 0)
            i = 1;

        GetComponent<RectTransform>().localPosition = ArrowLocations[i-1];
        i++;
    } 
    void Update()
    {
        if (Input.GetKey(KeyCode.V))
        {
            UpdateArrowLocation();
        }

    }
}
