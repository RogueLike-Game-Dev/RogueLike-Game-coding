using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingEnvController : MonoBehaviour
{
    [SerializeField] private List<GameObject> agentsGameObjects = new List<GameObject>();
    [SerializeField] private SpriteRenderer background;
    private GameObject[] apples;
    private BossAgent agent1;
    private BossAgent agent2;
    private EntityStats agent1Stats;
    private EntityStats agent2Stats;
    private int maxHp1;
    private int maxHp2;
    private List<(float, float)> platformsPositions = new List<(float, float)>
    {
        ((float)-8.47, (float)5.72),
        ((float)8.47, (float)5.72),
        ((float)-8.47, (float)-0.31),
        ((float)8.47, (float)-0.31),
        ((float)-13.5, (float)-4.32),
        ((float)13.5, (float)-4.32)
    };
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
        apples = GameObject.FindGameObjectsWithTag("Apple");
    }

    private void Agent2Stats_OnHPChange()
    {
        Debug.Log("Agent2 HP Change");
        Debug.Log("Current agent 2 HP is" + agent1Stats.currentHP);
        if (agent2Stats.currentHP<=0) // If agent died
        {
            agent1.AddReward(1f); // Set positive rewards for the winning agent
            agent2.AddReward(-1f); // Negative reward for the losing agent

            // End Episodes and reset env
            agent2.EndEpisode();
            agent1.EndEpisode();
            ResetEnv("agent1");
        }
        // The agent didn't die and it's HP increased (he picked an apple)
        else if(agent2Stats.pickedApple)
        {
            agent2Stats.pickedApple = false;
            Debug.Log("Current agent 2 picked an apple!");
            // we have to check if he really needed it
            if (agent2Stats.previousHP <= 65)
            {
                agent2.AddReward(0.25f);
            }
        }
        // The agent didn't die but received dmg, penalize
        else if(agent2Stats.currentHP < agent2Stats.previousHP)
        {
            Debug.Log("Current agent 2 was demaged!");
            agent2.AddReward(-0.1f);
            agent1.AddReward(0.2f);
        }
    }

    private void Agent1Stats_OnHPChange()
    {
        Debug.Log("Agent1 HP Change");
        Debug.Log("Current agent 1 HP is" + agent1Stats.currentHP);
        Debug.Log("Current currentHP is" + agent1Stats.previousHP);
        Debug.Log("Current previousHP is" + agent1Stats.previousHP);
        if (agent1Stats.currentHP <= 0)
        {
            agent2.AddReward(1f); // Set positive rewards for the winning agent
            agent1.AddReward(-1f);  // Negative reward for the losing agent

            // End Episodes and reset env
            agent2.EndEpisode();
            agent1.EndEpisode();
            ResetEnv("agent2");
        }
        else if(agent1Stats.pickedApple)
        {
            agent1Stats.pickedApple = false;
            Debug.Log("Current agent 1 picked an apple!");
            if (agent1Stats.previousHP <= 65)
            {
                agent1.AddReward(0.25f);
            }
        }
        else if (agent1Stats.currentHP < agent1Stats.previousHP)// The agent didn't die but received dmg, penalize
        {
            Debug.Log("Current agent 1 was damaged!");
            agent1.AddReward(-0.1f);
            // Reward the other agent
            agent2.AddReward(0.2f);
        }
        /// there's one case left when agent1Stats.currentHP >= agent1Stats.previousHP - when we reset the env
        /// we should do nothing in this case
    }

    public void ResetEnv(string whoWon)
    {
        Debug.Log("Won: " + whoWon+ " Reset episode");
        if (whoWon == "agent1")
            background.color = Color.green;
        else
            background.color = Color.magenta;
        agent2Stats.Heal(100000);
        agent1Stats.Heal(100000);

        // We reactivate the apples and put them randomly in the possible chosen positions - so the agents don't memorize
        // their position, which could lead to overfitting
       /* var firstChoice = Random.Range(0, platformsPositions.Count - 1);
        var secondChoice = Random.Range(0, platformsPositions.Count - 1);
        while(secondChoice ==  firstChoice)
        {
            secondChoice = Random.Range(0, platformsPositions.Count - 1);
        }
        apples[0].transform.localPosition = new Vector3(platformsPositions[firstChoice].Item1, platformsPositions[firstChoice].Item2, 10);
        apples[1].transform.localPosition = new Vector3(platformsPositions[secondChoice].Item1, platformsPositions[secondChoice].Item2, 10);
       */
        apples[0].SetActive(true);
        apples[1].SetActive(true);

        var choice = Random.Range(0, 2);
        if (choice == 1)
        {
            agentsGameObjects[0].transform.localPosition = new Vector3(-(Random.value * 8), -8f, 10);
            agentsGameObjects[1].transform.localPosition = new Vector3(Random.value * 8, -8f, 10);
        }
        else
        {
            agentsGameObjects[1].transform.localPosition = new Vector3(-(Random.value * 8), -8f, 10);
            agentsGameObjects[0].transform.localPosition = new Vector3(Random.value * 8, -8f, 10);
        }
       
    }
}
