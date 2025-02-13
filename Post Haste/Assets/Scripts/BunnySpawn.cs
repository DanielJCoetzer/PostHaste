using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BunnySpawn : MonoBehaviour
{
    public GameObject bunnyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GameObject bunny = Instantiate(bunnyPrefab, transform.position, transform.rotation);
        bunny.GetComponent<BunnyMovement>().house = this.GameObject();
    }
}
