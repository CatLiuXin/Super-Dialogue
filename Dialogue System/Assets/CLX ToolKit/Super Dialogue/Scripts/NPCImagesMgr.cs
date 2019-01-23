using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CLX
{
    public class NPCImagesMgr : MonoBehaviour
    {
        private static string imagesPath = "Images/NPC/";
        private static NPCImagesMgr _instance;
        public static NPCImagesMgr Instance
        {
            get
            {
                return _instance;
            }
        }
        private static Dictionary<NPC_TYPE, Sprite> npcImages = new Dictionary<NPC_TYPE, Sprite>();

        public static Sprite GetImage(NPC_TYPE type)
        {
            Sprite image;
            if (npcImages.TryGetValue(type, out image) == false)
            {
                image = LoadImage(type);
            }
            return image;
        }

        private static Sprite LoadImage(NPC_TYPE type)
        {
            Sprite image = Resources.Load<Sprite>(imagesPath + type);
            if (image == null)
            {
                Debug.LogError("Can't find The image of " + imagesPath + type + "!");
                return null;
            }
            return image;
        }

    }

}