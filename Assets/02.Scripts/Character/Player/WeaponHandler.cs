using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Player player;

    /*
    public void ThrowWeapon() // 무기 투척
    {
        Debug.Log("무기 투척 실행");
        GameObject weapon = player.currentWeapon;

        if (weapon != null)
        {
            // 타겟 에너미 위치 가져오기
            Vector3 targetPosition = player.stateMachine.Target.transform.position;

            // 무기를 플레이어의 손에서 분리
            weapon.transform.SetParent(null);
            Debug.Log("분리");

            // 무기를 타겟 에너미 방향으로 던지기
            Rigidbody weaponRigidbody = weapon.GetComponent<Rigidbody>();
            Vector3 throwDirection = (targetPosition - weapon.transform.position).normalized;
            float throwForce = 5f; // 던지는 힘
            weaponRigidbody.velocity = throwDirection * throwForce;
            //weaponRigidbody.AddForce(throwDirection *  throwForce, ForceMode.Impulse);

            player.currentWeapon = null;
        }
    }
    */

}