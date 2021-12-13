using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingEnvController : MonoBehaviour
{
    [SerializeField] private List<GameObject> agentsGameObjects = new List<GameObject>();
    [SerializeField] private SpriteRenderer background;
    private BossAgent agent1;
    private BossAgent agent2;
    private EntityStats agent1Stats;
    private EntityStats agent2Stats;
    private int maxHp1;
    private int maxHp2;
    // Start is called before the first frame update
    void Start()
    {
        agent1 = agentsGameObjects[0].GetComponent<BossAgent>();
        agent2 = agentsGameObjects[1].GetComponent<BossAgent>();
        agent1Stats = agentsGameObjects[0].GetComponent<EntityStats>();
        agent2Stats = agentsGameObjects[1].GetComponent<EntityStats>();
        maxHp1 = agent1Stats.maxHP;
        maxHp2 = agent2Stats.maxHP;
        agent1Stats.OnHPChange += Agent1Stats_OnHPChange;
        agent2Stats.OnHPChange += Agent2Stats_OnHPChange;
    }

    private void Agent2Stats_OnHPChange()
    {
        Debug.Log("Agent2 HP Change");
        if(agent2Stats.currentHP<=0) //If agent died
        {
            agent1.SetReward(1f); //Set positive rewards for the winning agent
            agent2.SetReward(-1f); //Negative reward for the losing agent

            //End Episodes and reset env
            agent2.EndEpisode();
            agent1.EndEpisode();
            ResetEnv("agent1");
        }
        else //The agent didn't die but received dmg, penalize
        {
           // agent2.AddReward(-0.1f);
            //Reward the other agent
           // agent1.AddReward(0.2f);
        }
    }

    private void Agent1Stats_OnHPChange()
    {
        Debug.Log("Agent1 HP Change");
        if (agent1Stats.currentHP <= 0)
        {
            agent2.SetReward(1f); //Set positive rewards for the winning agent
            agent1.SetReward(-1f);  //Negative reward for the losing agent

            //End Episodes and reset env
            agent2.EndEpisode();
            agent1.EndEpisode();
            ResetEnv("agent2");
        }
        else //The agent didn't die but received dmg, penalize
        {
            //agent2.AddReward(-0.1f);
            //Reward the other agent
           // agent1.AddReward(0.2f);
        }
    }

    public void ResetEnv(string whoWon)
    {
        Debug.Log("Won:" + whoWon+ " Reset episode");
        if (whoWon == "agent1")
            background.color = Color.green;
        else
            background.color = Color.magenta;
        agent2Stats.Heal(100000);
        agent1Stats.Heal(100000);
        var choice = Random.Range(0, 2);
        if (choice == 1)
        {
            agentsGameObjects[0].transform.localPosition = new Vector2(-(Random.value * 8), -8f);
            agentsGameObjects[1].transform.localPosition = new Vector2(Random.value * 8, -8f);

           
        }
        else
        {
            agentsGameObjects[1].transform.localPosition = new Vector2(-(Random.value * 8), -8f);
            agentsGameObjects[0].transform.localPosition = new Vector2(Random.value * 8, -8f);

           
        }
    }
}
