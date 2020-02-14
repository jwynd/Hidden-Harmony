using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChadAnimationCaller : MonoBehaviour
{
    private Animator thisAnim;

    // Start is called before the first frame update
    void Start()
    {
        thisAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    // === PUBLIC METHODS === //
    // trigger Chad's idle "Horizontal Arm Wave" animation -- animation 0
    public void Chad_IdleWave(double speed = 0.5)
    {
        thisAnim.SetFloat("spd0_idle", (float)speed);
        thisAnim.SetTrigger("tgr0_idle");
    }

    // trigger Chad's idle "Looking Around" animation -- animation 5
    public void Chad_IdleLook(double speed = 0.5)
    {
        thisAnim.SetFloat("spd5_idle", (float)speed);
        thisAnim.SetTrigger("tgr5_idle");
    }

    // Chad will sit down, then remain in his idle sit (Animation 7 -> Animation 8)
    public void Chad_SitDown(double speed = 1.0)
    {
        thisAnim.SetFloat("spd8_sitting", (float)speed);
        thisAnim.SetTrigger("tgr7_sitDown");
    }

    // Chad will stand up, then go back to idle 5 (Animation 8->9->5)
    public void Chad_StandUp()
    {
        thisAnim.SetTrigger("tgr9_standUp");
    }

    // Chad will excitedly wave his arms, then go back to idle (Animation 2 -> 5)
    public void Chad_ExcitedWave(double speed = 1.0){
        thisAnim.SetFloat("spd2_emote", (float)speed);
        thisAnim.SetTrigger("tgr2_wave");
    }

    
}
