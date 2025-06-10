/*
    Confetti_prefab.cs
        �Q�[���N���A��Ɏ�������o���܂�

        ����
        �E������
        �E�g���I�������폜
 */
using UnityEngine;

public class ConfettiController : MonoBehaviour
{
    [SerializeField] SpriteRenderer my;
    [SerializeField] Color[] colors;
    [SerializeField] float fall_speed;
    float rotate_speed = 1.3f;

    void Start()
    {
        transform.Rotate(0, 0, Random.Range(0.0f, 180.0f));

        // ��]���x�������_���ɂ���
        rotate_speed += Random.Range(-0.7f, 0.7f);
        rotate_speed *= Random.Range(0, 2) == 0 ? 1 : -1;

        //�F������
        my.color = colors[Random.Range(0, colors.Length)];
    }

    void Update()
    {
        transform.position += Vector3.down * fall_speed;
        transform.Rotate(0, 0, rotate_speed);

        // ���ɃX�N���[���A�E�g�ō폜
        float offScreen_y = -10;
        if (transform.position.y < offScreen_y)
        {
            Destroy(gameObject);
        }
    }
}
