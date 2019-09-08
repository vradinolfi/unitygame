using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : MonoBehaviour
{

    private Player player;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.health <= 200f && player.health > 151f)
        {
            this.GetComponent<RawImage>().color = Color.green;
        }
        else if (player.health < 151f && player.health > 101f)
        {
            this.GetComponent<RawImage>().color = Color.yellow;
        }
        else if (player.health < 101f && player.health > 51f)
        {
            this.GetComponent<RawImage>().color = Color.magenta;
        }
        else
        {
            this.GetComponent<RawImage>().color = Color.red;
        }
    }
}
