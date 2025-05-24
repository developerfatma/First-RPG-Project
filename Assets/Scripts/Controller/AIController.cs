using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Controller
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float aggroCooldownTime = 5f;
        [SerializeField] float waypointTolerence = 1f;
        [SerializeField] float waypointLifetime = 3f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float shoutDistance = 5f;

        [SerializeField] PatrolPath patrolPath;

        GameObject player;
        Fighter fighter;
        Health health;
        Mover mover;

        Vector3 enemyLocation;
        float timeSinceLastSawPlayer;
        float timeSinceArrivedWaypoint;
        float timeSinceAggrevate = Mathf.Infinity;
        
        int currentWaypointIndex = 0;
        
        void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            enemyLocation = transform.position;
        }

        
        void Update()
        {

            if (health.IsDead())
            {
                return;
            }

            if (IsAggrevated() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                fighter.Attack(player);

                AggrevateNearbyEnemies();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
            else
            {
                Vector3 nextPosition = enemyLocation;

                if (patrolPath != null)
                {
                    if (AtWaypoint())
                    {
                        timeSinceArrivedWaypoint = 0;
                        CycleWaypoint();
                    }

                    nextPosition = GetNextWaypoint();
                }

                if (timeSinceArrivedWaypoint > waypointLifetime)
                {
                    mover.StartMoveAction(nextPosition, patrolSpeedFraction);
                }

            }

            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedWaypoint += Time.deltaTime;
            timeSinceAggrevate += Time.deltaTime;

        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach(RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;

                ai.Aggrevate();
            }
        }

        private bool IsAggrevated()
        {
            return DistanceToPlayer() < chaseDistance || timeSinceAggrevate < aggroCooldownTime;
        }

        public void Aggrevate()
        {
            timeSinceAggrevate = 0;
        }

        

        private Vector3 GetNextWaypoint()
        {
            return patrolPath.GetWaypointPosition(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceWaypoint = Vector3.Distance(transform.position, GetNextWaypoint());
            return distanceWaypoint < waypointTolerence;
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}

