using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLX
{
    public class DialogueTrigger : MonoBehaviour
    {

        public Dialogue dialogue;

        public void TriggerDialogue()
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }

    }
}