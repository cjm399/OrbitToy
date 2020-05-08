using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class Goal : MonoBehaviour
{
    public GameObject winText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        winText.SetActive(true);
    }

}
