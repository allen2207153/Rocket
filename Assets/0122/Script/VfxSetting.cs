using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//VFX�f�[�^�\����
[Serializable]
public struct VFX
{
    public string tag;          //�����p�̃^�O
    public GameObject VfxPrefab; //VFX�f�[�^
}
//���j���[
[CreateAssetMenu(menuName = "ScriptableObject/VFX Setting", fileName = "VfxSetting")]


public class VfxSetting : ScriptableObject
{
    [SerializeField] private List<VFX> VFXList; //VFX�\���̂̃��X�g


    //���X�g���擾����
    public List<VFX> Get_VFXList
    {
        get { return VFXList; }
    }
}