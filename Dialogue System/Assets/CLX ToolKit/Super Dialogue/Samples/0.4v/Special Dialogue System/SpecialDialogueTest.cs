using CLX;
using UnityEngine.UI;
using UnityEngine;

namespace DialogueGuidance
{
    public class SpecialDialogueTest : MonoBehaviour
    {
        Text mText;
        [SerializeField]
        Dialogue dialogue;
        bool shouldCheck = false;

        /// <summary>
        /// 示例特殊文本规则为：颜色（可选） 实际文本
        /// 例如：Red 文本（这个文本展现出来的就是红色）
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sText"></param>
        /// <returns></returns>
        System.Action SpecialEvent(string text, out string sText)
        {
            string[] texts = text.Split(' ');
            if(texts.Length == 1)
            {
                sText = texts[0];
                return () => {
                    mText.color = Color.black;
                };
            }
            System.Action action = null;
            switch(texts[0])
            {
                case "Red":
                    {
                        action = () => {
                            mText.color = Color.red;
                            Debug.Log("Red Text!");
                        };
                        break;
                    }
                case "Blue":
                    {
                        action = () => {
                            mText.color = Color.blue;
                            Debug.Log("Blue Text!");
                        };
                        break;
                    }
            }
            sText = texts[1];
            return action;
        }

        public void TriggerDialogue()
        {
            DialogueManager.Instance.StartDialogue(dialogue,true,SpecialEvent);
        }

        private void Start()
        {
            mText = GameObject.Find("DialogueText").GetComponent<Text>();

            #region 注册事件
            dialogue.RegisterAction(Dialogue.DIALOGUE_EVENT.ON_BEGIN, () => {
                GetComponent<Button>().interactable = false;
                shouldCheck = true;
            });
            dialogue.RegisterAction(Dialogue.DIALOGUE_EVENT.ON_END, () => {
                GetComponent<Button>().interactable = true;
                shouldCheck = false;
            });
            #endregion
        }
    }
}