using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    class Component
    {
        private EntityBase Entity;

        public Component(EntityBase entity)
        {
            Entity = entity;
            Entity.Components.Add(this);
        }
    }
}
