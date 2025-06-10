/*
    ArrowController.cs
        �eArrow�̋������������܂�
        
        ����
        �E�㕔�ŏ����ҋ@
        �E���i����
        �E�g���I�������폜
 */

using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float fall_speed;
    public float wait_time;
    public float stoptime_lastwait;
    [HideInInspector] public Vector2 velocity = Vector2.zero;

    Vector2 startpos;
    float elapsed_time = 0;

    void Start()
    {
        startpos = transform.position;
        if (velocity == Vector2.zero)
        {
            velocity = Vector2.down * fall_speed;
        }
    }
    void Update()
    {
        // ���̃I�u�W�F�N�g�̌o�ߎ���
        elapsed_time += Time.deltaTime;

        if (wait_time < elapsed_time)
        {
            AfterWaitUpdate();
        }
        else
        {
            float movetime = wait_time - stoptime_lastwait;
            transform.position = startpos + (3f * Mathf.Min(movetime, elapsed_time) / movetime * velocity);
                //�����ʒu���班�������i�񂾈ʒu�܂ł������ړ���������~������Ɂ�Arrow�𔭎�
        }

    }

    // �������n�܂��Ă����Update����
    void AfterWaitUpdate()
    {
        // �X�V�O�̍��W
        Vector2 myPosition = transform.position;

        // ��������
        transform.position = myPosition + velocity;

        // ���ɃX�N���[���A�E�g��
        float offScreen_y = -10;
        if (transform.position.y < offScreen_y)
        {
            Destroy(gameObject);
        }
        // �����ɂ�������
        float far = 20;
        if (far < Vector2.Distance(Vector2.zero, transform.position))
        {
            Destroy(gameObject);
        }
    }
}
