using System.Collections.Generic;
using UnityEngine;
using System;

namespace CLX
{
    [System.Serializable]
    public class Dialogue
    {
        /// <summary>
        /// Dialogue 事件类型
        /// </summary>
        public enum DIALOGUE_EVENT
        {
            ON_BEGIN,
            ON_STEP,
            ON_END
        }

        /// <summary>
        /// 对白事件的名字，根据名字加载对话
        /// 加载的格式是：DialoguePath + event_name + ".xml/txt"
        /// event_name的格式是：xml文件所在文件夹名+文件名
        /// 例如：test01/test
        /// </summary>
        [SerializeField]
        private string event_name = "";

        Dictionary<DIALOGUE_EVENT, Action> events = new Dictionary<DIALOGUE_EVENT, Action>();

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

        Dialogue()
        {
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="action">对应事件</param>
        public void RegisterAction(DIALOGUE_EVENT type, Action action)
        {
            events.Add(type, action);
        }

        /// <summary>
        /// 事件执行
        /// </summary>
        /// <param name="type">事件种类</param>
        public void OnAction(DIALOGUE_EVENT type)
        {
            Action action;
            if(events==null)
            {
                Debug.Log("Action NULL");
            }
            if(events.TryGetValue(type,out action))
            {
                if(action!=null)
                {
                    action();
                }
                else
                {
                    Debug.Log("The Action is NULL!");
                }
            }
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