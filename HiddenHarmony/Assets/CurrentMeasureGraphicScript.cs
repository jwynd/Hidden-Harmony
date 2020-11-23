using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CurrentMeasureGraphicScript : MonoBehaviour
{
    private List<Vector2> BarLocations = new List<Vector2>();
    private int i;
    private RectTransform rect;
        // Start is called before the first frame update
    void Awake()
    {
        Sequencer.Changed += UpdateGraphicLocation;
        rect = GetComponent<RectTransform>();
        i = 0;
        var foundSequencerBars = FindObjectsOfType<SequencerBar>();
        //List<GameObject> Bars = new List<GameObject>();
        List<RectTransform> BarTransforms = new List<RectTransform>();
        foreach (SequencerBar barObject in foundSequencerBars)
        {
            BarTransforms.Add(barObject.gameObject.GetComponent<RectTransform>());

            BarTransforms = BarTransforms.OrderBy(RectTransform => RectTransform.gameObject.name).ToList(); // done
        }
        foreach (RectTransform bar in BarTransforms)
        {
            BarLocations.Add(bar.localPosition);
        }
        rect.localPosition = BarLocations[0];

    }
    void UpdateGraphicLocation()
    {
        i++;
        if ( i >= 4)
            i = 0;
        rect.localPosition = BarLocations[i];
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.V))
        {
            UpdateGraphicLocation();
        }

    }
}
