using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class PointBlockManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI blocksRemaningDisplay;
    private int blocksRemaining = 0;
    public int BlocksRemaining
    {
        get => blocksRemaining;
        set
        {
            blocksRemaining = value;
            if(blocksRemaningDisplay)
            {
                blocksRemaningDisplay.text = blocksRemaining.ToString();
                if(blocksRemaining <= 0)
                {
                    blocksRemaningDisplay.text = "You win. The mudmen are pleased. Turn the game off.";
                }   
            }
        }
    }
    public static PointBlockManager thePointBlockManager;
    
    private void Start()
    {
        thePointBlockManager = this;
    }
}
