using System;
using System.Collections.Generic;

namespace Helpers
{
    [Serializable()]
    public class BaseEntity
    {

        public BaseEntity()
        {
            id = new object();
        }
        public BaseEntity(object id, string name)
        {
            this.id = id;
            this.name = name;
        }
        public BaseEntity(object id, string name, string Details)
        {
            this.id = id;
            this.name = name;
            this.Details = Details;
        }

        public BaseEntity(string name)
        {
            this.name = name;
        }

        private object id;
        public object Id
        {
            get { return id; }
            set
            {
                if (id == null)
                {
                    id = new object();
                }
                id = value;
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string details;
        public string Details
        {
            get { return details; }
            set { details = value; }
        }
    }

    public class IdName : BaseEntity
    {
        public IdName() { }
        public IdName(string name) : base(name) { }
        public IdName(object id, string name) : base(id, name) { }

    }
    public class IdNameCollection : List<IdName> { }
}
