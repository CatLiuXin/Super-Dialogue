using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPC_TYPE
{
    Player,
    Box,
}
public static class NPCEnumMgr
{
    private static Dictionary<string,NPC_TYPE> npcName = new Dictionary<string, NPC_TYPE>();
    public static NPC_TYPE GetNPCString(string str_type)
    {
        NPC_TYPE type;
        if (npcName.TryGetValue(str_type,out type) == false)
        {
            if(System.Enum.TryParse<NPC_TYPE>(str_type,out type) == false)
            {
                Debug.LogError("The type is not illigal"+str_type);
            }
        }
        return type;
    }
}
