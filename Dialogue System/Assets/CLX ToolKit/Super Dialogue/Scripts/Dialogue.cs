using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

    /// <summary>
    /// 对白事件的名字，根据名字加载对话
    /// 加载的格式是：DialoguePath + event_name + ".xml"
    /// event_name的格式是：xml文件所在文件夹名+文件名
    /// 例如：test01/test
    /// </summary>
    [SerializeField]
    private string event_name = "";

    private List<DialogueInfo> infos = null;

    public List<DialogueInfo> GetDialogueInfos()
    {
        if(infos == null)
        {
            infos = DialogueManager.GetDialogueInfos(event_name);
        }
        return infos;
    }

    Dialogue(string n_event)
    {
        event_name = n_event;
    }

}

public class DialogueInfo
{
    public NPC_TYPE npcType;
    public string Info;
    public DialogueInfo(NPC_TYPE type,string info)
    {
        npcType = type;
        Info = info;
    }
}