using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion2 : MonoBehaviour {

    // Destroy minion if collided with bullets //

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Turret_Bullet" || collision.gameObject.tag == "MinionBullet")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
