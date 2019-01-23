using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CLX
{
    /// <summary>
    /// 对白UI表现
    /// 此脚本所挂载的对象必须有以下子物体：
    /// 1. 名为NameText的Text物体
    /// 2. 名为DialogueText的Text物体
    /// 3. 一个Image物体
    /// </summary>
    public class EventDialogueUI : MonoBehaviour, IDialogueUI
    {
        private Text nameText;
        private Text dialogueText;
        private Image EventPic;
        private Animator animator;

        private Queue<string> sentences;
        private Queue<NPC_TYPE> types;

        /// <summary>
        /// 从Dialogue对象里得到相关的信息
        /// 包括说话的人和其说的话
        /// </summary>
        /// <param name="dialogue"></param>
        public void SetDialogue(Dialogue dialogue)
        {
            animator.SetBool("IsOpen", true);

            sentences.Clear();
            types.Clear();

            foreach (var dialogueInfo in dialogue.GetDialogueInfos())
            {
                sentences.Enqueue(dialogueInfo.Info);
                types.Enqueue(dialogueInfo.npcType);
            }
        }

        public void ShowNextSentence()
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
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
            animator.SetBool("IsOpen", false);
        }

        private void Start()
        {
            sentences = new Queue<string>();
            types = new Queue<NPC_TYPE>();
            animator = GetComponent<Animator>();
            EventPic = transform.Find("NPCImage").GetComponent<Image>();
            nameText = transform.Find("NameText").GetComponent<Text>();
            dialogueText = transform.Find("DialogueText").GetComponent<Text>();
        }

    }
}