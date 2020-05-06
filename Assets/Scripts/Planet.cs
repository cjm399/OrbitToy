using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Planet : MonoBehaviour
{

    public float mass;
    public float radius;
    public Vector2 initialVelocity;

    private Vector2 currentVelocity;
    [HideInInspector]
    public Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        currentVelocity = initialVelocity;
    }

    private void Start()
    {
        Universe.Instance.planets.Add(this);
    }

    public void UpdateVelocity(Planet[] allPlanets, float timeStep)
    {
        foreach(var body in allPlanets)
        {
            if(body != this)
            {
                float sqrDst = (body.rigidBody.position - rigidBody.position).sqrMagnitude;
                Vector2 forceDir = (body.rigidBody.position - rigidBody.position).normalized;
                Vector2 force = forceDir * Universe.Instance.gravitationalConstant * mass * body.mass / sqrDst;
                Vector2 acceleration = force / mass;

                currentVelocity += acceleration * timeStep;
            }
        }
    }

    public void UpdatePosition(float timeStep)
    {
        rigidBody.position += currentVelocity * timeStep;
    }
}
