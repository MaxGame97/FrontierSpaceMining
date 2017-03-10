using UnityEngine;
using System.Collections;

public class OpenGate : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pirate")
        {
            transform.parent.FindChild("LaserAnimationRed").gameObject.SetActive(true);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pirate")
        {
            transform.parent.FindChild("LaserAnimationRed").gameObject.SetActive(false);
        }
    }


}
