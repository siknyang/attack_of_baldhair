using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHitText : MonoBehaviour
{
    public float moveSpeed = 1f; // �ؽ�Ʈ �̵� �ӵ�
    public float alphaSpeed = 2f; // ���� ��ȯ �ӵ�
    public float destroyTime = 1f; // ���� �ð�
    TextMeshPro text;
    Color alpha;

    void Start()
    {
        text = GetComponent<TextMeshPro>();
        alpha = text.color;
        Invoke("DestroyText", destroyTime);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    private void DestroyText()
    {
        Destroy(gameObject);
    }
}
