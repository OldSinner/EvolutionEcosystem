using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float Speed { get; set; }
    void Start()
    {
        Speed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontal, vertical, 0) * Speed * Time.deltaTime);
    }
}
