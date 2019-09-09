using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandSpriteOnAwake : MonoBehaviour
{
    [SerializeField] private float speed = 1;

    [SerializeField] private AnimationCurve xCurve;
    [SerializeField] private AnimationCurve yCurve;

    private float cValX;
    private float cValY;
    private float startTime;
    private int counter = 0;
    private float lastFrame = 0f;

    void Awake()
    {
        startTime = Time.time;
        gameObject.transform.localScale = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        cValX = this.xCurve.Evaluate((Time.time - startTime) * speed);
        cValY = this.yCurve.Evaluate((Time.time - startTime) * speed);

        if(cValX == lastFrame)
        {
            // Been on the same value for at least 2 frames
            counter++;
        } else
        {
            // Reset counter
            counter = 0;
            lastFrame = cValX;
        }

        if(counter > 5)
        {
            // Done with the animation
            Destroy(this);
        }

        gameObject.transform.localScale = new Vector3(cValX, cValY, 0);
    }
}
