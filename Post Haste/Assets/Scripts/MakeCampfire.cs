using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using TMPro;
using UnityEngine.Rendering.Universal;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class MakeCampfire : MonoBehaviour
{
    public GameObject campfirePrefab;
    private DayNightScript dayNightScript;
    public int wood = 0;
    private bool atWood = false;
    public int fireReq = 8;
    private GameObject currentWood;

    public TMP_Text woodText;
    public Image woodImage;
    // Start is called before the first frame update
    void Awake()
    {
        dayNightScript = GameObject.Find("Global Volume").GetComponent<DayNightScript>();
        
    }
    public void OnMakeFire(InputAction.CallbackContext context) {
        if (wood >= fireReq) {
            GameObject fire = Instantiate(campfirePrefab, transform.position, transform.rotation);
            if (dayNightScript.isDay) {
                fire.GetComponentInChildren<Light2D>().intensity = 0;
            }
            dayNightScript.lights.Add(fire);
            wood -= fireReq;
        }
        
    }

    void Update() {
        if (wood >0) {
            woodImage.enabled = true;
            woodText.enabled = true;
            woodText.text = wood.ToString();
        }
        else {
            woodImage.enabled = false;
            woodText.enabled = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if (context.interaction is PressInteraction && atWood) {
            wood++;
            Destroy(currentWood);
            atWood = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Wood")) {
            atWood = true;
            currentWood = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Wood")) {
            atWood = false;
        }
    }
}
