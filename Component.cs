using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    class Component
    {
        private EntityBase entity;

        public Component(EntityBase entity)
        {
            this.entity = entity;
            this.entity.components.Add(this);
        }
    }
}
