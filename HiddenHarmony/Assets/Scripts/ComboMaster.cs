using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboMaster : MonoBehaviour
{
    public Text ComboDisplayText;

    private ComboSystem blue = new ComboSystem(new string[] { "a", "s", "d" });
    private ComboSystem green = new ComboSystem(new string[] { "a", "d", "s" });
    private ComboSystem red = new ComboSystem(new string[] { "s", "a", "d" });
    private ComboSystem yellow = new ComboSystem(new string[] { "s", "d", "a" });
    private ComboSystem purple = new ComboSystem(new string[] { "d", "a", "s" });
    private ComboSystem cyan = new ComboSystem(new string[] { "d", "s", "a" });

    private void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", Color.black);

        ComboDisplayText.text = "Press a combination of A, S, and D!";
    }

    void Update()
    {
        Renderer rend = GetComponent<Renderer>();

        if (blue.Check())
        {
            rend.material.SetColor("_Color", Color.blue);
            ComboDisplayText.text = "ASD: BLUE";
            Debug.Log("Sphere is Blue");
        }
        if (green.Check())
        {
            rend.material.SetColor("_Color", Color.green);
            ComboDisplayText.text = "ADS: GREEN";
            Debug.Log("Sphere is Green");
        }
        if (red.Check())
        {
            rend.material.SetColor("_Color", Color.red);
            ComboDisplayText.text = "SAD: RED";
            Debug.Log("Sphere is Red");
        }
        if (yellow.Check())
        {
            rend.material.SetColor("_Color", Color.yellow);
            ComboDisplayText.text = "SDA: YELLOW";
            Debug.Log("Sphere is Yellow");
        }
        if (purple.Check())
        {
            rend.material.SetColor("_Color", Color.magenta);
            ComboDisplayText.text = "DAS: PURPLE";
            Debug.Log("Sphere is Purple");
        }
        if (cyan.Check())
        {
            rend.material.SetColor("_Color", Color.cyan);
            ComboDisplayText.text = "DSA: CYAN";
            Debug.Log("Sphere is cyan");
        }
    }
}
