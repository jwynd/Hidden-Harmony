using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChadAnimationTest : MonoBehaviour
{
    public GameObject ChadRef;
    private ChadAnimationCaller chadAnim;

    // Start is called before the first frame update
    void Start()
    {
        chadAnim = ChadRef.GetComponent<ChadAnimationCaller>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // === TESTING === //
        // if "5" is pressed, go to idle 5
        if(Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5)){
            chadAnim.Chad_IdleLook();
        }

        
        // if "0" is pressed, go to idle 0
        if(Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0)){
            chadAnim.Chad_IdleWave();
        }

        // if "7" is pressed, sit down
        if(Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7)){
            chadAnim.Chad_SitDown();
        }

        // if "9" is pressed, stand up
        if(Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.Alpha9)){
            chadAnim.Chad_StandUp();
        }

        // if "2" is pressed, wave arms excitedly
        if(Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2)){
            chadAnim.Chad_ExcitedWave();
        }

        // NEW

        // if "3" is pressed, tips Mushroom
        if(Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3)){
            chadAnim.Chad_HatTip();
        }

        // if "1" is pressed
        if(Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1)){
            chadAnim.Chad_SadHeadShake();
        }

        // if "4" is pressed
        if(Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4)){
            chadAnim.Chad_Bow();
        }

        // if "6" is pressed
        if(Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6)){
            chadAnim.Chad_IdleFloat();
        }
    }
}
