using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CLX
{
    public class DialogueDesignPanel : EditorWindow
    {
        [MenuItem("CLX/Super Dialogue Text Design")]
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow(typeof(DialogueDesignPanel));
            window.titleContent = new GUIContent("文本编辑器");
            window.minSize = new Vector2(400, 400);
        }

        #region private Data
        private List<DialogueInfo> dialogueInfos;
        private DialogueInfo tmpDialogueInfo;
        private string dialoguePath;
        private bool errorPath;
        private int idx_read;
        private string tmpStr;

        private int IdxRead
        {
            get
            {
                return idx_read;
            }
            set
            {
                idx_read = value;
                if (dialogueInfos.Count <= value || value < 0)
                {
                    return;
                }
                ChangeNowInfo(IdxRead);
            }
        }

        /// ReadDialogue 函数中，切换时是否数组越界
        bool isError;
        #endregion

        DialogueDesignPanel()
        {
            dialogueInfos = new List<DialogueInfo>();
            tmpDialogueInfo = new DialogueInfo(NPC_TYPE.Box, "");
            dialoguePath = "";
            errorPath = false;
            IdxRead = 0;
            isError = false;
        }

        private DialogueInfo GetNowInfo()
        {
            return tmpDialogueInfo;
        }

        private DialogueInfo ChangeNowInfo(int id)
        {
            tmpDialogueInfo.Info = dialogueInfos[id].Info;
            tmpDialogueInfo.npcType = dialogueInfos[id].npcType;
            return GetNowInfo();
        }

        /// <summary>
        /// 编辑器绘制
        /// </summary>
        private void OnGUI()
        {
            DrawBeginText();

            GUILayout.BeginHorizontal();
            ChooseXML();
            GUILayout.EndHorizontal();

            #region 提示语句
            GUILayout.BeginVertical();
            if (errorPath == true)
            {
                EditorGUILayout.HelpBox("请选择 " + Application.dataPath + " /Resources/" +
                        DialogueManager.DialoguePath + "文件夹内的文件!", MessageType.Error);
            }
            else
            {
                EditorGUILayout.HelpBox("请选择 " + Application.dataPath + " /Resources/" +
                        DialogueManager.DialoguePath + "文件夹内的文件!", MessageType.None);
            }
            GUILayout.EndVertical();
            #endregion

            /// 若有文本加载了，则显示
            if (string.IsNullOrEmpty(dialoguePath) == false)
            {
                ReadDialogue(); GUILayout.BeginHorizontal();

                #region Show Buttons
                if (GUILayout.Button("重置此对话", GUILayout.MaxWidth(200)))
                {

                    DialogueReset();
                }
                if (GUILayout.Button("保存此对话", GUILayout.MaxWidth(200)))
                {
                    Save();
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("删除此对白", GUILayout.MaxWidth(200)))
                {

                    DeleteDialogue();
                }
                if (GUILayout.Button("添加新对白", GUILayout.MaxWidth(200)))
                {
                    AddDialogue();
                }
                GUILayout.EndHorizontal();
                if (GUILayout.Button("保存到文件", GUILayout.MaxWidth(400)))
                {
                    SaveToXML();
                }
                #endregion

            }

        }

        void DrawBeginText()
        {
            GUILayout.BeginVertical();

            //绘制标题
            GUILayout.Space(10);
            GUI.skin.label.fontSize = 24;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Super Dialogue 0.2v 文本编辑器");
            GUILayout.EndVertical();

            GUI.skin.label.fontSize = 16;
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            GUILayout.Label("Save Path", EditorStyles.boldLabel);
            //绘制文本
            GUILayout.Space(10);
        }

        /// <summary>
        /// 选择XML文件路径
        /// </summary>
        void ChooseXML()
        {
            dialoguePath = EditorGUILayout.TextField(dialoguePath, GUILayout.ExpandWidth(false), GUILayout.Width(300f));
            if (GUILayout.Button("打开", GUILayout.ExpandWidth(false)))
            {
                dialoguePath = EditorUtility.OpenFilePanel("选择XML文本文件", dialoguePath, "xml,txt");

                /// 如果不包含这个字符串的话，说明路径不合法
                if (dialoguePath.Contains(Application.dataPath + "/Resources/" +
                    DialogueManager.DialoguePath) == false)
                {
                    errorPath = true;
                    dialoguePath = "";
                }
                else
                {
                    //dialoguePath = dialoguePath.Replace(Application.dataPath + "/Resources/" +
                    //    DialogueManager.DialoguePath, "");
                    //dialoguePath = dialoguePath.Replace(".xml", "");
                    dialogueInfos = DialogueManager.GetDialogueInfos(dialoguePath);
                    if (dialogueInfos.Count == 0)
                    {
                        IdxRead = -1;
                    }
                    else
                    {
                        IdxRead = 0;
                    }
                    errorPath = false;
                }
            }
        }

        /// <summary>
        /// 加载对话
        /// </summary>
        void ReadDialogue()
        {
            if (IdxRead != -1)
            {
                #region 显示对白信息
                DialogueInfo info = GetNowInfo();
                GUILayout.BeginHorizontal();
                GUI.DrawTexture(GUILayoutUtility.GetRect((position.width / 2), 0, GUILayout.MaxHeight(200), GUILayout.MaxWidth(200)),
                    NPCImagesMgr.GetImage(info.npcType).texture);

                GUILayout.BeginVertical();
                info.npcType = (NPC_TYPE)EditorGUILayout.EnumPopup(info.npcType, GUILayout.MaxWidth(200));
                GUI.enabled = false;
                GUILayout.TextArea(info.Info, GUILayout.MaxWidth(200), GUILayout.MaxHeight(50));
                GUI.enabled = true;

                EditorStyles.textField.wordWrap = true;
                tmpStr = EditorGUILayout.TextArea(tmpStr, GUILayout.MaxWidth(200), GUILayout.MaxHeight(50));
                EditorStyles.textField.wordWrap = false;

                if (GUILayout.Button("采用此对话", GUILayout.MaxWidth(200)))
                {
                    info.Info = tmpStr;
                }

                #endregion
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
            }

            GUILayout.BeginHorizontal();

            #region 切换当前对话Buttons
            if (GUILayout.Button("上一个对话", GUILayout.MaxWidth(80)))
            {
                if (IdxRead <= 0)
                {
                    isError = true;
                }
                else
                {
                    isError = false;
                    IdxRead--;
                }
            }

            if (GUILayout.Button("下一个对话", GUILayout.MaxWidth(80)))
            {
                if (IdxRead >= dialogueInfos.Count - 1)
                {
                    isError = true;
                }
                else
                {
                    isError = false;
                    IdxRead++;
                }
            }
            #endregion

            GUILayout.EndHorizontal();

            if (isError == true)
            {
                EditorGUILayout.HelpBox("向前或者向后已经没有对白", MessageType.Error);
            }
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 重置当前对白信息
        /// </summary>
        void DialogueReset()
        {
            if (idx_read >= 0)
                ChangeNowInfo(IdxRead);
        }

        /// <summary>
        /// 暂时保存当前信息
        /// </summary>
        void Save()
        {
            if (IdxRead < 0 || IdxRead >= dialogueInfos.Count) return;
            dialogueInfos[IdxRead].Info = GetNowInfo().Info;
            dialogueInfos[IdxRead].npcType = GetNowInfo().npcType;
        }

        /// <summary>
        /// 删除该对白
        /// </summary>
        void DeleteDialogue()
        {
            if (dialogueInfos.Count <= 0 || IdxRead < 0)
            {
                return;
            }
            dialogueInfos.RemoveAt(IdxRead);
            IdxRead--;
            DialogueReset();
        }

        /// <summary>
        /// 添加对白
        /// </summary>
        void AddDialogue()
        {
            dialogueInfos.Insert(IdxRead + 1, new DialogueInfo(NPC_TYPE.Box, ""));
            IdxRead++;
        }

        /// <summary>
        /// 按XML格式保存成XML/TXT文件
        /// </summary>
        void SaveToXML()
        {
            DialogueManager.CreateXMLFile(dialoguePath, dialogueInfos);
        }

    }
}