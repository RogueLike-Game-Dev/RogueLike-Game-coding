using UnityEngine;
using UnityEngine.UI;

public class LevelDiana : MonoBehaviour
{
    private const int itemsToCollect = 8;
    private const int enemiesToFight = 0;
    private EntityStats playerStats;
    private GameObject player;
    private Transform oneColumnDialogueText;
    private Transform twoColumnDialogueText;
    private GameObject dialogueBox;
    private string[] NPCdialogueTexts;
    private int currentDialogueIndex;
    private int smallFontSize;
    private int bigFontSize;
    private int stringOffset;

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
        
        finishedLevel = false;
        visitedNPC = false;
    }

    void Update()
    {
        dialogueBox = GameObject.Find("DialogueBox");

        if (dialogueBox)
        {
            oneColumnDialogueText = dialogueBox.gameObject.transform.GetChild(0);
            twoColumnDialogueText = dialogueBox.gameObject.transform.GetChild(1);
        }
        
        if (currentDialogueIndex == 2)
        {
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
        else
        {
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

                if (finishedLevel)
                {
                    visitedNPC = true;
                }
            }
        }

        if (playerStats.collectibles == itemsToCollect &&
            playerStats.enemiesKilled == enemiesToFight)
        {
            currentDialogueIndex = NPCdialogueTexts.Length - 1;
            finishedLevel = true;
        }

        if (currentDialogueIndex == NPCdialogueTexts.Length - 2)
        {
            var continueButton = GameObject.Find("ContinueButton");
            if (continueButton)
            {
                continueButton.SetActive(false);
            }
        }

        if (finishedLevel && visitedNPC)
        {
            Debug.Log("FINISHED LEVEL!");
        }
    }

    public void ContinueButtonHandler()
    {
        currentDialogueIndex++;
    }
}
