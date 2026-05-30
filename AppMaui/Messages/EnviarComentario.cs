using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Messages
{
    public class EnviarComentario : ValueChangedMessage<bool>
    {
        public EnviarComentario() : base(true)
        {
        }
    }
}
