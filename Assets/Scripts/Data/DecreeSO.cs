using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reigns/Decree")]
public class DecreeSO : ScriptableObject
{
    public string title;

    [TextArea(3, 5)]
    public string description;

    public DecreeChoice leftChoice;
    public DecreeChoice rightChoice;
}
