using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    [Serializable]
    public class BaseViewModel<TType>
    {
        public TType Id { get; set; }
    }
}
