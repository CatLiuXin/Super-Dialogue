using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CLX
{
    /// <summary>
    /// 面板的展示逻辑抽象接口
    /// </summary>
    public interface IDialogueUI
    {
        /// <summary>
        /// 将该人的对话展示出来
        /// </summary>
        /// <param name="dialogue"></param>
        void SetDialogue(Dialogue dialogue,bool isSpecial = false, SpecialEvent sEvent = null);
        void ShowNextSentence();
    }
}