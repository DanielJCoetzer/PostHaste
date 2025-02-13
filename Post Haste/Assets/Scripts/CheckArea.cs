using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckArea : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == player) {
            player.GetComponent<ManageNeeds>().isAtFire = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == player) {
            player.GetComponent<ManageNeeds>().isAtFire = false;
        }
    }
}
