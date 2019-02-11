using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CLX
{
    /// <summary>
    /// 特殊文本转换成对白
    /// 以及根据特殊文本产生对应执行事件
    /// </summary>
    /// <param name="text"></param>
    /// <param name="sText"></param>
    /// <returns></returns>
    public delegate System.Action SpecialEvent(string text,out string sText);

    /// <summary>
    /// 对白UI表现
    /// 此脚本所挂载的对象必须有以下子物体：
    /// 1. 名为NameText的Text物体
    /// 2. 名为DialogueText的Text物体
    /// 3. 一个Image物体
    /// </summary>
    public class EventDialogueUI : BaseDilogueUI
    {
        private Text nameText;
        private Text dialogueText;
        private Image EventPic;
        private Animator animator;

        private Queue<string> sentences;
        private Queue<NPC_TYPE> types;
        private Queue<System.Action> actions;

        private bool mIsSpecial = false;

        /// <summary>
        /// 从Dialogue对象里得到相关的信息
        /// 包括说话的人和其说的话
        /// </summary>
        /// <param name="dialogue"></param>
        public override void SetDialogue(Dialogue dialogue,bool isSpecial = false, SpecialEvent sEvent = null)
        {
            base.SetDialogue(dialogue);
            animator.SetBool("IsOpen", true);

            sentences.Clear();
            types.Clear();

            if(isSpecial)
            {
                mIsSpecial = true;
                foreach (var dialogueInfo in dialogue.GetDialogueInfos())
                {
                    string info = "";
                    types.Enqueue(dialogueInfo.npcType);
                    actions.Enqueue(sEvent(dialogueInfo.Info, out info));
                    sentences.Enqueue(info);
                }
                return;
            }

            foreach (var dialogueInfo in dialogue.GetDialogueInfos())
            {
                sentences.Enqueue(dialogueInfo.Info);
                types.Enqueue(dialogueInfo.npcType);
            }
        }

        public override void ShowNextSentence()
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
            base.ShowNextSentence();
            if (mIsSpecial)
            {
                actions.Dequeue()?.Invoke();
            }
            NPC_TYPE type = types.Dequeue();
            EventPic.sprite = NPCImagesMgr.GetImage(type);
            string sentence = sentences.Dequeue();
            nameText.text = type.ToString();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        IEnumerator TypeSentence(string sentence)
        {
            dialogueText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }
        }

        void EndDialogue()
        {
            nowDialogue.OnAction(Dialogue.DIALOGUE_EVENT.ON_END);
            animator.SetBool("IsOpen", false);
            mIsSpecial = false;
        }

        private void Start()
        {
            sentences = new Queue<string>();
            types = new Queue<NPC_TYPE>();
            actions = new Queue<System.Action>();

            animator = GetComponent<Animator>();
            EventPic = transform.Find("NPCImage").GetComponent<Image>();
            nameText = transform.Find("NameText").GetComponent<Text>();
            dialogueText = transform.Find("DialogueText").GetComponent<Text>();
        }

    }
}