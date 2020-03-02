using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary_Controller : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}
