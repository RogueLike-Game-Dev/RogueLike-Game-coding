using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class TagEnvController : MonoBehaviour
{
    [SerializeField] private GameObject catcherAgentObj;
    [SerializeField] private GameObject runnerAgentObj;
    [SerializeField] private SpriteRenderer background;
    private Agent catcher;
    private Agent runner;
    private EntityStats agent1Stats;
    private EntityStats agent2Stats;
    private int maxHp1;
    private int maxHp2;
    public int runnerThresholdReward = 1;
    // Start is called before the first frame update
    void Start()
    {
        catcher = catcherAgentObj.GetComponent<BossAgent>();
        runner = runnerAgentObj.GetComponent<BossAgent>();
        agent1Stats = catcherAgentObj.GetComponent<EntityStats>();
        agent2Stats = runnerAgentObj.GetComponent<EntityStats>();
        maxHp1 = agent1Stats.maxHP;
        maxHp2 = agent2Stats.maxHP;
    }

    public void ResetEnv(string whoWon)
    {
        Debug.Log("Won:" + whoWon + " Reset episode");
        catcher.AddReward(1);
        runner.AddReward(-1);
        if (runner.GetCumulativeReward() > runnerThresholdReward) // If the runner evaded the catcher long enough, he won
            background.color = Color.green;
        else
            background.color = Color.magenta;
        var choice = Random.Range(0, 2);
        catcher.EndEpisode();
        runner.EndEpisode();
        if (choice == 1)
        {
            runnerAgentObj.transform.localPosition = new Vector2(-(Random.value * 8), -8f);
            catcherAgentObj.transform.localPosition = new Vector2(Random.value * 8, -8f);


        }
        else
        {
            runnerAgentObj.transform.localPosition = new Vector2(-(Random.value * 8), -8f);
            catcherAgentObj.transform.localPosition = new Vector2(Random.value * 8, -8f);


        }
    }
}
