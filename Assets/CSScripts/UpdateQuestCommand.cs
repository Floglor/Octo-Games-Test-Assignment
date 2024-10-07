using Naninovel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace CSScripts
{
    [CommandAlias("updatequest")]
    public class UpdateQuestCommand : Command
    {
        public IntegerParameter QuestStage;
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            if (Assigned(QuestStage))
            {
                QuestLog questLog = Resources.FindObjectsOfTypeAll(typeof(QuestLog))[0].GetComponent<QuestLog>(); 
                GameObject.Find("QuestLogButton").GetComponent<Toggle>().SetIsOnWithoutNotify(true);
                questLog.gameObject.SetActive(true);
                questLog.ProgressQuest(QuestStage);
            }
            return default;
        }
    }
}