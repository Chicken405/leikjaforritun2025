using System.Collections;
using UnityEngine;

public class EaglePost : MonoBehaviour
{
    public bool flipFlop;

    public void MoveEaglePost(float range, float timer)
    {
        if (flipFlop)
        {
            transform.position += new Vector3(range, 0f, 0f);
        }
        else
        {
            transform.position += new Vector3(-range, 0f, 0f);
        }
        flipFlop = !flipFlop;
    }
}
