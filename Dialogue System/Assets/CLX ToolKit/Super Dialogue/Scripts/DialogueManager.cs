using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

/// <summary>
/// 对白管理
/// 和 ITalkUI 协调使用
/// 对话UI的管理层
/// </summary>
public class DialogueManager : MonoBehaviour {

    private static DialogueManager _instance;
    public static DialogueManager Instance
    {
        get {
            return _instance;
        }
    }

    /// <summary>
    /// 对白的信息文件所在的文件夹的路径
    /// </summary>
    private static string DialoguePath = "Text/Dialogue/";

    private IDialogueUI dialogueUI;

	// Use this for initialization
	void Start () {
        dialogueUI = GetComponent<IDialogueUI>();
        _instance = this;
	}

	public void StartDialogue (Dialogue dialogue)
	{
        dialogueUI.SetDialogue(dialogue);
		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
        dialogueUI.ShowNextSentence();
	}
    
    public static List<DialogueInfo> GetDialogueInfos(string name)
    {
        TextAsset text = Resources.Load<TextAsset>(DialoguePath + name);
        return ReadSingleXml(text);
    }

    private static List<DialogueInfo> ReadSingleXml(TextAsset mText)
    {
        List<DialogueInfo> dialogues = new List<DialogueInfo>();
        XmlDocument mDocuemnt = new XmlDocument();
        //加载Xml文本
        mDocuemnt.LoadXml(mText.text);
        //获取根节点
        XmlElement mElement = mDocuemnt.DocumentElement;
        //读取节点值
        XmlNodeList names = mElement.SelectNodes("/Dialogs/Dialog/name");
        XmlNodeList infos = mElement.SelectNodes("/Dialogs/Dialog/info");
        
        for(int i=0;i<names.Count;i++)
        {
            NPC_TYPE type;
            type = NPCEnumMgr.GetNPCString(names[i].InnerText);
            dialogues.Add(new DialogueInfo(type, infos[i].InnerText));
        }

        //返回数组
        return dialogues;
    }

}