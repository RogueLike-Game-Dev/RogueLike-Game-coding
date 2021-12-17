using UnityEngine;
using UnityEngine.UI;

public class LevelDiana : MonoBehaviour
{
    private const int itemsToCollect = 8;
    private const int enemiesToFight = 8;
    private EntityStats playerStats;
    private GameObject player;
    private GameObject camera;
    private Transform oneColumnDialogueText;
    private Transform twoColumnDialogueText;
    private GameObject dialogueBox;
    private string[] NPCdialogueTexts;
    private int currentDialogueIndex;
    private int smallFontSize;
    private int bigFontSize;
    private int stringOffset;
    public GameObject continueButton;
    public GameObject nextLevelDoor;

    private bool visitedNPC;
    private bool finishedLevel;

    void Start()
    {
        NPCdialogueTexts = new string[4];
        NPCdialogueTexts[0] = "AAH! Those monsters! Hey, you! Can you help me, please? Some thieves stole all my food!";
        NPCdialogueTexts[1] =
            "I just wanted to have a nice picnic, and now I have nothing to eat... Can you retrieve these for me and take care of the theives?";
        NPCdialogueTexts[2] = "- crunchy bread\n" +
                              "- fireworks drinks\n" +
                              "- fire potions\n" +
                              "- poison" +
                              "- smelly fish\n" +
                              "- rotten grapes\n" +
                              "- chicken leg\n" +
                              "- raw mutton";
        NPCdialogueTexts[3] = "Horray! Thank you very much!";
        stringOffset = 58;
        currentDialogueIndex = 0;
        smallFontSize = 12;
        bigFontSize = 13;
        
        player = GameObject.Find("Player");
        playerStats = player.GetComponent<EntityStats>();
        player.transform.position = new Vector3(-3.5f, -3.2f, 0.0f);
        player.GetComponent<Rigidbody2D>().gravityScale = 1.5f;
        player.GetComponent<Rigidbody2D>().mass = 2.0f;

        camera = player.transform.GetChild(0).gameObject;
        camera.transform.localPosition = new Vector3(3.05f, 1.0f, -10.0f);
        camera.GetComponent<Camera>().orthographicSize = 5;

        var minimap = camera.transform.GetChild(0).gameObject;
        minimap.transform.localPosition = new Vector3(5.0f, 3.0f, 0.0f);
        minimap.GetComponent<Camera>().orthographicSize = 9;
        
        finishedLevel = false;
        visitedNPC = false;
    }

    void Update()
    {
        print("ITEMS COLLECTED:" + playerStats.collectibles);
        print("ENEMIES KILLED:" + playerStats.enemiesKilled);
        dialogueBox = GameObject.Find("DialogueBox");

        // if the level is finished, the player must return the goods to the NPC
        if (finishedLevel && currentDialogueIndex == 2)
        {
            continueButton.SetActive(true);
            continueButton.transform.GetChild(0).GetComponent<Text>().text = "Return goods >";
        }
        
        if (dialogueBox)
        {
            oneColumnDialogueText = dialogueBox.gameObject.transform.GetChild(0);
            twoColumnDialogueText = dialogueBox.gameObject.transform.GetChild(1);
        }
        
        // 2 -> the list of goods that must be returned
        if (currentDialogueIndex == 2)
        {
            // 2 -> the list must be shown on 2 columns
            if (oneColumnDialogueText)
            {
                oneColumnDialogueText.gameObject.SetActive(false);
            }
            
            if (twoColumnDialogueText)
            {
                var firstColumn = twoColumnDialogueText.transform.GetChild(0);
                var secondColumn = twoColumnDialogueText.transform.GetChild(1);
                
                var firstColumnText = NPCdialogueTexts[2].Substring(0, stringOffset);
                var secondColumnText = NPCdialogueTexts[2].Substring(stringOffset);
                
                var firstColTextComponent = firstColumn.GetComponent<Text>();
                var secondColTextComponent = secondColumn.GetComponent<Text>();
                
                firstColTextComponent.fontSize = smallFontSize;
                firstColTextComponent.text = firstColumnText;
                secondColTextComponent.fontSize = smallFontSize;
                secondColTextComponent.text = secondColumnText;
                
                twoColumnDialogueText.gameObject.SetActive(true);
            }
        }
        // 0, 1 or 3 -> dialogue
        else
        {
            // the dialogue must be shown on a single column
            if (twoColumnDialogueText)
            {
                twoColumnDialogueText.gameObject.SetActive(false);
            }

            if (oneColumnDialogueText)
            {
                var textComponent = oneColumnDialogueText.GetComponent<Text>();
                textComponent.fontSize = bigFontSize;
                oneColumnDialogueText.GetComponent<Text>().text = NPCdialogueTexts[currentDialogueIndex];

                oneColumnDialogueText.gameObject.SetActive(true);
            }
            
            // 3 -> player returned goods to the NPC
            if (currentDialogueIndex == 3)
            {
                visitedNPC = true;
            }
        }

        // finished level, now the player must return to the NPC
        if (playerStats.collectibles >= itemsToCollect &&
            playerStats.enemiesKilled >= enemiesToFight)
        {
            finishedLevel = true;
        }

        // if the level isn't finished, then the player cannot press the Continue button
        if (currentDialogueIndex >= 2)
        {
            if (continueButton && !finishedLevel)
            {
                continueButton.SetActive(false);
            }
        }

        if (finishedLevel && visitedNPC)
        {
            continueButton.SetActive(false);
            nextLevelDoor.SetActive(true);
        }
    }

    public void ContinueButtonHandler()
    {
        currentDialogueIndex++;
    }
}
