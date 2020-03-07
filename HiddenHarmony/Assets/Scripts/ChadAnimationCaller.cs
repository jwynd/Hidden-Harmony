using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChadAnimationCaller : MonoBehaviour
{
    public static ChadAnimationCaller Instance; // singleton construction
    private Animator thisAnim;

    void Awake()
    {
        // this region must remain at the top of Awake
        #region SingletonConstruction
        if(Instance != null && Instance != this){
            Debug.LogError("Attempting to create two instances of Singleton design pattern: ChadAnimationController");
            Destroy(this);
        }
        Instance = this;
        #endregion
    }
    // Start is called before the first frame update
    void Start()
    {
        thisAnim = GetComponent<Animator>();
        thisAnim.SetBool("isSitting", false);
    }

    // Uncommment if needed
    // void Update()
    // {
        
        
    // }

    // === PUBLIC METHODS === //
    #region PublicMethods

    // triggers/returns Chad's animation back to his base idle (idle 5)
    // This is nearly identical to Chad_IdleLook, except it will
    //      check to see if Chad is in a state that requires a special
    //      exit, and calls that exit animation.
    public void Chad_ReturnToBase(float speed = 0.5f)
    {
        thisAnim.SetFloat("spd5_idle", speed);

        // check to see if chad is sitting
        if(thisAnim.GetBool("isSitting")){
            // if chad is sitting, call StandUp
            // this will automatically return to base idle
            Chad_StandUp();
        } else {
            // trigger idle 5
            thisAnim.SetTrigger("tgr5_idle");
        }
    }


    // trigger Chad's idle "Horizontal Arm Wave" animation (Animation 0)
    //  To exit, must return to base idle (IdleLook)
    public void Chad_IdleWave(float speed = 0.5f)
    {
        thisAnim.SetFloat("spd0_idle", speed);
        thisAnim.SetTrigger("tgr0_idle");
    }

    // trigger Chad's Base idle "Looking Around" animation (Animation 5)
    //   This animation connects to all others.
    public void Chad_IdleLook(float speed = 0.5f)
    {
        thisAnim.SetFloat("spd5_idle", speed);
        thisAnim.SetTrigger("tgr5_idle");
    }

    // Chad will sit down, then remain in his idle sit (Animation 7 -> Animation 8)
    //  To exit, must call ChadStandUp(), which will return to base idle
    public void Chad_SitDown(float speed = 1.0f)
    {
        thisAnim.SetBool("isSitting", true);
        thisAnim.SetFloat("spd8_sitting", speed);
        thisAnim.SetTrigger("tgr7_sitDown");
    }

    // Chad will stand up, then go back to idle 5 (Animation 8->9->5)
    public void Chad_StandUp()
    {
        thisAnim.SetBool("isSitting", false);
        thisAnim.SetTrigger("tgr9_standUp");
    }

    // Chad will excitedly wave his arms, then return to idle (Animation 2 -> 5)
    public void Chad_ExcitedWave(float speed = 1.0f)
    {
        thisAnim.SetFloat("spd2_emote", speed);
        thisAnim.SetTrigger("tgr2_wave");
    }

    // Chad will tip his mushroom like a hat, then return to idle (Animation 3->5)
    public void Chad_HatTip(float speed = 1.0f)
    {
        thisAnim.SetFloat("spd3_emote", speed);
        thisAnim.SetTrigger("tgr3_hatTip");
    }

    // Chad will somberly shake his head, then return to idle (Animation 1->5)
    public void Chad_SadHeadShake(float speed = 1.0f)
    {
        thisAnim.SetFloat("spd1_emote", speed);
        thisAnim.SetTrigger("tgr1_headShake");
    }

    // Chad will bow, then return to idle (Animation 4->5)
    public void Chad_Bow(float speed = 1.0f)
    {
        thisAnim.SetFloat("spd4_emote", speed);
        thisAnim.SetTrigger("tgr4_emote");
    }

    // Chad will float idlly, and loop (Animation 6)
    //  To end the loop, must return to base idle (5)
    public void Chad_IdleFloat(float speed = 1.0f)
    {
        thisAnim.SetFloat("spd6_idle", speed);
        thisAnim.SetTrigger("tgr6_idle");
    }

    #endregion
    
}
