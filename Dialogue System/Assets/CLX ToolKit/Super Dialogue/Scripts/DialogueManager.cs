using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

/// <summary>
/// �԰׹���
/// �� ITalkUI Э��ʹ��
/// �Ի�UI�Ĺ����
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
    /// �԰׵���Ϣ�ļ����ڵ��ļ��е�·��
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
        //����Xml�ı�
        mDocuemnt.LoadXml(mText.text);
        //��ȡ���ڵ�
        XmlElement mElement = mDocuemnt.DocumentElement;
        //��ȡ�ڵ�ֵ
        XmlNodeList names = mElement.SelectNodes("/Dialogs/Dialog/name");
        XmlNodeList infos = mElement.SelectNodes("/Dialogs/Dialog/info");
        
        for(int i=0;i<names.Count;i++)
        {
            NPC_TYPE type;
            type = NPCEnumMgr.GetNPCString(names[i].InnerText);
            dialogues.Add(new DialogueInfo(type, infos[i].InnerText));
        }

        //��������
        return dialogues;
    }

}