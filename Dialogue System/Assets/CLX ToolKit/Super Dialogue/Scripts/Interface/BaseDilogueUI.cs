using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLX {
    public class BaseDilogueUI :MonoBehaviour,IDialogueUI
    {
        protected Dialogue nowDialogue;

        public virtual void SetDialogue(Dialogue dialogue,bool isSpecial=false, SpecialEvent sEvent = null)
        {
            nowDialogue = dialogue;
            dialogue.OnAction(Dialogue.DIALOGUE_EVENT.ON_BEGIN);
        }

        public virtual void ShowNextSentence()
        {
            /// 进行逻辑处理和调用事件
            nowDialogue.OnAction(Dialogue.DIALOGUE_EVENT.ON_STEP);
        }
    }
}