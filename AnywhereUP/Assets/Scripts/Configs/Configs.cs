using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Anywhere/Configs")]
public class Configs : ScriptableObject
{
    public string m_Host = "https://aw.weacw.com";
    public string m_GetInfoHost = "https://aw.weacw.com/anywhere/getinfo.php?";
    public string m_SearchInfoHost = "https://aw.weacw.com/anywhere/searchinfo.php?";
}
