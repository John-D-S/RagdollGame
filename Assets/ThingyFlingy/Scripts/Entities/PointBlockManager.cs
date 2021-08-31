using TMPro;
using UnityEngine;

public class PointBlockManager : MonoBehaviour
{
    [SerializeField, Tooltip("The text that displays the number of blocks remaining.")] private TextMeshProUGUI blocksRemaningDisplay;
    // the number of blocks remaining.
    private int blocksRemaining = 0;
    // gets and sets blocksRemaining and changes the blocksRemainingDisplayText to display the number of blocks remaining,
    // or a message when the number of blocks remaining is 0.
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
    // the pointblockManager singleton.
    public static PointBlockManager thePointBlockManager;
    
    private void Start()
    {
        // set the pointblock manager 
        thePointBlockManager = this;
    }
}
