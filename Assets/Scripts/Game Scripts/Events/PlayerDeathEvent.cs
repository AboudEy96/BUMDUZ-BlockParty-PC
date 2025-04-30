using System;
using UnityEngine;

public class PlayerDeathEvent : MonoBehaviour
{
    public void OnVoidDeath(GameObject player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>(); // الحصول على الـ Rigidbody
        if (rb != null)
        {
            // تأكد أن الـ Rigidbody ليس في وضع Kinematic
            rb.isKinematic = false;
            
            // زيادة القوة للتأكد من الدفع بشكل أكبر
            Vector3 pushUp = new Vector3(0, 100f, 0); // قوة أكبر للتأكد من الدفع بشكل واضح
            rb.AddForce(pushUp, ForceMode.Impulse); // استخدام Impulse لتطبيق القوة بشكل مفاجئ

            // التأكد من إعدادات الفيزيائيات: تأكد أن الكتلة ليست ثقيلة جدًا، والـ drag ليس مرتفعًا
            rb.mass = 1f; // ضبط الكتلة على 1 إذا كانت مرتفعة جدًا
            rb.drag = 0f; // التأكد من أن الـ drag صفر
            rb.angularDrag = 0f; // التأكد من أن الـ angular drag صفر
        }
        else
        {
            Debug.LogWarning("No Rigidbody found on player!");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
    
            OnVoidDeath(other.gameObject);
    }
}