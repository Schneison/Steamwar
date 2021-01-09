
using Steamwar.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Objects
{
    public static class ObjectExtensions
    {
        public delegate void ObjectAction(ObjectData data, ObjectType type, ObjectContainer building);

        public delegate bool ObjectPredicate(ObjectData data, ObjectType type, ObjectContainer building);

        public static void ActOnObject(this ObjectContainer source, ObjectKind kind, ObjectPredicate predicate, ObjectAction action)
        {
            if (!(source is ObjectContainer))
            {
                return;
            }
            ObjectData data = source.Data;
            if (data == null)
            {
                return;
            }
            ObjectType type = data.Type;
            if (type == null || kind != data.Kind || !predicate(data, type, source))
            {
                return;
            }
            action(data, type, source);
        }

        public static void ActOnObject(this ObjectContainer source, ObjectKind kind, ObjectAction action)
        {
            if (!(source is ObjectContainer))
            {
                return;
            }
            ObjectData data = source.Data;
            if (data == null)
            {
                return;
            }
            ObjectType type = data.Type;
            if (type == null || kind != data.Kind)
            {
                return;
            }
            action(data, type, source);
        }

        public static void ActOnBuilding(this ObjectContainer source, ObjectPredicate predicate, ObjectAction action)
        {
            source.ActOnObject(ObjectKind.BUILDING, predicate, action);
        }

        public static void ActOnBuilding(this ObjectContainer source, ObjectAction action)
        {
            source.ActOnObject(ObjectKind.BUILDING, action);
        }

        public static void ActOnUnit(this ObjectContainer source, ObjectPredicate predicate, ObjectAction action)
        {
            source.ActOnObject(ObjectKind.UNIT, predicate, action);
        }

        public static void ActOnUnit(this ObjectContainer source, ObjectAction action)
        {
            source.ActOnObject(ObjectKind.UNIT, action);
        }
    }

}
