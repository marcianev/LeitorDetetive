using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Messages
{
    public class AdicionarTurma : ValueChangedMessage<bool>
    {
        public AdicionarTurma() : base(true)
        {

        }
    }
}
