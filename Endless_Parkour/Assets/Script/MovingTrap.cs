using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrap : Trap
{
    [SerializeField] float movingSpeed;
    [SerializeField] float rotatingSpeed;
    [SerializeField] Transform[] movingPoint;
    int i = 0;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        transform.position = movingPoint[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, movingPoint[i].position) < 0.1f)
            i = i < movingPoint.Length - 1 ? i + 1 : 0;
        transform.position = Vector3.MoveTowards(transform.position, movingPoint[i].position, movingSpeed * Time.deltaTime);
        //If trap is moving to the right, spining clockwise, else counter - clock wise
        transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotatingSpeed* (transform.position.x > movingPoint[i].position.x ? 1 : -1))); 
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
