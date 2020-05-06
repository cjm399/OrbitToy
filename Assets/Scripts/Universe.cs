using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
    public float gravitationalConstant = .00000002f;
    public List<Planet> planets = new List<Planet>();

    public static Universe Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Update()
    {
        foreach(Planet planet in planets)
        {
            planet.UpdateVelocity(planets.ToArray(), Time.deltaTime);
            planet.UpdatePosition(Time.deltaTime);
        }
    }

}
