using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    GameObject cam;
    [SerializeField] float parallaxEffect;
    float length;
    float xPosition;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector2(xPosition + distanceToMove, transform.position.y);
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        if (distanceMoved > xPosition + length)
            xPosition += length;
    }
}
