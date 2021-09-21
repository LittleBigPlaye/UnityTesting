using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DiscordManager : MonoBehaviour
{
#if UNITY_EDITOR || UNITY_STANDALONE
    DiscordController discordController;


    // Start is called before the first frame update
    void Start()
    {
        discordController = DiscordController.Instance;
        discordController.SetActivity("Debugging", "Trying to fix stuff");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        discordController.Update();
    }

    private void OnApplicationQuit()
    {
        discordController.Stop();
    }
#endif
}
