using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : MonoBehaviour
{
    GameObject mTarget;
    float mMoveSpeed;

	public void InitData(GameObject target, float move_speed, Color color)
    {
        mTarget = target;
        mMoveSpeed = move_speed;
		GetComponent<MeshRenderer> ().material.color = color;
    }
    
    private Vector3 mTargetPos;
    // Update is called once per frame
    void Update()
    {
		if (mTarget == null)//追人追到一半那个人死了
			Destroy(gameObject);
		else
		{
			Collider coll = mTarget.PropertyComponent ().bodyCollider;
			mTargetPos = mTarget.transform.position;
			if (coll is MeshCollider) {
				mTargetPos += GetComponentInParent<CharacterController> ().center;
			} else {
				if (coll is CharacterController) {
					mTargetPos += (coll as CharacterController).center;
				} else {
					mTargetPos.y = 1;
				}
			}

			float rang = Vector3.Distance(transform.position, mTargetPos);//子dan和di人距离
			float total_time = rang / this.mMoveSpeed;//子dan到di人的shijian
			if (rang < 8 && Time.deltaTime < total_time) {
				transform.position = Vector3.Lerp(this.transform.position, this.mTargetPos, Time.deltaTime / total_time);

			}else {
				Destroy(gameObject);
			}
		}
    }
}      
