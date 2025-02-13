using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ManageNeeds : MonoBehaviour
{
    public float health = 100;
    private DayNightScript daytime;
    public float hunger = 100;
    public float composure = 100;
    public float temperature = 0;

    public bool isAtFire = false;
    public bool isAtWolf = false;
    public bool saving = false;

    private float prevHunger = 100;
    private float prevComposure = 100;
    private float prevTemperature = 0;
    private float prevHealth = 100;

    public Vector2 lastSave;

    // UI Variables
    public TMP_Text healthText;
    public TMP_Text hungerText;
    public TMP_Text composureText;
    public TMP_Text temperatureText;

    public TMP_Text berryText;
    public Image berryImage;
    public TMP_Text mushroomText;
    public Image mushroomImage;
    public TMP_Text badMushroomText;
    public Image badMushroomImage;
    public Image MeatImage;
    public TMP_Text meatText;
    public Image letterImage;
    public Image letterButton;
    public bool hasLetter = false;

    private int berries = 0;
    private bool atBerry = false;
    private GameObject currentBerry;

    private int mushrooms = 0;
    private bool atMushroom = false;
    private GameObject currentMushroom;

    private int badMushrooms = 0;
    private bool atBadMushroom = false;
    private GameObject currentBadMushroom;
    
    private Light2D lighting;

    private int meat = 0;



    // Start is called before the first frame update
    void Start()
    {
        lastSave = transform.position;
        daytime = GameObject.Find("Global Volume").GetComponent<DayNightScript>();
        lighting = GameObject.Find("Light 2D").GetComponent<Light2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ManageHealth();
        ManageHunger();
        ManageComposure();
        ManageTemperature();
        ManageUI();
    }

    void ManageHealth(){
        if (health <= 0) {
            Death();
        }
        if (temperature <= -20 || hunger <= 0) {
            health -= 3 * Time.deltaTime;
        }
        if (composure <= 0) {
            health /= 2;
            composure = 25;
        }
        if (health > 100) {
            health = 100;
        }
    }

    void ManageHunger(){
        if (hunger >= 0) {
            if (composure < 50) {
                hunger -= 1.3f * Time.deltaTime;
            }
            else {
                hunger -= 0.7f * Time.deltaTime;
            }
        }
        if (hunger > 100) {
            hunger = 100;
        }    
    }

    void ManageComposure() {
        if (isAtWolf) {
            composure -= 3 * Time.deltaTime;
        }
        if (hunger < 25) {
            composure -= 1 * Time.deltaTime;
        }
        else if (isAtFire) {
            composure += 1 * Time.deltaTime;
        }
        else if ((!daytime.isDay ||  temperature < 0) && composure > 0) {
            composure -= 1 * Time.deltaTime;
        }
        if (composure > 100) {
            composure = 100;
        }

        lighting.intensity = 100/composure;
    }

    void ManageTemperature() {
        if (daytime.isDay) {
            if (temperature < 10) {
                temperature += 1 * Time.deltaTime;
            }
        }
        else {
            if (isAtFire) 
            {
                if (temperature < 15) {
                    temperature += 1 * Time.deltaTime;
                }
            }
            else 
            {
                if (temperature >= -20) {
                    temperature -= 1 * Time.deltaTime;
                }
            }
            
        }
    }

    void Death() {
        transform.position = lastSave;
        health = prevHealth;
        temperature = prevTemperature;
        hunger = prevHunger;
        composure = prevComposure;

        berries = 0;
        mushrooms = 0;
        badMushrooms = 0;
        meat = 0;
        GetComponent<MakeCampfire>().wood = 0;
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if (context.interaction is HoldInteraction && isAtFire && !daytime.isDay) {
            saving = true;
            lastSave = transform.position;
            prevComposure = composure;
            prevHealth = health;
            prevHunger = hunger;
            prevTemperature = temperature;
        }

        if (context.interaction is PressInteraction && atBerry) {
            berries++;
            Destroy(currentBerry);
        }

        if (context.interaction is PressInteraction && atMushroom) {
            mushrooms++;
            Destroy(currentMushroom);
        }

        if (context.interaction is PressInteraction && atBadMushroom) {
            badMushrooms++;
            Destroy(currentBadMushroom);
        }
    }

    public void EatBerry() {
        if (berries > 0) {
            berries--;
            hunger += 10;
        }
    }

    public void EatMushroom() {
        if (mushrooms > 0) {
            mushrooms--;
            if (isAtFire) {
                hunger += 30;
                composure += 5;
                health += 5;
            }
            else {
                hunger += 15;
            }
            
        }
    }

    public void EatBadMushroom() {
        if (badMushrooms > 0) {
            badMushrooms--;
            if (!isAtFire) {
                hunger += 15;
                health -= 15;
                composure -= 15;
            }
            else {
                hunger += 30;
                composure += 20;
                health += 10;
            }
            
        }
    }

    public void EatMeat() {
        if (meat > 0) {
            meat--;
            if (isAtFire) {
                hunger += 50;
                composure += 10;
                health += 10;
            }
            else {
                hunger += 25;
            }
            
        }
    }



    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Berry")) {
            atBerry = true;
            currentBerry = other.gameObject;
        }

        if (other.gameObject.CompareTag("Mushroom")) {
            atMushroom = true;
            currentMushroom = other.gameObject;
        }

        if (other.gameObject.CompareTag("Bad Mushroom")) {
            atBadMushroom = true;
            currentBadMushroom = other.gameObject;
        }
        if (other.gameObject.CompareTag("Wolf")) {
            health -= 30;
        }
        if (other.gameObject.CompareTag("Bunny")) {
            Destroy(other.gameObject);
            meat++;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Berry")) {
            atBerry = false;
        }

        if (other.gameObject.CompareTag("Mushroom")) {
            atMushroom = false;
        }

        if (other.gameObject.CompareTag("Bad Mushroom")) {
            atBadMushroom = false;
        }
    }

    private void ManageUI() {
        healthText.text = "Health : " + ((int)health).ToString();
        hungerText.text = "Hunger : " + ((int)hunger).ToString();
        composureText.text = "Composure : " + ((int)composure).ToString();

        if (berries > 0) {
            berryText.text = berries.ToString();
            berryText.enabled = true;
            berryImage.enabled = true;
        }
        else {
            berryText.enabled = false;
            berryImage.enabled = false;
        }

        if (mushrooms > 0) {
            mushroomText.text = mushrooms.ToString();
            mushroomText.enabled = true;
            mushroomImage.enabled = true;
        }
        else {
            mushroomText.enabled = false;
            mushroomImage.enabled = false;
        }

        if (badMushrooms > 0) {
            badMushroomText.text = badMushrooms.ToString();
            badMushroomText.enabled = true;
            badMushroomImage.enabled = true;
        }
        else {
            badMushroomText.enabled = false;
            badMushroomImage.enabled = false;
        }

        if (meat > 0) {
            meatText.text = meat.ToString();
            meatText.enabled = true;
            MeatImage.enabled = true;
        }
        else {
            meatText.enabled = false;
            MeatImage.enabled = false;
        }

        if (hasLetter) {
            letterImage.enabled = true;
        }
        else {
            letterImage.enabled = false;
        }
    }

    public void GetDirections() {
        if (hasLetter) {
            letterButton.GetComponent<DialogueTrigger>().TriggerDialogue();
        }
    }
}
