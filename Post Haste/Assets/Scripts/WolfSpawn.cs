using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WolfSpawn : MonoBehaviour
{
    public GameObject wolfPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GameObject wolf = Instantiate(wolfPrefab, transform.position, transform.rotation);
        wolf.GetComponent<WolfMovement>().house = this.GameObject();
    }
}
