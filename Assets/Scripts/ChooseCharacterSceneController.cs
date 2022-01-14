using UnityEngine;
using UnityEngine.UI;

public class ChooseCharacterSceneController : MonoBehaviour
{
    
    private string[] characterNames;
    private string[] characterDescriptions;

    private Text nameText;
    private Text descriptionText;
    private int index;

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
            "Zhax description",
            "Demetria description",
            "Esteros description",
            "Lyn description",
            "Random description"
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
}
