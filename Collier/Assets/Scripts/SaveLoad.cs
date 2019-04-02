using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public static bool loaded = false;
    public static int LEVELS = 5;
    public static int STAGES = 2;
    public static Dictionary<string, int> levelUnlocked = new Dictionary<string, int>();
    // Start is called before the first frame update
    void Awake()
    {
        if (!loaded)
        {
            for (int i = 1; i <= SaveLoad.LEVELS; i++)
            {
                for (int j = 1; j <= SaveLoad.STAGES; j++)
                {
                    string key = $"Level_{i}_{j}";
                    if (!PlayerPrefs.HasKey(key))
                    {
                        PlayerPrefs.SetInt(key, -1);
                    }
                    // if the first level is locked by save or a save doesn't exist,
                    // unlock the first level
                    if (key == "Level_1_1" &&
                        (!PlayerPrefs.HasKey(key) || PlayerPrefs.GetInt(key) < 0))
                    {
                        PlayerPrefs.SetInt(key, 0);
                        levelUnlocked[key] = 0;
                    }
                    levelUnlocked[key] = PlayerPrefs.GetInt(key);
                }
            }
            loaded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            for (int i = 1; i <= SaveLoad.LEVELS; i++)
            {
                for (int j = 1; j <= SaveLoad.STAGES; j++)
                {
                    string key = $"Level_{i}_{j}";
                    levelUnlocked[key] = -1;
                    PlayerPrefs.DeleteKey(key);
                }
            }
            loaded = false;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            for (int i = 1; i <= SaveLoad.LEVELS; i++)
            {
                for (int j = 1; j <= SaveLoad.STAGES; j++)
                {
                    string key = $"Level_{i}_{j}";
                    levelUnlocked[key] = ((levelUnlocked[key] + 2) % 5) - 1;
                }
            }
        }
    }
}
