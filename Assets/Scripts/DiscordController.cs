using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using System;
#if UNITY_EDITOR || UNITY_STANDALONE
public class DiscordController
{

    private static DiscordController instance = null;
    public static DiscordController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DiscordController();
            }
            return instance;
        }
    }

    private Discord.Discord discord;

    private DiscordController()
    {
        discord = new Discord.Discord(887605450504302613, (UInt64)Discord.CreateFlags.Default);
    }

    public void SetActivity(String state, String details)
    {
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            State = state,
            Details = details,
        };
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                Debug.Log("Discord Status set");
            }
            else
            {
                Debug.LogError("Unable to set Discord Status");
            }
        });
    }

    public void Update()
    {
        discord?.RunCallbacks();
    }

    public void Stop() {
        discord.Dispose();
    }

}
#endif
