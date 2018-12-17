using UnityEngine;
public class ComboSystem
{
    public string[] ComboInputArray;
    private int currentIndex = 0;

    public float allowedTimeBetweenButtons = 0.3f; //tweak as needed
    private float timeLastButtonPressed;

    public ComboSystem(string[] tempComboArray)
    {
        ComboInputArray = tempComboArray;
    }

    //usage: call this once a frame. when the combo has been completed, it will return true
    public bool Check()
    {
        if (Time.time > timeLastButtonPressed + allowedTimeBetweenButtons) currentIndex = 0;
        {
            if (currentIndex < ComboInputArray.Length)
            {
                if ((ComboInputArray[currentIndex] == "a" && Input.GetKeyDown(KeyCode.A)) ||
                (ComboInputArray[currentIndex] == "s" && Input.GetKeyDown(KeyCode.S)) ||
                (ComboInputArray[currentIndex] == "d" && Input.GetKeyDown(KeyCode.D)) ||
                (ComboInputArray[currentIndex] != "a" && ComboInputArray[currentIndex] != "s" && ComboInputArray[currentIndex] != "d" && Input.GetButtonDown(ComboInputArray[currentIndex])))
                {
                    timeLastButtonPressed = Time.time;
                    currentIndex++;
                }

                if (currentIndex >= ComboInputArray.Length)
                {
                    currentIndex = 0;
                    return true;
                }
                else return false;
            }
        }

        return false;
    }
}
