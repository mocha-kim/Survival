using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest System/Quest")]
public class QuestObject : ScriptableObject
{
    public Quest data = new();

    public QuestType type;
    public QuestCampType camp;
    public QuestStatus status;

    public string title;
    public string summary;
    [TextArea(15, 20)]
    public string description;
}
