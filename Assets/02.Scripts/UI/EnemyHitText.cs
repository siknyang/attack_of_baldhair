using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHitText : MonoBehaviour
{
    public float moveSpeed = 1f; // 텍스트 이동 속도
    public float alphaSpeed = 2f; // 투명도 변환 속도
    public float destroyTime = 1f; // 삭제 시간
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
