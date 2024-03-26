using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private GameObject player;
    private Player playerScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        playerScript = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().isKinematic = playerScript.attributes[1].value.ModifiedValue < 10;
    }
}
