using System;
using System.Collections.Generic;

namespace Thinktecture.MessageTransformer.Models
{
    public class Task
    {
        public string Message { get; set; }
        public List<object> Parameters { get; set; }


        public string Compose()
        {
            return string.Format(Message, Parameters.ToArray());
        }

        public string Reverse()
        {
            var input = Compose();
            var chars = input.ToCharArray();
            Array.Reverse(chars);
            return string.Concat(chars);
        }
    }
}