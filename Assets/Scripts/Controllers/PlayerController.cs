﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class PlayerController : NetworkBehaviour {

    [SerializeField] private LayerMask _movementMask;

    private Character _character;
    private Camera _cam;

    private void Awake() 
    {
        _cam = Camera.main;
    }

    public void SetCharacter(Character character, bool isLocalPlayer) 
    {
        _character = character;
        if (isLocalPlayer)
        {
            _cam.GetComponent<CameraController>().target = character.transform;
        }
    }

    private void Update() 
    {
        if (isLocalPlayer) 
        {
            if (_character != null && !EventSystem.current.IsPointerOverGameObject()) 
            {
                if (Input.GetMouseButton(1)) 
                {
                    Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100f, _movementMask)) 
                    {
                        CmdSetMovePoint(hit.point);
                    }
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100f, ~(1 << LayerMask.NameToLayer("Player"))))
                    {
                        Interactable interactable = hit.collider.GetComponent<Interactable>();
                        if (interactable != null)
                        {
                            CmdSetFocus(interactable.GetComponent<NetworkIdentity>());
                        }
                    }
                }
            }
        }
    }

    [Command]
    public void CmdSetMovePoint(Vector3 point) 
    {
        _character.SetMovePoint(point);
    }

    [Command]
    public void CmdSetFocus(NetworkIdentity newFocus)
    {
        _character.SetNewFocus(newFocus.GetComponent<Interactable>());
    }

    private void OnDestroy() 
    {
        if (_character != null) Destroy(_character.gameObject);
    }
}
