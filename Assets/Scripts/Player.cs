using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // Destroy Bullets that collide with player //

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Turret_Bullet" || collision.gameObject.tag == "Minion_Bullet")
        {
            Destroy(collision.gameObject);
        }
    }
}
