using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindManager : MonoBehaviour
{

    private Dictionary<string, KeyCode> keybindDict;
    private GameObject currKey;

    public Text up, down, attack, special_attack, dash, interact, item1, item2;

    void Start()
    {
        keybindDict = new Dictionary<string, KeyCode>();

        keybindDict.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
        keybindDict.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        keybindDict.Add("Attack", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Attack", "Mouse0")));
        keybindDict.Add("Special Attack", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Special Attack", "Mouse1")));
        keybindDict.Add("Dash", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Dash", "E")));
        keybindDict.Add("Interact", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact", "G")));
        keybindDict.Add("Item1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Item1", "Alpha1")));
        keybindDict.Add("Item2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Item2", "Alpha2")));

        up.text = keybindDict["Up"].ToString();
        down.text = keybindDict["Down"].ToString();
        attack.text = keybindDict["Attack"].ToString();
        special_attack.text = keybindDict["Special Attack"].ToString();
        dash.text = keybindDict["Dash"].ToString();
        interact.text = keybindDict["Interact"].ToString();
        item1.text = keybindDict["Item1"].ToString();
        item2.text = keybindDict["Item2"].ToString();
    }

    void OnGUI()
    {
        if (currKey != null)
        {
            Event currEvent = Event.current;
            if (currEvent.isKey)
            {
                keybindDict[currKey.name] = currEvent.keyCode;
                currKey.transform.GetChild(0).GetComponent<Text>().text = currEvent.keyCode.ToString();
                currKey = null;
                SaveKeybinds();
            }
            else if (currEvent.isMouse)
            {
                if (currEvent.button == 0)
                    keybindDict[currKey.name] = KeyCode.Mouse0;
                else if (currEvent.button == 1)
                    keybindDict[currKey.name] = KeyCode.Mouse1;
                
                currKey.transform.GetChild(0).GetComponent<Text>().text = keybindDict[currKey.name].ToString();
                currKey = null;
                SaveKeybinds();
            }
        }
    }

    public void ChangeKey(GameObject clicked)
    {
        currKey = clicked;
    }

    public void SaveKeybinds()
    {
        foreach(var keybind in keybindDict)
        {
            PlayerPrefs.SetString(keybind.Key, keybind.Value.ToString());
            //Debug.Log(PlayerPrefs.GetString(keybind.Key));
        }

        PlayerPrefs.Save();
        
    }
}
