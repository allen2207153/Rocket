using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//サウンドデータ構造体
[Serializable]
public struct Sound
{
    public string tag;          //検索用のタグ
    public AudioClip audioClip; //サウンドデータ

    public float volume;        //音量調整
    public float fadeTime;      //フェードイン/アウト

    public bool isLoop;         //ループ設定
}

//サウンドデータ管理
[CreateAssetMenu(menuName = "ScriptableObject/SoundSetting", fileName = "SoundSetting")]
public class SoundSetting : ScriptableObject
{
    [SerializeField] private List<Sound> BGMList; //BGM構造体のリスト
    [SerializeField] private List<Sound> SEList; //SE構造体のリスト
    

    //リストを取得する
    public List<Sound> Get_SEList
    {
        get { return SEList; }
    }

 public List<Sound> Get_BGMList
  {
     get { return BGMList; }
 }


}