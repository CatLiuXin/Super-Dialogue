using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace CLX
{
    /// <summary>
    /// �԰׹���
    /// �� ITalkUI Э��ʹ��
    /// �Ի�UI�Ĺ����
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {

        private static DialogueManager _instance;
        public static DialogueManager Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// �԰׵���Ϣ�ļ����ڵ��ļ��е�·��
        /// </summary>
        public static string DialoguePath
        {
            get
            {
                return "Text/Dialogue/";
            }
        }

        private IDialogueUI dialogueUI;

        // Use this for initialization
        void Start()
        {
            dialogueUI = GetComponent<IDialogueUI>();
            _instance = this;
        }

        public void StartDialogue(Dialogue dialogue)
        {
            dialogueUI.SetDialogue(dialogue);
            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            dialogueUI.ShowNextSentence();
        }

        public static List<DialogueInfo> GetDialogueInfos(string name)
        {
            return ReadSingleXml(name);
        }

        /// <summary>
        /// ��ȡXML�ļ�ת����List<DialogueInfo>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static List<DialogueInfo> ReadSingleXml(string path)
        {
            List<DialogueInfo> dialogues = new List<DialogueInfo>();
            XDocument mDocuemnt;
            XElement mElement;

            List<string> infos;
            List<string> names;

            //����Xml�ı�
            try
            {
                mDocuemnt = XDocument.Load(path);
                //��ȡ���ڵ�
                mElement = mDocuemnt.Root;
                //��ȡ�ڵ�ֵ
                names = (from seg in mElement.Descendants("name") select (string)seg).ToList();
                infos = (from seg in mElement.Descendants("info") select (string)seg).ToList();
            }
            catch
            {
                mDocuemnt = CreateXMLFile(path);
                mElement = mDocuemnt.Root;
                names = (from seg in mElement.Descendants("name") select (string)seg).ToList();
                infos = (from seg in mElement.Descendants("info") select (string)seg).ToList();
            }

            for (int i = 0; i < names.Count; i++)
            {
                NPC_TYPE type;
                type = NPCEnumMgr.GetNPCString(names[i]);
                dialogues.Add(new DialogueInfo(type, infos[i]));
            }

            //��������
            return dialogues;
        }

        /// <summary>
        /// ���洴��XML�ļ�
        /// </summary>
        /// <param name="path"></param>
        /// <param name="infos"></param>
        /// <returns></returns>
        public static XDocument CreateXMLFile(string path, List<DialogueInfo> infos = null)
        {
            XDocument doc;
            if (infos == null)
            {
                doc = new XDocument(new XElement("Dialogues",
                                                   new XElement("Dialogue",
                                                       new XElement("name", "Box"),
                                                       new XElement("info", ""))));
                doc.Save(path);
                print("Create in:" + path);
                return doc;
            }
            doc = new XDocument(new XElement("Dialogues"));
            XElement element = doc.Root;
            foreach (var info in infos)
            {
                element.Add(InfoToElement(info));
            }
            print(path);
            doc.Save(path);
            return doc;
        }

        /// <summary>
        /// ��DialogueInfo����XML���
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static XElement InfoToElement(DialogueInfo info)
        {
            return new XElement("Dialogue",
                new XElement("name", info.npcType.ToString()),
                new XElement("info", info.Info));
        }

    }
}