using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    Vector2 desiredDestination;
    public float speed = 4f;

    private bool playerFound = false;
    private GameObject player;
    private bool atHouse = false;
    public GameObject house;

    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFound) 
        {
            // Chase Player
            desiredDestination = rb.position - player.GetComponent<Rigidbody2D>().position;
        }
        else if(!atHouse) {
            // Move Normally
            desiredDestination = house.GetComponent<Rigidbody2D>().position - rb.position;
            if ((rb.position.x <= house.GetComponent<Rigidbody2D>().position.x + 1 && rb.position.x >= house.GetComponent<Rigidbody2D>().position.x - 1) && (rb.position.y <= house.GetComponent<Rigidbody2D>().position.y + 1 && rb.position.y >= house.GetComponent<Rigidbody2D>().position.y - 1)) {
                atHouse = true;
            }
        }
        else {
            desiredDestination = Vector2.zero;
            if (!((rb.position.x <= house.GetComponent<Rigidbody2D>().position.x + 1 && rb.position.x >= house.GetComponent<Rigidbody2D>().position.x - 1) && (rb.position.y <= house.GetComponent<Rigidbody2D>().position.y + 1 && rb.position.y >= house.GetComponent<Rigidbody2D>().position.y - 1))) {
                atHouse = false;
            }
        }

        Move();
    }

    void Move() 
    {
        desiredDestination = desiredDestination.normalized;

        Vector2 velocity = desiredDestination * speed;


        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == player) {
            playerFound = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == player) {
            playerFound = false;
        }
    }



}
