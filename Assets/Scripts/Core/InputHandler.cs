using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Steamwar.Units;
using Steamwar.UI.Menus;
using Steamwar.Sectors;
using Steamwar.Renderer;
using Steamwar.Utils;
using Steamwar.Objects;

namespace Steamwar {

    public class InputHandler : Singleton<InputHandler>
    {
        public GameObject[] listenersObjects;
        public IMouseListener[] listeners;
        public float minScroll = 3.5F;
        public float maxScroll = 7.5F;
        public Vector2 lastMouse;
        public EscMenu escMenu;

        private bool[] buttonDown = new bool[] { false, false, false};

        void Start()
        {
            List<IMouseListener> mouseListeners = new List<IMouseListener>
            {
                ConstructionManager.Instance
            };
            foreach (GameObject obj in listenersObjects)
            {
                mouseListeners.AddRange(obj.GetComponents<IMouseListener>());
            }
            this.listeners = mouseListeners.ToArray();
        }

        void UpdateCameraPosition(Camera camera, float xAxisValue, float yAxisValue)
        {
            SectorData sectorData = SessionManager.session.activeSector;
            Sector sector = sectorData.sector;
            Vector3 position = camera.transform.position + new Vector3(xAxisValue * 0.25F, yAxisValue * 0.25F, 0.0f);
            Vector3 screenLeftBottom = camera.ViewportToWorldPoint(new Vector3(0.0F, 0.0F));
            Vector3 screenRightTop = camera.ViewportToWorldPoint(new Vector3(1.0F, 1.0F));
            Vector3 screenSize = screenRightTop - screenLeftBottom;
            Vector4 bounds = sectorData.bounds;
            if ((position.x + screenSize.x / 2) > (bounds.z))
            {
                position.x = (bounds.z) - screenSize.x / 2;
            }
            else if ((position.x - screenSize.x / 2) < (bounds.x))
            {
                position.x = bounds.x + screenSize.x / 2;
            }
            if ((position.y + screenSize.y / 2) > (bounds.w))
            {
                position.y = (bounds.w) - screenSize.y / 2;
            }
            else if ((position.y - screenSize.y / 2) < (bounds.y))
            {
                position.y = bounds.y + screenSize.y / 2;
            }
            camera.transform.position = position;
        }

        void Update()
        {
            Vector2 mousePosition = Input.mousePosition;
            Camera camera = SessionManager.Instance.mainCamera;
            float xAxisValue = Input.GetAxis("Horizontal");
            float yAxisValue = Input.GetAxis("Vertical");
            float scrollDelta= Input.GetAxis("Mouse ScrollWheel");
            if (scrollDelta < 0.0F && camera.orthographicSize < maxScroll)
            {
                camera.orthographicSize += 0.25F;
            }
            else if (scrollDelta > 0.0F && camera.orthographicSize > minScroll)
            {
                camera.orthographicSize--;
            }
            if (camera != null && (xAxisValue != 0 || yAxisValue != 0 || scrollDelta != 0))
            {
                UpdateCameraPosition(camera, xAxisValue, yAxisValue);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {

            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(SessionRenderer.Instance.selection != null)
                {
                    if (!SessionRenderer.Instance.selection.Deselect())
                    {
                        if(escMenu != null)
                        {
                            if (escMenu.gameObject.activeSelf)
                            {
                                escMenu.gameObject.SetActive(false);
                            }
                            else
                            {
                                escMenu.gameObject.SetActive(true);
                            }
                        }
                    }
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                buttonDown[0] = true;
                foreach(IMouseListener listener in listeners)
                {
                    if (listener != null && listener.MouseDown())
                    {
                        break;
                    }
                }
            }
            else if (buttonDown[0])
            {
                buttonDown[0] = false;
                foreach (IMouseListener listener in listeners)
                {
                    if (listener != null && listener.MouseUp())
                    {
                        break;
                    }
                }
            }
            else if(lastMouse != mousePosition)
            {
                foreach (IMouseListener listener in listeners)
                {
                    if (listener != null && listener.MouseMove(mousePosition, lastMouse))
                    {
                        break;
                    }
                }
            }
            lastMouse = mousePosition;
        }

    }
}
