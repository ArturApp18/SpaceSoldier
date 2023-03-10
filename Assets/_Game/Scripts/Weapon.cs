using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts
{
	public class Weapon : MonoBehaviour
	{
		[SerializeField] private PointerInput _pointerInput;

		[SerializeField] private Transform _gunPosition;
		[SerializeField] private GameObject _shootVFX;

		[SerializeField] private float _range;
		[SerializeField] private float _damage;
		[SerializeField] private float _fireRate;
		

		private void OnEnable()
		{
			_pointerInput.OnStartShooting += StartShoot;
			_pointerInput.OnEndShooting += EndShoot;
		}

		private void OnDisable()
		{
			_pointerInput.OnStartShooting -= StartShoot;
			_pointerInput.OnEndShooting -= EndShoot;
		}


		private void StartShoot()
		{
			StartCoroutine(Shooting());
		}

		private void EndShoot()
		{
			StopCoroutine(Shooting()); 
		}

		private void Shoot()
		{
			RaycastHit hit;
			GameObject muzzleFlash = Instantiate(_shootVFX, _gunPosition.position, _gunPosition.rotation);
			Destroy(muzzleFlash, 0.1f);
			if(Physics.Raycast(_gunPosition.position, _gunPosition.transform.forward, out hit, _range))
			{
				GameObject impact = Instantiate(_shootVFX, hit.point, Quaternion.LookRotation(hit.normal));
				Destroy(impact, 0.1f);
				Debug.Log(hit.collider.gameObject.name);
			}
		}

		private IEnumerator Shooting()
		{
			while (_pointerInput.isPressed)
			{
				Shoot();
				yield return new WaitForSeconds(_fireRate);
			}
		}
	}
}