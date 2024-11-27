using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sway : MonoBehaviour
{
    public Player_Movement move;

    [Header("Sway")]
    public float step = 0.01f;
    public float maxStepDistance = 0.06f;
    Vector3 swayPos;

    [Header("Sway Rotation")]
    public float rotationStep = 4f;
    public float maxRotationStep = 5f;
    Vector3 swayEulerRot;

    public float smooth = 10f;
    float smoothRot = 12f;

    [Header("Bobbing")]
    public float speedCurve;
    float curveSin { get => Mathf.Sin(speedCurve); }
    float curveCos { get => Mathf.Cos(speedCurve); }

    public Vector3 travelLimit = Vector3.one * 0.025f;
    public Vector3 bobLimit = Vector3.one * 0.01f;
    Vector3 bobPosition;

    public float bobExaggeration;

    [Header("Bob Rotation")]
    public Vector3 multiplier;
    Vector3 bobEulerRotation;

    [Header("Recoil")]
    public Vector2 recoil;  // �������������洢X��Y�����ϵ�ƫ����
    public Vector2 addSpeed = new Vector2(0.5f, 0.75f); // �����������ٶ�
    public Vector2 subSpeed = new Vector2(3f, 5f); // �����������ٶ�
    public Vector2 maxRecoil = new Vector2(1, 5); // �����������ֵ

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Sway();
        SwayRotation();
        BobOffset();
        BobRotation();
        CompositePositionRotation();
        HandleRecoil();  // ���������
    }

    Vector2 walkInput;
    Vector2 lookInput;

    void GetInput()
    {
        walkInput.x = Input.GetAxis("Horizontal");
        walkInput.y = Input.GetAxis("Vertical");
        walkInput = walkInput.normalized;

        lookInput.x = Input.GetAxis("Mouse X");
        lookInput.y = Input.GetAxis("Mouse Y");
    }

    void Sway()
    {
        Vector3 invertLook = lookInput * -step;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = invertLook;
    }

    void SwayRotation()
    {
        Vector2 invertLook = lookInput * -rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);

        swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
    }

    void CompositePositionRotation()
    {
        // ������������ת���뵽����������λ�ú���ת��
        // �� recoil �� Vector2 ת��Ϊ Vector3��z ������Ϊ 0
        Vector3 recoilRotation = new Vector3(-recoil.y, recoil.x, 0);

        // ʹ�� Vector3 �ӷ��� swayEulerRot �� recoilRotation ���
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos + bobPosition, Time.deltaTime * smooth);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulerRot + recoilRotation) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
    }

    void BobOffset()
    {
        speedCurve += Time.deltaTime * (move.grounded ? (Input.GetAxis("Horizontal") + Input.GetAxis("Vertical")) * bobExaggeration : 1f) + 0.01f;

        bobPosition.x = (curveCos * bobLimit.x * (move.grounded ? 1 : 0)) - (walkInput.x * travelLimit.x);
        bobPosition.y = (curveSin * bobLimit.y) - (Input.GetAxis("Vertical") * travelLimit.y);
        bobPosition.z = -(walkInput.y * travelLimit.z);
    }

    void BobRotation()
    {
        bobEulerRotation.x = (walkInput != Vector2.zero ? multiplier.x * (Mathf.Sin(2 * speedCurve)) : multiplier.x * (Mathf.Sin(2 * speedCurve) / 2));
        bobEulerRotation.y = (walkInput != Vector2.zero ? multiplier.y * curveCos : 0);
        bobEulerRotation.z = (walkInput != Vector2.zero ? multiplier.z * curveCos * walkInput.x : 0);
    }

    // ���������
    void HandleRecoil()
    {
        // ʹ��Mathf.MoveTowards�𽥽�recoil.x��recoil.y���ٵ�0
        recoil.x = Mathf.MoveTowards(recoil.x, 0, subSpeed.x * Time.deltaTime);
        recoil.y = Mathf.MoveTowards(recoil.y, 0, subSpeed.y * Time.deltaTime);

        // ������������к��������
        if (Input.GetKey(KeyCode.Mouse0))
        {
            AddRecoil();
        }

        // ��������Ӧ�õ���������ת��
        transform.localEulerAngles = new Vector3(-recoil.y, recoil.x, 0);
    }

    // ���Ӻ�����
    void AddRecoil()
    {
        recoil.x = Mathf.Clamp(recoil.x + Random.Range(-1, 1) * addSpeed.x, -maxRecoil.x, maxRecoil.x);
        recoil.y = Mathf.Clamp(recoil.y + addSpeed.y, 0, maxRecoil.y);
    }
}