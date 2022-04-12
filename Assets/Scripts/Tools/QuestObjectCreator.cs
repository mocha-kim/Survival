using System.IO;
using UnityEditor;
using UnityEngine;

public class QuestObjectCreator : EditorWindow
{
    private QuestType questType = 0;
    private CampType campType = 0;

    private string questTitle = "";
    private string description = "";
    private string summary = "";

    private int targetID = 0;
    private int targetCount = 0;
    private int targetNPCID = 0;

    private int rewardGold = 0;
    [SerializeField]
    private int[] rewardItemIDs;
    [SerializeField]
    private int[] rewardItemCounts;

    SerializedObject obj;
    SerializedProperty rewardItemIDSP;
    SerializedProperty rewardItemCountSP;

    private const string path = "Assets/GameSystems/QuestSystem/";
    private const string databasePath = "Assets/Databases/QuestDatabase.asset";

    [MenuItem("Tools/Quest Object Creator")]
    public static void Init()
    {
        QuestObjectCreator window = GetWindow<QuestObjectCreator>(true, "Quest Object Creator");
        window.Show();
    }

    private void OnEnable()
    {
        ScriptableObject target = this;
        obj = new SerializedObject(target);
        rewardItemIDSP = obj.FindProperty("rewardItemIDs");
        rewardItemCountSP = obj.FindProperty("rewardItemCounts");
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("General", EditorStyles.largeLabel);
        questTitle = EditorGUILayout.TextField("Title", questTitle);
        EditorGUILayout.LabelField("Description");
        description = EditorGUILayout.TextArea(description);
        summary = EditorGUILayout.TextField("Summary", summary);
        EditorGUILayout.Space();

        questType = (QuestType)EditorGUILayout.EnumPopup("Quest Type", questType);
        campType = (CampType)EditorGUILayout.EnumPopup("Camp Type", campType);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Target", EditorStyles.largeLabel);
        targetID = EditorGUILayout.IntField("Target ID", targetID);
        targetCount = EditorGUILayout.IntField("Target Count", targetCount);
        if (questType == QuestType.TransferItem)
        {
            targetNPCID = EditorGUILayout.IntField("Destination NPC ID", targetNPCID);
        }
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Reward", EditorStyles.largeLabel);
        rewardGold = EditorGUILayout.IntField("Reward Money", rewardGold);
        EditorGUILayout.LabelField("Reward Item IDs");
        EditorGUILayout.PropertyField(rewardItemIDSP);
        EditorGUILayout.LabelField("Reward Item Counts");
        EditorGUILayout.PropertyField(rewardItemCountSP);
        obj.ApplyModifiedProperties();

        if (GUILayout.Button("Create"))
        {
            if (CreateQuestObject())
            {
                Debug.Log("Quest Object Created!");
            }
        }
    }

    bool CreateQuestObject()
    {
        if (questTitle == "" || description == "" || summary == "")
        {
            Debug.Log("General informations did not set");
            return false;
        }
        QuestObject questObject = CreateInstance<QuestObject>();

        questObject.title = questTitle;
        questObject.description = description;
        questObject.summary = summary;

        questObject.type = questType;
        questObject.camp = campType;
        questObject.status = QuestStatus.None;

        questObject.data.targetID = targetID;
        questObject.data.currentCount = 0;
        questObject.data.goalCount = targetCount;
        if (questType == QuestType.TransferItem)
        {
            questObject.data.destNPCID = targetNPCID;
        }

        questObject.data.rewardGold = rewardGold;
        try
        {
            for (int i = 0; i < rewardItemIDs.Length; i++)
            {
                questObject.data.rewardItemIds.Add(rewardItemIDs[i]);
                questObject.data.rewardItemCounts.Add(rewardItemCounts[i]);
            }
        }
        catch
        {
            Debug.Log("Reward items id-count must be paired");
            return false;
        }

        if (Path.GetFileName(path + questTitle) == "")
        {
            Debug.Log(questTitle + " is already exist");
            return false;
        }
        AssetDatabase.CreateAsset(questObject, path + questTitle + ".asset");
        AssetDatabase.Refresh();

        var database = AssetDatabase.LoadAssetAtPath<QuestDatabase>(databasePath);
        if (database == null)
        {
            Debug.Log("Quest database not found");
            return false;
        }
        database.data.Add(questObject);
        database.OnValidate();
        return true;
    }
}