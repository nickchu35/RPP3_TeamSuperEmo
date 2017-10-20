using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour {

	public float _raycastLength = 10f;
	public Transform _pickupHolder;
	public float _pickupSpeed = 0.5f;

	private GameObject _pickup = null;
	private RaycastHit _hit;
	private bool _updatePickup = false;

	// Use this for initialization
	void Start () {
		GameObject _uiCam = GameObject.FindGameObjectWithTag ("UICamera");
		_uiCam.gameObject.SetActive (false);
		
	}
	
	// Update is called once per frame
	void Update () {
		// Debug.Log ("Update");

		this.CheckRaycast ();

		this.UpdatePickup ();
	}

	private void CheckRaycast(){
		// Debug.Log (Input.mousePosition);
		//Ray raycast = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay (Camera.main.transform.position, Camera.main.transform.forward * 10, Color.black);

		//if (Physics.Raycast(raycast, out _hit, _raycastLength)){
		Debug.Log(Camera.main.transform.forward);

		if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out _hit, _raycastLength)){
			if (_hit.collider.gameObject.tag == "Pickup"){
				//if (Input.GetKeyUp(KeyCode.Mouse0)){
				if(Input.GetButtonUp("Fire1")){
					if (_hit.collider.gameObject != this._pickup) {
						this.PickupItem ();
					} else {
						this._pickup = null;
						this._updatePickup = false; 
					}
					// activate UI?
				}
			}
		}
	}

	private void PickupItem(){
		Debug.Log("Pickup!");
		this._pickup = _hit.collider.gameObject;
		//Destroy(_hit.collider.gameObject);
		_hit.collider.gameObject.transform.DOMove(_pickupHolder.position, _pickupSpeed).SetEase(Ease.OutExpo).OnComplete(this.ActivateUpdatePickup);
		//_hit.collider.gameObject.transform.DOMove
	}

	private void UpdatePickup(){
		if (this._updatePickup && this._pickup) {
			this._pickup.gameObject.transform.position = _pickupHolder.position;
		}
	}

	private void ActivateUpdatePickup(){
		this._updatePickup = true;
	}
}
