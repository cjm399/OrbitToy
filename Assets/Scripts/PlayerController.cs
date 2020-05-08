using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject playerBallPrefab;
    public float scrollAmount;
    public float massDensity = 20f;

    public Sprite planetSprite;
    public LineRenderer arrowLineRenderer;


    private GameObject currentlyConstructedPlanetGO;
    private Planet currentlyConstructedPlanet;

    private bool isDrawing = false;
    private bool isGivingVelocity = false;
    private Vector3 mouseClickPosition = Vector3.zero;

    private float previousRadius = 1f;
    private float previousMass = 5f;
    private Vector2 previousVelocity = Vector2.zero;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
            worldPoint.z = 0;
            SpawnPlanet(worldPoint, previousRadius, previousMass, previousVelocity);
        }

        if(!isDrawing && !isGivingVelocity && Input.GetKeyDown(KeyCode.Mouse1))
        {
            isDrawing = true;
            mouseClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
            mouseClickPosition.z = 0;
            StartCoroutine(ShowPlanetVisualization());
        }
        else if(isDrawing && Input.GetKeyUp(KeyCode.Mouse1))
        {
            Vector3 mouseEndPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
            mouseEndPosition.z = 0;

            previousMass = massDensity * previousRadius;
            isGivingVelocity = true;
            isDrawing = false;
            StartCoroutine(ShowPlanetVelocity());
        }

        if(isGivingVelocity && Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector3 VelocityPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
            VelocityPosition.z = 0;

            float distanceFromCenter = Vector3.Distance(VelocityPosition, mouseClickPosition);

            float speed = distanceFromCenter - previousRadius;

            if (speed < 0)
                speed = 0;
            //speed = Mathf.Clamp(speed, 0, 80);

            Vector3 direction =  VelocityPosition - mouseClickPosition;
            direction = direction.normalized;

            previousVelocity = new Vector2(direction.x, direction.y) * speed;

            currentlyConstructedPlanet = currentlyConstructedPlanetGO.AddComponent<Planet>();

            currentlyConstructedPlanet.mass = previousMass;
            currentlyConstructedPlanet.radius = previousRadius;
            currentlyConstructedPlanet.initialVelocity = previousVelocity;

            currentlyConstructedPlanetGO.AddComponent<OnCollisionEnterDestroy>();

            isGivingVelocity = false;
        }

        CameraInput();
    }


    private void CameraInput()
    {
        if (Camera.main.orthographicSize > scrollAmount && Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Camera.main.orthographicSize -= scrollAmount;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.orthographicSize += scrollAmount;
        }
    }

    private void SpawnPlanet(Vector3 _spawnLocation, float _radius, float _mass, Vector2 _initialVelocity)
    {
        GameObject planet = Instantiate(playerBallPrefab, _spawnLocation, Quaternion.identity, null);
        planet.transform.localScale = new Vector3(_radius * 2, _radius * 2);
        Planet planetPlanet = planet.GetComponent<Planet>();
        planet.AddComponent<OnCollisionEnterDestroy>();


        planetPlanet.mass = _mass;
        planetPlanet.radius = _radius;
        planetPlanet.initialVelocity = _initialVelocity;
    }


    private IEnumerator ShowPlanetVisualization()
    {
        currentlyConstructedPlanetGO = new GameObject();
        currentlyConstructedPlanetGO.transform.parent = this.transform;
        currentlyConstructedPlanetGO.transform.position = mouseClickPosition;
        SpriteRenderer sRend = currentlyConstructedPlanetGO.AddComponent<SpriteRenderer>();
        currentlyConstructedPlanetGO.AddComponent<CircleCollider2D>();
        Rigidbody2D body = currentlyConstructedPlanetGO.AddComponent<Rigidbody2D>();
        body.gravityScale = 0;
        sRend.sprite = planetSprite;

        while (isDrawing)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
            mousePosition.z = 0;
            previousRadius = Vector3.Distance(mouseClickPosition, mousePosition);
            Vector3 size = new Vector3(previousRadius * 2, previousRadius * 2);
            currentlyConstructedPlanetGO.transform.localScale = size;

            yield return null;
        }
    }

    private IEnumerator ShowPlanetVelocity()
    {
        arrowLineRenderer.gameObject.SetActive(true);
        arrowLineRenderer.SetPosition(0, mouseClickPosition);
        while (isGivingVelocity)
        {
            Vector3 VelocityPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
            arrowLineRenderer.SetPosition(1, VelocityPosition);

            yield return null;
        }

        arrowLineRenderer.gameObject.SetActive(false);
    }

}
