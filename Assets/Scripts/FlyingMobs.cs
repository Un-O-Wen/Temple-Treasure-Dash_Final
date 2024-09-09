using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMobs : MonoBehaviour
{
    public float flightSpeed = 3f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone damageDetectionZone;
    public List<Transform> waypoints;
    public Collider2D deathCollider;

    Animator animator;
    Rigidbody2D rb;
    Damageable damageable;

    Transform nextWayPoint;
    int waypointNum = 0;

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        nextWayPoint = waypoints[waypointNum];
    }

    private void OnEnable()
    {
        damageable.damageableDeath.AddListener(OnDeath);
    }
    // Update is called once per frame
    void Update()
    {
        HasTarget = damageDetectionZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void Flight()
    {
        //Fly to next waypoint
        Vector2 directionToWayPoint = (nextWayPoint.position - transform.position).normalized;

        //Check if we have reached waypoint
        float distance = Vector2.Distance(nextWayPoint.position, transform.position);

        rb.velocity = directionToWayPoint * flightSpeed;
        UpdateDirection();
        //See if we need to switch waypoints
        if (distance <= waypointReachedDistance)
        {
            //switches to next waypoint
            waypointNum++;

            if(waypointNum >= waypoints.Count)
            {
                //loops back to orig waypoint
                waypointNum = 0;
            }

            nextWayPoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;
        if (transform.localScale.x > 0)
        {
            //Right
            if(rb.velocity.x < 0)
            {
                //Flips
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            //Left
            if (rb.velocity.x > 0)
            {
                //Flips
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }

        }
    }

    public void OnDeath()
    {
        rb.gravityScale = 2f;
        rb.velocity = new Vector2(0, rb.velocity.y);
        deathCollider.enabled = true;
    }

}
