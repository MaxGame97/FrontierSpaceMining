using UnityEngine;
using System.Collections;

public class CannonBehaviour : MonoBehaviour {

    private Animator anim;

    private bool shooting = false;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("CannonShoot"))
        {
            shooting = true;
        }
        else
        {
            shooting = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Player" && shooting)
        other.gameObject.GetComponent<PlayerBehaviour>().KillPlayer();
    }

}
