using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�T�E���h�f�[�^�\����
[Serializable]
public struct Sound
{
    public string tag;          //�����p�̃^�O
    public AudioClip audioClip; //�T�E���h�f�[�^

    public float volume;        //���ʒ���
    public float fadeTime;      //�t�F�[�h�C��/�A�E�g

    public bool isLoop;         //���[�v�ݒ�
}

//�T�E���h�f�[�^�Ǘ�
[CreateAssetMenu(menuName = "ScriptableObject/SoundSetting", fileName = "SoundSetting")]
public class SoundSetting : ScriptableObject
{
    [SerializeField] private List<Sound> BGMList; //BGM�\���̂̃��X�g
    [SerializeField] private List<Sound> SEList; //SE�\���̂̃��X�g
    

    //���X�g���擾����
    public List<Sound> Get_SEList
    {
        get { return SEList; }
    }

 public List<Sound> Get_BGMList
  {
     get { return BGMList; }
 }


}