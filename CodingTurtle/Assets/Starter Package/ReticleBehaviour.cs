/*
 * Copyright 2021 Google LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ReticleBehaviour : MonoBehaviour
{
    public GameObject Child;
    public DrivingSurfaceManager DrivingSurfaceManager;
    public ARPlane CurrentPlane;

    [Header("Custom parameters")]
    [Tooltip("The prefab to spawn when the user clicks on the screen")]
    [SerializeField] private GameObject _prefabToSpawn;
    [SerializeField] private GameObject _prefabFather;

    private bool _isPlaced = false;

    // Start is called before the first frame update
    private void Start()
    {
        Child = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        // Get the center of the screen
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        // Raycast to the center of the screen
        var hits = new List<ARRaycastHit>();
        DrivingSurfaceManager.RaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinBounds);

        CurrentPlane = null;
        ARRaycastHit? hit = null;
        if (hits.Count > 0)
        {
            // If you don't have a locked plane already...
            var lockedPlane = DrivingSurfaceManager.LockedPlane;
            hit = lockedPlane == null
                // ... use the first hit in `hits`.
                ? hits[0]
                // Otherwise use the locked plane, if it's there.
                : hits.SingleOrDefault(x => x.trackableId == lockedPlane.trackableId);
        }

        if (hit.HasValue)
        {
            CurrentPlane = DrivingSurfaceManager.PlaneManager.GetPlane(hit.Value.trackableId);
            // Move this reticle to the location of the hit.
            transform.position = hit.Value.pose.position;

            // Rotate the reticle to be parallel to the detected plane
            transform.rotation = CurrentPlane.transform.rotation;
        }
        Child.SetActive(CurrentPlane != null);

        // Spawn the object on click only one time
        if (!_isPlaced) spawnGivenGameObjectOnClick();
    }

    private void spawnGivenGameObjectOnClick()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0) && CurrentPlane != null)
        {
            // Calculate rotation to face the camera
            Quaternion lookRotation = new Quaternion(0, -Camera.main.transform.rotation.y, 0, Camera.main.transform.rotation.w);

            // Spawn the object at the reticle's position and with the calculated rotation as child of the prefabFather
            GameObject childObject = Instantiate(_prefabToSpawn, transform.position, lookRotation);
            childObject.transform.parent = _prefabFather.transform;

            _isPlaced = true;
        }
    }
}
