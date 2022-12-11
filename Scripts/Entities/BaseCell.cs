using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

public class BaseCell : MonoBehaviour
{
    private float FoodCount;
    public float FoodLoose = 5f;
    public float Speed = 1f;
    public float RotationSpeed = 4f;
    public float LookRange = 10f;
    public Rigidbody2D rigidBody2D;
    public float[] eyes;
    public float[] eyesDistance;
    void Start()
    {
        //Init
        rigidBody2D = GetComponent<Rigidbody2D>();
        FoodCount = 100f;
        eyes = new float[5] { 0, 0, 0, 0, 0 };
        eyesDistance = new float[5] { 0, 0, 0, 0, 0 };
    }
    void Update()
    {
        Look();
    }
    void Move()
    {
        transform.Translate(Time.deltaTime * Vector2.up * Speed);
    }
    void TurnRight()
    {
        transform.Rotate(Time.deltaTime * Vector3.forward * RotationSpeed);
    }
    void TurnLeft()
    {
        transform.Rotate(Time.deltaTime * Vector3.forward * -RotationSpeed);
    }
    void Look()
    {
        var hits = new float[5][];
        hits[0] = ResolveCollision(ShotRays(transform.up - (transform.right / 2)));
        hits[1] = ResolveCollision(ShotRays(transform.up - (transform.right / 4)));
        hits[2] = ResolveCollision(ShotRays(transform.up));
        hits[3] = ResolveCollision(ShotRays(transform.up + (transform.right / 4)));
        hits[4] = ResolveCollision(ShotRays(transform.up + (transform.right / 2)));
        for (int i = 0; i < eyes.Length; i++)
        {
            eyes[i] = hits[i][0];
            eyesDistance[i] = hits[i][1];
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * LookRange));
        Gizmos.DrawLine(transform.position, transform.position + ((transform.up + (transform.right / 2)) * LookRange));
        Gizmos.DrawLine(transform.position, transform.position + ((transform.up - (transform.right / 2)) * LookRange));
        Gizmos.DrawLine(transform.position, transform.position + ((transform.up - (transform.right / 4)) * LookRange));
        Gizmos.DrawLine(transform.position, transform.position + ((transform.up + (transform.right / 4)) * LookRange));
    }
    RaycastHit2D[] ShotRays(Vector3 direction)
    {
        return Physics2D.RaycastAll(transform.position, direction, LookRange);
    }

    float[] ResolveCollision(RaycastHit2D[] hits)
    {
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject == gameObject)
            {
                continue;
            }
            if (hit.collider.tag == "Food")
            {
                return new[] { 100, Vector3.Distance(transform.position, hit.collider.transform.position) };
            }
            if (hit.collider.tag == "Obs")
            {
                return new[] { -100, Vector3.Distance(transform.position, hit.collider.transform.position) };
            }
            if (hit.collider.tag == "Cell")
            {
                continue;
            }
        }
        return new[] { 0f, 0f };
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            Destroy(collision.gameObject);
        }
    }

}
