using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovedInside : MonoBehaviour
{
    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        player.isIndoors = true;
    }
}
