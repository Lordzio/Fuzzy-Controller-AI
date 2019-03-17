using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour {

	public GameObject player;
	public float baseAgentSpeed = 8;
	private Vector3 position;
	public NavMeshAgent agent;
	private int Tactics = 0; //0 - walk 1 - charge
	private float chargeDuration = 0;
    private float chargeCooldown = 10;
    private bool retreat = false;
    private bool scatter = false;

    public int enemyID;

    public bool playerDetected = false;

    public bool leader = false;
    public GameObject enemyGroup;


    // Use this for initialization
    void Start () {
        for (int i = 0; i < enemyGroup.transform.childCount; i++)
            enemyGroup.transform.GetChild(i).GetComponent<EnemyBehaviour>().enemyID = i;

        enemyGroup.transform.GetChild(0).GetComponent<EnemyBehaviour>().leader = true;
    }
	

	public void ChangeTactics(int tactic) {
        Tactics = tactic;
    }
	

	// Update is called once per frame
	void Update () {
		UpdatePosition(); //Updates player's position

        if (chargeCooldown > 0) chargeCooldown -= Time.deltaTime;       

		if (Tactics == 0) {
            retreat = false;
            scatter = false;
			WalkForward();
		}
	
		if (Tactics == 1) {
            retreat = false;
            scatter = false;
			Charge();
		}

        if (Tactics == 2)
        {
            retreat = true;
            scatter = false;
            Retreat();
        }

        if (Tactics == 3)
        {
            retreat = false;
            scatter = true;
            Scatter();
        }

	}
	
	void WalkForward(){
        agent.isStopped = false;
		if (agent.speed > baseAgentSpeed) {
			agent.speed = baseAgentSpeed;
		}
		agent.SetDestination(new Vector3(position.x, position.y, position.z));
	}
	
	void Charge() {
        agent.isStopped = false;
        agent.SetDestination(new Vector3(position.x, position.y, position.z));
		if (agent.speed == baseAgentSpeed * 2) chargeDuration+=Time.deltaTime;
		if (chargeCooldown <= 0) {
            agent.speed = baseAgentSpeed * 2;
            chargeCooldown = 10;
            chargeDuration = 0;
        }
		else if (chargeDuration >= 3){
			agent.speed = baseAgentSpeed;
			chargeDuration=0;
		}
	}

    void Retreat()
    {
        agent.isStopped = false;
        if (agent.speed > baseAgentSpeed)
        {
            agent.speed = baseAgentSpeed;
        }

        float distanceToPlayer = Vector3.Distance(player.transform.position, enemyGroup.transform.GetChild(enemyID).position);
        if (leader)
        { 
            Vector3 playerPosition = player.transform.position;
            Vector3 leaderPosition = enemyGroup.transform.GetChild(enemyID).position;

            Vector3 vectorSub = leaderPosition - playerPosition;

            Vector3 destination = playerPosition + 35 * Vector3.Normalize(vectorSub);

            if (distanceToPlayer < 35)
                agent.SetDestination(destination);
            else
                Hold();
        }
        else
        {
            Vector3 leaderPosition = new Vector3();
            for (int i = 0; i < enemyGroup.transform.childCount; i++)
                if (enemyGroup.transform.GetChild(i).GetComponent<EnemyBehaviour>().leader == true)
                    leaderPosition = enemyGroup.transform.GetChild(i).position;

            float distanceToLeader = Vector3.Distance(leaderPosition, enemyGroup.transform.GetChild(enemyID).position);

            if (distanceToPlayer < 10)
            {
                Vector3 playerPosition = player.transform.position;

                Vector3 vectorSub = enemyGroup.transform.GetChild(enemyID).position - playerPosition;

                Vector3 destination = playerPosition + 35 * Vector3.Normalize(vectorSub);
                agent.SetDestination(destination);
            }
            else if (distanceToLeader > 7 )
                agent.SetDestination(new Vector3(leaderPosition.x, leaderPosition.y, leaderPosition.z));
            else
                Hold();
        }
    }

    void Scatter()
    {
        agent.isStopped = false;
        if (agent.speed > baseAgentSpeed)
        {
            agent.speed = baseAgentSpeed;
        }
        float distanceToPlayer = Vector3.Distance(player.transform.position, enemyGroup.transform.GetChild(enemyID).position);

        Vector3 playerPosition = player.transform.position;
        Vector3 leaderPosition = enemyGroup.transform.GetChild(enemyID).position;

        Vector3 vectorSub = leaderPosition - playerPosition;

        Vector3 destination = playerPosition + 35 * Vector3.Normalize(vectorSub);

        if (distanceToPlayer < 35)
            agent.SetDestination(destination);
        else
            Hold();
    }

    void Hold()
    {
        agent.isStopped = true;
    }
	
	void SetSpeed()
    {
        if (chargeDuration < 3)
        {
            if (agent.speed == baseAgentSpeed)
                agent.speed = baseAgentSpeed * 2;
        }
        else
        {
            agent.speed = baseAgentSpeed;
        }
    }
	void UpdatePosition() {
        float distance = Vector3.Distance(player.transform.position, enemyGroup.transform.GetChild(enemyID).position);
        if (!retreat && !scatter)
        {
            if (distance < 25 || playerDetected)
            {
                for (int i = 0; i < enemyGroup.transform.childCount; i++)
                    enemyGroup.transform.GetChild(i).GetComponent<EnemyBehaviour>().playerDetected = true;

                if (leader || distance < 15)
                    position = player.transform.position;
                else
                {
                    for (int i = 0; i < enemyGroup.transform.childCount; i++)
                        if (enemyGroup.transform.GetChild(i).GetComponent<EnemyBehaviour>().leader == true)
                        {
                            position = enemyGroup.transform.GetChild(i).position;
                            break;
                        }
                }
            }
            else
                Hold();
        }
    }
}
