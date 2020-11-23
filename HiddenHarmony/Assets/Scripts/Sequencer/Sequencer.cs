using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Text.RegularExpressions;

public class Sequencer : MonoBehaviour
{
    private static Sequencer _instance;
    public Timekeeper timekeeper;
    //[HideInInspector]
    public int NumOfMeasures;
    public int CurrentMeasure;
    public int CurrentOffset;
    public delegate void MeasureChange();
    public static event MeasureChange Changed;
    private bool isCoroutineExecuting = false;
    //public static event MeasureChange;

    //How many bars are there?
    //Which offset is clicked on which bar
    public static Sequencer Instance { get { return _instance; } }
    // Bar# and the offset on it
    Dictionary<int, int> BarsOffets = new Dictionary<int, int>();

    //Depending on the current measure, we will have to get the the associated offset with the bar. 
    void Awake()
    {
        isCoroutineExecuting = false;
        CurrentOffset = 0;
        CurrentMeasure = 1;
        //timekeeper = GameObject.Find("GameplayObjects/Timekeeper").GetComponents<Timekeeper>(); 
        SequencerBar.OnVariableChange += UpdatedOffest;
        Changed += UpdateMeasure;
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        var foundSequencerBars = FindObjectsOfType<SequencerBar>();
        foreach (SequencerBar bar in foundSequencerBars)
        {
            //String.Split
            NumOfMeasures++;

            string input = bar.gameObject.name;
            string mynumber = Regex.Replace(input, @"\D", "");
            int ScriptNumber = int.Parse(mynumber);

            BarsOffets.Add(ScriptNumber, bar.SelectedBarOffest);
        }

    }
    void UpdatedOffest(int newOffset, int BarNumber)
    {
        BarsOffets[BarNumber] = newOffset;
        CurrentOffset = BarsOffets[CurrentMeasure];
        //Debug.Log(BarNumber);
        //Debug.Log(newOffset);

    }
    void UpdateMeasure()
    {
        CurrentMeasure++;
        if (CurrentMeasure > NumOfMeasures)
            CurrentMeasure = 1;
        CurrentOffset = BarsOffets[CurrentMeasure];
    }
    void OnDestroy()
    {
        SequencerBar.OnVariableChange -= UpdatedOffest;
    }
    void Update()
    {
        var i = timekeeper.CurrentHalfBeat();

        if (i % 32 == 0)
            StartCoroutine(CauseEvent()); // this ensures that the event is called only once and not multiple times in the same frame
    }
    IEnumerator CauseEvent()
    {

        if (isCoroutineExecuting)
            yield break;
        isCoroutineExecuting = true;
        yield return new WaitForSeconds(1.0f);
        Changed.Invoke();
        isCoroutineExecuting = false;
    }
}

