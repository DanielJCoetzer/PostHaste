using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using TMPro; // using text mesh for the clock display
 
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // used to access the volume component

public class DayNightScript : MonoBehaviour
{
    private ManageNeeds needs;
    public TextMeshProUGUI timeDisplay; // Display Time
    public TextMeshProUGUI dayDisplay; // Display Day
    public Volume ppv; // this is the post processing volume
    public float campfireSpeed = 5f;
    public float campfireIntensity = 5f;
    private float campfireTime = 0;
    private bool intensityReached = true;

    public TMP_Text saveText;
    public TMP_Text surviveText;
    private int textCounter = 0;
 
    public float tick; // Increasing the tick, increases second rate
    public float seconds; 
    public int mins;
    public int hours;
    public int days = 1;

    public bool isDay = false;
    private bool updated = false;
 
    public bool activateLights; // checks if lights are on
    //public GameObject[] lights; // all the lights we want on when its dark
    public List<GameObject> lights = new List<GameObject>();
    //public SpriteRenderer[] stars; // star sprites 
    // Start is called before the first frame update
    void Start()
    {
        ppv = gameObject.GetComponent<Volume>();
        ppv.weight = 0.1f;
        needs = GameObject.Find("Player").GetComponent<ManageNeeds>();
    }
 
    // Update is called once per frame
    void FixedUpdate() // we used fixed update, since update is frame dependant. 
    {
        CalcTime();
        DisplayTime();
        ManageSave();
    }
 
    public void CalcTime() // Used to calculate sec, min and hours
    {
        seconds += Time.fixedDeltaTime * tick; // multiply time between fixed update by tick
 
        if (seconds >= 60) // 60 sec = 1 min
        {
            seconds = 0;
            mins += 1;

            if (!intensityReached) {
                campfireTime += 1;
            }
            else {
                campfireTime = 0;
            }
            if (needs.saving) {
                textCounter++;
            }
            
        }
 
        if (mins >= 60) //60 min = 1 hr
        {
            mins = 0;
            hours += 1;
        }
 
        if (hours >= 24) //24 hr = 1 day
        {
            hours = 0;
            days += 1;
        }
        ControlPPV(); // changes post processing volume after calculation
    }
 
    public void ControlPPV() // used to adjust the post processing slider.
    {
        if (!needs.saving) {
            // NIGHT TIME
            if(hours>=21 || hours<6) // dusk at 21:00 / 9pm    -   until 22:00 / 10pm
            {
                if (ppv.weight <0.7f) {
                    ppv.weight =  (float)mins / 60; // since dusk is 1 hr, we just divide the mins by 60 which will slowly increase from 0 - 1 
                }

                if (hours<22) {
                    for (int i = 0; i < lights.Count; i++)
                    {
                        if (lights[i].GetComponentInChildren<Light2D>().intensity < campfireIntensity) {
                            intensityReached = false;
                            lights[i].GetComponentInChildren<Light2D>().intensity = (float)campfireTime/12;
                        }
                        else {
                            intensityReached = true;
                        }
                        if (lights[i].GetComponentInChildren<Light2D>().intensity > campfireIntensity) {
                            lights[i].GetComponentInChildren<Light2D>().intensity = campfireIntensity;
                        }
                    }
                }
            }
            if (hours>=22 || hours<6) {
                isDay = false;
            }
            // DAY TIME
            if(hours>=6 && hours<21) // Dawn at 6:00 / 6am    -   until 7:00 / 7am
            {
                isDay = true;
                if (ppv.weight > 0.1f) {
                    ppv.weight =  0.8f - (float)mins / 60; // since dusk is 1 hr, we just divide the mins by 60 which will slowly increase from 0 - 1 
                }

                for (int i = 0; i < lights.Count; i++)
                {
                    if (lights[i].GetComponentInChildren<Light2D>().intensity > 0.2f) {
                        intensityReached = false;
                        lights[i].GetComponentInChildren<Light2D>().intensity = campfireIntensity - (float)campfireTime/10;
                    }
                    else {
                        intensityReached = true;

                    }
                }
            }
        }

        // Saving
        else {
            if (ppv.weight <1f) {
                    ppv.weight =  (float)textCounter+ 0.7f/ 5; // since dusk is 1 hr, we just divide the mins by 60 which will slowly increase from 0 - 1 
                    for (int i = 0; i < lights.Count; i++)
                    {
                        if (lights[i].GetComponentInChildren<Light2D>().intensity > 0f) {
                            intensityReached = false;
                            lights[i].GetComponentInChildren<Light2D>().intensity = campfireIntensity - (float)textCounter;
                        }
                    }
                }
            if (textCounter == 8 && updated == false) {
                updated = true;
                hours = 6;
                mins = 0;
                if (hours < 20) {
                    days++;
                }
            }
            if (textCounter < 2) {
                updated = false;
            }
            campfireTime = 0;
        }
    }

    void ManageSave() {
        if (needs.saving && textCounter < 10) {
            saveText.text = "Saved";
            if (days == 0 || (days == 1 && hours < 7)) {
                surviveText.text = "You Survived 1 Day";
            }
            else
            {
                if (hours < 7) {
                    surviveText.text = "You Survived " + (days) + " Days";
                }
                else {
                    surviveText.text = "You Survived " + (days + 1) + " Days";
                }
                
            }
        }
        else {
            needs.saving = false;
            textCounter = 0;
            surviveText.text = "";
            saveText.text = "";
        }
    }
 
    public void DisplayTime() // Shows time and day in ui
    {
 
        //timeDisplay.text = string.Format("{0:00}:{1:00}", hours, mins); // The formatting ensures that there will always be 0's in empty spaces
        //dayDisplay.text = "Day: " + days; // display day counter
    }

}
