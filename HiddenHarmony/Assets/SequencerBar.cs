using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class SequencerBar : MonoBehaviour
{
    // Start is called before the first frame update
    public delegate void OnVariableChangeDelegate(int changedOffset, int BarNumber);
    public static event OnVariableChangeDelegate OnVariableChange;
    private int m_selectedBarOffest;
    private int ScriptNumber;
    public int SelectedBarOffest
    {
        get { return m_selectedBarOffest; }
        set
        {
            if (m_selectedBarOffest == value) return;
            m_selectedBarOffest = value;
            if (OnVariableChange != null)
                OnVariableChange(m_selectedBarOffest, ScriptNumber);
        }
    }
    

    public  Sprite OnSprite;
    private  Sprite OffSprite;
    private List<Button> SequencerButtons = new List<Button>();

    private Button LastButtonClicked;
    void Awake()
    {
        SelectedBarOffest = 0;
        string input = this.gameObject.name;
        string mynumber = Regex.Replace(input, @"\D", "");
        ScriptNumber = int.Parse(mynumber);


        foreach (Transform child in transform)
        {

            //String.Split, parse for int
            Button BarButton = child.gameObject.GetComponent<Button>();
            OffSprite = BarButton.image.sprite;
            if (BarButton != null)
            {
                SequencerButtons.Add(BarButton);
                BarButton.onClick.AddListener(delegate { ButtonClick(BarButton); });
                if (int.Parse(BarButton.gameObject.name) == 0)
                {
                    LastButtonClicked = BarButton;
                    LastButtonClicked.image.sprite = OnSprite;
                }


            }
        }
    }

    // Update is called once per frame
     void ButtonClick(Button BarButton)
    {
        if (LastButtonClicked != BarButton)
        {
            LastButtonClicked = BarButton;
            SelectedBarOffest = int.Parse(LastButtonClicked.name);
            foreach (Button buttons in SequencerButtons)
            {
                if(buttons == LastButtonClicked)
                {
                    LastButtonClicked.image.sprite = OnSprite;
                }
                else
                {
                    buttons.image.sprite = OffSprite;
                }
            }

        }

    }
    
}
