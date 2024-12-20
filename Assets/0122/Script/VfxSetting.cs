using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//VFXデータ構造体
[Serializable]
public struct VFX
{
    public string tag;          //検索用のタグ
    public GameObject VfxPrefab; //VFXデータ
}
//メニュー
[CreateAssetMenu(menuName = "ScriptableObject/VFX Setting", fileName = "VfxSetting")]


public class VfxSetting : ScriptableObject
{
    [SerializeField] private List<VFX> VFXList; //VFX構造体のリスト


    //リストを取得する
    public List<VFX> Get_VFXList
    {
        get { return VFXList; }
    }
}