using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTheSigils
{
    public class EmptySigil : AbilityBehaviour
    {
        public override Ability Ability
        {
            get
            {
                return new Ability();
            }
        }
    }
}
