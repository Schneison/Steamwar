using System.Collections;
using UnityEngine;

namespace Steamwar
{
    public interface ISessionListener
    {
        void OnCreateSession(Session session);

        void OnLoadSession(Session session);

        void OnSaveSession(Session session);

        void OnFinishLoading();

    }
}