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
    }

    // Uncommment if needed
    // void Update()
    // {
        
        
    // }

    // === PUBLIC METHODS === //
    #region PublicMethods
    // trigger Chad's idle "Horizontal Arm Wave" animation -- animation 0
    public void Chad_IdleWave(float speed = 0.5f)
    {
        thisAnim.SetFloat("spd0_idle", speed);
        thisAnim.SetTrigger("tgr0_idle");
    }

    // trigger Chad's idle "Looking Around" animation -- animation 5
    public void Chad_IdleLook(float speed = 0.5f)
    {
        thisAnim.SetFloat("spd5_idle", speed);
        thisAnim.SetTrigger("tgr5_idle");
    }

    // Chad will sit down, then remain in his idle sit (Animation 7 -> Animation 8)
    public void Chad_SitDown(float speed = 1.0f)
    {
        thisAnim.SetFloat("spd8_sitting", speed);
        thisAnim.SetTrigger("tgr7_sitDown");
    }

    // Chad will stand up, then go back to idle 5 (Animation 8->9->5)
    public void Chad_StandUp()
    {
        thisAnim.SetTrigger("tgr9_standUp");
    }

    // Chad will excitedly wave his arms, then go back to idle (Animation 2 -> 5)
    public void Chad_ExcitedWave(float speed = 1.0f){
        thisAnim.SetFloat("spd2_emote", speed);
        thisAnim.SetTrigger("tgr2_wave");
    }
    #endregion
    
}
