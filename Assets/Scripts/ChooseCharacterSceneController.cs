using UnityEngine;
using UnityEngine.UI;

public class ChooseCharacterSceneController : MonoBehaviour
{
    
    private string[] characterNames;
    private string[] characterDescriptions;

    private Text nameText;
    private Text descriptionText;
    private int index;

    private int chosenCharacterIndex = -1;

    private void Start()
    {
        nameText = GameObject.Find("Name").GetComponent<Text>();
        descriptionText = GameObject.Find("Description").GetComponent<Text>();
        
        characterNames = new [] 
        {
            "Zhax",
            "Demetria",
            "Esteros",
            "Lyn",
            "Random"
        };

        characterDescriptions = new[]
        {
            "Don't be fooled by the small size Zhax the Satyr, because he can, and will, use everything in his power to destroy his enemies. They should watch out for object flying towards them, they may have angered him.",
            "Although Demetria might still be a rookie in the arts of the arcane, her powerful fireball assures she won't go down without a fight.",
            "Esteros the Mage is a powerful being which can manipulate object around him. He can destroy objects on a whim for his safety.",
            "Lyn wields the power of lighting to bring down on his enemies. Although it may seem underwhelming, I wouldn't like to be on the receiving end of the lighting strike.",
            "If you cannot decide between this amazing choice of heroes, or simply want to test your luck, this option is just for you!"
        };
    }

    void Update()
    {
        index = ChooseCharacterController.index;

        if (index != -1)
        {
            nameText.text = characterNames[index];
            descriptionText.text = characterDescriptions[index];
        }
        // else
        // {
        //     nameText.text = "Choose";
        //     
        // }
    }

    public void UpdateChosenCharacter()
    {
        if (index == 4)
        {
            chosenCharacterIndex = Random.Range(0, 4);
        }
        else
        {
            chosenCharacterIndex = index;
        }

        SetChosenCharacter(chosenCharacterIndex);
    }

    private void SetChosenCharacter(int idx)
    {
        switch (idx)
        {
            case 0:
                PlayerMovement.characterType = PlayerMovement.CharacterType.Zhax;
                break;
            case 1:
                PlayerMovement.characterType = PlayerMovement.CharacterType.Demetria;
                break;
            case 2:
                PlayerMovement.characterType = PlayerMovement.CharacterType.Esteros;
                break;
            case 3:
                PlayerMovement.characterType = PlayerMovement.CharacterType.Lyn;
                break;
        }
    }
}
