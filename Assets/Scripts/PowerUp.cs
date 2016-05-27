using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    float lifeTime = 15f;
    void Update()
    {
        gameObject.transform.Rotate(0, 0, 50 * Time.deltaTime);
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //ADD EFFECT/SCORE WHATEVER
            Destroy(gameObject);
        }
    }

    

}