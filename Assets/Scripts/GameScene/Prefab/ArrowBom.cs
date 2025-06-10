/*
    ArrowBom.cs
        �U����1�Ƃ��Ă���ArrowBom�̋������v�Z���܂�
        
        ����
        �E�㕔�ŏ����ҋ@
        �E��]���Ȃ��痎����
        �E��������Arrow�𔭎�
 */

using UnityEngine;
/// <summary>
/// �U����1�Ƃ��Ă���ArrowBom�̋������v�Z���܂�
/// </summary>
public class ArrowBom : MonoBehaviour
{
    /// <summary>
    /// �d��
    /// </summary>
    [SerializeField] float gravity;

    [SerializeField] GameObject Arrow;

    [SerializeField] float wait_time;
    [SerializeField] float stoptime_lastwait;
    [SerializeField] float explosion_time;
    Vector2 velocity = Vector2.zero;

    Vector2 startpos;
    float elapsed_time = 0;
    float rotate_speed = 0.5f;

    void Start()
    {
        gravity *= -1;
        startpos = transform.position;

        // ��]�̋��������R�ȃ����_����
        rotate_speed += Random.Range(-0.1f, 0.1f);
        rotate_speed *= Random.Range(0, 2) == 0 ? 1 : -1;
    }
    void Update()
    {
        // ���̃I�u�W�F�N�g�̌o�ߎ���
        elapsed_time += Time.deltaTime;

        if (wait_time < elapsed_time) // �ҋ@�㗎��
        {
            AfterWaitUpdate();
        }
        else // �ҋ@��
        {
            float movetime = wait_time - stoptime_lastwait;
            transform.position = startpos + (3f * Mathf.Min(movetime, elapsed_time) / movetime * Vector2.down * 0.25f);
            //�����ʒu���班�������i�񂾈ʒu�܂ł������ړ���������~������Ɂ�Arrow�𔭎�
        }

    }

    // �������n�܂��Ă����Update����
    void AfterWaitUpdate()
    {
        // �d�͂ŗ���
        velocity += Vector2.up * gravity;
        transform.position += (Vector3)velocity;

        // ��]���Ȃ��痎�����鋓��
        transform.Rotate(0, 0, rotate_speed);

        // ���Ԃ��o�Ɣ���
        if (explosion_time < elapsed_time)
        {
            Explosion();
            Destroy(gameObject);
        }
    }
    void Explosion()
    {
        int arrowCount = 8;
        for (int i = 0; i < arrowCount; i++)
        {
            // arrow�����ꂼ��̌����ƈʒu�Ő���
            float angle = (2 * Mathf.PI / arrowCount * i) + transform.eulerAngles.z;
            Vector2 direction = new(Mathf.Sin(angle), Mathf.Cos(angle));

            ArrowController arrow =
                Instantiate(Arrow, transform.position + ((Vector3)direction * 1.0f), Quaternion.Euler(0, 0, 180 - (angle * Mathf.Rad2Deg)))
                .GetComponent<ArrowController>();
            arrow.wait_time = 0;
            arrow.velocity = direction * 0.25f;
        }
    }
}
