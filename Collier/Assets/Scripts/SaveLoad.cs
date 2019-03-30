using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    static bool loaded = false;
    static Dictionary<string, int> levelUnlocked = new Dictionary<string, int>();
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!loaded)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    string key = $"Level_{i}_{j}";
                    if (!PlayerPrefs.HasKey(key))
                    {
                        PlayerPrefs.SetInt(key, -1);
                    }
                    levelUnlocked[key] = PlayerPrefs.GetInt(key);
                }
            }
            loaded = true;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    string key = $"Level_{i}_{j}";
                    PlayerPrefs.DeleteKey(key);
                }
            }
            loaded = false;
        }
    }
}
