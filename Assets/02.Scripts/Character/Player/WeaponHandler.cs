using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Player player;

    /*
    public void ThrowWeapon() // ���� ��ô
    {
        Debug.Log("���� ��ô ����");
        GameObject weapon = player.currentWeapon;

        if (weapon != null)
        {
            // Ÿ�� ���ʹ� ��ġ ��������
            Vector3 targetPosition = player.stateMachine.Target.transform.position;

            // ���⸦ �÷��̾��� �տ��� �и�
            weapon.transform.SetParent(null);
            Debug.Log("�и�");

            // ���⸦ Ÿ�� ���ʹ� �������� ������
            Rigidbody weaponRigidbody = weapon.GetComponent<Rigidbody>();
            Vector3 throwDirection = (targetPosition - weapon.transform.position).normalized;
            float throwForce = 5f; // ������ ��
            weaponRigidbody.velocity = throwDirection * throwForce;
            //weaponRigidbody.AddForce(throwDirection *  throwForce, ForceMode.Impulse);

            player.currentWeapon = null;
        }
    }
    */

}