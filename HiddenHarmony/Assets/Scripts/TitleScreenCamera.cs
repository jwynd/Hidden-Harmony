using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenCamera : MonoBehaviour
{
    public GameObject TitlePerspectives;
    private List<Transform> perspectives = new List<Transform>();
    private Transform t;
    private int currentPerspective; // child object index
    private Transform cameraPoint;
    private Transform cameraTarget;

    private float timer;
    public float timerSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //perspectives = TitlePerspectives.GetComponentsInChildren<Transform>();
        for(int i=0; i < TitlePerspectives.transform.childCount; i++)
        {
            t = TitlePerspectives.transform.GetChild(i);
            perspectives.Add(t);
        }

        currentPerspective = 0;
        timer = timerSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timer < 0)
        {
            // Change perspective
            //print("Changing perspective: " + perspectives[currentPerspective].gameObject.name);
            cameraPoint = perspectives[currentPerspective].GetChild(0);
            cameraTarget = perspectives[currentPerspective].GetChild(1);
            gameObject.transform.position = cameraPoint.position;
            gameObject.transform.LookAt(cameraTarget.position);

            timer = timerSpeed;
            currentPerspective = (currentPerspective + 1) % perspectives.Count;
        } else
        {
            // tick down timer
            timer -= Time.deltaTime;
        }
    }
}
