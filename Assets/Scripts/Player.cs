using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float moveSpeed = 25f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
        {
            transform.position += new Vector3(Input.GetAxisRaw("Horizontal") / moveSpeed, 0f, 0f);
        }
        if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            transform.position += new Vector3(0f, Input.GetAxisRaw("Vertical") / moveSpeed, 0f);
        }
    }
}
