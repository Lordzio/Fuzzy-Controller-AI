using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupTacticControl : MonoBehaviour {

    private FuzzyController fuzzyController;
    public GameObject npcGroup;
    public GameObject player;
    public Text playerAggressionText;
    public Text playerEffectivenessText;
    public int playerAggression;
    public int playerEffectiveness;
    private float tacTimer = 0;
    private float timer1 = 0;
    private float timer2 = 0;
    private float timer3 = 0;
    private float timerFear = 0;
    private int Tactics = 1;
    private int shots = 0;
    private Vector3 oldPlayerPosition, playerVec, enemyVec;
    public bool lockAggression = false;
    public bool lockEffectiveness = false;
    // Use this for initialization
    void Start () {
        fuzzyController = new FuzzyController();
        oldPlayerPosition = new Vector3();
        playerVec = new Vector3();
        enemyVec = new Vector3();
        oldPlayerPosition = player.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {

        timer1 += Time.deltaTime;
        timer2 += Time.deltaTime;
        timer3 += Time.deltaTime;
        if (timerFear > 0)
        {
            playerAggression = 100;
            playerEffectiveness = 100;

            timerFear -= Time.deltaTime;
        }
        else
        {
            if (!lockAggression)
            {

                if (timer1 >= 0.05)
                {
                    if (npcGroup != null)
                    {
                        if (npcGroup.transform.GetChild(0) != null)
                        {

                            if (oldPlayerPosition != player.transform.position)
                            {

                                playerVec = player.transform.position - oldPlayerPosition;
                                enemyVec = npcGroup.transform.GetChild(0).transform.position - player.transform.position;

                                float angle = Vector3.Angle(enemyVec, playerVec);

                                if (angle <= 45)
                                    if (playerAggression < 98)
                                        playerAggression += 3;
                                if (angle >= 135)
                                    if (playerAggression > 2)
                                        playerAggression -= 3;


                            }
                            oldPlayerPosition = player.transform.position;

                        }
                    }
                    timer1 = 0;
                }
            }

            if (timer2 >= 0.5)
            {
                if (!lockAggression)
                {
                    if (shots >= 2)
                    {
                        if (playerAggression < 96)
                            playerAggression += 5;
                    }
                    else if (playerAggression > 0)
                        playerAggression -= 1;

                    shots = 0;
                    timer2 = 0;
                }
            }

            if (timer3 >= 0.5)
            {
                if (!lockEffectiveness)
                    if (playerEffectiveness > 2)
                        playerEffectiveness -= 3;

                timer3 = 0;
            }
        }
        playerAggressionText.text = "Player Aggression: " + playerAggression;
        playerEffectivenessText.text = "Player Effectiveness: " + playerEffectiveness;
        UpdateTactics();
    }


    void UpdateTactics()
    {
        tacTimer += Time.deltaTime;
        if (tacTimer >= 1)
        {
            Tactics = fuzzyController.GetStrategy(playerAggression, playerEffectiveness);
            foreach(Transform NPC in npcGroup.transform)
            {
                NPC.GetComponent<EnemyBehaviour>().ChangeTactics(Tactics);
                if (Tactics == 0)
                    NPC.GetComponent<EnemyLife>().ProvideTactics("Attack");
                if (Tactics == 1)
                    NPC.GetComponent<EnemyLife>().ProvideTactics("Charge");
                if (Tactics == 2)
                    NPC.GetComponent<EnemyLife>().ProvideTactics("Retreat");
                if (Tactics == 3)
                    NPC.GetComponent<EnemyLife>().ProvideTactics("Panic");
            }

            tacTimer = 0;
        }
    }

    public void PlayerShot()
    {
        shots++;
    }

    public void PlayerShotHit()
    {
        if (playerEffectiveness < 91)
            playerEffectiveness += 10;
    }

    public void PlayerFear(float time)
    {
        timerFear = time;
    }
}
