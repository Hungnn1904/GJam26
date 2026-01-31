using UnityEngine;

[System.Serializable]
public class Decree
{
    public string title;
    public string description;

    // Left choice
    public string leftText;
    public int leftPopularity;
    public int leftMilitary;
    public int leftFinance;
    public int leftReligion;

    // Right choice
    public string rightText;
    public int rightPopularity;
    public int rightMilitary;
    public int rightFinance;
    public int rightReligion;

    public bool IsLeftSafe()
    {
        return leftPopularity >= -10 && leftReligion >= -10;
    }

    public bool IsRightSafe()
    {
        return rightPopularity >= -10 && rightReligion >= -10;
    }
}
