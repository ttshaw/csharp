using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    class Component
    {
        private EntityBase MyEntity;

        public Component(EntityBase entity)
        {
            MyEntity = entity;
            MyEntity.Components.Add(this);
        }
    }
}
