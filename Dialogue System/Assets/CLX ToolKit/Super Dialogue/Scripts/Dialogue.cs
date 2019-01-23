using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CLX
{
    [System.Serializable]
    public class Dialogue
    {

        /// <summary>
        /// 对白事件的名字，根据名字加载对话
        /// 加载的格式是：DialoguePath + event_name + ".xml/txt"
        /// event_name的格式是：xml文件所在文件夹名+文件名
        /// 例如：test01/test
        /// </summary>
        [SerializeField]
        private string event_name = "";

        private List<DialogueInfo> infos = null;

        public List<DialogueInfo> GetDialogueInfos()
        {
            if (infos == null)
            {
                if (System.IO.File.Exists(Application.dataPath + "/Resources/" +
                    DialogueManager.DialoguePath + event_name + ".txt"))
                {
                    infos = DialogueManager.GetDialogueInfos(Application.dataPath + "/Resources/" +
                        DialogueManager.DialoguePath + event_name + ".txt");
                }
                else
                {
                    infos = DialogueManager.GetDialogueInfos(Application.dataPath + "/Resources/" +
                        DialogueManager.DialoguePath + event_name + ".xml");
                }
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
        public DialogueInfo(NPC_TYPE type, string info)
        {
            npcType = type;
            Info = info;
        }
    }
}