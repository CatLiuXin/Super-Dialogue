using UnityEngine;
using CLX;/// Dilogue的命名空间
using UnityEngine.UI;/// 与UI打交道要加的命名空间

namespace DialogueGuidance {
    public class DilogueEventSystemTest : MonoBehaviour
    {
        [SerializeField]
        Dialogue dialogue;

        bool shouldCheck = false;

        private void Start()
        {
            #region 注册事件
            dialogue.RegisterAction(Dialogue.DIALOGUE_EVENT.ON_BEGIN, () => {
                Debug.Log("Dialogue Begin!");
                GetComponent<Button>().interactable = false;
                shouldCheck = true;
            });
            dialogue.RegisterAction(Dialogue.DIALOGUE_EVENT.ON_STEP, () => {
                Debug.Log("Dialogue Step");
            });
            dialogue.RegisterAction(Dialogue.DIALOGUE_EVENT.ON_END, () => {
                Debug.Log("Dialogue End!");
                GetComponent<Button>().interactable = true;
                shouldCheck = false;
            });
            #endregion
        }

        public void TriggerDialogue()
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }

        private void Update()
        {
            if(shouldCheck)
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    DialogueManager.Instance.DisplayNextSentence();
                }
            }
        }

    }
}